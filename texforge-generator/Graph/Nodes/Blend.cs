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

        protected Atom BlendAtoms(Atom a, Atom b)
        {
            if (a == null)
                return b;
            if (b == null)
                return a;

            Operations.Operation operation = GetOperation(a, b);
            return operation.Execute();
        }

        protected Operations.Operation GetOperation(Atom a, Atom b)
        {
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

            return operation;
        }

        public override object Process()
        {
            Atom result = null;
            Socket s = inputSockets[0];
            if ( s.connection != null )
                result = s.connection.atom;
            
            
            for (int i = 1; i < inputs.Value; ++i )
            {
                if ( inputSockets[i].connection != null )
                    result = BlendAtoms(result, inputSockets[i].connection.atom);               
            }

            displayAtom = result;
            return result;
        }

    }
}
