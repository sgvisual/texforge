using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using texforge.Base;
using System.Xml.Linq;

namespace texforge.Graph
{
    public class Node 
    {
        protected string name;
        protected UniqueName uniqueName;

        public string Name
        {
            get { return name; }
        }

        public string ID
        {
            get { return uniqueName.Value; }
        }

        public Node(string name, string id)
        {
            this.name = name;
            uniqueName = new UniqueName(id);
        }

        public virtual object Process()
        {
            return null;
        }

        public virtual void Save(XElement element)
        {
        }

        public virtual void Load(XElement element)
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
                inputSockets.Add(new Socket(this, name));
            else
                outputSockets.Add(new Socket(this, name));
        }

        protected List<Socket> inputSockets = new List<Socket>();
        public List<Socket> InputSockets
        {
            get { return inputSockets; }
        }
        protected List<Socket> outputSockets = new List<Socket>();
        public List<Socket> OutputSockets
        {
            get { return outputSockets; }
        }

        public Socket GetSocket(string name)
        {
            foreach (Socket s in inputSockets)
            {
                if (s.name.CompareTo(name) == 0)
                    return s;
            }

            foreach (Socket s in outputSockets)
            {
                if (s.name.CompareTo(name) == 0)
                    return s;
            }

            return null;
        }
        
        public class Socket
        {
            public enum Type
            {
                Input,
                Output
            }
            public Socket(Node owner, string name)
            {
                this.name = name;
                this.owner = owner;
            }

            public string name;
            public readonly Node owner;
        }

    }
}
