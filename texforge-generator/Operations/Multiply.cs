﻿using System;
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
            return MultiplyBitmaps(ref operandA, ref operandB);
        }

    }
}