using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZachJohnson.Promptu.UIModel;
using System.Windows.Media;
using System.Windows.Interop;
using ZachJohnson.Promptu.SkinApi;
using System.Windows;
using System.Windows.Forms;

namespace ZachJohnson.Promptu.WpfUI
{
    internal class WpfWindowManager : WindowManager
    {
        private const int WM_ACTIVATE = 0x0006;
        private const int WA_INACTIVE = 0;
        private const int WA_ACTIVE = 1;
        private const int WA_CLICKACTIVE = 2;

        private Dictionary<IntPtr, WindowInterceptBase> windows = new Dictionary<IntPtr, WindowInterceptBase>();

        public WpfWindowManager()
        {
        }

        protected override void TryRegisterCore<T>(object window, T obj, Action<T> onActivate, bool doNotCloseWhenUnregistered)
        {
            IntPtr handle = GetHandle(window);
            WindowInterceptBase intercept;
            intercept = new WindowIntercept<T>(handle, doNotCloseWhenUnregistered, this, obj, onActivate);
            intercept.ApplyHandle();
#if !NO_WPF || WPF_MINIMAL
            windows.Add(handle, intercept);
#endif
        }

        protected override void UnregisterCore(object window)
        {
            IntPtr handle = GetHandle(window);
            WindowInterceptBase windowIntercept;

            if (!this.windows.TryGetValue(handle, out windowIntercept))
            {
                return;
            }

            windowIntercept.ReleaseHandle();
            windows.Remove(handle);
            if (!windowIntercept.DoNotCloseWhenUnregistered)
            {
                ((Window)window).Close();
            }   
        }

        private static IntPtr GetHandle(object window)
        {
//#if NO_WPF
//            return IntPtr.Zero;
//#endif
            IUIElement uiElement = window as IUIElement;
            if (uiElement != null)
            {
                window = uiElement.UIObject;
            }

            Window cast = window as Window;
            if (cast == null)
            {
#if NO_WPF
                return IntPtr.Zero;
#endif
                throw new ArgumentException("'window' does not derive from System.Windows.Window.");
            }

            HwndSource source = (HwndSource)PresentationSource.FromVisual(cast);
            //WindowInteropHelper helper = new WindowInteropHelper(cast);
            //return new WindowInteropHelper(cast).Handle;

            if (source == null)
            {
                WpfToolkitHost.InitializeWindow(cast);
                source = (HwndSource)PresentationSource.FromVisual(cast);
                //helper = new WindowInteropHelper(cast);
            }

            //if (helper.Handle == IntPtr.Zero)
            //{
            //    throw new ArgumentException("Missing window handle.");
            //}

            return source.Handle;
        }

        private class WindowIntercept<T> : WindowInterceptBase
        {
            private T obj;
            private Action<T> actionWhenActivated;
            private WpfWindowManager manager;

            public WindowIntercept(
                IntPtr handle, 
                bool doNotCloseWhenUnregistered, 
                WpfWindowManager manager, 
                T obj, 
                Action<T> actionWhenActivated)
                : base(handle, doNotCloseWhenUnregistered)
            {
                this.actionWhenActivated = actionWhenActivated;
                this.manager = manager;
                this.obj = obj;
            }

            protected override void WndProc(ref Message m)
            {
                if (m.Msg == WM_ACTIVATE)
                {
                    switch ((int)m.WParam)
                    {
                        case WA_INACTIVE:
                            IntPtr windowFocusGoingTo = m.LParam;
                            if (!this.manager.windows.ContainsKey(windowFocusGoingTo))
                            {
                                this.manager.OnActivationLost(EventArgs.Empty);
                            }

                            break;
                        case WA_CLICKACTIVE:
                        case WA_ACTIVE:
                            if (this.actionWhenActivated != null)
                            {
                                this.actionWhenActivated(this.obj);
                            }

                            break;
                        default:
                            break;
                    }
                }

                base.WndProc(ref m);
            }
        }

        private class WindowInterceptBase : NativeWindow
        {
            private bool doNotCloseWhenUnregistered;
            private IntPtr handle;

            public WindowInterceptBase(IntPtr handle, bool doNotCloseWhenUnregistered)
            {
                this.handle = handle;
                this.doNotCloseWhenUnregistered = doNotCloseWhenUnregistered;
            }

            public void ApplyHandle()
            {
                try
                {
                    this.AssignHandle(this.handle);
                }
                catch (NullReferenceException ex)
                {
                    //this.ReleaseHandle();
                    //this.AssignHandle(this.handle);
#if DEBUG
                    System.Diagnostics.Debug.WriteLine("NullReferenceException assigning handle");
#endif
                    //this.ReleaseHandle();
                    throw new OopsTryThatAgainException("Error assigning the window handle.", ex);
                }
            }

            public bool DoNotCloseWhenUnregistered
            {
                get { return this.doNotCloseWhenUnregistered; }
            }
        }
    }
}
