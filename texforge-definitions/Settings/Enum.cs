using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace texforge_definitions.Settings
{
    [SettingAttributes(CanRandomize=false)]
    public abstract class Enumeration : texforge_definitions.Setting<string>
    {
        List<string> availableValues = new List<string>();
        public List<string> AvailableValues
        {
            get { return availableValues; }
        }

        protected Enumeration(string name, string value, string defaultValue, Type enumeration)
            : base(name, value, defaultValue)
        {
            foreach (object item in Enum.GetValues(enumeration))
                availableValues.Add(item.ToString());
        }
    }
}
