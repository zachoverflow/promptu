// Copyright 2022 Zach Johnson
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

namespace ZachJohnson.Promptu.PluginModel
{
    using System;
    using System.ComponentModel;
    using System.Globalization;
    using System.Xml;
    using ZachJohnson.Promptu.Configuration;
    using ZachJohnson.Promptu.PluginModel.Internals;

    public class PromptuPlugin : INotifyPropertyChanged
    {
        private FileSystemDirectory folder;
        private string name;
        private string creator;
        private string creatorContact;
        private string id;
        private string description;
        private LazyExternalObject<PromptuPluginEntryPoint> entryPoint;
        private Version version;
        private ReleaseVersion promptuVersionCompiledAgainst;
        private bool enabled;
        private ReleaseVersion checkedCompatibleWith;
        private bool isCompatible;
        private bool isInstalled;
        private object cachedImage;
        private string updateUrl;

        internal PromptuPlugin(
            string name,
            string id,
            Version version,
            ReleaseVersion promptuVersionCompiledAgainst,
            string creator,
            string creatorContact,
            string description,
            LazyExternalObject<PromptuPluginEntryPoint> entryPoint,
            FileSystemDirectory folder,
            string updateUrl)
        {
            this.name = name;
            this.creator = creator;
            this.creatorContact = creatorContact;
            this.description = description;
            this.entryPoint = entryPoint;
            this.id = id;
            this.version = version;
            this.folder = folder;
            this.promptuVersionCompiledAgainst = promptuVersionCompiledAgainst;
            this.updateUrl = updateUrl;

            this.entryPoint.Loaded += this.HandleEntryPointLoaded;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        internal event EventHandler SettingOrPropertyChanged;

        public FileSystemDirectory Folder
        {
            get { return this.folder; }
        }

        public string UpdateUrl
        {
            get { return this.updateUrl; }
        }

        public Version Version
        {
            get { return this.version; }
        }

        public bool IsInstalled
        {
            get
            {
                return this.isInstalled;
            }

            set
            {
                if (this.isInstalled != value)
                {
                    this.isInstalled = value;
                    this.OnPropertyChanged(new PropertyChangedEventArgs("IsInstalled"));
                }
            }
        }

        public bool IsCompatible
        {
            get
            {
                return this.isCompatible;
            }
            
            internal set
            {
                if (this.isCompatible != value)
                {
                    this.isCompatible = value;
                    this.entryPoint.LoadIsEnabled = value;
                    this.OnPropertyChanged(new PropertyChangedEventArgs("IsCompatible"));
                }
            }
        }

        public string Id
        {
            get
            {
                return this.id;
            }
        }

        public bool HasLoaded
        {
            get { return this.entryPoint.HasLoaded; }
        }

        public PromptuPluginEntryPoint EntryPoint
        {
            get { return this.entryPoint.Instance; }
        }

        public string AssemblyPath
        {
            get { return this.entryPoint.AssemblyPath; }
        }

        public bool CanConfigure
        {
            get
            {
                return this.Enabled && this.entryPoint.HasLoaded && this.entryPoint.Instance.Options.Count > 0;
            }
        }

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
                    PromptuPluginEntryPoint entryPoint = this.entryPoint.Instance;

                    if (entryPoint != null)
                    {
                        this.enabled = value;
                        entryPoint.IsEnabled = value;
                        entryPoint.Hooks.Enabled = value;
                        entryPoint.Factory.SetForceHideAll(!value);

                        try
                        {
                            if (value)
                            {
                                entryPoint.OnLoad();
                            }
                            else
                            {
                                entryPoint.OnUnload();
                            }
                        }
                        catch (Exception ex)
                        {
                            ErrorConsole.WriteLine(this.id, "Unhandled exception in plugin.  Details logged in exceptions.log");
                            ExceptionLogger.LogException(ex, String.Format(CultureInfo.CurrentCulture, "unknown, \"{0}\" at fault", this.id));
                        }
                    }

                    this.OnPropertyChanged(new PropertyChangedEventArgs("Enabled"));
                    this.OnPropertyChanged(new PropertyChangedEventArgs("CanConfigure"));
                }
            }
        }

        public string Name
        {
            get { return this.name; }
        }

        public string Creator
        {
            get { return this.creator; }
        }

        public string CreatorContact
        {
            get { return this.creatorContact; }
        }

        public string Description
        {
            get { return this.description; }
        }

        internal string SettingPersistXml
        {
            get;
            set;
        }

        internal string PropertyPersistXml
        {
            get;
            set;
        }

        internal ReleaseVersion PromptuVersionCompiledAgainst
        {
            get { return this.promptuVersionCompiledAgainst; }
        }

        internal object CachedImage
        {
            get { return this.cachedImage; }
            set { this.cachedImage = value; }
        }

        internal ReleaseVersion CheckedCompatibleWith
        {
            get
            {
                return this.checkedCompatibleWith;
            }

            set
            {
                this.checkedCompatibleWith = value;
            }
        }

        public static PromptuPlugin FromFolder(FileSystemDirectory folder)
        {
            FileSystemFile pluginMainfest = ((FileSystemDirectory)folder) + "plugin.xml";

            foreach (FileSystemFile file in folder.GetFiles())
            {
                if (file.Extension.ToUpperInvariant() == ".DELETEME")
                {
                    try
                    {
                        file.DeleteIfExists();
                    }
                    catch (UnauthorizedAccessException)
                    {
                    }
                }
            }

            if (pluginMainfest.Exists)
            {
                XmlDocument document = new XmlDocument();

                try
                {
                    document.Load(pluginMainfest);
                }
                catch (XmlException e)
                {
                    ErrorConsole.WriteLineFormat(pluginMainfest, "XmlException while parsing \"plugin.xml\". Message: {0}", e.Message);
                    return null;
                }

                foreach (XmlNode root in document.ChildNodes)
                {
                    if (root.Name.ToUpperInvariant() == "PLUGIN")
                    {
                        bool belongsToPromptu = false;
                        string promptuVersionCompiledFor = null;

                        string id = null;
                        string name = null;
                        string description = null;
                        string version = null;
                        string creator = null;
                        string creatorContact = null;

                        string entryAssembly = null;
                        string entryClass = null;
                        string updateUrl = null;

                        foreach (XmlAttribute attribute in root.Attributes)
                        {
                            switch (attribute.Name.ToUpperInvariant())
                            {
                                case "ID":
                                    id = attribute.Value;
                                    break;
                                case "VERSION":
                                    version = attribute.Value;
                                    break;
                                case "UPDATEURL":
                                    updateUrl = attribute.Value;
                                    break;
                                default:
                                    break;
                            }
                        }

                        foreach (XmlNode node in root.ChildNodes)
                        {
                            switch (node.Name.ToUpperInvariant())
                            {
                                case "INFO":
                                    foreach (XmlNode infoNode in node.ChildNodes)
                                    {
                                        switch (infoNode.Name.ToUpperInvariant())
                                        {
                                            case "NAME":
                                                name = infoNode.InnerText;
                                                break;
                                            case "DESCRIPTION":
                                                description = infoNode.InnerText;
                                                break;
                                            case "CREATOR":
                                                creator = infoNode.InnerText;
                                                break;
                                            case "CREATORCONTACT":
                                                creatorContact = infoNode.InnerText;
                                                break;
                                            default:
                                                break;
                                        }
                                    }

                                    break;
                                case "TARGETAPP":
                                    foreach (XmlAttribute attribute in node.Attributes)
                                    {
                                        switch (attribute.Name.ToUpperInvariant())
                                        {
                                            case "ID":
                                                if (attribute.Value.ToUpperInvariant() == "HTTP://PROMPTULAUNCHER.COM/PROMPTU")
                                                {
                                                    belongsToPromptu = true;
                                                }

                                                break;
                                            case "VERSIONCOMPILEDFOR":
                                                promptuVersionCompiledFor = attribute.Value;
                                                break;
                                            default:
                                                break;
                                        }
                                    }

                                    break;
                                case "ENTRYPOINT":
                                    foreach (XmlAttribute attribute in node.Attributes)
                                    {
                                        switch (attribute.Name.ToUpperInvariant())
                                        {
                                            case "ASSEMBLY":
                                                entryAssembly = attribute.Value;
                                                break;
                                            case "CLASS":
                                                entryClass = attribute.Value;
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

                        if (id == null)
                        {
                            ErrorConsole.WriteLine(pluginMainfest, "Missing \"id\" attribute on the \"Plugin\" node.");
                            return null;
                        }
                        else if (!belongsToPromptu)
                        {
                            ErrorConsole.WriteLine(id, "Promptu (\"http://promptulauncher.com/promptu\") is not specified as this plugin's target app.  Plugin not loaded.");
                            return null;
                        }
                        else if (promptuVersionCompiledFor == null)
                        {
                            ErrorConsole.WriteLine(id, "Missing \"versionCompiledFor\" attribute on the \"Plugin/TargetApp\" node.");
                            return null;
                        }
                        else if (entryAssembly == null)
                        {
                            ErrorConsole.WriteLine(id, "Missing \"assembly\" attribute on the \"Plugin/EntryPoint\" node.");
                            return null;
                        }
                        else if (entryClass == null)
                        {
                            ErrorConsole.WriteLine(id, "Missing \"class\" attribute on the \"Plugin/EntryPoint\" node.");
                            return null;
                        }
                        else if (version == null)
                        {
                            ErrorConsole.WriteLine(id, "Missing \"version\" attribute on the \"Plugin\" node.");
                            return null;
                        }
                        else if (name == null)
                        {
                            ErrorConsole.WriteLine(id, "Missing \"Plugin/Info/Name\" node.");
                            return null;
                        }
                        else if (name.Length == 0)
                        {
                            ErrorConsole.WriteLine(id, "Empty \"Plugin/Info/Name\" node.");
                            return null;
                        }

                        ReleaseVersion promptuVersionCompiledAgainst;

                        try
                        {
                            promptuVersionCompiledAgainst = new ReleaseVersion(promptuVersionCompiledFor);
                        }
                        catch (FormatException)
                        {
                            ErrorConsole.WriteLine(id, "Invalid \"versionCompiledFor\" attribute on the \"Plugin/TargetApp\" node.  The version must be in the format: \"{number}.{number}.{number}.{number}\".");
                            return null;
                        }

                        ReleaseVersion releaseVersion;

                        try
                        {
                            releaseVersion = new ReleaseVersion(version);
                        }
                        catch (FormatException)
                        {
                            ErrorConsole.WriteLine(id, "Invalid \"version\" attribute on the \"Plugin\" node.  The version must be in the format: \"{number}.{number}.{number}.{number}\".");
                            return null;
                        }

                        FileSystemFile assemblyFile = folder + entryAssembly;

                        if (!assemblyFile.Exists)
                        {
                            ErrorConsole.WriteLineFormat(id, "Invalid \"assembly\" attribute on the \"Plugin/EntryPoint\" node.  \"{0}\" does not exist.", assemblyFile);
                            return null;
                        }

                        return new PromptuPlugin(
                            name,
                            id,
                            releaseVersion.ToVersion(),
                            promptuVersionCompiledAgainst,
                            creator,
                            creatorContact,
                            description,
                            new LazyExternalObject<PromptuPluginEntryPoint>(assemblyFile, entryClass, id),
                            folder,
                            updateUrl);
                    }
                }
            }

            return null;
        }

        protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            PropertyChangedEventHandler handler = this.PropertyChanged;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        private void HandleEntryPointLoaded(object sender, EventArgs e)
        {
            PromptuPluginEntryPoint entryPoint = this.entryPoint.Instance;

            if (entryPoint == null)
            {
                return;
            }

            string propertyPersistXml = this.PropertyPersistXml;
            string settingPersistXml = this.SettingPersistXml;

            XmlDocument tempDocument = new XmlDocument();

            if (propertyPersistXml != null)
            {
                tempDocument.LoadXml(propertyPersistXml);
                entryPoint.SavingProperties.LoadSettingsFrom(tempDocument.ChildNodes[0]);
            }

            if (settingPersistXml != null)
            {
                tempDocument.LoadXml(settingPersistXml);

                SettingsBase settings = entryPoint.SavingSettings;
                if (settings != null)
                {
                    settings.UpdateFrom(tempDocument.ChildNodes[0]);
                }
            }

            this.PropertyPersistXml = null;
            this.SettingPersistXml = null;

            entryPoint.SavingSettingsChanged += this.HandleSettingOrPropertyChanged;
            entryPoint.SavingProperties.PropertyChanged += this.HandleSettingOrPropertyChanged;
        }

        private void HandleSettingOrPropertyChanged(object sender, EventArgs e)
        {
            if (this.IsInstalled)
            {
                this.OnSettingOrPropertyChanged(EventArgs.Empty);
            }
        }

        private void OnSettingOrPropertyChanged(EventArgs e)
        {
            EventHandler handler = this.SettingOrPropertyChanged;
            if (handler != null)
            {
                handler(this, e);
            }
        }
    }
}
