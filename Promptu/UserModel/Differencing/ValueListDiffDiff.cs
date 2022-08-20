using System;
using System.Collections.Generic;
using System.Text;
using ZachJohnson.Promptu.UI;
using ZachJohnson.Promptu.UIModel;
using System.Globalization;

namespace ZachJohnson.Promptu.UserModel.Differencing
{
    internal class ValueListDiffDiff : DiffDiff<ValueListDiff, ValueList, ValueListDiffDiff>
    {
        private DiffDiffEntry<string> name;
        private DiffDiffEntry<bool> useNamespaceInterpretation;
        private DiffDiffEntry<bool> useItemTranslations;
        private List<DiffDiffEntry<ValueListItem>> items = new List<DiffDiffEntry<ValueListItem>>();
        //private bool itemsHaveChanges;

        public ValueListDiffDiff(ValueListDiff priorityDiff, ValueListDiff secondaryDiff)
            : base(priorityDiff, secondaryDiff)
        {
            if (priorityDiff == null && secondaryDiff == null)
            {
                throw new ArgumentException("'priorityDiff' and 'secondaryDiff' cannot both be null.");
            }
            else if (priorityDiff != null && secondaryDiff != null && priorityDiff.BaseItem != secondaryDiff.BaseItem)
            {
                // TODO got here
                throw new ArgumentException("The two diffs do not share the same base.  Cannot diff diffs of difference bases.");
            }

            this.name = new DiffDiffEntry<string>(
                priorityDiff == null ? null : priorityDiff.Name,
                secondaryDiff == null ? null : secondaryDiff.Name);

            this.useItemTranslations = new DiffDiffEntry<bool>(
                priorityDiff == null ? null : priorityDiff.UseItemTranslations,
                secondaryDiff == null ? null : secondaryDiff.UseItemTranslations);

            this.useNamespaceInterpretation = new DiffDiffEntry<bool>(
                priorityDiff == null ? null : priorityDiff.UseItemTranslations,
                secondaryDiff == null ? null : secondaryDiff.UseItemTranslations);

            int largestParameterCount = 0;

            if (priorityDiff != null)
            {
                largestParameterCount = priorityDiff.Items.Count;
            }

            if (secondaryDiff != null && secondaryDiff.Items.Count > largestParameterCount)
            {
                largestParameterCount = secondaryDiff.Items.Count;
            }

            for (int i = 0; i < largestParameterCount; i++)
            {
                DiffEntry<ValueListItem> priorityItem = null;
                DiffEntry<ValueListItem> secondaryItem = null;

                if (priorityDiff != null && i < priorityDiff.Items.Count)
                {
                    priorityItem = priorityDiff.Items[i];
                }

                if (priorityDiff != null && i < secondaryDiff.Items.Count)
                {
                    secondaryItem = secondaryDiff.Items[i];
                }

                DiffDiffEntry<ValueListItem> diffDiffEntry = new DiffDiffEntry<ValueListItem>(priorityItem, secondaryItem);

                this.items.Add(diffDiffEntry);

                //if (diffDiffEntry.HasChanges)
                //{
                //    this.itemsHaveChanges = true;
                //}
            }

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
        }

        public DiffDiffEntry<string> Name
        {
            get { return this.name; }
        }

        public DiffDiffEntry<bool> UseNamespaceInterpretation
        {
            get { return this.useNamespaceInterpretation; }
        }

        public DiffDiffEntry<bool> UseItemTranslations
        {
            get { return this.useItemTranslations; }
        }

        public List<DiffDiffEntry<ValueListItem>> Items
        {
            get { return this.items; }
        }

        protected override ObjectConflictInfo GetDisplayCore(DiffVersion diffVersion, ValueListDiff diff)
        {
            ObjectConflictInfo info = new ObjectConflictInfo(new ItemCompareEntry(
                diff.RevisedItem.Name,
                this.GetCorrectState(this.name.HasChanges)),
                ConflictObjectType.ValueList);

            info.Attributes.Add(new ItemCompareEntry(String.Format(
                CultureInfo.CurrentCulture,
                Localization.UIResources.ValueListDetailsUseNamespaceInterpretationFormat,
                diff.RevisedItem.UseNamespaceInterpretation),
                this.GetCorrectState(this.UseNamespaceInterpretation.HasChanges)));

            info.Attributes.Add(new ItemCompareEntry(String.Format(
                CultureInfo.CurrentCulture,
                Localization.UIResources.ValueListDetailsUseItemTranslationsFormat,
                diff.RevisedItem.UseItemTranslations),
                this.GetCorrectState(this.UseItemTranslations.HasChanges)));

            if (diff.RevisedItem.Count == 1)
            {
                info.Attributes.Add(new ItemCompareEntry(
                    String.Format(CultureInfo.CurrentCulture, Localization.UIResources.ValueListItemsCountSingular, diff.RevisedItem.Count),
                    EntryState.Normal));
            }
            else
            {
                info.Attributes.Add(new ItemCompareEntry(
                    String.Format(CultureInfo.CurrentCulture, Localization.UIResources.ValueListItemsCountPlural, diff.RevisedItem.Count),
                    EntryState.Normal));
            }

            //info.Attributes.Add(new ItemCompareEntry(
            //    Localization.UIResources.ValueListDetailsItemsText,
            //    this.GetCorrectState(this.itemsHaveChanges)));

            //bool atLeastOneItemAdded = false;

            //if (diff.Items.Count > 0)
            //{
            //    for (int i = 0; i < diff.Items.Count; i++)
            //    {
            //        DiffEntry<ValueListItem> itemEntry = diff.Items[i];
            //        if (itemEntry.RevisedValue != null)
            //        {
            //            if (!atLeastOneItemAdded)
            //            {
            //                atLeastOneItemAdded = true;
            //            }

            //            StringBuilder itemTextualEntry = new StringBuilder();

            //            // I18N
            //            itemTextualEntry.AppendFormat("Value: '{0}'", itemEntry.RevisedValue.Value);

            //            if (diff.RevisedItem.UseItemTranslations)
            //            {
            //                itemTextualEntry.AppendFormat("; Translation '{0}'", itemEntry.RevisedValue.Translation);
            //            }

            //            info.Attributes.Add(new ItemCompareEntry(String.Format(
            //                "[{1}] {0}",
            //                itemTextualEntry,
            //                i + 1),
            //                this.GetCorrectState(this.itemsHaveChanges)));
            //        }
            //    }
            //}

            //if (!atLeastOneItemAdded)
            //{
            //    info.Attributes.Add(new ItemCompareEntry(
            //    Localization.UIResources.ValueListDetailsNoItems,
            //    this.GetCorrectState(this.itemsHaveChanges)));
            //}

            return info;
        }

        protected override ConflictObjectType GetConflictObjectType()
        {
            return ConflictObjectType.ValueList;
        }

        protected override string GetObjectTypeNameCore()
        {
            return Localization.UIResources.ValueListTypeName;
        }
    }
}
