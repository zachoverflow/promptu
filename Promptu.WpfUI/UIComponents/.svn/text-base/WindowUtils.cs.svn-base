using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Interop;

namespace ZachJohnson.Promptu.WpfUI.UIComponents
{
    public static class WindowUtils
    {
        private const int GWL_STYLE = -16;
        private const int GWL_EXSTYLE = -20;
        private const int WS_MAXIMIZEBOX = 0x00010000;
        private const int WS_MINIMIZEBOX = 0x00020000;
        private const int WS_EX_DLGMODALFRAME = 0x0001;
        private const int SWP_NOSIZE = 0x0001;
        private const int SWP_NOMOVE = 0x0002;
        private const int SWP_NOZORDER = 0x0004;
        private const int SWP_FRAMECHANGED = 0x0020;

        public static readonly DependencyProperty CanMaximizeProperty =
            DependencyProperty.RegisterAttached(
            "CanMaximize",
            typeof(bool),
            typeof(WindowUtils),
            new PropertyMetadata(true, CanMaximizePropertyChanged));

        public static readonly DependencyProperty CanMinimizeProperty =
            DependencyProperty.RegisterAttached(
            "CanMinimize",
            typeof(bool),
            typeof(WindowUtils),
            new PropertyMetadata(true, CanMinimizePropertyChanged));

        public static readonly DependencyProperty ShowIconProperty =
            DependencyProperty.RegisterAttached(
            "ShowIcon",
            typeof(bool),
            typeof(WindowUtils),
            new PropertyMetadata(true, ShowIconPropertyChanged));

        public readonly static DependencyProperty CloseOnEscapeProperty = DependencyProperty.RegisterAttached(
                "CloseOnEscape",
                typeof(bool),
                typeof(WindowUtils),
                new FrameworkPropertyMetadata(HandleCloseOnEscapeChanged));

        private static void CanMaximizePropertyChanged(
            DependencyObject obj, 
            DependencyPropertyChangedEventArgs e)
        {
            Window window = obj as Window;

            if (window == null)
            {
                return;
            }

            RoutedEventHandler loadedHandler = null;

            loadedHandler = delegate
             {
                 SetMaximizeEnabled(window, (bool)e.NewValue);
                 window.Loaded -= loadedHandler;
             };

            if (window.IsLoaded)
            {
                loadedHandler(null, null);
            }
            else
            {
                window.Loaded += loadedHandler;
            }
        }

        private static void CanMinimizePropertyChanged(
            DependencyObject obj,
            DependencyPropertyChangedEventArgs e)
        {
            Window window = obj as Window;

            if (window == null)
            {
                return;
            }

            RoutedEventHandler loadedHandler = null;

            loadedHandler = delegate
            {
                SetMinimizeEnabled(window, (bool)e.NewValue);
                window.Loaded -= loadedHandler;
            };

            if (window.IsLoaded)
            {
                loadedHandler(null, null);
            }
            else
            {
                window.Loaded += loadedHandler;
            }
        }

        private static void ShowIconPropertyChanged(
            DependencyObject obj,
            DependencyPropertyChangedEventArgs e)
        {
            Window window = obj as Window;

            if (window == null)
            {
                return;
            }

            RoutedEventHandler loadedHandler = null;

            loadedHandler = delegate
            {
                NativeSetShowIcon(window, (bool)e.NewValue);
                window.Loaded -= loadedHandler;
            };

            if (window.IsLoaded)
            {
                loadedHandler(null, null);
            }
            else
            {
                window.Loaded += loadedHandler;
            }
        }

        public static bool GetCanMaximize(DependencyObject obj)
        {
            return (bool)obj.GetValue(CanMaximizeProperty);
        }

        public static void SetCanMaximize(DependencyObject obj, bool value)
        {
            obj.SetValue(CanMaximizeProperty, value);
        }

        public static bool GetCanMinimize(DependencyObject obj)
        {
            return (bool)obj.GetValue(CanMinimizeProperty);
        }

        public static void SetCanMinimize(DependencyObject obj, bool value)
        {
            obj.SetValue(CanMinimizeProperty, value);
        }

        public static bool GetShowIcon(DependencyObject obj)
        {
            return (bool)obj.GetValue(ShowIconProperty);
        }

        public static void SetShowIcon(DependencyObject obj, bool value)
        {
            obj.SetValue(ShowIconProperty, value);
        }

        private static void SetMaximizeEnabled(Window window, bool value)
        {
            if (window == null)
            {
                throw new ArgumentNullException("window");
            }

            IntPtr hWnd = new WindowInteropHelper(window).Handle;
            int currentStyle = NativeMethods.GetWindowLongPtr(hWnd, GWL_STYLE);
            NativeMethods.SetWindowLongPtr(
                hWnd,
                GWL_STYLE,
                value ? currentStyle | WS_MAXIMIZEBOX : currentStyle & ~WS_MAXIMIZEBOX);
        }

        private static void SetMinimizeEnabled(Window window, bool value)
        {
            if (window == null)
            {
                throw new ArgumentNullException("window");
            }

            IntPtr hWnd = new WindowInteropHelper(window).Handle;
            int currentStyle = NativeMethods.GetWindowLongPtr(hWnd, GWL_STYLE);
            NativeMethods.SetWindowLongPtr(
                hWnd,
                GWL_STYLE,
                value ? currentStyle | WS_MINIMIZEBOX : currentStyle & ~WS_MINIMIZEBOX);
        }

        private static void NativeSetShowIcon(Window window, bool value)
        {
            if (window == null)
            {
                throw new ArgumentNullException("window");
            }

            IntPtr hWnd = new WindowInteropHelper(window).Handle;
            int currentStyle = NativeMethods.GetWindowLongPtr(hWnd, GWL_EXSTYLE);
            NativeMethods.SetWindowLongPtr(
                hWnd,
                GWL_EXSTYLE,
                value ? currentStyle & ~WS_EX_DLGMODALFRAME : currentStyle | WS_EX_DLGMODALFRAME);

            NativeMethods.SendMessage(hWnd, (int)WindowsMessages.WM_SETICON, (IntPtr)0, IntPtr.Zero);
            NativeMethods.SendMessage(hWnd, (int)WindowsMessages.WM_SETICON, (IntPtr)1, IntPtr.Zero);

            NativeMethods.SetWindowPos(hWnd, IntPtr.Zero, 0, 0, 0, 0, SWP_NOMOVE | SWP_NOSIZE | SWP_NOZORDER | SWP_FRAMECHANGED);
        }

        public static bool GetCloseOnEscape(DependencyObject obj)
        {
            return (bool)obj.GetValue(CloseOnEscapeProperty);
        }

        public static void SetCloseOnEscape(DependencyObject obj, bool value)
        {
            obj.SetValue(CloseOnEscapeProperty, value);
        }

        private static void HandleCloseOnEscapeChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            Window window = obj as Window;
            if (window != null)
            {
                if ((bool)e.NewValue)
                {
                    window.PreviewKeyDown += new System.Windows.Input.KeyEventHandler(HandleWindowPreviewKeyDown);
                }
                else
                {
                    window.PreviewKeyDown -= new System.Windows.Input.KeyEventHandler(HandleWindowPreviewKeyDown);
                }
            }
        }

        private static void HandleWindowPreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            Window window = sender as Window;
            if (window != null && e.Key == System.Windows.Input.Key.Escape)
            {
                IReportDragState reportDragState = window as IReportDragState;
                if (reportDragState == null || !reportDragState.IsDragInProgress)
                {
                    window.Close();
                }
            }
        }
    }
}
