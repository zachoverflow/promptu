using System;
using System.Collections.Generic;
using System.Text;

namespace ZachJohnson.Promptu.UIModel
{
    internal abstract class WindowManager
    {
        public WindowManager()
        {
        }

        public event EventHandler ActivationLost;

        public void TryRegister(object window, bool doNoCloseWhenUnregistered)
        {
            this.TryRegister<object>(window, null, null, doNoCloseWhenUnregistered);
        }

        public void TryRegister<T>(object window, T obj, Action<T> onActivate, bool doNotCloseWhenUnregistered)
        {
            this.TryRegisterCore(window, obj, onActivate, doNotCloseWhenUnregistered);
        }

        public void Unregister(object window)
        {
            this.UnregisterCore(window);
        }

        protected abstract void TryRegisterCore<T>(object window, T obj, Action<T> onActivate, bool doNotCloseWhenUnregistered);

        protected abstract void UnregisterCore(object window);

        protected virtual void OnActivationLost(EventArgs e)
        {
            EventHandler handler = this.ActivationLost;
            if (handler != null)
            {
                handler(this, e);
            }
        }
    }
}
