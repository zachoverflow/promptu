// Copyright 2022 Zach Johnson
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

namespace ZachJohnson.Promptu.Collections
{
    using System;
    using System.Collections;
    using System.Collections.Extensions;
    using System.Collections.Generic;
    using System.Extensions;
    using System.Globalization;
    using System.Text;

    internal class TrieDictionary<TValue> : IEnumerable<string>, ITrie where TValue : class
    {
        private readonly List<TrieDictionaryNode> endingNodes = new List<TrieDictionaryNode>();
        private KeyAbstractionCollection<TrieDictionaryNode> keyAbstraction;
        private TrieDictionaryNode children;
        private SortMode sortMode;
        private int blockingRaiseChanged;
        private CaseSensitivity defaultCaseSensitivity;
        private bool allowInsert;

        public TrieDictionary(SortMode sortMode)
            : this(sortMode, CaseSensitivity.Sensitive)
        {
        }

        public TrieDictionary(SortMode sortMode, CaseSensitivity defaultCaseSensitivity)
            : this(sortMode, defaultCaseSensitivity, false)
        {
        }

        public TrieDictionary(SortMode sortMode, CaseSensitivity defaultCaseSensitivity, bool allowInsert)
        {
            this.keyAbstraction = new KeyAbstractionCollection<TrieDictionaryNode>(this.endingNodes);
            this.sortMode = sortMode;
            this.defaultCaseSensitivity = defaultCaseSensitivity;
            this.children = new TrieDictionaryNode('a', null, this.sortMode);
            this.allowInsert = allowInsert;
        }

        public event EventHandler Changed;

        public SortMode SortMode
        {
            get { return this.sortMode; }
        }

        public KeyAbstractionCollection<TrieDictionaryNode> Keys
        {
            get { return this.keyAbstraction; } 
        }

        public int Count
        {
            get { return this.endingNodes.Count; }
        }
        
        public TValue this[int index]
        {
            get { return this.GetItemCore(index); }
            set { this.SetItemCore(index, value); }
        }

        public TValue this[string key, CaseSensitivity caseSensitivity]
        {
            get 
            {
                if (this.defaultCaseSensitivity == CaseSensitivity.Insensitive)
                {
                    key = key.ToUpper(CultureInfo.CurrentCulture);
                    caseSensitivity = CaseSensitivity.Sensitive;
                }

                return this.GetItemCore(key, caseSensitivity); 
            }

            set 
            {
                if (this.defaultCaseSensitivity == CaseSensitivity.Insensitive)
                {
                    key = key.ToUpper(CultureInfo.CurrentCulture);
                    caseSensitivity = CaseSensitivity.Sensitive;
                }

                this.SetItemCore(key, caseSensitivity, value); 
            }
        }

        public void AddChangeEventBlocker()
        {
            this.blockingRaiseChanged++;
        }

        public void RemoveChangeEventBlocker()
        {
            if (this.blockingRaiseChanged > 0)
            {
                this.blockingRaiseChanged--;
            }
        }

        public TValue TryGetItem(string key, CaseSensitivity caseSensitivity, out bool found)
        {
            if (this.defaultCaseSensitivity == CaseSensitivity.Insensitive)
            {
                key = key.ToUpper(CultureInfo.CurrentCulture);
                caseSensitivity = CaseSensitivity.Sensitive;
            }

            return this.TryGetItemCore(key, caseSensitivity, out found);
        }

        public void SortKeys()
        {
            SortedList<string, TrieDictionaryNode> sortedItems = new SortedList<string, TrieDictionaryNode>(this.endingNodes.Count);
            foreach (TrieDictionaryNode endingNode in this.endingNodes)
            {
                sortedItems.Add(endingNode.GetFollowUpValue(), endingNode);
            }

            this.endingNodes.Clear();
            this.endingNodes.AddRange(sortedItems.Values);
        }

        public Dictionary<string, TValue> FindAllThatStartWith(string filter, CaseSensitivity caseSensitivity)
        {
            if (this.defaultCaseSensitivity == CaseSensitivity.Insensitive)
            {
                filter = filter.ToUpper(CultureInfo.CurrentCulture);
                caseSensitivity = CaseSensitivity.Sensitive;
            }

            return this.FindAllThatStartWith(filter, caseSensitivity, null);
        }

        public Dictionary<string, TValue> FindAllThatStartWith(string filter, CaseSensitivity caseSensitivity, Filter<string, TValue> itemFilter)
        {
            if (this.defaultCaseSensitivity == CaseSensitivity.Insensitive)
            {
                filter = filter.ToUpper(CultureInfo.CurrentCulture);
                caseSensitivity = CaseSensitivity.Sensitive;
            }

            Dictionary<string, TValue> filtered = new Dictionary<string, TValue>();
            this.children.FindAllThatStartWith(filter.ToCharArray(), 0, new char[0], caseSensitivity, filtered, itemFilter);
            return filtered;
        }

        public void Add(string key, TValue value)
        {
            this.Insert(this.Count, key, value);
        }

        public void Insert(int index, string key, TValue value)
        {
            if (this.defaultCaseSensitivity == CaseSensitivity.Insensitive)
            {
                key = key.ToUpper(CultureInfo.CurrentCulture);
            }

            this.InsertCore(index, key, value);
            this.OnChanged(EventArgs.Empty);
        }

        public bool Remove(string key)
        {
            if (this.defaultCaseSensitivity == CaseSensitivity.Insensitive)
            {
                key = key.ToUpper(CultureInfo.CurrentCulture);
            }

            return this.RemoveCore(key);
        }

        public string TryFindKey(string startsWith, CaseSensitivity caseSensitivity)
        {
            if (this.defaultCaseSensitivity == CaseSensitivity.Insensitive)
            {
                startsWith = startsWith.ToUpper(CultureInfo.CurrentCulture);
                caseSensitivity = CaseSensitivity.Sensitive;
            }

            StringBuilder builder = new StringBuilder();

            if (!this.children.Find(ref startsWith, 0, builder, caseSensitivity, false))
            {
                return null;
            }

            return builder.ToString();
        }

        public string TryFindWholeKey(string startsWith, CaseSensitivity caseSensitivity)
        {
            if (this.defaultCaseSensitivity == CaseSensitivity.Insensitive)
            {
                startsWith = startsWith.ToUpper(CultureInfo.CurrentCulture);
                caseSensitivity = CaseSensitivity.Sensitive;
            }

            StringBuilder builder = new StringBuilder();

            if (!this.children.Find(ref startsWith, 0, builder, caseSensitivity, true))
            {
                return null;
            }

            return builder.ToString();
        }

        public void Clear()
        {
            this.ClearCore();
        }

        public bool Contains(string wholeString, CaseSensitivity caseSensitivity)
        {
            if (caseSensitivity == CaseSensitivity.Insensitive && this.defaultCaseSensitivity != CaseSensitivity.Insensitive)
            {
                string wholeStringToUpper = wholeString.ToUpperInvariantNullSafe();
                foreach (string key in this.Keys)
                {
                    if (key.ToUpperInvariant() == wholeStringToUpper)
                    {
                        return true;
                    }
                }

                return false;
            }

            return this.Keys.Contains(wholeString);
        }

        public void AddRange(TrieDictionary<TValue> collection)
        {
            foreach (string key in collection)
            {
                this.Add(key, collection[key, CaseSensitivity.Sensitive]);
            }

            this.OnChanged(EventArgs.Empty);
        }

        public IEnumerator<string> GetEnumerator()
        {
            return this.keyAbstraction.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
        
        protected virtual void ClearCore()
        {
            this.children = new TrieDictionaryNode('a', null, this.sortMode);
            this.endingNodes.Clear();
            this.OnChanged(EventArgs.Empty);
        }

        protected virtual bool RemoveCore(string key)
        {
            TrieDictionaryNode endingNode;
            bool removed = this.children.Remove(ref key, 0, out endingNode);

            if (endingNode != null)
            {
                this.endingNodes.Remove(endingNode);
            }

            if (removed)
            {
                this.OnChanged(EventArgs.Empty);
            }

            return removed;
        }

        protected virtual void InsertCore(int index, string key, TValue value)
        {
            TrieDictionaryNode endingNode;
            this.children.Define(ref key, 0, value, out endingNode);

            if (endingNode != null)
            {
                if (this.allowInsert)
                {
                    this.endingNodes.Insert(index, endingNode);
                }
                else
                {
                    this.endingNodes.Add(endingNode);
                }
            }
        }

        protected virtual TValue GetItemCore(int index)
        {
            return this.endingNodes[index].Value;
        }

        protected virtual void SetItemCore(int index, TValue value)
        {
            this.endingNodes[index].Value = value;
        }

        protected virtual TValue GetItemCore(string key, CaseSensitivity caseSensitivity)
        {
            bool found;
            TValue value = this.children.GetValue(ref key, 0, caseSensitivity, out found);
            if (!found)
            {
                throw new KeyNotFoundException(String.Format(CultureInfo.CurrentCulture, "Key '{0}' is not in the list.", key));
            }

            return value;
        }

        protected virtual void SetItemCore(string key, CaseSensitivity caseSensitivity, TValue value)
        {
            bool found;
            this.children.SetItem(ref key, 0, caseSensitivity, value, out found);
            if (!found)
            {
                throw new KeyNotFoundException(String.Format(CultureInfo.CurrentCulture, "Key '{0}' is not in the list.", key));
            }

            this.OnChanged(EventArgs.Empty);
        }

        protected virtual TValue TryGetItemCore(string key, CaseSensitivity caseSensitivity, out bool found)
        {
            return this.children.GetValue(ref key, 0, caseSensitivity, out found);
        }

        protected virtual void OnChanged(EventArgs e)
        {
            if (this.blockingRaiseChanged <= 0)
            {
                EventHandler handler = this.Changed;
                if (handler != null)
                {
                    handler(this, e);
                }
            }
        }

        private static int CompareEndingNodes(TrieDictionaryNode x, TrieDictionaryNode y)
        {
            return x.GetFollowUpValue().CompareTo(y.GetFollowUpValue());
        }

        public class TrieDictionaryNode : TrieNodeBase<TrieDictionaryNode>
        {
            private TValue value;

            public TrieDictionaryNode(char character, TrieDictionaryNode parent, SortMode sortMode)
                : base(character, parent, sortMode)
            {
            }

            public TValue Value
            {
                get { return this.value; }
                set { this.value = value; }
            }

            public void Define(ref string chars, int index, TValue value, out TrieDictionaryNode endingNode)
            {
                if (index > chars.Length - 1)
                {
                    if (this.ContainsEnding == false)
                    {
                        this.value = value;
                        this.ContainsEnding = true;
                        this.EndingLastAdded = true;
                        endingNode = this;
                    }
                    else
                    {
                        throw new ArgumentException(String.Format(CultureInfo.CurrentCulture, "Key '{0}' is already present.", chars));
                    }
                }
                else
                {
                    char charToAdd = chars[index];
                    if (!this.Children.ContainsKey(charToAdd))
                    {
                        if (this.SortMode == SortMode.DecendingFromLastAdded)
                        {
                            this.Children.Insert<char, TrieDictionaryNode>(
                                charToAdd,
                                new TrieDictionaryNode(charToAdd, this, this.SortMode),
                                0);
                        }
                        else
                        {
                            this.Children.Add(charToAdd, new TrieDictionaryNode(charToAdd, this, this.SortMode));
                        }
                    }
                    else if (this.SortMode == SortMode.DecendingFromLastAdded)
                    {
                        TrieDictionaryNode collection = this.Children[charToAdd];
                        this.Children.Remove(charToAdd);
                        this.Children.Insert<char, TrieDictionaryNode>(
                                charToAdd,
                                collection,
                                0);
                    }

                    this.EndingLastAdded = false;
                    this.Children[charToAdd].Define(ref chars, index + 1, value, out endingNode);
                }
            }

            public bool Remove(ref string chars, int index, out TrieDictionaryNode endingNode)
            {
                if (index > chars.Length - 1)
                {
                    if (this.ContainsEnding)
                    {
                        this.ContainsEnding = false;
                        endingNode = this;
                        return true;
                    }

                    endingNode = null;
                    return false;
                }
                else
                {
                    char charToRemove = chars[index];
                    if (!this.Children.ContainsKey(charToRemove))
                    {
                        endingNode = null;
                        return false;
                    }

                    bool removed = this.Children[charToRemove].Remove(ref chars, index + 1, out endingNode);

                    TrieDictionaryNode child = this.Children[charToRemove];

                    if (child.Children.Count == 0 && !child.ContainsEnding)
                    {
                        this.Children.Remove(charToRemove);
                    }

                    return removed;
                }
            }

            public void SetItem(ref string chars, int index, CaseSensitivity caseSensitivity, TValue value, out bool found)
            {
                found = true;
                if (index > chars.Length - 1)
                {
                    if (this.ContainsEnding)
                    {
                        this.value = value;
                        return;
                    }
                    else
                    {
                        found = false;
                        return;
                    }
                }
                else
                {
                    char nextChar = chars[index];

                    bool containsInThisCasing = this.Children.ContainsKey(nextChar);

                    if (caseSensitivity == CaseSensitivity.Insensitive && !containsInThisCasing)
                    {
                        nextChar = nextChar.ReverseCasing(CultureInfo.CurrentCulture);
                        containsInThisCasing = this.Children.ContainsKey(nextChar);
                    }

                    if (!containsInThisCasing)
                    {
                        found = false;
                        return;
                    }

                    if (caseSensitivity == CaseSensitivity.Insensitive)
                    {
                        bool foundByChild;
                        this.Children[nextChar].SetItem(ref chars, index + 1, caseSensitivity, value, out foundByChild);
                        if (!foundByChild)
                        {
                            nextChar = nextChar.ReverseCasing(CultureInfo.CurrentCulture);
                            if (!this.Children.ContainsKey(nextChar))
                            {
                                found = false;
                                return;
                            }
                        }
                        else
                        {
                            return;
                        }
                    }

                    this.Children[nextChar].SetItem(ref chars, index + 1, caseSensitivity, value, out found);
                }
            }

            public TValue GetValue(ref string chars, int index, CaseSensitivity caseSensitivity, out bool found)
            {
                found = true;
                if (index > chars.Length - 1)
                {
                    if (this.ContainsEnding)
                    {
                        return this.value;
                    }
                    else
                    {
                        found = false;
                        return null;
                    }
                }
                else
                {
                    char nextChar = chars[index];

                    bool containsInThisCasing = this.Children.ContainsKey(nextChar);

                    if (caseSensitivity == CaseSensitivity.Insensitive && !containsInThisCasing)
                    {
                        nextChar = nextChar.ReverseCasing(CultureInfo.CurrentCulture);
                        containsInThisCasing = this.Children.ContainsKey(nextChar);
                    }

                    if (!containsInThisCasing)
                    {
                        found = false;
                        return null;
                    }

                    if (caseSensitivity == CaseSensitivity.Insensitive)
                    {
                        bool foundByChild;
                        TValue value = this.Children[nextChar].GetValue(ref chars, index + 1, caseSensitivity, out foundByChild);

                        if (!foundByChild)
                        {
                            nextChar = nextChar.ReverseCasing(CultureInfo.CurrentCulture);
                            if (!this.Children.ContainsKey(nextChar))
                            {
                                found = false;
                                return null;
                            }
                        }
                        else
                        {
                            return value;
                        }
                    }

                    return this.Children[nextChar].GetValue(ref chars, index + 1, caseSensitivity, out found);
                }
            }

            public void FindAllThatStartWith(char[] chars, int index, char[] actualChars, CaseSensitivity caseSensitivity, Dictionary<string, TValue> items, Filter<string, TValue> itemFilter)
            {
                if (index > chars.Length - 1)
                {
                    if (this.ContainsEnding)
                    {
                        string name = new string(actualChars);
                        if (itemFilter != null)
                        {
                            if (!itemFilter.IsValid(name, this.value))
                            {
                                return;
                            }

                            name = itemFilter.TranslateKey(name, this.value);
                        }

                        items.Add(name, this.value);
                    }

                    foreach (char c in Children.Keys)
                    {
                        char[] charsUpToThisPoint = new char[actualChars.Length + 1];
                        actualChars.CopyTo(charsUpToThisPoint, 0);
                        charsUpToThisPoint[actualChars.Length] = c;
                        this.Children[c].FindAllThatStartWith(chars, index + 1, charsUpToThisPoint, caseSensitivity, items, itemFilter);
                    }

                    return;
                }
                else
                {
                    char nextChar = chars[index];
                    char originalChar = nextChar;

                    bool containsInThisCasing = this.Children.ContainsKey(nextChar);

                    if (caseSensitivity == CaseSensitivity.Insensitive && !containsInThisCasing)
                    {
                        nextChar = nextChar.ReverseCasing(CultureInfo.CurrentCulture);
                        containsInThisCasing = this.Children.ContainsKey(nextChar);
                    }

                    if (!containsInThisCasing)
                    {
                        return;
                    }

                    char[] charsUpToThisPoint = new char[actualChars.Length + 1];
                    actualChars.CopyTo(charsUpToThisPoint, 0);
                    charsUpToThisPoint[actualChars.Length] = nextChar;

                    if (caseSensitivity == CaseSensitivity.Insensitive)
                    {
                        this.Children[nextChar].FindAllThatStartWith(chars, index + 1, charsUpToThisPoint, caseSensitivity, items, itemFilter);

                        char whatWillBeNextChar = nextChar.ReverseCasing(CultureInfo.CurrentCulture);
                        if (whatWillBeNextChar == nextChar || !this.Children.ContainsKey(whatWillBeNextChar))
                        {
                            return;
                        }

                        nextChar = whatWillBeNextChar;
                        charsUpToThisPoint[actualChars.Length] = nextChar;
                    }

                    this.Children[nextChar].FindAllThatStartWith(chars, index + 1, charsUpToThisPoint, caseSensitivity, items, itemFilter);
                }
            }

            public bool Find(ref string chars, int index, StringBuilder builder, CaseSensitivity caseSensitivity, bool onlyReturnWholeKey)
            {
                if (index > chars.Length - 1)
                {
                    if (onlyReturnWholeKey)
                    {
                        return this.ContainsEnding;
                    }

                    if (!this.ContainsEnding || (this.ContainsEnding && !this.EndingLastAdded && this.SortMode == SortMode.DecendingFromLastAdded))
                    {
                        if (this.Children.Count == 0)
                        {
                            if (this.ContainsEnding && this.SortMode == SortMode.DecendingFromLastAdded)
                            {
                                return true;
                            }
                            else
                            {
                                return false;
                            }
                        }
                        else
                        {
                            foreach (char c in Children.Keys)
                            {
                                builder.Append(c.ToString());
                                this.Children[c].Find(ref chars, index + 1, builder, caseSensitivity, onlyReturnWholeKey);
                                break;
                            }

                            return true;
                        }
                    }

                    return true;
                }
                else
                {
                    char nextChar = chars[index];
                    char originalChar = nextChar;

                    bool containsInThisCasing = this.Children.ContainsKey(nextChar);

                    if (caseSensitivity == CaseSensitivity.Insensitive && !containsInThisCasing)
                    {
                        nextChar = nextChar.ReverseCasing(CultureInfo.CurrentCulture);
                        containsInThisCasing = this.Children.ContainsKey(nextChar);
                    }

                    if (!containsInThisCasing)
                    {
                        return false;
                    }

                    StringBuilder subsequentBuilder = new StringBuilder();

                    if (caseSensitivity == CaseSensitivity.Insensitive)
                    {
                        if (!this.Children[nextChar].Find(ref chars, index + 1, subsequentBuilder, caseSensitivity, onlyReturnWholeKey))
                        {
                            char whatWillBeNextChar = nextChar.ReverseCasing(CultureInfo.CurrentCulture);

                            subsequentBuilder = new StringBuilder();
                            if (whatWillBeNextChar == nextChar || !this.Children.ContainsKey(whatWillBeNextChar))
                            {
                                return false;
                            }

                            nextChar = whatWillBeNextChar;
                        }
                        else
                        {
                            builder.Append(nextChar);
                            builder.Append(subsequentBuilder.ToString());
                            return true;
                        }
                    }

                    builder.Append(nextChar);

                    bool found = this.Children[nextChar].Find(ref chars, index + 1, subsequentBuilder, caseSensitivity, onlyReturnWholeKey);   

                    builder.Append(subsequentBuilder.ToString());

                    return found;
                }
            }
        }
    }
}
