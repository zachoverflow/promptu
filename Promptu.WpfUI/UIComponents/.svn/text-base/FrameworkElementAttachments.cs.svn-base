using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media.Animation;

namespace ZachJohnson.Promptu.WpfUI.UIComponents
{
    internal static class FrameworkElementAttachments
    {
        public static readonly DependencyProperty DisableAnimationsProperty;

        static FrameworkElementAttachments()
        {
            FrameworkPropertyMetadata disableAnimationsMetadata = new FrameworkPropertyMetadata();
            disableAnimationsMetadata.Inherits = true;

            DisableAnimationsProperty = DependencyProperty.RegisterAttached(
               "DisableAnimations",
               typeof(bool),
               typeof(FrameworkElementAttachments),
               disableAnimationsMetadata);
        }

        public static bool GetDisableAnimations(DependencyObject obj)
        {
            return (bool)obj.GetValue(DisableAnimationsProperty);
        }

        public static void SetDisableAnimations(DependencyObject obj, bool value)
        {
            obj.SetValue(DisableAnimationsProperty, value);
        }

        public static void ApplyAnimation(this FrameworkElement element, Storyboard storyboard)
        {
            if (element == null)
            {
                throw new ArgumentNullException("element");
            }

            if (!GetDisableAnimations(element))
            {
                element.BeginStoryboard(storyboard);
            }
            else
            {
                storyboard.Begin(element, true);
                storyboard.SkipToFill(element);
            }
        }
    }
}
