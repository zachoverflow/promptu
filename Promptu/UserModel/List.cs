using System;
using System.Collections.Generic;
using System.Collections.Generic.Extensions;
using System.Text;
using ZachJohnson.Promptu.Collections;
using System.Xml;
using System.Xml.Extensions;
using System.IO;
using ZachJohnson.Promptu.FileFileSystem;
using ZachJohnson.Promptu.UserModel.Differencing;
using ZachJohnson.Promptu.UI;
using System.IO.Extensions;
//using ZachJohnson.Promptu.DynamicEntryModel;
using ZachJohnson.Promptu.UserModel.Collections;
using ZachJohnson.Promptu.UIModel.Presenters;
using System.ComponentModel;
using System.Threading;
using System.Threading.Extensions;
using System.Globalization;

namespace ZachJohnson.Promptu.UserModel
{
    internal class List : INamed, INotifyPropertyChanged
    {
        internal const string ListConfigId = "listconfig";
        private const int currentPackageVersion = 1;
        private FileSystemDirectory directory;
        private SyncLocation syncLocation;
        private string name;
        private CommandCollectionWrapper commands;
        private FunctionCollectionWrapper functions;
        private AssemblyReferenceCollectionWrapper assemblyReferences;
        private ValueListCollectionWrapper valueLists;
        private DateTime? lastUpdateTimestamp;
        private DateTime? lastUpdateCheckTimestamp;
        private bool isOwnedByUser;
        private AssemblyReferencesManifest assemblyReferencesManifest;
        private bool publishOnChildrenSaved = true;
        private ListSyncInfo syncInfo;
        private bool ignorePrepareForSync;
        private bool enabled = true;
        private Profile associatedProfile;
        private ActionSyncManager publishSyncManager = new ActionSyncManager();
        private object exportToken = new object();

        public List(
            FileSystemDirectory directory,
            string name,
            Profile associatedProfile)
            : this(
            directory,
            name,
            new CommandCollectionWrapper(directory + "Commands.xml", null),
            new FunctionCollectionWrapper(directory + "Functions.xml", null),
            new AssemblyReferenceCollectionWrapper(directory + "AssemblyReferences.xml", null),
            new AssemblyReferencesManifest(directory + "AssemblyReferencesManifest.xml"),
            new ValueListCollectionWrapper(directory + "ValueLists.xml", null),
            associatedProfile)
        {
        }

#if DEBUG
        public List(
            FileSystemDirectory directory,
            string name,
            CommandCollectionWrapper commands,
            FunctionCollectionWrapper functions,
            AssemblyReferenceCollectionWrapper assemblyReferences,
            AssemblyReferencesManifest assemblyReferencesManifest,
            ValueListCollectionWrapper valueLists)
        {
            if (name == null)
            {
                throw new ArgumentNullException("name");
            }
            else if (commands == null)
            {
                throw new ArgumentNullException("commands");
            }
            else if (functions == null)
            {
                throw new ArgumentNullException("functions");
            }
            else if (assemblyReferences == null)
            {
                throw new ArgumentNullException("assemblyReferences");
            }
            else if (assemblyReferencesManifest == null)
            {
                throw new ArgumentNullException("assemblyReferencesManifest");
            }
            else if (valueLists == null)
            {
                throw new ArgumentNullException("valueLists");
            }

            this.directory = directory;
            this.name = name;
            this.commands = commands;
            this.functions = functions;
            this.assemblyReferences = assemblyReferences;
            this.valueLists = valueLists;
            this.Commands.Saved += this.ChildrenSaved;
            this.functions.Saved += this.ChildrenSaved;
            this.assemblyReferences.Saved += this.ChildrenSaved;
            this.valueLists.Saved += this.ChildrenSaved;
            this.assemblyReferencesManifest = assemblyReferencesManifest;
            this.syncInfo = new ListSyncInfo();

            this.Commands.AddingItem += this.PrepareForSync;
            this.Commands.RemovingItem += this.PrepareForSync;
            this.functions.AddingItem += this.PrepareForSync;
            this.functions.RemovingItem += this.PrepareForSync;
            this.assemblyReferences.AddingItem += this.PrepareForSync;
            this.assemblyReferences.RemovingItem += this.PrepareForSync;
            this.valueLists.AddingItem += this.PrepareForSync;
            this.valueLists.RemovingItem += this.PrepareForSync;
        }
#endif

        public List(
            FileSystemDirectory directory, 
            string name,
            CommandCollectionWrapper commands, 
            FunctionCollectionWrapper functions, 
            AssemblyReferenceCollectionWrapper assemblyReferences,
            AssemblyReferencesManifest assemblyReferencesManifest,
            ValueListCollectionWrapper valueLists,
            Profile associatedProfile)
        {
            if (name == null)
            {
                throw new ArgumentNullException("name");
            }
            else if (commands == null)
            {
                throw new ArgumentNullException("commands");
            }
            else if (functions == null)
            {
                throw new ArgumentNullException("functions");
            }
            else if (assemblyReferences == null)
            {
                throw new ArgumentNullException("assemblyReferences");
            }
            else if (assemblyReferencesManifest == null)
            {
                throw new ArgumentNullException("assemblyReferencesManifest");
            }
            else if (associatedProfile == null)
            {
                throw new ArgumentNullException("associatedProfile");
            }
            else if (valueLists == null)
            {
                throw new ArgumentNullException("valueLists");
            }

            this.directory = directory;
            this.name = name;
            this.commands = commands;
            this.functions = functions;
            this.assemblyReferences = assemblyReferences;
            this.valueLists = valueLists;
            this.Commands.Saved += this.ChildrenSaved;
            this.functions.Saved += this.ChildrenSaved;
            this.assemblyReferences.Saved += this.ChildrenSaved;
            this.valueLists.Saved += this.ChildrenSaved;
            this.assemblyReferencesManifest = assemblyReferencesManifest;
            this.syncInfo = new ListSyncInfo();

            //this.syncInfo.AssemblyReferenceIdentifierChanges.AddingItem += this.PrepareForSync;
            //this.syncInfo.AssemblyReferenceIdentifierChanges.RemovingItem += this.PrepareForSync;
            //this.syncInfo.CommandIdentifierChanges.AddingItem += this.PrepareForSync;
            //this.syncInfo.CommandIdentifierChanges.RemovingItem += this.PrepareForSync;
            //this.syncInfo.FunctionIdentifierChanges.AddingItem += this.PrepareForSync;
            //this.syncInfo.FunctionIdentifierChanges.RemovingItem += this.PrepareForSync;
            this.Commands.AddingItem += this.PrepareForSync;
            this.Commands.RemovingItem += this.PrepareForSync;
            this.functions.AddingItem += this.PrepareForSync;
            this.functions.RemovingItem += this.PrepareForSync;
            this.assemblyReferences.AddingItem += this.PrepareForSync;
            this.assemblyReferences.RemovingItem += this.PrepareForSync;
            this.valueLists.AddingItem += this.PrepareForSync;
            this.valueLists.RemovingItem += this.PrepareForSync;

            this.associatedProfile = associatedProfile;

            //if (commandIdGenerator == null)
            //{
            //    this.commandIdGenerator = new IdGenerator();
            //}
            //else
            //{
            //    this.commandIdGenerator = commandIdGenerator;
            //}

            //if (functionIdGenerator == null)
            //{
            //    this.functionIdGenerator = new IdGenerator();
            //}
            //else
            //{
            //    this.functionIdGenerator = functionIdGenerator;
            //}

            //if (assemblyReferenceIdGenerator == null)
            //{
            //    this.assemblyReferenceIdGenerator = new IdGenerator();
            //}
            //else
            //{
            //    this.assemblyReferenceIdGenerator = assemblyReferenceIdGenerator;
            //}
        }

        public event EventHandler SyncAffectingChangeOccuring;

        public event EventHandler LastUpdateCheckTimestampChanged;

        public event EventHandler EnabledChanged;

        public event PropertyChangedEventHandler PropertyChanged;

        public bool Enabled
        {
            get 
            { 
                return this.enabled; 
            }

            set 
            {
                if (this.enabled != value)
                {
                    this.enabled = value;
                    this.OnEnabledChanged(EventArgs.Empty);
                    this.RaisePropertyChanged("Enabled");
                }
            }
        }

        public bool IsOwnedByUser
        {
            get { return this.isOwnedByUser; }
            set { this.isOwnedByUser = value; }
        }

        public bool PublishOnChildrenSaved
        {
            get { return this.publishOnChildrenSaved; }
            set { this.publishOnChildrenSaved = value; }
        }

        public SyncLocation SyncLocation
        {
            get 
            {
                return this.syncLocation; 
            }

            set
            { 
                SyncLocation oldLocation = this.syncLocation;
                if (oldLocation != null)
                {
                    oldLocation.FileChanged -= this.HandleSyncLocationFileChanged;
                }

                this.syncLocation = value;

                if (value != null)
                {
                    value.FileChanged += this.HandleSyncLocationFileChanged;
                }

                this.RaisePropertyChanged("ListSyncLocationDisplay");
                this.RaisePropertyChanged("ListSyncTimeDisplay");
            }
        }

        private void HandleSyncLocationFileChanged(object sender, EventArgs e)
        {
            InternalGlobals.CurrentProfile.Sync();
        }

        // INotifyPropertyChanged implemented
        public string ListSyncLocationDisplay
        {
            get
            {
                SyncLocation location = this.SyncLocation;
                if (location != null)
                {
                    return String.Format(CultureInfo.CurrentCulture, Localization.UIResources.ListLocationFormat, location.Path);
                }

                return String.Empty;
            }
        }

        public string ListSyncTimeDisplay
        {
            get
            {
                SyncLocation location = this.SyncLocation;
                if (location != null)
                {
                    DateTime? lastUpdateCheckTime = this.LastUpdateCheckTimestamp;
                    if (lastUpdateCheckTime != null)
                    {
                        return String.Format(
                            CultureInfo.CurrentCulture, 
                            Localization.UIResources.ListSynchronizeTimestampFormat,
                            lastUpdateCheckTime.Value.ToLocalTime().ToString(Localization.UIResources.ListSynchronizeTimestampToStringFormat, CultureInfo.CurrentCulture));
                    }
                }
                    
                return String.Empty;
            }
        }

        public DateTime? LastUpdateTimestamp
        {
            get 
            { 
                return this.lastUpdateTimestamp; 
            }

            set 
            { 
                this.lastUpdateTimestamp = value;
            }
        }
        
        public DateTime? LastUpdateCheckTimestamp
        {
            get 
            { 
                return this.lastUpdateCheckTimestamp; 
            }

            set 
            {
                this.lastUpdateCheckTimestamp = value;
                this.OnLastUpdateCheckTimestampChanged(EventArgs.Empty);
                this.RaisePropertyChanged("ListSyncTimeDisplay");
            }
        }

        public string FolderName
        {
            get { return this.directory.Name; }
        }

        public ListSyncInfo SyncInfo
        {
            get { return this.syncInfo; }
        }

        public FileSystemDirectory Directory
        {
            get { return this.directory; }
        }

        public AssemblyReferencesManifest AssemblyReferencesManifest
        {
            get { return this.assemblyReferencesManifest; }
        }

        public string Name
        {
            get 
            { 
                return this.name; 
            }

            set
            {
                this.name = value;
                this.RaisePropertyChanged("Name");
                this.SaveConfig();
            }
        }

        public CommandCollectionWrapper Commands
        {
            get { return this.commands; }
        }

        //public IdGenerator CommandIdGenerator
        //{
        //    get { return this.commandIdGenerator; }
        //}

        public FunctionCollectionWrapper Functions
        {
            get { return this.functions; }
        }

        public ValueListCollectionWrapper ValueLists
        {
            get { return this.valueLists; }
        }

        //public IdGenerator FunctionIdGenerator
        //{
        //    get { return this.functionIdGenerator; }
        //}

        public AssemblyReferenceCollectionWrapper AssemblyReferences
        {
            get { return this.assemblyReferences; }
        }

        //public IdGenerator AssemblyReferenceIdGenerator
        //{
        //    get { return this.assemblyReferenceIdGenerator; }
        //}
#if DEBUG
        public static List FromPdcDecompile(Stream stream, out FileFileDirectory assemblyDirectory, out FileFileDirectory packageRoot)
        {
            ParameterlessVoid syncCallback = new ParameterlessVoid(delegate { });
            stream.Position = 0;

            DateTime timestamp = FileFileDirectory.ReadContainerTimestamp(stream);
            packageRoot = FileFileDirectory.FromContainer(stream);

            if (packageRoot.Files.Contains("meta"))
            {
                try
                {
                    FileFile metaFile = packageRoot.Files["meta"];
                    PdcMeta meta = PdcMeta.FromStream(metaFile.Contents);

                    if (meta.Revision > currentPackageVersion)
                    {
                        throw new NewerPackageVersionException("Cannot read the package because it was made with a later version of Promptu which is incompatible with the currently running version.  Please update your version of Promptu.");
                    }
                }
                catch (XmlException)
                {
                }
            }

            if (!packageRoot.Directories.Contains("Assemblies"))
            {
                throw new CorruptPackageException("The package appears to be corrupt.");
            }

            assemblyDirectory = packageRoot.Directories["Assemblies"];

            if (!packageRoot.Files.Contains("List.xml"))
            {
                throw new CorruptPackageException("The package does not contain List.xml.");
            }
            else if (!packageRoot.Files.Contains("Commands.xml"))
            {
                throw new CorruptPackageException("The package does not contain Commands.xml.");
            }
            else if (!packageRoot.Files.Contains("Functions.xml"))
            {
                throw new CorruptPackageException("The package does not contain Functions.xml.");
            }
            else if (!packageRoot.Files.Contains("AssemblyReferences.xml"))
            {
                throw new CorruptPackageException("The package does not contain AssemblyReferences.xml.");
            }
            else if (!packageRoot.Files.Contains("ValueLists.xml"))
            {
                throw new CorruptPackageException("The package does not contain ValueLists.xml.");
            }

            AssemblyReferenceCollectionWrapper assemblyReferences = AssemblyReferenceCollectionWrapper.FromFile(
                string.Empty,
                packageRoot.Files["AssemblyReferences.xml"].Contents,
                false,
                syncCallback);


            //AssemblyReferencesManifest manifest = baseList.AssemblyReferencesManifest.Clone();

            //List<string> alreadyUnloadedAssemblies = new List<string>();

            //foreach (AssemblyReference reference in assemblyReferences)
            //{
            //    if (assemblyDirectory.Files.Contains(reference.CachedName) && !alreadyUnloadedAssemblies.Contains(reference.CachedName))
            //    {
            //        reference.CachedName = PromptuSettings.AssemblyCache.InstallAssembly(
            //            reference.Filename,
            //            assemblyDirectory.Files[reference.CachedName].Contents);
            //        alreadyUnloadedAssemblies.Add(reference.CachedName);
            //    }

            //    if (!baseList.assemblyReferencesManifest.Contains(reference.Name))
            //    {
            //        reference.OwnedByUser = false;
            //    }
            //    else
            //    {
            //        bool removeFromManifest = false;
            //        if (baseList.assemblyReferences.Contains(reference.Name))
            //        {
            //            removeFromManifest = baseList.assemblyReferences[reference.Name].Filepath != reference.Filepath;
            //        }
            //        else
            //        {
            //            removeFromManifest = true;
            //        }

            //        if (removeFromManifest)
            //        {
            //            manifest.Remove(reference.Name);
            //            reference.OwnedByUser = true;
            //        }
            //        else
            //        {
            //            reference.OwnedByUser = true;
            //        }
            //    }
            //}

            CommandCollectionWrapper commands = CommandCollectionWrapper.FromFile(string.Empty, packageRoot.Files["Commands.xml"].Contents);
            FunctionCollectionWrapper functions = FunctionCollectionWrapper.FromFile(string.Empty, packageRoot.Files["Functions.xml"].Contents);

            ValueListCollectionWrapper valueLists = ValueListCollectionWrapper.FromFile(string.Empty, packageRoot.Files["ValueLists.xml"].Contents);

            XmlDocument listConfigDocument = new XmlDocument();
            StreamReader reader = new StreamReader(packageRoot.Files["List.xml"].Contents);
            listConfigDocument.LoadXml(reader.ReadToEnd());

            string name = null;

            foreach (XmlNode root in listConfigDocument.ChildNodes)
            {
                switch (root.Name.ToUpperInvariant())
                {
                    case "LIST":
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

                        break;
                    default:
                        break;
                }
            }

            if (name == null)
            {
                throw new LoadException("'List.xml' is missing the 'name' attribute under the 'List' root node.");
            }

            List list = new List(string.Empty, name, commands, functions, assemblyReferences, new AssemblyReferencesManifest(String.Empty), valueLists);
            list.isOwnedByUser = false;
            list.LastUpdateTimestamp = timestamp;
            return list;
        }
#endif

        public static List FromPdc(Stream stream, List baseList, Profile associatedProfile, out FileFileDirectory assemblyDirectory, out FileFileDirectory packageRoot, ParameterlessVoid syncCallback)
        {
            if (associatedProfile == null)
            {
                throw new ArgumentNullException("associatedProfile");
            }

            stream.Position = 0;

            DateTime timestamp = FileFileDirectory.ReadContainerTimestamp(stream);
            packageRoot = FileFileDirectory.FromContainer(stream);

            if (packageRoot.Files.Contains("meta"))
            {
                try
                {
                    FileFile metaFile = packageRoot.Files["meta"];
                    PdcMeta meta = PdcMeta.FromStream(metaFile.Contents);

                    if (meta.Revision > currentPackageVersion)
                    {
                        throw new NewerPackageVersionException("Cannot read the package because it was made with a later version of Promptu which is incompatible with the currently running version.  Please update your version of Promptu.");
                    }
                }
                catch (XmlException)
                {
                }
            }

            if (!packageRoot.Directories.Contains("Assemblies"))
            {
                throw new CorruptPackageException("The package appears to be corrupt.");
            }

            assemblyDirectory = packageRoot.Directories["Assemblies"];

            if (!packageRoot.Files.Contains("List.xml"))
            {
                throw new CorruptPackageException("The package does not contain List.xml.");
            }
            else if (!packageRoot.Files.Contains("Commands.xml"))
            {
                throw new CorruptPackageException("The package does not contain Commands.xml.");
            }
            else if (!packageRoot.Files.Contains("Functions.xml"))
            {
                throw new CorruptPackageException("The package does not contain Functions.xml.");
            }
            else if (!packageRoot.Files.Contains("AssemblyReferences.xml"))
            {
                throw new CorruptPackageException("The package does not contain AssemblyReferences.xml.");
            }
            else if (!packageRoot.Files.Contains("ValueLists.xml"))
            {
                throw new CorruptPackageException("The package does not contain ValueLists.xml.");
            }

            AssemblyReferenceCollectionWrapper assemblyReferences = AssemblyReferenceCollectionWrapper.FromFile(
                baseList.assemblyReferences.Filepath,
                packageRoot.Files["AssemblyReferences.xml"].Contents,
                false,
                syncCallback);


            AssemblyReferencesManifest manifest = baseList.AssemblyReferencesManifest.Clone();

            List<string> alreadyUnloadedAssemblies = new List<string>();

            foreach (AssemblyReference reference in assemblyReferences)
            {
                if (assemblyDirectory.Files.Contains(reference.CachedName) && !alreadyUnloadedAssemblies.Contains(reference.CachedName))
                {
                    reference.CachedName = InternalGlobals.AssemblyCache.InstallAssembly(
                        reference.Filename,
                        assemblyDirectory.Files[reference.CachedName].Contents);
                    alreadyUnloadedAssemblies.Add(reference.CachedName);
                }

                if (!baseList.assemblyReferencesManifest.Contains(reference.Name))
                {
                    reference.OwnedByUser = false;
                }
                else
                {
                    bool removeFromManifest = false;
                    if (baseList.assemblyReferences.Contains(reference.Name))
                    {
                        removeFromManifest = baseList.assemblyReferences[reference.Name].Filepath != reference.Filepath;
                    }
                    else
                    {
                        removeFromManifest = true;
                    }

                    if (removeFromManifest)
                    {
                        manifest.Remove(reference.Name);
                        reference.OwnedByUser = true;
                    }
                    else
                    {
                        reference.OwnedByUser = true;
                    }
                }
            }

            CommandCollectionWrapper commands = CommandCollectionWrapper.FromFile(baseList.Commands.Filepath, packageRoot.Files["Commands.xml"].Contents);
            FunctionCollectionWrapper functions = FunctionCollectionWrapper.FromFile(baseList.functions.Filepath, packageRoot.Files["Functions.xml"].Contents);

            ValueListCollectionWrapper valueLists = ValueListCollectionWrapper.FromFile(baseList.valueLists.Filepath, packageRoot.Files["ValueLists.xml"].Contents);

            XmlDocument listConfigDocument = new XmlDocument();
            StreamReader reader = new StreamReader(packageRoot.Files["List.xml"].Contents);
            listConfigDocument.LoadXml(reader.ReadToEnd());

            string name = null;

            foreach (XmlNode root in listConfigDocument.ChildNodes)
            {
                switch (root.Name.ToUpperInvariant())
                {
                    case "LIST":
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

                        break;
                    default:
                        break;
                }
            }

            if (name == null)
            {
                throw new LoadException("'List.xml' is missing the 'name' attribute under the 'List' root node.");
            }

            List list = new List(baseList.Directory, name, commands, functions, assemblyReferences, manifest, valueLists, associatedProfile);
            list.isOwnedByUser = baseList.isOwnedByUser;
            list.LastUpdateTimestamp = timestamp;
            return list;
        }

        //public void BlockChildrenSave()
        //{
        //    this.AssemblyReferences.BlockSave = true;
        //    this.Commands.BlockSave = true;
        //    this.Functions.BlockSave = true;
        //    this.ValueLists.BlockSave = true;
        //}

        //public void UnblockChildrenSave(bool save)
        //{
        //    this.AssemblyReferences.BlockSave = false;
        //    this.Commands.BlockSave = false;
        //    this.Functions.BlockSave = false;
        //    this.ValueLists.BlockSave = false;

        //    if (save)
        //    {
        //        this.SaveAll();
        //    }
        //}

        internal void UpdateCachedIconsAsync()
        {
            if (!InternalGlobals.CurrentProfile.ShowCommandTargetIcons)
            {
                return;
            }

            BackgroundWorker worker = new BackgroundWorker();
            worker.DoWork += HandleUpdateCacheIconAsync;
            worker.RunWorkerAsync();
        }

        private void HandleUpdateCacheIconAsync(object sender, DoWorkEventArgs e)
        {
            this.UpdateAllCachedIcons(false);
            InternalGlobals.CurrentProfile.WebIconLastModifiedDictionary.Save();
        }

        private List<CompositeItem<Command, List>> GetAllCommands()
        {
            List<CompositeItem<Command, List>> commands = new List<CompositeItem<Command, List>>();

            using (DdMonitor.Lock(this.Commands))
            {
                foreach (Command command in this.Commands)
                {
                    commands.Add(new CompositeItem<Command, List>(command, this));
                }
            }

            return commands;
        }

        private void UpdateCachedIconsList(List<CompositeItem<Command, List>> commands, bool userInitiated)
        {
            PromptuWebClient webClient = Updater.GetFaviconWebClient();

            foreach (CompositeItem<Command, List> command in commands)
            {
                command.Item.UpdateCacheIcon(command.ListFrom, userInitiated, webClient);
            }
        }

        public void UpdateAllCachedIcons(bool userInitiated)
        {
            if (!InternalGlobals.CurrentProfile.ShowCommandTargetIcons)
            {
                return;
            }

            List<CompositeItem<Command, List>> allCommandsAndLists = this.GetAllCommands();
            this.UpdateCachedIconsList(allCommandsAndLists, userInitiated);
            InternalGlobals.CurrentProfile.WebIconLastModifiedDictionary.Save();
            //PromptHandler.GetInstance().NotifySuggestionAffectingChange();
        }

        public void SyncIfNecessary(ref List<DiffDiffBase> needToAskUserAbout, bool force)
        {
            if (this.syncLocation == null || !InternalGlobals.SyncAllowed)
            {
                return;
                //throw new NoSyncLocationException("The sync location is null.");
            }

            if (this.syncLocation.Exists)
            {
                this.syncLocation.HookUpFileChangedNotifications();
                bool sync = false;

                InternalGlobals.SyncSynchronizer.CancelSyncsAndWait();
                InternalGlobals.SyncSynchronizer.CancelSyncs = false;
                try
                {
                    using (Stream stream = this.syncLocation.GetReadStream())
                    //using (MemoryStream memoryStream = new MemoryStream())
                    {
                        if (this.syncInfo.SyncBase != null || this.lastUpdateTimestamp == null)
                        {
                            sync = true;
                        }
                        else
                        {
                            try
                            {
                                DateTime timestamp = FileFileDirectory.ReadContainerTimestamp(stream);
                                sync = timestamp != this.lastUpdateTimestamp;
                            }
                            catch (BadFileFormatException)
                            {
                                sync = true;
                            }
                        }

                        if (sync || force)
                        {
                            this.ResolveChanges(stream, ref needToAskUserAbout);
                        }
                        else
                        {
                            this.LastUpdateCheckTimestamp = DateTime.Now.ToUniversalTime();
                        }
                    }
                }
                catch (UnauthorizedAccessException)
                {
                }

                this.UpdateCachedIconsAsync();
            }
            else
            {
                this.PublishChanges(true);
            }
        }

        public List Clone(Profile newAssociatedProfile)
        {
            List clone = new List(
                this.directory,
                this.name,
                this.Commands.Clone(),
                this.functions.Clone(),
                this.assemblyReferences.Clone(),
                this.assemblyReferencesManifest.Clone(),
                this.valueLists.Clone(),
                newAssociatedProfile);

            clone.isOwnedByUser = this.isOwnedByUser;
            clone.LastUpdateTimestamp = this.LastUpdateTimestamp;
            clone.LastUpdateCheckTimestamp = this.LastUpdateCheckTimestamp;

            return clone;
        }

        private void PrepareForSync(object sender, EventArgs e)
        {
            this.PrepareForSync();
        }

        private void PrepareForSync()
        {
            if (!this.ignorePrepareForSync && this.syncLocation != null && this.syncInfo.SyncBase == null)
            {
                using (DdMonitor.Lock(this.syncInfo))
                {
                    this.OnSyncAffectingChangeOccuring(EventArgs.Empty);
                    this.syncInfo.SyncBase = this.Clone(this.associatedProfile);
                    using (FileStream stream = new FileStream(this.Directory + "SyncBase.pdc", FileMode.Create))
                    {
                        this.syncInfo.SyncBase.Export(stream, true);
                    }
                }
            }
        }

        public void PublishChangesAsync(bool blindOverwrite)
        {
            BackgroundWorker worker = new BackgroundWorker();
            worker.DoWork += delegate { this.PublishChanges(blindOverwrite); };
            worker.RunWorkerAsync();
            //Action<bool> action = 
            //Thread publishThread = new Thread(this.PublishChangesThreadStart);
            //publishThread.IsBackground = true;
            //publishThread.SetApartmentState(ApartmentState.STA);
            //publishThread.Start(blindOverwrite);
        }

        //private void PublishChangesThreadStart(object blindOverwrite)
        //{
        //    this.PublishChanges((bool)blindOverwrite);
        //}

        public void PublishChanges(bool blindOverwrite)
        {
            SyncLocation publishLocation = this.syncLocation;
            if (publishLocation == null || !InternalGlobals.SyncAllowed)
            {
                return;
                //throw new NoSyncLocationException("The sync location is null.");
            }

            //if (!this.syncLocation.Exists)
            //{
            //    if (this.syncLocation.CanCreate)
            //    {
            //        this.syncLocation.Create();
            //    }
            //}
            bool syncLocationExists = publishLocation.Exists;

            if (syncLocationExists || publishLocation.CanCreate)
            {
                this.publishSyncManager.WaitingActions++;
                using (DdMonitor.Lock(this.publishSyncManager.ActionSyncToken))
                {
                    this.publishSyncManager.WaitingActions--;

                    if (this.publishSyncManager.WaitingActions <= 0)
                    {
                        //System.Threading.Thread.Sleep(2000);
                        if (!blindOverwrite)
                        {
                            if (syncLocationExists)
                            {
                                bool resolveChanges = false;
                                //try
                                //{
                                try
                                {
                                    using (Stream stream = publishLocation.GetReadStream())
                                    {
                                        if (this.syncInfo.SyncBase != null || this.lastUpdateTimestamp == null)
                                        {
                                            resolveChanges = true;
                                        }
                                        else
                                        {
                                            try
                                            {
                                                DateTime timestamp = FileFileDirectory.ReadContainerTimestamp(stream);
                                                if (timestamp != this.lastUpdateTimestamp)
                                                {
                                                    resolveChanges = true;
                                                }
                                            }
                                            catch (BadFileFormatException)
                                            {
                                                resolveChanges = true;
                                            }
                                        }

                                        if (resolveChanges)
                                        {
                                            List<DiffDiffBase> needToAskUserAbout = new List<DiffDiffBase>();
                                            this.ResolveChanges(stream, ref needToAskUserAbout);

                                            if (needToAskUserAbout.Count > 0)
                                            {
                                                InternalGlobals.GuiManager.ToolkitHost.MainThreadDispatcher.Invoke(new ParameterlessVoid(delegate
                                                {
                                                    CollisionResolvingDialogPresenter dialog = new CollisionResolvingDialogPresenter(needToAskUserAbout);
                                                   // InternalGlobals.UISettings.CollisionResolvingDialogSettings.ImpartTo(dialog.NativeInterface);
                                                    dialog.ShowDialog(Skins.PromptHandler.GetDialogOwner());
                                                    //InternalGlobals.UISettings.CollisionResolvingDialogSettings.UpdateFrom(dialog.NativeInterface);
                                                }), null);

                                                this.SaveAll();
                                            }

                                            this.UpdateCachedIconsAsync();
                                        }
                                    }
                                }
                                catch (UnauthorizedAccessException)
                                {
                                }
                                // }
                            }
                        }

                        try
                        {
                            using (Stream stream = publishLocation.GetWriteStream())
                            {
                                this.LastUpdateTimestamp = this.Export(stream, false);
                                //rootDirectory.SaveInContainer(stream);
                                stream.Close();
                            }
                        }
                        catch (UnauthorizedAccessException)
                        {
                            return;
                        }

                        this.syncInfo.SyncBase = null;
                        string syncBasePdcFilename = this.directory + "SyncBase.pdc";
                        if (File.Exists(syncBasePdcFilename))
                        {
                            File.Delete(syncBasePdcFilename);
                        }

                        //this.SyncInfo.ClearIdentifierChanges();

                        //rootDirectory.Dispose();
                        this.LastUpdateCheckTimestamp = DateTime.Now.ToUniversalTime();
                    }
                }

                Skins.PromptHandler.GetInstance().SetupDialog.UpdateToCurrentListThreadSafe();
            }
        }

        public void ResolveChanges(Stream pdcStream, ref List<DiffDiffBase> needToAskUserAbout)
        {
            try
            {
                InternalGlobals.SyncSynchronizer.NotifySyncStarting();
                this.ignorePrepareForSync = true;
                bool saveThis = true;
                try
                {
                    FileFileDirectory assemblyDirectory;
                    FileFileDirectory packageRoot;
                    List cloudList = List.FromPdc(pdcStream, this, this.associatedProfile, out assemblyDirectory, out packageRoot, InternalGlobals.CurrentProfile.GetSyncCallback());

                    if (this.syncInfo.SyncBase != null)
                    {
                        List baseList = this.syncInfo.SyncBase;

                        using (DdMonitor.Lock(cloudList.AssemblyReferences))
                        {
                            foreach (AssemblyReference assemblyReference in cloudList.AssemblyReferences)
                            {
                                AssemblyReference baseReference = baseList.AssemblyReferences.TryGet(assemblyReference.Id);
                                if (baseReference != null)
                                {
                                    if (baseReference.OwnedByUser)
                                    {
                                        assemblyReference.OwnedByUser = baseReference.Name == assemblyReference.Name && baseReference.Filepath == assemblyReference.Filepath;
                                    }
                                }
                            }
                        }

                        this.name = cloudList.Name;

                        CommandChangeFinder commandChangeFinder = new CommandChangeFinder(
                            baseList.Commands,
                            this.Commands,
                            cloudList.Commands);

                        AssemblyReferenceChangeFinder assemblyReferenceChangeFinder = new AssemblyReferenceChangeFinder(
                            baseList.AssemblyReferences,
                            this.AssemblyReferences,
                            cloudList.AssemblyReferences);

                        FunctionChangeFinder functionChangeFinder = new FunctionChangeFinder(
                            baseList.Functions,
                            this.Functions,
                            cloudList.Functions);

                        ValueListChangeFinder valueListChangeFinder = new ValueListChangeFinder(
                            baseList.ValueLists,
                            this.ValueLists,
                            cloudList.ValueLists);

                        List<CommandDiffDiff> commandChanges = commandChangeFinder.GetResults();
                        List<AssemblyReferenceDiffDiff> assemblyReferenceChanges = assemblyReferenceChangeFinder.GetResults();
                        List<FunctionDiffDiff> functionChanges = functionChangeFinder.GetResults();
                        List<ValueListDiffDiff> valueListChanges = valueListChangeFinder.GetResults();

                        InternalGlobals.CurrentProfile.CompositeFunctionsAndCommandsMediator.AllowRegenerate = false;
                        InternalGlobals.CurrentProfile.History.AddChangeEventBlocker();

                        if (InternalGlobals.SyncSynchronizer.CancelSyncs)
                        {
                            return;
                        }

                        InternalGlobals.SyncSynchronizer.WaitIfPauseSyncs();

                        this.Commands.BlockSave = true;
                        needToAskUserAbout.AddRange<DiffDiffBase, CommandDiffDiff>(
                            new CommandAutoChangeResolver(this.Commands, this.Commands.IdGenerator, InternalGlobals.CurrentProfile.History, DiffVersion.Priority).ResolveNonConflictingChangesAndPrepareConflictingForUI(
                                commandChanges,
                                this.Commands.IdGenerator,
                                cloudList.Commands.IdGenerator,
                                false));
                        this.Commands.BlockSave = false;
                        this.Commands.Save(false);

                        if (InternalGlobals.SyncSynchronizer.CancelSyncs)
                        {
                            return;
                        }

                        InternalGlobals.SyncSynchronizer.WaitIfPauseSyncs();

                        this.AssemblyReferences.BlockSave = true;
                        needToAskUserAbout.AddRange<DiffDiffBase, AssemblyReferenceDiffDiff>(
                            new AssemblyReferenceAutoChangeResolver(this.AssemblyReferences, this.AssemblyReferences.IdGenerator, InternalGlobals.CurrentProfile.History, DiffVersion.Priority).ResolveNonConflictingChangesAndPrepareConflictingForUI(
                                assemblyReferenceChanges,
                                this.AssemblyReferences.IdGenerator,
                                cloudList.AssemblyReferences.IdGenerator,
                                false));

                        //List<string> alreadyUnloadedAssemblies = new List<string>();

                        //foreach (AssemblyReference reference in this.AssemblyReferences)
                        //{
                        //    //if (!assemblyDirectory.Files.Contains(reference.CachedName))
                        //    //{
                        //    //    //messages.Add(Locazlization.MessageFormats.PackageIsMissingAssembly, reference.CachedName);
                        //    //}
                        //    if (assemblyDirectory.Files.Contains(reference.CachedName) && !alreadyUnloadedAssemblies.Contains(reference.CachedName))
                        //    {
                        //        reference.CachedName = PromptuSettings.AssemblyCache.InstallAssembly(
                        //            reference.CachedName,
                        //            assemblyDirectory.Files[reference.CachedName].Contents);
                        //        alreadyUnloadedAssemblies.Add(reference.CachedName);
                        //    }

                        //    if (!this.assemblyReferencesManifest.Contains(reference.Name))
                        //    {
                        //        //reference.Filepath = null;
                        //        reference.OwnedByUser = false;
                        //    }
                        //    else
                        //    {
                        //        bool removeFromManifest = false;
                        //        if (this.assemblyReferences.Contains(reference.Name))
                        //        {
                        //            removeFromManifest = this.assemblyReferences[reference.Name].Filepath != reference.Filepath;
                        //        }
                        //        else
                        //        {
                        //            removeFromManifest = true;
                        //        }

                        //        if (removeFromManifest)
                        //        {
                        //            this.assemblyReferencesManifest.Remove(reference.Name);
                        //            //reference.Filepath = null;
                        //            reference.OwnedByUser = true;
                        //        }
                        //        else
                        //        {
                        //            reference.OwnedByUser = true;
                        //        }
                        //    }
                        //}

                        this.AssemblyReferences.BlockSave = false;
                        this.AssemblyReferences.Save(false);

                        if (InternalGlobals.SyncSynchronizer.CancelSyncs)
                        {
                            return;
                        }

                        InternalGlobals.SyncSynchronizer.WaitIfPauseSyncs();

                        this.Functions.BlockSave = true;
                        needToAskUserAbout.AddRange<DiffDiffBase, FunctionDiffDiff>(
                            new FunctionAutoChangeResolver(this.Functions, this.Functions.IdGenerator, InternalGlobals.CurrentProfile.History, DiffVersion.Priority).ResolveNonConflictingChangesAndPrepareConflictingForUI(
                                functionChanges,
                                this.Functions.IdGenerator,
                                cloudList.Functions.IdGenerator,
                                false));
                        this.Functions.BlockSave = false;
                        this.Functions.Save(false);

                        this.ValueLists.BlockSave = true;
                        needToAskUserAbout.AddRange<DiffDiffBase, ValueListDiffDiff>(
                            new ValueListAutoChangeResolver(this.ValueLists, this.ValueLists.IdGenerator, InternalGlobals.CurrentProfile.History, DiffVersion.Priority).ResolveNonConflictingChangesAndPrepareConflictingForUI(
                                valueListChanges,
                                this.ValueLists.IdGenerator,
                                cloudList.ValueLists.IdGenerator,
                                false));
                        this.ValueLists.BlockSave = false;
                        this.ValueLists.Save(false);

                        InternalGlobals.SyncSynchronizer.NotifyEssentiallyPaused();
                        InternalGlobals.CurrentProfile.History.RemoveChangeEventBlocker();
                        InternalGlobals.CurrentProfile.CompositeFunctionsAndCommandsMediator.AllowRegenerate = true;
                        InternalGlobals.CurrentProfile.CompositeFunctionsAndCommandsMediator.RegenerateAll();
                        InternalGlobals.SyncSynchronizer.UnNotifyEssentiallyPaused();

                        if (InternalGlobals.SyncSynchronizer.CancelSyncs)
                        {
                            return;
                        }
                    }
                    else
                    {
                        cloudList.SaveAll();

                        TrieList cloudCommands = cloudList.Commands.ConstructFindOptimizedStringCollection();
                        TrieList cloudFunctions = cloudList.functions.ConstructFindOptimizedStringCollection();

                        InternalGlobals.CurrentProfile.CompositeFunctionsAndCommandsMediator.AllowRegenerate = false;

                        InternalGlobals.CurrentProfile.History.AddChangeEventBlocker();

                        using (DdMonitor.Lock(this.Commands))
                        {
                            foreach (Command command in this.Commands)
                            {
                                command.RemoveEntriesFromHistoryNotPresentIn(cloudCommands, InternalGlobals.CurrentProfile.History);
                            }
                        }

                        using (DdMonitor.Lock(this.Functions))
                        {
                            foreach (Function function in this.functions)
                            {
                                function.RemoveEntriesFromHistoryNotPresentIn(cloudFunctions, InternalGlobals.CurrentProfile.History);
                            }
                        }

                        cloudList.Commands.IdGenerator.Align(this.Commands.IdGenerator);
                        cloudList.Functions.IdGenerator.Align(this.Functions.IdGenerator);
                        cloudList.AssemblyReferences.IdGenerator.Align(this.AssemblyReferences.IdGenerator);

                        InternalGlobals.CurrentProfile.History.RemoveChangeEventBlocker();

                        InternalGlobals.SyncSynchronizer.NotifyEssentiallyPaused();
                        InternalGlobals.CurrentProfile.CompositeFunctionsAndCommandsMediator.AllowRegenerate = true;
                        InternalGlobals.CurrentProfile.CompositeFunctionsAndCommandsMediator.RegenerateAll();
                        InternalGlobals.SyncSynchronizer.UnNotifyEssentiallyPaused();

                        //List<string> alreadyUnloadedAssemblies = new List<string>();

                        //foreach (AssemblyReference reference in cloudList.AssemblyReferences)
                        //{
                        //    //if (!assemblyDirectory.Files.Contains(reference.CachedName))
                        //    //{
                        //    //    //messages.Add(Locazlization.MessageFormats.PackageIsMissingAssembly, reference.CachedName);
                        //    //}
                        //    if (assemblyDirectory.Files.Contains(reference.CachedName) && !alreadyUnloadedAssemblies.Contains(reference.CachedName))
                        //    {
                        //        reference.CachedName = PromptuSettings.AssemblyCache.InstallAssembly(
                        //            reference.CachedName,
                        //            assemblyDirectory.Files[reference.CachedName].Contents);
                        //        alreadyUnloadedAssemblies.Add(reference.CachedName);
                        //    }

                        //    if (!this.assemblyReferencesManifest.Contains(reference.Name))
                        //    {
                        //        //reference.Filepath = null;
                        //        reference.OwnedByUser = false;
                        //    }
                        //    else
                        //    {
                        //        bool removeFromManifest = false;
                        //        if (this.assemblyReferences.Contains(reference.Name))
                        //        {
                        //            removeFromManifest = this.assemblyReferences[reference.Name].Filepath != reference.Filepath;
                        //        }
                        //        else
                        //        {
                        //            removeFromManifest = true;
                        //        }

                        //        if (removeFromManifest)
                        //        {
                        //            this.assemblyReferencesManifest.Remove(reference.Name);
                        //            //reference.Filepath = null;
                        //            reference.OwnedByUser = true;
                        //        }
                        //        else
                        //        {
                        //            reference.OwnedByUser = true;
                        //        }
                        //    }
                        //}

                        saveThis = false;
                        this.LastUpdateTimestamp = cloudList.LastUpdateTimestamp;
                    }

                    //foreach (CommandDiffDiff changedCommand in changedCommands)
                    //{
                    //    if (changedCommand.HasConflictingChanges)
                    //    {
                    //        askUserAbout.Add(changedCommand);
                    //    }
                    //    else if (changedCommand.HasChanges)
                    //    {
                    //        CommandDiff diffWithChanges = changedCommand.GetChanged();
                    //        if (diffWithChanges != null)
                    //        {
                    //            if (diffWithChanges.Status == DiffStatus.Deleted)
                    //            {
                    //                this.commands.Remove(diffWithChanges.BaseItem.Name);
                    //            }
                    //        }
                    //    }
                    //}
                }
                catch (FileFileSystemException)
                {
                }
                catch (CorruptPackageException)
                {
                }

                this.syncInfo.SyncBase = null;
                string syncBasePdcFilename = this.directory + "SyncBase.pdc";
                if (File.Exists(syncBasePdcFilename))
                {
                    File.Delete(syncBasePdcFilename);
                }

                if (saveThis)
                {
                    pdcStream.Close();
                    this.SaveAll(true, true);
                }

                InternalGlobals.SyncSynchronizer.WaitIfPauseSyncs();
                this.Reload(true);
            }
            finally
            {
                this.ignorePrepareForSync = false;
                InternalGlobals.SyncSynchronizer.NotifySyncEnded();
                this.LastUpdateCheckTimestamp = DateTime.Now.ToUniversalTime();
            }
        }

        //public void SyncLocalAssemblyReferences()
        //{
        //    foreach (AssemblyReference reference in this.assemblyReferences.ToArray())
        //    {
        //        reference.SyncIfNessesary();
        //    }
        //}

        public DateTime Export(Stream streamTo, bool includeAssemblyReferencesManifest)
        {
            using (DdMonitor.Lock(this.exportToken))
            {
                FileFileDirectory rootDirectory = new FileFileDirectory(this.name);
                FileFileDirectory assemblyDirectory = new FileFileDirectory("Assemblies");
                rootDirectory.Directories.Add(assemblyDirectory);

                FileSystemFile listFile = this.directory + "List.xml";
                FileSystemFile commandsFile = this.directory + "Commands.xml";
                FileSystemFile assemblyReferencesFile = this.directory + "AssemblyReferences.xml";
                FileSystemFile functionsFile = this.directory + "Functions.xml";
                FileSystemFile valueListsFile = this.directory + "ValueLists.xml";
                FileSystemFile assemblyReferencesManifestFile = this.directory + "AssemblyReferencesManifest.xml";

                if (!listFile.Exists
                    || !commandsFile.Exists
                    || !assemblyReferencesFile.Exists
                    || !functionsFile.Exists
                    || !valueListsFile.Exists
                    || (includeAssemblyReferencesManifest && !assemblyReferencesManifestFile.Exists))
                {
                    this.SaveAll(false, false);
                }

                rootDirectory.Files.Add(new FileFile("List.xml", new FileStream(listFile, FileMode.Open, FileAccess.Read)));
                rootDirectory.Files.Add(new FileFile("Commands.xml", new FileStream(commandsFile, FileMode.Open, FileAccess.Read)));
                rootDirectory.Files.Add(new FileFile("AssemblyReferences.xml", new FileStream(assemblyReferencesFile, FileMode.Open, FileAccess.Read)));
                rootDirectory.Files.Add(new FileFile("Functions.xml", new FileStream(functionsFile, FileMode.Open, FileAccess.Read)));
                rootDirectory.Files.Add(new FileFile("ValueLists.xml", new FileStream(valueListsFile, FileMode.Open, FileAccess.Read)));

                MemoryStream metaStream = new MemoryStream();
                StreamWriter writer = new StreamWriter(metaStream);
                writer.Write(Localization.Promptu.ListMeta);
                writer.Flush();

                rootDirectory.Files.Add(new FileFile("meta", metaStream));
                //rootDirectory.Files.Add(new FileFile("meta", new FileStream(@"C:\Users\Admin\Desktop\latermetatest.txt", FileMode.Open)));

                if (includeAssemblyReferencesManifest)
                {
                    rootDirectory.Files.Add(new FileFile("AssemblyReferencesManifest.xml", new FileStream(assemblyReferencesManifestFile, FileMode.Open, FileAccess.Read)));
                }

                using (DdMonitor.Lock(this.AssemblyReferences))
                {
                    foreach (AssemblyReference reference in this.AssemblyReferences)
                    {
                        if (reference.CachedName != null)
                        {
                            if (InternalGlobals.AssemblyCache.Contains(reference.CachedName))
                            {
                                FileSystemFile cachedPath = InternalGlobals.AssemblyCache[reference.CachedName].File;
                                if (!assemblyDirectory.Files.Contains(reference.CachedName))
                                {
                                    if (!cachedPath.Exists)
                                    {
                                        reference.SyncIfNessesary();
                                    }

                                    //Stream contents;
                                    //LoadedAssembly loadedAssembly = PromptuSettings.LoadedAssemblies.TryGet(reference.CachedName);
                                    //if (loadedAssembly != null)
                                    //{
                                    //    contents = loadedAssembly.Bytes.ToMemoryStream();
                                    //}
                                    //else
                                    //{
                                    Stream contents = new FileStream(cachedPath, FileMode.Open);
                                    //}

                                    assemblyDirectory.Files.Add(new FileFile(reference.CachedName, contents));
                                }
                            }
                        }
                    }
                }

                DateTime timestamp = rootDirectory.SaveInContainer(streamTo);
                rootDirectory.Dispose();
                return timestamp;
            }
        }

        public void SaveAll()
        {
            this.SaveAll(true, false);
        }

        private void SaveAll(bool publish, bool blindPublish)
        {
            this.SaveConfig(false, blindPublish);
            this.Functions.Save(false);
            this.AssemblyReferences.Save(false);
            this.Commands.Save(false);
            this.assemblyReferencesManifest.Save();
            this.valueLists.Save(false);
            if (publish)
            {
                this.PublishChangesAsync(blindPublish);
            }
        }

        public void SaveConfig()
        {
            this.SaveConfig(true, false);
        }

        public static string GetFolderNameFromSubPath(FileSystemFile path)
        {
           // Uri uri = new Uri(path);
            return path.GetParentDirectory().Name;
        }

        private void SaveConfig(bool publishChanges, bool blindPublish)
        {
            XmlDocument document = new XmlDocument();
            
            XmlNode listNode = document.CreateElement("List");

            listNode.Attributes.Append(XmlUtilities.CreateAttribute("name", this.name, document));
            document.AppendChild(listNode);

            this.directory.CreateIfDoesNotExist();

            InternalGlobals.FailedToSaveFiles.Remove(this.FolderName, ListConfigId);

            string path = this.directory + "List.xml";
            try
            {
                document.Save(path);
            }
            catch (IOException)
            {
                InternalGlobals.FailedToSaveFiles.Add(
                    new FailedToSaveFile(this.FolderName, ListConfigId, path, new ResaveHandler(InternalGlobals.ResaveListItem)));
            }

            if (publishChanges)
            {
                this.PublishChangesAsync(blindPublish);
            }
        }

        private void Reload(bool fromSync)
        {
            if (fromSync)
            {
                InternalGlobals.SyncSynchronizer.WaitIfPauseSyncs();
            }

            try
            {
                InternalGlobals.CurrentProfile.CompositeFunctionsAndCommandsMediator.AllowRegenerate = false;

                if (fromSync)
                {
                    InternalGlobals.SyncSynchronizer.NotifyEssentiallyPaused();
                }

                InternalGlobals.CurrentProfile.CompositeFunctionsAndCommandsMediator.RegenerationFinished.WaitOneCallDeadlock(10000);

                if (fromSync)
                {
                    InternalGlobals.SyncSynchronizer.UnNotifyEssentiallyPaused();
                }

                List list = List.FromFolder(this.directory, this.associatedProfile);
                this.AssemblyReferences.Clear();
                this.AssemblyReferences.AddRange(list.assemblyReferences);

                if (fromSync)
                {
                    InternalGlobals.SyncSynchronizer.WaitIfPauseSyncs();
                }

                using (DdMonitor.Lock(this.AssemblyReferences))
                {
                    foreach (AssemblyReference reference in this.AssemblyReferences)
                    {
                        reference.OwnedByUser = this.assemblyReferencesManifest.Contains(reference.Name);
                    }
                }

                if (fromSync)
                {
                    InternalGlobals.SyncSynchronizer.WaitIfPauseSyncs();
                }

                this.Commands.Clear();
                this.Commands.AddRange(list.Commands);

                if (fromSync)
                {
                    InternalGlobals.SyncSynchronizer.WaitIfPauseSyncs();
                }

                this.Functions.Clear();
                this.Functions.AddRange(list.Functions);

                if (fromSync)
                {
                    InternalGlobals.SyncSynchronizer.WaitIfPauseSyncs();
                }

                this.ValueLists.Clear();
                this.ValueLists.AddRange(list.ValueLists);

                if (fromSync)
                {
                    InternalGlobals.SyncSynchronizer.WaitIfPauseSyncs();
                }

                this.name = list.name;
            }
            finally
            {
                if (fromSync)
                {
                    InternalGlobals.SyncSynchronizer.NotifyEssentiallyPaused();
                }

                InternalGlobals.CurrentProfile.CompositeFunctionsAndCommandsMediator.AllowRegenerate = true;
                InternalGlobals.CurrentProfile.CompositeFunctionsAndCommandsMediator.RegenerateAll();

                if (fromSync)
                {
                    InternalGlobals.SyncSynchronizer.UnNotifyEssentiallyPaused();
                }
            }
        }

        public static List FromFolder(FileSystemDirectory folder, Profile associatedProfile)
        {
            if (associatedProfile == null)
            {
                throw new ArgumentNullException("assiociatedProfile");
            }

            string commandsFilename = folder + "Commands.xml";
            string assemblyReferencesFilename = folder + "AssemblyReferences.xml";
            string functionsFilename = folder + "Functions.xml";
            string listConfigFilename = folder + "List.xml";
            string assemblyReferencesManifestFilename = folder + "AssemblyReferencesManifest.xml";
            string syncBasePdcFilename = folder + "SyncBase.pdc";
            string valueListsFilename = folder + "ValueLists.xml";

            CommandCollectionWrapper commands = CommandCollectionWrapper.FromFile(commandsFilename, true);
            AssemblyReferenceCollectionWrapper assemblyReferences = AssemblyReferenceCollectionWrapper.FromFile(assemblyReferencesFilename, true, associatedProfile.GetSyncCallback());
            assemblyReferences.LoadAllLoadExportedFunctions();

            FunctionCollectionWrapper functions = FunctionCollectionWrapper.FromFile(functionsFilename, true);
            AssemblyReferencesManifest assemblyReferencesManifest = AssemblyReferencesManifest.FromFile(assemblyReferencesManifestFilename, true);
            ValueListCollectionWrapper valueLists = ValueListCollectionWrapper.FromFile(valueListsFilename, true);

            List<string> foundOwnedNames = new List<string>();
            
            foreach (AssemblyReference reference in assemblyReferences)
            {
                reference.OwnedByUser = assemblyReferencesManifest.Contains(reference.Name);
                if (reference.OwnedByUser)
                {
                    foundOwnedNames.Add(reference.Name);
                }
            }

            if (foundOwnedNames.Count != assemblyReferencesManifest.Count)
            {
                List<string> removeNames = new List<string>();
                foreach (string assemblyReferenceName in assemblyReferencesManifest)
                {
                    if (!foundOwnedNames.Contains(assemblyReferenceName))
                    {
                        removeNames.Add(assemblyReferenceName);
                    }
                }

                foreach (string nameToRemove in removeNames)
                {
                    assemblyReferencesManifest.Remove(nameToRemove);
                }

                assemblyReferencesManifest.Save();
            }

            XmlDocument listConfigDocument = new XmlDocument();

            try
            {
                listConfigDocument.LoadXml(File.ReadAllText(listConfigFilename));
            }
            catch (XmlException ex)
            {
                ex.Data.Add(InternalGlobals.ExceptionPathToken, listConfigFilename);
                throw;
            }
            catch (IOException ex)
            {
                ex.Data.Add(InternalGlobals.ExceptionPathToken, listConfigFilename);
                throw;
            }

            string name = null;

            foreach (XmlNode root in listConfigDocument.ChildNodes)
            {
                switch (root.Name.ToUpperInvariant())
                {
                    case "LIST":
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

                        break;
                    default:
                        break;
                }
            }

            if (name == null)
            {
                throw new LoadException(CultureInfo.CurrentCulture, "'{0}' is missing the 'name' attribute under the 'List' root node.", listConfigFilename);
            }

            List list = new List(folder, name, commands, functions, assemblyReferences, assemblyReferencesManifest, valueLists, associatedProfile);
            if (File.Exists(syncBasePdcFilename))
            {
                using (FileStream stream = new FileStream(syncBasePdcFilename, FileMode.Open))
                {
                    try
                    {
                        FileFileDirectory assemblyDirectory;
                        FileFileDirectory packageRoot;
                        list.syncInfo.SyncBase = List.FromPdc(stream, list, associatedProfile, out assemblyDirectory, out packageRoot, associatedProfile.GetSyncCallback());

                        if (packageRoot.Files.Contains("AssemblyReferencesManifest.xml"))
                        {
                            InheritableList<string> manifest = new InheritableList<string>();
                            XmlDocument document = new XmlDocument();
                            document.Load(packageRoot.Files["AssemblyReferencesManifest.xml"].Contents);

                            AssemblyReferencesManifest.LoadFrom(document, manifest);

                            using (DdMonitor.Lock(list.syncInfo.SyncBase.AssemblyReferences))
                            {
                                foreach (AssemblyReference reference in list.syncInfo.SyncBase.AssemblyReferences)
                                {
                                    reference.OwnedByUser = manifest.Contains(reference.Name);
                                }
                            }
                        }
                    }
                    catch (FileFileSystemException)
                    {
                    }
                    catch (CorruptPackageException)
                    {
                    }
                    catch (NewerPackageVersionException)
                    {
                    }
                    catch (XmlException)
                    {
                    }
                }
            }

            return list;
        }

        private void ChildrenSaved(object sender, EventArgs e)
        {
            if (this.publishOnChildrenSaved)
            {
                this.PublishChangesAsync(false);
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

        protected virtual void OnLastUpdateCheckTimestampChanged(EventArgs e)
        {
            EventHandler handler = this.LastUpdateCheckTimestampChanged;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        protected virtual void OnEnabledChanged(EventArgs e)
        {
            EventHandler handler = this.EnabledChanged;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        protected void RaisePropertyChanged(string name)
        {
            this.OnPropertyChanged(new PropertyChangedEventArgs(name));
        }

        protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            PropertyChangedEventHandler handler = this.PropertyChanged;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        public override string ToString()
        {
            return this.Name;
        }
    }
}
