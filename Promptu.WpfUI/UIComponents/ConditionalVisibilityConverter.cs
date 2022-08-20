using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Markup;
using System.Windows;
using System.Windows.Data;

namespace ZachJohnson.Promptu.WpfUI.UIComponents
{
    internal class ConditionalVisibilityConverter : MarkupExtension, IValueConverter
    {
        private Visibility ifEmpty;
        private Visibility ifNotEmpty;

        public ConditionalVisibilityConverter(Visibility ifEmpty, Visibility ifNotEmpty)
        {
            this.ifEmpty = ifEmpty;
            this.ifNotEmpty = ifNotEmpty;
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return String.IsNullOrEmpty((string)value) ? this.ifEmpty : this.ifNotEmpty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
