//-----------------------------------------------------------------------
// <copyright file="PromptuPluginCollection.cs" company="ZachJohnson">
//     Copyright (c) Zach Johnson. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace ZachJohnson.Promptu.PluginModel
{
    using System;
    using System.Collections.ObjectModel;
    using System.ComponentModel;

    internal class PromptuPluginCollection : Collection<PromptuPlugin>
    {
        private bool ignoreChanges;

        public PromptuPluginCollection()
        {
        }

        internal event EventHandler ChildSettingOrPropertyChanged;

        internal event PropertyChangedEventHandler ChildPropertyChanged;

        public PromptuPlugin TryGet(string id)
        {
            foreach (PromptuPlugin plugin in this)
            {
                if (plugin != null && plugin.Id == id)
                {
                    return plugin;
                }
            }

            return null;
        }

        public bool Contains(string id)
        {
            foreach (PromptuPlugin plugin in this)
            {
                if (plugin != null && plugin.Id == id)
                {
                    return true;
                }
            }

            return false;
        }

        public void DisableAndResetAll()
        {
            try
            {
                this.ignoreChanges = true;
                foreach (PromptuPlugin plugin in this)
                {
                    plugin.Enabled = false;
                    plugin.IsInstalled = false;
                }
            }
            finally
            {
                this.ignoreChanges = false;
            }
        }

        protected override void InsertItem(int index, PromptuPlugin item)
        {
            base.InsertItem(index, item);
            this.AttachTo(item);
        }

        protected override void ClearItems()
        {
            foreach (PromptuPlugin plugin in this)
            {
                this.DetachFrom(plugin);
            }

            base.ClearItems();
        }

        protected override void RemoveItem(int index)
        {
            if (index >= 0 && index < this.Count)
            {
                this.DetachFrom(this[index]);
            }

            base.RemoveItem(index);
        }

        protected override void SetItem(int index, PromptuPlugin item)
        {
            if (index >= 0 && index < this.Count)
            {
                this.DetachFrom(this[index]);
            }

            base.SetItem(index, item);

            this.AttachTo(item);
        }

        protected virtual void OnChildPropertyChanged(PropertyChangedEventArgs e)
        {
            PropertyChangedEventHandler handler = this.ChildPropertyChanged;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        protected virtual void OnChildSettingOrPropertyChanged(EventArgs e)
        {
            EventHandler handler = this.ChildSettingOrPropertyChanged;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        private void AttachTo(PromptuPlugin item)
        {
            if (item != null)
            {
                item.PropertyChanged += this.HandlePluginPropertyChanged;
                item.SettingOrPropertyChanged += this.HandlePluginSettingOrPropertyChanged;
            }
        }

        private void DetachFrom(PromptuPlugin item)
        {
            if (item != null)
            {
                item.PropertyChanged -= this.HandlePluginPropertyChanged;
                item.SettingOrPropertyChanged -= this.HandlePluginSettingOrPropertyChanged;
            }
        }

        private void HandlePluginPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (!this.ignoreChanges)
            {
                this.OnChildPropertyChanged(e);
            }
        }

        private void HandlePluginSettingOrPropertyChanged(object sender, EventArgs e)
        {
            this.OnChildSettingOrPropertyChanged(EventArgs.Empty);
        }
    }
}
