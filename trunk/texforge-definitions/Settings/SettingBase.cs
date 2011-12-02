using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Xml.Linq;

namespace texforge_definitions.Settings
{
    public abstract class SettingBase 
    {
        public abstract void Save(XElement element);
        public abstract void Load(XElement element);
    }
}
