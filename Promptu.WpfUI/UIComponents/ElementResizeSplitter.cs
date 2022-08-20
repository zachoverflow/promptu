using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Shapes;
using System.Windows;
using System.Windows.Input;

namespace ZachJohnson.Promptu.WpfUI.UIComponents
{
    internal class ElementResizeSplitter : ContentControl
    {
        public static readonly DependencyProperty QuantityToChangeProperty =
            DependencyProperty.Register(
                "QuantityToChange",
                typeof(double),
                typeof(ElementResizeSplitter));

        private bool mouseIsCaptured;
        private Point lastMouseLocation;
        private Orientation orientation = Orientation.Vertical;

        public ElementResizeSplitter()
        {
        }

        public double QuantityToChange
        {
            get { return (double)this.GetValue(QuantityToChangeProperty); }
            set { this.SetValue(QuantityToChangeProperty, value); }
        }

        public Orientation Orientation
        {
            get { return this.orientation; }
            set { this.orientation = value; }
        }

        protected override void OnPreviewMouseDown(System.Windows.Input.MouseButtonEventArgs e)
        {
            this.lastMouseLocation = e.GetPosition(this);
            this.mouseIsCaptured = true;
            Mouse.Capture(this);

            base.OnPreviewMouseDown(e);
        }

        protected override void OnMouseUp(MouseButtonEventArgs e)
        {
            this.mouseIsCaptured = false;
            Mouse.Capture(null);
            base.OnMouseUp(e);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            if (this.mouseIsCaptured)
            {
                Point position = e.GetPosition(this);
                if (this.orientation == System.Windows.Controls.Orientation.Vertical)
                {
                    this.QuantityToChange -= position.Y - this.lastMouseLocation.Y;
                }
                else
                {
                    this.QuantityToChange += position.X - this.lastMouseLocation.X;
                }
            }
        }
    }
}
