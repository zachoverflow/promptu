using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.ObjectModel;
using System.Xml;
using ZachJohnson.Promptu.Collections;
using ZachJohnson.Promptu.UserModel.Differencing;

namespace ZachJohnson.Promptu.UserModel.Collections
{
    class FunctionCollection : ItemWithIdChangeNotifiedList<Function>
    {
        internal const string XmlAlias = "functions";
        public FunctionCollection()
        {
        }

        public Function this[string name, string parameterSignature]
        {
            get
            {
                string uppercaseName = name.ToUpperInvariant();

                using (DdMonitor.Lock(this))
                {
                    foreach (Function item in this)
                    {
                        if (item.ParameterSignature == parameterSignature && uppercaseName == item.Name.ToUpperInvariant())
                        {
                            return item;
                        }
                    }
                }

                throw new ArgumentException("No function has that name.");
            }
        }

        public TrieList GetStringIds()
        {
            TrieList stringIds = new TrieList(SortMode.Alphabetical);

            using (DdMonitor.Lock(this))
            {
                foreach (Function function in this)
                {
                    stringIds.Add(function.GetStringId());
                }
            }

            return stringIds;
        }

        public void RemoveEntriesFromHistory(HistoryCollection history)
        {
            using (DdMonitor.Lock(this))
            {
                foreach (Function function in this)
                {
                    function.RemoveEntriesFromHistory(history);
                }
            }
        }

        public FunctionCollection Clone()
        {
            return this.CloneCore();
        }

        protected virtual FunctionCollection CloneCore()
        {
            FunctionCollection clone = new FunctionCollection();
            using (DdMonitor.Lock(this))
            {
                foreach (Function item in this)
                {
                    clone.Add(item.Clone());
                }
            }

            return clone;
        }

        public TrieList ConstructFindOptimizedStringCollection()
        {
            TrieList collection = new TrieList(SortMode.DecendingFromLastAdded);
            using (DdMonitor.Lock(this))
            {
                foreach (Function function in this)
                {
                    if (!collection.Contains(function.Name, CaseSensitivity.Insensitive))
                    {
                        collection.Add(function.Name);
                    }
                }
            }

            return collection;
        }

        public bool Contains(string name, string parameterSignature)
        {
            string uppercaseName = name.ToUpperInvariant();

            using (DdMonitor.Lock(this))
            {
                foreach (Function item in this)
                {
                    if (item.ParameterSignature == parameterSignature && uppercaseName == item.Name.ToUpperInvariant())
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public Function TryGet(string name, string parameterSignature)
        {
            string uppercaseName = name.ToUpperInvariant();

            using (DdMonitor.Lock(this))
            {
                foreach (Function item in this)
                {
                    if (name == item.Name && item.ParameterSignature == parameterSignature)
                    {
                        return item;
                    }
                }
            }

            return null;
        }

        protected override List<Function> GetConflictsWithCore(Function item)
        {
            List<Function> conficts = new List<Function>();
            if (item != null)
            {
                Function function = this.TryGet(item.Name, item.ParameterSignature);
                if (function != null)
                {
                    conficts.Add(function);
                }
            }

            return conficts;
        }

        //protected override Function TryGetSimilarCore(Function item)
        //{
        //    if (item != null)
        //    {
        //        return this.TryGet(item.Name, item.NumberOfParameters);
        //    }

        //    return null;
        //}

        //public Function TryGet(string name, int parameterCount, out int? index)
        //{
        //    for (int i = 0; i < this.Count; i++)
        //    {
        //        Function item = this[i];
        //        if (name == item.Name && item.NumberOfParameters == parameterCount)
        //        {
        //            index = i;
        //            return item;
        //        }
        //    }

        //    index = null;
        //    return null;
        //}

        //public Function TryGet(string name, int parameterCount, FunctionIdentifierChangeCollection identifierChanges, out int? index)
        //{
        //    if (identifierChanges == null)
        //    {
        //        return TryGet(name, parameterCount, out index);
        //    }

        //    for (int i = 0; i < this.Count; i++)
        //    {
        //        Function item = this[i];
        //        IFunction compareAs;

        //        FunctionIdentifierChange change = identifierChanges.TryGetIdentifierChangeFromRevisedItem(item);

        //        if (change != null)
        //        {
        //            compareAs = change.CreateFilter(ItemType.Base);
        //        }
        //        else
        //        {
        //            compareAs = item;
        //        }

        //        if (name == compareAs.Name && compareAs.NumberOfParameters == parameterCount)
        //        {
        //            index = i;
        //            return item;
        //        }
        //    }

        //    index = null;
        //    return null;
        //}

        public bool ContainsAnyNamed(string name)
        {
            string uppercaseName = name.ToUpperInvariant();
            using (DdMonitor.Lock(this))
            {
                foreach (Function item in this)
                {
                    if (uppercaseName == item.Name.ToUpperInvariant())
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public bool Remove(string name, string parameterSignature)
        {
            Function toRemove = null;
            using (DdMonitor.Lock(this))
            {
                foreach (Function item in this)
                {
                    if (name == item.Name && item.ParameterSignature == parameterSignature)
                    {
                        toRemove = item;
                        break;
                    }
                }

                if (toRemove != null)
                {
                    this.Remove(toRemove);
                    return true;
                }
            }

            return false;
        }

        public static FunctionCollection FromXml(XmlNode node)
        {
            TrieDictionary<List<string>> loaded = new TrieDictionary<List<string>>(SortMode.DecendingFromLastAdded);
            List<int> loadedIds = new List<int>();
            FunctionCollection functions = new FunctionCollection();
            if (node.Name.ToLowerInvariant() == XmlAlias)
            {
                foreach (XmlNode innerNode in node.ChildNodes)
                {
                    if (innerNode.Name.ToLowerInvariant() == Function.XmlAlias)
                    {
                        try
                        {
                            Function function = Function.FromXml(innerNode);
                            List<string> alreadyUsedParameterSignatures;
                            bool found;
                            alreadyUsedParameterSignatures = loaded.TryGetItem(function.Name, CaseSensitivity.Insensitive, out found);
                            if (!found)
                            {
                                alreadyUsedParameterSignatures = new List<string>();
                            }

                            if (!alreadyUsedParameterSignatures.Contains(function.ParameterSignature) && Function.IsValidFunction(function))
                            {
                                if (function.Id != null)
                                {
                                    if (!loadedIds.Contains(function.Id.Value))
                                    {
                                        loadedIds.Add(function.Id.Value);
                                    }
                                    else
                                    {
                                        function.Id = null;
                                    }
                                }

                                functions.Add(function);
                                alreadyUsedParameterSignatures.Add(function.ParameterSignature);
                                if (!found)
                                {
                                    loaded.Add(function.Name, alreadyUsedParameterSignatures);
                                }
                            }
                            else
                            {
                            }
                        }
                        catch (LoadException)
                        {
                        }
                    }
                }
            }
            else
            {
                throw new ArgumentException("The node is not named " + XmlAlias + ".");
            }

            return functions;
        }

        public XmlNode ToXml(XmlDocument document)
        {
            XmlNode node = document.CreateElement("Functions");
            using (DdMonitor.Lock(this))
            {
                foreach (Function item in this)
                {
                    node.AppendChild(item.ToXml(document));
                }
            }

            return node;
        }
    }
}
