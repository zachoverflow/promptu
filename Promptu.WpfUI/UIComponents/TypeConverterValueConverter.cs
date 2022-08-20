using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.ComponentModel;
using System.Windows;

namespace ZachJohnson.Promptu.WpfUI.UIComponents
{
    internal class TypeConverterValueConverter : DependencyObject, IValueConverter
    {
        public static readonly DependencyProperty TypeConverterProperty =
            DependencyProperty.Register(
                "TypeConverter",
                typeof(TypeConverter),
                typeof(TypeConverterValueConverter));

        public TypeConverterValueConverter()
        {
        }

        public TypeConverter TypeConverter
        {
            get { return (TypeConverter)this.GetValue(TypeConverterProperty); }
            set { this.SetValue(TypeConverterProperty, value); }
        }

        //public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        //{
        //    TypeConverter converter = (TypeConverter)values[1];

        //    return converter.ConvertTo(values[0], targetType);
        //}

        //public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        //{
        //    object[] values = new object[2];

        //    values[1] = Binding.DoNothing;

        //    TypeConverter converter = (TypeConverter)values[1];

        //    return converter.ConvertTo(value, targetTypes[0]);
        //}

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            TypeConverter converter = this.TypeConverter;
            if (converter != null)
            {
                return converter.ConvertTo(value, targetType);
            }

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            TypeConverter converter = this.TypeConverter;
            if (converter != null)
            {
                return converter.ConvertTo(value, targetType);
            }

            return value;
        }
    }
}
