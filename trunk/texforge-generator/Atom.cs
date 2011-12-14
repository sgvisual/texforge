using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.Threading.Tasks;

namespace texforge
{
    public class Atom
    {
        protected Size size;
        protected Bitmap bitmap;
        protected BitmapData bitmapData = null;
        protected Color color;
        
        public Color AtomColor
        {
            get { return color; }
            set { color = value; Clear(color); }
        }

        public Atom(System.Drawing.Color color, Size size, PixelFormat pixelFormat)
        {
            this.size = size;
            this.color = color;

            bitmap = new Bitmap(size.Width, size.Height, pixelFormat);

            Clear(color);

        }

        public Atom(Size size, PixelFormat pixelFormat, Color[][] colors)
        {
            bitmap = new Bitmap(size.Width, size.Height, pixelFormat);

            // TODO: optimize
            for (int i = 0; i < size.Width; i++)
            {
                for (int j = 0; j < size.Height; j++)
                {
                    bitmap.SetPixel(i, j, colors[i][j]);
                }
            }

        }

        public Atom(Image image, Size size)
        {
            bitmap = new Bitmap(image, size);
            size = bitmap.Size;
        }

        public Atom(byte[] bytes, Size size, PixelFormat pixelFormat)
        {
            bitmap = new Bitmap(size.Width, size.Height, pixelFormat);

            Write(bytes);

        }

        public Atom(Size size, PixelFormat pixelFormat)
        {
            this.size = size;
            bitmap = new Bitmap(size.Width, size.Height, pixelFormat);            
        }

        public void Clear(Color color)
        {
            Lock();

            int bytes = Math.Abs(bitmapData.Stride) * bitmap.Height;
            byte[] rgb = new byte[bytes];

            IntPtr ptr = bitmapData.Scan0;

            //Parallel.For(0, bytes, r => { if (r % 4 == 3) rgb[r] = color.A; if (r % 3 == 2) rgb[r] = color.R; if (r % 3 == 1) rgb[r] = color.G; if (r % 3 == 0) rgb[r] = color.B; });

            for (int i = 0; i < bytes - 4; i += 4)
            {
                rgb[i + 0] = color.B;
                rgb[i + 1] = color.G;
                rgb[i + 2] = color.R;
                rgb[i + 3] = color.A;
            }

            System.Runtime.InteropServices.Marshal.Copy(rgb, 0, ptr, bytes);

            Unlock();
        }

        public void FromFile(String filename)
        {
            Image i = Image.FromFile(filename);
            
            bitmap = new Bitmap(i, size);
            
        }

        public Bitmap Result
        {
            get { return bitmap; }
        }

        public void Write(byte[] raw)
        {
            Lock();

            int bytes = Math.Abs(bitmapData.Stride) * bitmap.Height;
            byte[] rgb = new byte[bytes];

            IntPtr ptr = bitmapData.Scan0;

            System.Runtime.InteropServices.Marshal.Copy(raw, 0, ptr, bytes);


            Unlock();
        }

        public byte[] ToBytes()
        {
            Lock();
            
            int numBytes = bitmapData.Stride * bitmap.Height;
            byte[] bytes = new byte[numBytes];
            IntPtr ptr = bitmapData.Scan0;
            System.Runtime.InteropServices.Marshal.Copy(ptr, bytes, 0, numBytes);

            Unlock();

            return bytes;
        }

        //static public Atom operator + (Atom lhs, Atom rhs)
        //{
        //    Lock();
        //    BitmapData rhsData = rhs.Lock();

        //    IntPtr thisPtr = bitmapData.Scan0;
        //    byte[] thisRGB = new byte[bitmapData.Stride * bitmapData.Height];


        //    IntPtr rhsPtr = rhsData.Scan0;
        //    byte[] rhsRGB = new byte[rhsData.Stride * rhsData.Height];

        //    System.Runtime.InteropServices.Marshal.Copy(thisPtr, thisRGB, 0, bitmapData.Stride * bitmapData.Height);
        //    System.Runtime.InteropServices.Marshal.Copy(rhsPtr, rhsRGB, 0, rhsData.Stride * rhsData.Height);

        //    for (int i = 0; i < bitmapData.Stride * bitmapData.Height; ++i)
        //    {
        //        thisRGB[i] += rhsRGB[i];

        //    }
        //    Unlock();
        //    rhs.Unlock();

        //    return this;
        //}

        public void DrawPixel(int x, int y, int color)
        {
            bitmap.SetPixel(x, y, Color.White);
        }

        protected BitmapData Lock()
        {
            if (bitmapData != null)
                throw new Exception("Cannot Lock an Atom that is already locked");

            bitmapData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadWrite, bitmap.PixelFormat);
            return bitmapData;
        }

        protected void Unlock()
        {
            if (bitmapData == null)
                throw new Exception("Unlock called on an Atom that was not locked.");

            bitmap.UnlockBits(bitmapData);
            bitmapData = null;
        }
    }
}
