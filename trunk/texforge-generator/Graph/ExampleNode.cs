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



    }
}
