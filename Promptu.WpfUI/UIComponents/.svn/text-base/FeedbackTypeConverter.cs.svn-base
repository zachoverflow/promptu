using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Markup;
using System.Windows.Data;

namespace ZachJohnson.Promptu.WpfUI.UIComponents
{
    internal class FeedbackTypeConverter : MarkupExtension, IValueConverter
    {
        private object ifError;
        private object ifWarning;
        private object ifMessage;

        public FeedbackTypeConverter(object ifError, object ifWarning, object ifMessage)
        {
            this.ifError = ifError;
            this.ifWarning = ifWarning;
            this.ifMessage = ifMessage;
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            FeedbackType type = (FeedbackType)value;

            switch (type)
            {
                case FeedbackType.Warning:
                    return this.ifWarning;
                case FeedbackType.Message:
                    return this.ifMessage;
                case FeedbackType.Error:
                    return this.ifError;
                default:
                    return null;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
