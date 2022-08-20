//using System;
//using System.Collections.Generic;
//using System.Text;
//using System.Windows.Forms;

//namespace ZachJohnson.Promptu.Skins
//{
//    internal class WindowManager
//    {
//        //private const int WM_ACTIVATE = 0x0006;
//        //private const int WA_INACTIVE = 0;
//        //private const int WA_ACTIVE = 1;
//        //private const int WA_CLICKACTIVE = 2;

//        //private Dictionary<IntPtr, WindowInterceptBase> windows = new Dictionary<IntPtr, WindowInterceptBase>();

//        //public WindowManager()
//        //{
//        //}

//        //public void RegisterWindow(IntPtr handle)
//        //{
//        //    this.RegisterWindow<object>(handle, null, null);
//        //}

//        //public void RegisterWindow<T>(IntPtr handle, T obj, Action<T> actionWhenActivated)
//        //{
//        //    windows.Add(handle, new WindowIntercept<T>(handle, this, obj, actionWhenActivated));
//        //}

//        //public void UnregisterWindow(IntPtr handle)
//        //{
//        //        WindowInterceptBase windowIntercept = this.windows[handle];
//        //        windowIntercept.ReleaseHandle();
//        //        windows.Remove(handle);
//        //}

//        //public event EventHandler ActivationLost;

//        //protected virtual void OnActivationLost(EventArgs e)
//        //{
//        //    EventHandler handler = this.ActivationLost;
//        //    if (handler != null)
//        //    {
//        //        handler(this, e);
//        //    }
//        //}

//        //private class WindowIntercept<T> : WindowInterceptBase
//        //{
//        //    private T obj;
//        //    private Action<T> actionWhenActivated;
//        //    private WindowManager manager;

//        //    public WindowIntercept(IntPtr handle, WindowManager manager, T obj, Action<T> actionWhenActivated)
//        //        : base(handle)
//        //    {
//        //        this.actionWhenActivated = actionWhenActivated;
//        //        this.manager = manager;
//        //        this.obj = obj;
//        //    }

//        //    protected override void WndProc(ref Message m)
//        //    {
//        //        if (m.Msg == WM_ACTIVATE)
//        //        {
//        //            switch ((int)m.WParam)
//        //            {
//        //                case WA_INACTIVE:
//        //                    IntPtr windowFocusGoingTo = m.LParam;
//        //                    if (!this.manager.windows.ContainsKey(windowFocusGoingTo))
//        //                    {
//        //                        this.manager.OnActivationLost(EventArgs.Empty);
//        //                    }

//        //                    break;
//        //                case WA_CLICKACTIVE:
//        //                case WA_ACTIVE:
//        //                    if (this.actionWhenActivated != null)
//        //                    {
//        //                        this.actionWhenActivated(this.obj);
//        //                    }

//        //                    break;
//        //                default:
//        //                    break;
//        //            }
//        //        }

//        //        base.WndProc(ref m);
//        //    }
//        //}

//        //private class WindowInterceptBase : NativeWindow
//        //{
//        //    public WindowInterceptBase(IntPtr handle)
//        //    {
//        //        this.AssignHandle(handle);
//        //    }
//        //}
//    }
//}
