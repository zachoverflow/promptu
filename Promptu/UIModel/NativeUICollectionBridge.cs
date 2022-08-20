using System;
using System.Collections.Generic;
using System.Text;
using ZachJohnson.Promptu.UIModel.Interfaces;

namespace ZachJohnson.Promptu.UIModel
{
    internal class NativeUICollectionBridge<T> : INativeUICollection<T>
    {
        private List<T> items = new List<T>();
        private INativeUICollection<T> bridgedCollection;

        public NativeUICollectionBridge()
        {
        }

        public void Insert(int index, T item)
        {
            this.items.Insert(index, item);

            INativeUICollection<T> collection = this.bridgedCollection;
            if (collection != null)
            {
                collection.Insert(index, item);
            }
        }

        public INativeUICollection<T> BridgedCollection
        {
            get 
            { 
                return this.bridgedCollection;
            }

            set
            {
                INativeUICollection<T> oldCollection = this.bridgedCollection;

                if (oldCollection != null)
                {
                    foreach (var item in this.items)
                    {
                        oldCollection.Remove(item);
                    }
                }

                this.bridgedCollection = value;
                if (value != null)
                {
                    int index = 0;
                    foreach (T item in this.items)
                    {
                        this.bridgedCollection.Insert(index, item);
                        index++;
                    }
                }
            }
        }

        public void Clear()
        {
            INativeUICollection<T> collection = this.bridgedCollection;
            if (collection != null)
            {
                collection.Clear();
            }

            this.items.Clear();
        }

        public void Remove(T item)
        {
            this.items.Remove(item);

            INativeUICollection<T> collection = this.bridgedCollection;
            if (collection != null)
            {
                collection.Remove(item);
            }
        }
    }
}
