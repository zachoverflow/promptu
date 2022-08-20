using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Data;

namespace ZachJohnson.Promptu.WpfUI.UIComponents
{
    internal class DataTemplateAttachments
    {
        public readonly static DependencyProperty BindingExpressionProperty;

        public static Binding GetBindingExpression(DependencyObject obj)
        {
            return (Binding)obj.GetValue(BindingExpressionProperty);
        }

        public static void SetBindingExpression(DependencyObject obj, Binding value)
        {
            obj.SetValue(BindingExpressionProperty, value);
        }

        static DataTemplateAttachments()
        {
            FrameworkPropertyMetadata metadata = new FrameworkPropertyMetadata((Binding)null);
            BindingExpressionProperty = DependencyProperty.RegisterAttached(
                "BindingExpression",
                typeof(Binding),
                typeof(DataTemplateAttachments),
                metadata);
        }
    }
}
