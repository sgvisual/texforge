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

    [SettingAttributes(CanRandomize=false)]
    public class BlendMode : texforge_definitions.Setting<eBlendMode>
    {

        public BlendMode(string name, eBlendMode value, eBlendMode defaultValue)
            : base(name, value, defaultValue)
        {
        }


    }

}
