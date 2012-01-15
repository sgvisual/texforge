using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace texforge.Operations
{
    public abstract class Operation 
    {
        public Operation()
        {
        }

        public Operation(Atom atom)
        {
            this.atom = atom;
        }

        public abstract Atom Execute();

        protected Atom atom;
    }
}
