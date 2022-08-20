using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Documents;
using System.Windows;
using System.Windows.Controls;

namespace ZachJohnson.Promptu.WpfUI.UIComponents
{
    internal class ErrorAdorner : Adorner
    {
        private ContentPresenter contentPresenter;
 
        public ErrorAdorner(UIElement adornedElement, object content, DataTemplate template)
            : base(adornedElement)
        {
            this.contentPresenter = new ContentPresenter();
            this.contentPresenter.Content = content;
            this.contentPresenter.ContentTemplate = template;
        }

        //protected override Size MeasureOverride(Size constraint)
        //{
        //    this.contentPresenter.Measure(constraint
        //    return base.MeasureOverride(constraint);
        //}
    }
}
