using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace texforge_definitions.Settings
{
    [Serializable()]
    public class SettingBase : ISerializable
    {
        public SettingBase()
        {
        }

        public SettingBase(SerializationInfo info, StreamingContext context)
        {
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
        }
    }
}
