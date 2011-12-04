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
        texforge_definitions.Settings.Color overlayColor = new texforge_definitions.Settings.Color("Color", new texforge_definitions.Types.Color(255, 0, 0, 255), new texforge_definitions.Types.Color(255, 0, 0, 255), new texforge_definitions.Types.Color(0, 0, 0, 0), new texforge_definitions.Types.Color(255, 255, 255, 255));
        BlendMode blendMode = new BlendMode("BlendMode", eBlendMode.None, eBlendMode.None);

        public ColorOverlayNode(string name, string id)
            : base(name, id)
        {
            RegisterSocket(Socket.Type.Input, "In");
            RegisterSocket(Socket.Type.Output, "Out");

            AddSetting(overlayColor);
            AddSetting(blendMode);
        }

        public override object Process()
        {
            if (inputSockets.Count == 0 || inputSockets[0].Connections.Count == 0)
                return null;

            nodeData.atom = inputSockets[0].Connections.First.Value.Data.atom;
            

            ColorBlend colorBlend = new ColorBlend(nodeData.atom, overlayColor.Value, blendMode.Value);
            nodeData.atom = colorBlend.Execute();


            return nodeData.atom;
        }

    }
}
