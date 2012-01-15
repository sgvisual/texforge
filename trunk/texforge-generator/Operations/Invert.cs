using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace texforge.Operations
{
    class Invert : Operation
    {
        public Invert(Atom atom)
            : base(atom)
        {
        }

        public override Atom Execute()
        {
            byte[] bytesA = atom.ToBytes();
            byte max = 0xff;
            
            for (int i = 0; i < bytesA.Length; ++i)
            {
                if ((i + 1) % 4 == 0) // don't invert alpha channel
                    continue;

                bytesA[i] = (byte)(max - bytesA[i]);
             }

            return new Atom(bytesA, atom.Result.Size, atom.Result.PixelFormat);
        }
    }
}
