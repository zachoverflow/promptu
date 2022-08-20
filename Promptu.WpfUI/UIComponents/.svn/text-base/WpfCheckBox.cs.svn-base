using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using ZachJohnson.Promptu.UIModel.Interfaces;
using System.Windows;

namespace ZachJohnson.Promptu.WpfUI.UIComponents
{
    internal class WpfCheckBox : CheckBox, ICheckBox
    {
        public WpfCheckBox()
        {
        }

        public event EventHandler CheckedChanged;

        bool ICheckBox.Checked
        {
            get
            {
                bool? isChecked = this.IsChecked;
                return isChecked != null && isChecked.Value;
            }

            set
            {
                this.IsChecked = value;
            }
        }

        public string Text
        {
            get { return (string)this.Content; }
            set { this.Content = value; }
        }

        public string ToolTipText
        {
            get { return (string)this.ToolTip; }
            set { this.ToolTip = value; }
        }

        public bool Enabled
        {
            get { return this.IsEnabled; }
            set { this.IsEnabled = value; }
        }

        public bool Visible
        {
            get { return this.Visibility == System.Windows.Visibility.Visible; }
            set { this.Visibility = value ? Visibility.Visible : Visibility.Collapsed; }
        }

        protected override void OnChecked(System.Windows.RoutedEventArgs e)
        {
            base.OnChecked(e);
            this.OnCheckedChanged(EventArgs.Empty);
        }

        protected override void OnUnchecked(System.Windows.RoutedEventArgs e)
        {
            base.OnUnchecked(e);
            this.OnCheckedChanged(EventArgs.Empty);
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
