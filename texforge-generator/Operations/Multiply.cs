using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using texforge_definitions;

namespace texforge.Operations
{
    public class Multiply : Operation
    {

        protected Atom operandA;
        protected Atom operandB;

        protected FloatSetting m_scalar = new FloatSetting("Coefficient", 1f, 1f, 0f, 1f);


        public Multiply(Atom a, Atom b)
        {
            operandA = a;
            operandB = b;
        }

        protected Atom MultiplyColor(ref Atom A, ref Atom B)
        {
            if (A.IsColor && B.IsColor )
            {
                byte a = (byte)((A.AtomColor.A * B.AtomColor.A) / 255);
                byte r = (byte)((A.AtomColor.R * B.AtomColor.R) / 255);
                byte g = (byte)((A.AtomColor.G * B.AtomColor.G) / 255);
                byte b = (byte)((A.AtomColor.B * B.AtomColor.B) / 255);
                return new Atom(System.Drawing.Color.FromArgb(a,r,g,b));
            }

            Atom color = A.IsColor ? A : B;
            Atom bitmap = A.IsBitmap ? A : B;

            byte[] source = bitmap.ToBytes();
            byte[] result = new byte[source.Length];

            int bytes = source.Length;
            for (int i = 0; i < bytes - 4; i += 4)
            {
                result[i + 0] = (byte)((source[i + 0] * color.AtomColor.B) / 255);
                result[i + 1] = (byte)((source[i + 1] * color.AtomColor.G) / 255);
                result[i + 2] = (byte)((source[i + 2] * color.AtomColor.R) / 255);
                result[i + 3] = (byte)((source[i + 3] * color.AtomColor.A) / 255);
            }

            return new Atom(result, bitmap.Result.Size, bitmap.Result.PixelFormat);
        }

        public Atom MultiplyBitmaps(ref Atom A, ref Atom B)
        {
            byte[] bytesA = A.ToBytes();
            byte[] bytesB = B.ToBytes();

            byte[] result = new byte[bytesA.Length];

            int bytes = bytesA.Length;
            Parallel.For(0, bytes, index =>
            {
                byte r = (byte)(((bytesA[index] * bytesB[index]) / 255f) * m_scalar.Value);
                result[index] = (byte)(Math.Min(r, 255f));
            });

            return new Atom(result, operandA.Result.Size, operandA.Result.PixelFormat);

        }

        public override Atom Execute()
        {
            if (operandA.IsBitmap && operandB.IsBitmap)
                return MultiplyBitmaps(ref operandA, ref operandB);

            return MultiplyColor(ref operandA, ref operandB);
        }

    }
}
