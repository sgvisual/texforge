using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using texforge.Graph.Nodes;
using System.IO;
using System.Windows.Forms;

namespace texforge.Graph
{
    public class UnitTest_Graph
    {
        public Graph graph;

        public UnitTest_Graph()
        {
            graph = new Graph(new GraphSettings());
            
            Node a = graph.CreateNode("Image", "");
            NodeData aData = new NodeData();
            aData.header.title = "Image";
            a.Data = aData;
            a.Data.header.point = new Point(-175, -60);
            Uri uri = new Uri(Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), @"..\..\..\data\tests\toplayer.jpg"));
            ((Nodes.Image)a).LoadImage(Path.GetFullPath(uri.AbsolutePath));
            a.Process();

            Node b = graph.CreateNode("Color", "");
            NodeData bData = new NodeData();
            bData.header.title = "Blend";
            b.Data = bData;
            b.Data.header.point = new Point(-45, 25);
            b.Process();

            Node c = graph.CreateNode("Blend", "");
            NodeData cData = new NodeData();
            cData.header.title = "Blend2";
            c.Data = cData;
            c.Data.header.point = new Point(100, -100);

            graph.ConnectNodes(a.GetSocket("Out"), c.GetSocket("A"));
            graph.ConnectNodes(b.GetSocket("Out"), c.GetSocket("B"));
            //graph.ConnectNodes(a.GetSocket("Out"), c.GetSocket("In"));
            //graph.ConnectNodes(b, c);
            //graph.ConnectNodes(a, c);
            
            
            //graph.Save(@"c:\projects\texforge\test.xml");
            //graph.Load(@"c:\projects\texforge\test.xml");
        }

        public void Run()
        {
        }
    }
}
