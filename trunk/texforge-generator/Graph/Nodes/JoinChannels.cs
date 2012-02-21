using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace texforge.Graph.Nodes
{
    class JoinChannels : Node
    {
        public JoinChannels(string name, string id, Graph graph)
            : base(name, id, graph)
        {
            RegisterSocket(Socket.Type.Output, "Out");
            
            RegisterSocket(Socket.Type.Input, "Red");
            RegisterSocket(Socket.Type.Input, "Green");
            RegisterSocket(Socket.Type.Input, "Blue");
            RegisterSocket(Socket.Type.Input, "Alpha");        
        }

        public override object Process()
        {
            
            Atom result = null;
            Operations.Addition add;
            Atom previousAtom = (InputSockets[0].connection != null) ? InputSockets[0].connection.atom : null;
            for (int i = 1; i < 4; ++i )
            {
                Atom currentAtom = (InputSockets[i].connection != null) ? InputSockets[i].connection.atom : null;
                if (previousAtom != null && currentAtom != null)
                {
                    add = new Operations.Addition(previousAtom, currentAtom);
                    result = add.Execute();
                    previousAtom = result;
                }

                if (previousAtom == null)
                    previousAtom = currentAtom;
            }

            if (result == null && previousAtom != null)
                result = previousAtom;

            displayAtom = result;
            return result;

        }
    
    }
}
