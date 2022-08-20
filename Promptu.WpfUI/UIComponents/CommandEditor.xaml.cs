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
using System.Windows.Media.Animation;
using Microsoft.Win32.SafeHandles;
using System.Runtime.InteropServices;

namespace ZachJohnson.Promptu.WpfUI.UIComponents
{
    /// <summary>
    /// Interaction logic for CommandEditor.xaml
    /// </summary>
    internal partial class CommandEditor : PromptuWindow, ICommandEditor, IDisposable, IReportDragState
    {
        private Storyboard HideErrorSplitter;
        private Storyboard ShowErrorSplitter;
        private Storyboard ShowParameterItems;
        private Storyboard HideParameterItems;
        private ErrorPanelDisplayMode lastVisibleMode;
        private bool usingEyedropper;
        private Cursor eyedropperCursor;
        private string originalTarget;
        private string originalWorkingDirectory;
        private bool? originalGuessWorkingDirectory;
        private bool? parameterVisibilityState;
        private bool isDragInProgress;
        private ParameterlessVoid restartPromptu;
        private bool lastWasWorkingDirectory;
        private CustomTimer dragLeaveTimer;
        private DragArea previousDragArea;
        //private bool allowClosing;

        private enum DragArea
        {
            None = 0,
            WorkingDirectory,
            EverythingElse
        }

        public CommandEditor()
        {
            InitializeComponent();
            this.errorPanel.DisplayModeChanged += this.HandleErrorListDisplayModeChanged;

            this.HideErrorSplitter = (Storyboard)this.FindResource("HideErrorSplitter");
            this.ShowErrorSplitter = (Storyboard)this.FindResource("ShowErrorSplitter");
            this.HideParameterItems = (Storyboard)this.FindResource("HideParameterItems");
            this.ShowParameterItems = (Storyboard)this.FindResource("ShowParameterItems");

            this.Loaded += this.HandleLoaded;

            FrameworkElementAttachments.SetDisableAnimations(this, true);
            this.errorPanel.DisplayMode = ErrorPanelDisplayMode.Hidden;
            this.SetUIForParameters(false);

            this.eyedropper.MouseDown += this.HandleEyedropperMouseDown;

            this.AttachTo(this.name);
            this.AttachTo(this.executes);
            this.AttachTo(this.arguments);
            this.AttachTo(this.notes);
            this.AttachTo(this.workingDirectory);

            this.AllowDrop = true;

            this.dragLeaveTimer = new CustomTimer(100);
            this.dragLeaveTimer.AutoReset = false;
            this.dragLeaveTimer.Elapsed += this.HandleDragLeaveTimerElasped;
        }

        private void AttachTo(PromptuTextBox textbox)
        {
            textbox.AllowDrop = true;
            textbox.AddHandler(PromptuTextBox.PreviewDragEnterEvent, new DragEventHandler(this.HandlePreviewDragEnter));
            textbox.AddHandler(PromptuTextBox.PreviewDragOverEvent, new DragEventHandler(this.HandlePreviewDragOver));
            textbox.AddHandler(PromptuTextBox.PreviewDragLeaveEvent, new DragEventHandler(this.HandlePreviewDragLeave));
            textbox.AddHandler(PromptuTextBox.PreviewDropEvent, new DragEventHandler(this.HandlePreviewDrop));
        }

        public new ITextInput Name
        {
            get { return this.name; }
        }

        public ParameterlessVoid RestartPromptu
        {
            set { this.restartPromptu = value; }
        }

        public ITextInput Target
        {
            get { return this.executes; }
        }

        public ITextInput Arguments
        {
            get { return this.arguments; }
        }

        public ITextInput WorkingDirectory
        {
            get { return this.workingDirectory; }
        }

        public string NameLabelText
        {
            set
            {
                this.nameLabel.Content = value;
            }
        }

        public string ExecutesLabelText
        {
            set
            {
                this.executesLabel.Content = value;
            }
        }

        public string ArgumentsLabelText
        {
            set
            {
                this.argumentsLabel.Content = value;
            }
        }

        public string StartupStateLabelText
        {
            set
            {
                this.startupStateLabel.Content = value;
            }
        }

        public ICheckBox RunAsAdmin
        {
            get { return this.runAsAdministrator; }
        }

        public IComboInput StartupState
        {
            get { return this.startupState; }
        }

        public string WorkingDirectoryLabelText
        {
            set
            {
                this.workingDirectoryLabel.Content = value;
            }
        }

        public string NotesLabelText
        {
            set
            {
                this.notesLabel.Content = value;
            }
        }

        public ITextInput Notes
        {
            get { return this.notes; }
        }

        public ICheckBox ShowParamHistory
        {
            get { return this.showParameterHistory; }
        }

        public IButton TestButton
        {
            get { return this.testCommandButton; }
        }

        public ICheckBox GuessWorkingDirectory
        {
            get { return this.guessWorkingDirectory; }
        }

        public IButton ViewAvailableFunctionsButton
        {
            get { return this.viewFunctionsButton; }
        }

        public IButton OkButton
        {
            get { return this.okButton; }
        }

        public IButton CancelButton
        {
            get { return this.cancelButton; }
        }

        public ICollectionEditor CommandParameterMetaInfoPanel
        {
            get { return this.commandParameterPanel; }
        }

        public string EyedropperToolTip
        {
            set
            {
            }
        }

        public IErrorPanel ErrorPanel
        {
            get { return this.errorPanel; }
        }

        public void CloseWithOK()
        {
            this.DialogResult = true;
        }

        public bool IsCreatedAndNotDisposing
        {
            get { return true; }
        }

        public void EnsureErrorListVisible()
        {
            this.errorPanel.DisplayMode = ErrorPanelDisplayMode.Normal;
        }

        public bool MustShowErrorList
        {
            set
            {
                if (value)
                {
                    this.errorPanel.DisplayMode = this.lastVisibleMode;
                }
                else
                {
                    this.errorPanel.DisplayMode = ErrorPanelDisplayMode.Hidden;
                }
            }
        }

        public void SetUIForParameters(bool thereAreParameters)
        {
            if (this.parameterVisibilityState != thereAreParameters)
            {
                this.parameterVisibilityState = thereAreParameters;
                if (thereAreParameters)
                {
                    this.ApplyAnimation(this.ShowParameterItems);
                }
                else
                {
                    this.ApplyAnimation(this.HideParameterItems);
                }
            }
        }

        public string Text
        {
            get { return this.Title; }
            set { this.Title = value; }
        }

        public UIModel.UIDialogResult ShowModal()
        {
            return WpfToolkitHost.ShowDialogUIDialogResult(this);
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

        public string MainInstructions
        {
            set { this.mainInstructions.Text = value; }
        }

        public ErrorPanelDisplayMode LastVisibleErrorPanelDisplayMode
        {
            get 
            { 
                return this.lastVisibleMode; 
            }

            set
            {
                if (value != ErrorPanelDisplayMode.Collapsed && value != ErrorPanelDisplayMode.Normal)
                {
                    return;
                }

                this.lastVisibleMode = value;

                if (this.errorPanel.DisplayMode == ErrorPanelDisplayMode.Collapsed ||
                    this.errorPanel.DisplayMode == ErrorPanelDisplayMode.Normal)
                {
                    this.errorPanel.DisplayMode = value;
                }
            }
        }

        public void Dispose()
        {
            this.Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            this.executes.Dispose();
            this.arguments.Dispose();
            this.name.Dispose();
            this.workingDirectory.Dispose();
            this.notes.Dispose();
        }

        private void HandleErrorListDisplayModeChanged(object sender, EventArgs e)
        {
            ErrorPanelDisplayMode currentMode = this.errorPanel.DisplayMode;

            if (currentMode != ErrorPanelDisplayMode.Hidden)
            {
                this.lastVisibleMode = currentMode;
            }

            if (currentMode == ErrorPanelDisplayMode.Normal)
            {
                this.ApplyAnimation(this.ShowErrorSplitter);
            }
            else
            {
                this.ApplyAnimation(this.HideErrorSplitter);
            }
        }

        private void HandleLoaded(object sender, EventArgs e)
        {
            FrameworkElementAttachments.SetDisableAnimations(this, false);
        }

        protected override void OnPreviewDragEnter(DragEventArgs e)
        {
            this.isDragInProgress = true;
            this.HandleDragEnter(e);
            base.OnPreviewDragEnter(e);
        }

        private void HandlePreviewDragEnter(object sender, DragEventArgs e)
        {
            this.isDragInProgress = true;
            this.HandleDragEnter(e);
        }

        private void HandleDragEnter(DragEventArgs e)
        {
            this.HandleDragEvent(e);
        }

        private DragArea AreaMouseIn
        {
            get
            {
                return this.CursorIsInsideWorkingDirectoryArea ? DragArea.WorkingDirectory : DragArea.EverythingElse;
            }
        }

        private void HandleDragEvent(DragEventArgs e)
        {
            this.dragLeaveTimer.Halt();
            DragArea areaIn = this.AreaMouseIn;

            if (this.previousDragArea != DragArea.None)
            {
                if (areaIn != this.previousDragArea)
                {
                    this.HandleAbstractDragLeave(this.previousDragArea);
                }
            }

            if (areaIn != this.previousDragArea)
            {
                this.HandleAbstractDragEnter(areaIn, e);
            }
            else
            {
                this.HandleAbstractDragOver(areaIn, e);
            }

            this.previousDragArea = areaIn;
            e.Handled = true;
            this.dragLeaveTimer.Start();
        }

        private void HandleAbstractDragLeave(DragArea area)
        {
            if (area == DragArea.WorkingDirectory)
            {
                this.workingDirectory.Text = this.originalWorkingDirectory;
                this.guessWorkingDirectory.IsChecked = this.originalGuessWorkingDirectory;
            }
            else
            {
                this.Target.Text = this.originalTarget;
            }
        }

        private void HandleAbstractDragEnter(DragArea area, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                this.originalTarget = this.Target.Text;
                this.originalWorkingDirectory = this.workingDirectory.Text;
                this.originalGuessWorkingDirectory = this.guessWorkingDirectory.IsChecked.Value;

                e.Effects = DragDropEffects.Copy;
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);

                if (files.Length > 0)
                {
                    if (area == DragArea.WorkingDirectory)
                    {
                        this.guessWorkingDirectory.IsChecked = false;
                        this.workingDirectory.Text = files[0];
                    }
                    else
                    {
                        this.Target.Text = files[0];
                    }
                }

                e.Handled = true;
            }
        }

        private void HandleAbstractDragDrop(DragArea area, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);

                if (files.Length > 0)
                {
                    if (area == DragArea.WorkingDirectory)
                    {
                        this.guessWorkingDirectory.IsChecked = false;
                        this.workingDirectory.Text = files[0];
                        this.lastWasWorkingDirectory = false;
                    }
                    else
                    {
                        this.Target.Text = files[0];
                    }
                    e.Handled = true;
                }
            }
        }

        private void HandleAbstractDragOver(DragArea area, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effects = DragDropEffects.Copy;
                e.Handled = true;
            }
        }

        private void HandlePreviewDragOver(object sender, DragEventArgs e)
        {
            this.HandleDragEvent(e);
        }

        protected override void OnDragOver(DragEventArgs e)
        {
            this.HandleDragEvent(e);
            base.OnDragOver(e);
        }

        private bool CursorIsInsideWorkingDirectoryArea
        {
            get
            {
                Point position = CorrectGetPosition(this.workingDirectoryBorder);
                return ! (position.X < 0 || position.Y < 0 || position.Y > this.workingDirectoryBorder.ActualHeight || position.X > this.workingDirectoryBorder.ActualWidth);
            }
        }

        private void HandleDragLeaveTimerElasped(object sender, EventArgs e)
        {
            this.dragLeaveTimer.Halt();
            this.BeginInvoke(new ParameterlessVoid( delegate
            {
                this.HandleAbstractDragLeave(this.previousDragArea);
                this.previousDragArea = DragArea.None;
                //if (CursorIsInsideWorkingDirectoryArea)
                //{
                //    this.workingDirectory.Text = this.originalWorkingDirectory;
                //    this.guessWorkingDirectory.IsChecked = this.originalGuessWorkingDirectory;
                //    this.lastWasWorkingDirectory = false;
                //}
                //else
                //{
                //    this.dragLeaveTimer.Start();
                //}

            }), null);
        }

        public static Point CorrectGetPosition(Visual relativeTo)
        {
            Win32Point w32Mouse = new Win32Point();
            GetCursorPos(ref w32Mouse);
            return relativeTo.PointFromScreen(new Point(w32Mouse.X, w32Mouse.Y));
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct Win32Point
        {
            public Int32 X;
            public Int32 Y;
        };

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool GetCursorPos(ref Win32Point pt);


        //protected override void OnDragEnter(DragEventArgs e)
        //{
        //    base.OnDragEnter(e);
        //}

        private void HandlePreviewDragLeave(object sender, DragEventArgs e)
        {
            this.HandleDragLeave(e);
        }

        protected override void OnPreviewDragLeave(DragEventArgs e)
        {
            this.HandleDragLeave(e);
            //Point location = e.GetPosition(this);
            //if ((location.X < 5 || location.Y < 5 || location.X > this.Width - 10 || location.Y > this.Height - 10)
            //    && e.Data.GetDataPresent(DataFormats.FileDrop))
            //{
            //    this.Target.Text = this.originalTarget;
            //    e.Handled = true;
            //    this.isNewDrag = true;
            //}

            base.OnPreviewDragLeave(e);
        }

        private void HandleDragLeave(DragEventArgs e)
        {
            this.isDragInProgress = false;
            this.HandleDragEnter(e);
        }

        private void HandleDragDrop(DragEventArgs e)
        {
            this.dragLeaveTimer.Halt();
            this.previousDragArea = DragArea.None;
            this.isDragInProgress = false;
            this.HandleAbstractDragDrop(this.AreaMouseIn, e);
        }

        private void HandlePreviewDrop(object sender, DragEventArgs e)
        {
            this.HandleDragDrop(e);
            e.Handled = true;
        }

        protected override void OnPreviewDrop(DragEventArgs e)
        {
            this.HandleDragDrop(e);
            base.OnPreviewDrop(e);
        }

        private void HandleEyedropperMouseDown(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                if (this.eyedropperCursor == null)
                {

                    this.eyedropperCursor = WpfUtilities.ConvertToCursor(
                        //(Brush)this.FindResource("Eyedropper"), 
                        new VisualBrush(this.eyedropper),
                        new Size(this.eyedropper.ActualWidth, this.eyedropper.ActualHeight), 
                        new Point(5, 5));
                }

                this.usingEyedropper = true;
                this.originalTarget = this.Target.Text;
                this.eyedropper.Visibility = System.Windows.Visibility.Hidden;
                //System.Diagnostics.Process.EnterDebugMode();
                Mouse.Capture(this);
                //NativeMethods.SetCursor(this.eyedropperCursor.Handle);
                //System.Windows.Forms.Cursor.Current = this.eyedropperCursor;
                Mouse.OverrideCursor = this.eyedropperCursor;
                //Mouse.SetCursor(this.eyedropperCursor);
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (this.usingEyedropper)
            {
                Point screenLocation = this.PointToScreen(e.GetPosition(this));
                bool executablePathNull;
                string path = WindowLocator.GetPathFromWindowAt(new System.Drawing.Point((int)screenLocation.X, (int)screenLocation.Y), true, out executablePathNull);

                if (executablePathNull)
                {
                    bool isRunningAdmin = Promptu.Utilities.PromptuIsRunningElevated;
                    PromptuTextBoxAttachments.SetUacError(this.executes, !isRunningAdmin);
                    this.Target.GiveTextValidationError(
                        isRunningAdmin ? 
                        Localization.UIResources.ProcessPathAccessIsDeniedElevated : 
                        Localization.UIResources.ProcessPathAccessIsDenied);
                }
                else
                {
                    this.Target.ClearTextValidationError();
                }

                this.Target.Text = path ?? this.originalTarget;
            }

            base.OnMouseMove(e);
        }

        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                if (this.usingEyedropper)
                {
                    this.Target.Text = this.originalTarget;
                    this.StopUsingEyedropper();
                    e.Handled = true;
                }
            }

            base.OnPreviewKeyDown(e);
        }

        private void HandleRestartPromptuClick(object sender, EventArgs e)
        {
            ParameterlessVoid restartPromptu = this.restartPromptu;
            if (restartPromptu != null)
            {
                restartPromptu();
            }
        }

        private void HandleCancelTargetError(object sender, EventArgs e)
        {
            this.Target.ClearTextValidationError();
        }

        private void StopUsingEyedropper()
        {
            if (this.usingEyedropper)
            {
                this.usingEyedropper = false;
                Mouse.Capture(null);
                Mouse.OverrideCursor = null;
                this.eyedropper.Visibility = System.Windows.Visibility.Visible;
                this.executes.DisplayCueIfValid();
                if (Promptu.Utilities.PromptuIsRunningElevated)
                {
                    this.Target.ClearTextValidationError();
                }
                //System.Diagnostics.Process.LeaveDebugMode();
            }
        }

        protected override void OnMouseUp(MouseButtonEventArgs e)
        {
            this.StopUsingEyedropper();
            base.OnMouseUp(e);
        }

        //protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        //{
        //    //if (!this.allowClosing)
        //    //{
        //    //    e.Cancel = true;
        //    //    this.Hide();
        //    //}

        //    base.OnClosing(e);
        //}

        public bool IsDragInProgress
        {
            get { return this.isDragInProgress; }
        }
    }
}
