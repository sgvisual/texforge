using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;

namespace texforge.Generators
{
    public abstract class Generator
    {
        public Generator(Size size, PixelFormat pixelFormat) 
        { 
            this.size = size;
            this.pixelFormat = pixelFormat;
        }
        public abstract Atom Generate();

        protected Size size;
        protected PixelFormat pixelFormat;
    }
}
