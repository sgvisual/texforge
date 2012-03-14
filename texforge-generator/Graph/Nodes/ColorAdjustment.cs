using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using texforge_definitions.Settings;

namespace texforge.Graph.Nodes
{
    //public enum eColorAdjustmentOperation
    //{
    //    Brightness,
    //    Contrast
    //}

    //class ColorAdjustmentOperation : Enumeration
    //{
    //    public ColorAdjustmentOperation(string name, eColorAdjustmentOperation value, eColorAdjustmentOperation defaultValue)
    //        : base(name, value.ToString(), defaultValue.ToString(), typeof(eColorAdjustmentOperation))
    //    {
    //    }
    //}

    //// TODO: make a Property Group class
    //public class PropertyGroup
    //{
    //}

    //public class BrightnessProperties : PropertyGroup
    //{
    //    public Float value = new Float("brightness", 1f, 1f, 0f, float.MaxValue);
    //}

    //public class ContrastProperties : PropertyGroup
    //{
    //    public Float value = new Float("contrast", 1f, 1f, 0f, float.MaxValue);
    //}





    public class ColorAdjustment : Node
    {
        //protected ColorAdjustmentOperation operation = new ColorAdjustmentOperation("Operation";
        
        protected Float brightness = new Float("brightness", 1f, 1f, 0f, float.MaxValue);
        protected Float contrast = new Float("contrast", 1f, 1f, 0f, float.MaxValue);

        public ColorAdjustment(string name, string id, Graph graph)
            : base(name, id, graph)
        {
            RegisterSocket(Socket.Type.Input, "In");
            RegisterSocket(Socket.Type.Output, "Out");

            brightness.OnChange += new EventHandler(brightness_OnChange);
            
            AddSetting(brightness);
            AddSetting(contrast);
        }

        void brightness_OnChange(object sender, EventArgs e)
        {
            
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

            ApplyBrightness(ref result);
            ApplyContrast(result);

            displayAtom = result;
            GetSocket("Out").atom = result;
            return result;
        }

        protected Atom ApplyBrightness(ref Atom atom)
        {
            byte[] bytesA = atom.ToBytes();

            for (int i = 0; i < bytesA.Length; ++i)
            {
                if ((i + 1) % 4 == 0) // don't affect alpha channel
                    continue;

                bytesA[i] = (byte)(Math.Min((bytesA[i] + (byte)(255f * brightness.Value)), (byte)255));
            }
            atom.Write(bytesA);
            return atom;
        }

        protected Atom ApplyContrast(Atom atom)
        {
            byte[] bytesA = atom.ToBytes();

            for (int i = 0; i < bytesA.Length; ++i)
            {
                if ((i + 1) % 4 == 0) // don't invert alpha channel
                    continue;

                byte half = (byte)(bytesA[i] * 0.5f);
                bytesA[i] = (byte)Math.Min((((bytesA[i] - half) * contrast.Value) + half), (byte)255);
            }
            atom.Write(bytesA);
            return atom;
        }
    }
}
