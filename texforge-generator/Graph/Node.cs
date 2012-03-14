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

        protected UniqueName uniqueName;
        protected Atom displayAtom;
        public Atom DisplayAtom
        {
            get { return displayAtom; }
        }

        protected texforge_definitions.Settings.String name;

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
            get
            {
                return dirty;
            }
        }

        public string TypeName
        {
            get { return GetType().Name; }
        }

        public string Name
        {
            get { return name.Value; }
            set { name.Value = value; }
        }

        public string ID
        {
            get { return uniqueName.Value; }
        }

        public Node(string name, string id, Graph graph)
        {
            this.name = new texforge_definitions.Settings.String("Name", name, string.Empty, string.Empty, string.Empty);

            this.graph = graph;
            uniqueName = new UniqueName(id);

            AddSetting(this.name);
        }

        public virtual object Process()
        {
            return null;
        }

        public bool ProcessIfDirty()
        {
            if (!dirty)
                return false;
            // Process parents first
            foreach (Socket socket in inputSockets)
            {
                foreach (Node parent in socket.Connections)
                {
                    parent.ProcessIfDirty();
                }
            }
            
            displayAtom = null;

            Process();
            dirty = false;
            return true;
        }

        public virtual void Save(XElement element)
        {
            foreach (SettingBase setting in settings)
            {
                XElement settingElement = new XElement("Setting");
                setting.Save(settingElement);
                element.Add(settingElement);
            }
        }

        public virtual void Load(XElement element)
        {
            IEnumerable<XElement> baseElement = element.Descendants("Setting");
            if( baseElement.Count() > 0 )
            {
                XElement settingElement = baseElement.First();
                foreach (SettingBase setting in settings)
                {
                    setting.Load(ref settingElement);
                }
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
                inputSockets.Add(new Socket(this, name, null));
            else
                outputSockets.Add(new Socket(this, name, null));
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

            System.Diagnostics.Debug.Assert(false, string.Format("Socket {0} not found for node {1}", name, this.name));
            return null;
        }
        
        public class Socket
        {
            public enum Type
            {
                Input,
                Output
            }

            public Socket(Node owner, string name, Atom atom)
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

            public Atom ConnectedAtom
            {
                get { return (connection != null) ? connection.atom : null; }
            }

            public Atom atom;
            public string name;
            public readonly Node owner;
            public Socket connection;

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

        public void Remove()
        {
            graph.RemoveNode(this);
        }

        public void DisconnectAll()
        {
            graph.DisconnectAllFromNode(this);
        }

        public void SetAsFinalOutput()
        {
            graph.Final = this;
        }

        protected Graph graph;
    }
}
