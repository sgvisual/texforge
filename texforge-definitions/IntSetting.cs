using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace texforge_definitions
{
    [SettingAttributes(CanRandomize=true)]
    public class IntSetting : Setting<int>
    {
        public IntSetting(string name, int value, int defaultValue, int min, int max)
            : base(name, value, defaultValue, min, max)
        {

        }
    }
}
