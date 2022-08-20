using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using ZachJohnson.Promptu.UIModel.Interfaces;
using System.Windows;

namespace ZachJohnson.Promptu.WpfUI.UIComponents
{
    internal class WpfMenuSeparator : Separator, IMenuSeparator
    {
        public WpfMenuSeparator()
        {
        }

        public bool Available
        {
            get
            {
                return this.Visibility == Visibility.Visible;
            }
            set
            {
                this.Visibility = value ? System.Windows.Visibility.Visible : System.Windows.Visibility.Collapsed;
            }
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            this.Margin = new Thickness(0);
        }
    }
}
