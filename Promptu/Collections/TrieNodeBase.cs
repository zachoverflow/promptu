//-----------------------------------------------------------------------
// <copyright file="TrieNodeBase.cs" company="ZachJohnson">
//     Copyright (c) Zach Johnson. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

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
