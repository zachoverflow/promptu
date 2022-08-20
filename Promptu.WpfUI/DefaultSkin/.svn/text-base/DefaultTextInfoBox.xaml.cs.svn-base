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
using ZachJohnson.Promptu.UIModel.RichText;

namespace ZachJohnson.Promptu.WpfUI.DefaultSkin
{
	/// <summary>
	/// Interaction logic for DefaultTextInfoBox.xaml
	/// </summary>
	internal partial class DefaultTextInfoBox : Window, ITextInfoBox
	{
        private InfoType infoType;

		public DefaultTextInfoBox()
		{
			this.InitializeComponent();

            FontFamilyConverter converter = new FontFamilyConverter();
            if (converter.ConvertToInvariantString(this.FontFamily).ToUpperInvariant() == "TAHOMA")
            {
                if (WpfUtilities.SegoeUIIsInstalled)
                {
                    this.FontFamily = new FontFamily("Segoe UI");
                }
            }
			
			// Insert code required on object creation below this point.
            //WpfToolkitHost.InitializeWindow(this);

            this.label.ImageClick += this.ForwardImageClick;

            this.label.Images.Add("upArrow", new ImageSourceWithPadding(WpfToolkitImages.UpArrow, new Thickness(2)));
            this.label.Images.Add("downArrow", new ImageSourceWithPadding(WpfToolkitImages.DownArrow, new Thickness(2)));
		}

        public event EventHandler<ImageClickEventArgs> ImageClick;

        //public double ScaledFontSize
        //{
        //    get
        //    {
        //        return this.LayoutTransform.Transform(new Point(this.FontSize, 0)).X;
        //    }

        //    set
        //    {
        //        ScaleTransform transform = ((ScaleTransform)((TransformGroup)this.LayoutTransform).Children[0]);

        //        double newScale = value / this.FontSize;
        //        transform.ScaleX = newScale;
        //        transform.ScaleY = newScale;

        //        this.label
        //    }
        //}

        public InfoType InfoType
        {
            get
            {
                return this.infoType;
            }
            set
            {
                this.infoType = value;
            }
        }

        RTElement ITextInfoBox.Content
        {
            get
            {
                return this.label.DisplayValue;
            }
            set
            {
                this.label.DisplayValue = value;
            }
        }

        public List<System.Drawing.Bitmap> Images
        {
            get { return new List<System.Drawing.Bitmap>(); }
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
                //this.Measure(new Size(this.MaxWidth, Double.PositiveInfinity));
                return WpfToolkitHost.ConvertSize(this.DesiredSize);
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
            return WpfToolkitHost.ConvertSize(this.RenderSize);
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
            this.UpdateLayout();
            this.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
        }

        public void BringToFront()
        {
            this.Activate();
        }

        public object UIObject
        {
            get { return this; }
        }

        private void ForwardImageClick(object sender, ImageClickEventArgs e)
        {
            this.OnImageClick(e);
        }

        protected virtual void OnImageClick(ImageClickEventArgs e)
        {
            EventHandler<ImageClickEventArgs> handler = this.ImageClick;
            if (handler != null)
            {
                handler(this, e);
            }
        }
    }
}