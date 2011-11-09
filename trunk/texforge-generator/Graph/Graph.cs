using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace texforge.Graph
{
    public class Graph
    {
        public struct Transition
        {
            public Transition(Node from, Node to)
            {
                this.from = from;
                this.to = to;
            }
            public Node from;
            public Node to;
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

        public void ConnectNodes(Node a, Node b)
        {
            Transition t = new Transition(a, b);
            transitions.Add(t);            
        }

        public void DisconnectNodes(Node a, Node b)
        {
            Transition? remove = null;
            foreach (Transition t in transitions)
            {
                if (t.from == a && t.to == b )
                {
                    remove = t;
                    break;
                }
            }

            if (remove != null)
                transitions.Remove(remove.Value);
        }


    }
}
