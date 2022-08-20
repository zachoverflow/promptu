using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows;
using System.Windows.Data;

namespace ZachJohnson.Promptu.WpfUI.UIComponents
{
    [MarkupExtensionReturnType(typeof(ImageSource))]
    internal class DrawingBrushConverter : MarkupExtension, IValueConverter
    {
        //private object drawingBrush;

        public DrawingBrushConverter()
        {
        }

        //public DrawingBrushConverterExtension(object drawingBrush)
        //{
        //    this.drawingBrush = drawingBrush;
        //}

        //public object DrawingBrush
        //{
        //    get { return this.drawingBrush; }
        //    set { this.drawingBrush = value; }
        //}

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            DrawingBrush brush = value as DrawingBrush;
            if (brush == null)
            {
                return null;
            }

            DrawingImage image = new DrawingImage();
            image.Drawing = brush.Drawing;
            image.Freeze();
            return image;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
