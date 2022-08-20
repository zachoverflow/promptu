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
using ZachJohnson.Promptu.PluginModel.Internals;

namespace ZachJohnson.Promptu.WpfUI.UIComponents
{
    /// <summary>
    /// Interaction logic for PluginUpdateDialog.xaml
    /// </summary>
    internal partial class PluginUpdateDialog : Window, IPluginUpdateDialog
    {
        private BulkObservableCollection<PluginUpdate> updates = new BulkObservableCollection<PluginUpdate>();

        public PluginUpdateDialog()
        {
            InitializeComponent();
            this.listBox.DataContext = updates;
        }

        public IButton InstallUpdatesButton
        {
            get { return this.installUpdatesButton; }
        }

        public IButton CancelButton
        {
            get { return this.cancelButton; }
        }

        public string MainInstructions
        {
            set { this.mainInstructions.Text = value; }
        }

        public void SetPluginUpdates(IEnumerable<PluginUpdate> pluginUpdates)
        {
            this.updates.Clear();
            this.updates.AddRange(pluginUpdates);
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

        public void CloseWithOk()
        {
            this.DialogResult = true;
        }
    }
}
