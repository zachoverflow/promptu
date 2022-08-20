using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using ZachJohnson.Promptu.UIModel.Interfaces;
using System.Runtime.CompilerServices;
using System.Windows;
using ZachJohnson.Promptu.UIModel;

namespace ZachJohnson.Promptu.WpfUI.UIComponents
{
    internal class WpfButton : Button, IButton
    {
        public static readonly DependencyProperty DialogResultProperty;
        private EventHandler click;

        public WpfButton()
        {
        }

        static WpfButton()
        {
            DialogResultProperty = DependencyProperty.Register(
                "DialogResult",
                typeof(WpfDialogResult),
                typeof(WpfButton));
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

        public WpfDialogResult DialogResult
        {
            get { return (WpfDialogResult)this.GetValue(DialogResultProperty); }
            set { this.SetValue(DialogResultProperty, value); }
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

            object localDialogResultObject = this.ReadLocalValue(DialogResultProperty);

            if (localDialogResultObject != DependencyProperty.UnsetValue)
            {
                WpfDialogResult localDialogResult = (WpfDialogResult)localDialogResultObject;
                if (localDialogResult != WpfDialogResult.None)
                {
                    try
                    {
                        Window.GetWindow(this).DialogResult = (WpfDialogResult)localDialogResult == WpfDialogResult.OK;
                    }
                    catch (InvalidOperationException)
                    {
                    }
                }
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
    }
}
