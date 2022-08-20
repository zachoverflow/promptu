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
    using System.Collections.Generic;
    using System.Text;

    internal abstract class TrieNodeBase<TTrieNode>
        where TTrieNode : TrieNodeBase<TTrieNode>
    {
        private static CharComparer charComparer = new CharComparer();
        private byte flags;
        private char character;
        private IDictionary<char, TTrieNode> children;
        private TTrieNode parent;

        public TrieNodeBase(char character, TTrieNode parent, SortMode sortMode)
        {
            this.SortMode = sortMode;
            this.character = character;
            this.parent = parent;
            if (sortMode == SortMode.DecendingFromLastAdded)
            {
                this.children = new Dictionary<char, TTrieNode>(0);
            }
            else
            {
                this.children = new SortedDictionary<char, TTrieNode>(charComparer);
            }
        }

        public char Character
        {
            get { return this.character; }
        }

        public TTrieNode Parent
        {
            get { return this.parent; }
        }

        protected bool ContainsEnding
        {
            get 
            { 
                return (this.flags & 1) != 0; 
            }

            set 
            {
                if (this.ContainsEnding != value)
                {
                    this.flags ^= 1;
                }
            }
        }

        protected bool EndingLastAdded
        {
            get
            {
                return (this.flags & 2) != 0;
            }

            set
            {
                if (this.EndingLastAdded != value)
                {
                    this.flags ^= 2;
                }
            }
        }

        protected SortMode SortMode
        {
            get 
            { 
                return (SortMode)((this.flags & 4) >> 2); 
            }

            private set
            {
                if (this.SortMode != value)
                {
                    this.flags ^= 4;
                }
            }
        }

        protected IDictionary<char, TTrieNode> Children
        {
            get { return this.children; }
        }

        public void FollowUp(StringBuilder builder)
        {
            if (this.Parent != null)
            {
                builder.Insert(0, this.Character);
                this.Parent.FollowUp(builder);
            }
        }

        public string GetFollowUpValue()
        {
            StringBuilder builder = new StringBuilder();
            this.FollowUp(builder);
            return builder.ToString();
        }
    }
}
