﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using texforge_definitions.Settings;

namespace texforge.Graph.Nodes
{
    public class Blend : Node
    {

        protected BlendMode blendMode = new BlendMode("BlendMode", eBlendMode.None, eBlendMode.None);
        protected Float blendAmount = new Float("Amount", 1f, 1f, 0f, 1f);
        protected Int inputs = new Int("Inputs", 2, 2, 2, 8);

        public Blend(string name, string id, Graph graph)
            : base(name, id, graph)
        {
            for (int i = 0; i < inputs.Value; ++i )
                RegisterSocket(Socket.Type.Input, string.Format("{0}", i));

            //RegisterSocket(Socket.Type.Input, "A");
            //RegisterSocket(Socket.Type.Input, "B");
            RegisterSocket(Socket.Type.Output, "Result");

            AddSetting(blendMode);
            AddSetting(blendAmount);
            AddSetting(inputs);

            inputs.OnChange += new EventHandler(inputs_OnChange);
        }

        void inputs_OnChange(object sender, EventArgs e)
        {
            if (inputs.Value <= inputs.Min)
                return;

            if (inputs.Value >= inputs.Max)
                return;

            graph.DisconnectAllFromNode(this);

            inputSockets.Clear();

            for (int i = 0; i < inputs.Value; ++i)
                RegisterSocket(Socket.Type.Input, string.Format("{0}", i));
        }
        
        public override object Process()
        {
            Atom a = null;
            if ( inputSockets[0].connection != null )
                a = inputSockets[0].connection.atom;

            Atom b = null;
            if ( inputSockets[1].connection != null )
                b = inputSockets[1].connection.atom;

            if (a == null && b == null)
                return null;

            if (b == null)
            {
                displayAtom = a;
                return a;
            }

            if (a == null)
            {
                displayAtom = b;
                return b;
            }

            Operations.Operation operation = null;

            switch ((eBlendMode)Enum.Parse(typeof(eBlendMode), blendMode.Value, true))
            {
                case eBlendMode.Add:
                    operation = new Operations.Addition(a, b);
                    break;

                case eBlendMode.Subtract:
                    operation = new Operations.Subtraction(a, b);
                    break;

                case eBlendMode.Multiply:
                    operation = new Operations.Multiply(a, b, blendAmount.Value);
                    break;

                case eBlendMode.Screen:
                    operation = new Operations.Screen(a, b);
                    break;

                    // TESTING
                default:
                    operation = new Operations.Multiply(a, b, blendAmount.Value);
                    break;
            }

            if (operation != null)
            {
                Atom atom = operation.Execute();
                GetSocket("Result").atom = atom;
                displayAtom = atom;
                return atom;
            }

            // TODO: throw exception or invalidate node

            return null;
        }
    }
}