using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace texforge_definitions.Settings
{
    [SettingAttributes(CanRandomize = false)]
    public class Bool : Setting<bool>
    {
        public Bool(string name, bool value, bool defaultValue, bool min, bool max)
            : base(name, value, defaultValue, min, max)
        {
        }
    }

}
