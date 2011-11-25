using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        protected BlendType blendType;

        public ColorBlend(Atom a, texforge_definitions.Types.Color color, BlendType blendType          )
        {
            operandA = a;
            this.blendColor = color;
            this.blendType = blendType;
        }

        public override Atom Execute()
        {
            if (operandA == null)
                return null;

            byte[] bytes = operandA.ToBytes();

            Random rnd = new Random();


            // TODO: fix for all colors
            int numBytes = bytes.Length;
            Parallel.For(0, numBytes, index =>
            {
                byte r = (byte)Math.Min(bytes[index] + (byte)rnd.Next(255), (byte)255);
                bytes[index] = r;
               // bytes[index] = (byte)r.Next(255); //(byte)(Math.Min(((bytes[index] + blendColor.red)), 255));

               // bytes[index] = (UInt32)(Math.Min(((bytes[index] + blendColor.ToUInt32())), UInt32.MaxValue));
            });

            return new Atom(bytes, operandA.Result.Size, operandA.Result.PixelFormat);
        }

    }
}
