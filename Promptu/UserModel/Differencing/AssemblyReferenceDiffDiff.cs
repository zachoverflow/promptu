using System;
using System.Collections.Generic;
using System.Text;
using ZachJohnson.Promptu.UI;
using ZachJohnson.Promptu.UIModel;

namespace ZachJohnson.Promptu.UserModel.Differencing
{
    class AssemblyReferenceDiffDiff : DiffDiff<AssemblyReferenceDiff, AssemblyReference, AssemblyReferenceDiffDiff>
    {
        private DiffDiffEntry<string> name;
        private DiffDiffEntry<string> filepath;
        //private DiffDiffEntry<string> cachedName;
        private DiffDiffEntry<bool> ownedByUser;
        //private AssemblyReferenceCollection resolveTo;

        public AssemblyReferenceDiffDiff(
            AssemblyReferenceDiff priorityDiff, 
            AssemblyReferenceDiff secondaryDiff)
            : base(priorityDiff, secondaryDiff)
        {
            if (priorityDiff == null && secondaryDiff == null)
            {
                throw new ArgumentException("'priorityDiff' and 'secondaryDiff' cannot both be null.");
            }
            else if (priorityDiff != null && secondaryDiff != null && priorityDiff.BaseItem != secondaryDiff.BaseItem)
            {
                throw new ArgumentException("The two diffs do not share the same base.  Cannot diff diffs of difference bases.");
            }

            this.name = new DiffDiffEntry<string>(
                priorityDiff == null ? null : priorityDiff.Name,
                secondaryDiff == null ? null : secondaryDiff.Name);

            this.filepath = new DiffDiffEntry<string>(
                priorityDiff == null ? null : priorityDiff.Filepath,
                secondaryDiff == null ? null : secondaryDiff.Filepath);

            //this.cachedName = new DiffDiffEntry<string>(
            //    priorityDiff == null ? null : priorityDiff.CachedName,
            //    secondaryDiff == null ? null : secondaryDiff.CachedName);

            this.ownedByUser = new DiffDiffEntry<bool>(
                priorityDiff == null ? null : priorityDiff.OwnedByUser,
                secondaryDiff == null ? null : secondaryDiff.OwnedByUser);

            if (priorityDiff == null)
            {
                this.HasConflictingChanges = false;
                this.HasChanges = secondaryDiff.HasChanges;
            }
            else if (secondaryDiff == null)
            {
                this.HasConflictingChanges = false;
                this.HasChanges = priorityDiff.HasChanges;
            }
            else if (priorityDiff.Status != DiffStatus.Default)
            {
                this.HasChanges = secondaryDiff.HasChanges;
                this.HasConflictingChanges = this.HasChanges;
            }
            else if (secondaryDiff.Status != DiffStatus.Default)
            {
                this.HasChanges = priorityDiff.HasChanges;
                this.HasConflictingChanges = this.HasChanges;
            }
            else
            {
                this.HasConflictingChanges = secondaryDiff.HasChanges && priorityDiff.HasChanges;
                this.HasChanges = secondaryDiff.HasChanges || priorityDiff.HasChanges;
            }
            //this.HasConflictingChanges = this.name.IsConflictingChange
            //    || this.filepath.IsConflictingChange
            //    || this.cachedName.IsConflictingChange
            //    || this.ownedByUser.IsConflictingChange;
        }

        public DiffDiffEntry<string> Name
        {
            get { return this.name; }
        }

        public DiffDiffEntry<string> Filepath
        {
            get { return this.filepath; }
        }

        //public DiffDiffEntry<string> CachedName
        //{
        //    get { return this.cachedName; }
        //}

        public DiffDiffEntry<bool> OwnedByUser
        {
            get { return this.ownedByUser; }
        }

        protected override ObjectConflictInfo GetDisplayCore(DiffVersion diffVersion, AssemblyReferenceDiff diff)
        {
            //List<ItemCompareEntry> entries = new List<ItemCompareEntry>();
            //StringBuilder builder = new StringBuilder();
            ObjectConflictInfo info = new ObjectConflictInfo(new ItemCompareEntry(
                diff.RevisedItem.Name,
                this.GetCorrectState(this.name.HasChanges)),
                ConflictObjectType.AssemblyReference);

            if (diff.RevisedItem.OwnedByUser)
            {
                //builder.AppendLine(this.Filepath.GetDiffEntry(diffVersion).RevisedValue);
                info.Attributes.Add(new ItemCompareEntry(
                    diff.RevisedItem.Filepath,
                    this.GetCorrectState(this.OwnedByUser.HasChanges)));
            }
            else
            {
                //builder.AppendLine(Localization.UIResources.ExternallySharedReference);
                info.Attributes.Add(new ItemCompareEntry(
                    Localization.UIResources.ExternallySharedReference,
                    this.GetCorrectState(this.OwnedByUser.HasChanges)));
            }

            //entries.Add(new GraphicalTextEntry(
            //    diff.RevisedItem.Name,
            //    PromptuFonts.EntryNameFont,
            //    this.GetCorrectColor(this.name.HasChanges)));

            //if (diff.RevisedItem.OwnedByUser)
            //{
            //    //builder.AppendLine(this.Filepath.GetDiffEntry(diffVersion).RevisedValue);
            //    entries.Add(new GraphicalTextEntry(
            //        diff.RevisedItem.Filepath,
            //        PromptuFonts.EntryDetailsFont,
            //        this.GetCorrectColor(this.OwnedByUser.HasChanges)));
            //}
            //else
            //{
            //    //builder.AppendLine(Localization.UIResources.ExternallySharedReference);
            //    entries.Add(new GraphicalTextEntry(
            //        Localization.UIResources.ExternallySharedReference,
            //        PromptuFonts.EntryDetailsFont,
            //        this.GetCorrectColor(this.OwnedByUser.HasChanges)));
            //}

            return info;
        }

        //protected override VisualDisplayInfo GetInfoForPriorityCore()
        //{
        //    AssemblyReferenceDiff diff = this.GetDiff(DiffVersion.Priority);
        //    if (diff != null)
        //    {
        //        return new VisualDisplayInfo(this.GetDisplayCore(DiffVersion.Priority, diff));
        //    }
        //    else
        //    {
        //        return new VisualDisplayInfo(this.GetNotPresentTextEntry());
        //    }
        //}

        //protected override VisualDisplayInfo GetInfoForSecondaryCore()
        //{
        //    AssemblyReferenceDiff diff = this.GetDiff(DiffVersion.Secondary);
        //    if (diff != null)
        //    {
        //        return new VisualDisplayInfo(this.GetDisplayCore(DiffVersion.Secondary, diff));
        //    }
        //    else
        //    {
        //        return new VisualDisplayInfo(this.GetNotPresentTextEntry());
        //    }
        //}

        protected override string GetObjectTypeNameCore()
        {
            return Localization.UIResources.AssemblyReferenceTypeName;
        }

        protected override ConflictObjectType GetConflictObjectType()
        {
            return ConflictObjectType.AssemblyReference;
        }
    }
}
