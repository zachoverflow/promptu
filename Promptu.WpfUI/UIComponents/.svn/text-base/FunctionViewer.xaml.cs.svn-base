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
    /// Interaction logic for FunctionViewer.xaml
    /// </summary>
    internal partial class FunctionViewer : PromptuWindow, IFunctionViewer
    {
        public FunctionViewer()
        {
            InitializeComponent();
            this.okButton.Click += this.HandleOkButtonClick;
        }

        public ISimpleCollectionViewer FunctionsList
        {
            get { return this.functionsList; }
        }

        public string SelectedFunctionName
        {
            set { this.functionName.Text = value; }
        }

        public string SelectedFunctionDescription
        {
            set { this.functionDetails.Text = value; }
        }

        public void Show(object owner)
        {
            WpfToolkitHost.SetWindowOwner(this, (Window)owner);
            this.Show();
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

        public IButton OkButton
        {
            get { return this.okButton; }
        }

        public string MainInstructions
        {
            set { this.mainInstructions.Text = value; }
        }

        private void HandleOkButtonClick(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
