using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows;

namespace ZachJohnson.Promptu.WpfUI.UIComponents
{
    internal class CtrlClickLinkConverter : IValueConverter
    {
        public CtrlClickLinkConverter()
        {
        }

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            Span span = new Span();
            span.Inlines.Add(new Run(value.ToString()));
            span.Inlines.Add(new LineBreak());
            Run ctrlClick = new Run(Localization.UIResources.CtrlClickToFollowLink);
            ctrlClick.FontWeight = FontWeights.Bold;
            span.Inlines.Add(ctrlClick);

            return span;
            //StringBuilder builder = new StringBuilder();
            //builder.AppendLine(value.ToString());
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
