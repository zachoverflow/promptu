using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.IO;
using System.Windows;
using Microsoft.Win32.SafeHandles;
using System.Windows.Interop;
using System.Diagnostics;
using System.Globalization;

namespace ZachJohnson.Promptu.WpfUI
{
    public static class WpfUtilities
    {
        private static bool? segoeUIIsInstalled;

        internal static double? TryParseDouble(string value, double? defaultValue)
        {
            try
            {
                return Convert.ToDouble(value, CultureInfo.InvariantCulture);
            }
            catch (FormatException)
            {
            }
            catch (OverflowException)
            {
            }

            try
            {
                return Convert.ToDouble(value, CultureInfo.CurrentCulture);
            }
            catch (FormatException)
            {
            }
            catch (OverflowException)
            {
            }

            return defaultValue;
        }

        internal static int TryParseInt32(string value, int defaultValue)
        {
            try
            {
                return Convert.ToInt32(value, CultureInfo.InvariantCulture);
            }
            catch (FormatException)
            {
            }
            catch (OverflowException)
            {
            }

            try
            {
                return Convert.ToInt32(value, CultureInfo.CurrentCulture);
            }
            catch (FormatException)
            {
            }
            catch (OverflowException)
            {
            }

            return defaultValue;
        }

        internal static short LOWORD(int dWord)
        {
            return (short)dWord;
        }

        internal static short HIWORD(int dWord)
        {
            return (short)(dWord >> 16);
        }

        internal static bool HitTestAncestorIsType<T>(this Visual visual, Point location)
        {
            HitTestResult result = VisualTreeHelper.HitTest(visual, location);
            //if (result.
            if (result == null)
            {
                return false;
            }

            DependencyObject parent = result.VisualHit;

            while (parent != null && !(parent is T))
            {
                parent = VisualTreeHelper.GetParent(parent);
            }

            return parent != null;
        }

        internal static Point GetCursorPosition(Visual relativeTo, Point fallback)
        {
            NativeMethods.POINT mousePosition = new NativeMethods.POINT();
            NativeMethods.GetCursorPos(ref mousePosition);

            try
            {
                return relativeTo.PointFromScreen(new Point(mousePosition.x, mousePosition.y));
            }
            catch (InvalidOperationException)
            {
                return fallback;
            }
        }

        internal static T HitTestAncestorGet<T>(this Visual visual, Point location) where T : DependencyObject
        {
            HitTestResult result = VisualTreeHelper.HitTest(visual, location);
            //if (result.
            if (result == null)
            {
                return null;
            }

            DependencyObject parent = result.VisualHit;

            while (parent != null && !(parent is T))
            {
                parent = VisualTreeHelper.GetParent(parent);
            }

            return (T)parent;
        }

        internal static bool IsOrAncestorIsType<T>(this Visual visual)
        {
            DependencyObject parent = visual;

            while (parent != null && !(parent is T))
            {
                parent = VisualTreeHelper.GetParent(parent);
            }

            return parent != null;
        }

        internal static T GetThisOrAncestor<T>(this Visual visual) where T : DependencyObject
        {
            DependencyObject parent = visual;

            while (parent != null && !(parent is T))
            {
                parent = VisualTreeHelper.GetParent(parent);
            }

            return (T)parent;
        }

        internal static bool SegoeUIIsInstalled
        {
            get
            {
                if (segoeUIIsInstalled == null)
                {
                    FontFamilyConverter converter = new FontFamilyConverter();
                    foreach (FontFamily family in Fonts.SystemFontFamilies)
                    {
                        if (converter.ConvertToInvariantString(family).ToUpperInvariant() == "SEGOE UI")
                        {
                            segoeUIIsInstalled = true;
                            break;
                        }
                    }

                    if (segoeUIIsInstalled == null)
                    {
                        segoeUIIsInstalled = false;
                    }
                }

                return segoeUIIsInstalled.Value;
            }
        }

        internal static IEnumerable<FontFamily> GetSystemFonts()
        {
            //Debug.WriteLine("CALLED!!!!");
            return Fonts.SystemFontFamilies.OrderBy<FontFamily, string>(GetFontFamilyName);
        }

        private static string GetFontFamilyName(FontFamily item)
        {
            return item.ToString();
        }

        internal static Cursor ConvertToCursor(Brush brush, Size size, Point hotSpot)
        {
            int width = (int)size.Width;
            int height = (int)size.Height;

            RenderTargetBitmap bitmapSource = new RenderTargetBitmap(width, height, 96, 96, PixelFormats.Default);

            DrawingVisual drawingVisual = new DrawingVisual();
            using (DrawingContext drawingContext = drawingVisual.RenderOpen())
            {
                //VisualBrush vb = new VisualBrush(visual);
                drawingContext.DrawRectangle(brush, null, new Rect(new Point(), new Size(width, height)));
            }

            bitmapSource.Render(drawingVisual);

            PngBitmapEncoder bitmapEncoder = new PngBitmapEncoder();
            bitmapEncoder.Frames.Add(BitmapFrame.Create(bitmapSource));

            MemoryStream pngStream = new MemoryStream();
            bitmapEncoder.Save(pngStream);

            System.Drawing.Bitmap bitmap = (System.Drawing.Bitmap)System.Drawing.Bitmap.FromStream(pngStream);
            //bitmap.Save("C:\\tmp\\cursor.png");
            //return CreateCursorFrom(bitmap, 5, 5);
            return System.Windows.Interop.CursorInteropHelper.Create(new SafeIconHandle(CreateCursorFrom(bitmap, (int)hotSpot.X, (int)hotSpot.Y)));
            //MemoryStream cursorStream = new MemoryStream();

            //return new System.Windows.Forms.Cursor(bitmap.GetHicon());

            //System.Drawing.Bitmap.FromHicon(bitmap.GetHicon()).Save("C:\\tmp\\cursor2.png");

            //System.Drawing.Icon.FromHandle(bitmap.GetHicon()).Save(cursorStream);
            //using (FileStream s = new FileStream("C:\\tmp\\cursor.ico", FileMode.Create))
            //{
            //    System.Drawing.Icon.FromHandle(bitmap.GetHicon()).Save(s);
            //}

            //cursorStream.Seek(2, SeekOrigin.Begin);
            //cursorStream.WriteByte(2);
            //cursorStream.Seek(10, SeekOrigin.Begin);
            //cursorStream.WriteByte((byte)WpfToolkitHost.ConvertToPhysicalPixels(hotSpot.X));
            //cursorStream.WriteByte((byte)WpfToolkitHost.ConvertToPhysicalPixels(hotSpot.Y));
            //cursorStream.Seek(0, SeekOrigin.Begin);

            //return new Cursor(cursorStream);
        }

        private static IntPtr CreateCursorFrom(
            System.Drawing.Bitmap bitmap, 
            int hotspotX, 
            int hotspotY)
        {
            NativeMethods.ICONINFO iconInfo = new NativeMethods.ICONINFO();
            iconInfo.IsIcon = false;
            iconInfo.xHotspot = hotspotX;
            iconInfo.yHotspot = hotspotY;

            IntPtr hBitmap = bitmap.GetHbitmap();
            IntPtr hMask = NativeMethods.CreateBitmap(bitmap.Width, bitmap.Height, 1,1, IntPtr.Zero);

            iconInfo.ColorBitmap = hBitmap;
            iconInfo.MaskBitmap = hMask;

            IntPtr hCursor = NativeMethods.CreateIconIndirect(ref iconInfo);

            NativeMethods.DeleteObject(hBitmap);
            NativeMethods.DeleteObject(hMask);

            return hCursor;
        }

        private class SafeIconHandle : SafeHandleZeroOrMinusOneIsInvalid
        {
            public SafeIconHandle(IntPtr handle)
                : base(true)
            {
                this.handle = handle;
            }

            override protected bool ReleaseHandle()
            {
                return NativeMethods.DestroyIcon(this.handle);
            }
        }

        public static bool ExtendGlass(
            Window window,
            double left,
            double right,
            double top,
            double bottom,
            Brush fallbackBackground)
        {
            if (!TryExtendGlass(
                window,
                left,
                right,
                top,
                bottom))
            {
                window.Background = fallbackBackground;
                return false;
            }

            return true;
        }

        public static bool TryExtendGlass(
            Window window, 
            double left, 
            double right, 
            double top, 
            double bottom)
        {
            if (Environment.OSVersion.Platform != PlatformID.Win32NT 
                || Environment.OSVersion.Version.Major < 6)
            {
                return false;
            }

            WindowInteropHelper interopHelper = new WindowInteropHelper(window);
            IntPtr hWnd = interopHelper.Handle;

            HwndSource mainWindowSrc = HwndSource.FromHwnd(hWnd);
            mainWindowSrc.CompositionTarget.BackgroundColor = Color.FromArgb(0, 0, 0, 0);

            if (hWnd == IntPtr.Zero)
            {
                return false;
            }

            NativeMethods.Margins margins = new NativeMethods.Margins();
            margins.cxLeftWidth = WpfToolkitHost.ConvertToPhysicalPixels(left);
            margins.cxRightWidth = WpfToolkitHost.ConvertToPhysicalPixels(right);
            margins.cyBottomHeight = WpfToolkitHost.ConvertToPhysicalPixels(bottom);
            margins.cyTopHeight = WpfToolkitHost.ConvertToPhysicalPixels(top);

            try
            {
                IntPtr returnValue = NativeMethods.DwmExtendFrameIntoClientArea(
                    hWnd,
                    ref margins);

                return returnValue == IntPtr.Zero;
            }
            catch (DllNotFoundException)
            {
                return false;
            }
        }

        //private void CreateCursor(System.Drawing.Bitmap bitmap, Point hotSpot)
        //{
        //    var iconInfo = new NativeMethods.IconInfo();
        //    NativeMethods.GetIconInfo(bmp.GetHicon(), ref iconInfo);

        //    iconInfo.xHotspot = xHotSpot;
        //    iconInfo.yHotspot = yHotSpot;
        //    iconInfo.fIcon = false;

        //    SafeIconHandle cursorHandle = NativeMethods.CreateIconIndirect(ref iconInfo);
        //    return CursorInteropHelper.Create(cursorHandle);
        //}

        //public ImageSource LoadImageSourceByUri(FileSystemDirectory baseDirectory, Uri uri)
        //{
        //    string totalPath = baseDirectory + uri.OriginalString;

        //    string[] split = totalPath.Split(';');

        //    FileSystemFile file = split[0];

        //    if (!file.Exists)
        //    {
        //        return null;
        //    }

        //    if (file.Extension.ToUpperInvariant() == "XAML")
        //    {
        //        if (split.Length != 2)
        //        {
        //            return null;
        //        }

        //        //XamlReader xamlReader = XamlReader.R
        //    }
        //}
    }
}
