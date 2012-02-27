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

        protected string originalName;

        public Operation(string name, string id, Graph graph)
            : base(name, id, graph)
        {
            RegisterSocket(Socket.Type.Input, "In");
            RegisterSocket(Socket.Type.Output, "Out");

            AddSetting(operationType);

            operationType.OnChange += new EventHandler(operationType_OnChange);

        }

        void operationType_OnChange(object sender, EventArgs e)
        {
            eOperationType t = (eOperationType)Enum.Parse(typeof(eOperationType), operationType.Value, true);

            if (originalName == null)
                originalName = Name;

            if (t != eOperationType.None)
                Name = operationType.Value;
            else
                Name = originalName;
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

                case eOperationType.Invert:
                    Invert();
                    break;
            }

            if (operation == null)
                return null;

            Atom atom = operation.Execute();
            GetSocket("Out").atom = atom;
            displayAtom = atom;
            return atom;

        }

        protected void Invert()
        {
            if (inputSockets[0].connection != null)
            {
                operation = new Operations.Invert(inputSockets[0].connection.atom);
            }
        }

        protected void Blur()
        {
            if (inputSockets[0].connection != null)
            {
                operation = new Operations.Blur(inputSockets[0].connection.atom);
            }
        }
    }
}
