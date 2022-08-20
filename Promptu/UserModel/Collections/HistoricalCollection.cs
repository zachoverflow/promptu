using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Extensions;
using System.IO;
using ZachJohnson.Promptu.Collections;

namespace ZachJohnson.Promptu.UserModel.Collections
{
    internal class HistoricalCollection : TrieDictionary<HistoryDetails>
    {
        public const int DefaultMaxCount = 150;
        private int maxCount = DefaultMaxCount;

        public HistoricalCollection()
            : base(SortMode.DecendingFromLastAdded, CaseSensitivity.Insensitive)
        {
        }

        public int MaxCount
        {
            get
            {
                return this.maxCount;
            }

            set
            {
                if (value < 0)
                {
                    value = 0;
                }

                this.maxCount = value;
            }
        }

        //protected override bool RemoveCore(string key)
        //{
        //    bool removed = base.RemoveCore(key);
        //    this.RegenerateFunctionHistory();
        //    this.RegenerateCommandParameterHistory();
        //    return removed;
        //}

        //protected override void ClearCore()
        //{
        //    base.ClearCore();
        //    this.RegenerateFunctionHistory();
        //    this.RegenerateCommandParameterHistory();
        //}

        //public void Add(string value)
        //{
        //    this.Add(value, new HistoryDetails(value));
        //}
        public void RemoveAllThatStartWith(string startsWith)
        {
            this.RemoveAllThatStartWith(new string[] { startsWith });
        }

        public void RemoveAllThatStartWith(IEnumerable<string> startsWithToRemove)
        {
            this.RemoveAllThatStartsWithCore(startsWithToRemove);
        }

        protected virtual void RemoveAllThatStartsWithCore(IEnumerable<string> startsWithToRemove)
        {
            if (startsWithToRemove == null)
            {
                throw new ArgumentNullException("startsWithToRemove");
            }

            List<string> entriesToRemove = new List<string>();
            foreach (string historyItemKey in this.Keys)
            {
                foreach (string name in startsWithToRemove)
                {
                    if (historyItemKey.StartsWith(name, StringComparison.InvariantCultureIgnoreCase))
                    {
                        entriesToRemove.Add(historyItemKey);
                    }
                }
            }

            foreach (string entryToRemove in entriesToRemove)
            {
                this.Remove(entryToRemove);
            }

            //this.Save();
        }

        protected override HistoryDetails GetItemCore(int index)
        {
            return base.GetItemCore(this.Count - index - 1);
        }

        public void AddRange(IEnumerable<HistoryDetails> collection)
        {
            this.AddChangeEventBlocker();
            foreach (HistoryDetails details in collection)
            {
                this.Add(details.EntryValue, details);
            }

            this.RemoveChangeEventBlocker();
            this.OnChanged(EventArgs.Empty);
        }

        public void RemoveByFilter(string filter, string itemId, int charCountToRemoveFromFilter)
        {
            if (charCountToRemoveFromFilter > filter.Length)
            {
                throw new ArgumentOutOfRangeException("'charCountToRemoveFromFilter' cannot be greater than the length of the filter.");
            }

            Dictionary<string, HistoryDetails> matches = this.FindAllThatStartWith(filter, CaseSensitivity.Insensitive);
            //bool wasRaisingChanged = this.RaiseChanged;
            //this.RaiseChanged = false;
            this.AddChangeEventBlocker();
            foreach (KeyValuePair<string, HistoryDetails> match in matches)
            {
                if (match.Value.ItemId == itemId)
                {
                    this.Remove(match.Key);
                }
            }

            if (charCountToRemoveFromFilter < filter.Length)
            {
                string newKey = filter.Substring(0, filter.Length - charCountToRemoveFromFilter);
                if (!this.Contains(newKey, CaseSensitivity.Insensitive))
                {
                    this.Add(newKey, new HistoryDetails(newKey, itemId));
                }
            }

            this.RemoveChangeEventBlocker();

            //if (!this.RaiseChanged)
            //{
            //    this.RaiseChanged = wasRaisingChanged;
            //}

            this.OnChanged(EventArgs.Empty);
        }

        protected override void InsertCore(int index, string item, HistoryDetails details)
        {
            this.AddChangeEventBlocker();
            bool found;
            HistoryDetails previousDetails;
            while ((previousDetails = this.TryGetItem(item, CaseSensitivity.Insensitive, out found)) != null && found)
            {
                this.Remove(previousDetails.EntryValue);
            }

            base.InsertCore(index, item, details);

            if (this.Count > this.MaxCount)
            {
                
                TrieDictionary<HistoryDetails> newRecentlyUsed = new TrieDictionary<HistoryDetails>(SortMode.DecendingFromLastAdded);

                int trimmedCount = this.MaxCount;

                if (trimmedCount >= 10)
                {
                    trimmedCount = (int)(trimmedCount * 0.9);
                }

                int difference = this.Count - trimmedCount;

                for (int i = 0; i < trimmedCount; i++)
                {
                    HistoryDetails movingDetails = base.GetItemCore(difference + i);
                    newRecentlyUsed.Add(movingDetails.EntryValue, movingDetails);
                }

                this.ClearForHitCeiling();
                this.AddRange(newRecentlyUsed);

                this.HandlePostAddHitCeiling();
            }
            else
            {
                this.HandlePostAdd(item, details);
            }

            this.RemoveChangeEventBlocker();
            this.OnChanged(EventArgs.Empty);
        }

        protected virtual void ClearForHitCeiling()
        {
            this.Clear();
        }

        protected virtual void HandlePostAddHitCeiling()
        {
        }

        protected virtual void HandlePostAdd(string item, HistoryDetails details)
        {
        }
    }
}

