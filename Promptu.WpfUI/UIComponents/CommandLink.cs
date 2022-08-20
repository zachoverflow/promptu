using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows;
using ZachJohnson.Promptu.UIModel.Interfaces;
using System.Runtime.CompilerServices;

namespace ZachJohnson.Promptu.WpfUI.UIComponents
{
    internal class CommandLink : Button, ICommandLink
    {
        public static readonly DependencyProperty LabelProperty =
            DependencyProperty.Register(
                "Label",
                typeof(string),
                typeof(CommandLink));

        public static readonly DependencyProperty SupplementalExplainationProperty =
            DependencyProperty.Register(
                "SupplementalExplaination",
                typeof(string),
                typeof(CommandLink));

        private EventHandler click;

        public CommandLink()
        {
        }

        event EventHandler ICommandLink.Click
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

        public string Label
        {
            get { return (string)this.GetValue(LabelProperty); }
            set { this.SetValue(LabelProperty, value); }
        }

        public string SupplementalExplaination
        {
            get { return (string)this.GetValue(SupplementalExplainationProperty); }
            set { this.SetValue(SupplementalExplainationProperty, value); }
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

        protected override void OnClick()
        {
            base.OnClick();

            EventHandler handler = this.click;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }
    }
}
