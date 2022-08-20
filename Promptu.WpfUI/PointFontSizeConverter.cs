using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace ZachJohnson.Promptu.WpfUI
{
    public class PointFontSizeConverter : TypeConverter
    {
        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            if (destinationType == typeof(int))
            {
                return true;
            }

            return base.CanConvertTo(context, destinationType);
        }

        public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(int))
            {
                return (int)Math.Round(((double)value) * 72 / 96, MidpointRounding.AwayFromZero);
            }

            return base.ConvertTo(context, culture, value, destinationType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
        {
            if (value != null)
            {
                if (value is int)
                {
                    double converted = ((int)value * 96 / 72);
                    return converted;
                }
            }

            return base.ConvertFrom(context, culture, value);
        }
    }
}
