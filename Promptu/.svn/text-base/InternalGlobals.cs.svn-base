using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Configuration;
using ZachJohnson.Promptu.UserModel;
using ZachJohnson.Promptu.Collections;
using ZachJohnson.Promptu.Skins;
using ZachJohnson.Promptu.AssemblyCaching;
using System.Runtime.InteropServices;
using System.IO;
using ZachJohnson.Promptu.UserModel.Collections;
using System.Reflection;
using ZachJohnson.Promptu.UIModel;
using ZachJohnson.Promptu.UIModel.Configuration;
using ZachJohnson.Promptu.SkinApi;
using ZachJohnson.Promptu.PluginModel;
using System.ComponentModel;
using ZachJohnson.Promptu.UIModel.Presenters;
using ZachJohnson.Promptu.UIModel.Interfaces;
using ZachJohnson.Promptu.PluginModel.Internals;
using System.Globalization;

namespace ZachJohnson.Promptu
{
    internal static class InternalGlobals
    {
        private static ReleaseVersion currentPromptuVersion = new ReleaseVersion(Assembly.GetExecutingAssembly().GetName().Version.ToString());
        private static ReleaseVersion lastPluginApiChangeVersion = new ReleaseVersion("0.9.0.6");
#if BETA
        private static readonly ReleaseType currentReleaseType = ReleaseType.Beta;
#else
        private static readonly ReleaseType currentReleaseType = ReleaseType.Release;
#endif
        private static AssemblyCache assemblyCache;
        private static GuiManager guiManager;
        private static object exceptionPathToken = "path";
        private static SyncSynchronizer syncSynchronizer = new SyncSynchronizer();
        //private static SkinCollection skins;
        private static ProfilePlacemarkCollection profilePlacemarks;
        private static Profile currentProfile;
        private static PromptuSkin currentSkin;
        private static PromptuSkinInstance currentSkinInstance;
        private static LoadedAssemblyCollection loadedAssemblies = new LoadedAssemblyCollection();
        private static System.Timers.Timer lockUpdateTimer;
        private static int lockUpdateCount;
        private static int lockUpdatesUntilLockedFileSave = 15;
        private static bool autoStartWithComputer;
        private static GlobalSettings globalSettings = GlobalSettings.FromFile(((FileSystemDirectory)Application.StartupPath) + "Config\\Misc");
        private static bool ignoreActivationLost;
        private static FailedToSaveFileCollection failedToSaveFiles = new FailedToSaveFileCollection();
        private static bool syncAllowed;
        private static OS runningOS;
        private static SkinCollection skins;
        private static bool skinPropertiesHaveChanged;
        private static PromptuPluginCollection availablePlugins;
        private static object availablePluginsLock = new object();
        private static FileSystemWatcher pluginWatcher;
        private static System.Timers.Timer pluginReloadTimer;
        private static Assembly toolkitAssembly;
        private static NotifyIconPresenter notifyIcon;
        private static NativeUICollectionBridge<IGenericMenuItem> suggestionItemContextMenuBridge = new NativeUICollectionBridge<IGenericMenuItem>();
        private static NativeUICollectionBridge<IGenericMenuItem> promptContextMenuBridge = new NativeUICollectionBridge<IGenericMenuItem>();
        private static UIContextMenu suggestionItemContextMenu;
        private static UIContextMenu promptContextMenu;
        private static PluginConfigWindowManager pluginConfigWindowManager = new PluginConfigWindowManager();
        //private static FileSystemDirectory iconCacheDirectory = ((FileSystemDirectory)Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)) + "\\Promptu\\icon-cache\\";
        //private static bool checkForPluginUpdatesBeforeLoad;
        //private static bool checkForPreReleaseUpdates;

        static InternalGlobals()
        {
            suggestionItemContextMenu = new UIContextMenu(new PassthroughNativeContextMenu(suggestionItemContextMenuBridge));
            promptContextMenu = new UIContextMenu(new PassthroughNativeContextMenu(promptContextMenuBridge));
            // TODO implement detection for OS
            runningOS = OS.Windows;
            
            AppDomain.CurrentDomain.AssemblyResolve += ResolveAssemblyName;
            lockUpdateTimer = new System.Timers.Timer(60000);
            lockUpdateTimer.Elapsed += TimeToUpdateLockFiles;
            lockUpdateTimer.Start();

            Application.ApplicationExit += HandleApplicationExit;

            FileSystemDirectory startupDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Startup);
            FileSystemFile shortcutFile = startupDirectory + "Promptu.lnk";
            if (shortcutFile.Exists)
            {
                try
                {
                    Shortcut shortcut = Shortcut.FromFile(shortcutFile);
                    if (shortcut.Target.ToUpperInvariant() == Application.ExecutablePath.ToUpperInvariant())
                    {
                        autoStartWithComputer = true;
                    }
                }
                catch (InvalidComObjectException)
                {
                }
            }

            pluginReloadTimer = new System.Timers.Timer(1000);
            pluginReloadTimer.AutoReset = false;
            pluginReloadTimer.Elapsed += HandlePluginReload;
        }

        //public static FileSystemDirectory IconCacheDirectory
        //{
        //    get { return iconCacheDirectory; }
        //}

        public static UIContextMenu SuggestionItemContextMenu
        {
            get { return suggestionItemContextMenu; }
        }

        public static UIContextMenu PromptContextMenu
        {
            get { return promptContextMenu; }
        }

        public static PluginConfigWindowManager PluginConfigWindowManager
        {
            get { return pluginConfigWindowManager; }
        }

        //public static bool CheckForPluginUpdatesBeforeLoad
        //{
        //    get { return checkForPluginUpdatesBeforeLoad; }
        //    set {checkForPluginUpdatesBeforeLoad =value;}
        //}

        public static NativeUICollectionBridge<IGenericMenuItem> SuggestionItemContextMenuBridge
        {
            get { return suggestionItemContextMenuBridge; }
        }

        public static NativeUICollectionBridge<IGenericMenuItem> PromptContextMenuBridge
        {
            get { return promptContextMenuBridge; }
        }

        public static NotifyIconPresenter NotifyIcon
        {
            get { return notifyIcon; }
            set { notifyIcon = value; }
        }

        public static Assembly ToolkitAssembly
        {
            get { return toolkitAssembly; }
            set { toolkitAssembly = value; }
        }

        public static ReleaseVersion CurrentPromptuVersion
        {
            get { return currentPromptuVersion; }
        }

        public static ReleaseVersion LastPluginApiChangeVersion
        {
            get { return lastPluginApiChangeVersion; }
        }

        public static ReleaseType CurrentReleaseType
        {
            get { return currentReleaseType; }
        }

        public static PromptuPluginCollection AvailablePlugins
        {
            get { return availablePlugins; }
            set { availablePlugins = value; }
        }

        public static object AvailablePluginsLock
        {
            get { return availablePluginsLock; }
        }

        //public static bool CheckForPreReleaseUpdates
        //{
        //    get { return checkForPreReleaseUpdates; }
        //    set { checkForPreReleaseUpdates = value; }
        //}

        public static SkinCollection Skins
        {
            get { return skins; }
            internal set { skins = value; }
        }

        public static OS RunningOS
        {
            get { return runningOS; }
        }

        internal static FailedToSaveFileCollection FailedToSaveFiles
        {
            get { return failedToSaveFiles; }
        }

        internal static void TryResaveAndAlertUser()
        {
            FailedToSaveFile[] files = FailedToSaveFiles.ToArray();
            foreach (FailedToSaveFile file in files)
            {
                file.ResaveHandler(file);
            }

            if (FailedToSaveFiles.Count > 0)
            {
                bool firstAttempt = true;

                while (true)
                {
                    // I18N
                    StringBuilder builder = new StringBuilder();
                    string stillOrEmpty = firstAttempt ? String.Empty : "still ";
                    string buttonName = firstAttempt ? "OK" : "Retry";
                    if (failedToSaveFiles.Count == 1)
                    {

                        builder.AppendLine(String.Format(CultureInfo.CurrentCulture, "Promptu {0}cannot save the following file because it is in use by another program:", stillOrEmpty));
                        builder.AppendLine(FailedToSaveFiles[0].Filepath);
                        builder.AppendLine();
                        builder.AppendLine(String.Format(CultureInfo.CurrentCulture, "Please close any programs that might be using it and click '{0}'.", buttonName));
                    }
                    else
                    {
                        builder.AppendLine(String.Format(CultureInfo.CurrentCulture, "Promptu {0}cannot save the following files because they are in use by another program:", stillOrEmpty));
                        foreach (FailedToSaveFile stillRemainingFile in FailedToSaveFiles)
                        {
                            builder.AppendLine(stillRemainingFile.Filepath);
                        }

                        builder.AppendLine();
                        builder.AppendLine(String.Format(CultureInfo.CurrentCulture, "Please close any programs that might be using them and click '{0}'.", buttonName));
                    }

                    if (firstAttempt)
                    {
                        UIMessageBox.Show(
                            builder.ToString(),
                            Localization.Promptu.AppName,
                            UIMessageBoxButtons.OK,
                            UIMessageBoxIcon.Warning,
                            UIMessageBoxResult.OK);
                    }
                    else
                    {
                        // Change to "try again?"
                        if (UIMessageBox.Show(
                           builder.ToString(),
                           Localization.Promptu.AppName,
                           UIMessageBoxButtons.YesNo,
                           UIMessageBoxIcon.Warning,
                           UIMessageBoxResult.Yes) != UIMessageBoxResult.Yes)
                        {
                            break;
                        }
                    }

                    files = FailedToSaveFiles.ToArray();
                    foreach (FailedToSaveFile file in files)
                    {
                        file.ResaveHandler(file);
                    }

                    firstAttempt = false;

                    if (FailedToSaveFiles.Count == 0)
                    {
                        break;
                    }
                }
            }
        }

        internal static void ResaveProfileItem(FailedToSaveFile file)
        {
            Profile profile = CurrentProfile;
            if (profile != null)
            {
                if (file.FileId != null)
                {
                    switch (file.FileId)
                    {
                        case Profile.ConfigResaveId:
                            profile.SaveConfig();
                            break;
                        case Profile.UIConfigResaveId:
                            profile.SaveUIConfig();
                            break;
                        case HistoryCollection.FileId:
                            profile.History.Save();
                            break;
                        //case SkinDataCollection.FileId:
                        //    profile.SkinsData.Save();
                        //    break;
                        case UserCookie.FileId:
                            UserCookie cookie = new UserCookie(profile.FolderName);
                            cookie.Save();
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        internal static void ResaveListItem(FailedToSaveFile file)
        {
            Profile profile = CurrentProfile;
            if (profile != null)
            {
                List list = profile.Lists.TryGet(file.Id);
                if (list == null)
                {
                    FailedToSaveFiles.Remove(file);
                }
                else
                {
                    if (file.FileId != null)
                    {
                        switch (file.FileId)
                        {
                            case List.ListConfigId:
                                list.SaveConfig();
                                break;
                            case AssemblyReferenceCollectionWrapper.FileId:
                                list.AssemblyReferences.Save();
                                break;
                            case AssemblyReferencesManifest.FileId:
                                list.AssemblyReferencesManifest.Save();
                                break;
                            case CommandCollectionWrapper.FileId:
                                list.Commands.Save();
                                break;
                            case FunctionCollectionWrapper.FileId:
                                list.Functions.Save();
                                break;
                            case ValueListCollectionWrapper.FileId:
                                list.ValueLists.Save();
                                break;
                            default:
                                break;
                        }
                    }
                }
            }
        }

        internal static bool SyncAllowed
        {
            get { return syncAllowed; }
            set { syncAllowed = value; }
        }

        internal static System.Reflection.Assembly ResolveAssemblyName(object sender, ResolveEventArgs e)
        {
            switch (e.Name)
            {
                case "Mono.Cecil, Version=0.9.1.0, Culture=neutral, PublicKeyToken=0738eb9f132ed756":
                    FileSystemFile cecilDll = ((FileSystemDirectory)Application.StartupPath) + "Common/Mono.Cecil.dll";
                    if (cecilDll.Exists)
                    {
                        return Assembly.LoadFrom(cecilDll);
                    }

                    break;
                default:
                    if (e.Name.StartsWith("Promptu.WpfUI,"))
                    {
                        return ToolkitAssembly;
                    }

                    break;
            }

            foreach (CachedAssembly cachedAssembly in AssemblyCache)
            {
                AssemblyName name = AssemblyName.GetAssemblyName(cachedAssembly.File);
                if (name.ToString() == e.Name)
                {
                    LoadedAssembly loadedAssembly = LoadedAssemblies.TryGet(cachedAssembly.File.Name);
                    if (loadedAssembly != null)
                    {
                        return loadedAssembly.Assembly;
                    }
                    else
                    {
                        return AssemblyReference.LoadAssembly(cachedAssembly.File);
                    }
                }
            }

            return null;
        }

        internal static ToolkitSettings UISettings
        {
            get { return GuiManager.ToolkitHost.Settings; }
        }

        internal static bool IgnoreActivationLost
        {
            get { return ignoreActivationLost; }
            set { ignoreActivationLost = value; }
        }

        internal static ProfilePlacemarkCollection ProfilePlacemarks
        {
            get 
            { 
                return profilePlacemarks; 
            }
            set 
            {
                profilePlacemarks = value;
            }
        }

        internal static object ExceptionPathToken
        {
            get { return exceptionPathToken; }
        }

        internal static bool AutoStartWithComputer
        {
            get 
            { 
                return autoStartWithComputer; 
            }

            set
            {
                if (value != autoStartWithComputer)
                {
                    bool success = true;
                    FileSystemDirectory startupDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Startup);
                    FileSystemFile shortcutFile = startupDirectory + "Promptu.lnk";
                    if (value)
                    {
                        Shortcut shortcut = new Shortcut(shortcutFile, Application.ExecutablePath);
                        shortcut.Save();
                    }
                    else
                    {
                        shortcutFile.DeleteIfExists();
                    }

                    if (success)
                    {
                        autoStartWithComputer = value;
                    }
                }
            }
        }

        internal static GlobalSettings Settings
        {
            get { return globalSettings; }
        }

        //public static string ProxyAddress
        //{
        //    get
        //    {
        //        FileSystemFile proxySettingsFile = ((FileSystemDirectory)Application.StartupPath) + "Config\\ProxySettings";
        //        string proxyAddress = String.Empty;
        //        if (proxySettingsFile.Exists)
        //        {
        //            string[] proxyFileLines = proxySettingsFile.ReadAllLines();
                    
        //            foreach (string line in proxyFileLines)
        //            {
        //                string trimmed = line.Trim();
        //                if (trimmed.Length > 0 && !trimmed.StartsWith("#"))
        //                {
        //                    proxyAddress = trimmed;
        //                    break;
        //                }
        //            }      
        //        }

        //        return proxyAddress;
        //    }

        //    set
        //    {
        //        FileSystemFile proxySettingsFile = ((FileSystemDirectory)Application.StartupPath) + "Config\\ProxySettings";
        //        string[] lines;
        //        if (proxySettingsFile.Exists)
        //        {
        //            lines = proxySettingsFile.ReadAllLines();
        //        }
        //        else
        //        {
        //            using (MemoryStream stream = new MemoryStream(Defaults.ProxySettings))
        //            using (StreamReader reader = new StreamReader(stream))
        //            {
        //                List<string> linesList = new List<string>();
        //                while (!reader.EndOfStream)
        //                {
        //                    linesList.Add(reader.ReadLine());
        //                }

        //                lines = linesList.ToArray();
        //            }
        //        }

        //        using (FileStream stream = new FileStream(proxySettingsFile, FileMode.Create))
        //        using (StreamWriter writer = new StreamWriter(stream))
        //        {
        //            bool set = false;
        //            foreach (string line in lines)
        //            {
        //                if (line.StartsWith("#"))
        //                {
        //                    writer.WriteLine(line);
        //                    continue;
        //                }

        //                if (set)
        //                {
        //                    writer.WriteLine("# " + line);
        //                }
        //                else
        //                {
        //                    set = true;
        //                    if (String.IsNullOrEmpty(value))
        //                    {
        //                        continue;
        //                    }

        //                    writer.WriteLine(value);
        //                }
        //            }

        //            if (!set)
        //            {
        //                set = true;
        //                if (!String.IsNullOrEmpty(value))
        //                {
        //                    writer.WriteLine(value);
        //                }
        //            }
        //        }
        //    }
        //}

        internal static AssemblyCache AssemblyCache
        {
            get { return assemblyCache; }
            set { assemblyCache = value; }
        }

        internal static GuiManager GuiManager
        {
            get { return guiManager; }
            set { guiManager = value; }
        }

        //public static SkinCollection Skins
        //{
        //    get { return skins; }
        //    set { skins = value; }
        //}

        internal static LoadedAssemblyCollection LoadedAssemblies
        {
            get { return loadedAssemblies; }
        }

        internal static SyncSynchronizer SyncSynchronizer
        {
            get { return syncSynchronizer; }
        }

        public static PromptuSkinInstance CurrentSkinInstance
        {
            get 
            {
                return currentSkinInstance; 
            }

            internal set
            {
                skinPropertiesHaveChanged = false;
                if (currentSkinInstance != null)
                {
                    ObjectPropertyCollection promptProperties = currentSkinInstance.Prompt.SavingProperties;
                    ObjectPropertyCollection suggestionProviderProperties = currentSkinInstance.SuggestionProvider.SavingProperties;
                    ObjectPropertyCollection tooltipsProperties = currentSkinInstance.InformationBoxPropertiesAndOptions.Properties;

                    if (promptProperties != null)
                    {
                        promptProperties.PropertyChanged -= HandleCurrentSkinPropertyChanged;
                    }

                    if (suggestionProviderProperties != null)
                    {
                        suggestionProviderProperties.PropertyChanged -= HandleCurrentSkinPropertyChanged;
                    }

                    if (tooltipsProperties != null)
                    {
                        tooltipsProperties.PropertyChanged -= HandleCurrentSkinPropertyChanged;
                    }
                }

                currentSkinInstance = value;

                if (value != null)
                {
                    ObjectPropertyCollection promptProperties = value.Prompt.SavingProperties;
                    ObjectPropertyCollection suggestionProviderProperties = currentSkinInstance.SuggestionProvider.SavingProperties;
                    ObjectPropertyCollection tooltipsProperties = currentSkinInstance.InformationBoxPropertiesAndOptions.Properties;

                    if (promptProperties != null)
                    {
                        promptProperties.PropertyChanged += HandleCurrentSkinPropertyChanged;
                    }

                    if (suggestionProviderProperties != null)
                    {
                        suggestionProviderProperties.PropertyChanged += HandleCurrentSkinPropertyChanged;
                    }

                    if (tooltipsProperties != null)
                    {
                        tooltipsProperties.PropertyChanged += HandleCurrentSkinPropertyChanged;
                    }
                }
            }
        }

        public static PromptuSkin CurrentSkin
        {
            get 
            { 
                return currentSkin; 
            }

            internal set 
            {
                //skinPropertiesHaveChanged = false;
                //if (currentSkin != null)
                //{
                //    ObjectPropertyCollection promptProperties = currentSkinInstance.Prompt.SavingProperties;
                //    ObjectPropertyCollection suggestionProviderProperties = currentSkin.SuggestionProvider.SavingProperties;
                //    ObjectPropertyCollection tooltipsProperties = currentSkin.InformationBoxPropertiesAndOptions.Properties;

                //    if (promptProperties != null)
                //    {
                //        promptProperties.PropertyChanged -= HandleCurrentSkinPropertyChanged;
                //    }

                //    if (suggestionProviderProperties != null)
                //    {
                //        suggestionProviderProperties.PropertyChanged -= HandleCurrentSkinPropertyChanged;
                //    }

                //    if (tooltipsProperties != null)
                //    {
                //        tooltipsProperties.PropertyChanged -= HandleCurrentSkinPropertyChanged;
                //    }
                //}

                currentSkin = value;

                //if (value != null)
                //{
                //    ObjectPropertyCollection promptProperties = value.Prompt.SavingProperties;
                //    ObjectPropertyCollection suggestionProviderProperties = currentSkin.SuggestionProvider.SavingProperties;
                //    ObjectPropertyCollection tooltipsProperties = currentSkin.InformationBoxPropertiesAndOptions.Properties;

                //    if (promptProperties != null)
                //    {
                //        promptProperties.PropertyChanged += HandleCurrentSkinPropertyChanged;
                //    }

                //    if (suggestionProviderProperties != null)
                //    {
                //        suggestionProviderProperties.PropertyChanged += HandleCurrentSkinPropertyChanged;
                //    }

                //    if (tooltipsProperties != null)
                //    {
                //        tooltipsProperties.PropertyChanged += HandleCurrentSkinPropertyChanged;
                //    }
                //}
            }
        }

        private static void HandleCurrentSkinPropertyChanged(object sender, EventArgs e)
        {
            skinPropertiesHaveChanged = true;
        }

        public static ProfilePlacemark PlacemarkOfCurrentProfile
        {
            get
            {
                if (CurrentProfile != null)
                {
                    return ProfilePlacemarks[CurrentProfile.FolderName];
                }

                return null;
            }
        }

        public static Profile CurrentProfile
        {
            get 
            { 
                return currentProfile; 
            }

            set
            {
                skinPropertiesHaveChanged = false;
                if (currentProfile != null)
                {
                    currentProfile.SyncAffectingChangeOccuring -= HandleSyncAffectingChangeOccuring;
                }

                currentProfile = value;
                string profileId = String.Empty;
                if (currentProfile != null)
                {
                    profileId = currentProfile.FolderName;
                    currentProfile.SyncAffectingChangeOccuring += HandleSyncAffectingChangeOccuring;
                //    iconCacheDirectory = 
                //}
                //else
                //{
                //    iconCacheDirectory = (((FileSystemDirectory)Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData))
                //        + "\\Promptu\\icon-cache\\");
                }

                ClipboardCopyData.LastSetData = null;

                UserCookie cookie = new UserCookie(profileId);
                cookie.Save();
            }
        }

        private static void HandleSyncAffectingChangeOccuring(object sender, EventArgs e)
        {
            SyncSynchronizer.CancelSyncs = true;
        }

        private static void TimeToUpdateLockFiles(object sender, EventArgs e)
        {
            lockUpdateCount++;
            lockUpdatesUntilLockedFileSave--;
            if (CurrentProfile != null)
            {
                CurrentProfile.UpdateLockFile();
            }

            if (lockUpdateCount >= 10)
            {
                lockUpdateCount = 0;
                ProfilePlacemark placemarkOfCurrentProfile = PlacemarkOfCurrentProfile;
                FileSystemDirectory startupPath = Application.StartupPath;
                profilePlacemarks = ProfilePlacemarkCollection.FromFolder(startupPath + "Profiles", true);
                if (placemarkOfCurrentProfile != null)
                {
                    profilePlacemarks.Remove(placemarkOfCurrentProfile.FolderName);
                    profilePlacemarks.Add(placemarkOfCurrentProfile);
                }
            }

            if (skinPropertiesHaveChanged)
            {
                if (PromptHandler.IsCreated)
                {
                    skinPropertiesHaveChanged = false;
                    Profile profile = CurrentProfile;
                    PromptuSkin skin = CurrentSkin;
                    PromptuSkinInstance skinInstance = CurrentSkinInstance;

                    if (profile != null && skin != null)
                    {
                        PromptHandler.GetInstance().InvokeOnMainThread(new ParameterlessVoid(delegate
                            {
                                profile.SkinsSettings.TrySerialize(skinInstance, skin.Id);
                            }));
                    }
                }
            }

            if (lockUpdatesUntilLockedFileSave <= 0)
            {
                lockUpdatesUntilLockedFileSave = 15;

                if (FailedToSaveFiles.Count > 0)
                {
                    FailedToSaveFile[] files = FailedToSaveFiles.ToArray();
                    foreach (FailedToSaveFile file in files)
                    {
                        file.ResaveHandler(file);
                    }
                }
            }

            if (Updater.FirstCheckHasRun && (DateTime.Now - Updater.LastUpdateCheckTime).TotalHours > 24)
            {
                Updater.CheckForUpdate(true);
                CurrentProfile.UpdateAllCachedIconsAsync(false);
            }
        }

        private static void HandleApplicationExit(object sender, EventArgs e)
        {
            lockUpdateTimer.Stop();
            Application.ApplicationExit -= HandleApplicationExit;
            if (CurrentProfile != null)
            {
                CurrentProfile.DeleteLockFile();
            }
        }

        internal static void HandlePluginReload(object sender, EventArgs e)
        {
            GuiManager.ToolkitHost.MainThreadDispatcher.BeginInvoke(new ParameterlessVoid(LoadPlugins), null);
        }

        private static void HandlePluginPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Enabled" || e.PropertyName == "IsCompatible")
            {
                CurrentProfile.PluginMeta.SaveMetadata((PromptuPluginCollection)sender);
            }
        }

        private static void HandlePluginSettingOrPropertyChanged(object sender, EventArgs e)
        {
             CurrentProfile.PluginMeta.SaveMetadata((PromptuPluginCollection)sender);
        }

        internal static void LoadPlugins()
        {
            pluginReloadTimer.Stop();
            PromptuPluginCollection pluginCollection = new PromptuPluginCollection();

            //if (pluginCollection == null)
            //{
            //    pluginCollection = new PromptuPluginCollection();
            //    availablePlugins = pluginCollection;
            //}
            //else
            //{
            //    pluginCollection.Clear();
            //}

            FileSystemDirectory directory = (((FileSystemDirectory)Application.StartupPath) + "Common\\Plugins");

            directory.CreateIfDoesNotExist();

            foreach (FileSystemDirectory childDirectory in directory.GetDirectories())
            {
                PromptuPlugin plugin = PromptuPlugin.FromFolder(childDirectory);

                if (plugin != null && !pluginCollection.Contains(plugin.Id))
                {
                    pluginCollection.Add(plugin);
                }
            }

            PromptuPluginCollection oldPlugins = availablePlugins;

            if (oldPlugins != null)
            {
                foreach (PromptuPlugin oldPlugin in oldPlugins)
                {
                    PromptuPlugin correspondingNewPlugin = pluginCollection.TryGet(oldPlugin.Id);

                    if (correspondingNewPlugin != null && correspondingNewPlugin.Folder == oldPlugin.Folder)
                    {
                        pluginCollection.Remove(correspondingNewPlugin);
                        pluginCollection.Add(oldPlugin);
                    }
                }

                oldPlugins.Clear();
            }

            using (DdMonitor.Lock(availablePluginsLock))
            {
                if (availablePlugins != null)
                {
                    availablePlugins.ChildPropertyChanged -= HandlePluginPropertyChanged;
                    availablePlugins.ChildSettingOrPropertyChanged -= HandlePluginSettingOrPropertyChanged;
                    availablePlugins.Clear();
                }

                availablePlugins = pluginCollection;

                availablePlugins.ChildPropertyChanged += HandlePluginPropertyChanged;
                availablePlugins.ChildSettingOrPropertyChanged += HandlePluginSettingOrPropertyChanged;
            }

            Profile currentProfile = CurrentProfile;
            if (currentProfile != null)
            {
                currentProfile.PluginMeta.LoadMetadata(pluginCollection);
            }

            if (PromptHandler.IsCreated)
            {
                PromptHandler.GetInstance().SetupDialog.UpdatePluginDisplays();
            }

            if (pluginWatcher == null && directory.Exists)
            {
                pluginWatcher = new FileSystemWatcher(directory);
                pluginWatcher.Changed += HandleSomeSortOfPluginDirectoryChange;
                pluginWatcher.Created += HandleSomeSortOfPluginDirectoryChange;
                pluginWatcher.Deleted += HandleSomeSortOfPluginDirectoryChange;
                pluginWatcher.Renamed += HandleSomeSortOfPluginDirectoryChange;
                pluginWatcher.EnableRaisingEvents = true;
            }
        }

        private static void HandleSomeSortOfPluginDirectoryChange(object sender, FileSystemEventArgs e)
        {
            pluginReloadTimer.Stop();
            pluginReloadTimer.Start();
        }
    }
}
