using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Markup;
using System.Windows.Data;
using ZachJohnson.Promptu.PluginModel;

namespace ZachJohnson.Promptu.WpfUI.UIComponents
{
    internal class GroupingConversionInfoConverter : MarkupExtension, IValueConverter
    {
        public GroupingConversionInfoConverter()
        {
        }

        public bool ForEditControl
        {
            get;
            set;
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            GroupingConversionInfo conversionInfo = value as GroupingConversionInfo;
            if (conversionInfo == null)
            {
                return value;
            }

            if (this.ForEditControl)
            {
                return conversionInfo.GroupEditControl && conversionInfo.GroupName != null ? conversionInfo.GroupName + "_edit" : null;
            }

            return conversionInfo.GroupName;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
