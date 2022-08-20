using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using ZachJohnson.Promptu.UserModel;

namespace ZachJohnson.Promptu.WpfUI.UIComponents
{
    internal class ObjectImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            CompositeItem<Command, List> command = value as CompositeItem<Command, List>;

            if (command != null)
            {
                return InternalGlobals.GuiManager.ToolkitHost.Images.Command;
            }

            CompositeItem<Function, List> function = value as CompositeItem<Function, List>;

            if (function != null)
            {
                return InternalGlobals.GuiManager.ToolkitHost.Images.Function;
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
