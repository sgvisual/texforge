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
            Atom atom = new Atom(size, pixelFormat);

            byte[] bytes = atom.ToBytes();

            Random rand = new Random();            

            for (int i = 0; i < bytes.Length; i += 4)
            {
                //int color = (rand.Next(int.MaxValue));
                float n = Noise.SmoothNoise(i, i +1);
                int color = (int)(n * 5);

                byte a = (byte)(color >> 24);
                byte r = (byte)(color >> 16);
                byte g = (byte)(color >> 8);
                byte b = (byte)(color);
               
                bytes[i + 0] = r;
                bytes[i + 1] = g;
                bytes[i + 2] = b;
                bytes[i + 3] = a;

            }

            atom.Write(bytes);

            return atom;
        }
    }
}
