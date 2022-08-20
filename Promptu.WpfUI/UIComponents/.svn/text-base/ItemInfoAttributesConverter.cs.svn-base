using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using ZachJohnson.Promptu.UIModel;

namespace ZachJohnson.Promptu.WpfUI.UIComponents
{
    internal class ItemInfoAttributesConverter : IValueConverter
    {
        public ItemInfoAttributesConverter()
        {
        }

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            ItemInfo info = value as ItemInfo;
            if (info != null)
            {
                StringBuilder result = new StringBuilder();

                foreach (string attribute in info.Attributes)
                {
                    result.AppendLine(attribute);
                }

                return result.ToString();
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
