using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using ZachJohnson.Promptu.UIModel.Interfaces;
using System.Windows;
using System.Runtime.CompilerServices;
using ZachJohnson.Promptu.UIModel;
using System.Windows.Media;

namespace ZachJohnson.Promptu.WpfUI.UIComponents
{
    internal class WpfMenuItem : MenuItem , IToolbarMenuItem, IMenuItem
    {
        private NativeUICollectionBinding<IGenericMenuItem> itemsBinding;
        private EventHandler click;

        public WpfMenuItem()
        {
            this.itemsBinding = new NativeUICollectionBinding<IGenericMenuItem>(
                new NativeUICollectionBinding<IGenericMenuItem>.InsertMethod(this.InsertIntoItems),
                new NativeUICollectionBinding<IGenericMenuItem>.ClearMethod(this.ClearItems),
                new NativeUICollectionBinding<IGenericMenuItem>.RemoveMethod(this.RemoveItem));
        }

        event EventHandler IToolbarMenuItem.Click
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

        event EventHandler IMenuItem.Click
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
            get { return (string)this.Header; }
            set { this.Header = value; }
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


        public UIModel.TextStyle TextStyle
        {
            set { WpfToolkitHost.ApplyTextStyleTo(this, value); }
        }

        public object Image
        {
            set 
            {
                ImageSource source = value as ImageSource;

                if (source != null)
                {
                    Image image = new Image();

                    image.Source = source;
                    this.Icon = image;
                }
                else
                {
                    this.Icon = value;
                }
            }
        }

        public INativeUICollection<IGenericMenuItem> SubItems
        {
            get { return this.itemsBinding; }
        }

        private void InsertIntoItems(int index, IGenericMenuItem item)
        {
            this.Items.Insert(index, item);
        }

        private void ClearItems()
        {
            this.Items.Clear();
        }

        private void RemoveItem(IGenericMenuItem item)
        {
            this.Items.Remove(item);
        }
    }
}
