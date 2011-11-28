using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using texforge_definitions.Settings;
using texforge.Operations;

namespace texforge.Graph.Nodes
{
    public class ColorOverlayNode : Node
    {
        Color overlayColor = new Color("Color", new texforge_definitions.Types.Color(255,255,255,255), new texforge_definitions.Types.Color(255,255,255,255), new texforge_definitions.Types.Color(0,0,0,0), new texforge_definitions.Types.Color(255,255,255,255));

        public ColorOverlayNode(string name, string id)
            : base(name, id)
        {
            RegisterSocket(Socket.Type.Input, "In");
            RegisterSocket(Socket.Type.Output, "Out");

            AddSetting(overlayColor);
        }

        public override object Process()
        {
            if (inputSockets.Count == 0 || inputSockets[0].connections.Count == 0)
                return null;

            nodeData.atom = inputSockets[0].connections.First.Value.Data.atom;
            

            ColorBlend colorBlend = new ColorBlend(nodeData.atom, new texforge_definitions.Types.Color(255,0,0,255), ColorBlend.BlendType.Additive);
            nodeData.atom = colorBlend.Execute();


            return nodeData.atom;
        }

    }
}
