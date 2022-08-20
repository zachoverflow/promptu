using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.IO;
using System.Globalization;
using System.Xml;
using ZachJohnson.Promptu.UserModel.Differencing;
using System.IO.Extensions;
using ZachJohnson.Promptu.AssemblyCaching;
using ZachJohnson.Promptu.UIModel;
using System.ComponentModel;
using ZachJohnson.Promptu.UserModel.Collections;

namespace ZachJohnson.Promptu.UserModel
{
    internal class AssemblyReference : IDiffable, INotifyPropertyChanged
    {
        internal const string XmlAlias = "assemblyreference";
        private FileSystemFile filepath;
        private string name;
        private Assembly assembly;
        private string cachedName;
        private DateTime? lastUpdatedTimestamp;
        private bool ownedByUser = true;
        private bool orphaned;
        private Id id;
        private ParameterlessVoid syncCallback;
        private List<int> idsSyncingOn = new List<int>(1);
        //private Version version;

        //public AssemblyReference(string name, FileSystemFile filepath)
        //    : this(name, filepath, null)
        //{
        //}

        public AssemblyReference(string name, FileSystemFile filepath, string cachedName, DateTime? lastUpdatedTimestamp, Id id, ParameterlessVoid syncCallback, bool orphaned)
        {
            this.filepath = filepath.GetAbsolutePath();
            this.name = name;
            this.cachedName = cachedName;
            this.lastUpdatedTimestamp = lastUpdatedTimestamp;
            this.id = id;
            this.syncCallback = syncCallback;
            this.orphaned = orphaned;

            //if (version == null)
            //{
            //    //try
            //    //{
            //    AssemblyName assemblyName = AssemblyName.GetAssemblyName(filepath);
            //    this.version = assemblyName.Version;
            //    //}
            //    //catch (BadImageFormatException)
            //    //{
            //    //    throw 
            //    //}
            //    //catch (ArgumentException)
            //    //{
            //    //}
            //    //catch (FileLoadException)
            //    //{
            //    //}
            //}
            //else
            //{
                //this.version = version;
            //}
        }

        public event EventHandler CachedNameChanged;

        public FileSystemFile Filepath
        {
            get { return this.filepath; }
            internal set { this.filepath = value; }
        }

        public Id Id
        {
            get { return this.id; }
            set { this.id = value; }
        }

        public string Filename
        {
            get 
            {
                return this.filepath.Name; 
            }
        }

        public string Name
        {
            get { return this.name; }
        }

        public override string ToString()
        {
            return this.Name;
        }

        public ParameterlessVoid SyncCallback
        {
            get { return this.syncCallback; }
            set { this.syncCallback = value; }
        }

        public AssemblyReference Clone(ParameterlessVoid syncCallback)
        {
            return this.Clone(null, syncCallback);
        }

        public AssemblyReference Clone(string renameName, ParameterlessVoid syncCallback)
        {
            AssemblyReference clone = new AssemblyReference(renameName == null ? this.name : renameName, this.filepath, this.cachedName, this.lastUpdatedTimestamp, this.id, syncCallback, this.orphaned);
            clone.ownedByUser = this.ownedByUser;
            return clone;
        }

        public ItemInfo GetItemInfo()
        {
            List<string> attributes = new List<string>();

            if (this.OwnedByUser)
            {
                //builder.AppendLine(this.Filepath.GetDiffEntry(diffVersion).RevisedValue);
                attributes.Add(this.Filepath);
            }
            else
            {
                //builder.AppendLine(Localization.UIResources.ExternallySharedReference);
                attributes.Add(Localization.UIResources.ExternallySharedReference);
            }

            return new ItemInfo(this.Name, attributes);
        }

        public bool OwnedByUser
        {
            get { return this.ownedByUser; }
            set { this.ownedByUser = value; }
        }

        public bool Orphaned
        {
            get { return this.orphaned; }
            set { this.orphaned = value; }
        }

        public DateTime? LastUpdatedTimestamp
        {
            get { return this.lastUpdatedTimestamp; }
            //get { return this.lastUpdatedTimestamp; }
        }

        public string CachedName
        {
            get 
            { 
                return this.cachedName; 
            }

            set
            {
                this.cachedName = value;
                this.OnCachedNameChanged(EventArgs.Empty);
            }
        }

        //public Version Version
        //{
        //    get
        //    {
        //        if (this.version == null)
        //        {
        //            //try
        //            //{
        //            AssemblyName assemblyName = AssemblyName.GetAssemblyName(filepath);
        //            this.version = assemblyName.Version;
        //            //}
        //            //catch (BadImageFormatException)
        //            //{
        //            //    throw 
        //            //}
        //            //catch (ArgumentException)
        //            //{
        //            //}
        //            //catch (FileLoadException)
        //            //{
        //            //}
        //        }

        //        return this.version;
        //    }
        //}

        public string GetFormattedIdentifier()
        {
            return this.Name;
        }

        public FunctionCollection LoadExportedFunctions()
        {
            FunctionCollection found = new FunctionCollection();
            Assembly assembly = this.GetAssembly(true);
            foreach (Type type in assembly.GetTypes())
            {
                foreach (MethodInfo method in type.GetMethods())
                {
                    string name = method.Name;
                    string description;
                    bool load = false;

                    foreach (Attribute attribute in Attribute.GetCustomAttributes(method))
                    {
                        DisplayNameAttribute nameAttribute = attribute as DisplayNameAttribute;
                        BindableAttribute bindableAttribute;
                        DescriptionAttribute descriptionAttribute;

                        if (nameAttribute != null)
                        {
                            if (Function.IsValidName(nameAttribute.DisplayName))
                            {
                                name = nameAttribute.DisplayName;
                            }
                        }
                        else if ((bindableAttribute = attribute as BindableAttribute) != null)
                        {
                            load = bindableAttribute.Bindable;
                        }
                        else if ((descriptionAttribute = attribute as DescriptionAttribute) != null)
                        {
                            description = descriptionAttribute.Description;
                        }
                    }

                    if (load)
                    {
                        ReturnValue returnValue;
                        if (method.ReturnType == typeof(string))
                        {
                            returnValue = ReturnValue.String;
                        }
                        else if (method.ReturnType == typeof(string[]))
                        {
                            returnValue = ReturnValue.StringArray;
                        }
                        else if (method.ReturnType == typeof(ValueList))
                        {
                            returnValue = ReturnValue.ValueList;
                        }
                        else
                        {
                            continue;
                        }

                        FunctionParameterCollection parameters = new FunctionParameterCollection();

                        bool invalid = false;

                        foreach (ParameterInfo parameter in method.GetParameters())
                        {
                            if (parameter.ParameterType != typeof(string))
                            {
                                invalid = true;
                                break;
                            }

                            parameters.Add(new FunctionParameter(
                                FunctionParameterValueType.String,
                                null,
                                false));
                        }

                        if (invalid)
                        {
                            continue;
                        }

                        found.Add(new Function(
                            name,
                            type.Name,
                            method.Name,
                            this.Name,
                            returnValue,
                            parameters,
                            null,
                            false));
                    }
                }
            }

            return found;
        }

        public void SyncIfNessesary()
        {
            if (this.ownedByUser && this.filepath.Exists)
            {
                CachedAssembly cachedAssembly;
                DateTime lastEditedTimestamp = this.filepath.GetLastEditedTimestampUtc();
                if (this.lastUpdatedTimestamp == null || lastEditedTimestamp > this.lastUpdatedTimestamp)
                {
                    //this.UninstallCachedAssemblyIfOnlyOneReference();
                    this.InstallAssemblyInCache();
                }
                else if ((cachedAssembly = InternalGlobals.AssemblyCache.TryGet(this.cachedName)) != null && !cachedAssembly.File.Exists)
                {
                    this.InstallAssemblyInCache();
                }
            }
        }

        public void UninstallCachedAssemblyIfNoReferences()
        {
            if (this.cachedName != null)
            {
                Dictionary<string, int> references = InternalGlobals.ProfilePlacemarks.GetAllCachedAssemblyNamesReferences();
                if (!references.ContainsKey(this.cachedName.ToUpperInvariant()))
                {
                    InternalGlobals.AssemblyCache.UninstallAssembly(this.cachedName);
                }

                this.cachedName = null;
            }
        }

        public void UninstallCachedAssemblyIfOnlyOneReference()
        {
            if (this.cachedName != null)
            {
                Dictionary<string, int> references = InternalGlobals.ProfilePlacemarks.GetAllCachedAssemblyNamesReferences();
                if (references[this.cachedName.ToUpperInvariant()] == 1)
                {
                    InternalGlobals.AssemblyCache.UninstallAssembly(this.cachedName);
                }

                this.cachedName = null;
            }
        }

        public void InstallAssemblyInCache()
        {
            this.InstallAssemblyInCache(true);
        }

        public void InstallAssemblyInCache(bool publishChanges)
        {
            //if (this.Filepath != null)
            //{
            if (!this.orphaned)
            {
                if (this.ownedByUser)
                {
                    this.CachedName = InternalGlobals.AssemblyCache.InstallAssembly(this.filepath);
                    this.lastUpdatedTimestamp = this.filepath.GetLastEditedTimestampUtc();
                }
                else
                {
                    if (this.syncCallback != null)
                    {
                        this.syncCallback.Invoke();
                    }
                }
            }
            //}
        }

        public Assembly GetAssembly(bool needFast)
        {
            if (this.assembly == null)
            {
                //if (!File.Exists(this.Filepath))
                //{
                //    throw new FatalLoadException(String.Format(CultureInfo.CurrentCulture, "No assembly could be found at {0}.", this.Filepath));
                //}

                CachedAssembly cachedAssembly = InternalGlobals.AssemblyCache.TryGet(this.CachedName);

                if (this.cachedName == null || cachedAssembly == null || !cachedAssembly.File.Exists)
                {
                    if (!this.Orphaned)
                    {
                        if (this.OwnedByUser)
                        {
                            if (!File.Exists(this.Filepath))
                            {
                                throw new LoadException(String.Format(CultureInfo.CurrentCulture, "No assembly could be found at {0}.", this.Filepath));
                            }
                            else
                            {
                                this.InstallAssemblyInCache();
                                //this.CachedName = PromptuSettings.AssemblyCache.InstallAssembly(this.filepath);
                            }
                        }
                        else
                        {
                            if (!needFast)
                            {
                                this.syncCallback();
                            }
                            //    throw new FatalLoadException(String.Format(CultureInfo.CurrentCulture, "Missing assembly {0}.", this.Filename));
                        }
                    }

                    if (this.cachedName == null || !InternalGlobals.AssemblyCache.Contains(this.CachedName))
                    {
                        throw new LoadException(CultureInfo.CurrentCulture, "The assembly {0} could not be sucessfully loaded.", this.Filename);
                    }
                }

                LoadedAssembly loadedAssembly = InternalGlobals.LoadedAssemblies.TryGet(this.CachedName);
                if (loadedAssembly != null)
                {
                    this.assembly = loadedAssembly.Assembly;
                }
                else
                {
                    this.SyncIfNessesary();
                    Assembly assembly = null;
                    using (MemoryStream bytes = new MemoryStream())
                    {
                        bool loadException = false;
                        FileSystemFile file = InternalGlobals.AssemblyCache[this.cachedName].File;
                        try
                        {
                            using (FileStream stream = new FileStream(file, FileMode.Open))
                            {
                                stream.TransferTo(bytes);
                            }
                        }
                        catch (IOException)
                        {
                            loadException = true;
                        }

                        if (!loadException)
                        {
                            try
                            {
                                assembly = Assembly.Load(bytes.ToArray());
                            }
                            catch (BadImageFormatException ex)
                            {
                                throw new LoadException(ex.Message);
                            }
                        }

                        if (assembly == null)
                        {
                            throw new LoadException(
                                String.Format(
                                CultureInfo.CurrentCulture,
                                "The assembly at {0} could not be loaded. Cannot proceed.",
                                this.Filepath));
                        }

                        this.assembly = assembly;

                        InternalGlobals.LoadedAssemblies.Add(new LoadedAssembly(this.cachedName, this.assembly));
                    }
                }
            }

            return this.assembly;
        }

        public static Assembly LoadAssembly(FileSystemFile file)
        {
            Assembly assembly = null;
            using (MemoryStream bytes = new MemoryStream())
            {
                bool loadException = false;
                try
                {
                    using (FileStream stream = new FileStream(file, FileMode.Open))
                    {
                        stream.TransferTo(bytes);
                    }
                }
                catch (IOException)
                {
                    loadException = true;
                }

                if (!loadException)
                {
                    try
                    {
                        assembly = Assembly.Load(bytes.ToArray());
                    }
                    catch (BadImageFormatException ex)
                    {
                        throw new LoadException(ex.Message);
                    }
                }

                InternalGlobals.LoadedAssemblies.Add(new LoadedAssembly(file.Name, assembly));
                return assembly;
            }
        }

        internal XmlNode ToXml(XmlDocument document)
        {
            XmlNode node = document.CreateElement("AssemblyReference");

            if (this.id != null)
            {
                node.Attributes.Append(XmlUtilities.CreateAttribute("id", this.id.ToString(), document));
            }

            //if (this.filepath != null)
            //{


                node.Attributes.Append(XmlUtilities.CreateAttribute("path", this.filepath.GetRelativePath(), document));
            //}

            node.Attributes.Append(XmlUtilities.CreateAttribute("name", this.name, document));

            if (this.cachedName != null)
            {
                node.Attributes.Append(XmlUtilities.CreateAttribute("cachedAs", this.cachedName, document));
            }

            if (this.Orphaned)
            {
                node.Attributes.Append(XmlUtilities.CreateAttribute("orphaned", this.orphaned, document));
            }

            if (this.LastUpdatedTimestamp != null)
            {
                node.Attributes.Append(XmlUtilities.CreateAttribute("lastUpdated", this.LastUpdatedTimestamp.Value.ToBinary(), document));
            }

            return node;
        }

        internal static AssemblyReference FromXml(XmlNode node, ParameterlessVoid syncCallback)
        {
            if (node.Name.ToLowerInvariant() != XmlAlias)
            {
                throw new ArgumentException("The node is not named " + XmlAlias + ".");
            }
            //else if (idGenerator == null)
            //{
            //    throw new ArgumentNullException("idGenerator");
            //}

            string path = null;
            string name = null;
            string cachedName = null;
            long? lastUpdatedTimestampInBinary = null;
            bool orphaned = false;
            Id id = null;

            foreach (XmlAttribute attribute in node.Attributes)
            {
                switch (attribute.Name.ToUpperInvariant())
                {
                    case "ID":
                        try
                        {
                            id = new Id(attribute.Value);
                        }
                        catch (FormatException)
                        {
                        }
                        catch (OverflowException)
                        {
                        }

                        break;
                    case "PATH":
                        path = attribute.Value;
                        break;
                    case "NAME":
                        name = attribute.Value;
                        break;
                    case "CACHEDAS":
                        cachedName = attribute.Value;
                        break;
                    case "LASTUPDATED":
                        lastUpdatedTimestampInBinary = Utilities.TryParseInt64(attribute.Value, lastUpdatedTimestampInBinary);
                        //try
                        //{
                        //    lastUpdatedTimestampInBinary = Convert.ToInt64(attribute.Value);
                        //}
                        //catch (FormatException)
                        //{
                        //}
                        //catch (OverflowException)
                        //{
                        //}

                        break;
                    case "ORPHANED":
                        orphaned = Utilities.TryParseBoolean(attribute.Value, orphaned);
                        //try
                        //{
                        //    orphaned = Convert.ToBoolean(attribute.Value);
                        //}
                        //catch (FormatException)
                        //{
                        //}

                        break;
                    default:
                        break;
                }
            }

            if (path == null)
            {
                throw new LoadException("The assembly's path was not found in the file.");
            }
            if (name == null)
            {
                throw new LoadException("The assembly's reference name was not found.");
            }
            // 2010-07-28: removing check for cached name
            //else if (cachedName == null)
            //{
            //    throw new LoadException("The assembly's cached name was not found.");
            //}

            DateTime? lastUpdatedTimestamp;

            if (lastUpdatedTimestampInBinary != null)
            {
                lastUpdatedTimestamp = DateTime.FromBinary(lastUpdatedTimestampInBinary.Value);
            }
            else
            {
                lastUpdatedTimestamp = null;
            }

            //FileSystemFile? file;
            //if (path == null)
            //{
            //    file = null;
            //}
            //else
            //{
            //    file = path;
            //}
            //if (id == null)
            //{
            //    id = idGenerator.GenerateId();
            //}

            return new AssemblyReference(name, path, cachedName, lastUpdatedTimestamp, id, syncCallback, orphaned);
        }

        protected virtual void OnCachedNameChanged(EventArgs e)
        {
            EventHandler handler = this.CachedNameChanged;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
