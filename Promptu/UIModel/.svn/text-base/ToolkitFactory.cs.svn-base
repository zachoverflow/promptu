using System;
using System.Collections.Generic;
using System.Text;
using ZachJohnson.Promptu.UIModel.Interfaces;
using ZachJohnson.Promptu.SkinApi;
using System.Windows.Forms;
using ZachJohnson.Promptu.PluginModel;

namespace ZachJohnson.Promptu.UIModel
{
    internal abstract class ToolkitFactory
    {
        public ToolkitFactory()
        {
        }

        public INotifyIcon ConstructNotifyIcon()
        {
            return this.ConstructNotifyIconCore();
        }

        public IUnhandledExceptionDialog ConstructUnhandledExceptionDialog()
        {
            return this.ConstructUnhandledExceptionDialogCore();
        }

        public ISplashScreen ConstructSplashScreen()
        {
            return this.ConstructSplashScreenCore();
        }

        public INewUserDialog ConstructNewUserDialog()
        {
            return this.ConstructNewUserDialogCore();
        }

        //public INewProfileDialog ConstructNewProfileDialog()
        //{
        //    return this.ConstructNewProfileDialogCore();
        //}

        public IMenuItem ConstructNewMenuItem()
        {
            return this.ConstructNewMenuItemCore();
        }

        public IMenuSeparator ConstructNewMenuSeparator()
        {
            return this.ConstructNewMenuSeparatorCore();
        }

        public IHotkeyInUseDialog ConstructHotkeyInUseDialog()
        {
            return this.ConstructHotkeyInUseDialogCore();
        }

        public ITabPage ConstructTabPage()
        {
            return this.ConstructTabPageCore();
        }

        public ISuperTabPage ConstructSuperTabPage()
        {
            return this.ConstructSuperTabPageCore();
        }

        public ITabControl ConstructSuperTabControl()
        {
            return this.ConstructSuperTabControlCore();
        }

        public ITabControl ConstructTabControl()
        {
            return this.ConstructTabControlCore();
        }

        public INativeContextMenu ConstructContextMenu()
        {
            return this.ConstructContextMenuCore();
        }

        public IOpenFileDialog ConstructOpenFileDialog()
        {
            return this.ConstructOpenFileDialogCore();
        }

        public IAssemblyReferenceEditor ConstructAssemblyReferenceEditor()
        {
            return this.ConstructAssemblyReferenceEditorCore();
        }

        public IArgumentDialog ConstructArgumentDialog()
        {
            return this.ConstructArgumentDialogCore();
        }

        public IFunctionEditor ConstructFunctionEditor()
        {
            return this.ConstructFunctionEditorCore();
        }

        public IValueListEditor ConstructValueListEditor()
        {
            return this.ConstructValueListEditorCore();
        }

        public IValueListSelectorWindow ConstructValueListSelectorWindow()
        {
            return this.ConstructValueListSelectorWindowCore();
        }

        public IFunctionInvocationEditor ConstructFunctionInvocationEditor()
        {
            return this.ConstructFunctionInvocationEditorCore();
        }

        public IFileSystemParameterSuggestionEditor ConstructFileSystemParameterSuggestionEditor()
        {
            return this.ConstructFileSystemParameterSuggestionEditorCore();
        }

        public IFunctionViewer ConstructFunctionViewer()
        {
            return this.ConstructFunctionViewerCore();
        }

        public ICommandEditor ConstructCommandEditor()
        {
            return this.ConstructCommandEditorCore();
        }

        public ISaveFileDialog ConstructSaveFileDialog()
        {
            return this.ConstructSaveFileDialogCore();
        }

        public IRenameDialog ConstructRenameDialog()
        {
            return this.ConstructRenameDialogCore();
        }

        public ICollisionResolvingDialog ConstructCollisionResolvingDialog()
        {
            return this.ConstructCollisionResolvingDialogCore();
        }

        public IProfileTabPanel ConstructProfileTabPanel()
        {
            return this.ConstructProfileTabPanelCore();
        }

        public ISetupPanel ConstructSetupPanel(ParameterlessVoid settingChangedCallback)
        {
            return this.ConstructSetupPanelCore(settingChangedCallback);
        }

        public ISetupDialog ConstructSetupDialog()
        {
            return this.ConstructSetupDialogCore();
        }

        public IObjectDisambiguator ConstructObjectDisambiguator()
        {
            return this.ConstructObjectDisambiguatorCore();
        }

        public IDownloadProgressDialog ConstructDownloadProgressDialog()
        {
            return this.ConstructDownloadProgressDialogCore();
        }

        public IOptionButton ConstructOptionButton()
        {
            return this.ConstructOptionButtonCore();
        }

        public IProfileConfigPanel ConstructProfileConfigPanel()
        {
            return this.ConstructProfileConfigPanelCore();
        }

        public IPluginUpdateDialog ConstructPluginUpdateDialog()
        {
            return this.ConstructPluginUpdateDialogCore();
        }

        //public IFlowContainer ConstructFlowContainer()
        //{
        //    return this.ConstructFlowContainerCore();
        //}

        //public IBorderedHotkeyControl ConstructHotkeyControl()
        //{
        //    return this.ConstructHotkeyControlCore();
        //}

        //public IProxySettingsPanel ConstructProxySettingsPanel()
        //{
        //    return this.ConstructProxySettingsPanelCore();
        //}

        public ITextInfoBox ConstructDefaultTextInfoBox()
        {
            return this.ConstructDefaultTextInfoBoxCore();
        }

        public IProgressInfoBox ConstructDefaultProgressInfoBox()
        {
            return this.ConstructDefaultProgressInfoBoxCore();
        }

        public IGlobalHotkey ConstructGlobalHotkey(HotkeyModifierKeys modifierKeys, Keys key, bool overrideIfNecessary, bool enabled)
        {
            return this.ConstructGlobalHotkeyCore(modifierKeys, key, overrideIfNecessary, enabled);
        }

        public IOverwriteDialog ConstructOverwriteDialog(bool showDoForRemaining)
        {
            return this.ConstructOverwriteDialogCore(showDoForRemaining);
        }

        public IConfirmDialog ConstructConfirmDialog(UIMessageBoxIcon icon)
        {
            return this.ConstructConfirmDialogCore(icon);
        }

        public IAppearanceTabPanel ConstructApperanceTabPanel()
        {
            return this.ConstructApperanceTabPanelCore();
        }

        public IPluginsTabPanel ConstructPluginsTabPanel()
        {
            return this.ConstructPluginsTabPanelCore();
        }

        public IList<T> ConstructBindingList<T>()
        {
            return this.ConstructBindingListCore<T>();
        }

        public IObjectPropertyCollectionEditor ConstructObjectPropertyCollectionEditor()
        {
            return this.ConstructObjectPropertyCollectionEditorCore();
        }

        public IOptionsTabPanel ConstructOptionsTabPanel()
        {
            return this.ConstructOptionsTabPanelCore();
        }

        public IPromptuOptionsPanel ConstructOptionsPanel()
        {
            return this.ConstructOptionsPanelCore();
        }

        public IOptionsDialog ConstructOptionsDialog()
        {
            return this.ConstructOptionsDialogCore();
        }

        public PropertiesAndOptions ConstructDefaultInformationBoxPropertiesAndOptions()
        {
            return ConstructDefaultInformationBoxPropertiesAndOptionsCore();
        }

        //public ICustomProxySettingsPanel ConstructCustomProxySettingsPanel()
        //{
        //    return this.ConstructCustomProxySettingsPanelCore();
        //}

        //public ITextInput ConstructTextInput()
        //{
        //    return this.ConstructTextInputCore();
        //}

        //public IStackWidget ConstructStackWidget()
        //{
        //    return ConstructStackWidgetCore();
        //}

        //public ILabel ConstructLabel()
        //{
        //    return ConstructLabelCore();
        //}

        public IAboutPanel ConstructAboutPanel()
        {
            return this.ConstructAboutPanelCore();
        }

        public IGetPluginsPanel ConstructGetPluginsPanel()
        {
            return this.ConstructGetPluginsPanelCore();
        }

        public IInstalledPluginsPanel ConstructInstalledPluginsPanel()
        {
            return this.ConstructInstalledPluginsPanelCore();
        }

        public IUpdateAvailableDialog ConstructUpdateAvailableDialog()
        {
            return ConstructUpdateAvailableDialogCore();
        }

        public IOptionsProgressBar ConstructOptionsProgressBar()
        {
            return this.ConstructOptionsProgressBarCore();
        }

        protected abstract PropertiesAndOptions ConstructDefaultInformationBoxPropertiesAndOptionsCore();

        protected abstract INotifyIcon ConstructNotifyIconCore();

        protected abstract IUnhandledExceptionDialog ConstructUnhandledExceptionDialogCore();

        protected abstract ISplashScreen ConstructSplashScreenCore();

        protected abstract INewUserDialog ConstructNewUserDialogCore();

        //protected abstract INewProfileDialog ConstructNewProfileDialogCore();

        protected abstract IMenuItem ConstructNewMenuItemCore();

        protected abstract IMenuSeparator ConstructNewMenuSeparatorCore();

        protected abstract IHotkeyInUseDialog ConstructHotkeyInUseDialogCore();

        protected abstract ITabPage ConstructTabPageCore();

        protected abstract ISuperTabPage ConstructSuperTabPageCore();

        protected abstract INativeContextMenu ConstructContextMenuCore();

        protected abstract IOpenFileDialog ConstructOpenFileDialogCore();

        protected abstract IAssemblyReferenceEditor ConstructAssemblyReferenceEditorCore();

        protected abstract IArgumentDialog ConstructArgumentDialogCore();

        protected abstract IFunctionEditor ConstructFunctionEditorCore();

        protected abstract IValueListEditor ConstructValueListEditorCore();

        protected abstract IValueListSelectorWindow ConstructValueListSelectorWindowCore();

        protected abstract IFunctionInvocationEditor ConstructFunctionInvocationEditorCore();

        protected abstract IFileSystemParameterSuggestionEditor ConstructFileSystemParameterSuggestionEditorCore();

        protected abstract IFunctionViewer ConstructFunctionViewerCore();

        protected abstract ICommandEditor ConstructCommandEditorCore();

        protected abstract ISaveFileDialog ConstructSaveFileDialogCore();

        protected abstract IRenameDialog ConstructRenameDialogCore();

        protected abstract ICollisionResolvingDialog ConstructCollisionResolvingDialogCore();

        protected abstract IProfileTabPanel ConstructProfileTabPanelCore();

        protected abstract ISetupPanel ConstructSetupPanelCore(ParameterlessVoid settingChangedCallback);

        protected abstract ISetupDialog ConstructSetupDialogCore();

        protected abstract IObjectDisambiguator ConstructObjectDisambiguatorCore();

        protected abstract IDownloadProgressDialog ConstructDownloadProgressDialogCore();

        protected abstract ITabControl ConstructTabControlCore();

        protected abstract ITabControl ConstructSuperTabControlCore();

        protected abstract IOptionButton ConstructOptionButtonCore();

        //protected abstract IFlowContainer ConstructFlowContainerCore();

        //protected abstract ITextInput ConstructTextInputCore();

        //protected abstract IStackWidget ConstructStackWidgetCore();

        //protected abstract ILabel ConstructLabelCore();

        //protected abstract IBorderedHotkeyControl ConstructHotkeyControlCore();

        //protected abstract IProxySettingsPanel ConstructProxySettingsPanelCore();

        protected abstract ITextInfoBox ConstructDefaultTextInfoBoxCore();

        protected abstract IProgressInfoBox ConstructDefaultProgressInfoBoxCore();

        protected abstract IGlobalHotkey ConstructGlobalHotkeyCore(HotkeyModifierKeys modifierKeys, Keys key, bool overrideIfNecessary, bool enabled);

        protected abstract IOverwriteDialog ConstructOverwriteDialogCore(bool showDoForRemaining);

        protected abstract IConfirmDialog ConstructConfirmDialogCore(UIMessageBoxIcon icon);

        protected abstract IAppearanceTabPanel ConstructApperanceTabPanelCore();

        protected abstract IPluginsTabPanel ConstructPluginsTabPanelCore();

        protected abstract IList<T> ConstructBindingListCore<T>();

        protected abstract IObjectPropertyCollectionEditor ConstructObjectPropertyCollectionEditorCore();

        protected abstract IOptionsTabPanel ConstructOptionsTabPanelCore();

        protected abstract IPromptuOptionsPanel ConstructOptionsPanelCore();

        protected abstract IProfileConfigPanel ConstructProfileConfigPanelCore();

        protected abstract IOptionsDialog ConstructOptionsDialogCore();

        protected abstract IAboutPanel ConstructAboutPanelCore();

        protected abstract IGetPluginsPanel ConstructGetPluginsPanelCore();

        protected abstract IInstalledPluginsPanel ConstructInstalledPluginsPanelCore();

        protected abstract IPluginUpdateDialog ConstructPluginUpdateDialogCore();

        protected abstract IUpdateAvailableDialog ConstructUpdateAvailableDialogCore();

        protected abstract IOptionsProgressBar ConstructOptionsProgressBarCore();

        //protected abstract ICustomProxySettingsPanel ConstructCustomProxySettingsPanelCore();
    }
}
