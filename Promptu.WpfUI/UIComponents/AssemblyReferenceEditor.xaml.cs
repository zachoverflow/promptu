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
    /// Interaction logic for AssemblyReferenceEditor.xaml
    /// </summary>
    internal partial class AssemblyReferenceEditor : PromptuWindow, IAssemblyReferenceEditor
    {
        public AssemblyReferenceEditor()
        {
            InitializeComponent();
        }

        public new ITextInput Name
        {
            get { return this.name; }
        }

        public ITextInput Path
        {
            get { return this.path; }
        }

        public IButton BrowseButton
        {
            get { return this.browsePathButton; }
        }

        public IButton OkButton
        {
            get { return this.okButton; }
        }

        public IButton CancelButton
        {
            get { return this.cancelButton; }
        }

        public string NameLabelText
        {
            set
            {
                this.nameLabel.Content = value;
            }
        }

        public string PathLabelText
        {
            set
            {
                this.pathLabel.Content = value;
            }
        }

        public string MainInstructions
        {
            set { this.mainInstructions.Text = value; }
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

        public UIModel.UIDialogResult ShowModal()
        {
            return WpfToolkitHost.ShowDialogUIDialogResult(this);
        }
    }
}
