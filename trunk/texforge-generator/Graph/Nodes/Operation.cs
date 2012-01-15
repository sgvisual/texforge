using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace texforge.Graph.Nodes
{
    class Operation : Node
    {
        protected OperationType operationType = new OperationType("OperationType", eOperationType.None, eOperationType.None);

        protected Operations.Operation operation;

        public Operation(string name, string id, Graph graph)
            : base(name, id, graph)
        {
            RegisterSocket(Socket.Type.Input, "In");
            RegisterSocket(Socket.Type.Output, "Out");

            AddSetting(operationType);
        }

        public override object Process()
        {
            switch ((eOperationType)Enum.Parse(typeof(eOperationType), operationType.Value, true))
            {
                case eOperationType.None:
                    return base.Process();

                case eOperationType.Blur:
                    Blur();
                    break;
            }

            if (operation == null)
                return null;

            Data.atom = operation.Execute();
            return Data.atom;

        }

        protected void Blur()
        {
            if (inputSockets[0].Connections.Count == 1)
            {
                operation = new Operations.Blur(inputSockets[0].Connections.First.Value.Data.atom);
            }
        }
    }
}
