using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace texforge_definitions.Settings
{
    public enum eGeneratorType
    {
        None,
        Noise,
        PerlinNoise
    }

    public class GeneratorType : Enumeration
    {
        public GeneratorType(string name, eGeneratorType value, eGeneratorType defaultValue)
            : base(name, value.ToString(), defaultValue.ToString(), typeof(eGeneratorType))
        {
            
        }


    }
}
