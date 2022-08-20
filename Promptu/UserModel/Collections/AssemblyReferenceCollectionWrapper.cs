using System;
using System.Collections.Generic;
using System.Text;
using ZachJohnson.Promptu.Collections;
using System.Xml;
using System.IO;
using System.Xml.Extensions;
//using ZachJohnson.Promptu.DynamicEntryModel;

namespace ZachJohnson.Promptu.UserModel.Collections
{
    internal class AssemblyReferenceCollectionWrapper : AssemblyReferenceCollection
    {
        internal const string FileId = "asmrefs";
        private string filepath;
        private IdGenerator idGenerator;
        private bool blockSave;

        public AssemblyReferenceCollectionWrapper(string filepath, IEnumerable<AssemblyReference> collection, IdGenerator idGenerator)
            : this(filepath, idGenerator)
        {
            this.AddRange(collection);
        }

        public AssemblyReferenceCollectionWrapper(string filepath, IdGenerator idGenerator)
        {
            if (filepath == null)
            {
                throw new ArgumentNullException(filepath);
            }

            if (idGenerator == null)
            {
                this.idGenerator = new IdGenerator();
            }
            else
            {
                this.idGenerator = idGenerator;
            }

            this.filepath = filepath;
        }

        public event EventHandler Saved;

        public string Filepath
        {
            get { return this.filepath; }
        }

        public bool BlockSave
        {
            get { return this.blockSave; }
            set { this.blockSave = value; }
        }

        public IdGenerator IdGenerator
        {
            get { return this.idGenerator; }
        }

        public static AssemblyReferenceCollectionWrapper FromFile(string filepath, Stream stream, bool returnNewIfFailure, ParameterlessVoid syncCallback)
        {
            if (returnNewIfFailure)
            {
                try
                {
                    return AssemblyReferenceCollectionWrapper.FromFile(filepath, stream, syncCallback);
                }
                catch (LoadException ex)
                {
                    Utilities.ShowPromptuErrorMessageBox(ex);
                }
                catch (XmlException ex)
                {
                    Utilities.ShowPromptuErrorMessageBox(ex);
                }
                catch (IOException ex)
                {
                    Utilities.ShowPromptuErrorMessageBox(ex);
                }

                return new AssemblyReferenceCollectionWrapper(filepath, null);
            }
            else
            {
                return AssemblyReferenceCollectionWrapper.FromFile(filepath, stream, syncCallback);
            }
        }

        public static AssemblyReferenceCollectionWrapper FromFile(string filepath, ParameterlessVoid syncCallback)
        {
            bool addPathInfo = true;
            try
            {
                using (FileStream stream = new FileStream(filepath, FileMode.Open))
                {
                    addPathInfo = false;
                    AssemblyReferenceCollectionWrapper wrapper = FromFile(filepath, stream, syncCallback);
                    stream.Close();
                    return wrapper;
                }
            }
            catch (IOException ex)
            {
                if (addPathInfo)
                {
                    ex.Data.Add(InternalGlobals.ExceptionPathToken, filepath);
                }

                throw;
            }
        }

        public static AssemblyReferenceCollectionWrapper FromFile(string filepath, bool returnNewIfFailure, ParameterlessVoid syncCallback)
        {
            bool addPathInfo = true;
            try
            {
                using (FileStream stream = new FileStream(filepath, FileMode.Open))
                {
                    addPathInfo = false;
                    AssemblyReferenceCollectionWrapper wrapper = FromFile(filepath, stream, returnNewIfFailure, syncCallback);
                    stream.Close();
                    return wrapper;
                }
            }
            catch (IOException ex)
            {
                if (addPathInfo)
                {
                    ex.Data.Add(InternalGlobals.ExceptionPathToken, filepath);
                }

                throw;
            }
        }

        public static AssemblyReferenceCollectionWrapper FromFile(string filepath, Stream stream, ParameterlessVoid syncCallback)
        {
            XmlDocument assemblyReferencesDocument = new XmlDocument();
            StreamReader reader = new StreamReader(stream);
            try
            {
                assemblyReferencesDocument.LoadXml(reader.ReadToEnd());
            }
            catch (XmlException ex)
            {
                ex.Data.Add(InternalGlobals.ExceptionPathToken, filepath);
                throw;
            }

            IdGenerator idGenerator = null;

            XmlNode assemblyReferencesNode = assemblyReferencesDocument.FindChild(AssemblyReferenceCollection.XmlAlias, false);

            if (assemblyReferencesNode == null)
            {
                Utilities.AddPathInformationAndThrow(
                    new LoadException("Missing 'AssemblyReferences' root node."),
                    filepath);
            }

            foreach (XmlAttribute attribute in assemblyReferencesNode.Attributes)
            {
                if (attribute.Name.ToUpperInvariant() == "NEXTID")
                {
                    try
                    {
                        idGenerator = new IdGenerator(attribute.Value);
                    }
                    catch (FormatException)
                    {
                    }
                    catch (OverflowException)
                    {
                    }
                }
            }

            if (idGenerator == null)
            {
                idGenerator = new IdGenerator();
            }

            using (AssemblyReferenceCollection collection = AssemblyReferenceCollection.FromXml(assemblyReferencesNode, syncCallback))
            {
                AssemblyReferenceCollectionWrapper wrapper = new AssemblyReferenceCollectionWrapper(
                    filepath,
                    collection,
                    idGenerator);
                idGenerator.SetNextIdFromCollectionAndSetMissingIds(wrapper);
                return wrapper;
            }
        }

        public new AssemblyReferenceCollectionWrapper Clone()
        {
            return (AssemblyReferenceCollectionWrapper)this.CloneCore();
        }

        protected override AssemblyReferenceCollection CloneCore()
        {
            return new AssemblyReferenceCollectionWrapper(this.filepath, base.CloneCore(), this.idGenerator.Clone());
        }

        public void Save()
        {
            this.Save(true);
        }

        public void Save(bool raiseEvent)
        {
            if (!this.blockSave)
            {
                XmlDocument document = new XmlDocument();
                XmlNode node = this.ToXml(document);
                node.Attributes.Append(XmlUtilities.CreateAttribute("nextId", this.idGenerator.Peek().ToString(), document));
                document.AppendChild(node);

                string listId = List.GetFolderNameFromSubPath(this.Filepath);

                InternalGlobals.FailedToSaveFiles.Remove(listId, FileId);

                try
                {
                    document.Save(this.filepath);
                }
                catch (IOException)
                {
                    InternalGlobals.FailedToSaveFiles.Add(
                        new FailedToSaveFile(listId, FileId, this.filepath, new ResaveHandler(InternalGlobals.ResaveListItem)));
                }

                if (raiseEvent)
                {
                    this.OnSaved(EventArgs.Empty);
                }
            }
        }

        //public void Reload()
        //{
        //    this.Clear();
        //    AssemblyReferenceCollectionWrapper wrapper = FromFile(this.filepath, true);
        //    this.AddRange(wrapper);
        //}

        protected override void OnItemCachedNameChanged(ItemEventArgs<AssemblyReference> e)
        {
            this.Save();
            base.OnItemCachedNameChanged(e);
        }

        protected virtual void OnSaved(EventArgs e)
        {
            EventHandler handler = this.Saved;
            if (handler != null)
            {
                handler(this, e);
            }
        }
    }
}
