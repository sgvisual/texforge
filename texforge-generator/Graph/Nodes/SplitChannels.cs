using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace texforge.Graph.Nodes
{
    class SplitChannels : Node
    {
        public SplitChannels(string name, string id, Graph graph)
            : base(name, id, graph)
        {
            RegisterSocket(Socket.Type.Input, "In");
            
            RegisterSocket(Socket.Type.Output, "Red");
            RegisterSocket(Socket.Type.Output, "Green");
            RegisterSocket(Socket.Type.Output, "Blue");
            RegisterSocket(Socket.Type.Output, "Alpha");        
        }

        public override object Process()
        {
            if (inputSockets[0].connection == null)
                return null;

            Atom inAtom = inputSockets[0].connection.atom;
            if (inAtom == null)
                return null;

            OutputSockets[0].atom = GetChannel(inAtom, eChannel.Red );
            OutputSockets[1].atom = GetChannel(inAtom, eChannel.Green );
            OutputSockets[2].atom = GetChannel(inAtom, eChannel.Blue );
            OutputSockets[3].atom = GetChannel(inAtom, eChannel.Alpha );

            displayAtom = inAtom;
            return inAtom;
        }

        protected enum eChannel { Red, Green, Blue, Alpha };
        protected Atom GetChannel(Atom atom, eChannel channel)
        {
            Atom outAtom = new Atom(atom.Result.Size, atom.Result.PixelFormat);

            byte[] bytesA = atom.ToBytes();
            
            for (int i = 0; i < bytesA.Length; i += 4)
            { 
                bytesA[i]     = (channel == eChannel.Blue) ? bytesA[i] : (byte)0;
                bytesA[i+1] = (channel == eChannel.Green) ? bytesA[i+1] : (byte)0;
                bytesA[i+2] = (channel == eChannel.Red) ? bytesA[i+2] : (byte)0;
                bytesA[i+3] = (channel == eChannel.Alpha) ? bytesA[i+3] : (byte)0xff;
            }

            outAtom.Write(bytesA);

            return outAtom;

        }

    }
}
