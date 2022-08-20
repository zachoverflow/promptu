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

namespace ZachJohnson.Promptu.WpfUI.UIComponents
{
    /// <summary>
    /// Interaction logic for FileSystemParameterSuggestionEditor.xaml
    /// </summary>
    internal partial class FileSystemParameterSuggestionEditor : PromptuWindow, IFileSystemParameterSuggestionEditor
    {
        public FileSystemParameterSuggestionEditor()
        {
            InitializeComponent();
        }

        public IErrorPanel ErrorPanel
        {
            get { return this.errorPanel; }
        }

        public IButton OkButton
        {
            get { return this.okButton; }
        }

        public IButton CancelButton
        {
            get { return this.cancelButton; }
        }

        public ITextInput Filter
        {
            get { return this.filter; }
        }

        public string FilterSupplementalInstructions
        {
            set { this.filterSupplement.Text = value; }
        }

        public bool IsCreatedAndNotDisposing
        {
            get { return true; }
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
            set { this.mainInstructions.Content = value; }
        }
    }
}
