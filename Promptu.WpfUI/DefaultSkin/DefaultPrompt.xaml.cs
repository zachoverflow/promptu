//-----------------------------------------------------------------------
// <copyright file="DefaultPrompt.xaml.cs" company="ZachJohnson">
//     Copyright (c) Zach Johnson. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace ZachJohnson.Promptu.WpfUI.DefaultSkin
{
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
    using System.Runtime.CompilerServices;
    using Winforms = System.Windows.Forms;
    using System.Windows.Interop;
    using System.Windows.Media.Animation;
    using System.Drawing.Extensions;
    using System.Windows.Threading;
    using ZachJohnson.Promptu.PluginModel;
    using ZachJohnson.Promptu.Localization;
    using ZachJohnson.Promptu.UIModel;

    /// <summary>
    /// Interaction logic for DefaultPrompt.xaml
    /// </summary>
    internal partial class DefaultPrompt : Window, IPrompt
    {
        private Winforms.MouseEventHandler mouseWheel;
        private Winforms.MouseEventHandler mouseDown;
        private Winforms.MouseEventHandler mouseUp;
        private bool doingMouseAction;
        private MouseActionType mouseActionType;
        private Point lastMouseLocation;
        private OptionPage options;
        private ObjectPropertyCollection savingProperties;
        private bool isCreated;
        private UIModel.PartitionedUIContextMenu contextMenu;
        private UIMenuItem contextCut;
        private UIMenuItem contextCopy;
        private UIMenuItem contextPaste;
        //private Storyboard fadeIn;
        //private Storyboard fadeOut;
        //private System.Drawing.Point? overrideLocation;
        //private System.Drawing.Point locationAtNextShow;

        public DefaultPrompt()
        {
            InitializeComponent();
            this.input.PreviewKeyDown += this.HandleInputKeyDown;

            this.contextMenu = new UIModel.PartitionedUIContextMenu();
            this.input.ContextMenu = (ContextMenu)this.contextMenu.NativeContextMenu;
            this.input.ContextMenuOpening += this.HandleContextMenuOpening;

            this.contextCut = new UIModel.UIMenuItem("DefaultPrompt.Cut", UIResources.CutMenuItemText);
            this.contextCut.Image = InternalGlobals.GuiManager.ToolkitHost.Images.Cut;
            this.contextCut.Click += this.HandleCut;
            this.contextMenu.Top.Items.Add(this.contextCut);

            this.contextCopy = new UIModel.UIMenuItem("DefaultPrompt.Copy", UIResources.CopyMenuItemText);
            this.contextCopy.Image = InternalGlobals.GuiManager.ToolkitHost.Images.Copy;
            this.contextCopy.Click += this.HandleCopy;
            this.contextMenu.Top.Items.Add(this.contextCopy);

            this.contextPaste = new UIModel.UIMenuItem("DefaultPrompt.Paste", UIResources.PasteMenuItemText);
            this.contextPaste.Image = InternalGlobals.GuiManager.ToolkitHost.Images.Paste;
            this.contextPaste.Click += this.HandlePaste;
            this.contextMenu.Top.Items.Add(this.contextPaste);

            this.contextMenu.Top.Items.Add(new UIMenuSeparator("DefaultPrompt.PasteSeparator"));

            FontFamilyConverter converter = new FontFamilyConverter();
            if (converter.ConvertToInvariantString(this.FontFamily).ToUpperInvariant() == "TAHOMA")
            {
                if (WpfUtilities.SegoeUIIsInstalled)
                {
                    this.FontFamily = new FontFamily("Segoe UI");
                }
            }

            this.options = new OptionPage();//new OptionPage(UIResources.DefaultPromptOptionsMainInstructions);
            this.savingProperties = new ObjectPropertyCollection();

            GroupingConversionInfo grouping = new GroupingConversionInfo("group", false);

            BoundObjectProperty<int> fontSize = new BoundObjectProperty<int>(
                "FontSize",
                UIResources.DefaultPromptOptionsFontSizeLabel,
                "ScaledFontSize",
                this,
                null,
                new TextConversionInfo("group", false, null, 30),
                new PointFontSizeConverter());

            BoundObjectProperty<FontFamily> fontFamily = new BoundObjectProperty<FontFamily>(
                "FontFamily",
                UIResources.DefaultPromptFontFamilyLabel,
                "FontFamily",
                this,
                null,
                new TextConversionInfo("group", false, null, 100),
                null);

            BoundObjectProperty<Color> backgroundColor = new BoundObjectProperty<Color>(
                "BackgroundColor",
                UIResources.DefaultPromptBackgroundColorText,
                "BackgroundColor",
                this,
                null,
                grouping,
                null);

            BoundObjectProperty<Color> textColor = new BoundObjectProperty<Color>(
               "TextColor",
               UIResources.DefaultPromptTextColorText,
               "TextColor",
               this,
               null,
               grouping,
               null);

            BoundObjectProperty<double> width = new BoundObjectProperty<double>(
                "Width",
                null,
                "PromptWidth",
                this);

            OptionsGroup appearanceGroup = new OptionsGroup("Appearance", UIResources.DefaultPromptOptionsAppearanceGroup);
            appearanceGroup.Add(fontFamily);
            appearanceGroup.Add(fontSize);
            appearanceGroup.Add(backgroundColor);
            appearanceGroup.Add(textColor);

            this.options.Groups.Add(appearanceGroup);

            this.savingProperties.Add(fontFamily);
            this.savingProperties.Add(fontSize);
            this.savingProperties.Add(backgroundColor);
            this.savingProperties.Add(textColor);
            this.savingProperties.Add(width);

            //this.Visibility = System.Windows.Visibility.Hidden;
            //this.Show();
            //this.Hide();
            //this.fadeIn = (Storyboard)this.FindResource("FadeIn");
            //this.fadeOut = (Storyboard)this.FindResource("FadeOut");
            //this.fadeOut.Completed += this.HandleFadeOutCompleted;
            //this.dropIn.Completed += this.HandleDropInCompleted;
            //WpfToolkitHost.InitializeWindow(this);
        }

        public bool IsCreated
        {
            get { return this.isCreated; }
        }

        void IPrompt.Hide()
        {
            this.Dispatcher.Invoke(new ParameterlessVoid(delegate { }), new TimeSpan(0, 0, 0, 1), DispatcherPriority.ApplicationIdle, null);
            this.Hide();
        }

        public Color BackgroundColor
        {
            get { return ((SolidColorBrush)this.promptBody_Copy.Fill).Color; }
            set { ((SolidColorBrush)this.promptBody_Copy.Fill).Color = value; }
        }

        public Color TextColor
        {
            get 
            { 
                return ((SolidColorBrush)this.input.Foreground).Color; 
            }

            set 
            { 
                ((SolidColorBrush)this.input.Foreground).Color = value; 
                //Color background = new Color();
                //background.A = 0;
                //background.R = value.R;
                //background.G = value.G;
                //background.B = value.B;

                //((SolidColorBrush)this.input.Background).Color = background; 
            }
        }

        public double ScaledFontSize
        {
            get
            {
                return this.essentialWindow.LayoutTransform.Transform(new Point(this.input.FontSize, 0)).X;
            }

            set
            {
                ScaleTransform transform = ((ScaleTransform)((TransformGroup)this.essentialWindow.LayoutTransform).Children[0]);

                double newScale = value / this.input.FontSize;
                transform.ScaleX = newScale;
                transform.ScaleY = newScale;
            }
        }

        public event EventHandler<KeyPressedEventArgs> KeyPressed;

        event Winforms.MouseEventHandler IPrompt.MouseWheel
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            add
            {
                this.mouseWheel = (Winforms.MouseEventHandler)Delegate.Combine(this.mouseWheel, value);
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            remove
            {
                this.mouseWheel = (Winforms.MouseEventHandler)Delegate.Remove(this.mouseWheel, value); 
            }
        }

        event Winforms.MouseEventHandler IPrompt.MouseDown
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            add
            {
                this.mouseDown = (Winforms.MouseEventHandler)Delegate.Combine(this.mouseDown, value);
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            remove
            {
                this.mouseDown = (Winforms.MouseEventHandler)Delegate.Remove(this.mouseDown, value); 
            }
        }

        event Winforms.MouseEventHandler IPrompt.MouseUp
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            add
            {
                this.mouseUp = (Winforms.MouseEventHandler)Delegate.Combine(this.mouseUp, value);
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            remove
            {
                this.mouseUp = (Winforms.MouseEventHandler)Delegate.Remove(this.mouseUp, value);
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

        public System.Drawing.Point Location
        {
            get
            {
                ////Point screen = this.promptBody.PointToScreen(new Point(0, 0));
                Point offset = this.promptBody.TransformToAncestor(this)
                              .Transform(new Point(0, 0));

                double x = this.Left + offset.X;
                double y = this.Top + offset.Y;

                return WpfToolkitHost.ConvertPoint(new Point(x, y));

                //return WpfToolkitHost.ConvertPoint(screen);
            }
            set
            {
                Point offset = this.promptBody.TransformToAncestor(this)
                              .Transform(new Point(0, 0));

                Point converted = WpfToolkitHost.ConvertPoint(value);

                this.Left = converted.X - offset.X;
                this.Top = converted.Y - offset.Y;
            }
        }

        //public System.Drawing.Point LocationAtNextShow
        //{
        //    set { this.locationAtNextShow = value; }
        //}

        public System.Drawing.Size Size
        {
            get
            {
                Point transformed = this.essentialWindow.LayoutTransform.Transform(new Point(this.promptBody.ActualWidth, this.promptBody.ActualHeight));

                return WpfToolkitHost.ConvertSize(new Size(transformed.X, transformed.Y));
            }
            set
            {
                Size converted = WpfToolkitHost.ConvertSize(value);
                this.essentialWindow.Height = (this.essentialWindow.Height - this.promptBody.Height) + converted.Height;
                this.essentialWindow.Width = (this.essentialWindow.Width - this.promptBody.Width) + converted.Width;
            }
        }

        public string Text
        {
            get { return this.input.Text; }
            set { this.input.Text = value; }
        }

        public int SelectionStart
        {
            get { return this.input.SelectionStart; }
            set { this.input.SelectionStart = value; }
        }

        public int SelectionLength
        {
            get { return this.input.SelectionLength; }
            set { this.input.SelectionLength = value; }
        }

        public bool ContainsFocus
        {
            get 
            {
                IInputElement element = System.Windows.Input.Keyboard.FocusedElement;

                if (element == null)
                {
                    return false;
                }

                return element == this || this.IsAncestorOf(element as DependencyObject);
            }
        }

        public void EnsureCreated()
        {
        }

        public void FocusOnTextInput()
        {
            System.Windows.Input.Keyboard.Focus(this.input);
        }

        public object UIObject
        {
            get { return this; }
        }

        void IPrompt.Activate()
        {
            this.Activate();
        }

        private void HandleCut(object sender, EventArgs e)
        {
            this.input.Cut();
        }

        private void HandleCopy(object sender, EventArgs e)
        {
            this.input.Copy();
        }

        private void HandlePaste(object sender, EventArgs e)
        {
            this.OnKeyPressed(new KeyPressedEventArgs(Winforms.Keys.Control | Winforms.Keys.V, false));
        }

        private void HandleContextMenuOpening(object sender, ContextMenuEventArgs e)
        {
            bool textIsSelected = this.input.SelectionLength > 0;
            this.contextCopy.Enabled = textIsSelected;
            this.contextCut.Enabled = textIsSelected;

            this.contextPaste.Enabled = Clipboard.ContainsText();
        }

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            this.OnIPromptMouseDown(WpfToolkitHost.Convert(e, this.promptBody));
            //e.Handled = true;
            base.OnMouseDown(e);
        }

        protected override void OnPreviewMouseDown(MouseButtonEventArgs e)
        {
            Point relativeToTextInput = WpfToolkitHost.RoundOut(e.GetPosition(this.input));
            //Point relativeToThis = e.GetPosition(this);

            ////if (this.Hit(relativeToThis) != this.input)
            //{
            //    this.doingMouseAction = true;
            //    this.lastMouseLocation = e.GetPosition(this.promptBody);
            //    Mouse.Capture(this.promptBody);
            //}

            if (!(relativeToTextInput.X >= 0
                && relativeToTextInput.Y >= 0
                && relativeToTextInput.X < this.input.ActualWidth
                && relativeToTextInput.Y < this.input.ActualHeight))
            {
                this.doingMouseAction = true;
                this.lastMouseLocation = e.GetPosition(this.promptBody);
                Mouse.Capture(this.promptBody);
            }

            base.OnPreviewMouseDown(e);
        }

        protected override void OnMouseUp(MouseButtonEventArgs e)
        {
            this.doingMouseAction = false;
            if (this.mouseActionType == MouseActionType.Move)
            {
                //this.OnLocationOnScreenChanged(EventArgs.Empty);
            }

            Mouse.Capture(null);

            this.OnIPromptMouseUp(WpfToolkitHost.Convert(e, this.promptBody));
            //e.Handled = true;
            base.OnMouseUp(e);
        }

        protected override void OnMouseWheel(MouseWheelEventArgs e)
        {
            this.OnIPromptMouseWheel(WpfToolkitHost.Convert(e, this.promptBody));
            //e.Handled = true;
            base.OnMouseWheel(e);
        }

        protected override void OnPreviewMouseWheel(MouseWheelEventArgs e)
        {
            base.OnPreviewMouseWheel(e);
        }

        protected virtual void OnIPromptMouseWheel(Winforms.MouseEventArgs e)
        {
            Winforms.MouseEventHandler handler = this.mouseWheel;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        protected virtual void OnIPromptMouseDown(Winforms.MouseEventArgs e)
        {
            Winforms.MouseEventHandler handler = this.mouseDown;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        protected virtual void OnIPromptMouseUp(Winforms.MouseEventArgs e)
        {
            Winforms.MouseEventHandler handler = this.mouseUp;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        protected virtual void OnKeyPressed(KeyPressedEventArgs e)
        {
            EventHandler<KeyPressedEventArgs> handler = this.KeyPressed;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        private double PromptWidth
        {
            get 
            {
                return this.essentialWindow.Width;
            }

            set
            {
                double realDifference = value - this.essentialWindow.Width;
                this.essentialWindow.Width += realDifference;
                this.Width += realDifference;
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            Point position = e.GetPosition(this.promptBody);

            if (!this.doingMouseAction)
            {
                
                if (position.X + 10 > this.promptBody.ActualWidth)
                {
                    this.Cursor = Cursors.SizeWE;
                    this.mouseActionType = MouseActionType.Resize;
                }
                else
                {
                    this.Cursor = Cursors.SizeAll;
                    this.mouseActionType = MouseActionType.Move;
                }
            }
            else
            {
                if (this.mouseActionType == MouseActionType.Resize)
                {
                    double difference = position.X - this.lastMouseLocation.X;
                    double proposedWidth = this.promptBody.ActualWidth + difference;

                    if (proposedWidth < 30)
                    {
                        proposedWidth = 30;
                    }
                    else
                    {
                        this.lastMouseLocation = position;
                    }

                    double realDifference = proposedWidth - this.promptBody.ActualWidth;
                    this.essentialWindow.Width += realDifference;
                    this.Width += realDifference;

                    //double realDifference = proposedWidth - this.promptBody.ActualWidth;
                    //this.essentialWindow.Width += realDifference;
                    //this.Width += realDifference;
                    //var oldSize = this.Size;
                    //this.Size = new System.Drawing.Size(
                    //    oldSize.Width + WpfToolkitHost.ConvertToPhysicalPixels(realDifference),
                    //    oldSize.Height);
                }
                else
                {
                    this.Location += WpfToolkitHost.ConvertSize(
                        position.X - this.lastMouseLocation.X,
                        position.Y - this.lastMouseLocation.Y);
                }
            }
            

            //Point relativeToTextInput = e.GetPosition(this.input);

            //if (relativeToTextInput.X >= 0 
            //    && relativeToTextInput.Y >= 0 
            //    && relativeToTextInput.X <= this.input.ActualWidth 
            //    && relativeToTextInput.Y <= this.input.ActualHeight)
            //{
            //    return;
            //}

            //

            
        }

        //private void ForwardMouseWheel(object sender, MouseWheelEventArgs e)
        //{
        //    this.OnIPromptMouseWheel(WpfToolkitHost.Convert(e, this.promptBody));
        //}

        //private void ForwardMouseDown(object sender, MouseButtonEventArgs e)
        //{
        //    this.OnIPromptMouseDown(WpfToolkitHost.Convert(e, this.promptBody));
        //}

        //private void ForwardMouse

        private void HandleInputKeyDown(object sender, KeyEventArgs e)
        {
            ModifierKeys modifiers = System.Windows.Input.Keyboard.Modifiers;
            KeyPressedEventArgs eventArgs = new KeyPressedEventArgs(
                WpfToolkitHost.ConvertKey(e.Key, modifiers),
                (modifiers & ModifierKeys.Windows) == ModifierKeys.Windows);

            this.OnKeyPressed(eventArgs);
            e.Handled = eventArgs.Cancel;
        }

        //void IPrompt.Show()
        //{
        //    //System.Drawing.Point locationAtNextShow = this.locationAtNextShow;
        //    //if (!this.IsVisible)
        //    //{
        //    //    this.Location = locationAtNextShow;
        //    //    System.Drawing.Point location = this.Location;
        //    //    Winforms.Screen screen = Winforms.Screen.FromPoint(location);

        //    //    //int currentScreenWidth = WpfToolkitHost.ConvertToPhysicalPixels(this.ActualWidth);
        //    //    int currentScreenHeight = WpfToolkitHost.ConvertToPhysicalPixels(this.ActualHeight);

        //    //    int differenceInHeight = WpfToolkitHost.ConvertToPhysicalPixels(this.Top) - screen.WorkingArea.Top;

        //    //    int newHeight = currentScreenHeight + differenceInHeight;

        //    //    this.Top = screen.WorkingArea.Top;
        //    //    this.Height = WpfToolkitHost.ConvertToDeviceIndependentPixels(newHeight);

        //    //    Canvas.SetTop(this.essentialWindow, Canvas.GetTop(this.essentialWindow) + WpfToolkitHost.ConvertToDeviceIndependentPixels(differenceInHeight));
        //    //    this.overrideLocation = location;
        //    //    this.Show();
        //    //    this.BeginStoryboard(this.dropIn);
        //    //}
        //    //else
        //    //{
        //    //    this.Location = locationAtNextShow;
        //    //    //this.Hide();

        //    //    //Winforms.Screen screen = Winforms.Screen.FromPoint(locationAtNextShow);

        //    //    ////Point offset = this.promptBody.TransformToAncestor(this)
        //    //    ////              .Transform(new Point(0, 0));

        //    //    //Point oldScreenLocation = this.essentialWindow.PointToScreen(new Point());

        //    //    //int xPadding = 300;//WpfToolkitHost.ConvertToPhysicalPixels(Canvas.GetLeft(this.essentialWindow));
        //    //    //int yPadding = 300;//WpfToolkitHost.ConvertToPhysicalPixels(this.ActualHeight - Canvas.GetTop(this.essentialWindow));

        //    //    //System.Drawing.Rectangle newRect = new System.Drawing.Rectangle(
        //    //    //    locationAtNextShow.X - xPadding,
        //    //    //    screen.WorkingArea.Top,
        //    //    //    WpfToolkitHost.ConvertToPhysicalPixels(this.ActualWidth),
        //    //    //    (locationAtNextShow.Y + yPadding) - screen.WorkingArea.Top);

        //    //    //System.Drawing.Rectangle oldRect = new System.Drawing.Rectangle(
        //    //    //    WpfToolkitHost.ConvertPoint(new Point(this.Top, this.Left)),
        //    //    //    WpfToolkitHost.ConvertSize(this.ActualWidth, this.ActualHeight));

        //    //    //System.Drawing.Rectangle boundingRect = newRect.GetBoundingRectangleWith(oldRect);

        //    //    //this.Top = WpfToolkitHost.ConvertToPhysicalPixels(boundingRect.Top);
        //    //    //this.Left = WpfToolkitHost.ConvertToPhysicalPixels(boundingRect.Left);
        //    //    //this.Width = WpfToolkitHost.ConvertToPhysicalPixels(boundingRect.Width);
        //    //    //this.Height = WpfToolkitHost.ConvertToPhysicalPixels(boundingRect.Height);

        //    //    //Point innerLocation = this.PointFromScreen(WpfToolkitHost.ConvertPoint(locationAtNextShow));

        //    //    //Storyboard transformStoryboard = new Storyboard();
        //    //    //transformStoryboard.FillBehavior = FillBehavior.HoldEnd;

        //    //    //DoubleAnimation topAnimation = new DoubleAnimation();
        //    //    //topAnimation.To = innerLocation.Y;
        //    //    //topAnimation.BeginTime = new TimeSpan(0, 0, 0);
        //    //    //topAnimation.Duration = new TimeSpan(0, 0, 0, 0, 100);
        //    //    //Storyboard.SetTargetProperty(topAnimation, new PropertyPath("(Canvas.Top)"));
        //    //    //Storyboard.SetTargetName(topAnimation, "essentialWindow");
        //    //    //transformStoryboard.Children.Add(topAnimation);

        //    //    //DoubleAnimation leftAnimation = new DoubleAnimation();
        //    //    //leftAnimation.To = innerLocation.X;
        //    //    //leftAnimation.BeginTime = new TimeSpan(0, 0, 0);
        //    //    //leftAnimation.Duration = new TimeSpan(0, 0, 0, 0, 100);
        //    //    //Storyboard.SetTargetProperty(leftAnimation, new PropertyPath("(Canvas.Left)"));
        //    //    //Storyboard.SetTargetName(leftAnimation, "essentialWindow");
        //    //    //transformStoryboard.Children.Add(leftAnimation);

        //    //    //Point oldRelativeLocation = this.PointFromScreen(oldScreenLocation);

        //    //    //Canvas.SetLeft(this.essentialWindow, oldRelativeLocation.X);
        //    //    //Canvas.SetTop(this.essentialWindow, oldRelativeLocation.Y);

        //    //    //this.BeginStoryboard(transformStoryboard);
        //    //}
        //    //if (!this.IsVisible)
        //    //{
        //    //    this.BeginStoryboard(this.fadeIn);
        //    //}

        //    this.Show();
        //}

        //void IPrompt.Hide()
        //{
        //    this.Hide();
        //    //this.fadeOut.Begin(this, true);
        //}

        //private void HandleFadeOutCompleted(object sender, EventArgs e)
        //{
        //    this.Hide();
        //    this.fadeOut.Remove(this);
        //}
        protected override void OnSourceInitialized(EventArgs e)
        {
            this.isCreated = true;
            MouseWheelIntercept intercept = new MouseWheelIntercept(this);
            base.OnSourceInitialized(e);
        }

        // Use a little Win32 hackery to get the mouse wheel event
        // while focused even when the mouse is not over the window
        private class MouseWheelIntercept : System.Windows.Forms.NativeWindow
        {
            private DefaultPrompt owner;

            public MouseWheelIntercept(DefaultPrompt owner)
            {
                this.owner = owner;
                WindowInteropHelper helper = new WindowInteropHelper(owner);

                this.AssignHandle(helper.Handle);
            }

            protected override void WndProc(ref Winforms.Message m)
            {
                bool callBase = true;
                switch ((WindowsMessages)m.Msg)
                {
                    case WindowsMessages.WM_MOUSEWHEEL:
                        // The delta is only the important thing to Promptu, 
                        // that is the only thing needed to be parsed from 
                        // the message to send along

                        short delta = WpfUtilities.HIWORD((int)m.WParam);

                        Winforms.MouseEventArgs eventArgs = new Winforms.MouseEventArgs(
                            Winforms.MouseButtons.None, 
                            0, 
                            0, 
                            0, 
                            delta);

                        this.owner.OnIPromptMouseWheel(eventArgs);
                        m.Result = IntPtr.Zero;
                        callBase = false;
                        break;
                    default:
                        break;
                }

                if (callBase)
                {
                    base.WndProc(ref m);
                }
            }
        }

        public UIModel.UIContextMenu PromptContextMenu
        {
            get { return this.contextMenu.Center; }
        }
    }
}
