using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace ZachJohnson.Promptu.WpfUI.UIComponents
{
    internal static class PromptuTextBoxAttachments
    {
        public static readonly DependencyProperty UacErrorProperty =
            DependencyProperty.RegisterAttached(
            "UacError",
            typeof(bool),
            typeof(PromptuTextBoxAttachments));

        public static bool GetUacError(DependencyObject obj)
        {
            return (bool)obj.GetValue(UacErrorProperty);
        }

        public static void SetUacError(DependencyObject obj, bool value)
        {
            obj.SetValue(UacErrorProperty, value);
        }
    }
}
