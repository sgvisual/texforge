using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace texforge.Operations
{
    public class Subtraction : Operation
    {
        protected Atom operandA;
        protected Atom operandB;

        public Subtraction(Atom a, Atom b)
        {
            operandA = a;
            operandB = b;
        }

        public enum eMode
        {
            Subtract,
            Average
        }
        eMode mode = eMode.Subtract;
        
        public eMode Mode
        {
            get { return mode; }
            set { mode = value; }
        }

        public override Atom Execute()
        {
            byte[] bytesA = operandA.ToBytes();
            byte[] bytesB = operandB.ToBytes();

            byte[] result = new byte[bytesA.Length];

            int bytes = bytesA.Length;
            Parallel.For(0, bytes, index =>
            {
                if (mode == eMode.Average)
                    result[index] = (byte)(Math.Max(((bytesA[index] - bytesB[index]) / 2), 0));
                else
                if (mode == eMode.Subtract)
                    result[index] = (byte)(Math.Max(((bytesA[index] - bytesB[index])), 0));

            });

            return new Atom(result, operandA.Result.Size, operandA.Result.PixelFormat);

        }



    }
}
