using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;
using System.Xml.Extensions;

namespace ZachJohnson.Promptu.UserModel.Collections
{
    class ValueListCollectionWrapper : ValueListCollection
    {
        internal const string FileId = "valuelists";
        private string filepath;
        private IdGenerator idGenerator;
        private bool blockSave;

        public ValueListCollectionWrapper(string filepath, IEnumerable<ValueList> collection, IdGenerator idGenerator)
            : this(filepath, idGenerator)
        {
            this.AddRange(collection);
        }

        public event EventHandler Saved;

        public ValueListCollectionWrapper(string filepath, IdGenerator idGenerator)
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

        public new ValueListCollectionWrapper Clone()
        {
            return (ValueListCollectionWrapper)this.CloneCore();
        }

        protected override ValueListCollection CloneCore()
        {
            return new ValueListCollectionWrapper(this.filepath, base.CloneCore(), this.idGenerator.Clone());
        }

        public static ValueListCollectionWrapper FromFile(string filepath, Stream stream, bool returnNewIfFailure)
        {
            if (returnNewIfFailure)
            {
                try
                {
                    return ValueListCollectionWrapper.FromFile(filepath, stream);
                }
                catch (LoadException ex)
                {
                    Utilities.ShowPromptuErrorMessageBox(ex);
                }
                catch (XmlException ex)
                {
                    Utilities.ShowPromptuErrorMessageBox(ex);
                }
                catch (FileNotFoundException)
                {
                }
                catch (IOException ex)
                {
                    Utilities.ShowPromptuErrorMessageBox(ex);
                }

                return new ValueListCollectionWrapper(filepath, null);
            }
            else
            {
                return ValueListCollectionWrapper.FromFile(filepath, stream);
            }
        }

        public static ValueListCollectionWrapper FromFile(string filepath, bool returnNewIfFailure)
        {
            bool addPathInfo = true;
            try
            {
                using (FileStream stream = new FileStream(filepath, FileMode.Open))
                {
                    addPathInfo = false;
                    ValueListCollectionWrapper wrapper = FromFile(filepath, stream, returnNewIfFailure);
                    stream.Close();
                    return wrapper;
                }
            }
            catch (FileNotFoundException)
            {
                if (returnNewIfFailure)
                {
                    return new ValueListCollectionWrapper(filepath, null);
                }
                else
                {
                    throw;
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

        public static ValueListCollectionWrapper FromFile(string filepath)
        {
            bool addPathInfo = true;
            try
            {
                using (FileStream stream = new FileStream(filepath, FileMode.Open))
                {
                    addPathInfo = false;
                    ValueListCollectionWrapper wrapper = FromFile(filepath, stream);
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

        public static ValueListCollectionWrapper FromFile(string filepath, Stream stream)
        {
            XmlDocument valueListsDocument = new XmlDocument();
            StreamReader reader = new StreamReader(stream);
            try
            {
                valueListsDocument.LoadXml(reader.ReadToEnd());
            }
            catch (XmlException ex)
            {
                ex.Data.Add(InternalGlobals.ExceptionPathToken, filepath);
                throw;
            }

            IdGenerator idGenerator = null;

            XmlNode valueListsNode = valueListsDocument.FindChild("ValueLists", false);

            if (valueListsNode == null)
            {
                Utilities.AddPathInformationAndThrow(
                    new LoadException("Missing 'ValueLists' root node."),
                    filepath);
            }

            foreach (XmlAttribute attribute in valueListsNode.Attributes)
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

            ValueListCollectionWrapper wrapper = new ValueListCollectionWrapper(filepath, ValueListCollection.FromXml(valueListsNode), idGenerator);
            idGenerator.SetNextIdFromCollectionAndSetMissingIds(wrapper);
            return wrapper;
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
                XmlNode node = this.ToXml("ValueLists", document);
                node.Attributes.Append(XmlUtilities.CreateAttribute("nextId", this.idGenerator.Peek().ToString(), document));
                document.AppendChild(node);
                //document.Save(this.filepath);

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
