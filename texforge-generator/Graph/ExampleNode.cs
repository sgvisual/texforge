using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace texforge.Graph
{
    public class ExampleNode : Node
    {
        public ExampleNode(string name, string id)
            : base(name, id)
        {
            RegisterSocket(Socket.Type.Input, "inTestA");
            RegisterSocket(Socket.Type.Input, "inTestB");

            RegisterSocket(Socket.Type.Output, "outA");
        }

        public override object Process()
        {
            Socket sockA = GetSocket("inTestA");
            Socket sockB = GetSocket("inTestB");

            Atom A = null;
            Atom B = null;

            if (sockA.connections.Count > 0)
                A = sockA.connections.First.Value.Data.atom;

            if (sockB.connections.Count > 0)
                B = sockB.connections.First.Value.Data.atom;

            Operations.Addition add = new Operations.Addition(A, B);
            Data.atom = add.Execute();

            return Data;
        }



    }
}
