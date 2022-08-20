using System;
using System.Collections.Generic;
using System.Text;
using ZachJohnson.Promptu.Collections;
using ZachJohnson.Promptu.UserModel.Collections;

namespace ZachJohnson.Promptu.UserModel.Differencing
{
    internal abstract class DiffMaker<TItem, TDiff, TItemCollection> 
        where TDiff : Diff<TItem, TDiff>
        where TItem : class, IHasId
        where TItemCollection : IIndexedCollection<TItem>, IItemsWithIdList<TItem>
    {
        public TDiff MakeDiffFromItem(
            TItem item,
            ItemType itemType, 
            TItemCollection collection)
        {
            return this.MakeMultipleDiffsFromItem(item, itemType, collection)[0];
        }

        //public TDiff MakeDiffFromLatest(TItemToDiff latestItem, TItemCollection collection)
        //{
        //    return this.MakeMultipleDiffsFromLatest(latestItem, collection)[0];
        //}

        public TDiff[] MakeMultipleDiffsFromItem(
            TItem item, 
            ItemType itemType,
            params TItemCollection[] collections)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }
            else if (collections == null)
            {
                throw new ArgumentNullException("collections");
            }

            TDiff[] items = new TDiff[collections.Length];

            for (int i = 0; i < collections.Length; i++)
            {
                int? index;
                //TItem baseItem;

                //if (itemIdentifierChanges != null)
                //{
                //    TIdentifierChange change = itemIdentifierChanges.TryGetIdentifierChangeFromRevisedItem(item);

                //    if (change != null)
                //    {
                //        iItem = change.CreateFilter(itemType);
                //    }
                //    else
                //    {
                //        iItem = item;
                //    }
                //}
                //else
                //{
                //    iItem = item;
                //}

                //TIdentifierChangeCollection collectionIdentifierChanges;
                //if (collectionsIdentifierChanges != null && i < collectionsIdentifierChanges.Length)
                //{
                //    collectionIdentifierChanges = collectionsIdentifierChanges[i];
                //}
                //else
                //{
                //    collectionIdentifierChanges = null;
                //}

                //TItem otherItem = this.GetCorrespondingItemFrom(iItem, collections[i], collectionIdentifierChanges, out index);
                TItem otherItem = collections[i].TryGet(item.Id, out index);


                if (itemType == ItemType.Revised)
                {
                    items[i] = this.CreateDiff(otherItem, item);
                }
                else
                {
                    items[i] = this.CreateDiff(item, otherItem);
                }
            }

            return items;
        }

        //public TDiff[] MakeMultipleDiffsFromLatest(TItemToDiff latestItem, params TItemCollection[] baseCollections)
        //{
        //    if (latestItem == null)
        //    {
        //        throw new ArgumentNullException("latestItem");
        //    }
        //    else if (baseCollections == null)
        //    {
        //        throw new ArgumentNullException("baseCollections");
        //    }

        //    return this.MakeMultipleDiffsFromLatestCore(latestItem, baseCollections);
        //}

        public DiffCollection<TDiff, TItem> MakeDiffs(
            TItemCollection baseCollection,
            TItemCollection revisedCollection)
        {
            return this.MakeMultipleDiffs(baseCollection, revisedCollection)[0];
        }

        public DiffCollection<TDiff, TItem>[] MakeMultipleDiffs(
            TItemCollection baseCollection,
            params TItemCollection[] revisedCollections)
        {
            if (baseCollection == null)
            {
                throw new ArgumentNullException("baseCollection");
            }
            else if (revisedCollections == null)
            {
                throw new ArgumentNullException("revisedCollections");
            }

            DiffCollection<TDiff, TItem>[] diffs = new DiffCollection<TDiff, TItem>[revisedCollections.Length];
            List<int>[] diffedIndexes = new List<int>[revisedCollections.Length];

            for (int i = 0; i < revisedCollections.Length; i++)
            {
                diffedIndexes[i] = new List<int>();
                diffs[i] = new DiffCollection<TDiff, TItem>();
            }

            for (int i = 0; i < baseCollection.Count; i++)
            {
                TItem baseItem = baseCollection[i];
                //TIItem baseIItem;

                //if (baseIdentifierChanges != null)
                //{
                //    TIdentifierChange change = baseIdentifierChanges.TryGetIdentifierChangeFromRevisedItem(baseItem);

                //    if (change != null)
                //    {
                //        baseIItem = change.CreateFilter(ItemType.Revised);
                //    }
                //    else
                //    {
                //        baseIItem = baseItem;
                //    }
                //}
                //else
                //{
                //    baseIItem = baseItem;
                //}

                for (int j = 0; j < revisedCollections.Length; j++)
                {

                    //Command latestItem = null;
                    //CommandCollection commandCollection = latestCollections[j];
                    //if (commandCollection.Contains(baseCommand.Name))
                    //{
                    //    latestItem = commandCollection[baseCommand.Name];
                    //    diffedIndexes[j].Add(commandCollection.IndexOf(latestItem));
                    //}

                    //TIdentifierChangeCollection revisedIdentifierChanges;
                    //if (revisedCollectionsIdentifierChanges != null && j < revisedCollectionsIdentifierChanges.Length)
                    //{
                    //    revisedIdentifierChanges = revisedCollectionsIdentifierChanges[j];
                    //}
                    //else
                    //{
                    //    revisedIdentifierChanges = null;
                    //}

                    int? index;
                    //TItem revisedItem = this.GetCorrespondingItemFrom(baseIItem, revisedCollections[j], revisedIdentifierChanges, out index);
                    TItem revisedItem = revisedCollections[j].TryGet(baseItem.Id, out index);


                    if (index != null)
                    {
                        diffedIndexes[j].Add(index.Value);
                    }

                    TDiff diff = this.CreateDiff(baseItem, revisedItem);
                    diffs[j].Add(diff);
                }
            }

            for (int i = 0; i < revisedCollections.Length; i++)
            {
                DiffCollection<TDiff, TItem> diffsWithThisCollection = diffs[i];
                TItemCollection collection = revisedCollections[i];
                List<int> diffedIndexesForThisCollection = diffedIndexes[i];

                if (diffedIndexesForThisCollection.Count != collection.Count)
                {
                    for (int j = 0; j < collection.Count; j++)
                    {
                        if (!diffedIndexesForThisCollection.Contains(j))
                        {
                            diffsWithThisCollection.Add(this.CreateDiff(null, collection[j]));
                        }
                    }
                }
            }

            return diffs;
        }

        //protected abstract TDiff MakeDiffFromBaseCore(TItemToDiff baseItem, TItemCollection collection);

        //protected abstract TDiff MakeDiffFromLatestCore(TItemToDiff baseItem, TItemCollection collection);

        //protected abstract TDiff[] MakeMultipleDiffsFromItemCore(TItem item, ItemIs itemIs, params TItemCollection[] latestCollections);

        //protected abstract DiffCollection<TDiff>[] MakeMultipleDiffsCore(TItemCollection baseCollection, params TItemCollection[] latestCollections);

        //protected abstract TItem GetCorrespondingItemFrom(TItem item, TItemCollection collection);

        //protected abstract TItem GetCorrespondingItemFrom(TIItem item, TItemCollection collection, TIdentifierChangeCollection identifierChanges, out int? index);

        protected abstract TDiff CreateDiff(TItem baseItem, TItem latestItem);
    }
}
