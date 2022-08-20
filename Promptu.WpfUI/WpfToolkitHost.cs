//-----------------------------------------------------------------------
// <copyright file="WpfToolkitHost.cs" company="ZachJohnson">
//     Copyright (c) Zach Johnson. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace ZachJohnson.Promptu.WpfUI
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using ZachJohnson.Promptu.UIModel;
    using ZachJohnson.Promptu.WpfUI.Configuration;
    using ZachJohnson.Promptu.WpfUI;
    using System.Drawing;
    using System.Windows;
    using System.Windows.Interop;
    using System.Windows.Input;
    using System.Runtime.InteropServices;
    using ZachJohnson.Promptu.SkinApi;
    using ZachJohnson.Promptu.Skins;
    using System.Windows.Media.Imaging;
    using System.Windows.Media;
    using System.IO;
    using Microsoft.Win32;
    using System.Speech.Recognition;

    internal delegate IntPtr HandleGetter(Window window);
 
    internal class WpfToolkitHost : ToolkitHost
    {
        private App app;
        private static float? systemPpi = null;

        public WpfToolkitHost()
            : base(
            "WPF",
            new WpfToolkitFactory(),
            new WpfToolkitSettings(),
            new WindowsKeyboard(),
            new WpfWindowManager(),
            new WindowsComputer(),
            WpfToolkitImages.NativeAppIcon,
#if NO_WPF
            new ZachJohnson.Promptu.WpfUI.Dummy.DummyToolkitImages(),
#else
            new WpfToolkitImages(),
#endif
            new WpfClipboard(),
            new MainThreadDispatcher())
        {
            this.app = new App();
            ((MainThreadDispatcher)this.MainThreadDispatcher).Dispatcher = this.app.Dispatcher;
            this.app.ShutdownMode = ShutdownMode.OnExplicitShutdown;
            this.app.Initialize();

            //this.app.Resources.MergedDictionaries.Add((ResourceDictionary)Application.LoadComponent(
            //    new Uri("Promptu.WpfUI;component/App.xaml", UriKind.RelativeOrAbsolute)));

            //this.app.InitializeComponent();

            //this.app.Resources.MergedDictionaries.Add((ResourceDictionary)Application.LoadComponent(
                //new Uri("Promptu.WpfUI;component/Images/PromptuIcons.xaml", UriKind.RelativeOrAbsolute)));

            //this.app.Resources.MergedDictionaries.Add((ResourceDictionary)Application.LoadComponent(
            //    new Uri("Promptu.WpfUI;component/AppResources.xaml", UriKind.RelativeOrAbsolute)));

            this.app.Exit += this.RaiseApplicationExit;
        }

        public static void InitializeWindow(Window window)
        {
            InitializeWindow(window, null);
        }

        public static void InitializeWindow(Window window, ParameterlessVoid whileShownCallback)
        {
            double oldWidth = window.Width;
            double oldHeight = window.Height;
            WindowStyle oldWindowStyle = window.WindowStyle;
            bool oldShowInTaskbar = window.ShowInTaskbar;
            bool oldShowActivated = window.ShowActivated;
            ResizeMode oldResizeMode = window.ResizeMode;
            double oldLeft = window.Left;
            double oldTop = window.Top;

            window.Width = 0;
            window.Height = 0;
            window.WindowStyle = WindowStyle.None;
            window.ShowInTaskbar = false;
            window.ShowActivated = false;
            window.ResizeMode = ResizeMode.NoResize;
            window.Left = -10000;
            window.Top = -10000;

            window.Show();
            window.Hide();

            window.Width = oldWidth;
            window.Height = oldHeight;
            window.WindowStyle = oldWindowStyle;
            window.ShowInTaskbar = oldShowInTaskbar;
            window.ShowActivated = oldShowActivated;
            window.ResizeMode = oldResizeMode;
            window.Left = oldLeft;
            window.Top = oldTop;
        }

        //protected override void SetClipboardDataCore(string format, object data)
        //{
        //    Clipboard.SetData(format, data);
        //}

        //protected override void ClearClipboardCore()
        //{
        //    Clipboard.Clear();
        //}

        //protected override string GetClipboardTextCore()
        //{
        //    return Clipboard.GetText();
        //}

        protected override void InitializeToolkitCore()
        {
        }

        protected override void StartMessageLoopCore()
        {
            this.app.Run();
        }

        protected override void ExitApplicationCore()
        {
            if (!this.app.Dispatcher.CheckAccess())
            {
                this.app.Dispatcher.Invoke(new ParameterlessVoid(this.app.Shutdown));
            }
            else
            {
                this.app.Shutdown();
            }
        }

        public override bool GetPathFromWindowAtIsSupported
        {
            get { return true; }
        }

        public static System.Windows.Point RoundOut(System.Windows.Point point)
        {
            return ConvertPoint(ConvertPoint(point));
        }

        protected override float ConvertToPhysicalQuantityCore(PTK.ScaledQuantity value)
        {
            if (value.ScalePpi == null)
            {
                return value.Value;
            }

            return (value.ScalePpi.Value / SystemPpi) * value.Value;
        }

        public static float SystemPpi
        {
            get
            {
                if (systemPpi == null)
                {
                    HandleRef desktopHwnd = new HandleRef(null, IntPtr.Zero);
                    HandleRef desktopDC = new HandleRef(null, NativeMethods.GetDC(desktopHwnd));

                     systemPpi = NativeMethods.GetDeviceCaps(desktopDC, 88 /*LOGPIXELSX*/);

                    NativeMethods.ReleaseDC(desktopHwnd, desktopDC);
                }

                return systemPpi.Value;
            }
        }

        protected override string GetPathFromWindowAtCore(System.Drawing.Point point, bool excludeThisProcess, out bool executablePathNull)
        {
            return WindowLocator.GetPathFromWindowAt(point, excludeThisProcess, out executablePathNull);
        }

        protected override Icon ExtractDirectoryIconCore(string path, IconSize size)
        {
            return ExtractIcon(path, size, false, NativeMethods.FILE_ATTRIBUTE_DIRECTORY);
        }

        protected override Icon ExtractFileIconCore(string path, IconSize size)
        {
            return ExtractIcon(path, size, false, NativeMethods.FILE_ATTRIBUTE_NORMAL);
        }

        protected override UIMessageBoxOptions GetDefaultUIMessageBoxOptions()
        {
            UIMessageBoxOptions options = UIMessageBoxOptions.None;

            if (System.Globalization.CultureInfo.CurrentUICulture.TextInfo.IsRightToLeft)
            {
                options |= UIMessageBoxOptions.RightAlign | UIMessageBoxOptions.RtlReading;
            }

            return options;
        }

        protected override UIMessageBoxResult ShowMessageBoxCore(
            string text,
            string caption,
            UIMessageBoxButtons buttons,
            UIMessageBoxIcon icon,
            UIMessageBoxResult defaultResult,
            UIMessageBoxOptions options)
        {
            return CastToUIMessageBoxResult(MessageBox.Show(
                text,
                caption,
                CastToMessageBoxButtons(buttons),
                CastToMessageBoxIcon(icon),
                CastToMessageBoxResult(defaultResult),
                CastToMessageBoxOptions(options)));
        }

        public static int ConvertToPhysicalPixels(double deviceIndependentPixels)
        {
            return (int)Math.Round(deviceIndependentPixels * (SystemPpi / 96F));
        }

        public static double ConvertToDeviceIndependentPixels(int pixels)
        {
            return pixels * (96F / SystemPpi);
        }

        public static System.Drawing.Size ConvertSize(double width, double height)
        {
            return new System.Drawing.Size(
                ConvertToPhysicalPixels(width),
                ConvertToPhysicalPixels(height));
        }

        public static System.Drawing.Size ConvertSize(System.Windows.Size size)
        {
            return new System.Drawing.Size(
                ConvertToPhysicalPixels(size.Width), 
                ConvertToPhysicalPixels(size.Height));
        }

        public static System.Windows.Size ConvertSize(System.Drawing.Size size)
        {
            return new System.Windows.Size(
                ConvertToDeviceIndependentPixels(size.Width), 
                ConvertToDeviceIndependentPixels(size.Height));
        }

        public static System.Drawing.Point ConvertPoint(System.Windows.Point point)
        {
            return new System.Drawing.Point(
                ConvertToPhysicalPixels(point.X),
                ConvertToPhysicalPixels(point.Y));
        }

        public static System.Windows.Point ConvertPoint(System.Drawing.Point point)
        {
            return new System.Windows.Point(
                ConvertToDeviceIndependentPixels(point.X), 
                ConvertToDeviceIndependentPixels(point.Y));
        }

        public static System.Windows.Forms.MouseEventArgs Convert(MouseWheelEventArgs e, IInputElement relativeTo)
        {
            var position = e.GetPosition(relativeTo);
            return new System.Windows.Forms.MouseEventArgs(
                ButtonsFromMouseEventArgs(e),
                1, // HACK
                ConvertToPhysicalPixels(position.X),
                ConvertToPhysicalPixels(position.Y),
                e.Delta);
        }

        public static System.Windows.Forms.MouseEventArgs Convert(MouseButtonEventArgs e, IInputElement relativeTo)
        {
            var position = e.GetPosition(relativeTo);
            return new System.Windows.Forms.MouseEventArgs(
                ButtonsFromMouseEventArgs(e),
                1, // HACK
                ConvertToPhysicalPixels(position.X),
                ConvertToPhysicalPixels(position.Y),
                0);
        }

        public static UIDialogResult ConvertToDialogResult(bool? value)
        {
            return value == true ? UIDialogResult.OK : UIDialogResult.Cancel;
        }

        public static void ApplyTextStyleTo(System.Windows.Controls.Control control, TextStyle style)
        {
            control.FontWeight = (style & TextStyle.Bold) == TextStyle.Bold ? FontWeights.Bold : FontWeights.Normal;
            control.FontStyle = (style & TextStyle.Italic) == TextStyle.Italic ? FontStyles.Italic : FontStyles.Normal;
        }

        public static UIDialogResult ShowDialogUIDialogResult(Window dialog)
        {
            return ConvertToDialogResult(ShowDialog(dialog, null));
        }

        public static bool? ShowDialog(Window dialog, Window owner)
        {
            WindowInteropHelper helper = new WindowInteropHelper(dialog);
            if (owner != null)
            {
                //owner.Dispatcher.Invoke(
                helper.Owner = GetHandleFrom(owner);//new WindowInteropHelper(owner).Handle;
            }
            else
            {
                helper.Owner = NativeMethods.GetActiveWindow();
            }

            return dialog.ShowDialog();
        }

        private static IntPtr GetHandleFrom(Window window)
        {
            if (!window.Dispatcher.CheckAccess())
            {
                return (IntPtr)window.Dispatcher.Invoke(new HandleGetter(GetHandleFrom), window);
            }
            else
            {
                return new WindowInteropHelper(window).Handle;
            }
        }

        public static void SetWindowOwner(Window window, Window owner)
        {
            WindowInteropHelper helper = new WindowInteropHelper(window);
            helper.Owner = new WindowInteropHelper(owner).Handle;
        }

        public static System.Windows.Forms.Keys ConvertKey(Key value, ModifierKeys modifers)
        {
            System.Windows.Forms.Keys keys = (System.Windows.Forms.Keys)KeyInterop.VirtualKeyFromKey(value);

            if ((modifers & ModifierKeys.Alt) == ModifierKeys.Alt)
            {
                keys |= System.Windows.Forms.Keys.Alt;
            }

            if ((modifers & ModifierKeys.Control) == ModifierKeys.Control)
            {
                keys |= System.Windows.Forms.Keys.Control;
            }

            if ((modifers & ModifierKeys.Shift) == ModifierKeys.Shift)
            {
                keys |= System.Windows.Forms.Keys.Shift;
            }

            return keys;
        }

        public static DragDropEffects CastToDragDropEffects(UIDragDropEffects effects)
        {
            DragDropEffects converted = DragDropEffects.None;

            if ((effects & UIDragDropEffects.Copy) == UIDragDropEffects.Copy)
            {
                converted |= DragDropEffects.Copy;
            }

            if ((effects & UIDragDropEffects.Link) == UIDragDropEffects.Link)
            {
                converted |= DragDropEffects.Link;
            }

            if ((effects & UIDragDropEffects.Move) == UIDragDropEffects.Move)
            {
                converted |= DragDropEffects.Move;
            }

            return converted;
        }

        public static UIDragDropEffects CastToUIDragDropEffects(DragDropEffects effects)
        {
            UIDragDropEffects converted = UIDragDropEffects.None;

            if ((effects & DragDropEffects.Copy) == DragDropEffects.Copy)
            {
                converted |= UIDragDropEffects.Copy;
            }

            if ((effects & DragDropEffects.Link) == DragDropEffects.Link)
            {
                converted |= UIDragDropEffects.Link;
            }

            if ((effects & DragDropEffects.Move) == DragDropEffects.Move)
            {
                converted |= UIDragDropEffects.Move;
            }

            return converted;
        }

        public static System.Windows.Forms.MouseButtons ButtonsFromMouseEventArgs(MouseEventArgs e)
        {
            System.Windows.Forms.MouseButtons buttons = System.Windows.Forms.MouseButtons.None;

            if (e.LeftButton == MouseButtonState.Pressed)
            {
                buttons |= System.Windows.Forms.MouseButtons.Left;
            }

            if (e.MiddleButton == MouseButtonState.Pressed)
            {
                buttons |= System.Windows.Forms.MouseButtons.Middle;
            }

            if (e.RightButton == MouseButtonState.Pressed)
            {
                buttons |= System.Windows.Forms.MouseButtons.Right;
            }

            if (e.XButton1 == MouseButtonState.Pressed)
            {
                buttons |= System.Windows.Forms.MouseButtons.XButton1;
            }

            if (e.XButton2 == MouseButtonState.Pressed)
            {
                buttons |= System.Windows.Forms.MouseButtons.XButton2;
            }

            return buttons;
        }

        public static MessageBoxOptions CastToMessageBoxOptions(UIMessageBoxOptions options)
        {
            MessageBoxOptions converted = (MessageBoxOptions)0;

            if ((options & UIMessageBoxOptions.RightAlign) == UIMessageBoxOptions.RightAlign)
            {
                converted |= MessageBoxOptions.RightAlign;
            }

            if ((options & UIMessageBoxOptions.RtlReading) == UIMessageBoxOptions.RtlReading)
            {
                converted |= MessageBoxOptions.RtlReading;
            }

            if ((options & UIMessageBoxOptions.DefaultDesktopOnly) == UIMessageBoxOptions.DefaultDesktopOnly)
            {
                converted |= MessageBoxOptions.DefaultDesktopOnly;
            }

            if ((options & UIMessageBoxOptions.ServiceNotification) == UIMessageBoxOptions.ServiceNotification)
            {
                converted |= MessageBoxOptions.ServiceNotification;
            }

            return converted;
        }

        public static MessageBoxImage CastToMessageBoxIcon(UIMessageBoxIcon icon)
        {
            switch (icon)
            {
                case UIMessageBoxIcon.Hand:
                    return MessageBoxImage.Hand;
                case UIMessageBoxIcon.Question:
                    return MessageBoxImage.Question;
                case UIMessageBoxIcon.Exclamation:
                    return MessageBoxImage.Exclamation;
                case UIMessageBoxIcon.Asterisk:
                    return MessageBoxImage.Asterisk;
                case UIMessageBoxIcon.Stop:
                    return MessageBoxImage.Stop;
                case UIMessageBoxIcon.Error:
                    return MessageBoxImage.Error;
                case UIMessageBoxIcon.Warning:
                    return MessageBoxImage.Warning;
                case UIMessageBoxIcon.Information:
                    return MessageBoxImage.Information;
                case UIMessageBoxIcon.None:
                default:
                    return MessageBoxImage.None;
            }
        }

        public static MessageBoxResult CastToMessageBoxResult(UIMessageBoxResult result)
        {
            switch (result)
            {
                case UIMessageBoxResult.OK:
                    return MessageBoxResult.OK;
                case UIMessageBoxResult.Yes:
                    return MessageBoxResult.Yes;
                case UIMessageBoxResult.Cancel:
                    return MessageBoxResult.Cancel;
                case UIMessageBoxResult.No:
                    return MessageBoxResult.No;
                default:
                    return MessageBoxResult.None;
            }
        }

        public static UIMessageBoxResult CastToUIMessageBoxResult(MessageBoxResult result)
        {
            switch (result)
            {
                case MessageBoxResult.OK:
                    return UIMessageBoxResult.OK;
                case MessageBoxResult.Yes:
                    return UIMessageBoxResult.Yes;
                case MessageBoxResult.Cancel:
                    return UIMessageBoxResult.Cancel;
                case MessageBoxResult.No:
                    return UIMessageBoxResult.No;
                default:
                    return UIMessageBoxResult.None;
            }
        }

        public static MessageBoxButton CastToMessageBoxButtons(UIMessageBoxButtons buttons)
        {
            switch (buttons)
            {
                case UIMessageBoxButtons.YesNo:
                    return MessageBoxButton.YesNo;
                case UIMessageBoxButtons.YesNoCancel:
                    return MessageBoxButton.YesNoCancel;
                case UIMessageBoxButtons.OKCancel:
                    return MessageBoxButton.OKCancel;
                default:
                    return MessageBoxButton.OK;
            }
        }

        private static Icon ExtractIcon(string path, IconSize size, bool openIcon, uint dwFileAttributes)
        {
            NativeMethods.SHFILEINFO fileInfo = new NativeMethods.SHFILEINFO();
            uint flags = NativeMethods.SHGFI_ICON | NativeMethods.SHGFI_USEFILEATTRIBUTES;

            if (size == IconSize.Large)
            {
                flags |= NativeMethods.SHGFI_LARGEICON;
            }
            else
            {
                flags |= NativeMethods.SHGFI_SMALLICON;
            }

            if (openIcon)
            {
                flags |= NativeMethods.SHGFI_OPENICON;
            }

            NativeMethods.SHGetFileInfo(path,
                dwFileAttributes,
                ref fileInfo,
                (uint)System.Runtime.InteropServices.Marshal.SizeOf(fileInfo),
                flags);

            if (fileInfo.hIcon != IntPtr.Zero)
            {
                Icon icon = (Icon)Icon.FromHandle(fileInfo.hIcon).Clone();
                NativeMethods.DestroyIcon(fileInfo.hIcon);
                return icon;
            }

            return null;
        }

        protected override void TrySetOwnerCore(object child, object owner)
        {
            IWin32Window win32Child = child as IWin32Window;
            IWin32Window win32Owner = owner as IWin32Window;
            if (win32Owner != null && win32Child != null && win32Owner.Handle != IntPtr.Zero && win32Child.Handle != IntPtr.Zero)
            {
                NativeMethods.SetOwner(win32Child, win32Owner);
            }
        }

        protected override KeyboardSnapshot TakeKeyboardSnapshotCore()
        {
            return new WindowsKeyboardSnapshot();
        }

        protected override PromptuSkinInstance CreateDefaultSkinInstanceCore()
        {
#if NO_WPF
            return new Dummy.DummySkinInstance();
#else
            return new DefaultSkin.SkinInstance();
#endif
        }

        protected override IEnumerable<PromptuSkin> GetDefaultSkinsCore()
        {
            List<PromptuSkin> defaultSkins = new List<PromptuSkin>();

#if NO_WPF
            defaultSkins.Add(new Dummy.DummySkin());
#else
            defaultSkins.Add(new DefaultSkin.Skin());
            defaultSkins.Add(new ClassicSkin.ClassicSkin());     
#endif

            return defaultSkins;
        }

        protected override SystemFileVisibilitySettings GetSystemFileVisibilityCore()
        {
            NativeMethods.SHELLSTATE state = new NativeMethods.SHELLSTATE();

            NativeMethods.SHGetSetSettings(
                ref state,
                //(uint)NativeMethods.SHELLFLAGS.fShowAllObjects,
                (uint)NativeMethods.SHELLFLAGS.fShowAllObjects | 0x00040000,
                false);

           return new SystemFileVisibilitySettings((state.bitVector1 & (1 << 15)) != 0, (state.bitVector1 & 0x00000001) != 0);
        }

        protected override object ConvertImageCore(object image)
        {
            return ConvertImageObject(image);
        }

        public static object ConvertImageObject(object image)
        {
            Bitmap bitmap = image as Bitmap;
            if (bitmap != null)
            {
                IntPtr hBitmap = bitmap.GetHbitmap();

                BitmapSource bitmapSource = null;
                //ImageSource returnValue = null;

                try
                {
                    Application.Current.Dispatcher.Invoke(new ParameterlessVoid(delegate
                    {
                        bitmapSource = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(hBitmap,
                            IntPtr.Zero, new Int32Rect(0, 0, bitmap.Width, bitmap.Height),
                            System.Windows.Media.Imaging.BitmapSizeOptions.FromEmptyOptions());

                        bitmapSource.Freeze();

                        //if (padding.Horizontal > 0 || padding.Vertical > 0)
                        //{
                        //    Rect bounds = new Rect(
                        //        padding.Left,
                        //        padding.Top,
                        //        bitmapSource.Width - padding.Horizontal,
                        //        bitmapSource.Height - padding.Vertical);

                        //    ImageDrawing drawing = new ImageDrawing(bitmapSource, bounds);
                        //    drawing.Freeze();
                        //    DrawingImage drawingImage = new DrawingImage(drawing);
                        //    drawingImage.Freeze();
                        //    returnValue = drawingImage;
                        //}
                        //else
                        //{
                        //    returnValue = bitmapSource;
                        //}
                    }));
                }
                finally
                {
                    NativeMethods.DeleteObject(hBitmap);
                }

                return bitmapSource ?? image;
            }

            return image;
        }

        protected override string ResolvePathCore(string path)
        {
            if (!path.Contains('/') && !path.Contains('\\'))
            {
                string fileToLookFor = path.ToUpperInvariant();

                if (!fileToLookFor.EndsWith(".exe", StringComparison.InvariantCultureIgnoreCase))
                {
                    fileToLookFor += ".EXE";
                }

                RegistryKey appPathsKey = Registry.LocalMachine.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\App Paths");

                foreach (string file in appPathsKey.GetSubKeyNames())
                {
                    if (file.ToUpperInvariant() == fileToLookFor)
                    {
                        RegistryKey fileKey = appPathsKey.OpenSubKey(file);

                        //string pathValue = fileKey.GetValue("PATH") as string;
                        //if (pathValue != null)
                        //{
                        //    return ((FileSystemDirectory)pathValue) + fileToLookFor;
                        //}

                        string defaultValue = fileKey.GetValue(null) as string;
                        if (defaultValue != null)
                        {
                            if (!defaultValue.EndsWith(fileToLookFor, StringComparison.InvariantCultureIgnoreCase))
                            {
                                defaultValue = ((FileSystemDirectory)defaultValue) + fileToLookFor;
                            }

                            return defaultValue;
                        }

                        break;
                    }
                }

                string pathEnvironmentVar = Environment.GetEnvironmentVariable("PATH", EnvironmentVariableTarget.Machine);
                if (pathEnvironmentVar != null)
                {
                    foreach (FileSystemDirectory dir in pathEnvironmentVar.Split(';'))
                    {
                        FileSystemFile possibleFile = dir + fileToLookFor;
                        if (possibleFile.Exists)
                        {
                            return possibleFile;
                        }
                    }
                }
            }

            return path;
        }
    }
}