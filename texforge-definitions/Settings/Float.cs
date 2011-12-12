using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace texforge_definitions.Settings
{
    [SettingAttributes(CanRandomize = true)]
    public class Float : Setting<float>
    {
        public Float(string name, float value, float defaultValue, float min, float max)
            : base(name, value, defaultValue, min, max)
        {
        }

        public override float Clamp(float value)
        {
            if (value < minValue) return minValue;
            if (value > maxValue) return maxValue;
            return value;
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
