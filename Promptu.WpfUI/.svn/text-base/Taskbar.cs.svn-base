using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Drawing.Extensions;

namespace ZachJohnson.Promptu.WpfUI
{
    internal class Taskbar
    {
        private const string className = "Shell_TrayWnd";
        private Rectangle bounds;
        private TaskbarPosition position;
        private bool alwaysOnTop;
        private bool autoHide;

        public enum TaskbarPosition
        {
            Unknown = -1,
            Left,
            Top,
            Right,
            Bottom,
        }

        public Taskbar()
        {
            IntPtr taskbarHandle = NativeMethods.FindWindow(className, null);

            if (taskbarHandle == IntPtr.Zero)
            {
                throw new NotSupportedException("Could not locate the taskbar window.");
            }

            NativeMethods.APPBARDATA data = NativeMethods.APPBARDATA.NewAPPBARDATA();
            data.hWnd = taskbarHandle;

            IntPtr result = NativeMethods.SHAppBarMessage((uint)NativeMethods.ABM.GetTaskbarPos, ref data);

            if (result == IntPtr.Zero)
            {
                throw new NotSupportedException("Unable to get information about the taskbar.");
            }

            this.position = (TaskbarPosition)data.uEdge;
            this.bounds = Rectangle.FromLTRB(data.rc.Left, data.rc.Top, data.rc.Right, data.rc.Bottom);

            data.cbSize = Marshal.SizeOf(typeof(NativeMethods.APPBARDATA));
            result = NativeMethods.SHAppBarMessage((uint)NativeMethods.ABM.GetState, ref data);
            int state = result.ToInt32();
            this.alwaysOnTop = (state & ABS.AlwaysOnTop) == ABS.AlwaysOnTop;
            this.autoHide = (state & ABS.Autohide) == ABS.Autohide;
        }

        public Rectangle Bounds
        {
            get { return this.bounds; }
        }

        public TaskbarPosition Position
        {
            get { return this.position; }
        }

        public Point Location
        {
            get { return this.bounds.Location; }
        }

        public Size Size
        {
            get { return this.bounds.Size; }
        }

        public bool AutoHide
        {
            get { return this.autoHide; }
        }

        public bool AlwaysOnTop
        {
            get { return this.alwaysOnTop; }
        }

        public static class ABS
        {
            public const int Autohide = 0x0000001;
            public const int AlwaysOnTop = 0x0000002;
        }

        public static Rectangle RemoveBoundsFromScreenIfNecessary(Rectangle screenBounds)
        {
            try
            {
                Taskbar taskbar = new Taskbar();
                if (taskbar.AutoHide && taskbar.Position != TaskbarPosition.Unknown)
                {
                    if (screenBounds.IntersectsWithOrContains(screenBounds))
                    {
                        Rectangle realScreenBounds = screenBounds;

                        switch (taskbar.Position)
                        {
                            case TaskbarPosition.Bottom:
                                realScreenBounds.Height -= taskbar.Size.Height;
                                break;
                            case TaskbarPosition.Right:
                                realScreenBounds.Width -= taskbar.Size.Width;
                                break;
                            case TaskbarPosition.Left:
                                realScreenBounds.Width -= taskbar.Size.Width;
                                realScreenBounds.X += taskbar.Size.Width;
                                break;
                            case TaskbarPosition.Top:
                                realScreenBounds.Height -= taskbar.Size.Height;
                                realScreenBounds.Y += taskbar.Size.Height;
                                break;
                        }

                        return realScreenBounds;
                    }
                }
            }
            catch (NotSupportedException)
            {
            }

            return screenBounds;
        }
    }
}
