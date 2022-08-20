using System;
using System.Collections.Generic;
using System.Text;
using ZachJohnson.Promptu.Collections;
using ZachJohnson.Promptu.UserModel.Collections;

namespace ZachJohnson.Promptu.UserModel.Differencing
{
    internal class ChangeFinder<
        TItem,
        TDiff,
        TDiffMaker,
        TDiffDiff,
        TDiffDiffMaker,
        TItemCollection>

        where TItem : class, IDiffable
        where TDiff : Diff<TItem, TDiff>
        where TDiffMaker : DiffMaker<TItem, TDiff, TItemCollection>, new()
        where TDiffDiff : DiffDiff<TDiff, TItem, TDiffDiff>
        where TDiffDiffMaker : DiffDiffMaker<TDiffDiff, TDiff, TItem, TItemCollection>, new()
        where TItemCollection : IIndexedCollection<TItem>, IItemsWithIdList<TItem>
    {
        private List<TDiffDiff> results;
        private TItemCollection baseCollection;
       // private TIdentifierChangeCollection baseCollectionIdentifierChanges;
        private TItemCollection priorityCollection;
        //private TIdentifierChangeCollection priorityCollectionIdentifierChanges;
        private TItemCollection secondaryCollection;

        public ChangeFinder(TItemCollection baseCollection,
            //TIdentifierChangeCollection baseCollectionIdentifierChanges,
            TItemCollection priorityCollection,
            //TIdentifierChangeCollection priorityCollectionIdentifierChanges,
            TItemCollection secondaryCollection)
            //TIdentifierChangeCollection secondaryCollectionIdentifierChanges)
        {
            this.baseCollection = baseCollection;
            //this.baseCollectionIdentifierChanges = baseCollectionIdentifierChanges;
            this.priorityCollection = priorityCollection;
            //this.priorityCollectionIdentifierChanges = priorityCollectionIdentifierChanges;
            this.secondaryCollection = secondaryCollection;
            //this.secondaryCollectionIdentifierChanges = secondaryCollectionIdentifierChanges;
        }

        public List<TDiffDiff> GetResults()
        {
            if (this.results == null)
            {
                this.FindChanges();
            }

            return this.results;
        }

        private void FindChanges()
        {
            TDiffMaker diffMaker = new TDiffMaker();
            DiffCollection<TDiff, TItem>[] baseToCollectionDiffs = diffMaker.MakeMultipleDiffs(
                baseCollection,
                priorityCollection,
                secondaryCollection);
            //DiffCollection<TDiff, TItem> baseToSecondaryDiffs = diffMaker.MakeDiffs(baseCollection, secondaryCollection);

            TDiffDiffMaker diffDiffMaker = new TDiffDiffMaker();

            this.results = diffDiffMaker.MakeDiffDiffs(
                baseToCollectionDiffs[0],
                baseToCollectionDiffs[1],
                DiffDiffType.OnlyChanged,
                this.priorityCollection,
                this.secondaryCollection);
        }
    }
}
