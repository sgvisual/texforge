using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using texforge.Base;
using System.Xml.Linq;
using texforge_definitions.Settings;

namespace texforge.Graph
{
    public class Node 
    {
        protected string name;
        protected UniqueName uniqueName;

        bool dirty = true;
        public bool Dirty
        {
            set 
            { 
                dirty = value;
                if (dirty)
                {
                    foreach (Socket socket in OutputSockets)
                    {
                        foreach (Node child in socket.Connections)
                        {
                            child.Dirty = true;
                        }
                    }
                }
            }
        }

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

        public void ProcessIfDirty()
        {
            if (!dirty)
                return;
            // Process parents first
            foreach (Socket socket in inputSockets)
            {
                foreach (Node parent in socket.Connections)
                {
                    parent.ProcessIfDirty();
                }
            }
            Data.atom = null;
            Process();
            dirty = false;
        }

        public virtual void Save(XElement element)
        {
            foreach (SettingBase setting in settings)
            {
                setting.Save(element);
            }
        }

        public virtual void Load(XElement element)
        {
            foreach (SettingBase setting in settings)
            {
                setting.Load(element);
            }
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
            dirty = true;
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

            public void AddConnection(Node node)
            {
                if (connections.Find(node) == null )
                    connections.AddLast(node);
            }

            public void RemoveConnection(Node node)
            {
                connections.Remove(node);
            }

            public string name;
            public readonly Node owner;
            protected LinkedList<Node> connections = new LinkedList<Node>();
            public LinkedList<Node> Connections
            {
                get { return connections; }
            }
        }

        protected LinkedList<SettingBase> settings = new LinkedList<SettingBase>();
        public LinkedList<SettingBase> Settings
        {
            get { return settings; }
        }

        protected void AddSetting(SettingBase setting)
        {
            settings.AddLast(setting);
        }


    }
}
