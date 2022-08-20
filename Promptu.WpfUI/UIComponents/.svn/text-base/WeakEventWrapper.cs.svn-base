using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace ZachJohnson.Promptu.WpfUI.UIComponents
{
    internal class WeakEventWrapper
    {
        private WeakReference<Delegate> target;

        public WeakEventWrapper(Delegate target)
        {
            this.target = new WeakReference<Delegate>(target);
        }

        public void Invoke(object sender, RoutedEventArgs e)
        {
            Delegate d = this.target.Target;
            if (d != null)
            {
                d.DynamicInvoke(sender, e);
            }
        }
    }
}
