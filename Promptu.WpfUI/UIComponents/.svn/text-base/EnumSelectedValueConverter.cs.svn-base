using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Markup;
using System.Windows.Data;
using ZachJohnson.Promptu.PluginModel;
using System.Collections.ObjectModel;
using System.Windows;
using System.ComponentModel;

namespace ZachJohnson.Promptu.WpfUI.UIComponents
{
    internal class EnumSelectedValueConverter : MarkupExtension, IMultiValueConverter
    {
        //public static readonly DependencyProperty ConversionInfoProperty =
        //    DependencyProperty.Register(
        //        "ConversionInfo",
        //        typeof(object),
        //        typeof(EnumValueConverter),
        //        new PropertyMetadata(HandleConversionInfoChanged));

        //private ObservableCollection<object> displayValues;

        public EnumSelectedValueConverter()
        {
        }

        //public event PropertyChangedEventHandler PropertyChanged;

        //public object ConversionInfo
        //{
        //    get { return this.GetValue(ConversionInfoProperty); }
        //    set { this.SetValue(ConversionInfoProperty, value); }
        //}

        //private static void HandleConversionInfoChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        //{
        //    EnumValueConverter converter = sender as EnumValueConverter;
        //    converter.RegenerateDisplayValues();
        //    //converter.OnPropertyChanged(new PropertyChangedEventArgs("DisplayValues"));
        //}

        //public ObservableCollection<object> DisplayValues
        //{
        //    get
        //    {
        //        if (this.displayValues == null)
        //        {
        //            this.RegenerateDisplayValues();
        //        }

        //        return this.displayValues;
        //    }
        //}

        //private void RegenerateDisplayValues()
        //{
        //    if (this.displayValues == null)
        //    {
        //        this.displayValues = new ObservableCollection<object>();
        //    }
        //    else
        //    {
        //        this.displayValues.Clear();
        //    }

        //    EnumConversionInfo enumConversionInfo = ConversionInfo as EnumConversionInfo;

        //    if (enumConversionInfo != null)
        //    {
        //        foreach (Enum enumValue in Enum.GetValues(enumConversionInfo.EnumType))
        //        {
        //            foreach (EnumValueInfo valueInfo in enumConversionInfo.Entries)
        //            {
        //                if (valueInfo.Value == enumValue)
        //                {
        //                    this.displayValues.Add(valueInfo);
        //                    continue;
        //                }
        //            }

        //            this.displayValues.Add(enumValue);
        //        }
        //    }
        //}

        //public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        //{
        //    Enum enumValue = value as Enum;

        //    if (enumValue == null)
        //    {
        //        return value;
        //    }

        //    EnumConversionInfo enumConversionInfo = parameter as EnumConversionInfo;

        //    if (enumConversionInfo == null)
        //    {
        //        return value;
        //    }

        //    foreach (EnumValueInfo valueInfo in enumConversionInfo.Entries)
        //    {
        //        if (valueInfo.Value == enumValue)
        //        {
        //            return valueInfo;
        //        }
        //    }

        //    return value;
        //}

        //public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        //{
        //    EnumValueInfo valueInfo = value as EnumValueInfo;

        //    if (valueInfo != null)
        //    {
        //        return valueInfo.Value;
        //    }

        //    return value;
        //}

        //protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)
        //{
        //    PropertyChangedEventHandler handler = this.PropertyChanged;
        //    if (handler != null)
        //    {
        //        handler(this, e);
        //    }
        //}

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }

        //    public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        //    {
        //        throw new NotImplementedException();
        //    }

        //    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        //    {
        //        EnumValueInfo valueInfo = value as EnumValueInfo;

        //        if (valueInfo != null)
        //        {
        //            return valueInfo.Value;
        //        }
        //    }
        //}

        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            Enum enumValue = values[0] as Enum;

            if (enumValue == null)
            {
                return values[0];
            }

            EnumConversionInfo enumConversionInfo = values[1] as EnumConversionInfo;

            if (enumConversionInfo == null)
            {
                return values[0];
            }

            foreach (EnumValueInfo valueInfo in enumConversionInfo.Entries)
            {
                if (enumValue.Equals(valueInfo.Value))
                {
                    return valueInfo;
                }
            }

            return values[0];
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            object[] result = new object[2];

            EnumValueInfo valueInfo = value as EnumValueInfo;

            result[0] = valueInfo != null ? valueInfo.Value : value;
            result[1] = Binding.DoNothing;

            return result;
        }
    }
}
