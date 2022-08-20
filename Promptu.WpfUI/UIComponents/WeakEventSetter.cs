using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.ComponentModel;
using System.Windows.Markup;

namespace ZachJohnson.Promptu.WpfUI.UIComponents
{
    internal class WeakEventSetter : EventSetter
    {
        //private WeakEventWrapper
        public WeakEventSetter()
        {
        }

        [TypeConverter(typeof(WeakEventSetterHandlerConverter))]
        public new Delegate Handler
        {
            get
            {
                return base.Handler;
            }

            set
            {
                base.CheckSealed();
                //WeakEventWrapper wrapper = new WeakEventWrapper(value);
                base.Handler = value;//new RoutedEventHandler(wrapper.Invoke);
            }
        }
    }
}
