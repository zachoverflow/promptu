using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using ZachJohnson.Promptu.UIModel.Interfaces;
using System.ComponentModel;
using System.Windows.Interop;
using System.Diagnostics;
using System.Windows.Threading;
using System.Windows.Forms.VisualStyles;

namespace ZachJohnson.Promptu.WpfUI.UIComponents
{
    /// <summary>
    /// Interaction logic for SetupDialog.xaml
    /// </summary>
    internal partial class SetupDialog : PromptuWindow, ISetupDialog
    {
        public static readonly DependencyProperty TabContentsHeightProperty =
            DependencyProperty.Register(
                "TabContentsHeight",
                typeof(double),
                typeof(SetupDialog),
                new PropertyMetadata(HandleTabContentsHeightPropertyChanged));

        private Border tabContentsBorder;
        private double lastTopMargin;
        private bool glassEnabled;
        private ParameterlessVoid settingChangedCallback;
        private Cursor defaultCursor;
        private bool lastActive;
        //private HwndSource hwndSource;

        public SetupDialog()
        {
            InitializeComponent();
            this.Loaded += this.HandleLoaded;

            this.SizeChanged += this.NotifySettingChanged;
            this.defaultCursor = this.Cursor;
            //this.StateChanged += this.HandleUISettingChanged;
            //this.LocationChanged += this.HandleUISettingChanged;
            //this.
            //((INotifyPropertyChanged)this).PropertyChanged += this.HandlePropertyChanged;
        }

        public ParameterlessVoid SettingChangedCallback
        {
            get { return this.settingChangedCallback; }
            set { this.settingChangedCallback = value; }
        }

        public string Text
        {
            get
            {
                return this.Title;
            }
            set
            {
                this.Title = value;
            }
        }

        public bool AllowSaveSettings
        {
            get { return this.IsSourceInitialized; }
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            if (this.IsSourceInitialized)
            {
                this.Dispatcher.BeginInvoke(new ParameterlessVoid(this.UpdateAfterApplyTemplate), DispatcherPriority.Render, null);
            }
        }

        private void UpdateAfterApplyTemplate()
        {
            this.UpdateTabContentsBorder();
            this.UpdateGlass(true);
            this.UpdateBackground(this.IsActive);
        }

        //public string CurrentPageHelpText
        //{
        //    get
        //    {
        //        return this.statusLabel.Text;
        //    }
        //    set
        //    {
        //        this.statusLabel.Text = value;
        //    }
        //}

        public bool IsShown
        {
            get { return this.IsVisible; }
        }

        public ITabControl MainTabs
        {
            get { return this.mainTabs; }
        }

        public void Refresh()
        {
        }

        public void ShowThreadSafe()
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new ParameterlessVoid(this.Show), null);
            }
            else
            {
                this.Show();
                this.Dispatcher.BeginInvoke(new ParameterlessVoid(this.UpdateAfterApplyTemplate), DispatcherPriority.Render, null);
            }
        }

        public void UnMinimizeIfNecessary()
        {
            if (this.WindowState == System.Windows.WindowState.Minimized)
            {
                this.WindowState = System.Windows.WindowState.Normal;
            }
        }

        public void ActivateThreadSafe()
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new ParameterlessVoid(this.ActivateInternal), null);
            }
            else
            {
                this.ActivateInternal();
            }
        }

        private void ActivateInternal()
        {
            this.Activate();
            //IInputElement element = Keyboard.FocusedElement;
            //if (element != null)
            //{
            //    element.Focus();
            //}
        }

        public void ShowOrBringToFront()
        {
            if (this.Visibility != System.Windows.Visibility.Visible)
            {
                this.Show();
            }

            this.ActivateInternal();
            //Keyboard.FocusedElement.Focus();
        }

        public void BeginUpdate()
        {
        }

        public void EndUpdate()
        {
        }

        public void BeginInvoke(Delegate method, object[] args)
        {
            this.Dispatcher.BeginInvoke(method, args);
        }

        public object Invoke(Delegate method, object[] args)
        {
            return this.Dispatcher.Invoke(method, args);
        }

        public bool InvokeRequired
        {
            get { return !this.Dispatcher.CheckAccess(); }
        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            // TODO evaluate shutdown
            e.Cancel = true;
            this.Hide();
        }

        private void HandleLoaded(object sender, EventArgs e)
        {
            this.UpdateTabContentsBorder();

            //SetupDialogMessageWindow messageWindow = new SetupDialogMessageWindow(
            //    this, 
            //    new System.Windows.Interop.WindowInteropHelper(this).Handle);
            this.UpdateGlass();
            this.UpdateBackground(true);
        }

        //protected override void OnStyleChanged(Style oldStyle, Style newStyle)
        //{
        //    base.OnStyleChanged(oldStyle, newStyle);
        //}

        private void UpdateTabContentsBorder()
        {
            this.tabContentsBorder = (Border)GetChildByType(this.mainTabs, typeof(Border));
            //this.tabContentsBorder.Name = "TheTabBorderElement";
            Binding heightBinding = new Binding("ActualHeight");
            heightBinding.Source = this.tabContentsBorder;
            //heightBinding.Mode = BindingMode.OneWayToSource;
            BindingOperations.ClearBinding(this, TabContentsHeightProperty);
            this.SetBinding(TabContentsHeightProperty, heightBinding);
        }

        private static void HandleTabContentsHeightPropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            SetupDialog dialog = obj as SetupDialog;
            if (dialog != null)
            {
                dialog.UpdateGlass();
            }
        }

        private void UpdateGlass()
        {
            this.UpdateGlass(false);
        }

        private void UpdateGlass(bool force)
        {
            if (this.tabContentsBorder == null)
            {
                return;
            }

            double topMargin = this.tabContentsBorder.TranslatePoint(new Point(), this).Y + 1;
            if (force || topMargin != this.lastTopMargin)
            {
                this.lastTopMargin = topMargin;
                this.glassEnabled = WpfUtilities.ExtendGlass(this, 0, 0, topMargin, 0, Brushes.Transparent);
            }
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            // TODO Test on xp this.glassEnabled && 
            if (e.Source == this)
            {
                try
                {
                    this.DragMove();
                }
                catch (InvalidOperationException)
                {
                }
            }

            base.OnMouseDown(e);
        }

        //private double TabHeight
        //{
        //    set
        //    {
        //    }
        //}

        //private static DependencyObject GetChildByType(DependencyObject parent, Type type)
        //{
        //    foreach (DependencyObject obj in LogicalTreeHelper.GetChildren(parent))
        //    {
        //        DependencyObject child = obj;
        //        //DependencyObject child = VisualTreeHelper.GetChild(parent, i);
        //        if (child.GetType() == type)
        //        {
        //            return child;
        //        }

        //        child = GetChildByType(child, type);

        //        if (child != null)
        //        {
        //            return child;
        //        }
        //    }

        //    return null;
        //}

        private static DependencyObject GetChildByType(DependencyObject parent, Type type)
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(parent, i);
                if (child.GetType() == type)
                {
                    return child;
                }

                child = GetChildByType(child, type);

                if (child != null)
                {
                    return child;
                }
            }

            return null;
        }

        //private void HandleUISettingChanged(object sender, EventArgs e)
        //{
        //    InternalGlobals.UISettings.SetupDialogSettings.UpdateFrom(this);
        //}

        protected override void OnLocationChanged(EventArgs e)
        {
            base.OnLocationChanged(e);
            this.NotifySettingChanged();
        }

        protected override void OnStateChanged(EventArgs e)
        {
            base.OnStateChanged(e);
            this.NotifySettingChanged();
        }

        private void NotifySettingChanged(object sender, EventArgs e)
        {
            this.NotifySettingChanged();
        }


        protected override void OnSourceInitialized(EventArgs e)
        {
            new SetupDialogMessageWindow(this, new WindowInteropHelper(this).Handle);

            //this.UpdateHwndSource();
            base.OnSourceInitialized(e);
            HwndSource source = PresentationSource.FromVisual(this) as HwndSource;
            source.AddHook(new HwndSourceHook(this.WndProcHook));


        }

        //private void UpdateHwndSource()
        //{
        //    try
        //    {
        //        HwndSource source = this.hwndSource;
        //        if (source != null)
        //        {
        //            source.Dispose();
        //        }

        //        source = PresentationSource.FromVisual(this) as HwndSource;
        //        source.AddHook(new HwndSourceHook(this.WndProcHook));
        //        this.hwndSource = source;
        //    }
        //    catch (Exception ex)
        //    {
        //    }
        //}

        private IntPtr WndProcHook(IntPtr handle, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            //System.Diagnostics.Debug.WriteLine(msg.ToString("x"));
            switch ((WindowsMessages)msg)
            {
                //case WindowsMessages.WM_POWERBROADCAST:
                //    this.UpdateHwndSource();
                //    break;
                case WindowsMessages.WM_THEMECHANGED:
                    //this.UpdateTabContentsBorder();
                    this.UpdateGlass(true);
                    this.UpdateBackground(this.IsActive);
                    break;
                case WindowsMessages.WM_DWMCOMPOSITIONCHANGED:
                    this.UpdateGlass(true);
                    this.UpdateBackground(this.IsActive);
                    break;
                case WindowsMessages.WM_NCACTIVATE:
                    if (this.UpdateBackground(wParam != IntPtr.Zero))
                    {
                        this.Dispatcher.Invoke(new ParameterlessVoid(delegate { }), new TimeSpan(0, 0, 0, 0, 100), DispatcherPriority.Render, null);
                    }

                    break;

            }
            //if ((WindowsMessages)msg == WindowsMessages.WM_DWMCOMPOSITIONCHANGED)
            //{
            //    this.UpdateGlass(true);
            //}

            return IntPtr.Zero;
        }

        private bool UpdateBackground(bool active)
        {
            //WindowInteropHelper interopHelper = new WindowInteropHelper(this);
            //IntPtr hWnd = interopHelper.Handle;

            //HwndSource mainWindowSrc = HwndSource.FromHwnd(hWnd);
            if (this.glassEnabled)
            {
                this.Background = Brushes.Transparent;
                return false;
            }
            else if (VisualStyleInformation.DisplayName.ToUpperInvariant() == "AERO STYLE")
            {
                this.Background = active ? SystemColors.GradientActiveCaptionBrush : SystemColors.GradientInactiveCaptionBrush;
                bool returnValue = active != this.lastActive;
                this.lastActive = active;
                return returnValue;
            }
            else
            {
                this.Background = SystemColors.ControlBrush;
                return false;
            }

            //mainWindowSrc.CompositionTarget.BackgroundColor = brush.Color;
        }

        private class SetupDialogMessageWindow : System.Windows.Forms.NativeWindow
        {
            private SetupDialog owner;

            public SetupDialogMessageWindow(SetupDialog owner, IntPtr handle)
            {
                this.owner = owner;
                this.AssignHandle(handle);
            }

            protected override void WndProc(ref System.Windows.Forms.Message m)
            {
                //switch ((WindowsMessages)m.Msg)
                //{
                //    case WindowsMessages.WM_THEMECHANGED:
                //        //this.UpdateTabContentsBorder();
                //        this.owner.UpdateGlass(true);
                //        this.owner.UpdateBackground(this.owner.IsActive);
                //        break;
                //    case WindowsMessages.WM_DWMCOMPOSITIONCHANGED:
                //        this.owner.UpdateGlass(true);
                //        this.owner.UpdateBackground(this.owner.IsActive);
                //        break;
                //    case WindowsMessages.WM_NCACTIVATE:
                //        if (this.owner.UpdateBackground(m.WParam != IntPtr.Zero))
                //        {
                //            this.owner.Dispatcher.Invoke(new ParameterlessVoid(delegate { }), new TimeSpan(0, 0, 0, 0, 100), DispatcherPriority.Render, null);
                //        }

                //        break;

                //}
                //System.Diagnostics.Debug.WriteLine(m);
                //if (m.Msg == (int)WindowsMessages.WM_NCHITTEST)
                //{
                //    Point mousePosition = new Point(
                //        WpfToolkitHost.ConvertToDeviceIndependentPixels(WpfUtilities.LOWORD((int)m.LParam)),
                //        WpfToolkitHost.ConvertToDeviceIndependentPixels(WpfUtilities.HIWORD((int)m.LParam)));

                //    mousePosition = this.owner.PointFromScreen(mousePosition);

                //    HitTestResult result = this.owner.HitTestCore(new PointHitTestParameters(mousePosition));
                //    if (result != null)
                //    {
                //        DependencyObject hit = result.VisualHit;
                //        if (hit == owner)
                //        {
                //            m.Result = (IntPtr)2;
                //            return;
                //        }
                //    }

                //    m.Result = (IntPtr)2;
                //    return;
                //}
                if (m.Msg == (int)WindowsMessages.WM_POWERBROADCAST)
                {
                    //MessageBox.Show(String.Format("POWER: {0}", m.WParam));
                    //if (m.WParam == (IntPtr)18)
                    //{
                    this.owner.UpdateGlass(true);
                    //}
                }
                else if (m.Msg == (int)WindowsMessages.WM_DWMCOMPOSITIONCHANGED)
                {
                    this.owner.UpdateGlass(true);
                }

                base.WndProc(ref m);
            }
        }

        private void NotifySettingChanged()
        {
            ParameterlessVoid callback = this.settingChangedCallback;
            if (callback != null)
            {
                callback();
            }
        }

        public void SetCursorToWait()
        {
            this.Cursor = Cursors.Wait;
        }

        public void SetCursorToDefault()
        {
            this.Cursor = this.defaultCursor;
        }
    }
}
