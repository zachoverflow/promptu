//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Windows;

//namespace ZachJohnson.Promptu.WpfUI.UIComponents
//{
//    internal class OptionsSimplifyingContext : DependencyObject
//    {
//        public static readonly DependencyProperty WidthProperty =
//            DependencyProperty.Register(
//                "Width",
//                typeof(double),
//                typeof(OptionsSimplifyingContext),
//                new PropertyMetadata(0.0d, null, CoerceWidth));

//        private List<WeakReference<FrameworkElement>> elements = new List<WeakReference<FrameworkElement>>();

//        private 

//        public OptionsSimplifyingContext()
//        {
//        }

//        public double Width
//        {
//            get { return (double)this.GetValue(WidthProperty); }
//            set { this.SetValue(WidthProperty, value); }
//        }

//        public void AddElement(FrameworkElement element)
//        {
//            lock (this.elements)
//            {
//                this.elements.Add(new WeakReference<FrameworkElement>(element));
//            }
//        }

//        public void RemoveElement(FrameworkElement element)
//        {
//            lock (this.elements)
//            {
//                List<WeakReference<FrameworkElement>> elementsToRemove = new List<WeakReference<FrameworkElement>>();

//                foreach (var item in this.elements)
//                {
//                    if (item.Target == element)
//                    {
//                        elementsToRemove.Add(item);
//                    }
//                }

//                foreach (var item in elementsToRemove)
//                {
//                    this.elements.Remove(item);
//                }
//            }
//        }

//        private static object CoerceWidth(DependencyObject obj, object value)
//        {
//            OptionsSimplifyingContext context = obj as OptionsSimplifyingContext;
//            if (context == null)
//            {
//                return value;
//            }

//            double greatestWidth = 0;

//            foreach (var elementReference in context.elements)
//            {
//                FrameworkElement element = elementReference.Target;
//                if (element == null)
//                {
//                    continue;
//                }

//                if (!element.IsMeasureValid)
//                {
//                    element.Measure(new Size(Double.PositiveInfinity, Double.PositiveInfinity));
//                }

//                double width = element.ActualWidth;
//                if (width > greatestWidth)
//                {
//                    greatestWidth = width;
//                }
//            }

//            return greatestWidth;
//        }
//    }
//}
