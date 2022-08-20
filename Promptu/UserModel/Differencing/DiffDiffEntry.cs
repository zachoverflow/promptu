using System;
using System.Collections.Generic;
using System.Text;

namespace ZachJohnson.Promptu.UserModel.Differencing
{
    internal class DiffDiffEntry<T> : DiffDiffEntryBase
    {
        private DiffEntry<T> priorityDiffEntry;
        private DiffEntry<T> secondaryDiffEntry;

        public DiffDiffEntry(DiffEntry<T> priorityDiffEntry, DiffEntry<T> secondaryDiffEntry)
        {
            //if (priorityDiffEntry == null)
            //{
            //    throw new ArgumentNullException("priorityDiffEntry");
            //}
            //else if (secondaryDiffEntry == null)
            //{
            //    throw new ArgumentNullException("secondaryDiffEntry");
            //}

            this.priorityDiffEntry = priorityDiffEntry;
            this.secondaryDiffEntry = secondaryDiffEntry;

            if (this.priorityDiffEntry == null)
            {
                this.HasConflictingChanges = this.secondaryDiffEntry == null || this.secondaryDiffEntry.HasChanged;
                this.HasChanges = this.HasConflictingChanges;
            }
            else if (this.secondaryDiffEntry == null)
            {
                this.HasConflictingChanges = this.priorityDiffEntry.HasChanged;
                this.HasChanges = this.priorityDiffEntry.HasChanged;
            }
            else if (this.priorityDiffEntry.HasChanged && this.secondaryDiffEntry.HasChanged)
            {
                this.HasConflictingChanges = this.priorityDiffEntry.ValueComparison(this.priorityDiffEntry.RevisedValue, this.secondaryDiffEntry.RevisedValue) != 0;
                this.HasChanges = this.priorityDiffEntry.HasChanged || this.secondaryDiffEntry.HasChanged;
            }
            else
            {
                this.HasChanges = this.priorityDiffEntry.HasChanged || this.secondaryDiffEntry.HasChanged;
            }
        }

        public DiffEntry<T> PriorityDiffEntry
        {
            get { return this.priorityDiffEntry; }
        }

        public DiffEntry<T> SecondaryDiffEntry
        {
            get { return this.secondaryDiffEntry; }
        }

        public DiffEntry<T> GetDiffEntry(DiffVersion diffVersion)
        {
            if (diffVersion == DiffVersion.Secondary)
            {
                return this.SecondaryDiffEntry;
            }
            else
            {
                return this.PriorityDiffEntry;
            }
        }
    }
}
