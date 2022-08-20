using System;
using System.Collections.Generic;
using System.Text;
using ZachJohnson.Promptu.UserModel;

namespace ZachJohnson.Promptu.UserModel.Differencing
{
    internal class AssemblyReferenceDiff : Diff<AssemblyReference, AssemblyReferenceDiff>
    {
        private DiffEntry<string> name;
        private DiffEntry<string> filepath;
        //private DiffEntry<string> cachedName;
        private DiffEntry<bool> ownedByUser;

        public AssemblyReferenceDiff(AssemblyReference baseItem, AssemblyReference revisedItem)
            : base(baseItem, revisedItem)
        {
            if (baseItem == null && revisedItem == null)
            {
                throw new ArgumentNullException("'baseItem' and 'revisedItem' cannot both be null.");
            }
            else if (baseItem == null)
            {
                this.Status = DiffStatus.New;
                this.HasChanges = true;
            }
            else if (revisedItem == null)
            {
                this.Status = DiffStatus.Deleted;
                this.HasChanges = true;
            }
            else
            {
                this.name = new DiffEntry<string>(
                    baseItem.Name,
                    revisedItem.Name,
                    ValueComparisons.StringComparison);
                this.filepath = new DiffEntry<string>(
                    baseItem.Filepath,
                    revisedItem.Filepath,
                    ValueComparisons.StringComparison);
                //this.cachedName = new DiffEntry<string>(
                //    baseItem.CachedName,
                //    revisedItem.CachedName,
                //    ValueComparisons.StringComparison);
                this.ownedByUser = new DiffEntry<bool>(
                    baseItem.OwnedByUser,
                    revisedItem.OwnedByUser,
                    ValueComparisons.BooleanComparison);
                this.HasChanges = this.name.HasChanged
                    || this.filepath.HasChanged
                    //|| this.cachedName.HasChanged
                    || this.ownedByUser.HasChanged;
            }
        }

        public DiffEntry<string> Name
        {
            get { return this.name; }
        }

        public DiffEntry<string> Filepath
        {
            get { return this.filepath; }
        }

        //public DiffEntry<string> CachedName
        //{
        //    get { return this.cachedName; }
        //}

        public DiffEntry<bool> OwnedByUser
        {
            get { return this.ownedByUser; }
        }

        protected override bool ConflictsWithCore(AssemblyReferenceDiff diff)
        {
            if (this.RevisedItem != null && diff.RevisedItem != null)
            {
                return this.RevisedItem.Name.ToUpperInvariant() == diff.RevisedItem.Name.ToUpperInvariant();
            }

            return false;
        }

        //protected override bool IsSimilarToCore(AssemblyReferenceDiff diff)
        //{
        //    string thisName;
        //    string diffName;

        //    if (this.Status == DiffStatus.Deleted)
        //    {
        //        thisName = this.BaseItem.Name;
        //    }
        //    else
        //    {
        //        thisName = this.RevisedItem.Name;
        //    }

        //    if (diff.Status == DiffStatus.Deleted)
        //    {
        //        diffName = diff.BaseItem.Name;
        //    }
        //    else
        //    {
        //        diffName = diff.RevisedItem.Name;
        //    }

        //    return thisName == diffName;
        //}

        protected override bool GetIdentifierHasChangedCore()
        {
            return this.Name.HasChanged;
        }
    }
}
