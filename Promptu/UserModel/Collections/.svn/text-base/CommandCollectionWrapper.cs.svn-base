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
    internal class CommandCollectionWrapper : CommandCollection
    {
        internal const string FileId = "commands";
        private string filepath;
        private IdGenerator idGenerator;
        private bool blockSave;

        public CommandCollectionWrapper(string filepath, IEnumerable<Command> collection, IdGenerator idGenerator)
            : this(filepath, idGenerator)
        {
            this.AddRange(collection);
        }

        public event EventHandler Saved;

        public CommandCollectionWrapper(string filepath, IdGenerator idGenerator)
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

        public new CommandCollectionWrapper Clone()
        {
            return (CommandCollectionWrapper)this.CloneCore();
        }

        protected override CommandCollection CloneCore()
        {
            return new CommandCollectionWrapper(this.filepath, base.CloneCore(), this.idGenerator.Clone());
        }

        public static CommandCollectionWrapper FromFile(string filepath, Stream stream, bool returnNewIfFailure)
        {
            if (returnNewIfFailure)
            {
                try
                {
                    return CommandCollectionWrapper.FromFile(filepath, stream);
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

                return new CommandCollectionWrapper(filepath, null);
            }
            else
            {
                return CommandCollectionWrapper.FromFile(filepath, stream);
            }
        }

        public static CommandCollectionWrapper FromFile(string filepath, bool returnNewIfFailure)
        {
            bool addPathInfo = true;
            try
            {
                using (FileStream stream = new FileStream(filepath, FileMode.Open))
                {
                    addPathInfo = false;
                    CommandCollectionWrapper wrapper = FromFile(filepath, stream, returnNewIfFailure);
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

        public static CommandCollectionWrapper FromFile(string filepath)
        {
            bool addPathInfo = true;
            try
            {
                using (FileStream stream = new FileStream(filepath, FileMode.Open))
                {
                    addPathInfo = false;
                    CommandCollectionWrapper wrapper = FromFile(filepath, stream);
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

        public static CommandCollectionWrapper FromFile(string filepath, Stream stream)
        {
            XmlDocument commandsDocument = new XmlDocument();
            StreamReader reader = new StreamReader(stream);
            try
            {
                commandsDocument.LoadXml(reader.ReadToEnd());
            }
            catch (XmlException ex)
            {
                ex.Data.Add(InternalGlobals.ExceptionPathToken, filepath);
                throw;
            }

            IdGenerator idGenerator = null;

            XmlNode commandsNode = commandsDocument.FindChild(CommandCollection.XmlAlias, false);

            if (commandsNode == null)
            {
                Utilities.AddPathInformationAndThrow(
                    new LoadException("Missing 'Commands' root node."),
                    filepath);
            }

            foreach (XmlAttribute attribute in commandsNode.Attributes)
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

            CommandCollectionWrapper wrapper = new CommandCollectionWrapper(filepath, CommandCollection.FromXml(commandsNode), idGenerator);
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
                XmlNode node = this.ToXml(document);
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
