using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZachJohnson.Promptu.UIModel;
using System.Windows;
using System.Windows.Media;
using System.Windows.Interop;
using System.Windows.Media.Imaging;

namespace ZachJohnson.Promptu.WpfUI
{
    internal class WpfToolkitImages : ToolkitImages
    {
        private ImageSource command;
        private ImageSource history;
        private ImageSource @namespace;
        private ImageSource function;
        private ImageSource nativeCommand;
        private ImageSource commandAndNamespace;
        private ImageSource functionAndNamespace;
        private ImageSource nativeCommandAndNamespace;
        private ImageSource historyAndNamespace;
        private Drawing namespaceOverlay;
        private ImageSource newCommand;
        private ImageSource newAssemblyReference;
        private ImageSource newFunction;
        private ImageSource newValueList;
        private ImageSource enable;
        private ImageSource disable;
        private ImageSource rename;
        private ImageSource unsubscribe;
        private ImageSource publish;
        private ImageSource export;
        private ImageSource delete;
        private ImageSource copy;
        private ImageSource paste;
        private ImageSource cut;
        private ImageSource edit;
        private ImageSource plugin;
        private ImageSource getPlugins;

        private static ImageSource upArrow;
        private static ImageSource downArrow;
        private static ImageSource appIcon;
        private static System.Drawing.Icon nativeAppIcon;
        private static IntPtr nativeAppIconHandle;

        public WpfToolkitImages()
        {
        }

        protected override object CommandCore
        {
            get 
            {
                if (this.command == null)
                {
                    this.command = new DrawingImage(
                        ((DrawingBrush)Application.Current.FindResource("Command")).Drawing);
                    this.command.Freeze();
                }

                return this.command;
            }
        }

        protected override object HistoryCore
        {
            get
            {
                if (this.history == null)
                {
                    this.history = new DrawingImage(
                        ((DrawingBrush)Application.Current.FindResource("History")).Drawing);
                    this.history.Freeze();
                }

                return this.history;
            }
        }

        protected override object NamespaceCore
        {
            get
            {
                if (this.@namespace == null)
                {
                    this.@namespace = new DrawingImage(
                        ((DrawingBrush)Application.Current.FindResource("Namespace")).Drawing);
                    this.@namespace.Freeze();
                }

                return this.@namespace;
            }
        }

        protected override object FunctionCore
        {
            get
            {
                if (this.function == null)
                {
                    this.function = new DrawingImage(
                        ((DrawingBrush)Application.Current.FindResource("Function")).Drawing);
                    this.function.Freeze();
                }

                return this.function;
            }
        }

        protected override object NativeCommandCore
        {
            get
            {
                if (this.nativeCommand == null)
                {
                    this.nativeCommand = new DrawingImage(
                        ((DrawingBrush)Application.Current.FindResource("NativeCommand")).Drawing);
                    this.nativeCommand.Freeze();
                }

                return this.nativeCommand;
            }
        }

        protected override object CommandAndNamespaceCore
        {
            get
            {
                if (this.commandAndNamespace == null)
                {
                    this.commandAndNamespace = Overlay(
                        ((DrawingBrush)Application.Current.FindResource("Command")).Drawing,
                        this.NamespaceOverlay);
                    this.commandAndNamespace.Freeze();
                }

                return this.commandAndNamespace;
            }
        }

        protected override object FunctionAndNamespaceCore
        {
            get
            {
                if (this.functionAndNamespace == null)
                {
                    this.functionAndNamespace = Overlay(
                        ((DrawingBrush)Application.Current.FindResource("Function")).Drawing,
                        this.NamespaceOverlay);
                    this.commandAndNamespace.Freeze();
                }

                return this.functionAndNamespace;
            }
        }

        protected override object NativeCommandAndNamespaceCore
        {
            get
            {
                if (this.nativeCommandAndNamespace == null)
                {
                    this.nativeCommandAndNamespace = Overlay(
                        ((DrawingBrush)Application.Current.FindResource("NativeCommand")).Drawing,
                        this.NamespaceOverlay);
                    this.nativeCommandAndNamespace.Freeze();
                }

                return this.nativeCommandAndNamespace;
            }
        }

        protected override object HistoryAndNamespaceCore
        {
            get
            {
                if (this.historyAndNamespace == null)
                {
                    this.historyAndNamespace = Overlay(
                        ((DrawingBrush)Application.Current.FindResource("History")).Drawing,
                        this.NamespaceOverlay);
                    this.historyAndNamespace.Freeze();
                }

                return this.historyAndNamespace;
            }
        }

        protected override object NewCommandCore
        {
            get
            {
                if (this.newCommand == null)
                {
                    this.newCommand = new DrawingImage(
                        ((DrawingBrush)Application.Current.FindResource("NewCommand")).Drawing);
                    this.newCommand.Freeze();
                }

                return this.newCommand;
            }
        }

        protected override object NewAssemblyReferenceCore
        {
            get
            {
                if (this.newAssemblyReference == null)
                {
                    this.newAssemblyReference = new DrawingImage(
                        ((DrawingBrush)Application.Current.FindResource("NewAssemblyReference")).Drawing);
                    this.newAssemblyReference.Freeze();
                }

                return this.newAssemblyReference;
            }
        }

        protected override object NewFunctionCore
        {
            get
            {
                if (this.newFunction == null)
                {
                    this.newFunction = new DrawingImage(
                        ((DrawingBrush)Application.Current.FindResource("NewFunction")).Drawing);
                    this.newFunction.Freeze();
                }

                return this.newFunction;
            }
        }

        protected override object NewValueListCore
        {
            get
            {
                if (this.newValueList == null)
                {
                    this.newValueList = new DrawingImage(
                        ((DrawingBrush)Application.Current.FindResource("NewValueList")).Drawing);
                    this.newValueList.Freeze();
                }

                return this.newValueList;
            }
        }

        protected override object EnableListCore
        {
            get
            {
                if (this.enable == null)
                {
                    this.enable = new DrawingImage(
                        ((DrawingBrush)Application.Current.FindResource("Enable")).Drawing);
                    this.enable.Freeze();
                }

                return this.enable;
            }
        }

        protected override object DisableListCore
        {
            get
            {
                if (this.disable == null)
                {
                    this.disable = new DrawingImage(
                        ((DrawingBrush)Application.Current.FindResource("Disable")).Drawing);
                    this.disable.Freeze();
                }

                return this.disable;
            }
        }

        protected override object RenameListCore
        {
            get
            {
                if (this.rename == null)
                {
                    this.rename = new DrawingImage(
                        ((DrawingBrush)Application.Current.FindResource("Rename")).Drawing);
                    this.rename.Freeze();
                }

                return this.rename;
            }
        }

        protected override object UnsubscribeListCore
        {
            get
            {
                if (this.unsubscribe == null)
                {
                    this.unsubscribe = new DrawingImage(
                        ((DrawingBrush)Application.Current.FindResource("Unsubscribe")).Drawing);
                    this.unsubscribe.Freeze();
                }

                return this.unsubscribe;
            }
        }

        protected override object PublishListCore
        {
            get
            {
                if (this.publish == null)
                {
                    this.publish = new DrawingImage(
                        ((DrawingBrush)Application.Current.FindResource("Publish")).Drawing);
                    this.publish.Freeze();
                }

                return this.publish;
            }
        }

        protected override object ExportListCore
        {
            get
            {
                if (this.export == null)
                {
                    this.export = new DrawingImage(
                        ((DrawingBrush)Application.Current.FindResource("Export")).Drawing);
                    this.export.Freeze();
                }

                return this.export;
            }
        }

        protected override object DeleteCore
        {
            get
            {
                if (this.delete == null)
                {
                    this.delete = new DrawingImage(
                        ((DrawingBrush)Application.Current.FindResource("RedX")).Drawing);
                    this.delete.Freeze();
                }

                return this.delete;
            }
        }

        protected override object CopyCore
        {
            get
            {
                if (this.copy == null)
                {
                    this.copy = new DrawingImage(
                        ((DrawingBrush)Application.Current.FindResource("Copy")).Drawing);
                    this.copy.Freeze();
                }

                return this.copy;
            }
        }

        protected override object PasteCore
        {
            get
            {
                if (this.paste == null)
                {
                    this.paste = new DrawingImage(
                        ((DrawingBrush)Application.Current.FindResource("Paste")).Drawing);
                    this.paste.Freeze();
                }

                return this.paste;
            }
        }

        protected override object PluginCore
        {
            get
            {
                if (this.plugin == null)
                {
                    this.plugin = new DrawingImage(
                        ((DrawingBrush)Application.Current.FindResource("DefaultPluginImage")).Drawing);
                    this.plugin.Freeze();
                }

                return this.plugin;
            }
        }

        protected override object GetPluginsCore
        {
            get
            {
                if (this.getPlugins == null)
                {
                    this.getPlugins = new DrawingImage(
                        ((DrawingBrush)Application.Current.FindResource("Incoming")).Drawing);
                    this.getPlugins.Freeze();
                }

                return this.getPlugins;
            }
        }

        protected override object CutCore
        {
            get
            {
                if (this.cut == null)
                {
                    this.cut = new DrawingImage(
                        ((DrawingBrush)Application.Current.FindResource("Cut")).Drawing);
                    this.cut.Freeze();
                }

                return this.cut;
            }
        }

        protected override object EditCore
        {
            get
            {
                if (this.edit == null)
                {
                    this.edit = new DrawingImage(
                        ((DrawingBrush)Application.Current.FindResource("Edit")).Drawing);
                    this.edit.Freeze();
                }

                return this.edit;
            }
        }

        public static ImageSource UpArrow
        {
            get
            {
#if NO_WPF
                return null;
#endif
                if (upArrow == null)
                {
                    upArrow = new DrawingImage(
                        ((DrawingBrush)Application.Current.FindResource("UpArrow")).Drawing);
                    upArrow.Freeze();
                }

                return upArrow;
            }
        }

        public static ImageSource DownArrow
        {
            get
            {
#if NO_WPF
                return null;
#endif
                if (downArrow == null)
                {
                    downArrow = new DrawingImage(
                        ((DrawingBrush)Application.Current.FindResource("DownArrow")).Drawing);
                    downArrow.Freeze();
                }

                return downArrow;
            }
        }

        public static ImageSource AppIcon
        {
            get
            {
#if NO_WPF
                return null;
#endif
                if (appIcon == null)
                {
                    appIcon = Imaging.CreateBitmapSourceFromHIcon(NativeAppIcon.Handle,
                        new Int32Rect(0, 0, 16, 16),
                        BitmapSizeOptions.FromEmptyOptions());

                    appIcon.Freeze();
                }

                return appIcon;
            }
        }

        public static System.Drawing.Icon NativeAppIcon
        {
            get
            {
                if (nativeAppIcon == null)
                {
                    nativeAppIcon = System.Drawing.Icon.FromHandle(NativeAppIconHandle);
                    //    string fileName = System.Reflection.Assembly.GetEntryAssembly().Location;
                    //    IntPtr hLibrary = NativeMethods.LoadLibrary(fileName);
                    //    if (!hLibrary.Equals(System.IntPtr.Zero))
                    //    {
                    //        IntPtr hIcon = NativeMethods.LoadIcon(hLibrary, "#32512");
                    //        if (!hIcon.Equals(System.IntPtr.Zero))
                    //        {
                    //            nativeAppIconHandle = hIcon;
                    //            nativeAppIcon = System.Drawing.Icon.FromHandle(hIcon);
                    //        }
                    //    }

                    //    nativeAppIcon = System.Drawing.Icon.FromHandle(StandardImages.Icon.GetHicon());
                    //}
                } 

                return nativeAppIcon;
            }
        }

        public static IntPtr NativeAppIconHandle
        {
            get
            {
                if (nativeAppIconHandle == IntPtr.Zero)
                {
                    string fileName = System.Reflection.Assembly.GetEntryAssembly().Location;

                    //NativeMethods.SHFILEINFO fileInfo = new NativeMethods.SHFILEINFO();
                    //uint flags = NativeMethods.SHGFI_ICON | NativeMethods.SHGFI_USEFILEATTRIBUTES;

                    //flags |= NativeMethods.SHGFI_LARGEICON;

                    ////if (size == IconSize.Large)
                    ////{
                    ////    flags |= NativeMethods.SHGFI_LARGEICON;
                    ////}
                    ////else
                    ////{
                    ////    flags |= NativeMethods.SHGFI_SMALLICON;
                    ////}

                    ////if (openIcon)
                    ////{
                    ////    flags |= NativeMethods.SHGFI_OPENICON;
                    ////}

                    //NativeMethods.SHGetFileInfo(fileName,
                    //    NativeMethods.FILE_ATTRIBUTE_NORMAL,
                    //    ref fileInfo,
                    //    (uint)System.Runtime.InteropServices.Marshal.SizeOf(fileInfo),
                    //    flags);

                    //if (fileInfo.hIcon != IntPtr.Zero)
                    //{
                    //    nativeAppIconHandle = fileInfo.hIcon;
                    //}

                    IntPtr hLibrary = NativeMethods.LoadLibrary(fileName);
                    if (!hLibrary.Equals(System.IntPtr.Zero))
                    {
                        IntPtr hIcon = NativeMethods.LoadIcon(hLibrary, "#32512");
                        if (!hIcon.Equals(System.IntPtr.Zero))
                        {
                            nativeAppIconHandle = hIcon;
                        }
                    }
                }

                if (nativeAppIconHandle == null)
                {
                    nativeAppIconHandle = StandardImages.Icon.GetHicon();
                }

                return nativeAppIconHandle;
                //IntPtr handle = nativeAppIconHandle;
                //if (handle != IntPtr.Zero)
                //{
                //    return handle;
                //}
            }
        }

        //public static ImageDrawing Convert(ImageSource imageSource)
        //{
        //    ImageDrawing drawing = new ImageDrawing(imageSource, new Rect(0,0, imageSource.Width, imageSource.Height));

        //    return drawing;
        //}

        public static ImageSource Overlay(Drawing background, Drawing overlay)
        {
            DrawingGroup backgroundEncapsulator = new DrawingGroup();
            backgroundEncapsulator.Children.Add(background);
            backgroundEncapsulator.Transform = new TranslateTransform(-background.Bounds.Left, -background.Bounds.Top);

            backgroundEncapsulator.Freeze();

            DrawingGroup overlayEncapsulator = new DrawingGroup();
            overlayEncapsulator.Children.Add(overlay);
            overlayEncapsulator.Transform = new TranslateTransform(-overlay.Bounds.Left, -overlay.Bounds.Top);

            overlayEncapsulator.Freeze();

            DrawingGroup composite = new DrawingGroup();
            
            composite.Children.Add(backgroundEncapsulator);
            composite.Children.Add(overlayEncapsulator);
            

            DrawingImage image = new DrawingImage(composite);
            composite.Freeze();
            image.Freeze();
            return image;
        }

        private Drawing NamespaceOverlay
        {
            get
            {
                if (this.namespaceOverlay == null)
                {
                    this.namespaceOverlay = 
                        ((DrawingBrush)Application.Current.FindResource("NamespaceOverlay")).Drawing;
                    this.namespaceOverlay.Freeze();
                }

                return this.namespaceOverlay;
            }
        }

        protected override object CreateCompositeWithNamespaceOverlayCore(System.Drawing.Bitmap bitmap)
        {
            ImageSource imageSource = WpfToolkitHost.ConvertImageObject(bitmap) as ImageSource;
            if (imageSource == null)
            {
                return null;
            }

            Rect bounds = new Rect(1, 1, NamespaceOverlay.Bounds.Width - 2, NamespaceOverlay.Bounds.Height - 2);

            ImageDrawing drawing = new ImageDrawing(imageSource, bounds);
            //drawing.ImageSource = imageSource;
            drawing.Freeze();

            return Overlay(drawing, NamespaceOverlay);
        }
    }
}
