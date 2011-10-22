using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using texforge;
using texforge.Operations;

namespace texforge_generator.Base
{
    public class Generator
    {

        protected Bitmap bitmap = null;
        protected Image image = null;
        public Bitmap ResultBitmap
        {
            get { return atom.Result; }
        }

        Atom atom;

        protected Random random;

        public void Generate(texforge_definitions.settings settings)
        {
            random = new Random(); // TODO: seed from settings

            Atom a = new Atom(new Size(settings.width, settings.height), PixelFormat.Format24bppRgb);           
            a.Clear( Color.Red );

            Atom b = new Atom(new Size(settings.width, settings.height), PixelFormat.Format24bppRgb);
            b.Clear(Color.Yellow);

            Addition add = new Addition(a, b);
            atom = add.Execute();
           
        }
    }
}
