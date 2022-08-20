using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Documents;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Media;
using System.Windows.Data;

namespace ZachJohnson.Promptu.WpfUI.UIComponents
{
    internal class TextBoxCueAdorner : Adorner
    {
        private TextBox cueTextBox;
        private ContentPresenter contentPresenter;

        public TextBoxCueAdorner(TextBox adornedElement, string cue)
            : base(adornedElement)
        {
            this.IsHitTestVisible = false;

            this.contentPresenter = new ContentPresenter();

            this.cueTextBox = new TextBox();
            this.cueTextBox.Style = (Style)this.FindResource("CueTextBoxStyle");
            this.cueTextBox.DataContext = adornedElement;
            this.cueTextBox.Text = cue;

            //Binding heightBinding = new Binding("Height");
            //heightBinding.Source = this.cueTextBox;

            //Binding widthBinding = new Binding("ActualWidth");
            //widthBinding.Source = this.cueTextBox;

            //adornedElement.SetBinding(TextBlock.HeightProperty, heightBinding);
            //adornedElement.SetBinding(TextBlock.WidthProperty, widthBinding);

            this.contentPresenter.Content = this.cueTextBox;
        }

        protected override System.Windows.Media.Visual GetVisualChild(int index)
        {
            return this.contentPresenter;
        }

        protected override int VisualChildrenCount
        {
            get
            {
                return 1;
            }
        }

        protected override Size MeasureOverride(Size constraint)
        {
            this.contentPresenter.Measure(this.AdornedElement.RenderSize);
            return this.AdornedElement.RenderSize;
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            this.contentPresenter.Arrange(new Rect(finalSize));
            return finalSize;
        }
    }
}
