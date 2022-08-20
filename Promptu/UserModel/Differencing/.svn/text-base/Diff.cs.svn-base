using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;

namespace ZachJohnson.Promptu.UserModel.Differencing
{
    internal abstract class Diff<T, TDiff> 
        where TDiff : Diff<T, TDiff> 
        where T : IHasId
    {
        private bool hasChanges;
        private DiffStatus status;
        private T baseItem;
        private T revisedItem;

        public Diff(T baseItem, T revisedItem)
        {
            this.baseItem = baseItem;
            this.revisedItem = revisedItem;
        }

        public bool HasChanges
        {
            get { return this.hasChanges; }
            protected set { this.hasChanges = value; } 
        }

        public DiffStatus Status
        {
            get { return this.status; }
            protected set { this.status = value; } 
        }

        public bool ConflictsWith(TDiff diff)
        {
            return this.ConflictsWithCore(diff);
        }

        public bool GetIdentifierHasChanged()
        {
            return this.GetIdentifierHasChangedCore();
        }

        public bool IsSimilarTo(TDiff diff)
        {
            Id thisId;
            Id diffId;

            if (this.Status == DiffStatus.Deleted)
            {
                thisId = this.BaseItem.Id;
            }
            else
            {
                thisId = this.RevisedItem.Id;
            }

            if (diff.Status == DiffStatus.Deleted)
            {
                diffId = diff.BaseItem.Id;
            }
            else
            {
                diffId = diff.RevisedItem.Id;
            }

            return thisId == diffId;
            //return this.IsSimilarToCore(diff);
        }

        //protected abstract bool IsSimilarToCore(TDiff diff);

        public Id GetId()
        {
            if (baseItem != null)
            {
                return baseItem.Id;
            }

            return revisedItem.Id;
        }

        public T RevisedItem
        {
            get { return this.revisedItem; }
        }

        public T BaseItem
        {
            get { return this.baseItem; }
        }

        public override string ToString()
        {
            return String.Format(CultureInfo.InvariantCulture, "HasChanges: {0}", this.HasChanges);
        }

        protected abstract bool ConflictsWithCore(TDiff diff);

        protected abstract bool GetIdentifierHasChangedCore();
    }
}
