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
    /// Interaction logic for ValueListEditor.xaml
    /// </summary>
    internal partial class ValueListEditor : PromptuWindow, IValueListEditor
    {
        public ValueListEditor()
        {
            InitializeComponent();
        }
        
        public string MainInstructions
        {
            set { this.mainInstructions.Text = value; }
        }

        public string NameLabelText
        {
            set { this.nameLabel.Content = value; }
        }

        ITextInput IValueListEditor.Name
        {
            get { return this.name; }
        }

        public IButton OkButton
        {
            get { return this.okButton; }
        }

        public IButton CancelButton
        {
            get { return this.cancelButton; }
        }

        public ICheckBox NamespaceInterpretation
        {
            get { return this.useNamespaceInterpretation; }
        }

        public ICheckBox TranslateValues
        {
            get { return this.enableValueTranslation; }
        }

        public ICollectionEditor ValueListContentsPanel
        {
            get { return this.valuesCollectionEditor; }
        }

        public void CloseWithOk()
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
