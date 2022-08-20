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

    internal class TrieList : IEnumerable<string>, ITrie
    {
        private readonly List<TrieListNode> endingNodes = new List<TrieListNode>();
        private KeyAbstractionCollection<TrieListNode> keyAbstraction;
        private TrieListNode children;
        private SortMode sortMode;

        public TrieList(SortMode sortMode)
        {
            this.keyAbstraction = new KeyAbstractionCollection<TrieListNode>(this.endingNodes);
            this.sortMode = sortMode;
            this.children = new TrieListNode('a', null, this.sortMode);
        }

        public TrieList(SortMode sortMode, IEnumerable<string> collection)
            : this(sortMode)
        {
            this.AddRange(collection);
        }

        public SortMode SortMode
        {
            get { return this.sortMode; }
        }

        public int Count
        {
            get { return this.endingNodes.Count; }
        }
        
        public string this[int index]
        {
            get { return this.GetItemCore(index); }
        }

        public void Add(string item)
        {
            this.AddCore(item);
        }

        public void AddRange(IEnumerable<string> collection)
        {
            foreach (string item in collection)
            {
                this.Add(item);
            }
        }

        public bool Remove(string item)
        {
            TrieListNode endingNode;
            bool removed = this.children.Remove(ref item, 0, out endingNode);

            if (endingNode != null)
            {
                this.endingNodes.Remove(endingNode);
            }

            return removed;
        }

        public string GetCorrectCasing(string str)
        {
            string strToUpper = str.ToUpperInvariantNullSafe();
            foreach (string str2 in this)
            {
                if (str2.ToUpperInvariant() == strToUpper)
                {
                    return str2;
                }
            }

            return str;
        }

        public string TryFind(string startsWith, CaseSensitivity caseSensitivity)
        {
            StringBuilder builder = new StringBuilder();

            if (!this.children.Find(ref startsWith, 0, builder, caseSensitivity))
            {
                return null;
            }

            return builder.ToString();
        }

        public List<string> FindAllThatStartWith(string filter, CaseSensitivity caseSensitivity)
        {
            List<string> found = new List<string>();
            this.children.FindAllThatStartWith(filter.ToCharArray(), 0, new char[0], caseSensitivity, found);
            return found;
        }

        public void Clear()
        {
            this.children = new TrieListNode('a', null, this.sortMode);
            this.endingNodes.Clear();
        }

        public bool Contains(string wholeString, CaseSensitivity caseSensitivity)
        {
            if (caseSensitivity == CaseSensitivity.Insensitive)
            {
                string wholeStringToUpper = wholeString.ToUpperInvariantNullSafe();
                foreach (string s in this)
                {
                    if (s.ToUpperInvariant() == wholeStringToUpper)
                    {
                        return true;
                    }
                }

                return false;
            }

            return this.keyAbstraction.Contains(wholeString);
        }

        public bool ContainsAny(IEnumerable<string> values, CaseSensitivity caseSensitivity)
        {
            foreach (string value in values)
            {
                if (this.Contains(value, caseSensitivity))
                {
                    return true;
                }
            }

            return false;
        }

        public string[] ToArray()
        {
            string[] array = new string[this.endingNodes.Count];
            this.keyAbstraction.CopyTo(array);
            return array;
        }

        public IEnumerator<string> GetEnumerator()
        {
            return this.keyAbstraction.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        protected virtual void AddCore(string item)
        {
            TrieListNode endingNode;
            this.children.Define(ref item, 0, out endingNode);

            if (endingNode != null)
            {
                this.endingNodes.Add(endingNode);
            }
        }

        protected virtual string GetItemCore(int index)
        {
            return this.keyAbstraction[index];
        }

        public class TrieListNode : TrieNodeBase<TrieListNode>
        {
            public TrieListNode(char character, TrieListNode parent, SortMode sortMode)
                : base(character, parent, sortMode)
            {
            }

            public void Define(ref string chars, int index, out TrieListNode endingNode)
            {
                if (index > chars.Length - 1)
                {
                    if (this.ContainsEnding == false)
                    {
                        this.ContainsEnding = true;
                        this.EndingLastAdded = true;
                        endingNode = this;
                    }
                    else
                    {
                        throw new ArgumentException(String.Format(CultureInfo.CurrentCulture, "The item '{0}' as already been added.", chars));
                    }
                }
                else
                {
                    char charToAdd = chars[index];
                    if (!this.Children.ContainsKey(charToAdd))
                    {
                        if (this.SortMode == SortMode.DecendingFromLastAdded)
                        {
                            this.Children.Insert<char, TrieListNode>(
                                charToAdd,
                                new TrieListNode(charToAdd, this, this.SortMode),
                                0);
                        }
                        else
                        {
                            this.Children.Add(charToAdd, new TrieListNode(charToAdd, this, this.SortMode));
                        }
                    }
                    else if (this.SortMode == SortMode.DecendingFromLastAdded)
                    {
                        TrieListNode collection = this.Children[charToAdd];
                        this.Children.Remove(charToAdd);
                        this.Children.Insert<char, TrieListNode>(
                                charToAdd,
                                collection,
                                0);
                    }

                    this.EndingLastAdded = false;
                    this.Children[charToAdd].Define(ref chars, index + 1, out endingNode);
                }
            }

            public bool Remove(ref string chars, int index, out TrieListNode endingNode)
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

                    TrieListNode child = this.Children[charToRemove];

                    if (child.Children.Count == 0 && !child.ContainsEnding)
                    {
                        this.Children.Remove(charToRemove);
                    }

                    return removed;
                }
            }

            public void FindAllThatStartWith(char[] chars, int index, char[] actualChars, CaseSensitivity caseSensitivity, List<string> found)
            {
                if (index > chars.Length - 1)
                {
                    if (this.ContainsEnding)
                    {
                        found.Add(new string(actualChars));
                    }

                    foreach (char c in Children.Keys)
                    {
                        char[] charsUpToThisPoint = new char[actualChars.Length + 1];
                        actualChars.CopyTo(charsUpToThisPoint, 0);
                        charsUpToThisPoint[actualChars.Length] = c;
                        this.Children[c].FindAllThatStartWith(chars, index + 1, charsUpToThisPoint, caseSensitivity, found);
                    }

                    return;
                }
                else
                {
                    char nextChar = chars[0];
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
                        this.Children[nextChar].FindAllThatStartWith(chars, index + 1, charsUpToThisPoint, caseSensitivity, found);

                        char whatWillBeNextChar = nextChar.ReverseCasing(CultureInfo.CurrentCulture);
                        if (whatWillBeNextChar == nextChar || !this.Children.ContainsKey(whatWillBeNextChar))
                        {
                            return;
                        }

                        nextChar = whatWillBeNextChar;
                        charsUpToThisPoint[actualChars.Length] = nextChar;
                    }

                    this.Children[nextChar].FindAllThatStartWith(chars, index + 1, charsUpToThisPoint, caseSensitivity, found);
                }
            }

            public bool Find(ref string chars, int index, StringBuilder builder, CaseSensitivity caseSensitivity)
            {
                if (index > chars.Length - 1)
                {
                    if (!this.ContainsEnding || (this.ContainsEnding && !this.EndingLastAdded && this.SortMode == SortMode.DecendingFromLastAdded))
                    {
                        if (this.Children.Count <= 0)
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
                                this.Children[c].Find(ref chars, index + 1, builder, caseSensitivity);
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
                        if (!this.Children[nextChar].Find(ref chars, index + 1, subsequentBuilder, caseSensitivity))
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

                    bool found = this.Children[nextChar].Find(ref chars, index + 1, subsequentBuilder, caseSensitivity);

                    builder.Append(nextChar);
                    builder.Append(subsequentBuilder.ToString());

                    return found;
                }
            }
        }    
    }
}
