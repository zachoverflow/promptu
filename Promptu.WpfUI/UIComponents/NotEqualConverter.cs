using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows.Markup;

namespace ZachJohnson.Promptu.WpfUI.UIComponents
{
    internal class NotEqualConverter : MarkupExtension, IValueConverter
    {
        //private NotBinding notBinding;
        private object notEqualTo;

        public NotEqualConverter(object notEqualTo)
        {
            this.notEqualTo = notEqualTo;
            //this.notBinding = notBinding;
        }

        //public object Value
        //{
        //    get { return this.notEqualTo; }
        //    set { this.notEqualTo = value; }
        //}

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            //object compareValue = this.notBinding.Binding.ProvideValue(null);
            return value != this.notEqualTo;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }
}
