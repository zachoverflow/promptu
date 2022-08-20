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
using ZachJohnson.Promptu.PluginModel;

namespace ZachJohnson.Promptu.WpfUI.UIComponents
{
    /// <summary>
    /// Interaction logic for ObjectPropertyCollectionEditor.xaml
    /// </summary>
    internal partial class OptionsCollectionEditor : UserControl, IObjectPropertyCollectionEditor
    {
        public OptionsCollectionEditor()
        {
            InitializeComponent();
            this.AddHandler(Button.ClickEvent, new RoutedEventHandler(this.HandleButtonClick));
        }

        public PluginModel.OptionsGroupCollection Properties
        {
            get
            {
                return (PluginModel.OptionsGroupCollection)this.items.DataContext;
            }
            set
            {
                this.items.DataContext = value;
            }
        }

        private void HandleButtonClick(object sender, RoutedEventArgs e)
        {
            e.Handled = false;
            Button button = e.OriginalSource as Button;
            if (button != null && button.Name == "fileSystemFileBrowse")
            {
                ObjectPropertyBase objectPropertyBase = button.DataContext as ObjectPropertyBase;
                if (objectPropertyBase != null)
                {
                    e.Handled = true;
                    FileSystemFile currentValue = (FileSystemFile)objectPropertyBase.ObjectValue;

                    string filter = Localization.UIResources.AllFilesFilter;
                    FileDialogType dialogType = FileDialogType.Save;

                    FileConversionInfo conversionInfo = objectPropertyBase.ConversionInfo as FileConversionInfo;
                    if (conversionInfo != null)
                    {
                        string providedFilter = conversionInfo.Filter;
                        if (providedFilter != null)
                        {
                            filter = providedFilter;
                        }

                        dialogType = conversionInfo.DialogType;
                    }

                    if (dialogType == FileDialogType.Open)
                    {
                        IOpenFileDialog dialog = InternalGlobals.GuiManager.ToolkitHost.Factory.ConstructOpenFileDialog();
                        dialog.Filter = filter;
                        dialog.Path = currentValue.Name;
                        if (currentValue.Path.Length > 0)
                        {
                            dialog.InitialDirectory = string.Empty;
                        }
                        else
                        {
                            try
                            {
                                dialog.InitialDirectory = currentValue.GetParentDirectory();
                            }
                            catch (ArgumentException)
                            {
                            }
                        }

                        if (dialog.ShowModal() == UIModel.UIDialogResult.OK)
                        {
                            objectPropertyBase.ObjectValue = (FileSystemFile)dialog.Path;
                        }
                    }
                    else
                    {
                        ISaveFileDialog dialog = InternalGlobals.GuiManager.ToolkitHost.Factory.ConstructSaveFileDialog();
                        dialog.Filter = filter;
                        dialog.Path = currentValue.Name;
                        if (currentValue.Path.Length > 0)
                        {
                            dialog.InitialDirectory = string.Empty;
                        }
                        else
                        {
                            try
                            {
                                dialog.InitialDirectory = currentValue.GetParentDirectory();
                            }
                            catch (ArgumentException)
                            {
                            }
                        }

                        if (dialog.ShowModal() == UIModel.UIDialogResult.OK)
                        {
                            objectPropertyBase.ObjectValue = (FileSystemFile)dialog.Path;
                        }
                    }
                }
            }
        }
    }
}