using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace texforge.Graph
{
    public class NodeData
    {
        public class Header
        {
            public string title;
            public string description;
            public string help;
            public Point point;
        }

        public Header header = new Header();
    }
}
