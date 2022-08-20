using System;
using System.Collections.Generic;
using System.Text;
using ZachJohnson.Promptu.UserModel.Collections;

namespace ZachJohnson.Promptu.UserModel.Differencing
{
    internal abstract class DiffDiffMaker<TDiffDiff, TDiff, TItem, TItemCollection> 
        where TDiff : Diff<TItem, TDiff>
        where TDiffDiff : DiffDiff<TDiff, TItem, TDiffDiff>
        where TItem : IDiffable
        where TItemCollection : IItemsWithIdList<TItem>
    {
        public DiffDiffMaker()
        {
        }

        //public TDiffDiff MakeDiffDiff(TDiff priorityDiff, TDiff secondaryDiff, TItem priorityDiffIdentifierConflict, TItem secondaryDiffIdentifierConflict)
        //{
        //    return this.CreateDiffDiff(priorityDiff, secondaryDiff, priorityDiffIdentifierConflict, secondaryDiffIdentifierConflict);
        //}

        //public TDiffDiff MakeDiffDiff(TDiff diff, ItemType itemType, DiffCollection<TDiff, TItem> diffs, TItemCollection )
        //{
        //    if (diff == null)
        //    {
        //        throw new ArgumentNullException("diff");
        //    }

        //    if (itemType == ItemType.Base)
        //    {
        //        return this.CreateDiffDiff(diff, diffs.TryGetSimilar(diff));
        //    }
        //    else
        //    {
        //        return this.CreateDiffDiff(diffs.TryGetSimilar(diff), diff);
        //    }
        //}

        public List<TDiffDiff> MakeDiffDiffs(
            DiffCollection<TDiff, TItem> priorityDiffs,
            DiffCollection<TDiff, TItem> secondaryDiffs,
            DiffDiffType diffDiffType,
            TItemCollection priorityRevisedCollection,
            TItemCollection secondaryRevisedCollection)
        {
            List<TDiffDiff> diffs = new List<TDiffDiff>();

            List<int> latestDiffUsedIndexes = new List<int>();
            foreach (TDiff diff in priorityDiffs)
            {
                int? index;
                TDiffDiff diffDiff = this.CreateDiffDiff(diff, secondaryDiffs.TryGetSimilar(diff, out index));
                //if ((diffDiffType == DiffDiffType.OnlyChanged && diffDiff.HasChanges)
                //    || (diffDiffType == DiffDiffType.OnlyConflicting && diffDiff.HasConflictingChanges)
                //    || diffDiffType == DiffDiffType.All)
                //{
                    if (diffDiff.ConflictingIdentifiersCouldExistForPriority)
                    {
                        diffDiff.PriorityDiffIdentifierConflicts.AddRange(secondaryRevisedCollection.GetConflictsWith(diffDiff.PriorityDiff.RevisedItem));
                    }

                    if (diffDiff.ConflictingIdentifiersCouldExistForSecondary)
                    {
                        diffDiff.SecondaryDiffIdentifierConflicts.AddRange(priorityRevisedCollection.GetConflictsWith(diffDiff.SecondaryDiff.RevisedItem));
                    }

                    diffs.Add(diffDiff);
                //}

                if (index != null)
                {
                    latestDiffUsedIndexes.Add(index.Value);
                }
            }

            if (latestDiffUsedIndexes.Count != secondaryDiffs.Count)
            {
                for (int i = 0; i < secondaryDiffs.Count; i++)
                {
                    if (!latestDiffUsedIndexes.Contains(i))
                    {
                        TDiffDiff diffDiff = this.CreateDiffDiff(null, secondaryDiffs[i]);
                        //if ((diffDiffType == DiffDiffType.OnlyChanged && diffDiff.HasChanges)
                        //    || (diffDiffType == DiffDiffType.OnlyConflicting && diffDiff.HasConflictingChanges) 
                        //    || diffDiffType == DiffDiffType.All)
                        //{
                            diffs.Add(diffDiff);
                        //}
                    }
                }
            }

            List<TDiffDiff> realDiffs = new List<TDiffDiff>();

            foreach (TDiffDiff diffDiff in diffs)
            {
                if ((diffDiffType == DiffDiffType.OnlyChanged && diffDiff.HasChanges)
                    || (diffDiffType == DiffDiffType.OnlyConflicting && diffDiff.HasConflictingChanges)
                    || diffDiffType == DiffDiffType.All)
                {
                    diffDiff.ClearResolvedConflictingIdentifiers(diffs);
                    realDiffs.Add(diffDiff);
                }
            }

            return realDiffs;
        }

        protected abstract TDiffDiff CreateDiffDiff(TDiff priorityDiff, TDiff secondaryDiff);
    }
}
