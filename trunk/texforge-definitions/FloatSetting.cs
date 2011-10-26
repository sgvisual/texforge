using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace texforge_definitions
{
    [SettingAttributes(CanRandomize=true)]
    public class FloatSetting : Setting<float>
    {
        public FloatSetting(string name, float value, float defaultValue, float min, float max)
            : base(name, value, defaultValue, min, max)
        {
        }

        public override void RandomizeBetween(float min, float max)
        {
            if (!Attributes.CanRandomize)
                return;

            Random r = new Random();

            value = min + (max * (float)r.NextDouble());
            
        }
    }
}
