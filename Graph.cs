using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace texforge
{
	public class Graph
	{

        public Graph()
        {
        }

        public void Render(Graphics graphics, Rectangle clip)
        {
            graphics.FillRectangle(Brushes.CornflowerBlue, clip);
        }

	}
}
