using System;
using System.Collections.Generic;
using System.Text;
using ZachJohnson.Promptu.Skins;
using System.Xml;
using ZachJohnson.Promptu.Collections;
//using ZachJohnson.Promptu.DynamicEntryModel;
using System.Xml.Extensions;
using System.IO;
using System.Windows.Forms;
using System.Extensions;
using ZachJohnson.Promptu.UserModel.Differencing;
using ZachJohnson.Promptu.UI;
using System.Drawing;
using System.Threading;
using ZachJohnson.Promptu.UserModel.Configuration;
using ZachJohnson.Promptu.UserModel.Collections;
using ZachJohnson.Promptu.UIModel.Presenters;
using ZachJohnson.Promptu.UIModel;
using ZachJohnson.Promptu.SkinApi;
using System.ComponentModel;
using ZachJohnson.Promptu.PluginModel;
using System.Globalization;
using System.Diagnostics;
using ZachJohnson.Promptu.UIModel.Interfaces;

namespace ZachJohnson.Promptu.UserModel
{
    public class Profile : ProfileBase, INotifyPropertyChanged
    {
        private static List<List> notifiedIncompatableLists = new List<List>();
        //private static readonly Size DefaultSuggesterSize = new Size(258, 158);
        private const int MininumSyncTimeInSeconds = 30;
        internal const string ConfigResaveId = "config";
        internal const string UIConfigResaveId = "uiconfig";
        //private FileSystemFile lockFile;
        //private FileSystemDirectory directory;
        //private string name;
        private GlobalHotkey hotkey;
        //private GlobalHotkey notesHotkey;
        private string skinId;
        private int syncFrequency;
        private TimeUnit syncFrequencyUnit;
        private ListCollection lists;
        private SuggestionConfig suggestionConfig;
        private CommandDefaults commandDefaults;
        private int selectedListIndex = -1;
        //private CommandCollectionComposite compositeCommands;
        private FunctionsCommandsComposite compositeFunctionsAndCommands;
        private FunctionsCommandsComposite allCompositeFunctionsAndCommands;
        private FunctionsCommandsCompositeMediator compositeFunctionsAndCommandsMediator;
        private FunctionCollectionComposite compositeFunctions;
        //private SkinDataCollection skinsData;
        private bool useSentimentalSlickRunFeatures;
        //private FindOptimizedStringCollection optimizedCommandNames;
        private HistoryCollection history;
        private System.Timers.Timer syncTimer;
        private System.Timers.Timer retrySyncTimer = new System.Timers.Timer();
        private List<List> retrySyncLists = new List<List>();
        private int retrySyncCount;
        //private UISettings uiSettings;
        private bool blockSaveUIConfig;
        private AssemblyReferenceCollectionComposite compositeAssemblyReferences;
        private BackgroundWorkQueue backgroundWorkQueue;
        private bool showHiddenFilesAndFolders;
        private bool showSystemFilesAndFolders;
        private bool autoDetectFileAndFolderVisibility;
        private bool showCommandTargetIcons;
        private bool autoFetchFavicons;
        //private Size suggesterSize;
        private ManualResetEvent syncPauseEvent = new ManualResetEvent(true);
        private bool syncTimersPaused;
        private bool syncTimerWasGoing;
        private bool retrySyncTimerWasGoing;
        private PositioningMode promptPositioningMode;
        private Point? promptLocation;
        private bool primaryFileSystemCommandValidationCompleted;
        private bool fileSystemCommandValidationRunning;
        //private FileSystemFile notesFile;
        private bool cancelFileSystemValidations;
        private SkinsSettings skinsSettings;
        private PluginMetaCollection pluginMeta;
        private FileSystemDirectory iconCacheDirectory;
        private LastModifiedDictionary webIconLastModifiedDictionary;

        internal Profile(
            FileSystemDirectory directory, 
            string name,
            GlobalHotkey hotkey,
            //GlobalHotkey notesHotkey, 
            string skinId, 
            int syncFrequency, 
            TimeUnit syncFrequencyUnit, 
            ListCollection lists, 
            SuggestionConfig suggestionConfig,
            CommandDefaults commandDefaults,
            int selectedListIndex,
            HistoryCollection history,
            //SkinDataCollection skinsData,
            bool useSentimentalSlickRunFeatures,
            //UISettings uiSettings,
            bool showHiddenFilesAndFolders,
            bool showSystemFilesAndFolders,
            bool autoDetectFileAndFolderVisibility,
            bool showCommandTargetIcons,
            bool autoFetchFavicons,
            //Size suggesterSize,
            PositioningMode promptPositioningMode,
            Point? promptLocation,
            bool showSplashScreen,
            PluginMetaCollection pluginMeta)
            : base(directory, name, showSplashScreen)
        {
            if (hotkey == null)
            {
                throw new ArgumentNullException("hotkey");
            }
            //else if (notesHotkey == null)
            //{
            //    throw new ArgumentNullException("hotkey");
            //}
            else if (skinId == null)
            {
                throw new ArgumentNullException("skinId");
            }
            else if (lists == null)
            {
                throw new ArgumentNullException("lists");
            }
            else if (suggestionConfig == null)
            {
                throw new ArgumentNullException("suggestionConfig");
            }
            else if (syncFrequency < 1)
            {
                throw new ArgumentOutOfRangeException("'syncFrequency' cannot be less than one.");
            }
            else if (commandDefaults == null)
            {
                throw new ArgumentNullException("commandDefaults");
            }
            else if (history == null)
            {
                throw new ArgumentNullException("history");
            }
            //else if (skinsData == null)
            //{
            //    throw new ArgumentNullException("skinsData");
            //}

            //this.directory = directory;
            //this.name = name;
            this.hotkey = hotkey;
            this.hotkey.Changed += this.HandleHotkeyChanged;
            this.skinId = skinId;
            this.syncFrequency = syncFrequency;
            this.syncFrequencyUnit = syncFrequencyUnit;
            this.UpdateSyncTimer();
            this.lists = lists;
            this.suggestionConfig = suggestionConfig;
            this.commandDefaults = commandDefaults;
            this.selectedListIndex = selectedListIndex;
            this.promptPositioningMode = promptPositioningMode;
            this.promptLocation = promptLocation;

            this.skinsSettings = new SkinsSettings(directory + "\\SkinsSettings.xml");

            //this.notesFile = directory + "Notes.txt";

            this.pluginMeta = pluginMeta ?? new PluginMetaCollection(directory + "Plugins.xml");

            //this.notesHotkey = notesHotkey;

            //this.notesHotkey = new GlobalHotkey(HotkeyModifierKeys.Win, Keys.K);
            //this.compositeCommands = new CommandCollectionComposite(this.Lists);

            this.allCompositeFunctionsAndCommands = new FunctionsCommandsComposite(this.Lists, false);

            this.compositeFunctionsAndCommands = new FunctionsCommandsComposite(this.Lists, true);

            this.compositeFunctionsAndCommandsMediator = new FunctionsCommandsCompositeMediator(this.Lists);
            this.compositeFunctionsAndCommandsMediator.AddClient(this.compositeFunctionsAndCommands);
            this.compositeFunctionsAndCommandsMediator.AddClient(this.allCompositeFunctionsAndCommands);

            this.compositeFunctions = new FunctionCollectionComposite(this.Lists);
            //this.optimizedCommandNames = new FindOptimizedStringCollection(SortMode.Alphabetical);
            this.compositeAssemblyReferences = new AssemblyReferenceCollectionComposite(this.Lists);
            this.useSentimentalSlickRunFeatures = useSentimentalSlickRunFeatures;
            this.history = history;
            //this.skinsData = skinsData;
            this.retrySyncTimer.Interval = 30000;
            this.retrySyncTimer.AutoReset = false;
            this.retrySyncTimer.Elapsed += this.TimeToRetrySync;

            this.autoDetectFileAndFolderVisibility = autoDetectFileAndFolderVisibility;

            //if (uiSettings == null)
            //{
            //    this.uiSettings = new UISettings();
            //}
            //else
            //{
            //    this.uiSettings = uiSettings;
            //}
            this.iconCacheDirectory = (((FileSystemDirectory)Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData))
                        + "\\Promptu\\icon-cache\\") + this.FolderName;

            this.showHiddenFilesAndFolders = showHiddenFilesAndFolders;
            this.showSystemFilesAndFolders = showSystemFilesAndFolders;

            this.showCommandTargetIcons = showCommandTargetIcons;
            this.autoFetchFavicons = autoFetchFavicons;

            //this.uiSettings.SettingChanged += this.SaveUIConfig;
            this.backgroundWorkQueue = new BackgroundWorkQueue();
            //this.suggesterSize = suggesterSize;

            this.Lists.SyncAffectingChangeOccuring += this.RaiseSyncAffectingChangeOccuringEvent;
            this.Lists.LastUpdateCheckTimestampChanged += this.ListLastUpdateCheckTimestampChanged;
            this.webIconLastModifiedDictionary = new LastModifiedDictionary(((FileSystemDirectory)(this.IconCacheDirectory + "web")) + "404.xml");
            this.webIconLastModifiedDictionary.Reload();
            //this.UpdateLockFile();
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public event PropertyChangedEventHandler PropertyChanged;

        internal event EventHandler ListsSynchronized;

        internal event EventHandler SyncAffectingChangeOccuring;

        internal FunctionsCommandsCompositeMediator CompositeFunctionsAndCommandsMediator
        {
            get { return this.compositeFunctionsAndCommandsMediator; }
        }

        internal PluginMetaCollection PluginMeta
        {
            get { return this.pluginMeta; }
        }

        internal SkinsSettings SkinsSettings
        {
            get { return this.skinsSettings; }
        }

        internal FileSystemDirectory IconCacheDirectory
        {
            get { return this.iconCacheDirectory; }
        }

        internal LastModifiedDictionary WebIconLastModifiedDictionary
        {
            get { return this.webIconLastModifiedDictionary; }
        }

        //interanl FileSystemFile NotesFile
        //{
        //    get { return this.notesFile; }
        //}

        internal BackgroundWorkQueue BackgroundWorkQueue
        {
            get { return this.backgroundWorkQueue; }
        }

        internal bool ShowSplashScreen
        {
            get 
            { 
                return this.ShowSplashScreenInternal; 
            }

            set
            {
                if (this.ShowSplashScreenInternal != value)
                {
                    this.ShowSplashScreenInternal = value;
                    this.SaveConfig();
                }
            }
        }

        internal Point? PromptLocation
        {
            get 
            { 
                return this.promptLocation; 
            }

            set
            {
                if (this.promptLocation != value)
                {
                    this.promptLocation = value;
                    this.SaveConfig();
                }
            }
        }

        // bound
        internal PositioningMode PromptPositioningMode
        {
            get 
            { 
                return this.promptPositioningMode; 
            }

            set
            {
                if (this.promptPositioningMode != value)
                {
                    this.promptPositioningMode = value;
                    this.SaveConfig();
                    this.OnPropertyChanged(new PropertyChangedEventArgs("PromptPositioningMode"));
                }
            }
        }

        internal bool PrimaryFileSystemCommandValidationCompleted
        {
            get { return this.primaryFileSystemCommandValidationCompleted; }
            //internal set { this.primaryFileSystemCommandValidationCompleted = true; }
        }

        internal bool ShowCommandTargetIcons
        {
            get
            {
                return this.showCommandTargetIcons;
            }

            set
            {
                if (value != this.showCommandTargetIcons)
                {
                    this.showCommandTargetIcons = value;
                    this.SaveConfig();
                    this.OnPropertyChanged(new PropertyChangedEventArgs("ShowCommandTargetIcons"));                    
                }
            }
        }

        internal bool AutoFetchFavicons
        {
            get
            {
                return this.autoFetchFavicons;
            }

            set
            {
                if (value != this.autoFetchFavicons)
                {
                    this.autoFetchFavicons = value;
                    this.SaveConfig();
                    this.OnPropertyChanged(new PropertyChangedEventArgs("AutoFetchFavicons"));
                }
            }
        }

        // bound
        internal bool AutoDetectFileAndFolderVisibility
        {
            get
            {
                return this.autoDetectFileAndFolderVisibility;
            }

            set
            {
                if (value != this.autoDetectFileAndFolderVisibility)
                {
                    this.autoDetectFileAndFolderVisibility = value;
                    this.SaveConfig();
                    this.OnPropertyChanged(new PropertyChangedEventArgs("AutoDetectFileAndFolderVisibility"));
                }
            }
        }

        // bound
        internal bool ShowSystemFilesAndFolders
        {
            get 
            { 
                return this.showSystemFilesAndFolders; 
            }

            set 
            {
                if (value != this.showSystemFilesAndFolders)
                {
                    this.showSystemFilesAndFolders = value;
                    this.SaveConfig();
                    this.OnPropertyChanged(new PropertyChangedEventArgs("ShowSystemFilesAndFolders"));
                }
            }
        }

        // bound
        internal bool ShowHiddenFilesAndFolders
        {
            get
            {
                return this.showHiddenFilesAndFolders;
            }

            set
            {
                if (value != this.showHiddenFilesAndFolders)
                {
                    this.showHiddenFilesAndFolders = value;
                    this.SaveConfig();
                    this.OnPropertyChanged(new PropertyChangedEventArgs("ShowHiddenFilesAndFolders"));
                }
            }
        }

        internal bool BlockSaveUIConfig
        {
            get { return this.blockSaveUIConfig; }
            set { this.blockSaveUIConfig = value; }
        }

        internal CommandDefaults CommandDefaults
        {
            get { return this.commandDefaults; }
        }

        internal bool UseSentimentalSlickRunFeatures
        {
            get { return this.useSentimentalSlickRunFeatures; }
        }

        //public CommandCollectionComposite CompositeCommands
        //{
        //    get { return this.compositeCommands; }
        //}

        internal FunctionsCommandsComposite CompositeFunctionsAndCommands
        {
            get { return this.compositeFunctionsAndCommands; }
        }

        internal FunctionsCommandsComposite AllCompositeFunctionsAndCommands
        {
            get { return this.allCompositeFunctionsAndCommands; }
        }

        internal FunctionCollectionComposite CompositeFunctions
        {
            get { return this.compositeFunctions; }
        }

        internal AssemblyReferenceCollectionComposite CompositeAssemblyReferences
        {
            get { return this.compositeAssemblyReferences; }
        }

        //public SkinDataCollection SkinsData
        //{
        //    get { return this.skinsData; }
        //}

        internal HistoryCollection History
        {
            get { return this.history; }
            set { this.history = value; }
        }

        //public UISettings UISettings
        //{
        //    get { return this.uiSettings; }
        //}

        internal string Name
        {
            get 
            { 
                return base.NameInternal; 
            }

            set
            {
                base.NameInternal = value;
                ProfilePlacemark placemark = InternalGlobals.ProfilePlacemarks.TryGet(this.FolderName);
                if (placemark != null)
                {
                    placemark.Name = value;
                }

                this.SaveConfig();
            }
        }

        // bound
        internal GlobalHotkey Hotkey
        {
            get { return this.hotkey; }
        }

        //internal GlobalHotkey NotesHotkey
        //{
        //    get { return this.notesHotkey; }
        //}

        //public Size SuggesterSize
        //{
        //    get
        //    {
        //        return this.suggesterSize;
        //    }

        //    set
        //    {
        //        if (value != this.suggesterSize)
        //        {
        //            this.suggesterSize = value;
        //            this.SaveConfig();
        //        }
        //    }
        //}

        internal string SkinId
        {
            get 
            {
                return this.skinId;
            }

            set 
            { 
                this.skinId = value;
                this.SaveConfig();
            }
        }

        internal TimeUnit SyncFrequencyUnit
        {
            get 
            { 
                return this.syncFrequencyUnit; 
            }

            set
            {
                this.syncFrequencyUnit = value;
                this.UpdateSyncTimer();
            }
        }

        internal int SyncFrequency
        {
            get 
            { 
                return this.syncFrequency; 
            }

            set
            {
                if (value < 1)
                {
                    throw new ArgumentOutOfRangeException("value cannot be less than one.");
                }

                this.syncFrequency = value;
                this.UpdateSyncTimer();
            }
        }

        internal ListCollection Lists
        {
            get { return this.lists; }
        }

        internal SuggestionConfig SuggestionConfig
        {
            get { return this.suggestionConfig; }
        }

        //public FindOptimizedStringCollection OptimizedCommandNames
        //{
        //    get { return this.optimizedCommandNames; }
        //}

        internal List SelectedList
        {
            get
            {
                if (this.selectedListIndex >= this.lists.Count)
                {
                    this.selectedListIndex = this.lists.Count - 1;
                }

                if (this.selectedListIndex < 0)
                {
                    return null;
                }
                else
                {
                    return this.lists[this.selectedListIndex];
                }
            }
        }

        internal int SelectedListIndex
        {
            get 
            { 
                return this.selectedListIndex; 
            }

            set
            {
                if (value < 0)
                {
                    value = -1;
                }
                else if (value >= this.lists.Count)
                {
                    value = -1;
                }

                this.selectedListIndex = value;
                this.SaveConfig();
            }
        }

        internal static Profile FromFolder(FileSystemDirectory folder)
        {
            FileSystemFile uiConfigFile = folder + "UI.xml";
            FileSystemFile profileConfigFile = folder + "Profile.xml";
            XmlDocument profileConfigDocument = new XmlDocument();
            try
            {
                profileConfigDocument.LoadXml(File.ReadAllText(profileConfigFile));
            }
            catch (XmlException ex)
            {
                ex.Data.Add(InternalGlobals.ExceptionPathToken, profileConfigFile.Path);
                throw;
            }
            catch (IOException ex)
            {
                ex.Data.Add(InternalGlobals.ExceptionPathToken, profileConfigFile.Path);
                throw;
            }

            string name = null;
            int? syncFrequency = null;
            TimeUnit? syncFrequencyUnit = null;
            string skinId = null;
            GlobalHotkey hotkey = null;
            //GlobalHotkey notesHotkey = null;
            KeySuggestionConfig spacebarConfig = new KeySuggestionConfig(false, true);
            ListCollection lists = new ListCollection();
            CommandDefaults commandDefaults = new CommandDefaults(false);
            int selectedListIndex = -1;
            bool useSentimentalSlickRunFeatures = false;
            //UISettings uiSettings = null;
            int maximumNumberOfHistoryEntries = HistoryCollection.DefaultMaxCount;
            bool showHiddenFilesAndFolders = false;
            bool showSystemFilesAndFolders = false;
            bool autoDetectFileAndFolderVisibility = false;
            bool showCommandTargetIcons = true;
            bool autoFetchFavicons = true;
            //Size suggesterSize = DefaultSuggesterSize;
            XmlNode listsNode = null;
            PositioningMode promptPositioningMode = PositioningMode.FollowMouse;
            int? promptX = null;
            int? promptY = null;
            bool showSplashScreen = true;
            //int notesSelectionStart = 0;
            //int notesSelectionLength = 0;

            List<string> alreadyUsedSyncLocations = new List<string>();

            //try
            //{
            //    if (uiConfigFile.Exists)
            //    {
            //        XmlDocument uiConfigDocument = new XmlDocument();
            //        uiConfigDocument.LoadXml(File.ReadAllText(uiConfigFile));

            //        foreach (XmlNode node in uiConfigDocument.ChildNodes)
            //        {
            //            if (node.Name.ToUpperInvariant() == "UI")
            //            {
            //                uiSettings = UISettings.FromXml(node);
            //            }
            //        }
            //    }
            //}
            //catch (XmlException)
            //{
            //}
            //catch (IOException)
            //{
            //}


            foreach (XmlNode root in profileConfigDocument.ChildNodes)
            {
                switch (root.Name.ToUpperInvariant())
                {
                    case "PROFILE":
                        foreach (XmlAttribute attribute in root.Attributes)
                        {
                            switch (attribute.Name.ToUpperInvariant())
                            {
                                case "NAME":
                                    name = attribute.Value;
                                    break;
                                case "USESENTIMENTALSLICKRUNFEATURES":
                                    useSentimentalSlickRunFeatures = Utilities.TryParseBoolean(
                                        attribute.Value,
                                        useSentimentalSlickRunFeatures);
                                    //try
                                    //{
                                    //    useSentimentalSlickRunFeatures = Convert.ToBoolean(attribute.Value);
                                    //}
                                    //catch (FormatException)
                                    //{
                                    //}

                                    break;
                                case "SHOWSPLASHSCREEN":
                                    showSplashScreen = Utilities.TryParseBoolean(
                                        attribute.Value,
                                        showSplashScreen);
                                    //try
                                    //{
                                    //    showSplashScreen = Convert.ToBoolean(attribute.Value);
                                    //}
                                    //catch (FormatException)
                                    //{
                                    //}

                                    break;
                                default:
                                    break;
                            }
                        }

                        foreach (XmlNode node in root.ChildNodes)
                        {
                            switch (node.Name.ToUpperInvariant())
                            {
                                case "SYNC":
                                    foreach (XmlAttribute attribute in node.Attributes)
                                    {
                                        switch (attribute.Name.ToUpperInvariant())
                                        {
                                            case "EVERY":
                                                int number;

                                                try
                                                {
                                                    try
                                                    {
                                                        number = Convert.ToInt32(attribute.Value.Substring(0, attribute.Value.Length - 3), CultureInfo.InvariantCulture);
                                                    }
                                                    catch (FormatException)
                                                    {
                                                        number = Convert.ToInt32(attribute.Value.Substring(0, attribute.Value.Length - 3), CultureInfo.CurrentCulture);
                                                    }
                                                    catch (OverflowException)
                                                    {
                                                        number = Convert.ToInt32(attribute.Value.Substring(0, attribute.Value.Length - 3), CultureInfo.CurrentCulture);
                                                    }

                                                    string unit = attribute.Value.Substring(attribute.Value.Length - 3);

                                                    switch (unit.ToUpperInvariant())
                                                    {
                                                        case "HRS":
                                                            syncFrequencyUnit = TimeUnit.Hour;
                                                            break;
                                                        case "MIN":
                                                            syncFrequencyUnit = TimeUnit.Minute;
                                                            break;
                                                        case "SEC":
                                                            syncFrequencyUnit = TimeUnit.Second;
                                                            break;
                                                        default:
                                                            break;
                                                    }

                                                    syncFrequency = number;
                                                }
                                                catch (FormatException)
                                                {
                                                    Utilities.AddPathInformationAndThrow(
                                                        new LoadException("The sync frequency is in an invalid format."),
                                                        profileConfigFile);
                                                }
                                                catch (OverflowException)
                                                {
                                                    Utilities.AddPathInformationAndThrow(
                                                        new LoadException("The sync frequency is not a valid 32 bit integer."),
                                                        profileConfigFile);
                                                }

                                                break;
                                            default:
                                                break;
                                        }
                                    }

                                    break;
                                case "SKIN":
                                    foreach (XmlAttribute attribute in node.Attributes)
                                    {
                                        switch (attribute.Name.ToUpperInvariant())
                                        {
                                            case "ID":
                                                skinId = attribute.Value;
                                                break;
                                            default:
                                                break;
                                        }
                                    }

                                    break;
                                // any changes here must be reflected in GetDefaults
                                case "HOTKEY":
                                    try
                                    {
                                        hotkey = GlobalHotkey.FromXml(node);
                                    }
                                    catch (LoadException)
                                    {
                                    }

                                    break;
                                //case "NOTES":
                                //    foreach (XmlNode childNode in node.ChildNodes)
                                //    {
                                //        switch (childNode.Name.ToUpperInvariant())
                                //        {

                                //            case "HOTKEY":
                                //                try
                                //                {
                                //                    notesHotkey = GlobalHotkey.FromXml(childNode);
                                //                }
                                //                catch (LoadException)
                                //                {
                                //                }

                                //                break;
                                //            default:
                                //                break;
                                //        }
                                //    }

                                //    break;
                                case "PROMPT":
                                    foreach (XmlAttribute attribute in node.Attributes)
                                    {
                                        switch (attribute.Name.ToUpperInvariant())
                                        {
                                            case "FOLLOWMOUSE":
                                                bool? value = Utilities.TryParseBoolean(attribute.Value, null);
                                                if (value != null)
                                                {
                                                    promptPositioningMode = value.Value ? PositioningMode.FollowMouse : PositioningMode.None;
                                                }

                                                //try
                                                //{
                                                //    bool value = Convert.ToBoolean(attribute.Value);
                                                //    promptPositioningMode = value ? PositioningMode.FollowMouse : PositioningMode.None;
                                                //}
                                                //catch (FormatException)
                                                //{
                                                //}

                                                break;
                                            case "POSITIONINGMODE":
                                                try
                                                {
                                                    promptPositioningMode = (PositioningMode)Enum.Parse(typeof(PositioningMode), attribute.Value);
                                                }
                                                catch (ArgumentException)
                                                {
                                                }

                                                break;
                                            case "X":
                                                promptX = Utilities.TryParseInt32(attribute.Value, promptX);
                                                //try
                                                //{
                                                //    promptX = Convert.ToInt32(attribute.Value);
                                                //}
                                                //catch (FormatException)
                                                //{
                                                //}
                                                //catch (OverflowException)
                                                //{
                                                //}

                                                break;
                                            case "Y":
                                                promptY = Utilities.TryParseInt32(attribute.Value, promptY);
                                                //try
                                                //{
                                                //    promptY = Convert.ToInt32(attribute.Value);
                                                //}
                                                //catch (FormatException)
                                                //{
                                                //}
                                                //catch (OverflowException)
                                                //{
                                                //}

                                                break;
                                            default:
                                                break;
                                        }
                                    }

                                    foreach (XmlNode childNode in node.ChildNodes)
                                    {
                                        switch (childNode.Name.ToUpperInvariant())
                                        {
                                            
                                            case "WHILESUGGESTING":
                                                foreach (XmlNode childChildNode in childNode.ChildNodes)
                                                {
                                                    switch (childChildNode.Name.ToUpperInvariant())
                                                    {
                                                        case "SPACEBAR":
                                                            spacebarConfig = KeySuggestionConfig.FromXml(childChildNode, spacebarConfig);
                                                            break;
                                                        default:
                                                            break;
                                                    }
                                                }

                                                break;
                                            default:
                                                break;
                                        }
                                    }

                                    break;

                                    /*XmlNode targetIconsNode = document.CreateElement("TargetIcons");
            targetIconsNode.Attributes.Append(XmlUtilities.CreateAttribute("show", this.ShowCommandTargetIcons, document));
            targetIconsNode.Attributes.Append(XmlUtilities.CreateAttribute("autoFetchFavicons", this.AutoFetchFavicons, document));
            profileNode.AppendChild(targetIconsNode);*/
                                case "TARGETICONS":
                                    foreach (XmlAttribute attribute in node.Attributes)
                                    {
                                        switch (attribute.Name.ToUpperInvariant())
                                        {
                                            case "SHOW":
                                                showCommandTargetIcons = Utilities.TryParseBoolean(attribute.Value, showCommandTargetIcons);
                                                break;
                                            case "AUTOFETCHFAVICONS":
                                                autoFetchFavicons = Utilities.TryParseBoolean(attribute.Value, autoFetchFavicons);
                                                break;
                                            default:
                                                break;
                                        }
                                    }

                                    break;
                                case "FILEANDFOLDERNAVIGATION":
                                    foreach (XmlAttribute attribute in node.Attributes)
                                    {
                                        switch (attribute.Name.ToUpperInvariant())
                                        {
                                            case "AUTODETECTVISIBILITY":
                                                autoDetectFileAndFolderVisibility = Utilities.TryParseBoolean(
                                                    attribute.Value,
                                                    autoDetectFileAndFolderVisibility);
                                                //try
                                                //{
                                                //    autoDetectFileAndFolderVisibility = Convert.ToBoolean(attribute.Value);
                                                //}
                                                //catch (FormatException)
                                                //{
                                                //}

                                                break;
                                            case "SHOWHIDDEN":
                                                showHiddenFilesAndFolders = Utilities.TryParseBoolean(
                                                    attribute.Value,
                                                    showHiddenFilesAndFolders);
                                                //try
                                                //{
                                                //    showHiddenFilesAndFolders = Convert.ToBoolean(attribute.Value);
                                                //}
                                                //catch (FormatException)
                                                //{
                                                //}

                                                break;
                                            case "SHOWSYSTEM":
                                                showSystemFilesAndFolders = Utilities.TryParseBoolean(
                                                    attribute.Value,
                                                    showSystemFilesAndFolders);
                                                //try
                                                //{
                                                //    showSystemFilesAndFolders = Convert.ToBoolean(attribute.Value);
                                                //}
                                                //catch (FormatException)
                                                //{
                                                //}

                                                break;
                                            default:
                                                break;
                                        }
                                    }

                                    break;
                                //case "SUGGESTER":
                                //    foreach (XmlAttribute attribute in node.Attributes)
                                //    {
                                //        switch (attribute.Name.ToUpperInvariant())
                                //        {
                                //            case "WIDTH":
                                //                try
                                //                {
                                //                    suggesterSize.Width = Convert.ToInt32(attribute.Value);
                                //                }
                                //                catch (FormatException)
                                //                {
                                //                }
                                //                catch (OverflowException)
                                //                {
                                //                }

                                //                break;
                                //            case "HEIGHT":
                                //                try
                                //                {
                                //                    suggesterSize.Height = Convert.ToInt32(attribute.Value);
                                //                }
                                //                catch (FormatException)
                                //                {
                                //                }
                                //                catch (OverflowException)
                                //                {
                                //                }

                                //                break;
                                //            default:
                                //                break;
                                //        }
                                //    }

                                //    break;
                                case "DEFAULTS":
                                    foreach (XmlNode childNode in node.ChildNodes)
                                    {
                                        switch (childNode.Name.ToUpperInvariant())
                                        {
                                            case "COMMAND":
                                                commandDefaults = CommandDefaults.FromXml(childNode, commandDefaults);
                                                break;
                                            default:
                                                break;
                                        }
                                    }

                                    break;
                                case "HISTORY":
                                    foreach (XmlAttribute attribute in node.Attributes)
                                    {
                                        switch (attribute.Name.ToUpperInvariant())
                                        {
                                            case "MAXIMUMNUMBEROFENTRIES":
                                                maximumNumberOfHistoryEntries = Utilities.TryParseInt32(
                                                    attribute.Value,
                                                    maximumNumberOfHistoryEntries);
                                                //try
                                                //{
                                                //    maximumNumberOfHistoryEntries = Convert.ToInt32(attribute.Value);
                                                //}
                                                //catch (FormatException)
                                                //{
                                                //}
                                                //catch (OverflowException)
                                                //{
                                                //}

                                                break;
                                            default:
                                                break;
                                        }
                                    }

                                    break;
                                case "LISTS":
                                    listsNode = node;

                                    break;
                                default:
                                    break;
                            }
                        }

                        break;
                    default:
                        break;
                }
            }

            if (name == null)
            {
                Utilities.AddPathInformationAndThrow(
                    new LoadException("The 'name' attribute is missing from the 'Profile' root node."),
                    profileConfigFile);
            }
            else if (syncFrequency == null)
            {
                Utilities.AddPathInformationAndThrow(
                    new LoadException("Missing or invalid sync frequency."),
                    profileConfigFile);
            }
            else if (syncFrequency < 0)
            {
                Utilities.AddPathInformationAndThrow(
                    new LoadException(CultureInfo.CurrentCulture, "The sync frequency cannot be less than zero.", profileConfigFile),
                    profileConfigFile);
            }
            else if (syncFrequencyUnit == null)
            {
                Utilities.AddPathInformationAndThrow(
                    new LoadException("Missing or invalid sync frequency unit."),
                    profileConfigFile);
            }
            //else if (skinName == null)
            //{
            //    Utilities.AddPathInformationAndThrow(
            //        new LoadException("Missing or invalid skin name."),
            //        profileConfigFile);
            //}
            else if (hotkey == null)
            {
                Utilities.AddPathInformationAndThrow(
                    new LoadException("Missing or invalid hotkey."),
                        profileConfigFile);
            }
            //else if (notesHotkey == null)
            //{
            //    notesHotkey = new GlobalHotkey(HotkeyModifierKeys.Win, Keys.J, false);
            //}

            if (skinId == null)
            {
                skinId = ToolkitHost.DefaultSkinId;
            }

            Point? promptLocation;
            if (promptX == null || promptY == null)
            {
                promptLocation = null;
            }
            else
            {
                promptLocation = new Point(promptX.Value, promptY.Value);
            }

            HistoryCollection history = HistoryCollection.FromFile(folder + "History.xml");

            history.MaxCount = maximumNumberOfHistoryEntries;
            history.ComplexHistory.MaxCount = maximumNumberOfHistoryEntries;

            //FileSystemFile skinsDataFile = folder + "SkinData.xml";
            //SkinDataCollection skinsData = null;

            //try
            //{
            //    skinsData = SkinDataCollection.FromXml(skinsDataFile, Globals.Skins);
            //}
            //catch (IOException)
            //{
            //}
            //catch (XmlException ex)
            //{
            //    Utilities.ShowPromptuErrorMessageBox(ex);
            //}
            //catch (LoadException ex)
            //{
            //    Utilities.ShowPromptuErrorMessageBox(ex);
            //}

            //if (skinsData == null)
            //{
            //    skinsData = new SkinDataCollection(skinsDataFile, Globals.Skins);
            //}

            Profile profile = new Profile(
                folder,
                name,
                hotkey,
                //notesHotkey,
                skinId,
                syncFrequency.Value,
                syncFrequencyUnit.Value,
                lists,
                new SuggestionConfig(spacebarConfig),
                commandDefaults,
                selectedListIndex,
                history,
                //skinsData,
                useSentimentalSlickRunFeatures,
                //uiSettings,
                showHiddenFilesAndFolders,
                showSystemFilesAndFolders,
                autoDetectFileAndFolderVisibility,
                showCommandTargetIcons,
                autoFetchFavicons,
                //suggesterSize,
                promptPositioningMode,
                promptLocation,
                showSplashScreen,
                //Profile.LoadPluginsFrom(folder + "plugins", folder + "Plugins.xml"));
                new PluginMetaCollection(folder + "Plugins.xml"));

            if (listsNode != null)
            {
                foreach (XmlAttribute attribute in listsNode.Attributes)
                {
                    switch (attribute.Name.ToUpperInvariant())
                    {
                        case "SELECTEDINDEX":
                            try
                            {
                                try
                                {
                                    selectedListIndex = Convert.ToInt32(attribute.Value, CultureInfo.InvariantCulture);
                                }
                                catch (FormatException)
                                {
                                    selectedListIndex = Convert.ToInt32(attribute.Value, CultureInfo.CurrentCulture);
                                }
                                catch (OverflowException)
                                {
                                    selectedListIndex = Convert.ToInt32(attribute.Value, CultureInfo.CurrentCulture);
                                }
                            }
                            catch (FormatException)
                            {
                                Utilities.AddPathInformationAndThrow(
                                    new LoadException("The selected list index is in an invalid format."),
                                    profileConfigFile);
                            }
                            catch (OverflowException)
                            {
                                Utilities.AddPathInformationAndThrow(
                                    new LoadException("The selected list index is not a valid 32 bit integer."),
                                    profileConfigFile);
                            }
                            break;
                        default:
                            break;
                    }
                }

                foreach (XmlNode childNode in listsNode.ChildNodes)
                {
                    switch (childNode.Name.ToUpperInvariant())
                    {
                        case "LIST":
                            string listFolder = null;
                            string listSyncLocation = null;
                            DateTime? lastUpdateTimestamp = null;
                            DateTime? lastUpdateCheckTimestamp = null;
                            bool owner = false;
                            bool enabled = true;

                            foreach (XmlAttribute attribute in childNode.Attributes)
                            {
                                switch (attribute.Name.ToUpperInvariant())
                                {
                                    case "FOLDER":
                                        listFolder = attribute.Value;
                                        break;
                                    case "SYNCLOCATION":
                                        listSyncLocation = attribute.Value;
                                        break;
                                    case "LASTUPDATE":
                                        lastUpdateTimestamp = Utilities.TryParseBinaryDateTime(attribute.Value, lastUpdateTimestamp);
                                        //try
                                        //{
                                        //    lastUpdateTimestamp = DateTime.FromBinary(Convert.ToInt64(attribute.Value));
                                        //}
                                        //catch (ArgumentException)
                                        //{
                                        //}
                                        //catch (FormatException)
                                        //{
                                        //}
                                        //catch (OverflowException)
                                        //{
                                        //}

                                        break;
                                    case "LASTUPDATECHECK":
                                        lastUpdateCheckTimestamp = Utilities.TryParseBinaryDateTime(attribute.Value, lastUpdateCheckTimestamp);
                                        //try
                                        //{
                                        //    lastUpdateCheckTimestamp = DateTime.FromBinary(Convert.ToInt64(attribute.Value));
                                        //}
                                        //catch (ArgumentException)
                                        //{
                                        //}
                                        //catch (FormatException)
                                        //{
                                        //}
                                        //catch (OverflowException)
                                        //{
                                        //}

                                        break;
                                    case "OWNER":
                                        owner = Utilities.TryParseBoolean(attribute.Value, owner);
                                        //try
                                        //{
                                        //    owner = Convert.ToBoolean(attribute.Value);
                                        //}
                                        //catch (FormatException)
                                        //{
                                        //}

                                        break;
                                    case "ENABLED":
                                        enabled = Utilities.TryParseBoolean(attribute.Value, enabled);
                                        //try
                                        //{
                                        //    enabled = Convert.ToBoolean(attribute.Value);
                                        //}
                                        //catch (FormatException)
                                        //{
                                        //}

                                        break;
                                    default:
                                        break;
                                }
                            }

                            if (listFolder == null ||
                                (!String.IsNullOrEmpty(listSyncLocation) && alreadyUsedSyncLocations.Contains(listSyncLocation)))
                            {
                                continue;
                            }

                            try
                            {
                                FileSystemDirectory listDirectory = folder + listFolder;
                                if (listDirectory.Exists)
                                {
                                    List list = List.FromFolder(listDirectory, profile);
                                    if (String.IsNullOrEmpty(listSyncLocation))
                                    {
                                        list.SyncLocation = null;
                                    }
                                    else
                                    {
                                        list.SyncLocation = new SyncLocation(listSyncLocation);
                                        alreadyUsedSyncLocations.Add(listSyncLocation);
                                    }

                                    list.LastUpdateTimestamp = lastUpdateTimestamp;
                                    list.LastUpdateCheckTimestamp = lastUpdateCheckTimestamp;

                                    list.IsOwnedByUser = owner;
                                    list.Enabled = enabled;

                                    lists.Add(list);
                                }
                            }
                            catch (LoadException ex)
                            {
                                Utilities.ShowPromptuErrorMessageBox(ex);
                            }
                            catch (FileNotFoundException ex)
                            {
                                Utilities.ShowPromptuErrorMessageBox(ex);
                            }
                            catch (XmlException ex)
                            {
                                Utilities.ShowPromptuErrorMessageBox(ex);
                            }
                            catch (DirectoryNotFoundException ex)
                            {
                                Utilities.ShowPromptuErrorMessageBox(ex);
                            }

                            break;
                        default:
                            break;
                    }
                }
            }

            profile.SelectedListIndex = selectedListIndex;

            return profile;
        }

        internal static Profile CreateNewFromUI()
        {
            try
            {
                Profile newProfile = Profile.CreateNew();
                InternalGlobals.ProfilePlacemarks.Add(newProfile.CreateProfilePlacemark());
                return newProfile;
            }
            catch (LoadException ex)
            {
                Utilities.ShowPromptuErrorMessageBox(ex);
            }
            catch (FileNotFoundException ex)
            {
                Utilities.ShowPromptuErrorMessageBox(ex);
            }
            catch (XmlException ex)
            {
                Utilities.ShowPromptuErrorMessageBox(ex);
            }

            return null;
        }

        internal static void GetDefaults(
            out HotkeyModifierKeys modifers,
            out Keys key,
            out string suggestedName)
        {
            XmlDocument document = new XmlDocument();
            document.LoadXml(Defaults.Profile);

            modifers = HotkeyModifierKeys.Win;
            key = Keys.Q;

            string name = "{CurrentUser}'s Profile";

            foreach (XmlNode root in document.ChildNodes)
            {
                if (root.Name.ToUpperInvariant() == "PROFILE")
                {
                    foreach (XmlAttribute attribute in root.Attributes)
                    {
                        switch (attribute.Name.ToUpperInvariant())
                        {
                            case "NAME":
                                name = attribute.Value;
                                break;
                            default:
                                break;
                        }
                    }

                    foreach (XmlNode node in root.ChildNodes)
                    {
                        switch (node.Name.ToUpperInvariant())
                        {
                            case "HOTKEY":
                                GlobalHotkey hotkey = GlobalHotkey.FromXml(node);
                                modifers = hotkey.ModifierKeys;
                                key = hotkey.Key;
                                break;
                            default:
                                break;
                        }
                    }
                }
            }

            suggestedName = name.Replace("{currentuser}", Environment.UserName, true);
            List<string> allProfileNames = new List<string>();
            foreach (ProfilePlacemark profile in InternalGlobals.ProfilePlacemarks)
            {
                allProfileNames.Add(profile.Name);
            }

            suggestedName = GeneralUtilities.GetAvailableIncrementingName(allProfileNames, suggestedName + "{+}", " - ({number})", false, InsertBase.Two);
        }

        internal static Profile CreateNew()
        {
            FileSystemDirectory profilesDirectory = Application.StartupPath + "\\Profiles\\";

            //FileSystemDirectory basicProfileDirectory = profilesDirectory + "Basic";

            FileSystemDirectory newProfileDirectory = profilesDirectory + profilesDirectory.GetAvailableDirectoryName("{+}", "{number}", InsertBase.Zero);;

                newProfileDirectory.CreateIfDoesNotExist();
                File.WriteAllText(newProfileDirectory + "Profile.xml", Defaults.Profile);
                FileSystemDirectory listDirectory = newProfileDirectory + "0";
                listDirectory.CreateIfDoesNotExist();
                File.WriteAllText(listDirectory + "List.xml", Defaults.List);
                File.WriteAllText(listDirectory + "Functions.xml", Defaults.Functions);
                File.WriteAllText(listDirectory + "Commands.xml", Defaults.Commands);
                File.WriteAllText(listDirectory + "AssemblyReferences.xml", Defaults.AssemblyReferences);
                File.WriteAllText(listDirectory + "AssemblyReferencesManifest.xml", Defaults.AssemblyReferencesManifest);
                File.WriteAllText(listDirectory + "ValueLists.xml", Defaults.ValueLists);
            //try
            //{
            //basicProfileDirectory.CopyTo(newProfileDirectory);
            Profile newProfile = Profile.FromFolder(newProfileDirectory);
            //profile.directory = newProfileDirectory;
            string suggestedName = newProfile.Name.Replace("{currentuser}", Environment.UserName, true);
            List<string> allProfileNames = new List<string>();
            foreach (ProfilePlacemark profile in InternalGlobals.ProfilePlacemarks)
            {
                allProfileNames.Add(profile.Name);
            }

            newProfile.Name = GeneralUtilities.GetAvailableIncrementingName(allProfileNames, suggestedName + "{+}", " - ({number})", false, InsertBase.Two);
            
            foreach (List list in newProfile.Lists)
            {
                list.Name = list.Name.Replace("{currentuser}", Environment.UserName, true);
            }

            newProfile.SaveAll();

            return newProfile;
            //}
            //catch (FatalLoadException ex)
            //{
            //    MessageBox.Show(ex.Message); // TODO implement better message reporting
            //}
            //catch (FileNotFoundException ex)
            //{
            //    MessageBox.Show(ex.Message);
            //}
            //catch (XmlException ex)
            //{
            //    MessageBox.Show(ex.Message);
            //}
        }

        private bool GetWhetherFileSystemValidationCanceled()
        {
            return this.cancelFileSystemValidations;
        }

        internal void QueueCommandFileSystemValidations()
        {
            if (!this.fileSystemCommandValidationRunning)
            {
                this.cancelFileSystemValidations = false;
                this.fileSystemCommandValidationRunning = true;
                CommandValidation lastValidation = null;

                Getter<bool> cancelationCallback = this.GetWhetherFileSystemValidationCanceled;

                foreach (List list in this.lists)
                {
                    using (DdMonitor.Lock(list.Commands))
                    {
                        foreach (Command command in list.Commands)
                        {
                            lastValidation = new CommandValidation(command, list, cancelationCallback);
                            this.backgroundWorkQueue.AddWork(new ParameterlessVoid(lastValidation.ValidateIsFileSystem));
                        }
                    }
                }

                if (!this.primaryFileSystemCommandValidationCompleted && lastValidation != null)
                {
                    lastValidation.ValidationCompleted += this.HandleLastFileSystemCommandValidationValidationCompleted;
                }
            }
        }

        internal void CancelFileSystemValidations()
        {
            this.cancelFileSystemValidations = true;
        }

        private void HandleLastFileSystemCommandValidationValidationCompleted(object sender, EventArgs e)
        {
            this.primaryFileSystemCommandValidationCompleted = true;
            this.fileSystemCommandValidationRunning = false;
        }

        internal ProfilePlacemark CreateProfilePlacemark()
        {
            return new ProfilePlacemark(this.Directory, this.Name, this.ShowSplashScreen);
        }

        internal void SyncLocalAssemblyReferences()
        {
            //foreach (List list in this.lists)
            //{
            //    list.SyncLocalAssemblyReferences();
            //}
        }

        internal void UpdateLockFile()
        {
            if (!this.IsExternallyLocked)
            {
                StringBuilder lockFileContents = new StringBuilder();
                lockFileContents.AppendFormat(CultureInfo.InvariantCulture, "{0}\n{1}\n{2}", DateTime.Now.ToBinary().ToString(CultureInfo.InvariantCulture), Environment.UserName, Environment.MachineName);
                try
                {
                    this.LockFile.WriteAllText(lockFileContents.ToString());
                }
                catch (IOException)
                {
                }
            }
        }

        public void UpdateAllCachedIconsAsync(bool userInitiated)
        {
            if (!this.ShowCommandTargetIcons)
            {
                return;
            }

            Thread cacheThread = new Thread(this.UpdateAllCachedIcons);
            cacheThread.IsBackground = true;
            cacheThread.Priority = ThreadPriority.Lowest;
            cacheThread.Start(userInitiated);
            //BackgroundWorker worker = new BackgroundWorker();
            //worker.DoWork += HandleUpdateAllCachedIconsAsync;
            //worker.RunWorkerAsync();
        }

        private void PurgeUnusedCachedIcons(List<CompositeItem<Command, List>> allCommandsAndLists)
        {
            if (!this.IconCacheDirectory.Exists)
            {
                return;
            }
           // Stopwatch s = new Stopwatch();
           // s.Start();
            //TrieList usedIcons = new TrieList(SortMode.Alphabetical);
            //TrieList usedWebIcons = new TrieList(SortMode.Alphabetical);
            List<string> usedIcons = new List<string>();//new TrieList(SortMode.Alphabetical);
            List<string> usedWebIcons = new List<string>();//new TrieList(SortMode.Alphabetical);

            foreach (CompositeItem<Command, List> command in allCommandsAndLists)
            {
                FileSystemFile? file = command.Item.GetIconPath(command.ListFrom);
                if (file != null)
                {
                    string fileName = file.Value.Name.ToUpperInvariant();
                    if (file.Value.GetParentDirectory().Name.ToUpperInvariant() == "WEB")
                    {
                        //if (!usedWebIcons.Contains(fileName, CaseSensitivity.Sensitive))
                        //{
                            usedWebIcons.Add(fileName);
                        //}
                    }
                    else
                    {
                        //if (!usedIcons.Contains(fileName, CaseSensitivity.Sensitive))
                        //{
                            usedIcons.Add(file.Value.Name.ToUpperInvariant());
                        //}
                    }
                }
            }

            try
            {
                foreach (FileSystemFile file in this.IconCacheDirectory.GetFiles())
                {
                    if (!usedIcons.Contains(file.Name.ToUpperInvariant()))//, CaseSensitivity.Sensitive))
                    {
                        file.Delete();
                    }
                }
            }
            catch (IOException)
            {
            }

            FileSystemDirectory webIconCache = this.IconCacheDirectory + "web";
            try
            {
                foreach (FileSystemFile file in webIconCache.GetFiles())
                {
                    string name = file.Name.ToUpperInvariant();
                    if (name != "404.XML" && !usedWebIcons.Contains(name))//, CaseSensitivity.Sensitive))
                    {
                        file.Delete();
                    }
                }
            }
            catch (IOException)
            {
            }

            //s.Stop();
            //Debug.WriteLine(String.Format("Elapsed: {0}", s.ElapsedMilliseconds));
        }

        //private void HandleUpdateAllCachedIconsAsync(object arg)//(object sender, DoWorkEventArgs e)
        //{
        //    this.UpdateAllCachedIconsList((List<CompositeItem<Command, List>>)arg);
        //}

        private List<CompositeItem<Command, List>> GetAllCommandsAndLists()
        {
            List<CompositeItem<Command, List>> commands = new List<CompositeItem<Command,List>>();

            using (DdMonitor.Lock(this.Lists))
            {
                foreach (List list in this.Lists)
                {
                    using (DdMonitor.Lock(list.Commands))
                    {
                        foreach (Command command in list.Commands)
                        {
                            commands.Add(new CompositeItem<Command, List>(command, list));
                        }
                    }
                }
            }

            return commands;
        }

        private void UpdateCachedIconsList(
            List<CompositeItem<Command, List>> commands, 
            bool userInitiated, 
            DoWorkEventArgs e, 
            IProgressReporter progressReporter)
        {
            PromptuWebClient webClient = Updater.GetFaviconWebClient();

            for (int i = 0; i < commands.Count; i++)
            {
                if (e != null && e.Cancel)
                {
                    return;
                }

                CompositeItem<Command, List> command = commands[i];

                command.Item.UpdateCacheIcon(command.ListFrom, userInitiated, webClient);

                if (progressReporter != null)
                {
                    progressReporter.ReportProgress(((double)(i + 1) / ((double)commands.Count)) * 100);
                }
            }
        }

        private void UpdateAllCachedIcons(object userInitiated)
        {
            this.UpdateAllCachedIcons((bool)userInitiated, true, null, null);
        }

        internal void UpdateAllCachedIcons(bool userInitiated, bool purge, DoWorkEventArgs e, IProgressReporter progressReporter)
        {
            if (!this.ShowCommandTargetIcons)
            {
                return;
            }

            List<CompositeItem<Command, List>> allCommandsAndLists = this.GetAllCommandsAndLists();

            if (e != null && e.Cancel)
            {
                return;
            }

            if (purge)
            {
                this.PurgeUnusedCachedIcons(allCommandsAndLists);

                if (e != null && e.Cancel)
                {
                    return;
                }
            }

            this.UpdateCachedIconsList(allCommandsAndLists, userInitiated, e, progressReporter);
            this.WebIconLastModifiedDictionary.Save();
            //PromptHandler.GetInstance().NotifySuggestionAffectingChange();
        }

        internal void DeleteLockFile()
        {
            if (!this.IsExternallyLocked)
            {
                this.LockFile.DeleteIfExists();
            }
        }

        internal void SaveAll()
        {
            this.SaveConfig();
            this.history.Save();
            foreach (List list in this.lists)
            {
                list.SaveAll();
            }
        }

        private void SaveUIConfig(object sender, EventArgs e)
        {
            this.SaveUIConfig();
        }

        public void SaveUIConfig()
        {
            // HACK disabled
            //if (this.blockSaveUIConfig)
            //{
            //    return;
            //}

            //XmlDocument document = new XmlDocument();

            //XmlNode uiSettingsNode = this.uiSettings.ToXml(document);

            //document.AppendChild(uiSettingsNode);

            //Globals.FailedToSaveFiles.Remove(null, UIConfigResaveId);

            //string path = this.Directory + "UI.xml";
            //try
            //{
            //    document.Save(path);
            //}
            //catch (IOException)
            //{
            //    Globals.FailedToSaveFiles.Add(new FailedToSaveFile(null, UIConfigResaveId, path, new ResaveHandler(Globals.ResaveProfileItem)));
            //}
        }

        private void ListLastUpdateCheckTimestampChanged(object sender, EventArgs e)
        {
            this.SaveConfig();
            //if (!PromptHandler.IsInitializing)
            //{
            //    PromptHandler.GetInstance().SetupDialog.ListSelector.UpdateListInfoDisplay();
            //}
        }

        internal void SaveConfig()
        {
            XmlDocument document = new XmlDocument();

            XmlNode profileNode = document.CreateElement("Profile");
            profileNode.Attributes.Append(XmlUtilities.CreateAttribute("name", this.Name, document));
            if (this.UseSentimentalSlickRunFeatures)
            {
                profileNode.Attributes.Append(XmlUtilities.CreateAttribute("useSentimentalSlickRunFeatures", this.UseSentimentalSlickRunFeatures, document));
            }

            if (!this.ShowSplashScreen)
            {
                profileNode.Attributes.Append(XmlUtilities.CreateAttribute("showSplashScreen", this.ShowSplashScreen, document));
            }

            string shortSyncFrequencyUnit;

            switch (this.syncFrequencyUnit)
            {
                case TimeUnit.Hour:
                    shortSyncFrequencyUnit = "hrs";
                    break;
                case TimeUnit.Second:
                    shortSyncFrequencyUnit = "sec";
                    break;
                default:
                    shortSyncFrequencyUnit = "min";
                    break;
            }

            XmlNode syncNode = document.CreateElement("Sync");
            syncNode.Attributes.Append(XmlUtilities.CreateAttribute("every", this.SyncFrequency + shortSyncFrequencyUnit, document));

            profileNode.AppendChild(syncNode);

            XmlNode skinNode = document.CreateElement("Skin");
            skinNode.Attributes.Append(XmlUtilities.CreateAttribute("id", this.skinId, document));

            profileNode.AppendChild(skinNode);

            XmlNode hotkeyNode = document.CreateElement("Hotkey");
            this.hotkey.ToXml(hotkeyNode);

            profileNode.AppendChild(hotkeyNode);

            //XmlNode notesNode = document.CreateElement("Notes");
            //XmlNode notesHotkeyNode = document.CreateElement("Hotkey");
            //this.notesHotkey.ToXml(notesHotkeyNode, document);
            //notesNode.AppendChild(notesHotkeyNode);

            //profileNode.AppendChild(notesNode);

            XmlNode defaultsNode = document.CreateElement("Defaults");
            XmlNode commandNode = document.CreateElement("Command");
            this.commandDefaults.ToXml(commandNode, document);

            defaultsNode.AppendChild(commandNode);
            profileNode.AppendChild(defaultsNode);

            XmlNode promptNode = document.CreateElement("Prompt");
            if (this.PromptPositioningMode != PositioningMode.FollowMouse)
            {
                promptNode.Attributes.Append(XmlUtilities.CreateAttribute("positioningMode", this.promptPositioningMode.ToString(), document));
            }

            if (this.PromptLocation != null)
            {
                promptNode.Attributes.Append(XmlUtilities.CreateAttribute("x", this.PromptLocation.Value.X, document));
                promptNode.Attributes.Append(XmlUtilities.CreateAttribute("y", this.PromptLocation.Value.Y, document));
            }

            XmlNode whileSuggestionNode = document.CreateElement("WhileSuggesting");
            XmlNode spacebarNode = document.CreateElement("Spacebar");
            this.suggestionConfig.Spacebar.ToXml(spacebarNode, document);

            whileSuggestionNode.AppendChild(spacebarNode);
            promptNode.AppendChild(whileSuggestionNode);
            profileNode.AppendChild(promptNode);

            XmlNode fileAndFolderNavigationNode = document.CreateElement("FileAndFolderNavigation");
            fileAndFolderNavigationNode.Attributes.Append(XmlUtilities.CreateAttribute("autoDetectVisibility", this.AutoDetectFileAndFolderVisibility, document));
            fileAndFolderNavigationNode.Attributes.Append(XmlUtilities.CreateAttribute("showHidden", this.ShowHiddenFilesAndFolders, document));
            fileAndFolderNavigationNode.Attributes.Append(XmlUtilities.CreateAttribute("showSystem", this.ShowSystemFilesAndFolders, document));
            profileNode.AppendChild(fileAndFolderNavigationNode);

            XmlNode targetIconsNode = document.CreateElement("TargetIcons");
            targetIconsNode.Attributes.Append(XmlUtilities.CreateAttribute("show", this.ShowCommandTargetIcons, document));
            targetIconsNode.Attributes.Append(XmlUtilities.CreateAttribute("autoFetchFavicons", this.AutoFetchFavicons, document));
            profileNode.AppendChild(targetIconsNode);

            //XmlNode suggesterSizeNode = document.CreateElement("Suggester");
            //suggesterSizeNode.Attributes.Append(XmlUtilities.CreateAttribute("width", this.suggesterSize.Width, document));
            //suggesterSizeNode.Attributes.Append(XmlUtilities.CreateAttribute("height", this.suggesterSize.Height, document));

            //profileNode.AppendChild(suggesterSizeNode);

            XmlNode historyNode = document.CreateElement("History");
            historyNode.Attributes.Append(XmlUtilities.CreateAttribute("maximumNumberOfEntries", this.history.MaxCount, document));
            profileNode.AppendChild(historyNode);

            XmlNode listsNode = document.CreateElement("Lists");

            listsNode.Attributes.Append(XmlUtilities.CreateAttribute("selectedIndex", this.selectedListIndex, document));

            foreach (List list in this.lists)
            {
                XmlNode listNode = document.CreateElement("List");
                if (list.IsOwnedByUser)
                {
                    listNode.Attributes.Append(XmlUtilities.CreateAttribute("owner", list.IsOwnedByUser, document));
                }
                
                if (!list.Enabled)
                {
                    listNode.Attributes.Append(XmlUtilities.CreateAttribute("enabled", list.Enabled, document));
                }

                listNode.Attributes.Append(XmlUtilities.CreateAttribute("folder", list.FolderName, document));

                if (list.SyncLocation != null)
                {
                    listNode.Attributes.Append(XmlUtilities.CreateAttribute("syncLocation", list.SyncLocation.GetRelativePath(), document));
                }

                if (list.LastUpdateTimestamp != null)
                {
                    listNode.Attributes.Append(XmlUtilities.CreateAttribute("lastUpdate", list.LastUpdateTimestamp.Value.ToBinary(), document));
                }

                if (list.LastUpdateCheckTimestamp != null)
                {
                    listNode.Attributes.Append(XmlUtilities.CreateAttribute("lastUpdateCheck", list.LastUpdateCheckTimestamp.Value.ToBinary(), document));
                }

                listsNode.AppendChild(listNode);
            }

            profileNode.AppendChild(listsNode);

            document.AppendChild(profileNode);

            this.Directory.CreateIfDoesNotExist();

            InternalGlobals.FailedToSaveFiles.Remove(null, ConfigResaveId);

            string path = this.Directory + "Profile.xml";
            try
            {
                document.Save(path);
            }
            catch (IOException)
            {
                // TODO add code when switch profile
                // TODO add menuitem for notes
                // TODO test notes scroll
                //if (!resaving)
                //{
                InternalGlobals.FailedToSaveFiles.Add(new FailedToSaveFile(null, ConfigResaveId, path, new ResaveHandler(InternalGlobals.ResaveProfileItem)));
                //}

            }
        }

        internal List CreateNewList(string baseNameFormat, bool empty)
        {
            string newName = GeneralUtilities.GetAvailableIncrementingName<List>(
                this.lists,
                baseNameFormat == null ? Localization.Promptu.NewListNameBasicFomat : baseNameFormat,
                Localization.Promptu.NewListNameInsertFormat,
                false,
                InsertBase.Two);

            FileSystemDirectory listDirectory = this.Directory + this.Directory.GetAvailableDirectoryName("{+}", "{number}", InsertBase.Zero);

            List list;
            if (!empty)
            {
                listDirectory.CreateIfDoesNotExist();
                File.WriteAllText(listDirectory + "List.xml", Defaults.List);
                File.WriteAllText(listDirectory + "Functions.xml", Defaults.Functions);
                File.WriteAllText(listDirectory + "Commands.xml", Defaults.Commands);
                File.WriteAllText(listDirectory + "AssemblyReferences.xml", Defaults.AssemblyReferences);
                File.WriteAllText(listDirectory + "AssemblyReferencesManifest.xml", Defaults.AssemblyReferencesManifest);
                File.WriteAllText(listDirectory + "ValueLists.xml", Defaults.ValueLists);

                list = List.FromFolder(listDirectory, this);
                list.Name = newName;
            }
            else
            {
                list = new List(
                listDirectory,
                newName,
                this);
            }

            this.lists.Add(list);
            list.SaveAll();
            this.SaveConfig();

            if (!empty)
            {
                try
                {
                    using (DdMonitor.Lock(list.AssemblyReferences))
                    {
                        foreach (AssemblyReference reference in list.AssemblyReferences)
                        {
                            reference.SyncIfNessesary();
                        }
                    }
                }
                catch (IOException)
                {
                }
                catch (CorruptPackageException)
                {
                }
                catch (NewerPackageVersionException)
                {
                }
                catch (FileFileSystem.FileFileSystemException)
                {
                }
                catch (XmlException)
                {
                }
                catch (LoadException)
                {
                }
            }

            return list;
        }

        internal void DeleteList(List list)
        {
            if (!this.lists.Contains(list))
            {
                throw new ArgumentException("The list is not in the list of lists.");
            }

            this.DeleteList(this.lists.IndexOf(list));
        }

        internal void DeleteList(int index)
        {
            if (index < 0)
            {
                throw new ArgumentOutOfRangeException("'index' cannot be less than zero.");
            }
            else if (index >= this.lists.Count)
            {
                throw new ArgumentOutOfRangeException("'index' cannot be greater than or equal to the number of lists.");
            }

            List list = this.lists[index];
            list.Directory.Delete();
            this.lists.RemoveAt(index);
            this.SaveConfig();
        }

        //public void UpdateOptimizedCommandNames()
        //{
        //    this.optimizedCommandNames.Clear();
        //    foreach (Command command in this.CompositeCommands.GetComposite())
        //    {
        //        this.optimizedCommandNames.Add(command.Name);
        //    }
        //}

        private void UpdateSyncTimer()
        {
            if (this.syncTimer != null)
            {
                this.syncTimer.Dispose();
            }

            int syncFrequencyInMilliseconds = this.SyncFrequency;
            switch (this.SyncFrequencyUnit)
            {
                case TimeUnit.Hour:
                    syncFrequencyInMilliseconds *= 3600000;
                    break;
                case TimeUnit.Second:
                    syncFrequencyInMilliseconds *= 1000;
                    break;
                default:
                    syncFrequencyInMilliseconds *= 60000;
                    break;
            }

            int mininumSyncFrequencyInMilliseconds = MininumSyncTimeInSeconds * 1000;
            if (syncFrequencyInMilliseconds < mininumSyncFrequencyInMilliseconds)
            {
                syncFrequencyInMilliseconds = mininumSyncFrequencyInMilliseconds;
            }

            this.syncTimer = new System.Timers.Timer(syncFrequencyInMilliseconds);
            this.syncTimer.Elapsed += this.TimeToSync;
            this.syncTimer.AutoReset = true;
            this.EnableSynchronization();
        }

        private void HandleHotkeyChanged(object sender, EventArgs e)
        {
            this.SaveConfig();
        }

        internal void PauseSyncTimers()
        {
            if (!this.syncTimersPaused)
            {
                this.syncTimerWasGoing = this.syncTimer.Enabled;
                this.retrySyncTimerWasGoing = this.retrySyncTimer.Enabled;
                this.syncTimer.Stop();
                this.retrySyncTimer.Stop();
                this.syncTimersPaused = true;
            }
        }

        internal void UnPauseSyncTimers()
        {
            if (this.syncTimersPaused)
            {
                if (this.syncTimerWasGoing)
                {
                    this.syncTimer.Start();
                }

                if (this.retrySyncTimerWasGoing)
                {
                    this.retrySyncTimer.Start();
                }
            }
        }

        private void TimeToSync(object sender, EventArgs e)
        {
            this.syncTimer.Stop();
            this.SyncInternal(false, false);
            this.syncTimer.Start();
        }

        private void TimeToRetrySync(object sender, EventArgs e)
        {
            this.retrySyncTimer.Stop();
            if (this.retrySyncCount < 3)
            {
                this.SyncInternal(true, false);
                retrySyncCount++;
            }
        }

        internal Dictionary<List, Exception> Sync(ref List<DiffDiffBase> needToAskUserAbout, out ListCollection conflictingChangesLists)
        {
            Dictionary<List, Exception> exceptions = new Dictionary<List, Exception>();
            conflictingChangesLists = new ListCollection();
            int lastNeedToAskUserAboutCount = 0;
            foreach (List list in this.lists)
            {
                try
                {
                    list.SyncIfNecessary(ref needToAskUserAbout, false);
                    if (lastNeedToAskUserAboutCount != needToAskUserAbout.Count)
                    {
                        lastNeedToAskUserAboutCount = needToAskUserAbout.Count;
                        conflictingChangesLists.Add(list);
                    }

                    using (DdMonitor.Lock(list.AssemblyReferences))
                    {
                        foreach (AssemblyReference reference in list.AssemblyReferences)
                        {
                            reference.SyncIfNessesary();
                        }
                    }
                }
                catch (IOException ex)
                {
                    exceptions.Add(list, ex);
                }
                catch (CorruptPackageException ex)
                {
                    exceptions.Add(list, ex);
                }
                catch (NewerPackageVersionException ex)
                {
                    exceptions.Add(list, ex);
                }
                catch (FileFileSystem.FileFileSystemException ex)
                {
                    exceptions.Add(list, ex);
                }
                catch (System.Xml.XmlException ex)
                {
                    exceptions.Add(list, ex);
                }
                catch (LoadException ex)
                {
                    exceptions.Add(list, ex);
                }
            }

            return exceptions;
        }

        internal void AutoRetrySyncAsync()
        {
            BackgroundWorker worker = new BackgroundWorker();
            worker.DoWork += this.AutoRetrySync;
            worker.RunWorkerAsync();
        }

        private void AutoRetrySync(object sender, DoWorkEventArgs e)
        {
            this.AutoRetrySync();
        }

        internal void AutoRetrySync()
        {
            this.SyncInternal(false, false);
        }

        internal ParameterlessVoid GetSyncCallback()
        {
            return new ParameterlessVoid(this.ForcedSync);
        }

        private void ForcedSync()
        {
            this.SyncInternal(false, true);
        }

        private void SyncInternal()
        {
            this.SyncInternal(false, false);
        }

        internal void Sync()
        {
            this.SyncInternal(false, false);
        }

        private void SyncInternal(bool retryingSync, bool forceSyncs)
        {
            if (!InternalGlobals.SyncAllowed)
            {
                return;
            }

            this.retrySyncTimer.Stop();
            IEnumerable<List> listsToSync;

            if (retryingSync)
            {
                listsToSync = new List<List>(this.retrySyncLists);
            }
            else
            {
                this.retrySyncCount = 0;
                listsToSync = this.lists;
            }

            this.retrySyncLists.Clear();

            List<DiffDiffBase> needToAskUserAbout = new List<DiffDiffBase>();

            ListCollection listsToSave = new ListCollection();

            int lastNeedToAskUserAboutCount = 0;

            foreach (List list in listsToSync)
            {
                try
                {
                    list.SyncIfNecessary(ref needToAskUserAbout, forceSyncs);
                    if (lastNeedToAskUserAboutCount != needToAskUserAbout.Count)
                    {
                        lastNeedToAskUserAboutCount = needToAskUserAbout.Count;
                        listsToSave.Add(list);
                    }

                    using (DdMonitor.Lock(list.AssemblyReferences))
                    {
                        foreach (AssemblyReference reference in list.AssemblyReferences)
                        {
                            reference.SyncIfNessesary();
                        }
                    }
                }
                catch (IOException)
                {
                    this.retrySyncLists.Add(list);
                }
                catch (CorruptPackageException)
                {
                }
                catch (NewerPackageVersionException)
                {
                    if (!notifiedIncompatableLists.Contains(list))
                    {
                        notifiedIncompatableLists.Add(list);
                        UIMessageBox.Show(
                            String.Format(CultureInfo.CurrentCulture, Localization.MessageFormats.NewerPackageUnreadableMessageFormat, list.SyncLocation.Path),
                            Localization.Promptu.AppName,
                            UIMessageBoxButtons.OK,
                            UIMessageBoxIcon.Warning,
                            UIMessageBoxResult.OK);
                    }
                }
                catch (FileFileSystem.FileFileSystemException)
                {
                }
                catch (XmlException)
                {
                }
                catch (LoadException)
                {
                }
            }

            if (needToAskUserAbout.Count > 0)
            {
                InternalGlobals.GuiManager.ToolkitHost.MainThreadDispatcher.Invoke(new ParameterlessVoid(delegate
                    {
                        CollisionResolvingDialogPresenter dialog = new CollisionResolvingDialogPresenter(needToAskUserAbout);
                        //InternalGlobals.UISettings.CollisionResolvingDialogSettings.ImpartTo(dialog.NativeInterface);
                        dialog.ShowDialog(Skins.PromptHandler.GetDialogOwner());
                        //InternalGlobals.UISettings.CollisionResolvingDialogSettings.UpdateFrom(dialog.NativeInterface);
                    }), null);

                listsToSave.SaveAll();
            }

            if (this.retrySyncLists.Count > 0)
            {
                this.retrySyncTimer.Start();
            }

            this.OnListsSynchronized(EventArgs.Empty);
        }

        internal void DisableSynchronization()
        {
            this.syncTimer.Stop();
        }

        internal void EnableSynchronization()
        {
            if (InternalGlobals.CurrentProfile == this)
            {
                this.syncTimer.Start();
            }
        }

        protected void RaiseSyncAffectingChangeOccuringEvent(object sender, EventArgs e)
        {
            this.OnSyncAffectingChangeOccuring(EventArgs.Empty);
        }

        protected virtual void OnListsSynchronized(EventArgs e)
        {
            EventHandler handler = this.ListsSynchronized;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        protected virtual void OnSyncAffectingChangeOccuring(EventArgs e)
        {
            EventHandler handler = this.SyncAffectingChangeOccuring;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            PropertyChangedEventHandler handler = this.PropertyChanged;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        //private void CacheIcons

        //private static PromptuPluginCollection LoadPluginsFrom(FileSystemDirectory directory, FileSystemFile metaFile)
        //{
        //    PromptuPluginCollection pluginCollection = new PromptuPluginCollection(metaFile);

        //    foreach (FileSystemDirectory childDirectory in directory.GetDirectories())
        //    {
        //        PromptuPlugin plugin = PromptuPlugin.FromFolder(childDirectory);

        //        if (plugin != null)
        //        {
        //            pluginCollection.Add(plugin);
        //        }
        //    }

        //    return pluginCollection;
        //}
    }
}
