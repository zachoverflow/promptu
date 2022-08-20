using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows;

namespace ZachJohnson.Promptu.WpfUI.UIComponents
{
    internal class InvincibleBorder : Border
    {
        static InvincibleBorder()
        {
            Border.HeightProperty.OverrideMetadata(typeof(InvincibleBorder), new FrameworkPropertyMetadata(HandleHeightChanged));
        }

        public InvincibleBorder()
        {
        }

        private static void HandleHeightChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            double newValue = (double)e.NewValue;

            if (!double.IsNaN(newValue))
            {
                ((InvincibleBorder)obj).Height = double.NaN;
            }
        }
    }
}
