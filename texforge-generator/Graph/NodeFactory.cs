using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace texforge.Graph
{
    public class NodeFactory
    {

        public NodeFactory() { RegisterNodes(); }

        static NodeFactory _instance = new NodeFactory();

        public static NodeFactory Get()
        {
            return _instance;
        }

        public void RegisterNodes()
        {
            nodes["ExampleNode"] = typeof(ExampleNode);
        }

        public Node Create(string name)
        {
            return (Node)System.Activator.CreateInstance(nodes[name]);
        }

        protected Dictionary<string, Type> nodes = new Dictionary<string,Type>();
    }
}
