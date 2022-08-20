using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ZachJohnson.Promptu.UIModel.Interfaces;
using System.Globalization;

namespace ZachJohnson.Promptu.WpfUI.UIComponents
{
    /// <summary>
    /// Interaction logic for WpfOptionButton.xaml
    /// </summary>
    internal partial class WpfOptionButton : UserControl, IOptionButton
    {
        public WpfOptionButton()
        {
            InitializeComponent();
            this.button.Click += this.HandleButtonClick;
        }

        public string Text
        {
            get { return (string)this.button.Content; }
            set { this.button.Content = value; }
        }

        public bool Enabled
        {
            get { return this.button.IsEnabled; }
            set { this.button.IsEnabled = value; }
        }

        public event EventHandler Click;

        public string ToolTipText
        {
            get { return (string)this.button.ToolTip; }
            set { this.button.ToolTip = value; }
        }

        private void HandleButtonClick(object sender, EventArgs e)
        {
            this.OnClick(EventArgs.Empty);
        }

        protected virtual void OnClick(EventArgs e)
        {
            EventHandler handler = this.Click;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        public bool Visible
        {
            get
            {
                return this.Visibility == System.Windows.Visibility.Visible;
            }
            set
            {
                this.Visibility = value ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        public int Indent
        {
            set
            {
                this.Margin = (Thickness)new IndentConverter().Convert(value, typeof(Thickness), null, CultureInfo.InvariantCulture);
            }
        }
    }
}
