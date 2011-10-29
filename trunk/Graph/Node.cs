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
    }
}
