using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace texforge.Graph.Nodes
{
    public class Texture : Node
    {

        public Texture(string name, string id, Graph graph)
            : base(name, id, graph)
        {
            RegisterSocket(Socket.Type.Input, "In");
            RegisterSocket(Socket.Type.Output, "Out");

        }

        public override object Process()
        {
            Atom inAtom = GetSocket("In").ConnectedAtom;
            if (inAtom == null)
            {
                displayAtom = null;
                GetSocket("Out").atom = null;
                return null;
            }

            Atom result = new Atom(inAtom.Result, inAtom.Result.Size);

            Graphics g = Graphics.FromImage(result.Result);

            TextureBrush textureBrush = new TextureBrush(result.Result, System.Drawing.Drawing2D.WrapMode.Tile);
            //textureBrush.ScaleTransform(0.25f, 0.25f);

            // TODO: expose iterations, rotation, translation, scaling

            Random random = new Random();
            g.Clear(System.Drawing.Color.Transparent);
            for (int i = 0; i < 10; ++i)
            {
                //textureBrush.ScaleTransform((float)random.NextDouble(), (float)random.NextDouble());
                textureBrush.RotateTransform(360f * (float)random.NextDouble(), System.Drawing.Drawing2D.MatrixOrder.Prepend);
                int x = (int)((1f + (2f * random.NextDouble())) * result.Result.Size.Width);
                int y = (int)((1f + (2f * random.NextDouble())) * result.Result.Size.Height);

                textureBrush.TranslateTransform(x, y);


                g.FillRectangle(textureBrush, 0, 0, textureBrush.Image.Width, textureBrush.Image.Height);
                g.Flush();
            }

            g.Flush();
            g.Dispose();

            displayAtom = result;
            GetSocket("Out").atom = result;

            return base.Process();
        }
    }
}
