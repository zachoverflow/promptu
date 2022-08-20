//-----------------------------------------------------------------------
// <copyright file="CompositeList.cs" company="ZachJohnson">
//     Copyright (c) Zach Johnson. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace ZachJohnson.Promptu.Collections
{
    using System;
    using System.Collections.Generic;

    internal class CompositeList<T>
    {
        private List<IList<T>> lists;

        public CompositeList()
        {
            this.lists = new List<IList<T>>();
        }

        public int Count
        {
            get
            {
                int count = 0;

                foreach (IList<T> list in this.lists)
                {
                    count += list.Count;
                }

                return count;
            }
        }

        public T this[int index]
        {
            get
            {
                if (index < 0)
                {
                    throw new ArgumentOutOfRangeException("Index cannot be less than zero.");
                }

                foreach (IList<T> list in this.lists)
                {
                    if (index >= list.Count)
                    {
                        index -= list.Count;
                    }
                    else
                    {
                        return list[index];
                    }
                }

                throw new ArgumentOutOfRangeException("Index cannot be greater than or equal to 'Count'.");
            }
        }

        public void ClearLists()
        {
            this.lists.Clear();
        }

        public void AddListToComposite(IList<T> list)
        {
            if (list == null)
            {
                throw new ArgumentNullException("list");
            }

            this.lists.Add(list);
        }
    }
}
