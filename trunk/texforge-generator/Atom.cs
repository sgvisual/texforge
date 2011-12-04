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
        
        public enum AtomType 
        {
            Color,
            Bitmap
        }

        protected AtomType atomType = AtomType.Bitmap;
        public bool IsColor
        {
            get { return atomType == AtomType.Color; }
        }

        public bool IsBitmap
        {
            get { return atomType == AtomType.Bitmap; }
        }        

        public Color AtomColor
        {
            get { return color; }
            set { color = value; }
        }

        public Atom(System.Drawing.Color color)
        {
            atomType = AtomType.Color;
            
            this.color = color;
        }

        public Atom(Image image)
        {
            bitmap = new Bitmap(image);
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

            Parallel.For(0, bytes, r => { if (r % 3 == 2) rgb[r] = color.R; if (r % 3 == 1) rgb[r] = color.G; if (r % 3 == 0) rgb[r] = color.B; });

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
