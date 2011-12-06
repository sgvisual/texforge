using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using texforge_definitions.Settings;

namespace texforge.Graph.Nodes
{
    public class Blend : Node
    {


        protected BlendMode blendMode = new BlendMode("BlendMode", eBlendMode.None, eBlendMode.None);
        
        public Blend(string name, string id, Graph graph)
            : base(name, id, graph)
        {
            RegisterSocket(Socket.Type.Input, "A");
            RegisterSocket(Socket.Type.Input, "B");
            RegisterSocket(Socket.Type.Output, "Result");

            AddSetting(blendMode);
        }

        public override object Process()
        {
            if (inputSockets[0].Connections.Count == 0)
            {
                if (inputSockets[1].Connections.Count > 0)
                    return inputSockets[1].Connections.First.Value.Data.atom;

                return null;
            }
            else
            if (inputSockets[1].Connections.Count == 0)
            {
                return inputSockets[0].Connections.First.Value.Data.atom;
            }

                
            Atom a = inputSockets[0].Connections.First.Value.Data.atom;
            Atom b = inputSockets[1].Connections.First.Value.Data.atom;

            Operations.Operation operation = null;

            switch (blendMode.Value)
            {
                case eBlendMode.Add:
                    operation = new Operations.Addition(a, b);
                    break;

                case eBlendMode.Subtract:
                    operation = new Operations.Subtraction(a, b);
                    break;

                case eBlendMode.Multiply:
                    operation = new Operations.Multiply(a, b);
                    break;

                case eBlendMode.Screen:
                    operation = new Operations.Screen(a, b);
                    break;

                    // TESTING
                default:
                    operation = new Operations.Multiply(a, b);
                    break;
            }

            if (operation != null)
            {
                Data.atom = operation.Execute();
                return Data.atom;
            }

            // TODO: throw exception or invalidate node

            return null;
        }
    }
}
