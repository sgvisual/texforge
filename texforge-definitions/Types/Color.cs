using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace texforge_definitions.Types
{
    public class Color
    {
        public byte red;
        public byte green;
        public byte blue;
        public byte alpha;

        public Color(byte red, byte green, byte blue, byte alpha)
        {
            this.red = red;
            this.green = green;
            this.blue = blue;
            this.alpha = alpha;
        }

        public UInt32 ToUInt32()
        {
            return (UInt32)((alpha << 24) | ((red) << 16) | ((green << 8)) | ((blue)));
        }

    }
}
