// Copyright 2022 Zach Johnson
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

namespace ZachJohnson.Promptu.Collections
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    internal class ChangeNotifiedList<T> : IIndexedCollection<T>, IList<T>
    {
        private List<T> items = new List<T>();

        public ChangeNotifiedList()
        {
        }

        public ChangeNotifiedList(IEnumerable<T> collection)
        {
            this.AddRange(collection);
        }

        public event EventHandler<ItemAndIndexEventArgs<T>> AddingItem;

        public event EventHandler<ItemAndIndexEventArgs<T>> RemovingItem;

        public event EventHandler<ItemAndIndexEventArgs<T>> ItemAdded;

        public event EventHandler<ItemAndIndexEventArgs<T>> ItemRemoved;

        public bool IsReadOnly
        {
            get { return false; }
        }

        public int Count
        {
            get { return this.items.Count; }
        }

        public T this[int index]
        {
            get
            {
                return this.items[index];
            }

            set
            {
                T currentItem = this[index];
                ItemAndIndexEventArgs<T> currentItemEventArgs = new ItemAndIndexEventArgs<T>(currentItem, index);
                ItemAndIndexEventArgs<T> newItemEventArgs = new ItemAndIndexEventArgs<T>(value, index);
                this.OnRemovingItem(currentItemEventArgs);
                this.OnAddingItem(newItemEventArgs);

                using (DdMonitor.Lock(this))
                {
                    this.items[index] = value;
                }

                this.OnItemRemoved(currentItemEventArgs);
                this.OnItemAdded(newItemEventArgs);
            }
        }

        public void Add(T item)
        {
            this.Insert(this.Count, item);
        }

        public void Insert(int index, T item)
        {
            if (!this.items.Contains(item))
            {
                ItemAndIndexEventArgs<T> eventArgs = new ItemAndIndexEventArgs<T>(item, index);
                this.OnAddingItem(eventArgs);

                using (DdMonitor.Lock(this))
                {
                    this.items.Insert(index, item);
                }

                this.OnItemAdded(eventArgs);
            }
        }

        public void AddRange(IEnumerable<T> collection)
        {
            foreach (T item in collection)
            {
                this.Add(item);
            }
        }

        public int IndexOf(T item)
        {
            return this.items.IndexOf(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            this.items.CopyTo(array, arrayIndex);
        }

        public bool Remove(T item)
        {
            if (this.Contains(item))
            {
                int index = this.items.IndexOf(item);
                ItemAndIndexEventArgs<T> eventArgs = new ItemAndIndexEventArgs<T>(item, index);
                this.OnRemovingItem(eventArgs);
                bool removed;

                using (DdMonitor.Lock(this))
                {
                    removed = this.items.Remove(item);
                }

                this.OnItemRemoved(eventArgs);
                return removed;
            }

            return false;
        }

        public void RemoveAt(int index)
        {
            if (index < 0)
            {
                throw new ArgumentOutOfRangeException("Index cannot be less than zero.");
            }
            else if (index >= this.Count)
            {
                throw new ArgumentOutOfRangeException("Index cannot be greater than or equal to the number of items.");
            }

            T item = this[index];
            ItemAndIndexEventArgs<T> eventArgs = new ItemAndIndexEventArgs<T>(item, index);
            this.OnRemovingItem(eventArgs);

            using (DdMonitor.Lock(this))
            {
                this.items.RemoveAt(index);
            }

            this.OnItemRemoved(eventArgs);
        }

        public bool Contains(T item)
        {
            return this.items.Contains(item);
        }

        public void Clear()
        {
            using (DdMonitor.Lock(this))
            {
                foreach (T item in this.ToArray())
                {
                    this.Remove(item);
                }
            }
        }

        public T[] ToArray()
        {
            return this.items.ToArray();
        }

        public IEnumerator<T> GetEnumerator()
        {
            return this.items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.items.GetEnumerator();
        }

        protected virtual void OnItemAdded(ItemAndIndexEventArgs<T> e)
        {
            EventHandler<ItemAndIndexEventArgs<T>> handler = this.ItemAdded;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        protected virtual void OnItemRemoved(ItemAndIndexEventArgs<T> e)
        {
            EventHandler<ItemAndIndexEventArgs<T>> handler = this.ItemRemoved;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        protected virtual void OnAddingItem(ItemAndIndexEventArgs<T> e)
        {
            EventHandler<ItemAndIndexEventArgs<T>> handler = this.AddingItem;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        protected virtual void OnRemovingItem(ItemAndIndexEventArgs<T> e)
        {
            EventHandler<ItemAndIndexEventArgs<T>> handler = this.RemovingItem;
            if (handler != null)
            {
                handler(this, e);
            }
        }
    }
}
