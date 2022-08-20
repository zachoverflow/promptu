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
using System.Windows.Shapes;
using ZachJohnson.Promptu.SkinApi;
using System.Windows.Threading;

namespace ZachJohnson.Promptu.WpfUI.DefaultSkin
{
    /// <summary>
    /// Interaction logic for DefaultProgressInfoBox.xaml
    /// </summary>
    internal partial class DefaultProgressInfoBox : Window, IProgressInfoBox
    {
        public DefaultProgressInfoBox()
        {
            InitializeComponent();
        }

        public string Text
        {
            set { this.label.Text = value; }
        }

        public int Mininum
        {
            set
            {
                this.progressBar.Minimum = value;
            }
        }

        public int Maximum
        {
            set
            {
                this.progressBar.Maximum = value;
            }
        }

        public int Value
        {
            get
            {
                return (int)this.progressBar.Value;
            }
            set
            {
                this.progressBar.Value = value;
            }
        }


        public bool Visible
        {
            get { return this.IsVisible; }
        }

        public System.Drawing.Size Size
        {
            get
            {
                return WpfToolkitHost.ConvertSize(new Size(this.Width, this.Height));
            }
            set
            {
                Size converted = WpfToolkitHost.ConvertSize(value);

                //this.Height = converted.Height;
                //this.Width = converted.Width;
            }
        }

        public System.Drawing.Size ActualSize
        {
            get
            {
                return WpfToolkitHost.ConvertSize(new Size(this.ActualWidth, this.ActualHeight));
            }
        }

        int? IInfoBox.MaxWidth
        {
            get
            {
                double maxWidth = this.MaxWidth;
                return Double.IsInfinity(maxWidth) ? null : (int?)WpfToolkitHost.ConvertToPhysicalPixels(maxWidth);
            }
            set
            {
                if (value == null)
                {
                    this.MaxWidth = double.PositiveInfinity;
                }
                else
                {
                    this.MaxWidth = WpfToolkitHost.ConvertToDeviceIndependentPixels(value.Value);
                }
            }
        }

        public System.Drawing.Point Location
        {
            get
            {
                return WpfToolkitHost.ConvertPoint(
                    new Point(this.Left, this.Top));
            }
            set
            {
                Point location = WpfToolkitHost.ConvertPoint(value);
                this.Left = location.X;
                this.Top = location.Y;
            }
        }

        public System.Drawing.Size GetPreferredSize(System.Drawing.Size proposedSize)
        {
            return this.Size;
        }

        public bool TopMost
        {
            get
            {
                return this.Topmost;
            }
            set
            {
                this.Topmost = value;
            }
        }

        public void Refresh()
        {
            this.Dispatcher.Invoke(new Action(delegate { }), DispatcherPriority.Background);
            //this.Dispatcher.Invoke(DispatcherPriority.Render, new Action(delegate() { System.Threading.Thread.Sleep(10); }));
            //DispatcherFrame frame = new DispatcherFrame();

            //Dispatcher.CurrentDispatcher.BeginInvoke(DispatcherPriority.Render, new DispatcherOperationCallback(delegate(object parameter)
            //{

            //    frame.Continue = false;

            //    return null;

            //}), null);

            //Dispatcher.PushFrame(frame);
        }

        public void BringToFront()
        {
            this.Activate();
        }

        public object UIObject
        {
            get { return this; }
        }
    }
}
