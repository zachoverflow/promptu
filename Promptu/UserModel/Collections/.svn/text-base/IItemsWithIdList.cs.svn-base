using System;
using System.Collections.Generic;
using System.Text;

namespace ZachJohnson.Promptu.UserModel.Collections
{
    internal interface IItemsWithIdList<T> : IEnumerable<T> where T : IHasId
    {
        T TryGet(Id id);

        T TryGet(Id id, out int? index);

        List<T> GetConflictsWith(T item);

        bool Remove(Id id);

        void Add(T item);
    }
}
