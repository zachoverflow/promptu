//-----------------------------------------------------------------------
// <copyright file="ItemWithIdChangeNotifiedList.cs" company="ZachJohnson">
//     Copyright (c) Zach Johnson. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace ZachJohnson.Promptu.Collections
{
    using System.Collections.Generic;
    using ZachJohnson.Promptu.UserModel;
    using ZachJohnson.Promptu.UserModel.Collections;

    internal abstract class ItemWithIdChangeNotifiedList<T> : ChangeNotifiedList<T>, IItemsWithIdList<T> where T : class, IHasId
    {
        public ItemWithIdChangeNotifiedList()
        {
        }

        public T TryGet(Id id)
        {
            int? index;
            return this.TryGet(id, out index);
        }

        public T TryGet(Id id, out int? index)
        {
            if (id != null)
            {
                for (int i = 0; i < this.Count; i++)
                {
                    T item = this[i];
                    if (item.Id == id)
                    {
                        index = i;
                        return item;
                    }
                }
            }

            index = null;
            return null;
        }

        public List<T> GetConflictsWith(T item)
        {
            return GetConflictsWithCore(item);
        }

        public bool Remove(Id id)
        {
            if (id != null)
            {
                for (int i = 0; i < this.Count; i++)
                {
                    T item = this[i];
                    if (item.Id == id)
                    {
                        return this.Remove(item);
                    }
                }
            }

            return false;
        }

        protected abstract List<T> GetConflictsWithCore(T item);
    }
}
