using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows;

namespace ZachJohnson.Promptu.WpfUI.UIComponents
{
    [MarkupExtensionReturnType(typeof(ImageSource))]
    internal class DrawingBrushConverterExtension : MarkupExtension
    {
        private object drawingBrush;

        public DrawingBrushConverterExtension()
        {
        }

        public DrawingBrushConverterExtension(object drawingBrush)
        {
            this.drawingBrush = drawingBrush;
        }

        public object DrawingBrush
        {
            get { return this.drawingBrush; }
            set { this.drawingBrush = value; }
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            if (this.drawingBrush == null)
            {
                return null;
            }

            DrawingImage image = new DrawingImage();
            image.Drawing = ((DrawingBrush)this.drawingBrush).Drawing;
            image.Freeze();
            return image;
        }
    }
}
