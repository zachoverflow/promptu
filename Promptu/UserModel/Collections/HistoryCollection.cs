using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Extensions;
using System.IO;
using ZachJohnson.Promptu.Collections;

namespace ZachJohnson.Promptu.UserModel.Collections
{
    internal class HistoryCollection : HistoricalCollection
    {
        internal const string FileId = "history";
        private string filepath;
        private FunctionHistoryCollection functionHistory = new FunctionHistoryCollection();
        private CommandParameterHistoryCollection commandParameterHistory = new CommandParameterHistoryCollection();
        private ComplexHistoryCollection complexHistory = new ComplexHistoryCollection();
        private bool blockComplexHistoryClear = false;

        public HistoryCollection(string filepath)
        {
            if (filepath == null)
            {
                throw new ArgumentNullException("filepath");
            }

            this.filepath = filepath;

            this.RegenerateFunctionHistory();
            this.RegenerateCommandParameterHistory();
        }

        //public HistoryCollection(string filepath, IEnumerable<string> collection)
        //    : this(filepath)
        //{
        //    this.AddRange(collection);
        //}

        public ComplexHistoryCollection ComplexHistory
        {
            get { return this.complexHistory; }
        }

        public CommandParameterHistoryCollection CommandParameterHistory
        {
            get { return this.commandParameterHistory; }
        }

        public FunctionHistoryCollection FunctionHistory
        {
            get { return this.functionHistory; }
        }

        public static HistoryCollection FromFile(string filepath)
        {
            HistoryCollection historyCollection = new HistoryCollection(filepath);

            try
            {
                XmlDocument document = new XmlDocument();
                document.LoadXml(File.ReadAllText(filepath));

                foreach (XmlNode root in document.ChildNodes)
                {
                    switch (root.Name.ToUpperInvariant())
                    {
                        case "HISTORY":
                            foreach (XmlNode entry in root.ChildNodes)
                            {
                                switch (entry.Name.ToUpperInvariant())
                                {
                                    case "ENTRY":
                                        string itemId = null;

                                        foreach (XmlAttribute attribute in entry.Attributes)
                                        {
                                            switch (attribute.Name.ToUpperInvariant())
                                            {
                                                case "ITEMID":
                                                    itemId = attribute.Value;
                                                    break;
                                                default:
                                                    break;
                                            }
                                        }

                                        historyCollection.Add(entry.InnerText, new HistoryDetails(entry.InnerText, itemId));
                                        break;
                                    case "COMPLEXENTRY":
                                        historyCollection.complexHistory.Add(entry.InnerText, new HistoryDetails(entry.InnerText, null));
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
            }
            catch (IOException)
            {
            }
            catch (XmlException)
            {
            }

            return historyCollection;
        }

        public void Save()
        {
            XmlDocument document = new XmlDocument();
            document.AppendHeader();
            XmlNode root = document.CreateElement("History");
            foreach (string entry in this)
            {
                if (!String.IsNullOrEmpty(entry))
                {
                    HistoryDetails details = this[entry, CaseSensitivity.Sensitive];
                    XmlNode node = document.CreateNewNode("Entry", details.EntryValue);
                    if (details != null && details.ItemId != null)
                    {
                        node.Attributes.Append(XmlUtilities.CreateAttribute("itemId", details.ItemId, document));
                    }

                    root.AppendChild(node);
                }
            }

            foreach (string entry in this.complexHistory)
            {
                if (!String.IsNullOrEmpty(entry))
                {
                    HistoryDetails details = this.complexHistory[entry, CaseSensitivity.Sensitive];
                    XmlNode node = document.CreateNewNode("ComplexEntry", details.EntryValue);
                    root.AppendChild(node);
                }
            }

            document.AppendChild(root);

            InternalGlobals.FailedToSaveFiles.Remove(null, FileId);

            try
            {
                document.Save(this.filepath);
            }
            catch (IOException)
            {
                InternalGlobals.FailedToSaveFiles.Add(
                    new FailedToSaveFile(null, FileId, this.filepath, new ResaveHandler(InternalGlobals.ResaveProfileItem)));
            }
        }

        private void RegenerateFunctionHistory()
        {
            this.functionHistory.RegenerateFrom(this);
        }

        private void RegenerateCommandParameterHistory()
        {
            this.commandParameterHistory.RegenerateFrom(this);
        }

        protected override bool RemoveCore(string key)
        {
            bool removed = base.RemoveCore(key);
            this.RegenerateFunctionHistory();
            this.RegenerateCommandParameterHistory();
            return removed;
        }

        protected override void ClearCore()
        {
            base.ClearCore();
            if (!this.blockComplexHistoryClear)
            {
                this.complexHistory.Clear();
            }

            this.RegenerateFunctionHistory();
            this.RegenerateCommandParameterHistory();
        }

        public void Clear(bool doNotClearComplexHistory)
        {
            if (doNotClearComplexHistory)
            {
                this.blockComplexHistoryClear = true;
                this.Clear();
                this.blockComplexHistoryClear = false;
            }
            else
            {
                this.Clear();
            }
        }

        protected override void ClearForHitCeiling()
        {
            this.Clear(true);
        }

        //public void Add(string value)
        //{
        //    this.Add(value, new HistoryDetails(value));
        //}
        //public void RemoveAllThatStartWith(string startsWith)
        //{
        //    this.RemoveAllThatStartWith(new string[] { startsWith } );
        //}

        protected override void RemoveAllThatStartsWithCore(IEnumerable<string> startsWithToRemove)
        {
            base.RemoveAllThatStartsWithCore(startsWithToRemove);
            this.Save();
        }

        protected override void HandlePostAdd(string item, HistoryDetails details)
        {
            base.HandlePostAdd(item, details);
            this.functionHistory.Add(details.EntryValue);
            this.commandParameterHistory.Add(item, details);
        }

        protected override void HandlePostAddHitCeiling()
        {
            base.HandlePostAddHitCeiling();
            this.RegenerateFunctionHistory();
            this.RegenerateCommandParameterHistory();
        }
    }
}
