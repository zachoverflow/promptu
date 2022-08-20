using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;
using ZachJohnson.Promptu.Collections;

namespace ZachJohnson.Promptu.UserModel.Collections
{
    internal class AssemblyReferencesManifest : InheritableList<string>
    {
        internal const string FileId = "asmmanifest";
        private FileSystemFile file;
        //private ChangeNotifiedList<string> ownedAssemblyReferenceNames;

        public AssemblyReferencesManifest(FileSystemFile file)
        {
            //this.ownedAssemblyReferenceNames = new ChangeNotifiedList<string>();
            this.file = file;
        }

        //public ChangeNotifiedList<string> OwnedAssemblyReferenceNames
        //{
        //    get { return this.ownedAssemblyReferenceNames; }
        //}

        public AssemblyReferencesManifest Clone()
        {
            AssemblyReferencesManifest clone = new AssemblyReferencesManifest(this.file);
            foreach (string item in this)
            {
                clone.Add(item);
            }

            return clone;
        }

        public static AssemblyReferencesManifest FromFile(FileSystemFile file, bool returnNewIfFailure)
        {
            if (returnNewIfFailure)
            {
                try
                {
                    return AssemblyReferencesManifest.FromFile(file);
                }
                catch (XmlException ex)
                {
                    Utilities.ShowPromptuErrorMessageBox(ex);
                }
                catch (IOException ex)
                {
                    Utilities.ShowPromptuErrorMessageBox(ex);
                }

                return new AssemblyReferencesManifest(file);
            }
            else
            {
                return AssemblyReferencesManifest.FromFile(file);
            }
        }

        public static AssemblyReferencesManifest FromFile(FileSystemFile file)
        {
            XmlDocument document = new XmlDocument();

            try
            {
                document.LoadXml(file.ReadAllText());
            }
            catch (XmlException ex)
            {
                ex.Data.Add(InternalGlobals.ExceptionPathToken, file.Path);
                throw;
            }
            catch (IOException ex)
            {
                ex.Data.Add(InternalGlobals.ExceptionPathToken, file.Path);
                throw;
            }

            AssemblyReferencesManifest manifest = new AssemblyReferencesManifest(file);

            LoadFrom(document, manifest);

            return manifest;
        }

        public static void LoadFrom(XmlDocument document, InheritableList<string> collection)
        {
            if (document == null)
            {
                throw new ArgumentNullException("document");
            }
            else if (collection == null)
            {
                throw new ArgumentNullException("collection");
            }

            TrieList loadedNames = new TrieList(SortMode.DecendingFromLastAdded);

            foreach (XmlNode root in document)
            {
                if (root.Name.ToUpperInvariant() == "OWNEDREFERENCES")
                {
                    foreach (XmlNode node in root.ChildNodes)
                    {
                        if (node.Name.ToUpperInvariant() == "REFERENCE")
                        {
                            foreach (XmlAttribute attribute in node.Attributes)
                            {
                                if (attribute.Name.ToUpperInvariant() == "NAME")
                                {
                                    string name = attribute.Value;
                                    if (!loadedNames.Contains(name, CaseSensitivity.Insensitive))
                                    {
                                        collection.Add(name);
                                        loadedNames.Add(name);
                                    }

                                    break;
                                }
                            }
                        }
                    }
                }
            }
        }

        public void Save()
        {
            XmlDocument document = new XmlDocument();
            XmlNode root = document.CreateElement("OwnedReferences");

            foreach (string name in this.ToArray())
            {
                XmlNode referenceNode = document.CreateElement("Reference");
                referenceNode.Attributes.Append(XmlUtilities.CreateAttribute("name", name, document));
                root.AppendChild(referenceNode);
            }

            document.AppendChild(root);

            string listId = List.GetFolderNameFromSubPath(this.file);

            InternalGlobals.FailedToSaveFiles.Remove(listId, FileId);

            try
            {
                document.Save(this.file);
            }
            catch (IOException)
            {
                InternalGlobals.FailedToSaveFiles.Add(
                        new FailedToSaveFile(listId, FileId, this.file, new ResaveHandler(InternalGlobals.ResaveListItem)));
            }
        }

        protected override void AddRangeCore(IEnumerable<string> collection)
        {
            base.AddRangeCore(collection);
            this.Save();
        }

        protected override void ClearCore()
        {
            base.ClearCore();
            this.Save();
        }

        protected override void InsertCore(int index, string item)
        {
            base.InsertCore(index, item);
            this.Save();
        }

        protected override void RemoveAtCore(int index)
        {
            base.RemoveAtCore(index);
            this.Save();
        }

        protected override bool RemoveCore(string item)
        {
            bool removed = base.RemoveCore(item);
            this.Save();
            return removed;
        }
    }
}
