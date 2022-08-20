using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using ZachJohnson.Promptu.UIModel.Interfaces;
using System.Windows;
using System.Windows.Media;
using System.Runtime.CompilerServices;

namespace ZachJohnson.Promptu.WpfUI.UIComponents
{
    public class WpfToolbarButton : Button, IToolbarButton, IButton
    {
        public static readonly DependencyProperty ImageProperty =
            DependencyProperty.Register(
            "Image",
            typeof(ImageSource),
            typeof(WpfToolbarButton));

        public static readonly DependencyProperty FullDisableProperty =
            DependencyProperty.Register(
            "FullDisable",
            typeof(bool),
            typeof(WpfToolbarButton),
            new PropertyMetadata(true));

        private EventHandler click;
        private ImageAndText imageAndText;

        public WpfToolbarButton()
        {
            this.imageAndText = new ImageAndText(null, null);
            //this.Content = this.imageAndText;
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

        public bool FullDisable
        {
            get { return (bool)this.GetValue(FullDisableProperty); }
            set { this.SetValue(FullDisableProperty, value); }
        }

        public string Text
        {
            get
            {
                return (string)this.Content;
            }
            set
            {
                this.Content = value;
            }
        }

        public string ToolTipText
        {
            get
            {
                return this.ToolTip as string;
            }
            set
            {
                this.ToolTip = value;
            }
        }

        public object Image
        {
            get 
            { 
                return this.GetValue(ImageProperty); 
            }

            set 
            {
                if (value is ImageSource)
                {
                    this.SetValue(ImageProperty, value);
                }
            }
        }

        //public object Temp
        //{
        //    get { return this.imageAndText.ImageSource; }
        //    set { this.imageAndText.ImageSource = (ImageSource)value; }
        //}

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

        public bool Visible
        {
            get { return this.Available; }
            set { this.Available = value; }
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
