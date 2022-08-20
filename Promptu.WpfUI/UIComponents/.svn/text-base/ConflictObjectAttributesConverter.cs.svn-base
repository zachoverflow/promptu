using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Markup;
using System.Windows.Data;
using ZachJohnson.Promptu.UIModel;

namespace ZachJohnson.Promptu.WpfUI.UIComponents
{
    internal class ConflictObjectAttributesConverter : MarkupExtension, IValueConverter
    {
        public ConflictObjectAttributesConverter()
        {
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            ObjectConflictInfo info = value as ObjectConflictInfo;
            if (info != null)
            {
                StringBuilder result = new StringBuilder();

                foreach (ItemCompareEntry entry in info.Attributes)
                {
                    result.AppendLine(entry.Text);
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
