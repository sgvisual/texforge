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
            m_size = size;
            m_pixelFormat = pixelFormat;
        }
        public abstract Atom Generate();

        protected Size m_size;
        protected PixelFormat m_pixelFormat;
    }
}
