using System;
using System.Collections.Generic;
using System.Text;
using ZachJohnson.Promptu.UserModel.Collections;

namespace ZachJohnson.Promptu.UserModel.Differencing
{
    internal class FunctionAutoChangeResolver
        : AutoChangeResolver<FunctionDiffDiff, FunctionDiff, Function, FunctionCollection>
    {
        public FunctionAutoChangeResolver(FunctionCollection synthesisCollection, IdGenerator synthesisIdGenerator, HistoryCollection history, DiffVersion ignoreHistoryRemovalForVersion)
            : base(synthesisCollection, synthesisIdGenerator, history, ignoreHistoryRemovalForVersion)
        {
        }

        protected override void AddCloneToCollection(Function itemToClone, FunctionCollection collection)
        {
            collection.Add(itemToClone.Clone());
        }

        protected override void RemoveItemEntriesFromHistory(HistoryCollection history, Function item)
        {
            item.RemoveEntriesFromHistory(history);
        }

        //protected override void RemoveCorrespondingFromCollection(Function item, FunctionCollection collection)
        //{
        //    collection.Remove(item.Name, item.NumberOfParameters);
        //}
    }
}
