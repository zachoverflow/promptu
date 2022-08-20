using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Markup;
using System.Windows.Media;

namespace ZachJohnson.Promptu.WpfUI.UIComponents
{
    [MarkupExtensionReturnType(typeof(Drawing))]
    internal class GetDrawingExtension : MarkupExtension
    {
        private DrawingBrush drawingBrush;

        public GetDrawingExtension(DrawingBrush Brush)
        {
            this.drawingBrush = Brush;
        }

        public DrawingBrush Brush
        {
            get { return this.drawingBrush; }
            set { this.drawingBrush = value; }
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this.drawingBrush == null ? null : this.drawingBrush.Drawing;
        }
    }
}
