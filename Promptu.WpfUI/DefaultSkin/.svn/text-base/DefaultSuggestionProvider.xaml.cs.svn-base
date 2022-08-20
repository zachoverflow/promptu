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
using ZachJohnson.Promptu.SkinApi;
using System.Collections.ObjectModel;
using System.Windows.Interop;
using System.Windows.Controls.Primitives;
using ZachJohnson.Promptu.PluginModel;
using ZachJohnson.Promptu.Localization;
using System.ComponentModel;
using ZachJohnson.Promptu.WpfUI.UIComponents;

namespace ZachJohnson.Promptu.WpfUI.DefaultSkin
{
    /// <summary>
    /// Interaction logic for DefaultSuggestionProvider.xaml
    /// </summary>
    internal partial class DefaultSuggestionProvider : Window, ISuggestionProvider, INotifyPropertyChanged
    {
        private static DependencyProperty highlightBorderProperty = DependencyProperty.Register(
            "HighlightBorder",
            typeof(SolidColorBrush),
            typeof(DefaultSuggestionProvider));

        private static DependencyProperty highlightTextProperty = DependencyProperty.Register(
            "HighlightText",
            typeof(SolidColorBrush),
            typeof(DefaultSuggestionProvider));

        private static DependencyProperty highlightBackgroundProperty = DependencyProperty.Register(
            "HighlightBackground",
            typeof(LinearGradientBrush),
            typeof(DefaultSuggestionProvider));

        private System.Drawing.Size? saveSize;
        private List<object> images = new List<object>();
        private NativeHelper nativeHelper;
        private int? lastHeight = 0;
        private OptionPage options;
        private ObjectPropertyCollection savingProperties;
        private ZachJohnson.Promptu.UIModel.UIContextMenu selectedItemContextMenu;
        private object itemSyncObject = new object();
        //private ObservableCollection<SuggestionItemWrapper> items = new ObservableCollection<SuggestionItemWrapper>();
        //private CenteringListBox listBox; //= new CenteringListBox();

        //private LinearGradientBrush highlightBrush;
        //SolidColorBrush highlightBorderBrush;
        //private int desiredIconSize; = 16;

        public DefaultSuggestionProvider()
        {
            //this.Width = 350d;
            //this.Height = 200d;
            //this.saveSize = WpfToolkitHost.ConvertSize(3d, 200d);
            InitializeComponent();
            //this.listBox = new CenteringListBox();
            //this.listBox.ItemsSource = this.items;
            this.HighlightText = SystemColors.ControlTextBrush.Clone();
            this.HighlightBackground = ((LinearGradientBrush)this.TryFindResource("HighlightBackground")).Clone();
            this.HighlightBorder = ((SolidColorBrush)this.TryFindResource("HighlightBorderColor")).Clone();
            this.options = new OptionPage(); //new OptionPage(Localization.UIResources.SuggestionProviderOptionsMainInstructions);
            this.savingProperties = new ObjectPropertyCollection();

            FontFamilyConverter converter = new FontFamilyConverter();
            if (converter.ConvertToInvariantString(this.FontFamily).ToUpperInvariant() == "TAHOMA")
            {
                if (WpfUtilities.SegoeUIIsInstalled)
                {
                    this.FontFamily = new FontFamily("Segoe UI");
                }
            }

            this.IsVisibleChanged += this.HandleIsVisibleChanged;

            GroupingConversionInfo grouping = new GroupingConversionInfo("group", false);

            BoundObjectProperty<FontFamily> fontFamily = new BoundObjectProperty<FontFamily>(
                "FontFamily",
                UIResources.DefaultSPFontFamilyLabel,
                "FontFamily",
                this,
                null,
                new TextConversionInfo("group", false, null, 100),
                null);

            BoundObjectProperty<int> fontSize = new BoundObjectProperty<int>(
               "FontSize",
               UIResources.DefaultSPOptionsFontSizeLabel,
               "FontSize",
               this.listBox,
               null,
               new TextConversionInfo("group", false, null, 30),
               new PointFontSizeConverter());

            BoundObjectProperty<Color> backgroundColor = new BoundObjectProperty<Color>(
                "BackgroundColor",
                UIResources.DefaultSPBackgroundColorText,
                "BackgroundColor",
                this,
                null,
                grouping,
                null);

            BoundObjectProperty<Color> textColor = new BoundObjectProperty<Color>(
               "TextColor",
               UIResources.DefaultSPTextColorText,
               "TextColor",
               this,
               null,
               grouping,
               null);

            BoundObjectProperty<Color> selectionBackgroundColor = new BoundObjectProperty<Color>(
                "SelectionBackgroundColor",
                UIResources.DefaultSPSelectionBackgroundColorText,
                "SelectionBackgroundColor",
                this,
                null,
                grouping,
                null);

            BoundObjectProperty<Color> selectionTextColor = new BoundObjectProperty<Color>(
                "SelectionTextColor",
                UIResources.DefaultSPSelectionTextColorText,
                "SelectionTextColor",
                this,
                null,
                grouping,
                null);

            BoundObjectProperty<Size> saveSize = new BoundObjectProperty<Size>(
                "Size",
                null,
                "DISaveSize",
                this);

            OptionsGroup appearanceGroup = new OptionsGroup("Appearance", UIResources.DefaultSPOptionsAppearanceGroup);
            appearanceGroup.Add(fontFamily);
            appearanceGroup.Add(fontSize);
            appearanceGroup.Add(backgroundColor);
            appearanceGroup.Add(textColor);
            appearanceGroup.Add(selectionBackgroundColor);
            appearanceGroup.Add(selectionTextColor);

            this.options.Groups.Add(appearanceGroup);

            this.savingProperties.Add(fontFamily);
            this.savingProperties.Add(fontSize);
            this.savingProperties.Add(backgroundColor);
            this.savingProperties.Add(textColor);
            this.savingProperties.Add(selectionBackgroundColor);
            this.savingProperties.Add(selectionTextColor);
            this.savingProperties.Add(saveSize);

            fontFamily.Value = this.FontFamily;

            this.selectedItemContextMenu = new UIModel.UIContextMenu();

            this.ContextMenu = (ContextMenu)this.selectedItemContextMenu.NativeContextMenuInterface;
            //this.ContextMenuOpening += this.HandleContextMenuOpening;
            //WpfToolkitHost.InitializeWindow(this);

            //this.listBox.MouseUp += this.RaiseUserInteractionFinishedEvent;
            this.listBox.SelectionChanged += this.RaiseSelectedIndexChanged;
            this.listBox.DesiredIconSizeChanged += this.HandleListBoxDesiredIconSizeChanged;

            this.listBox.AddHandler(ScrollBar.ScrollEvent, new ScrollEventHandler(this.HandleScrollChanged));
            this.listBox.MouseDoubleClick += this.HandleMouseDoubleClick;

            //this.SelectionColor = Colors.White;//Color.FromArgb(55, 0,  144, 255);
            //this.BackgroundColor = Colors.Black;
            
            //(this.listBox.ItemsPanel.).AddHandler(Control.MouseDoubleClickEvent, new MouseButtonEventHandler(this.HandleMouseDoubleClick));
            //this.listBox.MouseDoubleClick += this.HandleMouseDoubleClick;
            //this.listBox.ScrollFinished += this.RaiseUserInteractionFinishedEvent;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public event EventHandler SelectedIndexChanged;

        public event EventHandler UserInteractionFinished;

        public event EventHandler VisibleChanged;

        public new event System.Windows.Forms.MouseEventHandler MouseDoubleClick;

        public event EventHandler DesiredIconSizeChanged;

        public event CancelEventHandler SelectedItemContextMenuOpening;

        public SolidColorBrush HighlightBorder
        {
            get { return (SolidColorBrush)this.GetValue(highlightBorderProperty); }
            set { this.SetValue(highlightBorderProperty, value); }
        }

        public SolidColorBrush HighlightText
        {
            get { return (SolidColorBrush)this.GetValue(highlightTextProperty); }
            set { this.SetValue(highlightTextProperty, value); }
        }

        public LinearGradientBrush HighlightBackground
        {
            get { return (LinearGradientBrush)this.GetValue(highlightBackgroundProperty); }
            set { this.SetValue(highlightBackgroundProperty, value); }
        }

        protected virtual void OnSelectedItemContextMenuOpening(CancelEventArgs e)
        {
            CancelEventHandler handler = this.SelectedItemContextMenuOpening;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        protected override void OnContextMenuOpening(ContextMenuEventArgs e)
        {
            base.OnContextMenuOpening(e);

            if (this.HitTestAncestorIsType<ListBoxItem>(Mouse.GetPosition(this)))
            {
                CancelEventArgs eventArgs = new CancelEventArgs();
                this.OnSelectedItemContextMenuOpening(eventArgs);
                e.Handled = eventArgs.Cancel;
            }
            else
            {
                e.Handled = true;
            }
        }

        protected override void OnContextMenuClosing(ContextMenuEventArgs e)
        {
            base.OnContextMenuClosing(e);
            this.OnUserInteractionFinished(EventArgs.Empty);
        }

        //private Color BackgroundColorColor
        //{
        //    get
        //    {
        //        return ((SolidColorBrush)this.listBox.Background).Color;// = Colors.Black;
        //    }

        //    set
        //    {
        //        ((SolidColorBrush)this.listBox.Background).Color = value;
        //    }
        //}

        public Color SelectionTextColor
        {
            get { return this.HighlightText.Color; }
            set { this.HighlightText.Color = value; }
        }

        public Color SelectionBackgroundColor
        {
            get
            {
                //LinearGradientBrush brush = (LinearGradientBrush)this.TryFindResource("HighlightBackground");
                return this.HighlightBorder.Color;//this.HighlightBackground.GradientStops[1].Color;
            }

            set
            {
                //HSVColor color = new HSVColor(value);

                // OLD
                // highlight:
                // A + 5
                // H (same)
                // V (same)
                // S - 0.4392156862745098

                // border:
                // A + 90
                // H (same)
                // V (same)
                // S - 0.22352941176470587

                //new
                // highlight darker
                // H + -0.30341340075852941
                // S + -0.22352941176470587
                // V + 0

                // highlight lighter
                // H + 0.45288912024989258
                // S + -0.30980392156862746
                // V + 0

                //value.A = 55;



                //HSVColor color1 = new HSVColor(this.HighlightBackground.GradientStops[1].Color);
                //HSVColor color2 = new HSVColor(this.HighlightBackground.GradientStops[0].Color);
                //HSVColor borderColor = new HSVColor(this.HighlightBorder.Color);

                //int aDiff1 = color2.H - borderColor.H;
                
                //brush.GradientStops[1].Color = value;
                //HSVColor newMainColor = new HSVColor(value);
                //newMainColor.A = 55;

                try
                {
                    HSVColor newHighlightDark = new HSVColor(value);
                    newHighlightDark.H += -0.30341340075852941;
                    newHighlightDark.S += -0.22352941176470587;
                    newHighlightDark.Clamp();
                    //newHighlight.A = 60;
                    //newHighlight.S -= 0.4392156862745098;

                    //if (newHighlight.S < 0)
                    //{
                    //    newHighlight.S = 0;
                    //}

                    HSVColor newHighlightLight = new HSVColor(value);
                    newHighlightLight.H += 0.45288912024989258;
                    newHighlightLight.S += -0.30980392156862746;
                    newHighlightLight.Clamp();
                    //newBorderColor.A = 145;
                    //newBorderColor.S -= 0.22352941176470587;

                    //if (newBorderColor.S < 0)
                    //{
                    //    newBorderColor.S = 0;
                    //}

                    this.HighlightBackground.GradientStops[0].Color = newHighlightLight.ToColor();
                    this.HighlightBackground.GradientStops[1].Color = newHighlightDark.ToColor();

                    this.HighlightBorder.Color = value;//newBorderColor.ToColor();
                }
                catch
                {
                }

                //int aDiff1 = borderColor.S - color1.S;
                //int hDiff1 = color1.H - color2.H;
                //brush.GradientStops.
            }
        }

        public int DesiredIconSize
        {
            get { return WpfToolkitHost.ConvertToPhysicalPixels(this.listBox.DesiredIconSize); }
        }

        public Color BackgroundColor
        {
            get { return ((SolidColorBrush)this.listBox.Background).Color; }
            set { ((SolidColorBrush)this.listBox.Background).Color = value; }
        }

        public ZachJohnson.Promptu.UIModel.UIContextMenu SelectedItemContextMenu
        {
            get { return this.selectedItemContextMenu; }
        }

        public Color TextColor
        {
            get
            {
                return ((SolidColorBrush)this.listBox.Foreground).Color;
            }

            set
            {
                ((SolidColorBrush)this.listBox.Foreground).Color = value;
            }
        }

        public OptionPage Options
        {
            get { return this.options; }
        }

        public ObjectPropertyCollection SavingProperties
        {
            get { return this.savingProperties; }
        }

        public IList<object> Images
        {
            get { return this.images; }
        }

        public bool Visible
        {
            get { return this.IsVisible; }
        }

        private void HandleScrollChanged(object sender, ScrollEventArgs e)
        {
            if (e.ScrollEventType == ScrollEventType.EndScroll)
            {
                this.OnUserInteractionFinished(EventArgs.Empty);
            }
        }

        private Size DISaveSize
        {
            get
            {
                return WpfToolkitHost.ConvertSize(this.SaveSize);
            }

            set
            {
                if (value.Height != 0 && value.Width != 0)
                {
                    this.SaveSize = WpfToolkitHost.ConvertSize(value);
                }
            }
        }

        public bool ContainsFocus
        {
            get
            {
                IInputElement element = Keyboard.FocusedElement;

                if (element == null)
                {
                    return false;
                }

                return element == this || this.IsAncestorOf(element as DependencyObject);
            }
        }

        public int SelectedIndex
        {
            get
            {
                return this.listBox.SelectedIndex;
            }
            set
            {
                this.listBox.SelectedIndex = value;
            }
        }

        public SuggestionItem GetItem(int index)
        {
            return ((SuggestionItemWrapper)this.listBox.Items[index]).Item;
            //return this.items[index].Item;
        }

        public void EnsureCreated()
        {
        }

        public void ClearItems()
        {
            using (DdMonitor.Lock(this.itemSyncObject))
            {
                this.listBox.Items.Clear();
            }
        }

        public void AddItem(SuggestionItem item)
        {
            using (DdMonitor.Lock(this.itemSyncObject))
            {
#if !NO_LEAK
                this.listBox.Items.Add(new SuggestionItemWrapper(item, this.images));
                //this.items.Add(new SuggestionItemWrapper(item, this.images));
//#else
                //this.items.Add(new SuggestionItemWrapper(new SuggestionItem(SuggestionItemType.Command, "test", -1), this.images));
#endif
            }
        }

        public void CenterSelectedItem()
        {
            this.listBox.CenterSelectedItem();
        }

        public bool SuppressItemInfoToolTips
        {
            get { return false; }
        }

        public int ItemCount
        {
            get { return this.listBox.Items.Count; }
        }

        public void DoPageUpOrDown(Direction direction)
        {
            this.listBox.DoPageUpOrDown(direction);
        }

        public void ScrollToTop()
        {
            if (this.listBox.Items.Count > 0)
            {
                this.listBox.ScrollIntoView(this.listBox.Items[0]);
            }
        }

        public void ScrollToEnd()
        {
            if (this.listBox.Items.Count > 0)
            {
                this.listBox.ScrollIntoView(this.listBox.Items[this.listBox.Items.Count - 1]);
            }
        }

        void ISuggestionProvider.Activate()
        {
            this.Activate();
        }

        public void RefreshThreadSafe()
        {
            using (DdMonitor.Lock(this.itemSyncObject))
            {
                foreach (object item in this.listBox.Items)
                {
                    SuggestionItemWrapper wrapper = item as SuggestionItemWrapper;
                    if (wrapper != null)
                    {
                        wrapper.Refresh();
                    }
                }
            }
        }

        public void ScrollSuggestions(Direction direction)
        {
            this.listBox.Scroll(direction);
        }

        public System.Drawing.Point Location
        {
            get
            {
                return WpfToolkitHost.ConvertPoint(
                    new Point(this.Left, this.Top));
            }
            set
            {
                Point location = WpfToolkitHost.ConvertPoint(value);
                this.Left = location.X;
                this.Top = location.Y;
            }
        }

        public System.Drawing.Size Size
        {
            get
            {
                return WpfToolkitHost.ConvertSize(
                    new Size(this.Width, this.Height));
            }
            set
            {
                Size size = WpfToolkitHost.ConvertSize(value);
                this.Height = size.Height;
                this.Width = size.Width;
            }
        }

        public System.Drawing.Size SaveSize
        {
            get
            {
                if (this.saveSize == null)
                {
                    System.Drawing.Size baseSize = WpfToolkitHost.ConvertSize(335d, 200d);
                    System.Windows.Forms.Padding padding = this.Padding;
                    this.saveSize = new System.Drawing.Size(
                        baseSize.Width - padding.Horizontal,
                        baseSize.Height - padding.Vertical);
                }

                return this.saveSize.Value; 
            }
            set
            {
                this.saveSize = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs("SaveSize"));
                this.OnPropertyChanged(new PropertyChangedEventArgs("DISaveSize"));
            }
        }

        public int MinimumWidth
        {
            get { return WpfToolkitHost.ConvertToPhysicalPixels(16); }
        }

        public new System.Windows.Forms.Padding Padding
        {
            get 
            {
                int topBorderHeight = WpfToolkitHost.ConvertToPhysicalPixels((this.ActualHeight - this.listBox.ScrollViewer.ActualHeight) / 2);
                int leftBorderWidth = WpfToolkitHost.ConvertToPhysicalPixels((this.ActualWidth - this.listBox.ScrollViewer.ActualWidth) / 2);

                return new System.Windows.Forms.Padding(
                    leftBorderWidth,
                    topBorderHeight,
                    leftBorderWidth,
                    topBorderHeight);
            }
        }

        public int ItemHeight
        {
            get
            {
                //if (this.ItemCount >= 0)
                //{
                //    // hack
                //    //return WpfToolkitHost.ConvertToPhysicalPixels(18);
                    return WpfToolkitHost.ConvertToPhysicalPixels(this.listBox.ItemHeight);
                    //ListBoxItem item = (ListBoxItem)this.listBox.ItemContainerGenerator.ContainerFromIndex(0);
                    //return WpfToolkitHost.ConvertToPhysicalPixels(item.ActualHeight);

                    //ListBoxItem item = this.listBox.FirstVisibleItem;
                    //return WpfToolkitHost.ConvertToPhysicalPixels(item.ActualHeight);
                    //return 1;
                //}
                //else
                //{
                //    return 1;
                //}
            }
        }

        public System.Drawing.Rectangle GetItemBounds(int index)
        {
            ListBoxItem item = (ListBoxItem)this.listBox.ItemContainerGenerator.ContainerFromIndex(index);
            if (item == null)
            {
                return new System.Drawing.Rectangle(
                    WpfToolkitHost.ConvertToPhysicalPixels(0),
                    WpfToolkitHost.ConvertToPhysicalPixels(0),
                    WpfToolkitHost.ConvertToPhysicalPixels(this.listBox.ActualWidth),
                    WpfToolkitHost.ConvertToPhysicalPixels(this.ItemHeight));
            }
            else
            {
                Point location = this.PointFromScreen(item.PointToScreen(new Point()));
                return new System.Drawing.Rectangle(
                    WpfToolkitHost.ConvertToPhysicalPixels(location.X),
                    WpfToolkitHost.ConvertToPhysicalPixels(location.Y),
                    WpfToolkitHost.ConvertToPhysicalPixels(item.ActualWidth),
                    WpfToolkitHost.ConvertToPhysicalPixels(item.ActualHeight));
            }
        }

        public void BringToFront()
        {
            this.Activate();
        }

        public object UIObject
        {
            get { return this; }
        }

        private void RaiseUserInteractionFinishedEvent(object sender, EventArgs e)
        {
            this.OnUserInteractionFinished(EventArgs.Empty);
        }

        protected void RaiseSelectedIndexChanged(object sender, EventArgs e)
        {
            this.OnSelectedIndexChanged(EventArgs.Empty);
        }

        protected virtual void OnUserInteractionFinished(EventArgs e)
        {
            EventHandler handler = this.UserInteractionFinished;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        protected virtual void OnSelectedIndexChanged(EventArgs e)
        {
            EventHandler handler = this.SelectedIndexChanged;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        protected override void OnPreviewMouseUp(MouseButtonEventArgs e)
        {
            base.OnPreviewMouseUp(e);
            this.OnUserInteractionFinished(EventArgs.Empty);
        }

        private void HandleMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (this.listBox.HitTestAncestorIsType<ListBoxItem>(Mouse.GetPosition(this.listBox)))
            {
                this.OnMouseDoubleClick(WpfToolkitHost.Convert(e, this));
            }
        }

        //protected override void OnMouseDoubleClick(MouseButtonEventArgs e)
        //{
        //    base.OnMouseDoubleClick(e);
        //    if (!e.Source)
        //    {
        //        this.OnMouseDoubleClick(WpfToolkitHost.Convert(e, this));
        //    }
        //}

        protected override void OnSourceInitialized(EventArgs e)
        {
            this.nativeHelper = new NativeHelper(((HwndSource)PresentationSource.FromVisual(this)).Handle, this);
            base.OnSourceInitialized(e);
        }

        private class NativeHelper : System.Windows.Forms.NativeWindow
        {
            private DefaultSuggestionProvider parent;

            public NativeHelper(IntPtr handle, DefaultSuggestionProvider parent)
            {
                this.AssignHandle(handle);
                this.parent = parent;
            }

            protected override void WndProc(ref System.Windows.Forms.Message m)
            {
                bool callbase = true;
                switch ((WindowsMessages)m.Msg)
                {
                    case WindowsMessages.WM_WINDOWPOSCHANGING:
                        NativeMethods.WindowPos windowPos = (NativeMethods.WindowPos)m.GetLParam(typeof(NativeMethods.WindowPos));
                        if (this.parent.lastHeight == null || Math.Abs(this.parent.lastHeight.Value - windowPos.height) > 1)
                        {
                            int combinedBorderHeight = this.parent.Padding.Vertical;
                            int numberOfVisibleItems = (windowPos.height - combinedBorderHeight) / this.parent.ItemHeight;
                            if (numberOfVisibleItems < 1)
                            {
                                numberOfVisibleItems = 1;
                            }

                            if (numberOfVisibleItems > this.parent.ItemCount)
                            {
                                numberOfVisibleItems = this.parent.ItemCount;
                            }

                            windowPos.height = combinedBorderHeight + (numberOfVisibleItems * this.parent.ItemHeight);
                            System.Runtime.InteropServices.Marshal.StructureToPtr(windowPos, m.LParam, true);
                        }

                        break;
                    case WindowsMessages.WM_NCACTIVATE:
                        m.WParam = (IntPtr)1;
                        break;
                    case WindowsMessages.WM_EXITSIZEMOVE:
                        this.parent.SaveSize = this.parent.Size;//WpfToolkitHost.ConvertSize(this.parent.ActualWidth, this.parent.ActualHeight);
                        this.parent.OnUserInteractionFinished(EventArgs.Empty);
                        break;
                    default:
                        break;
                }

                if (callbase)
                {
                    //try
                    //{
                        base.WndProc(ref m);
                    //}
                    //catch (NullReferenceException)
                    //{
                    //}
                }
            }
        }

        public void EnsureVisible(int index)
        {
            this.listBox.ScrollIntoView(this.listBox.Items[index]);
        }

        private void HandleIsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            this.OnVisibleChanged(EventArgs.Empty);
        }

        private void HandleListBoxDesiredIconSizeChanged(object sender, EventArgs e)
        {
            this.OnDesiredIconSizeChanged(EventArgs.Empty);
        }

        protected virtual void OnMouseDoubleClick(System.Windows.Forms.MouseEventArgs e)
        {
            System.Windows.Forms.MouseEventHandler handler = this.MouseDoubleClick;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        protected virtual void OnVisibleChanged(EventArgs e)
        {
            EventHandler handler = this.VisibleChanged;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        private void OnDesiredIconSizeChanged(EventArgs e)
        {
            EventHandler handler = this.DesiredIconSizeChanged;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        //protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        //{
        //    base.OnRenderSizeChanged(sizeInfo);
        //    if (Mouse.)
        //    {
        //        this.saveSize = WpfToolkitHost.ConvertSize(sizeInfo.NewSize);
        //    }
        //}

        protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            PropertyChangedEventHandler handler = this.PropertyChanged;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        public void ScrollSelectedItemIntoView()
        {
            this.listBox.ScrollIntoView(this.listBox.SelectedItem);
        }
    }
}
