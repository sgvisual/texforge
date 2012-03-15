using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Xml.Linq;
using System.Drawing;
using System.ComponentModel;

namespace texforge.Graph
{
    public class Graph 
    {
        public class SharedThreadProperties
        {
            public SharedThreadProperties()
            {
                rendering = false;
                preventRendering = false;
                invalidate = false;
            }
            public bool rendering;
            public bool preventRendering;
            public bool invalidate;
        }

        public struct Transition
        {
            public Transition(Node.Socket from, Node.Socket to)
            {                
                this.from = from;
                this.to = to;

                this.from.AddConnection(to.owner);
                this.to.AddConnection(from.owner);
                
                to.owner.Dirty = true;
            }
            public Node.Socket from;
            public Node.Socket to;
        }

        Node proceduralFinal = null;
        public Node ProceduralFinal
        {
            get { return proceduralFinal; }
        }
        List<Node> finalOutPut = new List<Node>();
        public List<Node> FinalOutput
        {
            get { return finalOutPut; }
        }

        List<Node> nodes = new List<Node>();
        List<Transition> transitions = new List<Transition>();

        public List<Node> Nodes
        {
            get { return nodes; }
        }

        public List<Transition> Transitions
        {
            get { return transitions; }
        }

        bool dirty = true;
        public bool Dirty
        {
            get { return dirty; }
        }

        public Node CreateNode(string name, string id)
        {
            Node node = NodeFactory.Get().Create(name, id, this);
            nodes.Add(node);
            dirty = true;
            return node;
        }

        public Node GetNodeFromID(string id)
        {
            foreach (Node n in nodes)
            {
                if (n.ID.CompareTo(id) == 0)
                    return n;
            }

            return null;
        }

        public void ConnectNodes(Node.Socket a, Node.Socket b)
        {
            if (a == null || b == null)
                throw new Exception("Cannot connect nodes because a Socket was not found");

            a.connection = b;
            b.connection = a;

            Transition t = new Transition(a, b);
            transitions.Add(t);
            dirty = true;
        }

        protected void RemoveTransition(Transition remove)
        {
            remove.to.RemoveConnection(remove.from.owner);
            remove.from.RemoveConnection(remove.to.owner);
            remove.to.connection = null;
            remove.from.connection = null;
            transitions.Remove(remove);
            remove.to.owner.Dirty = true;
            dirty = true;
        }

        public void DisconnectAllFromNode(Node node)
        {
            Transition? remove = null;
            do
            {
                remove = null;
                foreach (Transition t in transitions)
                {
                    if (t.from.owner == node || t.to.owner == node)
                    {
                        remove = t;
                        break;
                    }
                }

                if (remove != null)
                {
                    RemoveTransition(remove.Value);
                }
            } while (remove != null);
        }

        public void RemoveNode(Node node)
        {
            DisconnectAllFromNode(node);
            nodes.Remove(node);
            finalOutPut.Remove(node);
            dirty = true;
        }

        public void DisconnectNodes(Node.Socket a, Node.Socket b)
        {
            Transition? remove = null;
            foreach (Transition t in transitions)
            {
                if (t.from == a && t.to == b)
                {
                    remove = t;
                    break;
                }
            }

            if (remove != null)
            {
                RemoveTransition(remove.Value);
            }
        }

        public Graph(GraphSettings settings)
        {
            this.settings = settings;
        }

        protected GraphSettings settings;
        public GraphSettings Settings
        {
            get { return settings; }
        }

        static T ParsePoint<T>(string str)
        {
            try
            {
                return (T)TypeDescriptor.GetConverter(typeof(T)).ConvertFromString(str.ToUpper().Replace(@"{X=", "").Replace(@"Y=", "").Replace(@"}", ""));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return default(T);
        }

        public void Load(string filename)
        {
            nodes.Clear();
            transitions.Clear();

            XDocument document = XDocument.Load(filename);

            XElement root = document.Root;

            XNode nodeList = root.Descendants("Nodes").First();

            IEnumerable<XElement> de = from el in root.Descendants("Nodes") select el;
            foreach (XElement el in de)
            {
                IEnumerable<XElement> de2 = from el2 in el.Descendants("Item") select el2;
                foreach (XElement el2 in de2)
                {
                    string nodeName = el2.Descendants("Name").First().Value;
                    string nodeID = el2.Descendants("ID").First().Value;
                    string point = el2.Descendants("Position").First().Value;
                    Point p  = ParsePoint<Point>(point);
                    
                    // TODO: some cleanup required
                    Node node = CreateNode(nodeName, nodeID);
                    node.Data = new NodeData();
                    node.Data.header = new NodeData.Header();
                    node.Data.header.point = p;

                    // TODO:
                    node.Load(el2.Descendants("Data").First());


                }
            }


            IEnumerable<XElement> det = from el in root.Descendants("Transitions") select el;
            foreach (XElement el in det)
            {
                IEnumerable<XElement> de2 = from el2 in el.Descendants("Item") select el2;
                foreach (XElement el2 in de2)
                {
                    string fromNodeID = el2.Descendants("FromNodeID").First().Value;
                    string fromSocketName = el2.Descendants("FromSocket").First().Value;
                    string toNodeID = el2.Descendants("ToNodeID").First().Value;
                    string toSocketName = el2.Descendants("ToSocket").First().Value;
                
                    Node A = GetNodeFromID(fromNodeID);
                    Node B = GetNodeFromID(toNodeID);

                    ConnectNodes(A.GetSocket(fromSocketName), B.GetSocket(toSocketName));
    
                }
            }

            // Need to load the nodes set as final

            dirty = true;
        }

        public void Save(string filename)
        {
            try
            {
                XDocument document = new XDocument();
                XElement root = new XElement("Graph");
                document.Add(root);
                XElement nodeElement = new XElement("Nodes");
                
                foreach (Node n in nodes)
                {
                    XElement item = new XElement("Item");
                    XElement name = new XElement("Name", n.TypeName);
                    XElement id  = new XElement("ID", n.ID);
                    XElement position = new XElement("Position", n.Data.header.point);

                    item.Add(name);
                    item.Add(id);
                    item.Add(position);

                    XElement data = new XElement("Data");
                    n.Save(data);
                    item.Add(data);

                    nodeElement.Add(item);

                }
                root.Add(nodeElement);


                XElement transitionElement = new XElement("Transitions");
                foreach (Transition t in transitions)
                {
                    XElement transition = new XElement("Item");
                    XElement fromNodeID = new XElement("FromNodeID", t.from.owner.ID);
                    XElement fromSocketName = new XElement("FromSocket", t.from.name);

                    transition.Add(fromNodeID);
                    transition.Add(fromSocketName);
                    
                    XElement toNodeID = new XElement("ToNodeID", t.to.owner.ID);
                    XElement toSocketName = new XElement("ToSocket", t.to.name);

                    transition.Add(toNodeID);
                    transition.Add(toSocketName);

                    transitionElement.Add(transition);

                }

                root.Add(transitionElement);

                document.Save(filename);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        protected int GetDepth(Node root, out Node deepest)
        {
            deepest = root;
            int depth = -1;
            foreach (Transition transition in Transitions)
            {
                if (transition.from.owner == root)
                {
                    Node current = null;
                    int currentDepth = GetDepth(transition.to.owner, out current);
                    if (currentDepth > depth)
                    {
                        depth = currentDepth;
                        deepest = current;
                    }
                }
            }
            return depth + 1;
        }

        public void Process(SharedThreadProperties sharedThreadProperties)
        {
            // If dirty, find the final node
            if (dirty)
            {
                dirty = false;
                if( FinalOutput.Count == 0 )
                {
                    proceduralFinal = null;
                    int currentDepth = -1;
                    foreach (Node node in Nodes)
                    {
                        if (node.InputSockets.Count == 0)
                        {
                            Node current;
                            int depth = GetDepth(node, out current);
                            if (depth > currentDepth)
                            {
                                currentDepth = depth;
                                proceduralFinal = current;
                            }
                        }
                    }
                }
                sharedThreadProperties.invalidate = true;
            }
            // Process all dirty nodes
            sharedThreadProperties.preventRendering = true;
            List<Node> existingNodes = new List<Node>();
            foreach (Node node in Nodes)
            {
                existingNodes.Add(node);
            }
            sharedThreadProperties.preventRendering = false;
            foreach (Node node in existingNodes)
            {
                while (sharedThreadProperties.rendering)
                    System.Threading.Thread.Sleep(1);
                if (node.ProcessIfDirty())
                    sharedThreadProperties.invalidate = true;
            }
        }

    }
}
