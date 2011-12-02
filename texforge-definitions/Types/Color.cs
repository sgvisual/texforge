using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace texforge_definitions.Types
{
    [TypeConverter(typeof(Color.ColorTypeConverter))]
    public class Color
    {
        public byte red;
        public byte green;
        public byte blue;
        public byte alpha;

        public Color(System.Drawing.Color color)
        {
            red = color.R;
            green = color.G;
            blue = color.B;
            alpha = color.A;
        }

        public System.Drawing.Color WindowsColor
        {
            get { return System.Drawing.Color.FromArgb(alpha, red, green, blue); } 
        }

        public Color(byte red, byte green, byte blue, byte alpha)
        {
            this.red = red;
            this.green = green;
            this.blue = blue;
            this.alpha = alpha;
        }

        public class ColorTypeConverter : TypeConverter
        {
            public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
            {
                if (sourceType == typeof(string))
                {
                    return true;
                }
                return base.CanConvertFrom(context, sourceType);
            }

            public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
            {
                if (value is string)
                {
                    string[] v = ((string)value).Split(new char[] { ',' });
                    string r = v[0].ToUpper().Replace(@"R=", "");
                    byte red = 0;
                    byte.TryParse(r, out red);
                    string g = v[1].ToUpper().Replace(@"G=", "");
                    byte green = 0;
                    byte.TryParse(g, out green);
                    string b = v[2].ToUpper().Replace(@"B=", "");
                    byte blue = 0;
                    byte.TryParse(b, out blue);
                    string a = v[0].ToUpper().Replace(@"A=", "");
                    byte alpha = 0;
                    byte.TryParse(a, out alpha);

                    return new Color(red, green, blue, alpha);
                }
                return base.ConvertFrom(context, culture, value);
            }

            public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
            {

                if (destinationType is string)
                {
                    Color color = (Color)value;
                    return string.Format("r={0},g={1},b={2},a={3}", color.red, color.green, color.blue, color.alpha);
                }

                return base.ConvertTo(context, culture, value, destinationType);
            }
        }

        public override string ToString()
        {
            return string.Format("r={0},g={1},b={2},a={3}", red, green, blue, alpha);
        }

        

        public UInt32 ToUInt32()
        {
            return (UInt32)((alpha << 24) | ((red) << 16) | ((green << 8)) | ((blue)));
        }

    }
}
