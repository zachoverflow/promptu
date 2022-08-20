//-----------------------------------------------------------------------
// <copyright file="UIComponentCollectionOverlay.cs" company="ZachJohnson">
//     Copyright (c) Zach Johnson. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace ZachJohnson.Promptu.PluginModel
{
    using System;
    using System.Collections.Generic;
    using ZachJohnson.Promptu.UIModel;

    public class UIComponentCollectionOverlay<TUIComponent> : IEnumerable<TUIComponent>
    {
        private IUIComponentCollection<TUIComponent> collection;
        private List<TUIComponent> ownedItems = new List<TUIComponent>();

        internal UIComponentCollectionOverlay(IUIComponentCollection<TUIComponent> collection)
        {
            this.collection = collection;
        }

        public int AllItemCount
        {
            get { return this.collection.Count; }
        }

        public void ClearOwned()
        {
            foreach (TUIComponent item in this.ownedItems)
            {
                this.collection.Remove(item);
            }

            this.ownedItems.Clear();
        }

        public void Add(TUIComponent item)
        {
            this.collection.Add(item);
            this.ownedItems.Add(item);
        }

        public void InsertAfter(string id, TUIComponent item)
        {
            this.collection.InsertAfter(id, item);
            this.ownedItems.Add(item);
        }

        public void InsertBefore(string id, TUIComponent item)
        {
            this.collection.InsertBefore(id, item);
            this.ownedItems.Add(item);
        }

        public void Insert(int index, TUIComponent item)
        {
            this.collection.Insert(index, item);
            this.ownedItems.Add(item);
        }

        public int IndexOf(TUIComponent item)
        {
            return this.collection.IndexOf(item);
        }

        public void RemoveAt(int index)
        {
            if (index < 0 || index >= this.AllItemCount)
            {
                throw new ArgumentOutOfRangeException("index");
            }

            TUIComponent item = this.collection[index];

            if (!this.ownedItems.Contains(item))
            {
                throw new ArgumentException("This collection does not own the item at that index.");
            }

            this.ownedItems.Remove(item);
            this.collection.RemoveAt(index);
        }

        public bool Remove(TUIComponent item)
        {
            if (!this.ownedItems.Contains(item))
            {
                throw new ArgumentException("This collection does not own that item.");
            }

            if (this.ownedItems.Remove(item))
            {
                this.collection.Remove(item);
                return true;
            }

            return false;
        }

        public IEnumerator<TUIComponent> GetEnumerator()
        {
            return this.collection.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
