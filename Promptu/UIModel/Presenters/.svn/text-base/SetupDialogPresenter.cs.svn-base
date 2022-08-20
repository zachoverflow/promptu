using System;
using System.Collections.Generic;
using System.Text;
using ZachJohnson.Promptu.UIModel.Interfaces;
using ZachJohnson.Promptu.UserModel;
using ZachJohnson.Promptu.Skins;
using System.ComponentModel;
using ZachJohnson.Promptu.PTK;
using System.IO;
using ZachJohnson.Promptu.SkinApi;
using ZachJohnson.Promptu.PluginModel;
using System.Security;
using System.Reflection;
using ZachJohnson.Promptu.FileFileSystem;
using System.Globalization;
using System.Timers;

namespace ZachJohnson.Promptu.UIModel.Presenters
{
    internal class SetupDialogPresenter : PresenterBase<ISetupDialog>, IDisposable
    {
        private static int RefreshIconsTimerInterval = 3000;
        private bool updating;

        private TabWidget mainTabs;
        private TabPage profileTab;
        private TabPage optionsTab;
        private TabPage pluginsTab;
        private TabPage appearanceTab;
        private TabPage aboutTab;
        private IProfileTabPanel profileTabPanel;
        private IAppearanceTabPanel appearanceTabPanel;
        private IAboutPanel aboutPanel;
        private IPluginsTabPanel pluginsTabPanel;

        private TabWidget listTabs;
        private TabPage commandsTab;
        private TabPage assemblyReferencesTab;
        private TabPage functionsTab;
        private TabPage valueListsTab;

        private IGetPluginsPanel getPluginsPanel;
        private IInstalledPluginsPanel installedPluginsPanel;

        //private TabWidget optionsTabs;
        //private TabPage basicOptionsTab;
        //private TabPage advancedOptionsTab;
        //private FlowWidget basicOptionsFlowContainer;

        private IOptionsTabPanel optionsTabPanel;
        private SuperTabWidget optionsTabs;
        private SuperTabPage popularOptionsTab;

        private AssemblyReferenceSetupPanelPresenter assemblyReferenceSetupPanel;
        private FunctionSetupPanelPresenter functionSetupPanel;
        private CommandSetupPanelPresenter commandSetupPanel;
        private ValueListSetupPanelPresenter valueListSetupPanel;
        private ListSelectorPresenter listSelector;

        private BoundObjectProperty<bool> autoDetectFileAndFolderVisibility;
        private BoundObjectProperty<bool> showSystemFilesAndFolders;
        private BoundObjectProperty<bool> showHiddenFilesAndFolders;

        private BoundObjectProperty<bool> showCommandTargetIcons;
        private BoundObjectProperty<bool> autoFetchFavicons;
        private IOptionButton refreshIconsButton;
        private Timer refreshIconsTimer;
        private bool doneMainIconRefresh;
        private bool doneFaviconRefresh;
        private IOptionsProgressBar refreshIconsProgressBar;

        private BoundObjectProperty<GlobalHotkey> promptHotkeyProperty;
        private BoundObjectProperty<PositioningMode> promptPositioningMode;
        private CallbackObjectProperty<bool> startPromptuWithComputer;
        private CallbackObjectProperty<bool> checkForPreReleaseUpdates;

        private BoundObjectProperty<bool> showSplashScreen;

        private BackedObjectProperty<bool> noProxy;
        private BackedObjectProperty<bool> useIEProxySettings;
        private BackedObjectProperty<bool> useCustomProxySettings;
        private BackedObjectProperty<string> customProxyAddress;
        private BackedObjectProperty<string> customProxyUsername;
        private BackedObjectProperty<SecureString> customProxyPassword;

        private IOptionButton clearHistoryButton;

        private IProfileConfigPanel profileConfigPanel;
        private OptionsTextEntry usingPreReleaseMessage;

        private bool userChangingProxySettings = true;
        private BackgroundWorker refreshIconsBackgroundWorker;

        //private HotkeyWidget hotkeyWidget;
        //private IProxySettingsPanel proxySettingsPanel;
        //private ICustomProxySettingsPanel customProxySettingsPanel;
        private System.Timers.Timer serializeProxySettingsTimer = new System.Timers.Timer(2000);

        public SetupDialogPresenter()
            : this(InternalGlobals.GuiManager.ToolkitHost.Factory.ConstructSetupDialog())
        {
        }

        public SetupDialogPresenter(ISetupDialog nativeInterface)
            : base(nativeInterface)
        {
            this.serializeProxySettingsTimer.AutoReset = false;
            this.serializeProxySettingsTimer.Elapsed += this.HandleSerializeProxySettingsTimerElapsed;

            ParameterlessVoid updateAllCallback = new ParameterlessVoid(this.UpdateToCurrentList);

            this.mainTabs = new TabWidget("Promptu.MainTabs", this.NativeInterface.MainTabs);
            this.mainTabs.SelectedTabChanged += this.HandleMainTabsSelectedIndexChanged;

            this.NativeInterface.Text = Localization.UIResources.SetupDialogText;
            this.NativeInterface.SettingChangedCallback = this.UpdateSetupDialogSettings;

            this.NativeInterface.Closed += this.HandleClosed;

            this.profileTab = new TabPage("Promptu.ProfileTab");
            this.profileTab.Text = Localization.UIResources.ProfileTabText;
            this.optionsTab = new TabPage("Promptu.OptionsTab");
            this.optionsTab.Text = Localization.UIResources.OptionsTabText;
            this.appearanceTab = new TabPage("Promptu.AppearanceTab");
            this.appearanceTab.Text = Localization.UIResources.AppearanceTabText;
            this.aboutTab = new TabPage("Promptu.AboutTab");
            this.aboutTab.Text = Localization.UIResources.AboutTabText;
            this.pluginsTab = new TabPage("Promptu.PluginsTab");
            this.pluginsTab.Text = Localization.UIResources.PluginsTabText;

            this.mainTabs.Add(this.profileTab);
            this.mainTabs.Add(this.optionsTab);          
            this.mainTabs.Add(this.appearanceTab);
            this.mainTabs.Add(this.pluginsTab);
            this.mainTabs.Add(this.aboutTab);

            this.profileTabPanel = InternalGlobals.GuiManager.ToolkitHost.Factory.ConstructProfileTabPanel();
            this.profileTabPanel.MainInstructions = Localization.UIResources.ProfileTabMainInstructions;
            this.profileTabPanel.SettingChangedCallback = this.UpdateProfileTabSettings;
            this.profileTab.HostedWidget = new ExternalWidget("Promptu.ProfileTabPanel", this.profileTabPanel);

            this.pluginsTabPanel = InternalGlobals.GuiManager.ToolkitHost.Factory.ConstructPluginsTabPanel();
            this.pluginsTab.HostedWidget = new ExternalWidget("Promptu.PluginTabPanel", this.pluginsTabPanel);

            SuperTabWidget pluginsTabs = new SuperTabWidget("Promptu.Plugins.Tabs", this.pluginsTabPanel.SuperTabs);

            SuperTabPage getPluginsTabPage = new SuperTabPage("Promptu.GetPlugins");
            getPluginsTabPage.Text = Localization.UIResources.GetPluginsTabText;
            getPluginsTabPage.Image = InternalGlobals.GuiManager.ToolkitHost.Images.GetPlugins;

            this.getPluginsPanel = InternalGlobals.GuiManager.ToolkitHost.Factory.ConstructGetPluginsPanel();
            getPluginsTabPage.HostedWidget = new ExternalWidget("Panel", this.getPluginsPanel);
            this.getPluginsPanel.MainInstructions = Localization.UIResources.GetPluginsMainInstructions;
            this.getPluginsPanel.InstallPluginClicked += this.HandleInstallPluginClicked;
            this.getPluginsPanel.AddPlugins(InternalGlobals.AvailablePlugins);

            this.getPluginsPanel.PluginBrowseButton.Text = Localization.UIResources.BrowseButtonText;
            this.getPluginsPanel.PluginBrowseButton.Click += this.HandleBrowseForPlugins;

            SuperTabPage installedPluginsTabPage = new SuperTabPage("Promptu.InstalledPlugins");
            installedPluginsTabPage.Text = Localization.UIResources.InstalledPluginsTabText;
            installedPluginsTabPage.Image = InternalGlobals.GuiManager.ToolkitHost.Images.Plugin;

            this.installedPluginsPanel = InternalGlobals.GuiManager.ToolkitHost.Factory.ConstructInstalledPluginsPanel();
            installedPluginsTabPage.HostedWidget = new ExternalWidget("Panel", this.installedPluginsPanel);
            this.installedPluginsPanel.MainInstructions = Localization.UIResources.InstalledPluginsMainInstructions;
            this.installedPluginsPanel.CreatorContactLinkClicked += this.HandlePluginCreatorContactLinkClicked;
            this.installedPluginsPanel.TogglePluginEnabledClicked += this.HandlePluginToggleEnabled;
            this.installedPluginsPanel.RemovePluginClicked += this.HandleRemovePluginClicked;
            this.installedPluginsPanel.ConfigurePluginClicked += this.HandlePluginConfigureClicked;
            this.installedPluginsPanel.CheckForUpdates.Text = Localization.UIResources.PluginCheckForUpdatesText;
            this.installedPluginsPanel.CheckForUpdates.Click += this.HandleCheckForPluginUpdates;

            pluginsTabs.Add(getPluginsTabPage);
            pluginsTabs.Add(installedPluginsTabPage);
            this.UpdateInstalledPlugins();
            pluginsTabs.SelectedIndex = 1;

            this.appearanceTabPanel = InternalGlobals.GuiManager.ToolkitHost.Factory.ConstructApperanceTabPanel();
            this.appearanceTabPanel.CreatorContactLinkClicked += this.HandleSkinCreatorContactLinkClicked;
            this.appearanceTabPanel.ConfigureSkinClicked += this.HandleConfigureSkinClicked;
            this.appearanceTabPanel.SelectedSkinChanged += this.HandleSkinChanged;
            this.appearanceTabPanel.MainInstructions = Localization.UIResources.AppearanceTabPanelMainInstructions;
            this.appearanceTab.HostedWidget = new ExternalWidget("Promptu.AppearanceTabPanel", this.appearanceTabPanel);

            this.listTabs = new TabWidget("Promptu.ListTabs", this.profileTabPanel.ListTabs);
            this.listTabs.SelectedTabChanged += this.HandleSwitchBetweenTabs;

            this.commandsTab = new TabPage("Promptu.List.CommandsTab");
            this.commandsTab.Text = Localization.UIResources.CommandsTabText;
            this.assemblyReferencesTab = new TabPage("Promptu.List.AssemblyReferencesTab");
            this.assemblyReferencesTab.Text = Localization.UIResources.AssemblyReferencesTabText;
            this.functionsTab = new TabPage("Promptu.List.FunctionsTab");
            this.functionsTab.Text = Localization.UIResources.FunctionsTabText;
            this.valueListsTab = new TabPage("Promptu.List.ValueListsTab");
            this.valueListsTab.Text = Localization.UIResources.ValueListsTabText;

            this.listTabs.Add(this.commandsTab);
            this.listTabs.Add(this.assemblyReferencesTab);
            this.listTabs.Add(this.functionsTab);
            this.listTabs.Add(this.valueListsTab);

            this.assemblyReferenceSetupPanel = new AssemblyReferenceSetupPanelPresenter(updateAllCallback, this.UpdateAssemblyReferenceSetupPanelSettings);
            //this.assemblyReferenceSetupPanel.UISettingChanged += this.UpdateAssemblyReferenceSetupPanelSettings;
            this.assemblyReferencesTab.HostedWidget = new ExternalWidget(
                "Promptu.AssemblyReferenceSetupPanel", 
                this.assemblyReferenceSetupPanel.NativeInterface);

            this.functionSetupPanel = new FunctionSetupPanelPresenter(updateAllCallback, this.UpdateFunctionSetupPanelSettings);
            //this.functionSetupPanel.UISettingChanged += this.UpdateFunctionSetupPanelSettings;
            this.functionsTab.HostedWidget = new ExternalWidget(
                "Promptu.FunctionSetupPanel", 
                this.functionSetupPanel.NativeInterface);

            this.valueListSetupPanel = new ValueListSetupPanelPresenter(updateAllCallback, this.UpdateValueListSetupPanelSettings);
            //this.valueListSetupPanel.UISettingChanged += this.UpdateValueListSetupPanelSettings;
            this.valueListsTab.HostedWidget = new ExternalWidget(
                "Promptu.ValueListSetupPanel",
                this.valueListSetupPanel.NativeInterface);

            this.commandSetupPanel = new CommandSetupPanelPresenter(updateAllCallback, this.UpdateCommandSetupPanelSettings);
            //this.commandSetupPanel.UISettingChanged += this.UpdateCommandSetupPanelSettings;
            this.commandsTab.HostedWidget = new ExternalWidget(
                "Promptu.CommandSetupPanel",
                this.commandSetupPanel.NativeInterface);

            this.aboutPanel = InternalGlobals.GuiManager.ToolkitHost.Factory.ConstructAboutPanel();
            this.aboutPanel.CheckForUpdates.Text = Localization.UIResources.CheckForUpdatesText;
            Version promptuVersion = Assembly.GetExecutingAssembly().GetName().Version;
            this.aboutPanel.Version = String.Format(
                CultureInfo.CurrentCulture, 
                Localization.Promptu.VersionFormat,
                promptuVersion.Major,
                promptuVersion.Minor,
                promptuVersion.Build,
                promptuVersion.Revision);

            this.aboutPanel.WebsiteLinkText = "www.PromptuLauncher.com";
            this.aboutPanel.ReleaseType = InternalGlobals.CurrentReleaseType;
            this.aboutPanel.WebsiteLinkClicked += this.VisitWebsite;
            this.aboutPanel.CheckForUpdates.Click += this.HandleCheckForUpdate;

            this.aboutTab.HostedWidget = new ExternalWidget("AboutPanel", this.aboutPanel);

            this.optionsTabPanel = InternalGlobals.GuiManager.ToolkitHost.Factory.ConstructOptionsTabPanel();

            this.optionsTabs = new SuperTabWidget("Promptu.OptionsTabs", this.optionsTabPanel.SuperTabs);

            this.popularOptionsTab = new SuperTabPage("Promptu.OptionsTabs.Popular");
            this.popularOptionsTab.Text = Localization.UIResources.PopularOptionsTabText;
            this.optionsTabs.Add(this.popularOptionsTab);

            IPromptuOptionsPanel popularOptionsPanel = InternalGlobals.GuiManager.ToolkitHost.Factory.ConstructOptionsPanel();
            popularOptionsPanel.MainInstructions = Localization.UIResources.PopularOptionsMainInstructions;

            this.popularOptionsTab.HostedWidget = new ExternalWidget("Promptu.OptionsTabs.Popular.Content", popularOptionsPanel);

            //IObjectPropertyCollectionEditor propertyEditor = Globals.GuiManager.ToolkitHost.Factory.ConstructObjectPropertyCollectionEditor();

            //this.optionsTabs = new TabWidget("Promptu.OptionsTabTabs");

            this.optionsTab.HostedWidget = new ExternalWidget("Promptu.OptionsTab.Panel", this.optionsTabPanel);//this.optionsTabs;

            OptionsGroupCollection popularProperties = new OptionsGroupCollection();
            popularOptionsPanel.Editor.Properties = popularProperties;

            OptionsGroup promptGroup = new OptionsGroup(
                "Promptu.PopularOptions.Prompt",
                Localization.UIResources.PromptGroupLabel);
                //Localization.UIResources.GeneralPopularOptionsLabel);

            popularProperties.Add(promptGroup);

            this.promptHotkeyProperty = new BoundObjectProperty<GlobalHotkey>(
                "Prompt.Hotkey",
                Localization.UIResources.PromptHotkeyLabel,
                "Hotkey",
                null);

            //this.promptHotkeyProperty.ValueChanged += this.HandleHotkeyWidgetHotkeyChanged;

            //this.promptHotkeyProperty = new BoundObjectProperty(
            //    "Prompt.Hotkey",
            //    Localization.UIResources.PromptHotkeyLabel,
            //    new GlobalHotkey(HotkeyModifierKeys.Win, System.Windows.Forms.Keys.Q, false),
            //    null,
            //    null);

            promptGroup.Add(this.promptHotkeyProperty);

            EnumConversionInfo positioningModeConversionInfo = new EnumConversionInfo(typeof(PositioningMode));
            positioningModeConversionInfo.Entries.Add(new EnumValueInfo(
                PositioningMode.CurrentScreen, 
                Localization.UIResources.PositioningModeCurrentScreen));

            positioningModeConversionInfo.Entries.Add(new EnumValueInfo(
                PositioningMode.FollowMouse,
                Localization.UIResources.PositioningModeFollowMouse));

            positioningModeConversionInfo.Entries.Add(new EnumValueInfo(
                PositioningMode.None,
                Localization.UIResources.PositioningModeNone));

            this.promptPositioningMode = new BoundObjectProperty<PositioningMode>(
                "Prompt.PositioningMode",
                Localization.UIResources.PromptPositioningModeLabel,
                "PromptPositioningMode",
                null,
                null,
                positioningModeConversionInfo,
                null);

            //new BackedObjectProperty<SkinApi.PositioningMode>(
            //    "Prompt.PositioningMode",
            //    Localization.UIResources.PromptPositioningModeLabel,
            //    PositioningMode.FollowMouse,
            //    null,
            //    positioningModeConversionInfo);

            promptGroup.Add(promptPositioningMode);

            OptionsGroup otherGroup = new OptionsGroup(
                "Promptu.PopularOptions.Other",
                "Other");

            popularProperties.Add(otherGroup);

            this.startPromptuWithComputer = new CallbackObjectProperty<bool>(
                "Promptu.StartWithComputer",
                Localization.UIResources.StartPromptuWithComputer,
                this.GetPromptuStartsWithComputer,
                this.SetPromptuStartsWithComputer,
                null,
                null);

            otherGroup.Add(this.startPromptuWithComputer);

            SuperTabPage suggestionsOptionsTab = new SuperTabPage("Promptu.OptionsTabs.Suggestions");
            suggestionsOptionsTab.Text = Localization.UIResources.SuggestionsTabText;
            this.optionsTabs.Add(suggestionsOptionsTab);

            IPromptuOptionsPanel suggestionOptionsPanel = InternalGlobals.GuiManager.ToolkitHost.Factory.ConstructOptionsPanel();
            suggestionOptionsPanel.MainInstructions = Localization.UIResources.SuggestionsOptionPanelMainInstructions;

            suggestionsOptionsTab.HostedWidget = new ExternalWidget("Promptu.OptionsTabs.Suggestions.Content", suggestionOptionsPanel);

            OptionsGroupCollection suggestionsProperties = new OptionsGroupCollection();
            suggestionOptionsPanel.Editor.Properties = suggestionsProperties;

            OptionsGroup suggestionIconGroup = new OptionsGroup(
                "Promptu.SuggestionsOptions.Icons",
                Localization.UIResources.SuggestionIconsGroupLabel);
            suggestionsProperties.Add(suggestionIconGroup);

            this.showCommandTargetIcons = new BoundObjectProperty<bool>(
                "ShowCommandTargetIcons",
                Localization.UIResources.ShowTargetIconsLabel,
                "ShowCommandTargetIcons",
                null);

            this.showCommandTargetIcons.ValueChanged += this.HandleShowCommandTargetIconsChanged;

            this.autoFetchFavicons = new BoundObjectProperty<bool>(
                "AutoFetchFavicons",
                Localization.UIResources.AutoFetchFaviconsLabel,
                "AutoFetchFavicons",
                null);

            this.autoFetchFavicons.ValueChanged += this.HandleAutoFetchFaviconsChanged;

            this.autoFetchFavicons.Indent = 1;

            this.refreshIconsButton = InternalGlobals.GuiManager.ToolkitHost.Factory.ConstructOptionButton();
            this.refreshIconsButton.Text = Localization.UIResources.RefreshIconsButtonText;
            this.refreshIconsButton.Indent = 1;
            this.refreshIconsButton.Click += this.HandleRefreshIconsButtonClick;

            this.refreshIconsProgressBar = InternalGlobals.GuiManager.ToolkitHost.Factory.ConstructOptionsProgressBar();
            this.refreshIconsProgressBar.Indent = 1;
            this.refreshIconsProgressBar.Text = Localization.UIResources.RefreshIconsProgressText;
            this.refreshIconsProgressBar.Visible = false;

            suggestionIconGroup.Add(this.showCommandTargetIcons);
            suggestionIconGroup.Add(this.autoFetchFavicons);
            suggestionIconGroup.Add(this.refreshIconsButton);
            suggestionIconGroup.Add(this.refreshIconsProgressBar);

            this.refreshIconsTimer = new Timer(RefreshIconsTimerInterval);
            this.refreshIconsTimer.AutoReset = false;
            this.refreshIconsTimer.Elapsed += this.HandleRefreshIconsTimerElapsed;
            
            OptionsGroup folderBrowsingGroup = new OptionsGroup(
                "Promptu.SuggestionsOptions.FolderBrowsing",
                Localization.UIResources.FolderBrowsingGroupLabel);
            suggestionsProperties.Add(folderBrowsingGroup);

            this.autoDetectFileAndFolderVisibility = new BoundObjectProperty<bool>(
                "AutoDetectFileAndFolderVisibility",
                Localization.UIResources.AutoDetectFileAndFolderVisibilityLabel,
                "AutoDetectFileAndFolderVisibility",
                null);

            this.autoDetectFileAndFolderVisibility.PropertyChanged += this.HandleAutoDetectFileAndFolderVisiblityChanged;

            //this.showSystemFilesAndFolders = new BackedObjectProperty<bool>(
            //    "ShowSystemFilesAndFolders",
            //    Localization.UIResources.ShowSystemFilesAndFolders,
            //    false,
            //    null,
            //    null);

            //this.showSystemFilesAndFolders.Indent = 1;

            //this.showHiddenFilesAndFolders = new BackedObjectProperty<bool>(
            //    "ShowHiddenFilesAndFolders",
            //    Localization.UIResources.ShowHiddenFilesAndFolders,
            //    false,
            //    null,
            //    null);

            //this.showHiddenFilesAndFolders.Indent = 1;

            this.showSystemFilesAndFolders = new BoundObjectProperty<bool>(
                "ShowSystemFilesAndFolders",
                Localization.UIResources.ShowSystemFilesAndFolders,
                "ShowSystemFilesAndFolders",
                null);

            this.showSystemFilesAndFolders.Indent = 1;

            this.showHiddenFilesAndFolders = new BoundObjectProperty<bool>(
                "ShowHiddenFilesAndFolders",
                Localization.UIResources.ShowHiddenFilesAndFolders,
                "ShowHiddenFilesAndFolders",
                null);

            this.showHiddenFilesAndFolders.Indent = 1;

            folderBrowsingGroup.Add(this.autoDetectFileAndFolderVisibility);
            folderBrowsingGroup.Add(this.showSystemFilesAndFolders);
            folderBrowsingGroup.Add(this.showHiddenFilesAndFolders);

            SuperTabPage profileOptionsTab = new SuperTabPage("Promptu.OptionsTabs.Profile");
            profileOptionsTab.Text = Localization.UIResources.ProfileOptionsTabText;
            this.optionsTabs.Add(profileOptionsTab);

            IPromptuOptionsPanel profilesOptionsPanel = InternalGlobals.GuiManager.ToolkitHost.Factory.ConstructOptionsPanel();
            profilesOptionsPanel.MainInstructions = Localization.UIResources.ProfileOptionPanelMainInstructions;

            profileOptionsTab.HostedWidget = new ExternalWidget("Promptu.OptionsTabs.Profile.Content", profilesOptionsPanel);

            OptionsGroupCollection profileOptions = new OptionsGroupCollection();
            profilesOptionsPanel.Editor.Properties = profileOptions;

            OptionsGroup currentProfileGroup = new OptionsGroup(
                "Promptu.ProfileOptions.CurrentProfile",
                Localization.UIResources.CurrentProfileGroupLabel);
            profileOptions.Add(currentProfileGroup);

            this.profileConfigPanel = InternalGlobals.GuiManager.ToolkitHost.Factory.ConstructProfileConfigPanel();
            this.profileConfigPanel.DeleteButton.Text = Localization.UIResources.DeleteButtonText;
            this.profileConfigPanel.RenameButton.Text = Localization.UIResources.RenameButtonText;
            this.profileConfigPanel.NewProfileButton.Text = Localization.UIResources.NewProfileButtonText;

            this.profileConfigPanel.CurrentProfiles.AddRange(InternalGlobals.ProfilePlacemarks);
            this.profileConfigPanel.CurrentProfiles.SelectedValue = InternalGlobals.PlacemarkOfCurrentProfile;
            this.profileConfigPanel.CurrentProfiles.SelectedIndexChanged += this.HandleProfilesComboBoxSelectionChanged;

            this.profileConfigPanel.RenameButton.Click += this.RenameProfileButtonClick;
            this.profileConfigPanel.NewProfileButton.Click += this.NewProfileButtonClick;
            this.profileConfigPanel.DeleteButton.Click += this.DeleteProfileButtonClick;

            currentProfileGroup.Add(this.profileConfigPanel);

            OptionsGroup profileOtherGroup = new OptionsGroup(
                "Promptu.ProfileOptions.Other",
                Localization.UIResources.ProfileOtherGroupLabel);
            profileOptions.Add(profileOtherGroup);

            this.clearHistoryButton = InternalGlobals.GuiManager.ToolkitHost.Factory.ConstructOptionButton();
            this.clearHistoryButton.Text = Localization.UIResources.ClearHistoryButtonText;
            this.clearHistoryButton.Click += this.HandleClearHistoryButtonClick;

            this.showSplashScreen = new BoundObjectProperty<bool>(
                "Profile.ShowSplashScreen",
                Localization.UIResources.ProfileShowSplashScreenLabel,
                "ShowSplashScreen",
                null);

            profileOtherGroup.Add(this.clearHistoryButton);
            profileOtherGroup.Add(this.showSplashScreen);

            SuperTabPage updatesOptionsTab = new SuperTabPage("Promptu.OptionsTabs.Updates");
            updatesOptionsTab.Text = Localization.UIResources.UpdatesOptionsTabText;
            this.optionsTabs.Add(updatesOptionsTab);

            IPromptuOptionsPanel updatesOptionsPanel = InternalGlobals.GuiManager.ToolkitHost.Factory.ConstructOptionsPanel();
            updatesOptionsPanel.MainInstructions = Localization.UIResources.UpdatesOptionPanelMainInstructions;

            updatesOptionsTab.HostedWidget = new ExternalWidget("Promptu.OptionsTabs.Updates.Content", updatesOptionsPanel);

            OptionsGroupCollection updatesOptions = new OptionsGroupCollection();
            updatesOptionsPanel.Editor.Properties = updatesOptions;

            OptionsGroup updatesOptionsGeneralGroup = new OptionsGroup(
                "Promptu.UpdatesOptions.General",
                Localization.UIResources.UpdatesGeneralGroupLabel);
            updatesOptions.Add(updatesOptionsGeneralGroup);

            this.checkForPreReleaseUpdates = new CallbackObjectProperty<bool>(
                "PreReleaseUpdates",
                Localization.UIResources.PreReleaseUpdatesLabel,
                this.GetCheckForPreReleaseUpdates,
                this.SetCheckForPreReleaseUpdates,
                null,
                null);

            updatesOptionsGeneralGroup.Add(this.checkForPreReleaseUpdates);

            if (InternalGlobals.CurrentReleaseType == ReleaseType.Beta || InternalGlobals.CurrentReleaseType == ReleaseType.Alpha)
            {
                this.usingPreReleaseMessage = new OptionsTextEntry(
                    Localization.UIResources.PreReleaseUsingPreReleaseSupplement,
                    TextEntryType.Error,
                    1);

                this.usingPreReleaseMessage.Visible = !InternalGlobals.Settings.CheckForPreReleaseUpdates;

                updatesOptionsGeneralGroup.Add(this.usingPreReleaseMessage);
            }

            updatesOptionsGeneralGroup.Add(
                new OptionsTextEntry(
                    Localization.UIResources.PreReleaseUpdatesSupplement, 
                    TextEntryType.Normal, 
                    1));

            IOptionButton contextualCheckForUpdatesButton = InternalGlobals.GuiManager.ToolkitHost.Factory.ConstructOptionButton();
            contextualCheckForUpdatesButton.Indent = 1;
            contextualCheckForUpdatesButton.Text = Localization.UIResources.CheckForUpdatesText;
            contextualCheckForUpdatesButton.Click += this.HandleCheckForUpdate;
            updatesOptionsGeneralGroup.Add(contextualCheckForUpdatesButton);

            OptionsGroup updatesOptionsProxyGroup = new OptionsGroup(
                "Promptu.UpdatesOptions.Proxy",
                Localization.UIResources.UpdatesProxyGroupLabel);
            updatesOptions.Add(updatesOptionsProxyGroup);

            BooleanConversionInfo proxyServerRadioGroup = new BooleanConversionInfo("proxyServer");

            this.noProxy = new BackedObjectProperty<bool>(
                "NoProxyServer",
                Localization.UIResources.NoProxyServerLabel,
                false,
                null,
                proxyServerRadioGroup);

            this.noProxy.ValueChanged += this.SerializeProxySettingsFromChangedEvent;
            updatesOptionsProxyGroup.Add(this.noProxy);

            this.useIEProxySettings = new BackedObjectProperty<bool>(
                "UseIEProxyServer",
                Localization.UIResources.UseIEProxyServerLabel,
                false,
                null,
                proxyServerRadioGroup);

            this.useIEProxySettings.ValueChanged += this.SerializeProxySettingsFromChangedEvent;
            updatesOptionsProxyGroup.Add(this.useIEProxySettings);

            this.useCustomProxySettings = new BackedObjectProperty<bool>(
                "UseCustomProxyServer",
                Localization.UIResources.UseCustomProxyServerLabel,
                false,
                null,
                proxyServerRadioGroup);

            this.useCustomProxySettings.ValueChanged += this.HandleCustomProxyValueChanged;
            updatesOptionsProxyGroup.Add(this.useCustomProxySettings);

            this.customProxyAddress = new BackedObjectProperty<string>(
                "CustomProxyAddress",
                Localization.UIResources.ProxyAddressLabelText,
                string.Empty,
                null,
                new TextConversionInfo("customProxy", true, "Example: http://myproxy:8080", 225));

            this.customProxyAddress.ValueChanged += this.HandleCustomProxyTextBoxTextChanged;
            this.customProxyAddress.Indent = 1;
            updatesOptionsProxyGroup.Add(customProxyAddress);

            this.customProxyUsername = new BackedObjectProperty<string>(
                "CustomProxyUsername",
                Localization.UIResources.ProxyUsernameLabelText,
                string.Empty,
                null,
                new GroupingConversionInfo("customProxy", true));

            this.customProxyUsername.ValueChanged += this.HandleCustomProxyTextBoxTextChanged;
            this.customProxyUsername.Indent = 1;
            updatesOptionsProxyGroup.Add(customProxyUsername);

            this.customProxyPassword = new BackedObjectProperty<SecureString>(
                "CustomProxyPassword",
                Localization.UIResources.ProxyPasswordLabelText,
                new SecureString(),
                null,
                new GroupingConversionInfo("customProxy", true));

            this.customProxyPassword.ValueChanged += this.HandleCustomProxyTextBoxTextChanged;
            this.customProxyPassword.Indent = 1;
            updatesOptionsProxyGroup.Add(customProxyPassword);

            this.optionsTabs.SelectedTab = this.popularOptionsTab;

            this.listSelector = new ListSelectorPresenter(this.profileTabPanel.ListSelector);
            this.listSelector.SelectedItemChanged += this.SelectedListChanged;
            this.listSelector.ListAdded += this.ListAdded;
            this.listSelector.ListDeleted += this.ListDeleted;
            this.listSelector.ListMoved += this.ListMoved;
            this.listSelector.UpdateSelectedItem += this.UpdateToCurrentList;

            this.UpdateProxySettings();

            this.appearanceTabPanel.AddSkins(InternalGlobals.Skins);

            this.UpdateUI(true);

            InternalGlobals.GuiManager.ToolkitHost.ApplicationExit += this.HandleApplicationExit;
        }

        //~SetupDialogPresenter()
        //{
        //    this.Dispose(false);
        //}

        public event EventHandler Closed;

        //public event EventHandler HotkeyChanged;

        //public void Dispose()
        //{
        //    this.Dispose(true);
        //    GC.SuppressFinalize(this);
        //}

        private void HandleRefreshIconsButtonClick(object sender, EventArgs e)
        {
            this.RefreshIconsInTheBackground(true);
        }

        private void HandleRefreshIconsTimerElapsed(object sender, EventArgs e)
        {
            this.RefreshIconsInTheBackground(false);
        }

        private void RefreshIconsInTheBackground(bool userInitiated)
        {
            BackgroundWorker worker = this.refreshIconsBackgroundWorker;
            if (worker != null)
            {
                worker.CancelAsync();
            }

            worker = new BackgroundWorker();
            worker.WorkerSupportsCancellation = true;

            this.doneMainIconRefresh = true;
            this.doneFaviconRefresh = userInitiated || this.autoFetchFavicons.Value;

            if (userInitiated)
            {
                this.refreshIconsProgressBar.IsIndeterminate = true;
                this.refreshIconsProgressBar.PercentComplete = 0;
                this.refreshIconsProgressBar.Visible = true;
                this.refreshIconsButton.Visible = false;
                worker.DoWork += this.HandleRefreshIconsAsyncUserInitiated;
                //worker.ProgressChanged += this.HandleRefreshIconsAsyncProgressChanged;
                worker.RunWorkerCompleted += this.HandleRefrehsIconsAsyncCompleted;
            }
            else
            {
                worker.DoWork += this.HandleRefreshIconsAsync;
            }

            worker.RunWorkerAsync();
            this.refreshIconsBackgroundWorker = worker;
        }

        //private void HandleRefreshIconsAsyncProgressChanged(object sender, ProgressChangedEventArgs e)
        //{
        //    this.refreshIconsProgressBar.IsIndeterminate = false;
        //    this.refreshIconsProgressBar.PercentComplete = e.ProgressPercentage;
        //}

        private void HandleRefrehsIconsAsyncCompleted(object sender, EventArgs e)
        {
            this.refreshIconsProgressBar.Visible = false;
            this.refreshIconsButton.Visible = true;
        }

        private void HandleRefreshIconsAsyncUserInitiated(object sender, DoWorkEventArgs e)
        {
            InternalGlobals.CurrentProfile.UpdateAllCachedIcons(true, false, e, this.refreshIconsProgressBar);
        }

        private void HandleRefreshIconsAsync(object sender, DoWorkEventArgs e)
        {
            InternalGlobals.CurrentProfile.UpdateAllCachedIcons(false, false, e, null);
        }

        protected override void Dispose(bool disposing)
        {
            //((IPromptuOptionsPanel)((ExternalWidget)this.popularOptionsTab.HostedWidget).NativeObject).Editor.Properties.DisposeAllChildren();
            //((IPromptuOptionsPanel)((ExternalWidget)this.por.HostedWidget).NativeObject).Editor.Properties.DisposeAllChildren();
            this.checkForPreReleaseUpdates.Dispose();
            this.startPromptuWithComputer.Dispose();
            this.customProxyAddress.Dispose();
            this.customProxyPassword.Dispose();
            this.customProxyUsername.Dispose();
            this.noProxy.Dispose();
            this.useIEProxySettings.Dispose();
            this.useCustomProxySettings.Dispose();
            this.showHiddenFilesAndFolders.Dispose();
            this.showSystemFilesAndFolders.Dispose();
            this.autoDetectFileAndFolderVisibility.Dispose();
            this.showSplashScreen.Dispose();
            this.promptPositioningMode.Dispose();
            this.promptHotkeyProperty.Dispose();

            this.profileConfigPanel.RenameButton.Click -= this.RenameProfileButtonClick;
            this.profileConfigPanel.NewProfileButton.Click -= this.NewProfileButtonClick;
            this.profileConfigPanel.DeleteButton.Click -= this.DeleteProfileButtonClick;

            this.listSelector.UpdateSelectedItem -= this.UpdateToCurrentList;
            this.aboutPanel.WebsiteLinkClicked -= this.VisitWebsite;
            this.aboutPanel.CheckForUpdates.Click -= this.HandleCheckForUpdate;

            this.listSelector.SelectedItemChanged -= this.SelectedListChanged;
            this.listSelector.ListAdded -= this.ListAdded;
            this.listSelector.ListDeleted -= this.ListDeleted;
            this.listSelector.ListMoved -= this.ListMoved;
            this.listSelector.UpdateSelectedItem -= this.UpdateToCurrentList;

            this.aboutPanel.WebsiteLinkClicked -= this.VisitWebsite;
            this.aboutPanel.CheckForUpdates.Click -= this.HandleCheckForUpdate;
            //this.pluginsTabPanel.CreatorContactLinkClicked -= this.HandlePluginCreatorContactLinkClicked;
            //this.pluginsTabPanel.TogglePluginEnabledClicked -= this.HandlePluginToggleEnabled;

            this.appearanceTabPanel.CreatorContactLinkClicked -= this.HandleSkinCreatorContactLinkClicked;
            this.appearanceTabPanel.ConfigureSkinClicked -= this.HandleConfigureSkinClicked;
            this.appearanceTabPanel.SelectedSkinChanged -= this.HandleSkinChanged;

            this.clearHistoryButton.Click -= this.HandleClearHistoryButtonClick;

            //this.assemblyReferenceSetupPanel.

            this.mainTabs.SelectedTabChanged -= this.HandleMainTabsSelectedIndexChanged;
            this.listTabs.SelectedTabChanged -= this.HandleSwitchBetweenTabs;

            this.serializeProxySettingsTimer.Elapsed -= this.HandleSerializeProxySettingsTimerElapsed;

            this.profileConfigPanel.CurrentProfiles.SelectedIndexChanged -= this.HandleProfilesComboBoxSelectionChanged;
            InternalGlobals.GuiManager.ToolkitHost.ApplicationExit -= this.HandleApplicationExit;
            this.autoDetectFileAndFolderVisibility.PropertyChanged -= this.HandleAutoDetectFileAndFolderVisiblityChanged;

            base.Dispose(disposing);
        }

        public void UpdatePluginDisplays()
        {
            this.getPluginsPanel.ClearPlugins();
            this.getPluginsPanel.AddPlugins(InternalGlobals.AvailablePlugins);
            this.UpdateInstalledPlugins();
        }

        private void DeleteProfileButtonClick(object sender, EventArgs e)
        {
            if (InternalGlobals.ProfilePlacemarks.Count <= 1)
            {
                UIMessageBox.Show(
                    Localization.Promptu.CannotDeleteProfileBecauseThereIsOnlyOne,
                    Localization.Promptu.AppName,
                    UIMessageBoxButtons.OK,
                    UIMessageBoxIcon.Error);
                return;
            }

            if (UIMessageBox.Show(
                String.Format(CultureInfo.CurrentCulture, Localization.MessageFormats.ConfirmDeleteProfile, InternalGlobals.CurrentProfile.Name),
                Localization.Promptu.AppName,
                UIMessageBoxButtons.YesNo,
                UIMessageBoxIcon.Asterisk,
                UIMessageBoxResult.No) == UIMessageBoxResult.Yes)
            {
                InternalGlobals.SyncSynchronizer.CancelSyncsAndWait();
                Profile profile = InternalGlobals.CurrentProfile;
                int index = InternalGlobals.ProfilePlacemarks.IndexOf(profile.FolderName);
                if (index >= InternalGlobals.ProfilePlacemarks.Count - 1)
                {
                    index = InternalGlobals.ProfilePlacemarks.Count - 2;
                }

                InternalGlobals.ProfilePlacemarks.Remove(profile.FolderName);
                profile.Directory.Delete();
                this.updating = true;
                this.profileConfigPanel.CurrentProfiles.Clear();
                this.profileConfigPanel.CurrentProfiles.AddRange(InternalGlobals.ProfilePlacemarks.ToArray());
                this.updating = false;
                this.profileConfigPanel.CurrentProfiles.SelectedIndex = index;
            }
        }

        private void NewProfileButtonClick(object sender, EventArgs e)
        {
            Profile newProfile = null;
            while (newProfile == null)
            {
                newProfile = Profile.CreateNewFromUI();

                NewUserDialogPresenter dialog = new NewUserDialogPresenter(NewUserDialogContext.NewProfile);

                if (dialog.ShowDialog() != UIDialogResult.OK)
                {
                    InternalGlobals.SyncSynchronizer.CancelSyncsAndWait();
                    InternalGlobals.ProfilePlacemarks.Remove(newProfile.FolderName);
                    newProfile.Directory.Delete();
                    return;
                }

                newProfile.Name = dialog.ProfileName;
                dialog.HotkeyPresenter.ImpartTo(newProfile.Hotkey);
            }

            this.updating = true;
            //PromptuSettings.Profiles.Add(newProfile);
            this.profileConfigPanel.CurrentProfiles.Clear();
            this.profileConfigPanel.CurrentProfiles.AddRange(InternalGlobals.ProfilePlacemarks.ToArray());
            this.profileConfigPanel.CurrentProfiles.SelectedValue = InternalGlobals.ProfilePlacemarks[newProfile.FolderName];
            PromptHandler.GetInstance().SwitchToProfile(newProfile, false);
            InternalGlobals.CurrentProfile.SyncLocalAssemblyReferences();
            this.updating = false;
            this.UpdateUI(false);
        }

        private void RenameProfileButtonClick(object sender, EventArgs e)
        {
            RenameDialogPresenter dialog = new RenameDialogPresenter(InternalGlobals.CurrentProfile.Name, Localization.UIResources.RenameProfileMainInstructions);
            if (dialog.ShowDialog() == UIDialogResult.OK)
            {
                InternalGlobals.CurrentProfile.Name = dialog.Value;
                this.updating = true;
                this.profileConfigPanel.CurrentProfiles.Clear();
                this.profileConfigPanel.CurrentProfiles.AddRange(InternalGlobals.ProfilePlacemarks);
                this.updating = false;
                this.profileConfigPanel.CurrentProfiles.SelectedValue = InternalGlobals.PlacemarkOfCurrentProfile;
            }
        }

        private void HandleProfilesComboBoxSelectionChanged(object sender, EventArgs e)
        {
            if (this.updating)
            {
                return;
            }

            ProfilePlacemark profilePlacemark = (ProfilePlacemark)this.profileConfigPanel.CurrentProfiles.SelectedValue;

            if (profilePlacemark.IsExternallyLocked)
            {
                UIMessageBox.Show(
                    String.Format(CultureInfo.CurrentCulture, Localization.UIResources.SelectedProfileLockedMessage, profilePlacemark.Name, profilePlacemark.Locker),
                    Localization.Promptu.AppName,
                    UIMessageBoxButtons.OK,
                    UIMessageBoxIcon.Error);
                this.profileConfigPanel.CurrentProfiles.SelectedValue = InternalGlobals.PlacemarkOfCurrentProfile;
                return;
            }

            try
            {
                Profile profile = profilePlacemark.GetEntireProfile();
                PromptHandler.GetInstance().SwitchToProfile(profile, false);
                InternalGlobals.CurrentProfile.SyncLocalAssemblyReferences();
            }
            catch (ProfileLoadException)
            {
                UIMessageBox.Show(
                    Localization.MessageFormats.ProfileIsCorrupted,
                    Localization.Promptu.AppName,
                    UIMessageBoxButtons.OK,
                    UIMessageBoxIcon.Error);
                InternalGlobals.ProfilePlacemarks.Remove(profilePlacemark);
            }

            this.UpdateUI(false);
        }

        private void HandleClearHistoryButtonClick(object sender, EventArgs e)
        {
            InternalGlobals.CurrentProfile.History.Clear();
            InternalGlobals.CurrentProfile.History.Save();
        }

        private bool GetPromptuStartsWithComputer()
        {
            return InternalGlobals.AutoStartWithComputer;
        }

        private void SetPromptuStartsWithComputer(bool value)
        {
            InternalGlobals.AutoStartWithComputer = value;
        }

        private bool GetCheckForPreReleaseUpdates()
        {
            return InternalGlobals.Settings.CheckForPreReleaseUpdates;
        }

        private void SetCheckForPreReleaseUpdates(bool value)
        {
            InternalGlobals.Settings.CheckForPreReleaseUpdates = value;

            if (this.usingPreReleaseMessage != null)
            {
                this.usingPreReleaseMessage.Visible = !value;
            }

            InternalGlobals.Settings.Save();
        }

        private void HandleAutoDetectFileAndFolderVisiblityChanged(object sender, EventArgs e)
        {
            bool autoDetect = (bool)this.autoDetectFileAndFolderVisibility.ObjectValue;

            this.showSystemFilesAndFolders.IsEnabled = !autoDetect;
            this.showHiddenFilesAndFolders.IsEnabled = !autoDetect;
        }

        private void HandleAutoFetchFaviconsChanged(object sender, EventArgs e)
        {
            this.refreshIconsTimer.Stop();

            if (this.autoFetchFavicons.Value && (!doneMainIconRefresh || !doneFaviconRefresh))
            {
                this.refreshIconsTimer.Start();
            }
        }

        private void HandleShowCommandTargetIconsChanged(object sender, EventArgs e)
        {
            this.refreshIconsTimer.Stop();

            bool subItemsEnabled = this.showCommandTargetIcons.Value;
            this.autoFetchFavicons.IsEnabled = subItemsEnabled;
            this.refreshIconsButton.Enabled = subItemsEnabled;

            if (this.showCommandTargetIcons.Value && !this.doneMainIconRefresh)
            {
                this.refreshIconsTimer.Start();
            }

            BackgroundWorker worker = new BackgroundWorker();
            worker.DoWork += this.NotifySuggestionAffectingChangeAsync;
            worker.RunWorkerAsync();
        }

        private void NotifySuggestionAffectingChangeAsync(object sender, EventArgs e)
        {
            PromptHandler.GetInstance().NotifySuggestionAffectingChange();
        }

        public ListSelectorPresenter ListSelector
        {
            get { return this.listSelector; }
        }

        public TabWidget ListTabs
        {
            get { return this.listTabs; }
        }

        public CommandSetupPanelPresenter CommandSetupPanel
        {
            get { return this.commandSetupPanel; }
        }

        public FunctionSetupPanelPresenter FunctionSetupPanel
        {
            get { return this.functionSetupPanel; }
        }

        public void Show()
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new ParameterlessVoid(this.ShowInternal));
            }
            else
            {
                this.ShowInternal();
            }
            //this.NativeInterface.ShowThreadSafe();
            //this.NativeInterface.UnMinimizeIfNecessary();
            //this.NativeInterface.ActivateThreadSafe();
        }

        private void ShowInternal()
        {
            // HACK
            //Globals.UISettings.SetupDialogSettings.ImpartTo(this);
            //this.UpdateHelpDisplay();
            //this.UpdateProxySettings();

            if (!this.NativeInterface.IsShown)
            {
                this.mainTabs.SelectedTab = this.profileTab;
            }

            this.NativeInterface.Refresh();

            this.NativeInterface.ShowThreadSafe();
            this.NativeInterface.UnMinimizeIfNecessary();
            this.NativeInterface.ActivateThreadSafe();
        }

        public void DoItemPaste()
        {
            if (this.mainTabs.SelectedTab == this.profileTab)
            {
                if (this.listTabs.SelectedTab == this.commandsTab)
                {
                    this.commandSetupPanel.Paste();
                }
                if (this.listTabs.SelectedTab == this.assemblyReferencesTab)
                {
                    this.assemblyReferenceSetupPanel.Paste();
                }
                else if (this.listTabs.SelectedTab == this.functionsTab)
                {
                    this.functionSetupPanel.Paste();
                }
                else if (this.listTabs.SelectedTab == this.valueListsTab)
                {
                    this.valueListSetupPanel.Paste();
                }
            }
        }

        private void SelectedListChanged(object sender, EventArgs e)
        {
            InternalGlobals.CurrentProfile.SelectedListIndex = this.listSelector.SelectedIndex;
            this.commandSetupPanel.ClearSelectedIndices();
            this.assemblyReferenceSetupPanel.ClearSelectedIndices();
            this.functionSetupPanel.ClearSelectedIndices();
            this.valueListSetupPanel.ClearSelectedIndices();
            this.UpdateToCurrentList();
        }

        public void EditNewCommand(List list, Command contents)
        {
            this.listSelector.SelectedIndex = InternalGlobals.CurrentProfile.Lists.IndexOf(list);
            this.mainTabs.SelectedTab = this.profileTab;
            this.listTabs.SelectedTab = this.commandsTab;
            this.commandSetupPanel.CreateNewItem(contents);
        }

        public void EditCommand(string name)
        {
            bool found;
            GroupedCompositeItem item = InternalGlobals.CurrentProfile.CompositeFunctionsAndCommands.TryGetItem(name, out found);
            if (found && item != null && item.Commands.Count > 0)
            {
                this.listSelector.SelectedIndex = InternalGlobals.CurrentProfile.Lists.IndexOf(item.Commands[0].ListFrom);
                this.mainTabs.SelectedTab = this.profileTab;
                this.listTabs.SelectedTab = this.commandsTab;
                this.commandSetupPanel.EditCommand(name);
            }
        }

        public void EditCommand(String name, List listFrom, Command contents)
        {
            this.listSelector.SelectedIndex = InternalGlobals.CurrentProfile.Lists.IndexOf(listFrom);
            this.mainTabs.SelectedTab = this.profileTab;
            this.listTabs.SelectedTab = this.commandsTab;
            this.commandSetupPanel.EditCommand(name, contents);
        }

        public void CreateNewCommand(string listFolderName)
        {
            List list = InternalGlobals.CurrentProfile.Lists.TryGet(listFolderName);
            if (list != null)
            {
                this.listSelector.SelectedIndex = InternalGlobals.CurrentProfile.Lists.IndexOf(list);
                this.mainTabs.SelectedTab = this.profileTab;
                this.listTabs.SelectedTab = this.commandsTab;
                this.commandSetupPanel.CreateNewItem();
            }
        }

        public void EditFunction(string name, string parameterSignature, List listFrom)
        {
            this.listSelector.SelectedIndex = InternalGlobals.CurrentProfile.Lists.IndexOf(listFrom);
            this.mainTabs.SelectedTab = this.profileTab;
            this.listTabs.SelectedTab = this.functionsTab;
            this.functionSetupPanel.EditFunction(name, parameterSignature);
        }

        public void EditFunction(string name, string parameterSignature)
        {
            CompositeItem<Function, List> function;
            bool found;
            GroupedCompositeItem item = InternalGlobals.CurrentProfile.CompositeFunctionsAndCommands.TryGetItem(name, out found);
            if (found && item != null && (function = item.TryGetFunction(name, parameterSignature)) != null)
            {
                this.listSelector.SelectedIndex = InternalGlobals.CurrentProfile.Lists.IndexOf(function.ListFrom);
                this.mainTabs.SelectedTab = this.profileTab;
                this.listTabs.SelectedTab = this.functionsTab;
                this.functionSetupPanel.EditFunction(name, parameterSignature);
            }
        }

        private void UpdateSetupDialogSettings()
        {
            if (!this.updating && this.NativeInterface.AllowSaveSettings)
            {
                InternalGlobals.UISettings.SetupDialogSettings.UpdateFrom(this.NativeInterface);
            }
        }

        private void UpdateCommandSetupPanelSettings()
        {
            if (!this.updating)
            {
                InternalGlobals.UISettings.CommandSetupPanelSettings.UpdateFrom(this.commandSetupPanel.NativeInterface);
            }
        }

        private void UpdateValueListSetupPanelSettings()
        {
            if (!this.updating)
            {
                InternalGlobals.UISettings.ValueListSetupPanelSettings.UpdateFrom(this.valueListSetupPanel.NativeInterface);
            }
        }

        private void UpdateFunctionSetupPanelSettings()
        {
            if (!this.updating)
            {
                InternalGlobals.UISettings.FunctionSetupPanelSettings.UpdateFrom(this.functionSetupPanel.NativeInterface);
            }

        }

        private void UpdateAssemblyReferenceSetupPanelSettings()
        {
            if (!this.updating)
            {
                InternalGlobals.UISettings.AssemblyReferenceSetupPanelSettings.UpdateFrom(this.assemblyReferenceSetupPanel.NativeInterface);
            }
        }

        private void UpdateProfileTabSettings()
        {
            if (!this.updating)
            {
                InternalGlobals.UISettings.ProfileTabSettings.UpdateFrom(this.profileTabPanel);
            }
        }

        private void ListAdded(object sender, ListAddedOrDeletedEventArgs e)
        {
            InternalGlobals.CurrentProfile.Lists[e.Index].UpdateCachedIconsAsync();
            this.UpdateTo(e.Index);
            PromptHandler.NotifyListsChanged();
        }

        private void ListDeleted(object sender, ListAddedOrDeletedEventArgs e)
        {
            int indexToUpdateTo;
            if (e.Index == 0 && InternalGlobals.CurrentProfile.Lists.Count > 1)
            {
                indexToUpdateTo = e.Index;
            }
            else
            {
                indexToUpdateTo = e.Index - 1;
            }

            this.UpdateTo(indexToUpdateTo);

            PromptHandler.NotifyListsChanged();
        }

        private void ListMoved(object sender, ListMovedEventArgs e)
        {
            this.NativeInterface.BeginUpdate();
            this.UpdateTo(e.NewIndex);
            this.NativeInterface.EndUpdate();
        }

        private void UpdateTo(int listIndex)
        {
            InternalGlobals.CurrentProfile.SelectedListIndex = listIndex;
            this.UpdateToCurrentList();
            this.listSelector.SelectedIndex = listIndex;
        }

        private void UpdateToCurrentList(object sender, EventArgs e)
        {
            this.UpdateToCurrentList();
        }

        public void UpdateToCurrentListThreadSafe()
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new ParameterlessVoid(this.UpdateToCurrentList));
            }
            else
            {
                this.UpdateToCurrentList();
            }
        }

        public void UpdateToCurrentList()
        {
            int selectedIndex = -1;
            if (InternalGlobals.CurrentProfile == null)
            {
            }
            else
            {
                selectedIndex = InternalGlobals.CurrentProfile.SelectedListIndex;
            }

            this.assemblyReferenceSetupPanel.UpdateToCurrentList();
            this.functionSetupPanel.UpdateToCurrentList();
            this.commandSetupPanel.UpdateToCurrentList();
            this.valueListSetupPanel.UpdateToCurrentList();

            this.listSelector.SelectedIndex = selectedIndex;
            this.NativeInterface.Refresh();
        }

        private void HandleSkinChanged(object sender, EventArgs e)
        {
            if (this.updating)
            {
                return;
            }

            PromptuSkin selectedSkin = this.appearanceTabPanel.SelectedSkin;
            if (selectedSkin == InternalGlobals.CurrentSkin)
            {
                return;
            }

            if (!PromptHandler.IsInitializing)
            {
                PromptHandler.GetInstance().ChangeSkin(selectedSkin);
            }
        }

        private void SelectSkin(PromptuSkin skin)
        {
            this.appearanceTabPanel.SelectedSkin = skin;
        }

        private void UpdateUI(bool impartSetupDialogSettings)
        {
            this.updating = true;

            this.SelectSkin(InternalGlobals.CurrentSkin);

            Profile profileDataContext = InternalGlobals.CurrentProfile;

            this.doneFaviconRefresh = profileDataContext.ShowCommandTargetIcons && profileDataContext.AutoFetchFavicons;
            this.doneMainIconRefresh = profileDataContext.ShowCommandTargetIcons;

            this.promptHotkeyProperty.DataContext = profileDataContext;
            this.promptPositioningMode.DataContext = profileDataContext;
            this.showHiddenFilesAndFolders.DataContext = profileDataContext;
            this.showSystemFilesAndFolders.DataContext = profileDataContext;
            this.autoDetectFileAndFolderVisibility.DataContext = profileDataContext;
            this.showCommandTargetIcons.DataContext = profileDataContext;
            this.autoFetchFavicons.DataContext = profileDataContext;
            this.showSplashScreen.DataContext = profileDataContext;

            //HACK this.pluginsTabPanel.ClearPlugins();
            //HACK this.pluginsTabPanel.AddPlugins(InternalGlobals.CurrentProfile.Plugins);
            //this.itemPropertyGrid.SelectedObject = this.itemPropertyGrid.SelectedObject;

            //this.proxyTableLayoutPanel.Height = this.proxyTableLayoutPanel.GetPreferredSize(new Size(this.proxyTableLayoutPanel.Width, 0)).Height;

            InternalGlobals.UISettings.AssemblyReferenceSetupPanelSettings.ImpartTo(this.assemblyReferenceSetupPanel.NativeInterface);
            InternalGlobals.UISettings.FunctionSetupPanelSettings.ImpartTo(this.functionSetupPanel.NativeInterface);
            InternalGlobals.UISettings.ValueListSetupPanelSettings.ImpartTo(this.valueListSetupPanel.NativeInterface);
            InternalGlobals.UISettings.CommandSetupPanelSettings.ImpartTo(this.commandSetupPanel.NativeInterface);
            InternalGlobals.UISettings.ProfileTabSettings.ImpartTo(this.profileTabPanel);
            if (impartSetupDialogSettings)
            {
                InternalGlobals.UISettings.SetupDialogSettings.ImpartTo(this.NativeInterface);
            }

            // HACK
            /*
            GlobalHotkey hotkey = Globals.CurrentProfile.Hotkey;

            try
            {
                this.hotkeyWidget.UnderlyingHotkeyKey = hotkey.Key;
                //this.hotkeySetupPanel.HotkeyKey = hotkeySetupPanel.
            }
            catch (ArgumentException)
            {
                UIMessageBox.Show(
                    Localization.Promptu.UnableToLoadHotkeyMessage,
                    Localization.Promptu.AppName,
                    UIMessageBoxButtons.OK,
                    UIMessageBoxIcon.Error,
                    UIMessageBoxResult.OK);
                Globals.CurrentProfile.Hotkey.Key = System.Windows.Forms.Keys.Q;
                this.hotkeyWidget.UnderlyingHotkeyKey = System.Windows.Forms.Keys.Q;
            }

            this.hotkeyWidget.HotkeyModifierKeys = hotkey.ModifierKeys;
            this.hotkeyWidget.OverrideHotkey = hotkey.OverrideIfNecessary;

            */

            //try
            //{
            //    this.notesHotkeySetupPanel.UnderlyingHotkeyKeyValue = Globals.CurrentProfile.NotesHotkey.Key;
            //}
            //catch (ArgumentException)
            //{
            //    Globals.CurrentProfile.Hotkey.Key = Keys.J;
            //    this.hotkeySetupPanel.UnderlyingHotkeyKeyValue = Keys.J;
            //}

            //this.notesHotkeySetupPanel.HotkeyModifierKeys = Globals.CurrentProfile.NotesHotkey.ModifierKeys;
            //this.notesHotkeySetupPanel.OverrideIfAlreadyInUse = Globals.CurrentProfile.NotesHotkey.ForceRegister;
            //this.notesHotkeySetupPanel.HotkeyEnabled = Globals.CurrentProfile.NotesHotkey.Enabled;

            //this.followMouseCheckBox.Checked = Globals.CurrentProfile.FollowMouse;

            this.listSelector.UpdateLists();
            this.listSelector.UpdateUI();
            this.UpdateToCurrentList();

            //this.showHiddenFilesAndFoldersCheckBox.Checked = Globals.CurrentProfile.NavigateHiddenFilesAndFolders;
            //this.showHiddenFilesAndFoldersCheckBox.Checked = Globals.CurrentProfile.NavigateSystemFilesAndFolders;

            //int previousDistance = this.skinSplitContainer.ReverseSplitterDistance;
            //this.skinSplitContainer.ReverseSplitterDistance = 10;
            //this.Refresh();
            //this.skinSplitContainer.ReverseSplitterDistance = previousDistance;

            this.updating = false;
        }

        private void HandleClosing(object sender, CancelEventArgs e)
        {
           // e.Cancel = true;
            InternalGlobals.UISettings.SetupDialogSettings.UpdateFrom(this.NativeInterface);
            //this.NativeInterface.Hide();
            this.mainTabs.SelectedTab = this.profileTab;
            this.listTabs.SelectedTab = this.commandsTab;
        }

        //private void UpdateHelpDisplay()
        //{
        //    string help;
        //    if (this.mainTabs.SelectedTab == this.profileTab)
        //    {
        //        help = Localization.InlineHelp.ProfileTab;
        //    }
        //    else if (this.mainTabs.SelectedTab == this.optionsTab)
        //    {
        //        help = Localization.InlineHelp.OptionsTab;
        //    }
        //    else if (this.mainTabs.SelectedTab == this.appearanceTab)
        //    {
        //        help = Localization.InlineHelp.VisualSettingsTab;
        //    }
        //    else
        //    {
        //        help = String.Empty;
        //    }

        //    this.NativeInterface.CurrentPageHelpText = help;
        //}

        public void BeginInvoke(Delegate method, object[] args)
        {
            this.NativeInterface.BeginInvoke(method, args);
        }

        //public object EndInvoke(IAsyncResult result)
        //{
        //    return this.NativeInterface.EndInvoke(result);
        //}

        public object Invoke(Delegate method, params object[] args)
        {
            return this.NativeInterface.Invoke(method, args);
        }

        public bool InvokeRequired
        {
            get { return this.NativeInterface.InvokeRequired; }
        }

        //private void HandleHotkeyWidgetHotkeyChanged(object sender, EventArgs e)
        //{
        //    if (this.updating)
        //    {
        //        return;
        //    }

        //    //InternalGlobals.CurrentProfile.SaveConfig();

        //    this.OnHotkeyChanged(e);
        //}

        //protected virtual void OnHotkeyChanged(EventArgs e)
        //{
        //    EventHandler handler = this.HotkeyChanged;
        //    if (handler != null)
        //    {
        //        handler(this, e);
        //    }
        //}

        private void HandleSwitchBetweenTabs(object sender, EventArgs e)
        {
            this.UpdateUIForSelectedTab();
        }

        private void UpdateUIForSelectedTab()
        {
            if (this.mainTabs.SelectedTab == this.profileTab)
            {
                if (this.listTabs.SelectedTab == this.commandsTab)
                {
                    this.commandSetupPanel.SelectAndUpdate();
                }
                if (this.listTabs.SelectedTab == this.assemblyReferencesTab)
                {
                    this.assemblyReferenceSetupPanel.SelectAndUpdate();
                }
                else if (this.listTabs.SelectedTab == this.functionsTab)
                {
                    this.functionSetupPanel.SelectAndUpdate();
                }
                else if (this.listTabs.SelectedTab == this.valueListsTab)
                {
                    this.valueListSetupPanel.SelectAndUpdate();
                }
            }
        }

        private void HandleMainTabsSelectedIndexChanged(object sender, EventArgs e)
        {
            //this.UpdateHelpDisplay();

            if (this.mainTabs.SelectedTab == this.aboutTab)
            {
                this.UpdateAboutImage();
            }
            else
            {
                //Image image = this.aboutPictureBox.Image;
                //if (image != null)
                //{
                //    image.Dispose();
                //    this.aboutPictureBox.Image = null;
                //}

                if (this.mainTabs.SelectedTab == this.optionsTab)
                {
                    this.UpdateProxySettings();
                }
            }

            this.UpdateUIForSelectedTab();
        }

        private void UpdateProxySettings()
        {
            try
            {
                this.userChangingProxySettings = false;
                switch (InternalGlobals.Settings.Proxy.Mode)
                {
                    case ProxyMode.FromIE:
                        this.useIEProxySettings.ObjectValue = true;
                        break;
                    case ProxyMode.CustomSettings:
                        this.useCustomProxySettings.ObjectValue = true;
                        break;
                    default:
                        this.noProxy.ObjectValue = true;
                        break;
                }

                this.customProxyAddress.ObjectValue = InternalGlobals.Settings.Proxy.Address;
                this.customProxyUsername.ObjectValue = InternalGlobals.Settings.Proxy.Username;
                this.customProxyPassword.ObjectValue = InternalGlobals.Settings.Proxy.Password;

                bool isCustomProxySettings = (bool)this.useCustomProxySettings.ObjectValue;

                this.customProxyAddress.IsEnabled = isCustomProxySettings;
                this.customProxyUsername.IsEnabled = isCustomProxySettings;
                this.customProxyPassword.IsEnabled = isCustomProxySettings;
            }
            finally
            {
                this.userChangingProxySettings = true;
            }
        }

        private void HandleApplicationExit(object sender, EventArgs e)
        {
            if (this.serializeProxySettingsTimer.Enabled)
            {
                this.serializeProxySettingsTimer.Stop();
                this.SerializeProxySettings(null);
            }
        }

        private void SerializeProxySettingsFromChangedEvent(object sender, EventArgs e)
        {
            ObjectPropertyBase property = sender as ObjectPropertyBase;
            if (property != null && !(bool)property.ObjectValue)
            {
                return;
            }

            this.SerializeProxySettings(property);
        }

        private void HandleCustomProxyTextBoxTextChanged(object sender, EventArgs e)
        {
            if (this.userChangingProxySettings)
            {
                this.serializeProxySettingsTimer.Stop();
                this.serializeProxySettingsTimer.Start();
            }
        }

        private void HandleSerializeProxySettingsTimerElapsed(object sender, EventArgs e)
        {
            this.serializeProxySettingsTimer.Stop();
            this.SerializeProxySettings(null);
        }

        private void SerializeProxySettings(ObjectPropertyBase changedToTrueProperty)
        {
            if (this.userChangingProxySettings)
            {
                bool setMode = false;

                if (changedToTrueProperty != null)
                {
                    if (changedToTrueProperty == this.useIEProxySettings)
                    {
                        InternalGlobals.Settings.Proxy.Mode = ProxyMode.FromIE;
                        setMode = true;
                    }
                    else if (changedToTrueProperty == this.useCustomProxySettings)
                    {
                        InternalGlobals.Settings.Proxy.Mode = ProxyMode.CustomSettings;
                        setMode = true;
                    }
                    else if (changedToTrueProperty == this.noProxy)
                    {
                        InternalGlobals.Settings.Proxy.Mode = ProxyMode.NoProxy;
                        setMode = true;
                    }
                }

                if (!setMode)
                {
                    if ((bool)this.useIEProxySettings.ObjectValue)
                    {
                        InternalGlobals.Settings.Proxy.Mode = ProxyMode.FromIE;
                    }
                    else if ((bool)this.useCustomProxySettings.ObjectValue)
                    {
                        InternalGlobals.Settings.Proxy.Mode = ProxyMode.CustomSettings;
                    }
                    else
                    {
                        InternalGlobals.Settings.Proxy.Mode = ProxyMode.NoProxy;
                    }
                }

                InternalGlobals.Settings.Proxy.Address = (string)this.customProxyAddress.ObjectValue;
                InternalGlobals.Settings.Proxy.Password = (SecureString)this.customProxyPassword.ObjectValue;
                InternalGlobals.Settings.Proxy.Username = (string)this.customProxyUsername.ObjectValue;
                InternalGlobals.Settings.Save();
            }
        }

        private void HandleCustomProxyValueChanged(object sender, EventArgs e)
        {
            bool isCustomProxySettings = (bool)this.useCustomProxySettings.ObjectValue;

            this.customProxyAddress.IsEnabled = isCustomProxySettings;
            this.customProxyUsername.IsEnabled = isCustomProxySettings;
            this.customProxyPassword.IsEnabled = isCustomProxySettings;

            if (isCustomProxySettings)
            {
                this.SerializeProxySettings(this.useCustomProxySettings);
            }
        }

        public void ShowUserToAbout()
        {
            this.UpdateAboutImage();
            this.mainTabs.SelectedTab = this.aboutTab;
            this.NativeInterface.ShowOrBringToFront();
            //if (!this.Visible)
            //{
            //    this.Show();
            //}
            //else
            //{
            //    this.BringToFront();
            //    this.Activate();
            //}
        }

        private void SettingChanged(object sender, EventArgs e)
        {
            
        }

        /*private void FollowMouseCheckBoxCheckedChanged(object sender, EventArgs e)
        {
            if (!this.updating)
            {
                Globals.CurrentProfile.FollowMouse = this.followMouseCheckBox.Checked;
            }
        }*/

        private void ClearHistoryButtonClick(object sender, EventArgs e)
        {
            InternalGlobals.CurrentProfile.History.Clear();
            InternalGlobals.CurrentProfile.History.Save();
        }

        private void HandleCheckForUpdate(object sender, EventArgs e)
        {
            Updater.CheckForUpdate(false);
        }

        private void HandleCheckForPluginUpdates(object sender, EventArgs e)
        {
            this.NativeInterface.SetCursorToWait();
            BackgroundWorker worker = new BackgroundWorker();
            worker.DoWork += this.CheckForPluginUpdates;
            worker.RunWorkerAsync();
        }

        private void CheckForPluginUpdates(object sender, EventArgs e)
        {
            Updater.CheckForPluginUpdates(true, false, this.ResetCursor);
        }

        private void ResetCursor()
        {
            this.Invoke(new ParameterlessVoid(this.NativeInterface.SetCursorToDefault));
        }

        private void VisitWebsite(object sender, EventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start("www.PromptuLauncher.com");
            }
            catch (System.ComponentModel.Win32Exception)
            {
                UIMessageBox.Show(
                    Localization.Promptu.ErrorVisitingWebsite,
                    Localization.Promptu.AppName,
                    UIMessageBoxButtons.OK,
                    UIMessageBoxIcon.Error,
                    UIMessageBoxResult.OK);
            }
            catch (FileNotFoundException)
            {
                UIMessageBox.Show(
                    Localization.Promptu.ErrorVisitingWebsite,
                    Localization.Promptu.AppName,
                    UIMessageBoxButtons.OK,
                    UIMessageBoxIcon.Error,
                    UIMessageBoxResult.OK);
            }
        }

        public void UpdateInstalledPlugins()
        {
            List<PromptuPlugin> plugins = new List<PromptuPlugin>();

            foreach (PromptuPlugin plugin in InternalGlobals.AvailablePlugins)
            {
                if (plugin.IsInstalled)
                {
                    plugins.Add(plugin);
                }
            }

            plugins.Sort(ComparePlugins);
            this.installedPluginsPanel.ClearPlugins();
            this.installedPluginsPanel.AddPlugins(plugins);
        }

        private static int ComparePlugins(PromptuPlugin x, PromptuPlugin y)
        {
            return x.Name.CompareTo(y.Name);
        }

        /*private void UpdateSkinPropertyGridDescriptionAreaHeight(object sender, EventArgs e)
        {
            if (!this.updating)
            {
                Globals.CurrentProfile.UISettings.SetupDialogSettings.SkinPropertyGridDescriptionAreaHeight = this.ItemPropertyGrid.DescriptionAreaHeight;
            }
        }

        private void ProfilesComboBoxSelectionChanged(object sender, EventArgs e)
        {
            if (this.updating)
            {
                return;
            }

            ProfilePlacemark profilePlacemark = (ProfilePlacemark)this.profileComboBox.SelectedItem;

            if (profilePlacemark.IsExternallyLocked)
            {
                MessageBox.Show(
                    String.Format(Localization.UIResources.SelectedProfileLockedMessage, profilePlacemark.Name, profilePlacemark.Locker),
                    Localization.Promptu.AppName,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error,
                    MessageBoxDefaultButton.Button1,
                    this.GetMessageBoxOptions());
                this.profileComboBox.SelectedItem = Globals.PlacemarkOfCurrentProfile;
                return;
            }

            try
            {
                Profile profile = profilePlacemark.GetEntireProfile();
                PromptHandler.GetInstance().SwitchToProfile(profile, false);
                Globals.CurrentProfile.SyncLocalAssemblyReferences();
            }
            catch (ProfileLoadException)
            {
                MessageBox.Show(
                    Localization.MessageFormats.ProfileIsCorrupted,
                    Localization.Promptu.AppName,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error,
                    MessageBoxDefaultButton.Button1,
                    this.GetMessageBoxOptions());
                Globals.ProfilePlacemarks.Remove(profilePlacemark);
            }


            this.UpdateUI(false);
        }

        private void DeleteProfileButtonClick(object sender, EventArgs e)
        {
            if (Globals.ProfilePlacemarks.Count <= 1)
            {
                MessageBox.Show(
                    Localization.Promptu.CannotDeleteProfileBecauseThereIsOnlyOne,
                    Localization.Promptu.AppName,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error,
                    MessageBoxDefaultButton.Button1,
                    this.GetMessageBoxOptions());
                return;
            }

            if (MessageBox.Show(
                String.Format(Localization.MessageFormats.ConfirmDeleteProfile, Globals.CurrentProfile.Name),
                Localization.Promptu.AppName,
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Asterisk,
                MessageBoxDefaultButton.Button2,
                this.GetMessageBoxOptions()) == DialogResult.Yes)
            {
                Globals.SyncSynchronizer.CancelSyncsAndWait();
                Profile profile = Globals.CurrentProfile;
                int index = Globals.ProfilePlacemarks.IndexOf(profile.FolderName);
                if (index >= Globals.ProfilePlacemarks.Count - 1)
                {
                    index = Globals.ProfilePlacemarks.Count - 2;
                }

                Globals.ProfilePlacemarks.Remove(profile.FolderName);
                profile.Directory.Delete();
                this.profileComboBox.Items.Clear();
                this.profileComboBox.Items.AddRange(Globals.ProfilePlacemarks.ToArray());
                this.profileComboBox.SelectedIndex = index;
            }
        }
        
        private void StartPromptuAutomaticallyCheckedChanged(object sender, EventArgs e)
        {
            if (!ignoreStartPromptuAutomaticallyCheckedChanged)
            {
                try
                {
                    Globals.AutoStartWithComputer = this.startPromptuAutomatically.Checked;
                }
                catch (InvalidComObjectException)
                {
                    ignoreStartPromptuAutomaticallyCheckedChanged = true;
                    this.startPromptuAutomatically.Checked = !this.startPromptuAutomatically.Checked;
                    this.startPromptuAutomatically.Enabled = false;
                    MessageBox.Show(
                        Localization.MessageFormats.CannotSetStartPromptuAutomatically,
                        Localization.Promptu.AppName,
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error,
                        MessageBoxDefaultButton.Button1,
                        this.GetMessageBoxOptions());
                }
            }
        }

        private void UpdateListDescriptionHeight(object sender, EventArgs e)
        {
            if (!this.updating && this.shown)
            {
                Globals.CurrentProfile.UISettings.SetupDialogSettings.ListDescriptionHeight = this.ListSelector.ListDescriptionSplitContainer.ReverseSplitterDistance;
            }
        }

        private void ShowHiddenFilesAndFoldersCheckedChanged(object sender, EventArgs e)
        {
            if (!this.updating && this.shown)
            {
                Globals.CurrentProfile.NavigateHiddenFilesAndFolders = this.showHiddenFilesAndFoldersCheckBox.Checked;
            }
        }

        private void ShowSystemFilesAndFoldersCheckedChanged(object sender, EventArgs e)
        {
            if (!this.updating && this.shown)
            {
                Globals.CurrentProfile.NavigateSystemFilesAndFolders = this.showSystemFilesAndFoldersCheckBox.Checked;
            }
        }

        private void SkinSplitContainerSplitterMoved(object sender, EventArgs e)
        {
            if (!this.updating && this.shown)
            {
                Globals.CurrentProfile.UISettings.SetupDialogSettings.SkinSplitterDistance = this.SkinSplitterDistance;
            }
        }

        private void PropertyAreaSplitContainerSplitterMoved(object sender, EventArgs e)
        {
            if (!this.updating && this.shown)
            {
                Globals.CurrentProfile.UISettings.SetupDialogSettings.PropertyAreaSplitterDistance = this.PropertyAreaSplitterDistance;
            }
        }

        private void ListSplitContainerSplitterMoved(object sender, EventArgs e)
        {
            if (!this.updating && this.shown)
            {
                Globals.CurrentProfile.UISettings.SetupDialogSettings.ListSplitterDistance = this.ListSplitterDistance;
            }
        }*/

        private void UpdateAboutImage()
        {
            int year = 2010;
            DateTime now = DateTime.Now;
            if (now.Year > year)
            {
                year = now.Year;
            }

            this.aboutPanel.Copyright = String.Format(
                CultureInfo.CurrentCulture, 
                Localization.Promptu.CopyrightFormat,
                year);
            //Image oldImage = this.aboutPictureBox.Image;
            //if (oldImage != null)
            //{
            //    oldImage.Dispose();
            //}

            //this.aboutLinkLabel.Height = this.aboutLinkLabel.GetPreferredSize(Size.Empty).Height;
            //this.aboutLinkLabel.SendToBack();
            //this.aboutPictureBox.Image = SpecialImages.AboutImage;
            //this.aboutPictureBox.UpdateSizeMode();
        }

        private void HandlePluginConfigureClicked(object sender, ObjectEventArgs<PromptuPlugin> e)
        {
            PromptuPlugin plugin = e.Object;
            if (plugin != null && plugin.Enabled && plugin.HasLoaded)
            {
                PromptuPluginEntryPoint entryPoint = plugin.EntryPoint;
                if (entryPoint == null)
                {
                    return;
                }

                InternalGlobals.PluginConfigWindowManager.ShowConfigFor(entryPoint);
            }
        }

        private void HandleConfigureSkinClicked(object sender, ObjectEventArgs<PromptuSkin> e)
        {
            PromptuSkin skin = e.Object;
            if (skin != null)
            {
                // TODO handle if no options
                OptionsDialogPresenter optionsDialog = new OptionsDialogPresenter();

                OptionPage promptPageFromSkin = InternalGlobals.CurrentSkinInstance.Prompt.Options;
                if (promptPageFromSkin != null)
                {
                    SuperTabPage promptPage = new SuperTabPage("prompt");
                    promptPage.Text = Localization.UIResources.PromptConfigurationTabText;
                    optionsDialog.Tabs.Add(promptPage);
                    IPromptuOptionsPanel promptPanel = InternalGlobals.GuiManager.ToolkitHost.Factory.ConstructOptionsPanel();
                    promptPanel.MainInstructions = promptPageFromSkin.MainInstructions ?? Localization.UIResources.StandardPromptOptionsMainInstructions;
                    promptPage.HostedWidget = new ExternalWidget("panel", promptPanel);
                    promptPanel.Editor.Properties = promptPageFromSkin.Groups;
                }

                OptionPage suggestionProviderPageFromSkin = InternalGlobals.CurrentSkinInstance.SuggestionProvider.Options;
                if (suggestionProviderPageFromSkin != null)
                {
                    SuperTabPage suggestionProviderPage = new SuperTabPage("suggestionProvider");
                    suggestionProviderPage.Text = Localization.UIResources.SuggestionProviderConfigurationTabText;
                    optionsDialog.Tabs.Add(suggestionProviderPage);
                    IPromptuOptionsPanel suggestionProviderPanel = InternalGlobals.GuiManager.ToolkitHost.Factory.ConstructOptionsPanel();
                    suggestionProviderPanel.MainInstructions = suggestionProviderPageFromSkin.MainInstructions ?? Localization.UIResources.StandardSuggestionProviderOptionsMainInstructions;
                    suggestionProviderPage.HostedWidget = new ExternalWidget("panel", suggestionProviderPanel);
                    suggestionProviderPanel.Editor.Properties = suggestionProviderPageFromSkin.Groups;

                }

                PropertiesAndOptions tooltipPropertiesAndOptions = InternalGlobals.CurrentSkinInstance.InformationBoxPropertiesAndOptions;

                if (tooltipPropertiesAndOptions != null)
                {
                    OptionPage tooltipPageFromSkin = tooltipPropertiesAndOptions.Options;
                    if (tooltipPageFromSkin != null)
                    {
                        SuperTabPage tooltipPage = new SuperTabPage("tooltips");
                        tooltipPage.Text = Localization.UIResources.ToolTipConfigurationTabText;
                        optionsDialog.Tabs.Add(tooltipPage);
                        IPromptuOptionsPanel tooltipPanel = InternalGlobals.GuiManager.ToolkitHost.Factory.ConstructOptionsPanel();
                        tooltipPanel.MainInstructions = tooltipPageFromSkin.MainInstructions ?? Localization.UIResources.StandardTooltipOptionsMainInstructions;
                        tooltipPage.HostedWidget = new ExternalWidget("panel", tooltipPanel);
                        tooltipPanel.Editor.Properties = tooltipPageFromSkin.Groups;
                    }
                }

                InternalGlobals.UISettings.SkinOptionsDialogSettings.ImpartTo(optionsDialog.NativeInterface);
                optionsDialog.ShowDialog();
                InternalGlobals.UISettings.SkinOptionsDialogSettings.UpdateFrom(optionsDialog.NativeInterface);
            }
        }

        private void VisitContactLink(string link)
        {
            try
            {
                System.Diagnostics.Process.Start(PromptuUtilities.SanitizeContactLink(link));
            }
            catch (FileNotFoundException)
            {
                UIMessageBox.Show(
                    Localization.UIResources.ContactLinkInvalid,
                    Localization.Promptu.AppName,
                    UIMessageBoxButtons.OK,
                    UIMessageBoxIcon.Error);
            }
            catch (System.ComponentModel.Win32Exception)
            {
                UIMessageBox.Show(
                    Localization.UIResources.ContactLinkInvalid,
                    Localization.Promptu.AppName,
                    UIMessageBoxButtons.OK,
                    UIMessageBoxIcon.Error);
            }
        }

        private void HandlePluginCreatorContactLinkClicked(object sender, ObjectEventArgs<PromptuPlugin> e)
        {
            PromptuPlugin plugin = e.Object;
            if (plugin != null)
            {
                this.VisitContactLink(plugin.CreatorContact);
            }
        }

        private void HandleInstallPluginClicked(object sender, ObjectEventArgs<PromptuPlugin> e)
        {
            PromptuPlugin plugin = e.Object;
            if (plugin != null)
            {
                InternalGlobals.CurrentProfile.PluginMeta.InstallPlugin(plugin, InternalGlobals.AvailablePlugins);
                this.UpdateInstalledPlugins();
            }
        }

        private void HandleRemovePluginClicked(object sender, ObjectEventArgs<PromptuPlugin> e)
        {
            PromptuPlugin plugin = e.Object;
            if (plugin != null)
            {
                plugin.IsInstalled = false;
                InternalGlobals.CurrentProfile.PluginMeta.Remove(plugin.Id);
                plugin.Enabled = false;
                InternalGlobals.CurrentProfile.PluginMeta.SaveMetadata(InternalGlobals.AvailablePlugins);
                this.UpdateInstalledPlugins();
            }
        }

        private void HandleBrowseForPlugins(object sender, EventArgs e)
        {
            IOpenFileDialog openFileDialog = InternalGlobals.GuiManager.ToolkitHost.Factory.ConstructOpenFileDialog();
            openFileDialog.Text = Localization.UIResources.BrowseForPluginsTitle;
            openFileDialog.Filter = Localization.UIResources.BrowseForPluginsFilter;

            if (openFileDialog.ShowModal() == UIDialogResult.OK)
            {
                FileSystemDirectory pluginDirectory = (((FileSystemDirectory)System.Windows.Forms.Application.StartupPath) + "Common\\Plugins");

                FileFileDirectory package;

                try
                {
                    package = FileFileDirectory.FromContainer(openFileDialog.Path);
                }
                catch (FileFileSystemException)
                {
                    UIMessageBox.Show(
                        Localization.UIResources.InvalidPluginFileMessage,
                        Localization.Promptu.AppName,
                        UIMessageBoxButtons.OK,
                        UIMessageBoxIcon.Error);

                    return;
                }

                FileSystemDirectory directory = pluginDirectory + pluginDirectory.GetAvailableDirectoryName("{+}", "{number}", InsertBase.Zero);

                package.SaveIn(directory);
                InternalGlobals.LoadPlugins();
            }
        }

        private void HandlePluginToggleEnabled(object sender, ObjectEventArgs<PromptuPlugin> e)
        {
            PromptuPlugin plugin = e.Object;
            if (plugin != null)
            {
                plugin.Enabled = !plugin.Enabled;
            }
        }

        private void HandleSkinCreatorContactLinkClicked(object sender, ObjectEventArgs<PromptuSkin> e)
        {
            PromptuSkin skin = e.Object;
            if (skin != null)
            {
                this.VisitContactLink(skin.CreatorContact);
            }
        }

        private void HandleClosed(object sender, EventArgs e)
        {
            this.SerializeProxySettings(null);
            InternalGlobals.GuiManager.ToolkitHost.ApplicationExit -= this.HandleApplicationExit;
            this.OnClosed(EventArgs.Empty);
        }

        protected virtual void OnClosed(EventArgs e)
        {
            EventHandler handler = this.Closed;
            if (handler != null)
            {
                handler(this, e);
            }
        }
    }
}
