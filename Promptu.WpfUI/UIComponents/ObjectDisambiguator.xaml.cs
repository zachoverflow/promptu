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
    /// Interaction logic for ObjectDisambiguator.xaml
    /// </summary>
    internal partial class ObjectDisambiguator : Window, IObjectDisambiguator
    {
        private BulkObservableCollection<object> items = new BulkObservableCollection<object>();

        public ObjectDisambiguator()
        {
            InitializeComponent();

            this.DataContext = items;
            this.listBox.MouseDoubleClick += this.HandleListBoxDoubleClick;
        }

        public IButton OkButton
        {
            get { return this.okButton; }
        }

        public IButton CancelButton
        {
            get { return this.cancelButton; }
        }

        public string MainInstructions
        {
            set
            {
                this.mainInstructions.Text = value;
            }
        }

        //public int SelectedIndex
        //{
        //    set 
        //    { 
        //        this.listBox.SelectedIndex = value; 
        //        this.listBox.ScrollIntoView(t
        //    }
        //}

        private void HandleListBoxDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (this.HitTestAncestorIsType<ListBoxItem>(Mouse.GetPosition(this)))
            {
                this.DialogResult = true;
            }
        }

        public void SetAmbiguousObjects(IEnumerable<object> ambiguousObjects)
        {
            this.items.Clear();
            this.items.AddRange(ambiguousObjects);
        }

        public object SelectedObject
        {
            get
            {
                return this.listBox.SelectedItem;
            }

            set
            {
                this.listBox.SelectedItem = value;
                this.listBox.ScrollIntoView(value);
            }
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
            return WpfToolkitHost.ConvertToDialogResult(this.ShowDialog());
        }
    }
}
