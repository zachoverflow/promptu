using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using ZachJohnson.Promptu.Collections;

namespace ZachJohnson.Promptu.UserModel.Collections
{
    internal class ValueListCollection : ItemWithIdChangeNotifiedList<ValueList>
    {
        public ValueListCollection()
        {
        }

        public ValueList this[string name]
        {
            get
            {
                ValueList list = this.TryGet(name);

                if (list == null)
                {
                    throw new ArgumentOutOfRangeException("Command not found.");
                }

                return list;
            }
        }

        public bool Contains(string name)
        {
            return this.TryGet(name) != null;
        }

        public ValueList TryGet(string name)
        {
            if (name == null)
            {
                throw new ArgumentNullException("name");
            }

            string nameToUpperInvarient = name.ToUpperInvariant();
            using (DdMonitor.Lock(this))
            {
                foreach (ValueList item in this)
                {
                    if (item.Name.ToUpperInvariant() == nameToUpperInvarient)
                    {
                        return item;
                    }
                }
            }

            return null;
        }

        public bool Remove(string name)
        {
            if (name == null)
            {
                throw new ArgumentNullException("name");
            }

            string nameToUpperInvarient = name.ToUpperInvariant();
            using (DdMonitor.Lock(this))
            {
                foreach (ValueList item in this)
                {
                    if (item.Name.ToUpperInvariant() == nameToUpperInvarient)
                    {
                        return this.Remove(item);
                    }
                }
            }

            return false;
        }

        public TrieList GetNames()
        {
            TrieList names = new TrieList(SortMode.Alphabetical);
            using (DdMonitor.Lock(this))
            {
                foreach (ValueList valueList in this)
                {
                    names.Add(valueList.Name);
                }
            }

            return names;
        }

        public int IndexOf(string name)
        {
            if (name == null)
            {
                throw new ArgumentNullException("name");
            }

            string nameToUpperInvarient = name.ToUpperInvariant();
            using (DdMonitor.Lock(this))
            {
                for (int i = 0; i < this.Count; i++)
                {
                    if (this[i].Name.ToUpperInvariant() == nameToUpperInvarient)
                    {
                        return i;
                    }
                }
            }

            return -1;
        }

        public static ValueListCollection FromXml(XmlNode node)
        {
            if (node == null)
            {
                throw new ArgumentNullException("node");
            }

            List<int> loadedIds = new List<int>();
            ValueListCollection valueLists = new ValueListCollection();

            foreach (XmlNode innerNode in node.ChildNodes)
            {
                switch (innerNode.Name.ToUpperInvariant())
                {
                    case "VALUELIST":
                        try
                        {
                            ValueList valueList = ValueList.FromXml(innerNode);

                            IHasId castValueList = (IHasId)valueList;

                            if (castValueList.Id != null)
                            {
                                if (loadedIds.BinarySearch(castValueList.Id.Value) < 0)
                                {
                                    loadedIds.Add(castValueList.Id.Value);
                                }
                                else
                                {
                                    castValueList.Id = null;
                                }
                            }

                            valueLists.Add(valueList);
                        }
                        catch (LoadException)
                        {
                        }

                        break;
                    default:
                        break;
                }
            }

            return valueLists;
        }

        public ValueListCollection Clone()
        {
            return this.CloneCore();
        }

        protected virtual ValueListCollection CloneCore()
        {
            ValueListCollection clone = new ValueListCollection();
            using (DdMonitor.Lock(this))
            {
                foreach (ValueList item in this)
                {
                    clone.Add(item.Clone());
                }
            }

            return clone;
        }

        public XmlNode ToXml(string name, XmlDocument document)
        {
            if (name == null)
            {
                throw new ArgumentNullException("name");
            }
            else if (document == null)
            {
                throw new ArgumentNullException("document");
            }

            XmlNode node = document.CreateElement(name);

            using (DdMonitor.Lock(this))
            {
                foreach (ValueList valueList in this)
                {
                    node.AppendChild(valueList.ToXml("ValueList", document));
                }
            }

            return node;
        }

        protected override List<ValueList> GetConflictsWithCore(ValueList item)
        {
            List<ValueList> conficts = new List<ValueList>();
            if (item != null)
            {
                ValueList valueList = this.TryGet(item.Name);
                if (valueList != null && valueList.Name.ToUpperInvariant() != item.Name.ToUpperInvariant())
                {
                    conficts.Add(valueList);
                }
            }

            return conficts;
        }
    }
}
