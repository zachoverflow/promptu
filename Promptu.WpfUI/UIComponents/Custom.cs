using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Controls;

namespace ZachJohnson.Promptu.WpfUI.UIComponents
{
    internal class Custom
    {
        public readonly static DependencyProperty DrawingBackgroundProperty;
        public readonly static DependencyProperty HotkeyStateProperty;

        public static Brush GetDrawingBackground(DependencyObject obj)
        {
            return (Brush)obj.GetValue(DrawingBackgroundProperty);
        }

        public static void SetDrawingBackground(DependencyObject obj, object value)
        {
            obj.SetValue(DrawingBackgroundProperty, value);
        }

        public static UIModel.HotkeyState GetHotkeyState(DependencyObject obj)
        {
            return (UIModel.HotkeyState)obj.GetValue(HotkeyStateProperty);
        }

        public static void SetHotkeyState(DependencyObject obj, object value)
        {
            obj.SetValue(HotkeyStateProperty, value);
        }

        static Custom()
        {
            //try
            //{
                FrameworkPropertyMetadata metadata = new FrameworkPropertyMetadata((Brush)null);
                DrawingBackgroundProperty = DependencyProperty.RegisterAttached(
                    "DrawingBackground",
                    typeof(Brush),
                    typeof(Custom),
                    metadata);

                HotkeyStateProperty = DependencyProperty.RegisterAttached(
                    "HotkeyState",
                    typeof(UIModel.HotkeyState),
                    typeof(Custom),
                    new FrameworkPropertyMetadata(UIModel.HotkeyState.Available));
            //}
            //catch (Exception eX)
            //{
            //    System.IO.File.WriteAllText("c:\\tmp\\exception.txt", eX.Message);
            //}
        }
    }
}
