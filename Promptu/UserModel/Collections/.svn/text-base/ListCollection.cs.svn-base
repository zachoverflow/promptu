using System;
using System.Collections.Generic;
using System.Text;
using ZachJohnson.Promptu.Collections;

namespace ZachJohnson.Promptu.UserModel.Collections
{
    internal class ListCollection : ChangeNotifiedList<List>
    {
        private int disabledListCount;

        public ListCollection()
        {
        }

        public ListCollection(IEnumerable<List> collection)
            : base(collection)
        {
        }

        public event EventHandler SyncAffectingChangeOccuring;

        public event EventHandler LastUpdateCheckTimestampChanged;

        public bool AnyListsAreDisabled
        {
            get
            {
                return this.disabledListCount > 0;
            }
        }

        public void SaveAll()
        {
            foreach (List list in this)
            {
                list.SaveAll();
            }
        }

        public List TryGet(string folderName)
        {
            foreach (List list in this)
            {
                if (list.FolderName == folderName)
                {
                    return list;
                }
            }

            return null;
        }

        protected void RaiseSyncAffectingChangeOccuringEvent(object sender, EventArgs e)
        {
            this.OnSyncAffectingChangeOccuring(EventArgs.Empty);
        }

        protected void RaiseLastUpdateCheckTimestampChangedEvent(object sender, EventArgs e)
        {
            this.OnLastUpdateCheckTimestampChanged(EventArgs.Empty);
        }

        protected virtual void OnSyncAffectingChangeOccuring(EventArgs e)
        {
            EventHandler handler = this.SyncAffectingChangeOccuring;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        protected virtual void OnLastUpdateCheckTimestampChanged(EventArgs e)
        {
            EventHandler handler = this.LastUpdateCheckTimestampChanged;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        protected override void OnItemAdded(ItemAndIndexEventArgs<List> e)
        {
            e.Item.SyncAffectingChangeOccuring += this.RaiseSyncAffectingChangeOccuringEvent;
            e.Item.LastUpdateCheckTimestampChanged += this.RaiseLastUpdateCheckTimestampChangedEvent;
            e.Item.EnabledChanged += this.HandleListEnabledChanged;
            if (!e.Item.Enabled)
            {
                this.disabledListCount++;
            }

            base.OnItemAdded(e);
        }

        protected override void OnItemRemoved(ItemAndIndexEventArgs<List> e)
        {
            e.Item.SyncAffectingChangeOccuring -= this.RaiseSyncAffectingChangeOccuringEvent;
            e.Item.LastUpdateCheckTimestampChanged -= this.RaiseLastUpdateCheckTimestampChangedEvent;
            e.Item.EnabledChanged -= this.HandleListEnabledChanged;
            if (!e.Item.Enabled)
            {
                this.disabledListCount--;
            }

            base.OnItemRemoved(e);
        }

        private void HandleListEnabledChanged(object sender, EventArgs e)
        {
            List list = sender as List;
            if (list != null)
            {
                if (list.Enabled)
                {
                    this.disabledListCount--;
                }
                else
                {
                    this.disabledListCount++;
                }
            }
        }
    }
}
