using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using ZachJohnson.Promptu.Collections;
using System.IO;
using ZachJohnson.Promptu.UserModel.Collections;

namespace ZachJohnson.Promptu.UserModel
{
    internal class ProfilePlacemark : ProfileBase
    {
        public ProfilePlacemark(FileSystemDirectory directory, string name, bool showSplashScreen)
            : base(directory, name, showSplashScreen)
        {
        }

        public string Name
        {
            get
            {
                return base.NameInternal;
            }

            set
            {
                base.NameInternal = value;
            }
        }

        public bool ShowSplashScreen
        {
            get { return this.ShowSplashScreenInternal; }
        }

        public Profile GetEntireProfile()
        {
            try
            {
                return Profile.FromFolder(this.Directory);
            }
            catch (LoadException ex)
            {
                throw new ProfileLoadException("Profile is corrupted.", ex);
            }
            catch (FileNotFoundException ex)
            {
                throw new ProfileLoadException("Profile is corrupted.", ex);
            }
            catch (XmlException ex)
            {
                throw new ProfileLoadException("Profile is corrupted.", ex);
            }
        }

        public static ProfilePlacemark FromFolder(FileSystemDirectory folder)
        {
            FileSystemFile profileConfigFile = folder + "Profile.xml";

            if (!profileConfigFile.Exists)
            {
                throw new ArgumentException("'folder' is not a valid profile folder.");
            }

            string name = null;
            bool showSplashScreen = true;

            XmlDocument profileConfigDocument = new XmlDocument();

            try
            {
                profileConfigDocument.LoadXml(profileConfigFile.ReadAllText());
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
                                case "SHOWSPLASHSCREEN":
                                    showSplashScreen = Utilities.TryParseBoolean(attribute.Value, showSplashScreen);
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

                        break;
                    default:
                        break;
                }
            }

            if (name == null)
            {
                Utilities.AddPathInformationAndThrow(
                    new LoadException("The 'name' attribute is missing from the 'Profile' root node."),
                    profileConfigFile.Path);
            }

            return new ProfilePlacemark(folder, name, showSplashScreen);
        }

        public List<AssemblyReference> GetAllAssemblyReferences()
        {
            List<AssemblyReference> assemblyReferences = new List<AssemblyReference>();

            if (InternalGlobals.CurrentProfile != null && InternalGlobals.CurrentProfile.FolderName == this.FolderName)
            {
                foreach (List list in InternalGlobals.CurrentProfile.Lists)
                {
                    assemblyReferences.AddRange(list.AssemblyReferences);
                }
            }
            else
            {
                try
                {
                    foreach (FileSystemDirectory listDirectory in this.GetListDirectories())
                    {
                        FileSystemFile assemblyReferencesFile = listDirectory + "AssemblyReferences.xml";
                        if (assemblyReferencesFile.Exists)
                        {
                            try
                            {
                                for (int tryCount = 0; tryCount < 3; tryCount++)
                                {
                                    try
                                    {
                                        using (AssemblyReferenceCollectionWrapper references = AssemblyReferenceCollectionWrapper.FromFile(assemblyReferencesFile, null))
                                        {
                                            assemblyReferences.AddRange(references);
                                        }
                                    }
                                    catch (IOException)
                                    {
                                        System.Threading.Thread.Sleep(100);
                                    }
                                }
                            }
                            catch (XmlException)
                            {
                            }
                            catch (LoadException)
                            {
                            }
                        }
                    }
                }
                catch (XmlException)
                {
                }
                catch (IOException)
                {
                }
            }

            return assemblyReferences;
        }

        public List<FileSystemDirectory> GetListDirectories()
        {
            List<FileSystemDirectory> listDirectories = new List<FileSystemDirectory>();
            FileSystemFile profileConfigFile = this.Directory + "Profile.xml";

            XmlDocument profileConfigDocument = new XmlDocument();
            for (int tryCount = 0; tryCount < 3; tryCount++)
            {
                try
                {
                    profileConfigDocument.LoadXml(profileConfigFile.ReadAllText());
                }
                catch (IOException)
                {
                    if (tryCount < 2)
                    {
                        System.Threading.Thread.Sleep(100);
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            foreach (XmlNode root in profileConfigDocument.ChildNodes)
            {
                switch (root.Name.ToUpperInvariant())
                {
                    case "PROFILE":
                        foreach (XmlNode node in root.ChildNodes)
                        {
                            switch (node.Name.ToUpperInvariant())
                            {
                                case "LISTS":
                                    foreach (XmlNode childNode in node.ChildNodes)
                                    {
                                        switch (childNode.Name.ToUpperInvariant())
                                        {
                                            case "LIST":
                                                foreach (XmlAttribute attribute in childNode.Attributes)
                                                {
                                                    switch (attribute.Name.ToUpperInvariant())
                                                    {
                                                        case "FOLDER":
                                                            FileSystemDirectory listDirectory = this.Directory + attribute.Value;
                                                            if (listDirectory.Exists)
                                                            {
                                                                listDirectories.Add(listDirectory);
                                                            }

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
                                default:
                                    break;
                            }
                        }

                        break;
                    default:
                        break;
                }
            }

            return listDirectories;
        }
    }
}
