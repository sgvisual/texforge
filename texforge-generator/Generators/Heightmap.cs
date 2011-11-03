using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;

namespace texforge.Generators
{
    public class Heightmap : Generator
    {
        public Heightmap(Size size, PixelFormat pixelFormat)
            : base(size, pixelFormat)
        {
        }
              

        public override Atom Generate()
        {
            Atom a = new Atom(m_size, m_pixelFormat);

            byte[] bytes = a.ToBytes();

            Random rand = new Random();            

            for (int i = 0; i < bytes.Length; i += 3)
            {
                //int color = (rand.Next(int.MaxValue));
                float n = Noise.SmoothNoise(i, i +1);
                int color = (int)(n * 5);

                byte r = (byte)(color >> 16);
                byte g = (byte)(color >> 8);
                byte b = (byte)(color);
               
                bytes[i + 0] = r;
                bytes[i + 1] = g;
                bytes[i + 2] = b;

            }

            a.Write(bytes);

            return a;
        }
    }
}
