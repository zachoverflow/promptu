using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Markup;
using System.Windows.Data;
using ZachJohnson.Promptu.PluginModel;

namespace ZachJohnson.Promptu.WpfUI.UIComponents
{
    internal class EnumValuesConverter : MarkupExtension, IValueConverter
    {
        public EnumValuesConverter()
        {
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            EnumConversionInfo enumConversionInfo = value as EnumConversionInfo;

            if (enumConversionInfo != null)
            {
                Array enumValues = Enum.GetValues(enumConversionInfo.EnumType);
                List<object> displayValues = new List<object>(enumValues.Length);

                foreach (Enum enumValue in enumValues)
                {
                    bool added = false;
                    foreach (EnumValueInfo valueInfo in enumConversionInfo.Entries)
                    {
                        if (enumValue.Equals(valueInfo.Value))
                        {
                            displayValues.Add(valueInfo);
                            added = true;
                            break;
                        }
                    }

                    if (added)
                    {
                        continue;
                    }

                    displayValues.Add(enumValue);
                }

                return displayValues;
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
