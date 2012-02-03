using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing.Imaging;
using System.Drawing;
using texforge.Base;

namespace texforge.Graph
{
    public class GraphSettings
    {
        // TODO: replace by Settings to take advantage of UI generation
        // TODO: needs to be saved and loaded
        public int ImageWidth = 512;
        public int ImageHeight = 512;
        public Size size = new Size(512, 512);
        public PixelFormat PixelFormat = System.Drawing.Imaging.PixelFormat.Format32bppArgb;
        public UniqueName FinalOutputNode = new UniqueName("");
    }
}
