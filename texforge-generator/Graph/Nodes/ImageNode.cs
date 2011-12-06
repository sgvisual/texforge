using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace texforge.Graph.Nodes
{
    public class ImageNode : Node
    {
        protected string filename;

        public ImageNode(string name, string id, Graph graph)
            : base(name, id, graph)
        {
            RegisterSocket(Socket.Type.Output, "Out");
        }

        public void LoadImage(string filename)
        {
            this.filename = filename;
            if ( File.Exists(filename) )
                nodeData.atom = new Atom(System.Drawing.Image.FromFile(filename), graph.Settings.size);
        }

        public override object Process()
        {
            LoadImage(filename);
            return nodeData.atom;
        }

        public override void Load(System.Xml.Linq.XElement element)
        {
            base.Load(element);

            string filename = element.Descendants("Image").First().Value;
            LoadImage(filename);
        }

        public override void Save(System.Xml.Linq.XElement element)
        {
            base.Save(element);

            element.Add(new System.Xml.Linq.XElement("Image", filename));
        }
    }
}
