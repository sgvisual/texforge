using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using texforge_definitions.Settings;

namespace texforge.Graph.Nodes
{
    public class Generator : Node
    {
        protected GeneratorType generatorType = new GeneratorType("GeneratorType", eGeneratorType.None, eGeneratorType.None);

        protected texforge_definitions.Settings.Color startColor = new texforge_definitions.Settings.Color("Start Color", new texforge_definitions.Types.Color(System.Drawing.Color.Black), new texforge_definitions.Types.Color(System.Drawing.Color.Black), new texforge_definitions.Types.Color(System.Drawing.Color.Black), new texforge_definitions.Types.Color(System.Drawing.Color.White));
        protected texforge_definitions.Settings.Color endColor = new texforge_definitions.Settings.Color("End Color", new texforge_definitions.Types.Color(System.Drawing.Color.White), new texforge_definitions.Types.Color(System.Drawing.Color.Black), new texforge_definitions.Types.Color(System.Drawing.Color.Black), new texforge_definitions.Types.Color(System.Drawing.Color.White));

        public Generator(string name, string id, Graph graph)
            : base(name, id, graph)
        {
            RegisterSocket(Socket.Type.Output, "Out");
            //generator = new texforge.Generators.Heightmap(graph.Settings.size, graph.Settings.PixelFormat);
            //generator = new texforge.Generators.PerlinNoise(graph.Settings.size, graph.Settings.PixelFormat);

            AddSetting(generatorType);
            AddSetting(startColor);
            AddSetting(endColor);
        }

      

        public override object Process()
        {
            switch ((eGeneratorType)Enum.Parse(typeof(eGeneratorType), generatorType.Value, true))
            {
                case eGeneratorType.None:
                    return null;

                case eGeneratorType.Noise:
                    generator = new texforge.Generators.Heightmap(graph.Settings.size, graph.Settings.PixelFormat);
                break;

                case eGeneratorType.PerlinNoise:
                    generator = new texforge.Generators.PerlinNoise(graph.Settings.size, graph.Settings.PixelFormat);
                    ((texforge.Generators.PerlinNoise)generator).StartColor = startColor.Value.WindowsColor;
                    ((texforge.Generators.PerlinNoise)generator).EndColor = endColor.Value.WindowsColor;
                    break;
            }

            Data.atom = generator.Generate();
            return Data.atom;
        }

        protected texforge.Generators.Generator generator; 
    }
}
