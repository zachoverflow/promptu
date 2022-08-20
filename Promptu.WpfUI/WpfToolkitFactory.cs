//-----------------------------------------------------------------------
// <copyright file="WpfToolkitFactory.cs" company="ZachJohnson">
//     Copyright (c) Zach Johnson. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace ZachJohnson.Promptu.WpfUI
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using ZachJohnson.Promptu.UIModel;
    using ZachJohnson.Promptu.UIModel.Interfaces;
    using System.Windows.Forms;
    using ZachJohnson.Promptu.WpfUI.UIComponents;
    using ZachJohnson.Promptu.SkinApi;
    using ZachJohnson.Promptu.PluginModel;
    using System.Windows.Media;

    internal class WpfToolkitFactory : ToolkitFactory
    {
        public WpfToolkitFactory()
        {
        }

        protected override IGlobalHotkey ConstructGlobalHotkeyCore(HotkeyModifierKeys modifierKeys, Keys key, bool overrideIfNecessary, bool enabled)
        {
            return new WindowsGlobalHotkey(modifierKeys, key, overrideIfNecessary, enabled);
        }

        protected override INotifyIcon ConstructNotifyIconCore()
        {
#if NO_WPF
            System.Diagnostics.Debug.WriteLine("ConstructNotifyIconCore");
            return new Dummy.DummyNotifyIcon();
#endif
            return new WpfNotifyIcon();
        }

        protected override IUnhandledExceptionDialog ConstructUnhandledExceptionDialogCore()
        {
#if NO_WPF
            System.Diagnostics.Debug.WriteLine("ConstructUnhandledExceptionDialogCore");
#endif
            return new UnhandledExceptionDialog();
        }

        protected override ISplashScreen ConstructSplashScreenCore()
        {
#if NO_WPF
            System.Diagnostics.Debug.WriteLine("ConstructSplashScreenCore");
            return new Dummy.DummySplashScreen();
#endif
            return new WpfSplashScreen();
        }

        protected override INewUserDialog ConstructNewUserDialogCore()
        {
#if NO_WPF
            System.Diagnostics.Debug.WriteLine("ConstructNewUserDialogCore");
#endif
            return new NewUserDialog();
        }

        protected override IMenuItem ConstructNewMenuItemCore()
        {
#if NO_WPF
            System.Diagnostics.Debug.WriteLine("ConstructNewMenuItemCore");
            return new Dummy.DummyMenuItem();
#endif
            return new WpfMenuItem();
        }

        protected override IMenuSeparator ConstructNewMenuSeparatorCore()
        {
#if NO_WPF
            System.Diagnostics.Debug.WriteLine("ConstructNewMenuSeparatorCore");
            return new Dummy.DummyMenuSeparator();
#endif
            return new WpfMenuSeparator();
        }

        protected override IHotkeyInUseDialog ConstructHotkeyInUseDialogCore()
        {
#if NO_WPF
            System.Diagnostics.Debug.WriteLine("ConstructHotkeyInUseDialogCore");
#endif
            return new HotkeyInUseDialog();
        }

        protected override ITabPage ConstructTabPageCore()
        {
#if NO_WPF
            System.Diagnostics.Debug.WriteLine("ConstructTabPageCore");
#endif
            return new WpfTabPage();
        }

        protected override INativeContextMenu ConstructContextMenuCore()
        {
#if NO_WPF
            System.Diagnostics.Debug.WriteLine("ConstructContextMenuCore");
#endif
            return new WpfContextMenu();
        }

        protected override IOpenFileDialog ConstructOpenFileDialogCore()
        {
#if NO_WPF
            System.Diagnostics.Debug.WriteLine("ConstructOpenFileDialogCore");
#endif
            return new WpfOpenFileDialog();
        }

        protected override IAssemblyReferenceEditor ConstructAssemblyReferenceEditorCore()
        {
#if NO_WPF
            System.Diagnostics.Debug.WriteLine("ConstructAssemblyReferenceEditorCore");
#endif
            return new AssemblyReferenceEditor();
        }

        protected override IArgumentDialog ConstructArgumentDialogCore()
        {
#if NO_WPF
            System.Diagnostics.Debug.WriteLine("ConstructArgumentDialogCore");
#endif
            return new ArgumentDialog();
        }

        protected override IFunctionEditor ConstructFunctionEditorCore()
        {
#if NO_WPF
            System.Diagnostics.Debug.WriteLine("ConstructFunctionEditorCore");
#endif
            return new FunctionEditor();
        }

        protected override IValueListEditor ConstructValueListEditorCore()
        {
#if NO_WPF
            System.Diagnostics.Debug.WriteLine("ConstructValueListEditorCore");
#endif
            return new ValueListEditor();
        }

        protected override IValueListSelectorWindow ConstructValueListSelectorWindowCore()
        {
#if NO_WPF
            System.Diagnostics.Debug.WriteLine("ConstructValueListSelectorWindowCore");
#endif
            return new ValueListSelectorWindow();
        }

        protected override IFunctionInvocationEditor ConstructFunctionInvocationEditorCore()
        {
#if NO_WPF
            System.Diagnostics.Debug.WriteLine("ConstructFunctionInvocationEditorCore");
#endif
            return new FunctionInvocationEditor();
        }

        protected override IFileSystemParameterSuggestionEditor ConstructFileSystemParameterSuggestionEditorCore()
        {
#if NO_WPF
            System.Diagnostics.Debug.WriteLine("ConstructFileSystemParameterSuggestionEditorCore");
#endif
            return new FileSystemParameterSuggestionEditor();
        }

        protected override IFunctionViewer ConstructFunctionViewerCore()
        {
#if NO_WPF
            System.Diagnostics.Debug.WriteLine("ConstructFunctionViewerCore");
#endif
            return new FunctionViewer();
        }

        protected override ICommandEditor ConstructCommandEditorCore()
        {
#if NO_WPF
            System.Diagnostics.Debug.WriteLine("ConstructCommandEditorCore");
#endif
            return new CommandEditor();
        }

        protected override ISaveFileDialog ConstructSaveFileDialogCore()
        {
#if NO_WPF
            System.Diagnostics.Debug.WriteLine("ConstructSaveFileDialogCore");
#endif
            return new WpfSaveFileDialog();
        }

        protected override IRenameDialog ConstructRenameDialogCore()
        {
#if NO_WPF
            System.Diagnostics.Debug.WriteLine("ConstructRenameDialogCore");
#endif
            return new RenameDialog();
        }

        protected override ICollisionResolvingDialog ConstructCollisionResolvingDialogCore()
        {
#if NO_WPF
            System.Diagnostics.Debug.WriteLine("ConstructCollisionResolvingDialogCore");
#endif
            return new CollisionResolvingDialog();
        }

        protected override IProfileTabPanel ConstructProfileTabPanelCore()
        {
#if NO_WPF
            System.Diagnostics.Debug.WriteLine("ConstructProfileTabPanelCore");
#endif
            return new ProfileTabPanel();
        }

        protected override ISetupPanel ConstructSetupPanelCore(ParameterlessVoid settingChangedCallback)
        {
#if NO_WPF
            System.Diagnostics.Debug.WriteLine("ConstructSetupPanelCore(ParameterlessVoid)");
#endif
            SetupPanel panel = new SetupPanel();
            panel.SettingChangedCallback = settingChangedCallback;
            return panel;
        }

        protected override ISetupDialog ConstructSetupDialogCore()
        {
#if NO_WPF
            System.Diagnostics.Debug.WriteLine("ConstructSetupDialogCore");
#endif
            SetupDialog dialog = new SetupDialog();
            //dialog.SettingChangedCallback = settingChangedCallback;
            return dialog;
        }

        protected override IObjectDisambiguator ConstructObjectDisambiguatorCore()
        {
#if NO_WPF
            System.Diagnostics.Debug.WriteLine("ConstructObjectDisambiguatorCore");
#endif
            return new ObjectDisambiguator();
        }

        protected override IDownloadProgressDialog ConstructDownloadProgressDialogCore()
        {
#if NO_WPF
            System.Diagnostics.Debug.WriteLine("ConstructDownloadProgressDialogCore");
#endif
            return new UpdateDownloadProgress();
        }

        protected override ITabControl ConstructTabControlCore()
        {
#if NO_WPF
            System.Diagnostics.Debug.WriteLine("ConstructTabControlCore");
#endif
            return new WpfTabControl();
        }

        //protected override IBorderedHotkeyControl ConstructHotkeyControlCore()
        //{
        //    throw new NotImplementedException();
        //}

        //protected override IProxySettingsPanel ConstructProxySettingsPanelCore()
        //{
        //    throw new NotImplementedException();
        //}

        protected override ITextInfoBox ConstructDefaultTextInfoBoxCore()
        {
#if NO_WPF
            System.Diagnostics.Debug.WriteLine("ConstructDefaultTextInfoBoxCore");
#endif
            return new DefaultSkin.DefaultTextInfoBox();
        }

        protected override IProgressInfoBox ConstructDefaultProgressInfoBoxCore()
        {
#if NO_WPF
            System.Diagnostics.Debug.WriteLine("ConstructDefaultProgressInfoBoxCore");
#endif
            return new DefaultSkin.DefaultProgressInfoBox();
        }

        protected override IOverwriteDialog ConstructOverwriteDialogCore(bool showDoForRemaining)
        {
#if NO_WPF
            System.Diagnostics.Debug.WriteLine("ConstructOverwriteDialogCore");
#endif
            return new OverwriteDialog(showDoForRemaining);
        }

        protected override IConfirmDialog ConstructConfirmDialogCore(UIMessageBoxIcon icon)
        {
#if NO_WPF
            System.Diagnostics.Debug.WriteLine("ConstructConfirmDialogCore");
#endif
            return new ConfirmDialog(icon);
        }

        protected override IAppearanceTabPanel ConstructApperanceTabPanelCore()
        {
#if NO_WPF
            System.Diagnostics.Debug.WriteLine("ConstructApperanceTabPanelCore");
#endif
            return new AppearanceTabPanel();
        }

        protected override IList<T> ConstructBindingListCore<T>()
        {
#if NO_WPF
            System.Diagnostics.Debug.WriteLine("ConstructBindingListCore");
#endif
            return new System.Collections.ObjectModel.ObservableCollection<T>();
        }

        protected override IObjectPropertyCollectionEditor ConstructObjectPropertyCollectionEditorCore()
        {
#if NO_WPF
            System.Diagnostics.Debug.WriteLine("ConstructObjectPropertyCollectionEditorCore");
#endif
            return new OptionsCollectionEditor();
        }

        protected override IOptionsTabPanel ConstructOptionsTabPanelCore()
        {
#if NO_WPF
            System.Diagnostics.Debug.WriteLine("ConstructOptionsTabPanelCore");
#endif
            return new OptionsTabPanel();
        }

        protected override ITabControl ConstructSuperTabControlCore()
        {
#if NO_WPF
            System.Diagnostics.Debug.WriteLine("ConstructSuperTabControlCore");
#endif
            return new SuperTabControl();
        }

        protected override ISuperTabPage ConstructSuperTabPageCore()
        {
#if NO_WPF
            System.Diagnostics.Debug.WriteLine("ConstructSuperTabPageCore");
#endif
            return new SuperTab();
        }

        protected override IPromptuOptionsPanel ConstructOptionsPanelCore()
        {
#if NO_WPF
            System.Diagnostics.Debug.WriteLine("ConstructOptionsPanelCore");
#endif
            return new PromptuOptionsPanel();
        }

        protected override IOptionButton ConstructOptionButtonCore()
        {
#if NO_WPF
            System.Diagnostics.Debug.WriteLine("ConstructOptionButtonCore");
#endif
            return new WpfOptionButton();
        }

        protected override IProfileConfigPanel ConstructProfileConfigPanelCore()
        {
#if NO_WPF
            System.Diagnostics.Debug.WriteLine("ConstructProfileConfigPanelCore");
#endif
            return new ProfileConfigPanel();
        }

        protected override IOptionsDialog ConstructOptionsDialogCore()
        {
#if NO_WPF
            System.Diagnostics.Debug.WriteLine("ConstructOptionsDialogCore");
#endif
            return new OptionsDialog();
        }

        protected override PropertiesAndOptions ConstructDefaultInformationBoxPropertiesAndOptionsCore()
        {
#if NO_WPF
            System.Diagnostics.Debug.WriteLine("ConstructDefaultInformationBoxPropertiesAndOptionsCore");
#endif
            PropertiesAndOptions propertiesAndOptions = new PropertiesAndOptions();

            DefaultSkin.DefaultTextInfoBox exampleBox = new DefaultSkin.DefaultTextInfoBox();

            //object pt = new System.Windows.FontSizeConverter().ConvertFrom("9pt");

            AppliedObjectProperty<FontFamily> fontFamily = new AppliedObjectProperty<FontFamily>(
                "FontFamily",
                Localization.UIResources.DefaultIBFontFamilyLabel,
                exampleBox.FontFamily,
                "FontFamily",
                null,
                new TextConversionInfo("group", false, null, 100),
                null);

            AppliedObjectProperty<int> fontSize = new AppliedObjectProperty<int>(
                "FontSize",
                Localization.UIResources.DefaultIBOptionsFontSizeLabel,
                exampleBox.FontSize,
                "FontSize",
                null,
                new TextConversionInfo("group", false, null, 30),
                new PointFontSizeConverter());

            propertiesAndOptions.Properties.Add(fontFamily);
            propertiesAndOptions.Properties.Add(fontSize);

            OptionsGroup appearanceGroup = new OptionsGroup("Appearance", Localization.UIResources.DefaultIBOptionsAppearanceGroup);
            appearanceGroup.Add(fontFamily);
            appearanceGroup.Add(fontSize);

            propertiesAndOptions.Options.Groups.Add(appearanceGroup);

            exampleBox.Close();

            return propertiesAndOptions;
        }

        protected override IAboutPanel ConstructAboutPanelCore()
        {
#if NO_WPF
            System.Diagnostics.Debug.WriteLine("ConstructAboutPanelCore");
#endif
            return new AboutPanel();
        }

        protected override IPluginsTabPanel ConstructPluginsTabPanelCore()
        {
#if NO_WPF
            System.Diagnostics.Debug.WriteLine("ConstructPluginsTabPanelCore");
#endif
            return new PluginsTabPanel();
        }

        protected override IGetPluginsPanel ConstructGetPluginsPanelCore()
        {
#if NO_WPF
            System.Diagnostics.Debug.WriteLine("ConstructGetPluginsPanelCore");
#endif
            return new GetPluginsPanel();
        }

        protected override IInstalledPluginsPanel ConstructInstalledPluginsPanelCore()
        {
#if NO_WPF
            System.Diagnostics.Debug.WriteLine("ConstructInstalledPluginsPanelCore");
#endif
            return new InstalledPluginsPanel();
        }

        protected override IPluginUpdateDialog ConstructPluginUpdateDialogCore()
        {
#if NO_WPF
            System.Diagnostics.Debug.WriteLine("ConstructPluginUpdateDialogCore");
#endif
            return new PluginUpdateDialog();
        }

        protected override IUpdateAvailableDialog ConstructUpdateAvailableDialogCore()
        {
#if NO_WPF
            System.Diagnostics.Debug.WriteLine("ConstructUpdateAvailableDialogCore");
#endif
            return new UpdateAvailableDialog();
        }

        protected override IOptionsProgressBar ConstructOptionsProgressBarCore()
        {
#if NO_WPF
            System.Diagnostics.Debug.WriteLine("ConstructOptionsProgressBarCore");
#endif
            return new WpfOptionsProgressBar();
        }
    }
}
