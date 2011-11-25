using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Xml.Linq;
using System.ComponentModel;

namespace texforge_definitions
{
    [Serializable()]
    public class Setting<T> : ISerializable
    {
        public Setting(string name, T value, T defaultValue)
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

        public Setting(SerializationInfo info, StreamingContext context)
        {
            Value = (T)(info.GetValue("Value", typeof(T)));
            DefaultValue = (T)(info.GetValue("DefaultValue", typeof(T)));
            Min = (T)(info.GetValue("Min", typeof(T)));
            Max = (T)(info.GetValue("Max", typeof(T)));
            Name = (string)(info.GetValue("Name", typeof(string)));
            Description = (string)(info.GetValue("Description", typeof(string)));
            Help = (string)(info.GetValue("Help", typeof(string)));
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Value", Value);
            info.AddValue("DefaultValue", DefaultValue);
            info.AddValue("Min", Min);
            info.AddValue("Max", Max);
            info.AddValue("Name", Name);
            info.AddValue("Description", Description);
            info.AddValue("Help", Help);
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
            get { return value; }
            set { this.value = value; }
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

        public virtual void Randomize()
        {

        }

        public virtual void RandomizeBetween(T min, T max)
        {

        }

        public virtual void Save(XElement element)
        {
            XElement baseElement = new XElement("Setting", name);            
            element.Add(baseElement);

            //baseElement.Add(new XElement("Type", typeof(T));
            baseElement.Add(new XElement("Value", value));
            baseElement.Add(new XElement("Default", defaultValue));
            baseElement.Add(new XElement("Min", minValue));
            baseElement.Add(new XElement("Max", maxValue));

        }

        public virtual void Load(System.Xml.Linq.XElement element)
        {
            XElement baseElement = element.Descendants("Setting").First();
            name = baseElement.Value;
            
            string valueStr = baseElement.Descendants("Value").First().Value;
            value = (T)TypeDescriptor.GetConverter(typeof(T)).ConvertFromString(valueStr);

            string defaultValueStr = baseElement.Descendants("Default").First().Value;
            defaultValue = (T)TypeDescriptor.GetConverter(typeof(T)).ConvertFromString(defaultValueStr);

            string minValueStr = baseElement.Descendants("Min").First().Value;
            minValue = (T)TypeDescriptor.GetConverter(typeof(T)).ConvertFromString(minValueStr);

            string maxValueStr = baseElement.Descendants("Max").First().Value;
            maxValue = (T)TypeDescriptor.GetConverter(typeof(T)).ConvertFromString(maxValueStr);

        }

        //public static readonly int seed = 0;
        //public static Random random = new Random(seed);
    }
}
