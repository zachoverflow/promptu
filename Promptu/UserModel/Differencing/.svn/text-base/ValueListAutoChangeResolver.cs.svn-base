using System;
using System.Collections.Generic;
using System.Text;
using ZachJohnson.Promptu.UserModel.Collections;

namespace ZachJohnson.Promptu.UserModel.Differencing
{
    internal class ValueListAutoChangeResolver : AutoChangeResolver<
        ValueListDiffDiff,
        ValueListDiff,
        ValueList,
        ValueListCollection>
    {
        public ValueListAutoChangeResolver(ValueListCollection synthesisCollection, IdGenerator synthesisIdGenerator, HistoryCollection history, DiffVersion ignoreHistoryRemovalForVersion)
            : base(synthesisCollection, synthesisIdGenerator, history, ignoreHistoryRemovalForVersion)
        {
        }

        protected override void AddCloneToCollection(ValueList itemToClone, ValueListCollection collection)
        {
            collection.Add(itemToClone.Clone());
        }

        protected override void RemoveItemEntriesFromHistory(HistoryCollection history, ValueList item)
        {
        }
    }
}
