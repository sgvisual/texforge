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
            Node inA = GetSocket("inTestA").owner;
            Node inB = GetSocket("inTestB").owner;

            Atom A = inA.Data.atom;
            Atom B = inB.Data.atom;

            Operations.Addition add = new Operations.Addition(A, B);
            Data.atom = add.Execute();

            return Data;
        }



    }
}
