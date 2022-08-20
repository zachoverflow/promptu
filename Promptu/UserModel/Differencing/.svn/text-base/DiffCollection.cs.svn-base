using System;
using System.Collections.Generic;
using System.Text;

namespace ZachJohnson.Promptu.UserModel.Differencing
{
    internal class DiffCollection<TDiff, TItem> : List<TDiff> 
        where TDiff : Diff<TItem, TDiff>
        where TItem : IHasId
    {
        public TDiff TryGetSimilar(TDiff diff)
        {
            int? index;
            return this.TryGetSimilar(diff, out index);
        }

        public TDiff TryGetSimilar(TDiff diff, out int? index)
        {
            for (int i = 0; i < this.Count; i++)
            {
                TDiff item = this[i];
                if (item.IsSimilarTo(diff))
                {
                    index = i;
                    return item;
                }
            }

            index = null;
            return null;
        }
    }
}
