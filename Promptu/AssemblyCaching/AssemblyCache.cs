using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Reflection;
using System.IO;
using System.IO.Extensions;
using ZachJohnson.Promptu.UserModel;
using System.Globalization;

namespace ZachJohnson.Promptu.AssemblyCaching
{
    internal class AssemblyCache : IEnumerable<CachedAssembly>
    {
        private FileSystemDirectory cacheDirectory;
        private CachedAssemblyCollection assemblies;
        private bool clearedUnused;

        public AssemblyCache(FileSystemDirectory cacheDirectory)
        {
            this.cacheDirectory = cacheDirectory;
            this.cacheDirectory.CreateIfDoesNotExist();
            this.assemblies = new CachedAssemblyCollection();
            foreach (FileSystemFile file in cacheDirectory.GetFiles())
            {
                switch (file.Extension.ToUpperInvariant())
                {
                    case ".DLL":
                    case ".EXE":
                        this.assemblies.Add(new CachedAssembly(file));
                        break;
                    default:
                        break;
                }
            }

            //FileSystemFile manifestFile = cacheDirectory + "\\Manifest.xml";
            //if (manifestFile.Exists)
            //{
            //    try
            //    {
            //        XmlDocument document = new XmlDocument();
            //        document.LoadXml(manifestFile.ReadAllText());

            //        foreach (XmlNode root in document.ChildNodes)
            //        {
            //            if (root.Name.ToUpperInvariant() == "ASSEMBLIES")
            //            {
            //                foreach (XmlNode assemblyNode in root.ChildNodes)
            //                {
            //                    if (assemblyNode.Name.ToUpperInvariant() == "ASSEMBLY")
            //                    {
            //                        string name = null;
            //                        string realName = null;

            //                        foreach (XmlAttribute attribute in assemblyNode.Attributes)
            //                        {
            //                            switch (attribute.Name.ToUpperInvariant())
            //                            {
            //                                case "NAME":
            //                                    name = attribute.Value;
            //                                    break;
            //                                case "REALNAME":
            //                                    realName = attribute.Value;
            //                                    break;
            //                                default:
            //                                    break;
            //                            }
            //                        }

            //                        if (realName != null && name != null)
            //                        {
            //                            FileSystemFile assemblyFile = this.cacheDirectory + name;
            //                            if (assemblyFile.Exists)
            //                            {
            //                                this.assemblies.Add(new CachedAssembly(assemblyFile));
            //                            }
            //                        }
            //                    }
            //                }
            //            }
            //        }
            //    }
            //    catch (XmlException)
            //    {
            //    }
            //}
            //this.cacheDirectory.
        }

        public void ClearAllExcept(List<string> names)
        {
            List<string> upperInvarientNames = new List<string>();
            foreach (string name in names)
            {
                upperInvarientNames.Add(name.ToUpperInvariant());
            }

            foreach (FileSystemFile file in this.cacheDirectory.GetFiles())
            {
                if (!upperInvarientNames.Contains(file.Name.ToUpperInvariant()))
                {
                    try
                    {
                        file.Delete();
                        if (this.assemblies.Contains(file.Name))
                        {
                            this.assemblies.Remove(file.Name);
                        }
                    }
                    catch (IOException)
                    {
                    }
                }
            }
        }

        public void ClearAllUnused()
        {
            Dictionary<string, int> used = InternalGlobals.ProfilePlacemarks.GetAllCachedAssemblyNamesReferences();
            this.ClearAllExcept(new List<string>(used.Keys));
            this.clearedUnused = true;
        }

        public void ClearAllUnusedIfNotDoneAlready()
        {
            if (this.clearedUnused)
            {
                return;
            }

            this.ClearAllUnused();
        }

        public CachedAssembly this[string cachedName]
        {
            get { return this.assemblies[cachedName]; }
            //get
            //{
            //    foreach (CachedAssembly assembly in this.assemblies)
            //    {
            //        if (assembly.File.Name == cachedName)
            //        {
            //            return assembly;
            //        }
            //    }

            //    throw new CachedAssemblyNotFoundException("No cached assembly met the specified requirements.");
            //}
        }

        public bool Contains(string cachedName)
        {
             return this.assemblies.Contains(cachedName);
        }

        public CachedAssembly TryGet(string cachedName)
        {
            return this.assemblies.TryGet(cachedName);
        }

        public string InstallAssembly(FileSystemFile fileFrom)
        {
            using (FileStream inStream = new FileStream(fileFrom, FileMode.Open))
            {
                string cachedName = this.InstallAssembly(fileFrom.Name, inStream);
                inStream.Close();
                return cachedName;
            }
            //string nameFormat = String.Format("{0}{1}{2}", fileFrom.NameWithoutExtension, "{+}", fileFrom.Extension);
            //string newName = this.cacheDirectory.GetAvailableFileName(nameFormat, " - ({number})", InsertBase.Two);
            //FileSystemFile cachedFile = this.cacheDirectory + newName;
            //using (FileStream inStream = new FileStream(fileFrom, FileMode.Open))
            //using (FileStream outStream = new FileStream(cachedFile, FileMode.Create))
            //{
            //    inStream.TransferTo(outStream);

            //    inStream.Close(;)
            //    outStream.Close();
            //}

            //this.SaveManifest();
        }
        
        public string InstallAssembly(string assemblyFileName, Stream stream)
        {
            string nameFormat = String.Format(CultureInfo.InvariantCulture, "{0}{1}{2}", Path.GetFileNameWithoutExtension(assemblyFileName), "{+}", Path.GetExtension(assemblyFileName));
            string newName = this.cacheDirectory.GetAvailableFileName(nameFormat, " - ({number})", InsertBase.Two);
            FileSystemFile cachedFile = this.cacheDirectory + newName;
            //using (FileStream inStream = new FileStream(fileFrom, FileMode.Open))
            using (FileStream outStream = new FileStream(cachedFile, FileMode.Create))
            {
                stream.TransferTo(outStream);
                outStream.Close();
            }

            string cachedFileInvarient = cachedFile.Path.ToUpperInvariant();

            using (FileStream cachedFileStream = new FileStream(cachedFile, FileMode.Open))
            {
                foreach (CachedAssembly assembly in this.assemblies)
                {
                    //LoadedAssembly alreadyLoadedAssembly = PromptuSettings.LoadedAssemblies.TryGet(assembly.File.Name);
                    //if (alreadyLoadedAssembly != null && alreadyLoadedAssembly.Bytes != null)
                    //{
                    //    if (alreadyLoadedAssembly.Bytes.IsExactCopyOf(cachedFileStream))
                    //    {
                    //        cachedFileStream.Close();
                    //        cachedFile.Delete();
                    //        return assembly.File.Name;
                    //    }
                    //}
                    //else 
                    try
                    {
                        if (assembly.File.IsExactCopyOf(cachedFileStream))
                        {
                            cachedFileStream.Close();
                            cachedFile.Delete();
                            return assembly.File.Name;
                        }
                    }
                    catch (UnauthorizedAccessException)
                    {
                    }
                }
            }
            
            this.assemblies.Add(new CachedAssembly(cachedFile));

            //this.SaveManifest();

            return cachedFile.Name;
        }

        public void UninstallAssembly(string fileName)
        {
            if (this.assemblies.Contains(fileName))
            {
                this.UninstallAssembly(this.assemblies[fileName]);
            }
        }

        public void UninstallAssembly(CachedAssembly assembly)
        {
            if (!this.assemblies.Contains(assembly))
            {
                throw new ArgumentException("The assembly is not in the cache.");
            }

            this.assemblies.Remove(assembly);
            try
            {
                assembly.File.DeleteIfExists();
            }
            catch (UnauthorizedAccessException)
            {
            }
            //this.SaveManifest();
        }

        //private void SaveManifest()
        //{
        //    XmlDocument document = new XmlDocument();
        //    XmlNode root = document.CreateElement("Assemblies");

        //    foreach (CachedAssembly assembly in this.assemblies)
        //    {
        //        XmlNode assemblyNode = document.CreateElement("Assembly");
        //        assemblyNode.Attributes.Append(XmlUtilites.CreateAttribute("name", assembly.File.Name, document));
        //        assemblyNode.Attributes.Append(XmlUtilites.CreateAttribute("realName", assembly.RealName, document));
        //        root.AppendChild(assemblyNode);
        //    }

        //    document.AppendChild(root);

        //    document.Save(cacheDirectory + "\\Manifest.xml");
        //}

        public IEnumerator<CachedAssembly> GetEnumerator()
        {
            return this.assemblies.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
