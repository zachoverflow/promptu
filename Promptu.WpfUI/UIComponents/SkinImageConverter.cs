using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Markup;
using System.Windows.Data;

namespace ZachJohnson.Promptu.WpfUI.UIComponents
{
    internal class SkinImageConverter : MarkupExtension, IValueConverter
    {
        private object defaultImage;

        public SkinImageConverter()
        {
        }

        public SkinImageConverter(object defaultImage)
        {
            this.defaultImage = defaultImage;
        }

        public object DefaultImage
        {
            get { return this.defaultImage; }
            set { this.defaultImage = value; }
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return this.defaultImage;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
