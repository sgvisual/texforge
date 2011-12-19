using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace texforge_definitions.Settings
{
    [SettingAttributes(CanRandomize = false)]
    public class String : Setting<string>
    {
        public String(string name, string value, string defaultValue, string min, string max)
            : base(name, value, defaultValue, min, max)
        {
        }



    }
}
