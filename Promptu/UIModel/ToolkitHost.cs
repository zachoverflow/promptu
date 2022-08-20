using System;
using System.Collections.Generic;
using System.Text;
using ZachJohnson.Promptu.UIModel.Configuration;
using ZachJohnson.Promptu.PTK;
using ZachJohnson.Promptu.SkinApi;
using System.Drawing;
using ZachJohnson.Promptu.Skins;
using ZachJohnson.Promptu.UIModel.Interfaces;

namespace ZachJohnson.Promptu.UIModel
{
    internal abstract class ToolkitHost
    {
        public const string DefaultSkinId = "www.promptulauncher.com/defaultskin";
        private readonly string toolkitName;
        private readonly ToolkitFactory factory;
        private readonly ToolkitSettings settings;
        private readonly Keyboard keyboard;
        private readonly WindowManager windowManager;
        private readonly Computer computer;
        private readonly Icon appIcon;
        private readonly ToolkitImages images;
        private readonly ToolkitClipboard clipboard;
        private readonly IThreadingInvoke mainThreadDispatcher;

        public ToolkitHost(
            string toolkitName, 
            ToolkitFactory factory, 
            ToolkitSettings settings, 
            Keyboard keyboard,
            WindowManager windowManager,
            Computer computer,
            Icon appIcon,
            ToolkitImages images,
            ToolkitClipboard clipboard,
            IThreadingInvoke mainThreadDispatcher)
        {
            if (toolkitName == null)
            {
                throw new ArgumentNullException("toolkitName");
            }
            else if (factory == null)
            {
                throw new ArgumentNullException("factory");
            }
            else if (settings == null)
            {
                throw new ArgumentNullException("settings");
            }
            else if (keyboard == null)
            {
                throw new ArgumentNullException("keyboard");
            }
            else if (windowManager == null)
            {
                throw new ArgumentNullException("windowManager");
            }
            else if (computer == null)
            {
                throw new ArgumentNullException("computer");
            }
            else if (appIcon == null)
            {
                throw new ArgumentNullException("appIcon");
            }
            else if (images == null)
            {
                throw new ArgumentNullException("images");
            }
            else if (clipboard == null)
            {
                throw new ArgumentNullException("clipboard");
            }
            else if (mainThreadDispatcher == null)
            {
                throw new ArgumentNullException("mainThreadDispatcher");
            }

            this.toolkitName = toolkitName.ToUpperInvariant();
            this.factory = factory;
            this.settings = settings;
            this.keyboard = keyboard;
            this.windowManager = windowManager;
            this.computer = computer;
            this.appIcon = appIcon;
            this.images = images;
            this.clipboard = clipboard;
            this.mainThreadDispatcher = mainThreadDispatcher;
        }

        public event EventHandler ApplicationExit;

        public string ToolkitName
        {
            get { return this.toolkitName; }
        }

        public IThreadingInvoke MainThreadDispatcher
        {
            get { return this.mainThreadDispatcher; }
        }

        public ToolkitClipboard Clipboard
        {
            get { return this.clipboard; }
        }

        public ToolkitImages Images
        {
            get { return this.images; }
        }

        public abstract bool GetPathFromWindowAtIsSupported { get; }

        public ToolkitFactory Factory
        {
            get { return this.factory; }
        }

        public ToolkitSettings Settings
        {
            get { return this.settings; }
        }

        public Icon AppIcon
        {
            get { return this.appIcon;}
        }

        public Keyboard Keyboard
        {
            get { return this.keyboard; }
        }

        public WindowManager WindowManager
        {
            get { return this.windowManager; }
        }

        public Computer Computer
        {
            get { return this.computer; }
        }

        public void InitializeToolkit()
        {
            this.InitializeToolkitCore();
        }

        public void StartMessageLoop()
        {
            this.StartMessageLoopCore();
        }

        public void ExitApplication()
        {
            this.ExitApplicationCore();
        }

        //public object ConvertImage(object image)
        //{
        //    return this.ConvertImage(image);
        //}

        public object ConvertImage(object image)
        {
            return this.ConvertImageCore(image);
        }

        //public void SetClipboardData(string format, object data)
        //{
        //    this.SetClipboardDataCore(format, data);
        //}

        //public void ClearClipboard()
        //{
        //    this.ClearClipboardCore();
        //}

        //public string GetClipboardText()
        //{
        //    return this.GetClipboardTextCore();
        //}

        public KeyboardSnapshot TakeKeyboardSnapshot()
        {
            return this.TakeKeyboardSnapshotCore();
        }

        internal PromptuSkinInstance CreateDefaultSkinInstance()
        {
            return this.CreateDefaultSkinInstanceCore();
        }

        public Icon ExtractDirectoryIcon(string path, IconSize size)
        {
            return this.ExtractDirectoryIconCore(path, size);
        }

        public Icon ExtractFileIcon(string path, IconSize size)
        {
            return this.ExtractFileIconCore(path, size);
        }

        public string ResolvePath(string path)
        {
            return this.ResolvePathCore(path);
        }

        public IEnumerable<PromptuSkin> GetDefaultSkins()
        {
            return this.GetDefaultSkinsCore();
        }

        public SystemFileVisibilitySettings GetSystemFileVisibility()
        {
            return this.GetSystemFileVisibilityCore();
        }

        public string GetPathFromWindowAt(Point point, bool excludeThisProcess, out bool executablePathNull)
        {
            return this.GetPathFromWindowAtCore(point, excludeThisProcess, out executablePathNull);
        }

        public void TrySetOwner(object child, object owner)
        {
            this.TrySetOwnerCore(child, owner);
        }

        public UIMessageBoxResult ShowMessageBox(
            string text,
            string caption,
            UIMessageBoxButtons buttons,
            UIMessageBoxIcon icon,
            UIMessageBoxResult defaultResult)
        {
            return this.ShowMessageBox(text,
                caption,
                buttons,
                icon,
                defaultResult,
                this.GetDefaultUIMessageBoxOptions());
        }

        public UIMessageBoxResult ShowMessageBox(
            string text,
            string caption,
            UIMessageBoxButtons buttons,
            UIMessageBoxIcon icon,
            UIMessageBoxResult defaultResult,
            UIMessageBoxOptions options)
        {
            return this.ShowMessageBoxCore(text,
                caption,
                buttons,
                icon,
                defaultResult,
                options);
        }

        public float ConvertToPhysicalQuantity(ScaledQuantity value)
        {
            return this.ConvertToPhysicalQuantityCore(value);
        }

        internal void NotifyProfileUnloading()
        {
            this.NotifyProfileUnloadingCore();
        }

        internal void NotifyProfileLoaded()
        {
            this.NotifyProfileLoadedCore();
        }

        //protected abstract string GetClipboardTextCore();

        //protected abstract void ClearClipboardCore();

        //protected abstract void SetClipboardDataCore(string format, object data);

        protected virtual void NotifyProfileUnloadingCore()
        {
        }

        protected virtual void NotifyProfileLoadedCore()
        {
        }

        protected abstract string ResolvePathCore(string path);

        protected abstract object ConvertImageCore(object image);

        protected abstract SystemFileVisibilitySettings GetSystemFileVisibilityCore();

        protected abstract IEnumerable<PromptuSkin> GetDefaultSkinsCore();

        protected abstract float ConvertToPhysicalQuantityCore(ScaledQuantity value);

        protected abstract void InitializeToolkitCore();

        protected abstract void StartMessageLoopCore();

        protected abstract void ExitApplicationCore();

        protected abstract PromptuSkinInstance CreateDefaultSkinInstanceCore();

        protected abstract UIMessageBoxOptions GetDefaultUIMessageBoxOptions();

        protected abstract Icon ExtractDirectoryIconCore(string path, IconSize size);

        protected abstract Icon ExtractFileIconCore(string path, IconSize size);

        protected abstract string GetPathFromWindowAtCore(Point point, bool excludeThisProcess, out bool executablePathNull);

        protected abstract KeyboardSnapshot TakeKeyboardSnapshotCore();

        protected abstract void TrySetOwnerCore(object child, object owner);

        protected abstract UIMessageBoxResult ShowMessageBoxCore(
            string text,
            string caption,
            UIMessageBoxButtons buttons,
            UIMessageBoxIcon icon,
            UIMessageBoxResult defaultResult,
            UIMessageBoxOptions options);

        protected void RaiseApplicationExit(object sender, EventArgs e)
        {
            this.OnApplicationExit(EventArgs.Empty);
        }

        protected virtual void OnApplicationExit(EventArgs e)
        {
            EventHandler handler = this.ApplicationExit;
            if (handler != null)
            {
                handler(this, e);
            }
        }
    }
}
