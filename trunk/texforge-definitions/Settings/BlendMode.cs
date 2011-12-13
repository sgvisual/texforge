using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace texforge_definitions.Settings
{
    public enum eBlendMode
    {
        None,
        Add,
        Subtract,
        Multiply,
        Divide,
        Screen,
        Overlay,
        Dodge,
        Burn
    }

    public class BlendMode : Enumeration
    {

        public BlendMode(string name, eBlendMode value, eBlendMode defaultValue)
            : base(name, value.ToString(), defaultValue.ToString(), typeof(eBlendMode))
        {
            
        }


    }

}
