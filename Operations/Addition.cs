using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Threading.Tasks;

namespace texforge.Operations
{
    public class Addition : Operation
    {
        protected Atom operandA;
        protected Atom operandB;

        public Addition(Atom a, Atom b)
        {
            operandA = a;
            operandB = b;
        }

        public override Atom Execute()
        {
     
            byte[] bytesA = operandA.ToBytes();
            byte[] bytesB = operandB.ToBytes();

            byte[] result = new byte[bytesA.Length];

            // assert numBytesA == numBytesB
            int bytes = bytesA.Length;
            Parallel.For(0, bytes, index => 
            { 
                result[index] = (byte)(Math.Min(((bytesA[index] + bytesB[index]) / 2), 255));
                //result[index] = (byte)(Math.Min(((bytesA[index] + bytesB[index])), 255));
            });

            return new Atom(result, operandA.Result.Size, operandA.Result.PixelFormat);
                        
        }
    }
}
