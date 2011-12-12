using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace texforge_definitions.Settings
{
    [SettingAttributes(CanRandomize = true)]
    public class Int : Setting<int>
    {
        public Int(string name, int value, int defaultValue, int min, int max)
            : base(name, value, defaultValue, min, max)
        {
        }

        public override int Clamp(int value)
        {
            if (value > maxValue) return maxValue;
            if (value < minValue) return minValue;
            return value;
        }

        public override void RandomizeBetween(int min, int max)
        {
            if (!Attributes.CanRandomize)
                return;

            Random r = new Random();

            value = r.Next(min, max);

        }
    }
}
