using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using texforge_definitions.Settings;

namespace texforge.Graph.Nodes
{
    public class Image : Node
    {
        protected Filename filename = new Filename("Filename", string.Empty, string.Empty, string.Empty, string.Empty);

        public Image(string name, string id, Graph graph)
            : base(name, id, graph)
        {
            RegisterSocket(Socket.Type.Output, "Out");

            AddSetting(filename);
        }

        public void LoadImage(string filename)
        {
            this.filename.Value = filename;
            if (File.Exists(filename))
            {
                displayAtom = new Atom(System.Drawing.Image.FromFile(filename), graph.Settings.size);
                GetSocket("Out").atom = displayAtom;
            }
        }

        public override object Process()
        {
            LoadImage(filename.Value);
            
            return displayAtom;
        }

        public override void Load(System.Xml.Linq.XElement element)
        {
            base.Load(element);

            string filename = element.Descendants("Image").First().Value;
            this.filename.Value = filename;
            LoadImage(filename);
        }

        public override void Save(System.Xml.Linq.XElement element)
        {
            base.Save(element);

            element.Add(new System.Xml.Linq.XElement("Image", filename.Value));
        }
    }
}
