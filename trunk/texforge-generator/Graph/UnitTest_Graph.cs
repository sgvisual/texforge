using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using texforge.Graph.Nodes;

namespace texforge.Graph
{
    public class UnitTest_Graph
    {
        public Graph graph;

        public UnitTest_Graph()
        {
            graph = new Graph();
            
            Node a = graph.CreateNode("ImageNode", "");//graph.CreateNode();
            NodeData aData = new NodeData();
            aData.header.title = "A";
            a.Data = aData;
            a.Data.header.point = new Point(-175, -60);
            //((ImageNode)a).LoadImage(@"C:\Projects\texforge\data\tests\toplayer.jpg");

            Node b = graph.CreateNode("ExampleNode", "");
            NodeData bData = new NodeData();
            bData.header.title = "B";
            b.Data = bData;
            b.Data.header.point = new Point(-45, 25);

            Node c = graph.CreateNode("ExampleNode", "");
            NodeData cData = new NodeData();
            cData.header.title = "C";
            c.Data = cData;
            c.Data.header.point = new Point(100, -100);

            graph.ConnectNodes(a.GetSocket("Out"), b.GetSocket("inTestA"));
            graph.ConnectNodes(b.GetSocket("outA"), c.GetSocket("inTestB"));
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
