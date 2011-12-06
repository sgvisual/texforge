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

        public enum eMode
        {
            Add,
            Average
        }
        eMode mode = eMode.Add;
        public eMode Mode
        {
            get { return mode; }
            set { mode = value; }
        }

        public Addition(Atom a, Atom b)
        {
            operandA = a;
            operandB = b;
        }

        protected Atom AddBitmaps(ref Atom A, ref Atom B)
        {
            if (operandA == null)
                return operandB;
            if (operandB == null)
                return operandA;
            byte[] bytesA = operandA.ToBytes();
            byte[] bytesB = operandB.ToBytes();

            if (bytesA.Length != bytesB.Length)
                throw new Exception("Atom sizes must be the same for Addition operation");

            byte[] result = new byte[bytesB.Length];

            int bytes = bytesB.Length;
            Parallel.For(0, bytes, index =>
            {
                if (mode == eMode.Average)
                    result[index] = (byte)(Math.Min(((bytesA[index] + bytesB[index]) / 2), 255));
                else
                if (mode == eMode.Add)
                    result[index] = (byte)(Math.Min(((bytesA[index] + bytesB[index])), 255));
            });

            return new Atom(result, operandA.Result.Size, operandA.Result.PixelFormat);

        }

        public override Atom Execute()
        {
            return AddBitmaps(ref operandA, ref operandB);                        
        }
    }
}
