using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using ZachJohnson.Promptu.UserModel;

namespace ZachJohnson.Promptu.WpfUI.UIComponents
{
    internal class ObjectNameConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            CompositeItem<Command, List> command = value as CompositeItem<Command, List>;

            if (command != null)
            {
                return command.Item.Name;
            }

            CompositeItem<Function, List> function = value as CompositeItem<Function, List>;

            if (function != null)
            {
                return function.Item.GetNamedSignatureIfPossible(
                    new UserModel.Collections.AssemblyReferenceCollectionComposite(InternalGlobals.CurrentProfile.Lists, function.ListFrom));
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
