using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace texforge.Generators
{
    public abstract class Generator
    {
        public abstract Atom Generate();
    }
}
