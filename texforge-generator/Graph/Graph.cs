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
   // [Serializable()]
    public class Graph //: ISerializable
    {
        public struct Transition
        {
            public Transition(Node.Socket from, Node.Socket to)
            {
                this.from = from;
                this.to = to;
            }
            public Node.Socket from;
            public Node.Socket to;
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

        public Node CreateNode(string name, string id)
        {
            Node node = NodeFactory.Get().Create(name, id);
            nodes.Add(node);
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
            Transition t = new Transition(a, b);
            transitions.Add(t);            
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
                transitions.Remove(remove.Value);
        }

        public Graph()
        { }

        public Graph(SerializationInfo info, StreamingContext context)
        {
            //int numNodes = (int)info.GetValue("NumNodes", typeof(int));
            //nodes = new List<Node>();
            //for (int i = 0; i < numNodes; ++i)
            //{
            //    info.GetValue
            //}
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            //info.A
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
              //      node.Data.header.title = el2.Value;
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
                    XElement name = new XElement("Name", n.Name);
                    XElement id  = new XElement("ID", n.ID);
                    XElement position = new XElement("Position", n.Data.header.point);

                    item.Add(name);
                    item.Add(id);
                    item.Add(position);
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
    }
}
