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

        public Node CreateNode(string name)
        {
            Node node = NodeFactory.Get().Create(name);
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
            XDocument document = XDocument.Load(filename);

            XElement root = document.Root;

            XNode nodes = root.Descendants("Nodes").First();
            //foreach (XElement nodeElement in nodes)
            {

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
                    nodeElement.Add(new XElement("Item", n.GetType().ToString()));
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
