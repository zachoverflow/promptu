using System;
using System.Collections.Generic;
using System.Text;

namespace ZachJohnson.Promptu.UserModel.Differencing
{
    internal class ValueListDiff : Diff<ValueList, ValueListDiff>
    {
        private DiffEntry<string> name;
        private DiffEntry<bool> useNamespaceInterpretation;
        private DiffEntry<bool> useItemTranslations;
        private List<DiffEntry<ValueListItem>> items = new List<DiffEntry<ValueListItem>>();
        private bool itemsHaveChanges;

        public ValueListDiff(ValueList baseItem, ValueList revisedItem)
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
                this.name = new DiffEntry<string>(baseItem.Name, revisedItem.Name, ValueComparisons.StringComparison);
                this.useItemTranslations = new DiffEntry<bool>(baseItem.UseItemTranslations, revisedItem.UseItemTranslations, ValueComparisons.BooleanComparison);
                this.useNamespaceInterpretation = new DiffEntry<bool>(baseItem.UseNamespaceInterpretation, revisedItem.UseNamespaceInterpretation, ValueComparisons.BooleanComparison);

                int largestParameterCount = baseItem.Count;

                if (revisedItem.Count > largestParameterCount)
                {
                    largestParameterCount = revisedItem.Count;
                }

                for (int i = 0; i < largestParameterCount; i++)
                {
                    ValueListItem baseListItem = null;
                    ValueListItem revisedListItem = null;

                    if (i < baseItem.Count)
                    {
                        baseListItem = baseItem[i];
                    }

                    if (i < revisedItem.Count)
                    {
                        revisedListItem = revisedItem[i];
                    }

                    DiffEntry<ValueListItem> diffEntry = new DiffEntry<ValueListItem>(baseListItem, revisedListItem, ValueComparisons.ValueListItemUIComparison);

                    this.items.Add(diffEntry);

                    if (diffEntry.HasChanged)
                    {
                        this.itemsHaveChanges = true;
                    }
                }

                this.HasChanges = this.name.HasChanged
                    || this.useNamespaceInterpretation.HasChanged
                    || this.useItemTranslations.HasChanged
                    || this.itemsHaveChanges;
            }
        }

        public DiffEntry<string> Name
        {
            get { return this.name; }
        }

        public DiffEntry<bool> UseItemTranslations
        {
            get { return this.useItemTranslations; }
        }

        public DiffEntry<bool> UseNamespaceInterpretation
        {
            get { return this.useNamespaceInterpretation; }
        }

        public List<DiffEntry<ValueListItem>> Items
        {
            get { return this.items; }
        }

        protected override bool ConflictsWithCore(ValueListDiff diff)
        {
            if (this.RevisedItem != null && diff.RevisedItem != null)
            {
                return this.RevisedItem.Name.ToUpperInvariant() == diff.RevisedItem.Name.ToUpperInvariant();
            }

            return false;
        }

        protected override bool GetIdentifierHasChangedCore()
        {
            return this.name.HasChanged;
        }
    }
}
