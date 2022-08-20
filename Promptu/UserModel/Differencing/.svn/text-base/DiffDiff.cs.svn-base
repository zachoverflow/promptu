using System;
using System.Collections.Generic;
using System.Text;
using ZachJohnson.Promptu.UI;
using System.Drawing;
using ZachJohnson.Promptu.UIModel;

namespace ZachJohnson.Promptu.UserModel.Differencing
{
    internal abstract class DiffDiff<TDiff, TItem, TDiffDiff> : DiffDiffBase
        where TDiffDiff : DiffDiff<TDiff, TItem, TDiffDiff>
        where TDiff : Diff<TItem, TDiff>
        where TItem : IDiffable
    {
        private TDiff priorityDiff;
        private TDiff secondaryDiff;
        private List<TItem> priorityDiffIdentifierConflicts;
        private List<TItem> secondaryDiffIdentifierConflicts;
        private IChangeResolver<TDiffDiff> resolver;

        public DiffDiff(TDiff priorityDiff, TDiff secondaryDiff)
        {
            this.priorityDiff = priorityDiff;
            this.secondaryDiff = secondaryDiff;
            this.secondaryDiffIdentifierConflicts = new List<TItem>();
            this.priorityDiffIdentifierConflicts = new List<TItem>();
        }

        public ObjectConflictInfo GetDisplay(DiffVersion diffVersion)
        {
            TDiff diff = this.GetDiff(diffVersion);
            if (diff == null)
            {
                return new ObjectConflictInfo(new ItemCompareEntry(String.Empty, EntryState.Normal), this.GetConflictObjectType());
            }
            else
            {
                return this.GetDisplayCore(diffVersion, diff);
            }
        }

        public IChangeResolver<TDiffDiff> Resolver
        {
            get { return this.resolver; }
            set { this.resolver = value; }
        }

        public bool ConflictingIdentifiersCouldExistForPriority
        {
            get { return this.GetConflictingIdentifiersCouldExistCore(DiffVersion.Priority); }
        }

        public bool ConflictingIdentifiersCouldExistForSecondary
        {
            get { return this.GetConflictingIdentifiersCouldExistCore(DiffVersion.Secondary); }
        }

        public void ClearResolvedConflictingIdentifiers(List<TDiffDiff> diffDiffs)
        {
            foreach (TDiffDiff diffDiff in diffDiffs)
            {
                if (diffDiff == this)
                {
                    continue;
                }

                if (diffDiff.HasConflictingChanges == false)
                {
                    if (diffDiff.PriorityDiff != null && diffDiff.SecondaryDiff != null)
                    {
                        if (diffDiff.PriorityDiff.Status == DiffStatus.Deleted && diffDiff.SecondaryDiff.BaseItem != null)
                        {
                            List<TItem> conflictsToRemove = new List<TItem>();
                            foreach (TItem conflict in this.PriorityDiffIdentifierConflicts)
                            {
                                if (conflict.Id == diffDiff.SecondaryDiff.BaseItem.Id)
                                {
                                    conflictsToRemove.Add(conflict);
                                }
                            }

                            foreach (TItem conflict in conflictsToRemove)
                            {
                                this.PriorityDiffIdentifierConflicts.Remove(conflict);
                            }
                        }

                        if (diffDiff.SecondaryDiff.Status == DiffStatus.Deleted && diffDiff.PriorityDiff.BaseItem != null)
                        {
                            List<TItem> conflictsToRemove = new List<TItem>();
                            foreach (TItem conflict in this.SecondaryDiffIdentifierConflicts)
                            {
                                if (conflict.Id == diffDiff.PriorityDiff.BaseItem.Id)
                                {
                                    conflictsToRemove.Add(conflict);
                                }
                            }

                            foreach (TItem conflict in conflictsToRemove)
                            {
                                this.SecondaryDiffIdentifierConflicts.Remove(conflict);
                            }

                        }
                    }
                }
            }
        }

        //public void ResolveTo(DiffVersion diffVersion)
        //{
        //    //this.ResolveToCore(diffVersion); 
        //}

        public TDiff PriorityDiff
        {
            get { return this.priorityDiff; }
        }

        public TDiff SecondaryDiff
        {
            get { return this.secondaryDiff; }
        }

        public List<TItem> PriorityDiffIdentifierConflicts
        {
            get { return this.priorityDiffIdentifierConflicts; }
        }

        public List<TItem> SecondaryDiffIdentifierConflicts
        {
            get { return this.secondaryDiffIdentifierConflicts; }
        }

        public TDiff GetChanged(out DiffVersion versionSelected)
        {
            if (this.priorityDiff != null)
            {
                if (this.priorityDiff.HasChanges)
                {
                    versionSelected = DiffVersion.Priority;
                    return this.priorityDiff;
                }
            }

            if (this.secondaryDiff != null)
            {
                if (this.secondaryDiff.HasChanges)
                {
                    versionSelected = DiffVersion.Secondary;
                    return this.secondaryDiff;
                }
            }

            versionSelected = default(DiffVersion);
            return null;
        }

        public TDiff GetDiff(DiffVersion version)
        {
            if (version == DiffVersion.Secondary)
            {
                return this.secondaryDiff;
            }
            else
            {
                return this.priorityDiff;
            }
        }

        protected abstract ObjectConflictInfo GetDisplayCore(DiffVersion diffVersion, TDiff diff);

        protected override bool GetImplicitlyResolved()
        {
            if (this.resolver != null)
            {
                Id id;
                if (this.priorityDiff != null)
                {
                    id = this.priorityDiff.GetId();
                }
                else
                {
                    id = this.secondaryDiff.GetId();
                }

                return this.resolver.GetWhetherImplicitlyResolved(id);
            }

            return false;
        }

        //protected abstract string ResolveToCore(DiffVersion diffVersion);

        protected ItemCompareEntry GetNotPresentTextEntry()
        {
            return new ItemCompareEntry(
                Localization.UIResources.NotPresentInList,
                this.GetCorrectState(true));
        }

        protected ItemCompareEntry GetDeletedTextEntry()
        {
            return new ItemCompareEntry(
                Localization.UIResources.Deleted,
                this.GetCorrectState(true));
        }

        protected EntryState GetCorrectState(bool changed)
        {
            //if (diffDiffEntry == null)
            //{
            //    throw new ArgumentNullException("diffDiffEntry");
            //}

            if (changed)
            {
                return EntryState.Error;//PromptuColors.ConflictingChangeColor;
            }
            else
            {
                return EntryState.Normal;//PromptuColors.NoConflictColor;
            }
        }

        protected bool GetConflictingIdentifiersCouldExistCore(DiffVersion version)
        {
            TDiff diff;
            if (version == DiffVersion.Priority)
            {
                diff = this.PriorityDiff;
                
            }
            else
            {
                diff = this.SecondaryDiff;
            }

            bool conflictingIdentifiersCouldExist = false;

            if (diff != null)
            {
                switch (diff.Status)
                {
                    case DiffStatus.New:
                        conflictingIdentifiersCouldExist = true;
                        break;
                    case DiffStatus.Deleted:
                        break;
                    default:
                        conflictingIdentifiersCouldExist = diff.GetIdentifierHasChanged();
                        break;
                }
            }

            return conflictingIdentifiersCouldExist;
            //else
            //{
            //    return this.SecondaryDiff.Name.HasChanged;
            //}
        }

        protected override void ResolveToCore(DiffVersion version)
        {
            if (this.resolver == null)
            {
                throw new ResolverMissingException("The resolver is missing.  Please set a resolver before calling this method.");
            }

            //TDiff diff = this.GetDiff(version);

            //if (diff == null)
            //{
            //    throw new ArgumentException("The supplied version does not have a corresponding diff.");
            //}

            this.resolver.ResolveToRevised((TDiffDiff)this, version);
        }

        protected override ObjectConflictInfo GetInfoForPriorityCore()
        {
            TDiff diff = this.GetDiff(DiffVersion.Priority);
            if (diff != null)
            {
                if (diff.Status == DiffStatus.Deleted)
                {
                    return new ObjectConflictInfo(this.GetDeletedTextEntry(), this.GetConflictObjectType());
                }
                else
                {
                    return this.GetDisplayCore(DiffVersion.Priority, diff);
                }
            }
            else
            {
                return new ObjectConflictInfo(this.GetNotPresentTextEntry(), this.GetConflictObjectType());
            }
        }

        protected override ObjectConflictInfo GetInfoForSecondaryCore()
        {
            TDiff diff = this.GetDiff(DiffVersion.Secondary);
            if (diff != null)
            {
                if (diff.Status == DiffStatus.Deleted)
                {
                    return new ObjectConflictInfo(this.GetDeletedTextEntry(), this.GetConflictObjectType());
                }
                else
                {
                    return this.GetDisplayCore(DiffVersion.Secondary, diff);
                }
            }
            else
            {
                return new ObjectConflictInfo(this.GetNotPresentTextEntry(), this.GetConflictObjectType());
            }
        }

        protected override int GetPriorityDiffIdentifierConflictsCount()
        {
            return this.priorityDiffIdentifierConflicts.Count;
        }

        protected override int GetSecondaryDiffIdentifierConflictsCount()
        {
            return this.secondaryDiffIdentifierConflicts.Count;
        }

        protected override VisualDisplayInfo GetInfoForPriorityIdentifierConflictsCore()
        {
            return this.GetInfoForIdentifierConflicts(DiffVersion.Priority);
        }

        protected override VisualDisplayInfo GetInfoForSecondaryIdentifierConflictsCore()
        {
            return this.GetInfoForIdentifierConflicts(DiffVersion.Secondary);
        }

        protected abstract ConflictObjectType GetConflictObjectType();

        protected virtual VisualDisplayInfo GetInfoForIdentifierConflicts(DiffVersion version)
        {
            List<ItemCompareEntry> graphicalEntries = new List<ItemCompareEntry>();
            List<TItem> identifierConflicts;
            if (version == DiffVersion.Priority)
            {
                identifierConflicts = this.PriorityDiffIdentifierConflicts;
            }
            else
            {
                identifierConflicts = this.SecondaryDiffIdentifierConflicts;
            }

            foreach (TItem item in identifierConflicts)
            {
                graphicalEntries.Add(new ItemCompareEntry(
                    item.GetFormattedIdentifier(), 
                    EntryState.Normal));
            }

            return new VisualDisplayInfo(graphicalEntries);
        }
    }
}
