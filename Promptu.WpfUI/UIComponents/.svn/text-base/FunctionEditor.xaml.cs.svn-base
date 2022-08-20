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
    /// Interaction logic for FunctionEditor.xaml
    /// </summary>
    internal partial class FunctionEditor : PromptuWindow, IFunctionEditor
    {
        public FunctionEditor()
        {
            InitializeComponent();
        }

        public string NameLabelText
        {
            set { this.nameLabel.Content = value; }
        }

        public string AssemblyLabelText
        {
            set { this.assemblyLabel.Content = value; }
        }

        public string ClassLabelText
        {
            set { this.classLabel.Content = value; }
        }

        public string MethodLabelText
        {
            set { this.methodLabel.Content = value; }
        }

        public string ReturnsLabelText
        {
            set { this.returnValueLabel.Content = value; }
        }

        public string MainInstructions
        {
            set { this.mainInstructions.Text = value; }
        }

        ITextInput IFunctionEditor.Name
        {
            get { return this.name; }
        }

        public IComboInput Assembly
        {
            get { return this.assembly; }
        }

        public ITextInput Class
        {
            get { return this.@class; }
        }

        public ITextInput Method
        {
            get { return this.method; }
        }

        public IComboInput ReturnValue
        {
            get { return this.returnValue; }
        }

        public IButton TestFunctionButton
        {
            get { return this.testFunctionButton; }
        }

        public IButton OkButton
        {
            get { return this.okButton; }
        }

        public IButton CancelButton
        {
            get { return this.cancelButton; }
        }

        public ICollectionEditor FunctionParameterPanel
        {
            get { return this.parameterEditor; }
        }

        public void CloseWithOK()
        {
            this.DialogResult = true;
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
    }
}
