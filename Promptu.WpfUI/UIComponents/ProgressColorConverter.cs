//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Windows.Data;
//using System.Windows.Media;
//using System.Windows.Controls;

//namespace ZachJohnson.Promptu.WpfUI.UIComponents
//{
//    internal class ProgressColorConverter : IValueConverter
//    {
//        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
//        {
//            bool error = (bool)value;

//            if (error)
//            {
//                return Color.FromRgb(255, 0, 0);
//            }

//            return ProgressBar.ForegroundProperty.GetMetadata().DefaultValue;
//        }

//        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
//        {
//            throw new NotImplementedException();
//        }
//    }
//}
