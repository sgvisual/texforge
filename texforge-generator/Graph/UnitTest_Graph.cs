using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace texforge.Graph
{
    public class UnitTest_Graph
    {
        public Graph graph;

        public UnitTest_Graph()
        {
            graph = new Graph();

            Node a = graph.CreateNode("ExampleNode");//graph.CreateNode();
            NodeData aData = new NodeData();
            aData.header.title = "A";
            a.Data = aData;
            a.Data.header.point = new Point(5, -60);

            Node b = graph.CreateNode("ExampleNode");
            NodeData bData = new NodeData();
            bData.header.title = "B";
            b.Data = bData;
            b.Data.header.point = new Point(45, 0);

            Node c = graph.CreateNode("ExampleNode");
            NodeData cData = new NodeData();
            cData.header.title = "C";
            c.Data = cData;
            c.Data.header.point = new Point(25, 60);

            graph.ConnectNodes(a, b);
            graph.ConnectNodes(b, c);
            graph.ConnectNodes(a, c);
        }

        public void Run()
        {
        }
    }
}
