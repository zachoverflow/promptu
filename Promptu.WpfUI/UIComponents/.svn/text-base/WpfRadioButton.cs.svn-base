using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using ZachJohnson.Promptu.UIModel.Interfaces;
using System.Runtime.CompilerServices;
using System.Windows;

namespace ZachJohnson.Promptu.WpfUI.UIComponents
{
    internal class WpfRadioButton : RadioButton, IRadioButton
    {
        private EventHandler click;

        public WpfRadioButton()
        {
        }

        public event EventHandler CheckedChanged;

        bool IRadioButton.Checked
        {
            get
            {
                return this.IsChecked != false;
            }
            set
            {
                this.IsChecked = value;
            }
        }

        event EventHandler IButton.Click
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            add
            {
                this.click = (EventHandler)Delegate.Combine(this.click, value);
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            remove
            {
                this.click = (EventHandler)Delegate.Remove(this.click, value);
            }
        }

        public string Text
        {
            get
            {
                return this.Content.ToString();
            }
            set
            {
                this.Content = value;
            }
        }

        public bool Enabled
        {
            get
            {
                return this.IsEnabled;
            }
            set
            {
                this.IsEnabled = value;
            }
        }

        public object Image
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public bool Visible
        {
            get { return this.Visibility == System.Windows.Visibility.Visible; }
            set { this.Visibility = value ? Visibility.Visible : Visibility.Collapsed; }
        }

        protected override void OnClick()
        {
            base.OnClick();
            EventHandler handler = this.click;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }

        public string ToolTipText
        {
            get
            {
                return (string)this.ToolTip;
            }
            set
            {
                this.ToolTip = value;
            }
        }

        //private static void HandleCheckedPropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        //{
        //    WpfCheckBoxButton button = obj as WpfCheckBoxButton;
        //    if (button != null)
        //    {
        //        button.OnCheckedChanged(EventArgs.Empty);
        //    }
        //}

        protected override void OnChecked(RoutedEventArgs e)
        {
            if (e.Source == this)
            {
                this.OnCheckedChanged(EventArgs.Empty);
            }

            base.OnChecked(e);
        }

        protected override void OnUnchecked(RoutedEventArgs e)
        {
            if (e.Source == this)
            {
                this.OnCheckedChanged(EventArgs.Empty);
            }

            base.OnUnchecked(e);
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
