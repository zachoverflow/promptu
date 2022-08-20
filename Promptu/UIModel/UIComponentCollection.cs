using System;
using System.Collections.Generic;
using System.Text;
using ZachJohnson.Promptu.UIModel.Interfaces;

namespace ZachJohnson.Promptu.UIModel
{
    public abstract class UIComponentCollection<TUIComponent, TComponentNativeInterface> : IUIComponentCollection<TUIComponent>
        where TUIComponent : UIComponent<TUIComponent, TComponentNativeInterface>
    {
        private List<TUIComponent> items = new List<TUIComponent>();

        public UIComponentCollection()
        {
        }

        public int Count
        {
            get { return this.items.Count; }
        }

        public void Add(TUIComponent item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }

            this.Insert(this.items.Count, item);
        }

        //public void AddInternal(TUIComponent item)
        //{
        //    if (item == null)
        //    {
        //        throw new ArgumentNullException("item");
        //    }

        //    this.InsertInternal(this.items.Count, item);
        //}

        //public void Insert(int index, TUIComponent item)
        //{
        //    if (item == null)
        //    {
        //        throw new ArgumentNullException("item");
        //    }

        //    if (item.ParentCollection != null && item.IsInternal)
        //    {
        //        throw new InvalidOperationException("Cannot add the item because it is internal to Promptu and you are not allowed to remove it from its current collection.");
        //    }

        //    this.InsertInternal(index, item);
        //}

        //public void InsertAfter(string id, TUIComponent item)
        //{
        //    this.InsertAfter(id, item, false);
        //}

        //internal void InsertAfterInternal(string id, TUIComponent item)
        //{
        //    this.InsertAfter(id, item, true);
        //}

        //public void InsertBefore(string id, TUIComponent item)
        //{
        //    this.InsertBefore(id, item, false);
        //}

        //internal void InsertBeforeInternal(string id, TUIComponent item)
        //{
        //    this.InsertBefore(id, item, true);
        //}

        public void InsertAfter(string id, TUIComponent item)
        {
            this.InsertNextTo(id, item, 1);
        }

        public void InsertBefore(string id, TUIComponent item)
        {
            this.InsertNextTo(id, item, 0);
        }

        private void InsertNextTo(string id, TUIComponent item, int offset)
        {
            if (id == null)
            {
                throw new ArgumentNullException("id");
            }
            else if (item == null)
            {
                throw new ArgumentNullException("item");
            }

            for (int i = 0; i < this.Count; i++)
            {
                if (this[i].Id == id)
                {
                    //if (isInternal)
                    //{
                    //    this.InsertInternal(i + offset, item);
                    //}
                    //else
                    //{
                        this.Insert(i + offset, item);
                        break;
                    //}
                }
            }
        }

        public void Insert(int index, TUIComponent item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }

            this.items.Insert(index, item);
            this.InsertIntoUnderlyingCollection(index, item);
            if (item.ParentCollection != null)
            {
                item.ParentCollection.Remove(item);
            }

            item.ParentCollection = this;
        }

        public int IndexOf(TUIComponent item)
        {
            return this.items.IndexOf(item);
        }

        //public void RemoveAt(int index)
        //{
        //    if (index > 0 && index < this.Count)
        //    {
        //        TUIComponent item = this[index];

        //        if (item.IsInternal)
        //        {
        //            throw new InvalidOperationException("Cannot remove the indicated item because it is internal to Promptu and you are not allowed to remove it.");
        //        }
        //    }

        //    this.RemoveAtInternal(index);
        //}

        public void RemoveAt(int index)
        {
            this.Remove(this[index]);
        }

        public TUIComponent this[int index]
        {
            get
            {
                return this.items[index];
            }
        }

        public bool Contains(TUIComponent item)
        {
            return this.items.Contains(item);
        }

        public void CopyTo(TUIComponent[] array, int arrayIndex)
        {
            this.items.CopyTo(array, arrayIndex);
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        //public bool Remove(TUIComponent item)
        //{
        //    if (item == null)
        //    {
        //        throw new ArgumentNullException("item");
        //    }

        //    if (item is ILockedInternal)
        //    {
        //        throw new InvalidOperationException("Cannot remove the item because it is internal to Promptu and you are not allowed to remove it.");
        //    }

        //    return this.RemoveInternal(item);
        //}

        public bool Remove(TUIComponent item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }

            bool sucess = this.items.Remove(item);
            this.RemoveFromUnderlyingCollection(item);
            //this.correspondingCollection.Remove(item.GenericMenuItem);
            return sucess;
        }

        public IEnumerator<TUIComponent> GetEnumerator()
        {
            return this.items.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        internal void Clear()
        {
            while (this.Count > 0)
            {
                this.RemoveAt(0);
            }
        }

        protected abstract void RemoveFromUnderlyingCollection(TUIComponent item);

        protected abstract void InsertIntoUnderlyingCollection(int index, TUIComponent item);
    }
}
