//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Windows.Controls;
//using System.Windows;
//using System.Windows.Data;

//namespace ZachJohnson.Promptu.WpfUI.UIComponents
//{
//    internal class OptionsLabel : Label
//    {
//        public static readonly DependencyProperty SimplifyingGroupNameProperty =
//            DependencyProperty.Register(
//                "SimplifyingGroupName",
//                typeof(string),
//                typeof(OptionsLabel),
//                new PropertyMetadata(HandleSimplifyingGroupNamePropertyChanged));

//        public OptionsLabel()
//        {
//        }

//        public string SimplifyingGroupName
//        {
//            get { return (string)this.GetValue(SimplifyingGroupNameProperty); }
//            set { this.SetValue(SimplifyingGroupNameProperty, value); }
//        }

//        private static void HandleSimplifyingGroupNamePropertyChanged(
//            DependencyObject obj, 
//            DependencyPropertyChangedEventArgs e)
//        {
//            OptionsLabel label = obj as OptionsLabel;
//            if (label == null)
//            {
//                return;
//            }

//            OptionsSimplifyingManager simplifyingManager = label.TryFindResource("simplifyingManager") as OptionsSimplifyingManager;

//            if (simplifyingManager == null)
//            {
//                return;
//            }

//            if (e.OldValue != null)
//            {
//                OptionsSimplifyingContext oldContext = simplifyingManager.TryGet((string)e.OldValue);

//                if (oldContext != null)
//                {
//                    oldContext.RemoveElement(label);
//                    label.SetBinding(WidthProperty, (Binding)null);
//                }
//            }

//            if (e.NewValue != null)
//            {
//                OptionsSimplifyingContext newContext = simplifyingManager[(string)e.NewValue];
//                newContext.AddElement(label);

//                Binding binding = new Binding("Width");
//                binding.Source = newContext;

//                label.SetBinding(MinWidthProperty, binding);
//            }
//        }
//    }
//}
