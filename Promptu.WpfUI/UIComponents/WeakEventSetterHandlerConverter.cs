using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Markup;
using System.ComponentModel;
using System.Xaml;
using System.Windows;
using System.Reflection;

namespace ZachJohnson.Promptu.WpfUI.UIComponents
{
    internal class WeakEventSetterHandlerConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return (sourceType == typeof(string));
        }

        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            return false;
        }

        public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
        {
            IRootObjectProvider service = context.GetService(typeof(IRootObjectProvider)) as IRootObjectProvider;
            if (service != null)
            {
                IProvideValueTarget target = context.GetService(typeof(IProvideValueTarget)) as IProvideValueTarget;
                if (target != null)
                {
                    string s;
                    WeakEventSetter eventSetter = target.TargetObject as WeakEventSetter;
                    if (eventSetter != null && (s = value as string) != null)
                    {
                        s = s.Trim();

                        MethodInfo targetMethod = service.RootObject.GetType().GetMethod(s, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);

                        WeakEventWrapper wrapper = new WeakEventWrapper(
                            Delegate.CreateDelegate(eventSetter.Event.HandlerType, service.RootObject, targetMethod));

                        MethodInfo wrapperMethod = typeof(WeakEventWrapper).GetMethod("Invoke", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);

                        return Delegate.CreateDelegate(eventSetter.Event.HandlerType, wrapper, wrapperMethod);
                            
                            //Delegate.CreateDelegate(eventSetter.Event.HandlerType, service.RootObject, s);
                    }
                }
            }

            return base.ConvertFrom(context, culture, value);
        }
    }
}
