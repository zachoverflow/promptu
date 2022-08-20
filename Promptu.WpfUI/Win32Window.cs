//-----------------------------------------------------------------------
// <copyright file="Window.cs" company="ZachJohnson">
//     Copyright (c) Zach Johnson. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace ZachJohnson.Promptu.WpfUI
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Diagnostics;
    using System.Drawing;
    using System.Windows.Forms;

    internal class Win32Window : IWin32Window
    {
        private const int WM_GETTEXT = 0x000D;
        private const int SMTO_ABORTIFHUNG = 0x0002;
        private const int WM_GETTEXTLENGTH = 0x000E;
        private IntPtr handle;

        public Win32Window(IntPtr handle)
        {
            this.handle = handle;
        }

        public IntPtr Handle
        {
            get { return this.handle; }
        }

        public Win32Window GetParent()
        {
            IntPtr parent = NativeMethods.GetAncestor(this.handle, 1);
            return new Win32Window(parent);
        }

        public static Win32Window FromPoint(Point point)
        {
            return new Win32Window(NativeMethods.WindowFromPoint(point));
        }

        public string Text
        {
            get
            {
                IntPtr result;
                StringBuilder text = new StringBuilder();
                if (NativeMethods.SendMessageTimeout(this.handle, WM_GETTEXTLENGTH, IntPtr.Zero, IntPtr.Zero, SMTO_ABORTIFHUNG, 2000, out result) != IntPtr.Zero)
                {
                    text.Capacity = result.ToInt32() + 1;
                    if (NativeMethods.SendMessageTimeout(this.handle, WM_GETTEXT, (IntPtr)text.Capacity, text, SMTO_ABORTIFHUNG, 2000, out result) != IntPtr.Zero)
                    {
                        return text.ToString();
                    }
                }

                return string.Empty;
                //StringBuilder text = new StringBuilder();
                //text.Capacity = NativeMethods.GetWindowTextLength(this.handle) + 1;
                //NativeMethods.GetWindowText(this.handle, text, text.Capacity);
                //return text.ToString();
            }
        }

        public string ClassName
        {
            get
            {
                StringBuilder className = new StringBuilder();
                className.Capacity = 100;
                NativeMethods.GetClassName(this.handle, className, className.Capacity + 1);
                return className.ToString().TrimEnd();
            }
        }

        //public Graphics GetGraphics()
        //{
        //    return Graphics.FromHdc(NativeMethods.GetDC(this.handle));
        //}

        //public Rectangle GetArea()
        //{
        //    NativeMethods.RECT rectangle;
        //    NativeMethods.GetWindowRect(this.handle, out rectangle);
        //    return new Rectangle(rectangle.Left, rectangle.Top, rectangle.Right - rectangle.Left, rectangle.Bottom - rectangle.Top);
        //}

        public Process GetProcess()
        {
            IntPtr processId = IntPtr.Zero;
            NativeMethods.GetWindowThreadProcessId(this.handle, out processId);

            return Process.GetProcessById(processId.ToInt32());
        }

        public Win32Window GetTopLevelParent()
        {
            Win32Window parent = this.GetParent();
            Process process = this.GetProcess();
            while (true)
            {
                Win32Window nextParent = parent.GetParent();
                Process parentProcess = parent.GetProcess();
                if (parentProcess.ProcessName == process.ProcessName)
                {
                    parent = nextParent;
                }
                else
                {
                    break;
                }
            }

            return parent;
        }

        public List<Win32Window> ChildWindows
        {
            get
            {
                List<Win32Window> childWindows = new List<Win32Window>();
                NativeMethods.EnumChildWindows(this.handle, new NativeMethods.EnumWindowsCallback(
                    delegate(IntPtr hWnd, int lParam)
                    {
                        childWindows.Add(new Win32Window(hWnd));
                        return true;
                    }), IntPtr.Zero);

                return childWindows;
            }
        }

        public override string ToString()
        {
            return this.ClassName;
        }
    }
}
