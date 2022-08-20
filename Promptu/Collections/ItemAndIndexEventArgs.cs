//-----------------------------------------------------------------------
// <copyright file="Action.cs" company="ZachJohnson">
//     Copyright (c) Zach Johnson. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace ZachJohnson.Promptu.Collections
{
    internal class ItemAndIndexEventArgs<T> : ItemEventArgs<T>
    {
        private int index;

        public ItemAndIndexEventArgs(T item, int index) 
            : base(item)
        {
            this.index = index;
        }

        public int Index
        {
            get { return this.index; }
        }
    }
}
