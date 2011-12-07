using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace texforge.Graph.Nodes
{
    public class Generator : Node
    {
        public Generator(string name, string id, Graph graph)
            : base(name, id, graph)
        {
            RegisterSocket(Socket.Type.Output, "Out");
            generator = new texforge.Generators.Heightmap(graph.Settings.size, graph.Settings.PixelFormat);
        }

        public override object Process()
        {
            Data.atom = generator.Generate();
            return Data.atom;
        }

        protected texforge.Generators.Generator generator; 
    }
}
