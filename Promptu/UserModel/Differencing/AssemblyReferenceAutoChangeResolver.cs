using System;
using System.Collections.Generic;
using System.Text;
using ZachJohnson.Promptu.UserModel.Collections;

namespace ZachJohnson.Promptu.UserModel.Differencing
{
    internal class AssemblyReferenceAutoChangeResolver 
        : AutoChangeResolver<AssemblyReferenceDiffDiff, AssemblyReferenceDiff, AssemblyReference, AssemblyReferenceCollection>
    {
        public AssemblyReferenceAutoChangeResolver(AssemblyReferenceCollection synthesisCollection, IdGenerator synthesisIdGenerator, HistoryCollection history, DiffVersion ignoreHistoryRemovalForVersion)
            : base(synthesisCollection, synthesisIdGenerator, history, ignoreHistoryRemovalForVersion)
        {
        }

        protected override void AddCloneToCollection(AssemblyReference itemToClone, AssemblyReferenceCollection collection)
        {
            collection.Add(itemToClone.Clone(itemToClone.SyncCallback));
        }

        protected override void RemoveItemEntriesFromHistory(HistoryCollection history, AssemblyReference item)
        {
        }

        //protected override void RemoveCorrespondingFromCollection(AssemblyReference item, AssemblyReferenceCollection collection)
        //{
        //    collection.Remove(item.Name);
        //}
    }
}
