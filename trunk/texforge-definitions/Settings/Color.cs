using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace texforge_definitions.Settings
{
    [SettingAttributes(CanRandomize=true)]
    public class Color : Setting<Types.Color>
    {
        public Color(string name, Types.Color value, Types.Color defaultValue, Types.Color min, Types.Color max)
            : base(name, value, defaultValue, min, max)
        {
        }

        public override void RandomizeBetween(Types.Color min, Types.Color max)
        {
            if (!Attributes.CanRandomize)
                return;

            Random r = new Random();

            value.red = (byte)r.Next(255);
            value.green = (byte)r.Next(255);
            value.blue = (byte)r.Next(255);
            value.alpha = (byte)r.Next(255);
            
        }

    }
}
