using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using texforge_definitions.Settings;

namespace texforge.Operations
{
    public class ColorBlend : Operation
    {
        public enum BlendType
        {
            Additive,
            Subtractive,
            Multiply,
            Divide,
            Screen
        }

        protected Atom operandA;
        protected texforge_definitions.Types.Color blendColor;
        protected eBlendMode blendMode;

        public ColorBlend(Atom a, texforge_definitions.Types.Color color, eBlendMode blendMode)
        {
            operandA = a;
            this.blendColor = color;
            this.blendMode = blendMode;
        }

        public override Atom Execute()
        {
            if (operandA == null)
                return null;

            byte[] bytes = operandA.ToBytes();


            for (int i = 0; i < bytes.Length - 4; i += 4)
            {
                // BGRA
                bytes[i + 0] = (byte)Math.Min((bytes[i + 0] + blendColor.blue), 255);
                bytes[i + 1] = (byte)Math.Min((bytes[i + 1] + blendColor.green), 255);
                bytes[i + 2] = (byte)Math.Min((bytes[i + 2] + blendColor.red), 255);
                bytes[i + 3] = (byte)Math.Min((bytes[i + 3] + blendColor.alpha), 255);
            }

            //Parallel.For(0, bytes.Length-4, i => 
            //{
            //    if ( i % 4 == 3 ) bytes[i + 3] = (byte)Math.Min((bytes[i + 3] + blendColor.blue), 255);
            //    if ( i % 4 == 2 ) bytes[i + 2] = (byte)Math.Min((bytes[i + 2] + blendColor.green), 255);
            //    if ( i % 4 == 1 ) bytes[i + 1] = (byte)Math.Min((bytes[i + 1] + blendColor.red), 255);
            //    if ( i % 4 == 0 ) bytes[i + 0] = (byte)Math.Min((bytes[i + 0] + blendColor.alpha), 255);
                
            //});

            return new Atom(bytes, operandA.Result.Size, operandA.Result.PixelFormat);
        }

    }
}
