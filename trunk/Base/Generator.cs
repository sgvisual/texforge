using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using texforge;
using texforge.Operations;
using texforge.Generators;

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

            Atom a = new Atom(new Size(settings.width, settings.height), PixelFormat.Format32bppArgb);           
            a.Clear( Color.Red );
            a.FromFile(@"C:\Projects\texforge\data\tests\toplayer.jpg");

            Atom b = new Atom(new Size(settings.width, settings.height), PixelFormat.Format32bppArgb);
            b.Clear(Color.Yellow);
            b.FromFile(@"C:\Projects\texforge\data\tests\bottomlayer.jpg");

            //Addition add = new Addition(a, b);
            //add.Mode = Addition.eMode.Add;
            //atom = add.Execute();

            //Multiply mul = new Multiply(a, b);
            //atom = mul.Execute();

            //Screen screen = new Screen(a, b);
            //atom = screen.Execute();

            //Subtraction subtraction = new Subtraction(a, b);
            //subtraction.Mode = Subtraction.eMode.Average;
            //atom = subtraction.Execute();

            //Blur blur = new Blur(a);
            //atom = blur.Execute();

            Heightmap h = new Heightmap(new Size(settings.width, settings.height), PixelFormat.Format32bppRgb);
            Atom t = h.Generate();

            Addition screen = new Addition(a, t);
            atom = screen.Execute();
            

        }
    }
}
