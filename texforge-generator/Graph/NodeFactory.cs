using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using texforge.Graph.Nodes;

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
            nodes["ImageNode"] = typeof(ImageNode);
            nodes["ColorOverlayNode"] = typeof(ColorOverlayNode);
        }

        public Node Create(string name, string id)
        {
            Node node = (Node)System.Activator.CreateInstance(nodes[name], new object[] { name, id });            
            return node;
        }

        protected Dictionary<string, Type> nodes = new Dictionary<string,Type>();
    }
}
