using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Xml.Linq;

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
                    Node node = CreateNode(nodeName, nodeID);
                    node.Data.header.title = el2.Value;
                }
            }


            IEnumerable<XElement> det = from el in root.Descendants("Transitions") select el;
            foreach (XElement el in det)
            {
                IEnumerable<XElement> de2 = from el2 in el.Descendants("Item") select el2;
                foreach (XElement el2 in de2)
                {
                    XElement source = el2.Descendants("source").First();
                    XElement dest  = el2.Descendants("destination").First();

                    
                    //Console.WriteLine(source.Value);
                    //Console.WriteLine(dest.Value);
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
                    item.Add(name);
                    item.Add(id);                    
                    nodeElement.Add(item);
                }
                root.Add(nodeElement);


                XElement transitionElement = new XElement("Transitions");
                foreach (Transition t in transitions)
                {
                    XElement transition = new XElement("Item");
                    transition.Add(new XElement("source", t.from.name));
                    transition.Add(new XElement("destination", t.to.name));
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
