//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Windows.Markup;
//using System.Windows.Data;
//using System.Windows;

//namespace ZachJohnson.Promptu.WpfUI.UIComponents
//{
//    internal class ImplicitConverterBinding : MarkupExtension
//    {
//        public ImplicitConverterBinding()
//        {
//        }

//        public ImplicitConverterBinding(string path)
//        {
//            this.Path = path;
//        }

//        public override object ProvideValue(IServiceProvider serviceProvider)
//        {
//            IProvideValueTarget valueProvider = serviceProvider.GetService(typeof(IProvideValueTarget)) as IProvideValueTarget;

//            if (valueProvider != null)
//            {
//                DependencyObject targetObject = valueProvider.TargetObject as DependencyObject;
//                DependencyProperty property = valueProvider.TargetProperty as DependencyProperty;

//                if (targetObject == null)
//                {
//                    throw new NotSupportedException("The target object is not a DependencyObject.");
//                }
//                else if (property == null)
//                {
//                    throw new NotSupportedException("The target property is not a DependencyProperty.");
//                }

//                Binding binding = new Binding(this.Path);
//                binding.Mode = this.Mode;
//                binding.UpdateSourceTrigger = this.UpdateSourceTrigger;

//                Binding converterBinding = new Binding("DataContext.TypeConverter");
//                converterBinding.Source = targetObject;

//                TypeConverterValueConverter typeConverterConverter = new TypeConverterValueConverter();
//                BindingOperations.SetBinding(typeConverterConverter, TypeConverterValueConverter.TypeConverterProperty, converterBinding);

//                binding.Converter = typeConverterConverter;

//                BindingOperations.SetBinding(targetObject, property, binding);
//            }

//            return null;
//        }

//        public BindingMode Mode { get; set; }

//        public UpdateSourceTrigger UpdateSourceTrigger { get; set; }

//        [ConstructorArgument("path")]
//        public string Path { get; set; }
//    }
//}
