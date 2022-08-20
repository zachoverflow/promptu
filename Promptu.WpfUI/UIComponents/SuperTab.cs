using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using ZachJohnson.Promptu.UIModel.Interfaces;

namespace ZachJohnson.Promptu.WpfUI.UIComponents
{
    internal class SuperTab : DependencyObject, ISuperTabPage
    {
        public static readonly DependencyProperty HeaderProperty =
            DependencyProperty.Register(
                "Header",
                typeof(object),
                typeof(SuperTab));

        public static readonly DependencyProperty ContentProperty =
            DependencyProperty.Register(
                "Content",
                typeof(object),
                typeof(SuperTab));

        public static readonly DependencyProperty ImageProperty =
            DependencyProperty.Register(
                "Image",
                typeof(object),
                typeof(SuperTab));

        public SuperTab()
        {
        }

        public object Header
        {
            get { return this.GetValue(HeaderProperty); }
            set { this.SetValue(HeaderProperty, value); }
        }

        public object Content
        {
            get { return this.GetValue(ContentProperty); }
            set { this.SetValue(ContentProperty, value); }
        }

        public string Text
        {
            set { this.Header = value; }
        }

        public object Image
        {
            get { return this.GetValue(ImageProperty); }
            set { this.SetValue(ImageProperty, value); }
        }

        public void SetContent(object content)
        {
            this.Content = content;
        }
    }
}
