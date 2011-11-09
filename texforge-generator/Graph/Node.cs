using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace texforge.Graph
{
    public class Node
    {
        public Node()
        {
        }

        protected NodeData nodeData;
        public NodeData Data
        {
            get { return nodeData; }
            set { nodeData = value; }
        }

        public void RegisterSocket(Socket.Type type, string name)
        {
            if ( type == Socket.Type.Input )
                inputSockets.Add(new Socket(name));
            else
                outputSockets.Add(new Socket(name));
        }

        protected List<Socket> inputSockets = new List<Socket>();
        protected List<Socket> outputSockets = new List<Socket>();
        public class Socket
        {
            public enum Type
            {
                Input,
                Output
            }
            public Socket(string name)
            {
                this.name = name;
            }

            public string name;
        }

    }
}
