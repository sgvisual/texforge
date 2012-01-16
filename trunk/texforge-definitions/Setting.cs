using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Xml.Linq;
using System.ComponentModel;
using texforge_definitions.Settings;

namespace texforge_definitions
{
    public class Setting<T> : SettingBase
    {
        public Setting(string name, T value, T defaultValue)
            : base()
        {
            this.name = name;
            this.value = value;
            this.defaultValue = defaultValue;
        }

        public Setting(string name, T value, T defaultValue, T min, T max)
        {
            this.name = name;
            this.value = value;
            this.defaultValue = defaultValue;
            this.minValue = min;
            this.maxValue = max;

            SetAttributes();
        }

        public void SetAttributes()
        {
            Attribute[] attrs = Attribute.GetCustomAttributes(this.GetType());
            foreach (Attribute a in attrs)
            {
                if (a is SettingAttributes)
                {
                    attributes = (SettingAttributes)a;
                    break;
                }
            }

        }

        protected T value;
        protected T defaultValue;
        protected T minValue;
        protected T maxValue;
        protected SettingAttributes attributes;

        protected string name;
        protected string description;
        protected string help;

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public string Description
        {
            get { return description; }
            set { description = value; }
        }

        public string Help
        {
            get { return help; }
            set { help = value; }
        }

        public SettingAttributes Attributes
        {
            get { return attributes; }
            set { attributes = value; }
        }

        public T Value
        {
            get { return Clamp(value); }
            set { this.value = value; if (OnChange != null) OnChange.Invoke(this, null); }
        }

        public T DefaultValue
        {
            get { return defaultValue; }
            set { defaultValue = value; }
        }

        public T Max
        {
            get { return maxValue; }
            set { maxValue = value; }
        }

        public T Min
        {
            get { return minValue; }
            set { minValue = value; }
        }

        // Clamp must be implemented by each deriving settings
        public virtual T Clamp(T value)
        {
            return value;
        }
  
        public virtual void Randomize()
        {
            RandomizeBetween(minValue, maxValue);
        }

        public virtual void RandomizeBetween(T min, T max)
        {
        }

        public override void Save(XElement element)
        {
            XElement baseElement = new XElement(name);
            
            baseElement.Add(new XElement("Value", value.ToString()));
            
            if ( defaultValue != null )
                baseElement.Add(new XElement("Default", defaultValue.ToString()));
            
            if ( minValue != null )
                baseElement.Add(new XElement("Min", minValue.ToString()));
            
            if ( maxValue != null )
                baseElement.Add(new XElement("Max", maxValue.ToString()));

            element.Add(baseElement);

        }

        public override void Load(ref System.Xml.Linq.XElement element)
        {

            //name = element.Descendants(;

            string valueStr = element.Descendants("Value").First().Value;
            value = (T)TypeDescriptor.GetConverter(typeof(T)).ConvertFromString(valueStr);

            string defaultValueStr = element.Descendants("Default").First().Value;
            defaultValue = (T)TypeDescriptor.GetConverter(typeof(T)).ConvertFromString(defaultValueStr);

            if ( element.Descendants("Min").Count() > 0 )
            {
                string minValueStr = element.Descendants("Min").First().Value;
                minValue = (T)TypeDescriptor.GetConverter(typeof(T)).ConvertFromString(minValueStr);
            }

            if (element.Descendants("Max").Count() > 0)
            {
                string maxValueStr = element.Descendants("Max").First().Value;
                maxValue = (T)TypeDescriptor.GetConverter(typeof(T)).ConvertFromString(maxValueStr);
            }

            IEnumerable<XElement> siblings = element.ElementsAfterSelf();
            if (siblings.Count() == 0)
                element = null;
            else
                element = siblings.First();
        }

        public event EventHandler OnChange;

        //public static readonly int seed = 0;
        //public static Random random = new Random(seed);
    }
}
