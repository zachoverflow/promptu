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
using System.Windows.Navigation;
using System.Windows.Shapes;
using ZachJohnson.Promptu.UIModel.Interfaces;
using System.Collections.ObjectModel;
using System.Windows.Media.Animation;

namespace ZachJohnson.Promptu.WpfUI.UIComponents
{
    /// <summary>
    /// Interaction logic for ErrorPanel.xaml
    /// </summary>
    internal partial class ErrorPanel : UserControl, IErrorPanel
    {
        //public static readonly RoutedEvent CloseButtonClickEvent =
        //    EventManager.RegisterRoutedEvent(
        //    "CloseButtonClickEvent",
        //    RoutingStrategy.Bubble,
        //    typeof(ParameterlessVoid),
        //    typeof(ErrorPanel));
        

        public static readonly DependencyProperty DisplayModeProperty =
            DependencyProperty.Register(
                "DisplayMode",
                typeof(ErrorPanelDisplayMode),
                typeof(ErrorPanel),
                new PropertyMetadata(HandleDisplayModePropertyChanged));

        public static readonly DependencyProperty SpecialOpacityProperty =
            DependencyProperty.Register(
                "SpecialOpacity",
                typeof(double),
                typeof(ErrorPanel),
                new PropertyMetadata(1.0d));

        public static readonly DependencyProperty ExpandButtonVisibilityProperty =
            DependencyProperty.Register(
                "ExpandButtonVisibility",
                typeof(Visibility),
                typeof(ErrorPanel),
                new PropertyMetadata(Visibility.Visible));

        private readonly Storyboard ToCollapsed;
        private readonly Storyboard ToHidden;
        private readonly Storyboard ToFullyFromHidden;
        private readonly Storyboard ToFullyFromCollapsed;
        private readonly Storyboard ToCollapsedFromHidden;
        private bool showAnimations;
        //private double realHeight;

        private BulkObservableCollection<FeedbackMessage> feedbackMessages;

        public ErrorPanel()
        {
            InitializeComponent();
            this.feedbackMessages = new BulkObservableCollection<FeedbackMessage>();
            this.messagesGrid.DataContext = feedbackMessages;
            this.messagesGrid.SelectionChanged += this.RaiseItemActivated;

            this.ToCollapsed = (Storyboard)this.FindResource("ToCollapsed");
            this.ToHidden = (Storyboard)this.FindResource("ToHidden");
            this.ToFullyFromHidden = (Storyboard)this.FindResource("ToFullyFromHidden");
            this.ToFullyFromCollapsed = (Storyboard)this.FindResource("ToFullyFromCollapsed");
            this.ToCollapsedFromHidden = (Storyboard)this.FindResource("ToCollapsedFromHidden");

            //this.CloseStoryboard.Completed += this.HandleCloseFinished;
            //this.OpenStoryboard.Completed += this.HandleOpenFinished;
            //FeedbackMessage temp = new FeedbackMessage("blahblahblah this is a really interesting error message that goes on and on", FeedbackType.Error, 0, 0, true);
            //temp.Location ="somewhere";
            //FeedbackMessage temp2 = new FeedbackMessage("blahblahblah this is a really interesting warning message that goes on and on", FeedbackType.Warning, 0, 0, true);
            //temp2.Location = "over";
            //FeedbackMessage temp3 = new FeedbackMessage("blahblahblah this is a really interesting error message that goes on and on", FeedbackType.Message, 0, 0, true);
            //temp3.Location = "the rainbow";
            //this.feedbackMessages.Add(temp);
            //this.feedbackMessages.Add(temp2);
            //this.feedbackMessages.Add(temp3);
            this.closeButton.Click += this.HandleCloseButtonClick;

            this.descriptionColumn.Header = Localization.UIResources.DescriptionColumnHeader;
            this.locationColumn.Header = Localization.UIResources.LocationColumnHeader;
            this.positionColumn.Header = Localization.UIResources.PositionColumnHeader;
        }

        public event EventHandler ItemActivated;

        public event EventHandler DisplayModeChanged;

        public int GetIndexOf(FeedbackMessage message)
        {
            return this.feedbackMessages.IndexOf(message);
        }

        public Visibility ExpandButtonVisibility
        {
            get { return (Visibility)this.GetValue(ExpandButtonVisibilityProperty); }
            set { this.SetValue(ExpandButtonVisibilityProperty, value); }
        }

        public bool ShowAnimations
        {
            get { return this.showAnimations; }
            set { this.showAnimations = value; }
        }

        public double SpecialOpacity
        {
            get { return (double)this.GetValue(SpecialOpacityProperty); }
            set { this.SetValue(SpecialOpacityProperty, value); }
        }

        public ErrorPanelDisplayMode DisplayMode
        {
            get { return (ErrorPanelDisplayMode)this.GetValue(DisplayModeProperty); }
            set { this.SetValue(DisplayModeProperty, value); }
        }

        internal BulkObservableCollection<FeedbackMessage> FeedbackMessages
        {
            get { return this.feedbackMessages; }
        }

        public string Caption
        {
            get
            {
                return this.titleLabel.Text;
            }
            set
            {
                this.titleLabel.Text = value;
            }
        }

        public ICheckBoxButton ErrorsButton
        {
            get { return this.errorsButton; }
        }

        public ICheckBoxButton WarningsButton
        {
            get { return this.warningsButton; }
        }

        public ICheckBoxButton MessagesButton
        {
            get { return this.messagesButton; }
        }

        public int PrimarySelectedIndex
        {
            get { return this.messagesGrid.SelectedIndex; }
        }

        public bool SomethingIsSelected
        {
            get { return this.messagesGrid.SelectedItems.Count > 0; }
        }

        public void SetMessages(FeedbackCollection messages)
        {
            this.feedbackMessages.Repopulate(messages);
        }

        private static void HandleDisplayModePropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            ErrorPanel panel = obj as ErrorPanel;
            if (obj != null)
            {
                panel.HandleDisplayModeChanged(
                    (ErrorPanelDisplayMode)e.OldValue, 
                    (ErrorPanelDisplayMode)e.NewValue);
            }
        }

        //private void ApplyAnimation(Storyboard storyboard)
        //{
        //    if (this.ShowAnimations)
        //    {
        //        this.BeginStoryboard(storyboard);
        //    }
        //    else
        //    {
        //        storyboard.Begin(this, true);
        //        storyboard.SkipToFill(this);
        //    }
        //}

        private void HandleDisplayModeChanged(ErrorPanelDisplayMode oldValue, ErrorPanelDisplayMode newValue)
        {
            switch (newValue)
            {
                case ErrorPanelDisplayMode.Normal:
                    Custom.SetDrawingBackground(this.closeButton, this.FindResource("ExpandDown"));
                    if (oldValue == ErrorPanelDisplayMode.Collapsed)
                    {
                        this.ApplyAnimation(this.ToFullyFromCollapsed);
                    }
                    else if (oldValue == ErrorPanelDisplayMode.Hidden)
                    {
                        this.ApplyAnimation(this.ToFullyFromHidden);
                    }

                    break;
                case ErrorPanelDisplayMode.Collapsed:
                    Custom.SetDrawingBackground(this.closeButton, this.FindResource("ExpandUp"));
                    if (oldValue == ErrorPanelDisplayMode.Hidden)
                    {
                        this.ApplyAnimation(this.ToCollapsedFromHidden);
                    }
                    else
                    {
                        this.ApplyAnimation(ToCollapsed);
                    }

                    break;
                case ErrorPanelDisplayMode.Hidden:
                    this.ApplyAnimation(ToHidden);
                    break;
                default:
                    break;
            }

            this.OnDisplayModeChanged(EventArgs.Empty);
        }

        protected virtual void OnDisplayModeChanged(EventArgs e)
        {
            EventHandler handler = this.DisplayModeChanged;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        private void RaiseItemActivated(object sender, EventArgs e)
        {
            this.OnItemActivated(EventArgs.Empty);
        }

        private void HandleCloseButtonClick(object sender, EventArgs e)
        {
            if (this.DisplayMode != ErrorPanelDisplayMode.Collapsed)
            {
                this.DisplayMode = ErrorPanelDisplayMode.Collapsed;
            }
            else
            {
                this.DisplayMode = ErrorPanelDisplayMode.Normal;
            }

            //this.OnCloseButtonClick(EventArgs.Empty);
        }

        protected virtual void OnItemActivated(EventArgs e)
        {
            EventHandler handler = this.ItemActivated;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        //protected virtual void OnCloseButtonClick(EventArgs e)
        //{
        //    EventHandler handler = this.CloseButtonClick;
        //    if (handler != null)
        //    {
        //        handler(this, e);
        //    }
        //}

        //private void HandleCloseFinished(object sender, EventArgs e)
        //{
            
        //}

        //private void HandleOpenFinished(object sender, EventArgs e)
        //{
        //    //ButtonAttachments.SetDrawingBackground(this.closeButton, this.FindResource("ExpandUp"));
        //}
    }
}
