using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using ZachJohnson.Promptu.UIModel.Interfaces;
using System.Windows.Input;
using System.Runtime.CompilerServices;
using System.Drawing;
using System.Windows;
using System.Windows.Documents;
using System.Reflection;
using System.Windows.Data;

namespace ZachJohnson.Promptu.WpfUI.UIComponents
{
    public class PromptuTextBox : TextBox, ITextInput, IDisposable
    {
        //private TextBox mainTextBox;
        //private TextBox cueTextBox;
        private delegate T Getter<T>();
        private Validator<string> textValidator;
        public static readonly DependencyProperty CueDisplayedProperty;
        public static readonly DependencyProperty ValidatedTextProperty;
        public static readonly DependencyProperty CueProperty;
        //public static readonly DependencyProperty TextProperty;
        //public static readonly DependencyProperty TextWrappingProperty;
        //public static readonly DependencyProperty ErrorDisplayTemplateProperty;
        //public static readonly DependencyProperty CueFontFamilyProperty;
        //public static readonly DependencyProperty CueFontStyleProperty;
        //public static readonly DependencyProperty CueFontWeightProperty;
        //public static readonly DependencyProperty CueForegroundProperty;
        //private string cue;
        private System.Windows.Forms.KeyEventHandler keyDown;
        private System.Timers.Timer mouseUpTimer;
        private EventHandler textChanged;
        private TextValidationRule textValidationRule;

        static PromptuTextBox()
        {
            CueDisplayedProperty = DependencyProperty.Register(
                "CueDisplayed",
                typeof(bool),
                typeof(PromptuTextBox),
                new PropertyMetadata(false));

            ValidatedTextProperty = DependencyProperty.Register(
                "ValidatedText",
                typeof(string),
                typeof(PromptuTextBox));

            CueProperty = DependencyProperty.Register(
                "Cue",
                typeof(string),
                typeof(PromptuTextBox));

            //TextProperty = DependencyProperty.Register(
            //    "Text",
            //    typeof(string),
            //    typeof(WpfTextInput));

            //TextWrappingProperty = DependencyProperty.Register(
            //    "TextWrapping",
            //    typeof(TextWrapping),
            //    typeof(WpfTextInput));

            //ErrorDisplayTemplateProperty = DependencyProperty.Register(
            //    "ErrorDisplayTemplate",
            //    typeof(ControlTemplate),
            //    typeof(WpfTextInput));

            //CueFontFamilyProperty = DependencyProperty.Register(
            //    "CueFontFamily",
            //    typeof(FontFamily),
            //    typeof(WpfTextInput));

            //CueFontStyleProperty = DependencyProperty.Register(
            //    "CueFontStyle",
            //    typeof(System.Windows.FontStyle),
            //    typeof(WpfTextInput));

            //CueFontWeightProperty = DependencyProperty.Register(
            //    "CueFontWeight",
            //    typeof(System.Windows.FontWeight),
            //    typeof(WpfTextInput));

            //CueForegroundProperty = DependencyProperty.Register(
            //    "CueForeground",
            //    typeof(Brush),
            //    typeof(WpfTextInput));
        }

        //public ControlTemplate ErrorDisplayTemplate
        //{
        //    get { return (ControlTemplate)this.GetValue(ErrorDisplayTemplateProperty); }
        //    set { this.SetValue(ErrorDisplayTemplateProperty, value); }
        //}

        public PromptuTextBox()
        {
            //this.mainTextBox = new TextBox();
            //this.cueTextBox = new TextBox();
            //this.cueTextBox.Visibility = Visibility.Collapsed;
            //this.cueTextBox.Style = (Style)this.FindResource("CueTextBoxStyle");
            //this.cueTextBox.DataContext = this;

            this.Loaded += this.HandleLoaded;
            this.mouseUpTimer = new System.Timers.Timer(1000);
            this.mouseUpTimer.Elapsed += this.HandleTimerElasped;
            Binding textBinding = new Binding("ValidatedText");
            textBinding.Source = this;
            textBinding.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
            this.textValidationRule = new TextValidationRule(this);
            textBinding.ValidationRules.Add(this.textValidationRule);
            textBinding.Mode = BindingMode.TwoWay;

            this.SetBinding(TextBox.TextProperty, textBinding);
        }

        public string ValidatedText
        {
            get { return (string)this.GetValue(ValidatedTextProperty); }
            set { this.SetValue(ValidatedTextProperty, value); }
        }

        //public string Text
        //{
        //    get { return (string)this.GetValue(TextProperty); }
        //    set { this.SetValue(TextProperty, value); }
        //}

        //public TextWrapping TextWrapping
        //{
        //    get { return (TextWrapping)this.GetValue(TextWrappingProperty); }
        //    set { this.SetValue(TextWrappingProperty, value); }
        //}

        public Validator<string> TextValidator
        {
            get { return this.textValidator; }
            set { this.textValidator = value; }
        }

        event System.Windows.Forms.KeyEventHandler ITextInput.KeyDown
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            add
            {
                this.keyDown = (System.Windows.Forms.KeyEventHandler)Delegate.Combine(this.keyDown, value);
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            remove
            {
                this.keyDown = (System.Windows.Forms.KeyEventHandler)Delegate.Remove(this.keyDown, value);
            }
        }

        event EventHandler ITextInput.TextChanged
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            add
            {
                this.textChanged = (EventHandler)Delegate.Combine(this.textChanged, value);
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            remove
            {
                this.textChanged = (EventHandler)Delegate.Remove(this.textChanged, value);
            }
        }

        string ITextInput.Text
        {
            get
            {
                if (this.Dispatcher.CheckAccess())
                {
                    return this.GetText();
                }
                else
                {
                    return (string)this.Dispatcher.Invoke(new Getter<string>(this.GetText));
                }
            }
            set
            {
                if (this.CueDisplayed)
                {
                    this.HideCue();
                }

                this.ValidatedText = value;

                this.DisplayCueIfValid();
            }
        }

        private string GetText()
        {
            if (this.CueDisplayed)
            {
                return string.Empty;
            }

            return this.ValidatedText;
        }

        //public void ShowError()
        //{
        //    AdornerLayer layer = AdornerLayer.GetAdornerLayer(this);
        //}

        //public FontFamily CueFontFamily
        //{
        //    get { return (FontFamily)this.GetValue(CueFontFamilyProperty); }
        //    set { this.SetValue(CueFontFamilyProperty, value); }
        //}

        //public System.Windows.FontStyle CueFontStyle
        //{
        //    get { return (System.Windows.FontStyle)this.GetValue(CueFontStyleProperty); }
        //    set { this.SetValue(CueFontStyleProperty, value); }
        //}

        //public FontWeight CueFontWeight
        //{
        //    get { return (FontWeight)this.GetValue(CueFontWeightProperty); }
        //    set { this.SetValue(CueFontWeightProperty, value); }
        //}

        //public Brush CueForeground
        //{
        //    get { return (Brush)this.GetValue(CueForegroundProperty); }
        //    set { this.SetValue(CueForegroundProperty, value); }
        //}

        public bool Enabled
        {
            get { return this.IsEnabled; }
            set { this.IsEnabled = value; }
        }

        public string Cue
        {
            get { return (string)this.GetValue(CueProperty); }
            set { this.SetValue(CueProperty, value); }
        }

        public bool CueDisplayed
        {
            get { return (bool)this.GetValue(CueDisplayedProperty); }
            private set { this.SetValue(CueDisplayedProperty, value); }
        }

        //public bool Multiline
        //{
        //    get { return this.AcceptsReturn; }
        //}

        public int? PhysicalWidth
        {
            set { throw new NotImplementedException(); }
        }

        public void Select()
        {
            Keyboard.Focus(this);
        }

        public void Dispose()
        {
            this.Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            this.mouseUpTimer.Dispose();
        }

        private void HandleLoaded(object sender, EventArgs e)
        {
            this.DisplayCueIfValid();
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            base.OnKeyUp(e);
            this.DisplayCueIfValid();
        }

        protected override void OnGotFocus(RoutedEventArgs e)
        {
            base.OnGotFocus(e);
            if (this.CueDisplayed)
            {
                this.SelectionStart = 0;
                this.SelectionLength = 0;
            }
        }

        private void HandleTimerElasped(object sender, EventArgs e)
        {
            if (!this.Dispatcher.CheckAccess())
            {
                this.Dispatcher.BeginInvoke(new ParameterlessVoid(this.DisplayCueIfValid));
            }
            else
            {
                this.DisplayCueIfValid();
            }
        }

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            this.mouseUpTimer.Stop();
            base.OnMouseDown(e);
            this.HideCue();
        }

        protected override void OnMouseUp(MouseButtonEventArgs e)
        {
            base.OnMouseUp(e);
            this.mouseUpTimer.Stop();
            this.mouseUpTimer.Start();
        }

        protected override void OnLostFocus(RoutedEventArgs e)
        {
            base.OnLostFocus(e);
            this.DisplayCueIfValid();
        }

        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            ModifierKeys modifiers = Keyboard.Modifiers;

            System.Windows.Forms.KeyEventArgs eventArgs =
                new System.Windows.Forms.KeyEventArgs(WpfToolkitHost.ConvertKey(e.Key, modifiers));

            this.OnITextInputKeyDown(eventArgs);
            e.Handled = eventArgs.SuppressKeyPress || eventArgs.Handled;

            switch (e.Key)
            {
                case Key.RightShift:
                case Key.LeftShift:
                case Key.LeftCtrl:
                case Key.RightCtrl:
                case Key.RightAlt:
                case Key.LeftAlt:
                case Key.Tab:
                case Key.Back:
                case Key.Enter:
                case Key.LWin:
                case Key.RWin:
                    break;
                case Key.Delete:
                    if (this.CueDisplayed)
                    {
                        e.Handled = true;
                    }

                    break;
                default:
                    this.HideCue();
                    break;
            }

            base.OnPreviewKeyDown(e);
        }

        //protected override void OnKeyDown(KeyEventArgs e)
        //{
        //    base.OnKeyDown(e);
        //    switch (e.KeyCode & Keys.KeyCode)
        //    {
        //        case Keys.RShiftKey:
        //        case Keys.LShiftKey:
        //        case Keys.ShiftKey:
        //        case Keys.LControlKey:
        //        case Keys.RControlKey:
        //        case Keys.ControlKey:
        //        case Keys.Menu:
        //        case Keys.LMenu:
        //        case Keys.RMenu:
        //        case Keys.Tab:
        //        case Keys.Back:
        //        case Keys.Enter:
        //            break;
        //        case Keys.Delete:
        //            if (this.CueDisplayed)
        //            {
        //                e.SuppressKeyPress = true;
        //            }

        //            break;
        //        default:
        //            this.HideMessage();
        //            break;
        //    }
        //}

        protected virtual void OnITextInputKeyDown(System.Windows.Forms.KeyEventArgs e)
        {
            System.Windows.Forms.KeyEventHandler handler = this.keyDown;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        protected override void OnTextChanged(TextChangedEventArgs e)
        {
            base.OnTextChanged(e);
            this.OnITextInputTextChanged(EventArgs.Empty);
        }

        protected virtual void OnITextInputTextChanged(EventArgs e)
        {
            EventHandler handler = this.textChanged;
            if (textChanged != null)
            {
                handler(this, e);
            }
        }

        public void DisplayCueIfValid()
        {
            if (!this.CueDisplayed && this.Text.Length <= 0 && !string.IsNullOrEmpty(this.Cue))
            {
                
                AdornerLayer layer = AdornerLayer.GetAdornerLayer(this);
                if (layer == null)
                {
                    return;
                }

                this.CueDisplayed = true;
                layer.Add(new TextBoxCueAdorner(this, this.Cue));


                //base.Text = this.cue;
                //if (this.IsLoaded)
                //{
                //    WpfInternalUndoManager undoManager = WpfInternalUndoManager.GetUndoManager(this);
                //    try
                //    {
                //        //this.LockCurrentUndoUnit();
                //        //undoManager.PopUndoStack();
                //    }
                //    catch (TargetInvocationException)
                //    {
                //    }
                //}

                //this.IsUndoEnabled = false;
                //base.Text = this.cue;
                //this.IsUndoEnabled = true;
            }
        }

        public void HideCue()
        {
            if (this.CueDisplayed)
            {
                this.CueDisplayed = false;
                AdornerLayer layer = AdornerLayer.GetAdornerLayer(this);

                Adorner[] adorners = layer.GetAdorners(this);
                if (adorners == null)
                {
                    return;
                }

                foreach (Adorner adorner in adorners)
                {
                    if (adorner is TextBoxCueAdorner)
                    {
                        adorner.Visibility = Visibility.Hidden;
                        layer.Remove(adorner);
                    }
                }

                //base.Text = string.Empty;
                //if (this.IsLoaded)
                //{
                //    WpfInternalUndoManager undoManager = WpfInternalUndoManager.GetUndoManager(this);
                //    try
                //    {
                //        //this.LockCurrentUndoUnit();
                //        //undoManager.PopUndoStack();
                //    }
                //    catch (TargetInvocationException)
                //    {
                //    }
                //}
            }
        }

        public void GiveTextValidationError(string message)
        {
            BindingExpression textBinding = this.GetBindingExpression(TextProperty);
            ValidationError error = new ValidationError(
                this.textValidationRule,
                textBinding,
                message,
                null);

            Validation.MarkInvalid(textBinding, error);
        }

        public void ClearTextValidationError()
        {
            Validation.ClearInvalid(this.GetBindingExpression(TextProperty));
        }

        protected override void OnPreviewDragEnter(DragEventArgs e)
        {
            base.OnPreviewDragEnter(e);
        }

        protected override void OnPreviewDragLeave(DragEventArgs e)
        {
            base.OnPreviewDragLeave(e);
        }

        protected override void OnPreviewDrop(DragEventArgs e)
        {
            base.OnPreviewDrop(e);
        }

        //protected override void OnPreviewDragOver(DragEventArgs e)
        //{
        //    base.OnPreviewDragOver(e);
        //    e.Handled = false;
        //}

        //protected override void OnDragEnter(DragEventArgs e)
        //{
        //    base.OnDragEnter(e);
        //    e.Handled = false;
        //}

        //protected override void OnDragLeave(DragEventArgs e)
        //{
        //    base.OnDragLeave(e);
        //    e.Handled = false;
        //}
    }
}
