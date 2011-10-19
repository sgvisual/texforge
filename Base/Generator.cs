using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;

namespace texforge_generator.Base
{
    public class Generator
    {

        protected Bitmap bitmap = null;
        protected Image image = null;
        public Image ResultImage
        {
            get { return image; }
        }

        public void Generate(texforge_definitions.settings settings)
        {
            bitmap = new Bitmap(settings.width, settings.height);

            BitmapData data = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            
            IntPtr raw = data.Scan0;

            int bytes = Math.Abs(data.Stride) * bitmap.Height;
            byte[] rgb = new byte[bytes];

            System.Runtime.InteropServices.Marshal.Copy(raw, rgb, 0, bytes);

            // Set every third value to 255. A 24bpp bitmap will look red.  
            for (int counter = 2; counter < rgb.Length; counter += 3)
                rgb[counter] = 255;

            // Copy the RGB values back to the bitmap
            System.Runtime.InteropServices.Marshal.Copy(rgb, 0, raw, bytes);

            bitmap.UnlockBits(data);
        }
    }
}
