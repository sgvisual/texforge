using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace texforge.Operations
{
    public class Screen : Operation
    {
        Atom operandA;
        Atom operandB;

        public Screen(Atom a, Atom b)
        {
            operandA = a;
            operandB = b;
        }

        public override Atom Execute()
        {
            byte[] bytesA = operandA.ToBytes();
            byte[] bytesB = operandB.ToBytes();

            byte[] result = new byte[bytesA.Length];

            int bytes = bytesA.Length;
            Parallel.For(0, bytes, index =>
            {
                result[index] = (byte)(255 - (((255 - bytesA[index]) * (255 - bytesB[index]))/255));
            });

            return new Atom(result, operandA.Result.Size, operandA.Result.PixelFormat);
        } 
    }
}
