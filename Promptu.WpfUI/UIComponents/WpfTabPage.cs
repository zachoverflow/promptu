using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZachJohnson.Promptu.UIModel.Interfaces;
using System.Windows;

namespace ZachJohnson.Promptu.WpfUI.UIComponents
{
    internal class WpfTabPage : System.Windows.Controls.TabItem, ITabPage
    {
        public WpfTabPage()
        {
        }

        public string Text
        {
            set { this.Header = value; }
        }

        public void SetContent(object content)
        {
            this.Content = content;
            //FrameworkElement element = content as FrameworkElement;
            //if (element != null)
            //{
            //    element.HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch;
            //    element.VerticalAlignment = System.Windows.VerticalAlignment.Stretch;
            //}
        }
    }
}
