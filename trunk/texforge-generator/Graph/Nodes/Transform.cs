using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using texforge_definitions.Settings;
using System.Drawing;
using System.Drawing.Imaging;

namespace texforge.Graph.Nodes
{
    public enum eFlip
    {
        None,
        Flip_X,
        Flip_Y,
        Flip_XY
    }

    public enum eRotate
    {
        None,
        Rotate_90,
        Rotate_180,
        Rotate_270
    }

    public class Flip : Enumeration
    {
        public Flip(string name, eFlip value, eFlip defaultValue)
            : base(name, value.ToString(), defaultValue.ToString(), typeof(eFlip))
        {
        }
    }

    public class Rotate : Enumeration
    {
        public Rotate(string name, eRotate value, eRotate defaultValue)
            : base(name, value.ToString(), defaultValue.ToString(), typeof(eRotate))
        {
        }
    }

    class Transform : Node
    {
        Flip flip = new Flip("Flip", eFlip.None, eFlip.None);
        Rotate rotate = new Rotate("Rotate", eRotate.None, eRotate.None);
        Float offsetX = new Float("Offset X", 0f, 0f, -float.MaxValue, float.MaxValue);
        Float offsetY = new Float("Offset Y", 0f, 0f, -float.MaxValue, float.MaxValue);

        public Transform(string name, string id, Graph graph)
            : base(name, id, graph)
        {
            RegisterSocket(Socket.Type.Input, "In");
            RegisterSocket(Socket.Type.Output, "Out");

            AddSetting(flip);
            AddSetting(rotate);
            AddSetting(offsetX);
            AddSetting(offsetY);
        }

        #region RotateFlipType

        System.Drawing.RotateFlipType GetRotateFlipType(eFlip flip, eRotate rotate)
        {
            switch (flip)
            {
                case eFlip.None:
                    return GetRotateTypeNoFlip(rotate);
                case eFlip.Flip_X:
                    return GetRotateTypeFlipX(rotate);
                case eFlip.Flip_Y:
                    return GetRotateTypeFlipY(rotate);
                case eFlip.Flip_XY:
                    return GetRotateTypeFlipXY(rotate);
            }

            return System.Drawing.RotateFlipType.RotateNoneFlipNone;
        }

        System.Drawing.RotateFlipType GetRotateTypeFlipXY(eRotate rotate)
        {
            switch (rotate)
            {
                case eRotate.Rotate_90:
                    return System.Drawing.RotateFlipType.Rotate90FlipXY;
                case eRotate.Rotate_180:
                    return System.Drawing.RotateFlipType.Rotate180FlipXY;
                case eRotate.Rotate_270:
                    return System.Drawing.RotateFlipType.Rotate270FlipXY;

                default:
                case eRotate.None:
                    return System.Drawing.RotateFlipType.RotateNoneFlipXY;
            }
        }

        System.Drawing.RotateFlipType GetRotateTypeFlipY(eRotate rotate)
        {
            switch (rotate)
            {
                case eRotate.Rotate_90:
                    return System.Drawing.RotateFlipType.Rotate90FlipY;
                case eRotate.Rotate_180:
                    return System.Drawing.RotateFlipType.Rotate180FlipY;
                case eRotate.Rotate_270:
                    return System.Drawing.RotateFlipType.Rotate270FlipY;

                default:
                case eRotate.None:
                    return System.Drawing.RotateFlipType.RotateNoneFlipY;
            }
        }

        System.Drawing.RotateFlipType GetRotateTypeFlipX(eRotate rotate)
        {
            switch (rotate)
            {
                case eRotate.Rotate_90:
                    return System.Drawing.RotateFlipType.Rotate90FlipX;
                case eRotate.Rotate_180:
                    return System.Drawing.RotateFlipType.Rotate180FlipX;
                case eRotate.Rotate_270:
                    return System.Drawing.RotateFlipType.Rotate270FlipX;

                default:
                case eRotate.None:
                    return System.Drawing.RotateFlipType.RotateNoneFlipX;
            }
        }

        System.Drawing.RotateFlipType GetRotateTypeNoFlip(eRotate rotate)
        {
            switch (rotate)
            {
                case eRotate.Rotate_90:
                    return System.Drawing.RotateFlipType.Rotate90FlipNone;
                case eRotate.Rotate_180:
                    return System.Drawing.RotateFlipType.Rotate180FlipNone;
                case eRotate.Rotate_270:
                    return System.Drawing.RotateFlipType.Rotate270FlipNone;

                default:
                case eRotate.None:
                    return System.Drawing.RotateFlipType.RotateNoneFlipNone;
            }
        }

#endregion

        public override object Process()
        {
            Atom inAtom = GetSocket("In").ConnectedAtom;
            if (inAtom == null)
            {
                displayAtom = null;
                GetSocket("Out").atom = null;
                return null;
            }

            System.Drawing.RotateFlipType rotateFlipType = GetRotateFlipType((eFlip)Enum.Parse(typeof(eFlip), flip.Value, true), (eRotate)Enum.Parse(typeof(eRotate), rotate.Value, true));

            Atom result = new Atom(inAtom.Result, inAtom.Result.Size);

            ApplyOffset(ref result, offsetX.Value, offsetY.Value);

            result.Result.RotateFlip(rotateFlipType);
            displayAtom = result;
            GetSocket("Out").atom = result;
            
            return base.Process();
        }

        protected void ApplyOffset(ref Atom atom, float offsetX, float offsetY)
        {
            Graphics g = Graphics.FromImage(atom.Result);

            TextureBrush textureBrush = new TextureBrush(atom.Result, System.Drawing.Drawing2D.WrapMode.Tile);
            textureBrush.TranslateTransform(offsetX * atom.Result.Width, offsetY * atom.Result.Height);
          
            g.Clear(System.Drawing.Color.Transparent);
            g.FillRectangle(textureBrush, new Rectangle(0,0,atom.Result.Size.Width, atom.Result.Size.Height));

            g.Flush();
            g.Dispose();

        }
    }
}
