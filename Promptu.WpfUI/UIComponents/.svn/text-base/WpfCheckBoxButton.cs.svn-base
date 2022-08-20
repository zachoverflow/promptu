using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZachJohnson.Promptu.UIModel.Interfaces;
using System.Windows;

namespace ZachJohnson.Promptu.WpfUI.UIComponents
{
    internal class WpfCheckBoxButton : WpfButton, ICheckBoxButton
    {
        public static readonly DependencyProperty CheckedProperty =
            DependencyProperty.Register(
            "Checked",
            typeof(bool),
            typeof(WpfCheckBoxButton),
            new PropertyMetadata(HandleCheckedPropertyChanged));

        public WpfCheckBoxButton()
        {
        }

        public event EventHandler CheckedChanged;

        public bool Checked
        {
            get
            {
                return (bool)this.GetValue(CheckedProperty);
            }
            set
            {
                this.SetValue(CheckedProperty, value);
            }
        }

        private static void HandleCheckedPropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            WpfCheckBoxButton button = obj as WpfCheckBoxButton;
            if (button != null)
            {
                button.OnCheckedChanged(EventArgs.Empty);
            }
        }

        protected virtual void OnCheckedChanged(EventArgs e)
        {
            EventHandler handler = this.CheckedChanged;
            if (handler != null)
            {
                handler(this, e);
            }
        }
    }
}
