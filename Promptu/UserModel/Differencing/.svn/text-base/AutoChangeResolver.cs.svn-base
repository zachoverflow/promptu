using System;
using System.Collections.Generic;
using System.Text;
using ZachJohnson.Promptu.Collections;
using ZachJohnson.Promptu.UserModel.Collections;

namespace ZachJohnson.Promptu.UserModel.Differencing
{
    internal abstract class AutoChangeResolver<TDiffDiff, TDiff, TItem, TItemCollection> : IChangeResolver<TDiffDiff>
        where TDiffDiff : DiffDiff<TDiff, TItem, TDiffDiff>
        where TDiff : Diff<TItem, TDiff>
        where TItemCollection : IIndexedCollection<TItem>, IItemsWithIdList<TItem>, new()
        where TItem : IDiffable
    {
        private TItemCollection synthesisCollection;
        private IdGenerator synthesisIdGenerator;
        private TItemCollection deletedIdentiferConflicts;
        private HistoryCollection history;
        private DiffVersion ignoreHistoryRemovalForVersion;

        public AutoChangeResolver(TItemCollection synthesisCollection, IdGenerator synthesisIdGenerator, HistoryCollection history, DiffVersion ignoreHistoryRemovalForVersion)
        {
            if (synthesisCollection == null)
            {
                throw new ArgumentNullException("synthesisCollection");
            }
            else if (synthesisIdGenerator == null)
            {
                throw new ArgumentNullException("synthesisIdGenerator");
            }

            this.synthesisIdGenerator = synthesisIdGenerator;
            this.ignoreHistoryRemovalForVersion = ignoreHistoryRemovalForVersion;
            this.history = history;
            this.synthesisCollection = synthesisCollection;
            this.deletedIdentiferConflicts = new TItemCollection();
        }

        public List<TDiffDiff> ResolveNonConflictingChangesAndPrepareConflictingForUI(
            List<TDiffDiff> changes, 
            IdGenerator priorityIdGenerator, 
            IdGenerator secondaryIdGenerator,
            bool forBothNewChangeSecondaryRatherThanPriority)
        {
            List<TDiffDiff> conflictingChanges = new List<TDiffDiff>();

            foreach (TDiffDiff changedItem in changes)
            {
                if (InternalGlobals.SyncSynchronizer.CancelSyncs)
                {
                    return conflictingChanges;
                }

                InternalGlobals.SyncSynchronizer.WaitIfPauseSyncs();

                if (changedItem.HasConflictingChanges || changedItem.HasChanges)
                {
                    //changedItem.PriorityDiffIdentifierConflicts.Clear();
                    //changedItem.SecondaryDiffIdentifierConflicts.Clear();
                    //changedItem.ClearResolvedConflictingIdentifiers(changes);
                    bool cannotResolveAutomatically = changedItem.PriorityDiffIdentifierConflictsCount > 0 || changedItem.SecondaryDiffIdentifierConflictsCount > 0;
                    if (changedItem.HasConflictingChanges || cannotResolveAutomatically)
                    {
                        bool resolveInUI = true;
                        if (!cannotResolveAutomatically)
                        {
                            if (changedItem.PriorityDiff.Status == DiffStatus.New && changedItem.SecondaryDiff.Status == DiffStatus.New)
                            {
                                if (!changedItem.PriorityDiff.ConflictsWith(changedItem.SecondaryDiff))
                                {
                                    resolveInUI = false;
                                    Id newId;
                                    if (priorityIdGenerator.Peek().Value > secondaryIdGenerator.Peek().Value)
                                    {
                                        newId = priorityIdGenerator.GenerateId();
                                    }
                                    else
                                    {
                                        newId = secondaryIdGenerator.GenerateId();
                                    }

                                    if (forBothNewChangeSecondaryRatherThanPriority)
                                    {
                                        changedItem.SecondaryDiff.RevisedItem.Id = newId;
                                    }
                                    else
                                    {
                                        changedItem.PriorityDiff.RevisedItem.Id = newId;
                                    }

                                    this.synthesisCollection.Remove(changedItem.PriorityDiff.GetId());
                                    this.AddCloneToCollection(changedItem.PriorityDiff.RevisedItem, this.synthesisCollection);
                                    this.AddCloneToCollection(changedItem.SecondaryDiff.RevisedItem, this.synthesisCollection);
                                }
                            }
                            else if (changedItem.PriorityDiff.Status == DiffStatus.Deleted && changedItem.SecondaryDiff.Status == DiffStatus.Deleted)
                            {
                                this.RemoveAllPossibleHistoryEntries(changedItem);
                                resolveInUI = false;
                                this.synthesisCollection.Remove(changedItem.PriorityDiff.GetId());
                            }
                        }

                        if (resolveInUI)
                        {
                            changedItem.Resolver = this;
                            conflictingChanges.Add(changedItem);
                        }
                    }
                    else if (changedItem.HasChanges)
                    {
                        DiffVersion version;
                        TDiff diffWithChanges = changedItem.GetChanged(out version);
                        if (diffWithChanges != null)
                        {
                            this.synthesisCollection.Remove(diffWithChanges.GetId());

                            if (version != this.ignoreHistoryRemovalForVersion)
                            {
                                this.RemoveAllPossibleHistoryEntries(changedItem);
                            }

                            if (diffWithChanges.Status != DiffStatus.Deleted)
                            {
                                this.AddCloneToCollection(diffWithChanges.RevisedItem, this.synthesisCollection);
                            }
                            else if (version == this.ignoreHistoryRemovalForVersion)
                            {
                                this.RemoveAllPossibleHistoryEntries(changedItem);
                            }
                        }
                    }
                }
            }

            priorityIdGenerator.Align(this.synthesisIdGenerator);
            secondaryIdGenerator.Align(this.synthesisIdGenerator);

            return conflictingChanges;
        }

        private void RemoveAllPossibleHistoryEntries(TDiffDiff diffDiff)
        {
            if (this.history == null)
            {
                return;
            }

            List<TItem> items = new List<TItem>();

            if (diffDiff.PriorityDiff != null)
            {
                if (diffDiff.PriorityDiff.BaseItem != null)
                {
                    items.Add(diffDiff.PriorityDiff.BaseItem);
                }

                if (diffDiff.PriorityDiff.RevisedItem != null)
                {
                    items.Add(diffDiff.PriorityDiff.RevisedItem);
                }
            }

            if (diffDiff.SecondaryDiff != null)
            {
                if (diffDiff.SecondaryDiff.BaseItem != null)
                {
                    items.Add(diffDiff.SecondaryDiff.BaseItem);
                }

                if (diffDiff.SecondaryDiff.RevisedItem != null)
                {
                    items.Add(diffDiff.SecondaryDiff.RevisedItem);
                }
            }

            foreach (TItem item in items)
            {
                this.RemoveItemEntriesFromHistory(this.history, item);
            }
        }

        public void PrepareForUI(List<TDiffDiff> changes)
        {
            foreach (TDiffDiff change in changes)
            {
                change.Resolver = this;
            }
        }

        protected abstract void RemoveItemEntriesFromHistory(HistoryCollection history, TItem item);

        protected abstract void AddCloneToCollection(TItem itemToClone, TItemCollection collection);

        public void ResolveToRevised(TDiffDiff diffDiff, DiffVersion version)
        {
            if (diffDiff.ImplicityResolved)
            {
                return;
            }

            TDiff diff = diffDiff.GetDiff(version);

            if (diff == null)
            {
                if (version == DiffVersion.Priority)
                {
                    diff = diffDiff.GetDiff(DiffVersion.Secondary);
                }
                else
                {
                    diff = diffDiff.GetDiff(DiffVersion.Priority);
                }

                if (diff != null)
                {
                    this.synthesisCollection.Remove(diff.GetId());
                }
                else
                {
                    throw new ArgumentException("Both PriorityDiff and SecondaryDiff of 'diffDiff' are null.");
                }

                return;
            }
            
            if (diff != null)
            {
                List<TItem> conflicts;
                if (version == DiffVersion.Priority)
                {
                    conflicts = diffDiff.PriorityDiffIdentifierConflicts;
                }
                else
                {
                    conflicts = diffDiff.SecondaryDiffIdentifierConflicts;
                }
                //this.RemoveCorrespondingFromCollection(diff.BaseItem, this.synthesisCollection);
                this.synthesisCollection.Remove(diff.GetId());

                if (conflicts != null)
                {
                    foreach (TItem conflict in conflicts)
                    {
                        this.synthesisCollection.Remove(conflict.Id);
                        this.deletedIdentiferConflicts.Add(conflict);
                    }
                }

                if (version != this.ignoreHistoryRemovalForVersion)
                {
                    this.RemoveAllPossibleHistoryEntries(diffDiff);
                }

                if (diff.Status != DiffStatus.Deleted)
                {
                    this.AddCloneToCollection(diff.RevisedItem, this.synthesisCollection);
                    this.synthesisIdGenerator.EnsureNextIdWillNotConflictWith(diff.RevisedItem.Id);
                }
                else if (version == this.ignoreHistoryRemovalForVersion)
                {
                    this.RemoveAllPossibleHistoryEntries(diffDiff);
                }
            }
        }

        public bool GetWhetherImplicitlyResolved(Id id)
        {
            return this.deletedIdentiferConflicts.TryGet(id) != null;
        }
    }
}
