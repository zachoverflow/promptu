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
    /// Interaction logic for WpfOptionsProgressBar.xaml
    /// </summary>
    internal partial class WpfOptionsProgressBar : UserControl, IOptionsProgressBar
    {
        public WpfOptionsProgressBar()
        {
            InitializeComponent();
        }

        public string Text
        {
            get
            {
                return this.label.Text;
            }
            set
            {
                this.label.Text = value;
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


        public bool IsIndeterminate
        {
            get
            {
                return this.progressBar.IsIndeterminate;
            }
            set
            {
                this.progressBar.IsIndeterminate = value;
            }
        }

        public double PercentComplete
        {
            get
            {
                return this.progressBar.Value;
            }
            set
            {
                this.progressBar.Value = value;
            }
        }

        public void ReportProgress(double value)
        {
            if (!this.Dispatcher.CheckAccess())
            {
                this.Dispatcher.Invoke(new Action<double>(this.ReportProgress), value);
            }
            else
            {
                this.IsIndeterminate = false;
                this.PercentComplete = value;
            }
        }
    }
}
