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
            
            Node a = graph.CreateNode("ImageNode", "");//graph.CreateNode();
            NodeData aData = new NodeData();
            aData.header.title = "A";
            a.Data = aData;
            a.Data.header.point = new Point(-175, -60);
            Uri uri = new Uri(Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), @"..\..\..\data\tests\toplayer.jpg"));
            ((ImageNode)a).LoadImage(Path.GetFullPath(uri.AbsolutePath));

            Node b = graph.CreateNode("ColorOverlayNode", "");
            NodeData bData = new NodeData();
            bData.header.title = "B";
            b.Data = bData;
            b.Data.header.point = new Point(-45, 25);
            //((ColorOverlayNode)b).Color = new texforge_definitions.Settings.Color("Color", new texforge_definitions.Types.Color()

            Node c = graph.CreateNode("ExampleNode", "");
            NodeData cData = new NodeData();
            cData.header.title = "C";
            c.Data = cData;
            c.Data.header.point = new Point(100, -100);

            graph.ConnectNodes(a.GetSocket("Out"), b.GetSocket("In"));
            graph.ConnectNodes(b.GetSocket("Out"), c.GetSocket("inTestB"));
            graph.ConnectNodes(a.GetSocket("Out"), c.GetSocket("inTestA"));
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
