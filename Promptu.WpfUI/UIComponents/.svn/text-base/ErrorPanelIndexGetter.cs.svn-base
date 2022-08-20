using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Markup;
using System.Windows.Data;
using System.Windows;
using System.Windows.Controls;
using System.Globalization;

namespace ZachJohnson.Promptu.WpfUI.UIComponents
{
    internal class ErrorPanelIndexGetter : DependencyObject, IMultiValueConverter
    {
        //BulkObservableCollection<FeedbackMessage> allMessages;
        //public static readonly DependencyProperty AllMessagesProperty =
        //    DependencyProperty.Register(
        //    "AllMessages",
        //    typeof(BulkObservableCollection<FeedbackMessage>),
        //    typeof(ErrorPanelIndexGetter));

        public ErrorPanelIndexGetter()
        {
        }

        //public BulkObservableCollection<FeedbackMessage> AllMessages
        //{
        //    get { return (BulkObservableCollection<FeedbackMessage>)this.GetValue(AllMessagesProperty); }
        //    set { this.SetValue(AllMessagesProperty, value); }
        //}

        //public BulkObservableCollection<FeedbackMessage> AllMessages
        //{
        //    get { return this.allMessages; }
        //    set { this.allMessages = value; }
        //}

        //public override object ProvideValue(IServiceProvider serviceProvider)
        //{
        //    return this;
        //}

        public object Convert(object[] value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            FeedbackMessage message = value[0] as FeedbackMessage;
            DataGrid grid = value[1] as DataGrid;
            if (message != null && grid != null)
            {
                int index = ((BulkObservableCollection<FeedbackMessage>)grid.DataContext).IndexOf(message) + 1;
                return index.ToString(CultureInfo.CurrentCulture);
            }

            return null;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
