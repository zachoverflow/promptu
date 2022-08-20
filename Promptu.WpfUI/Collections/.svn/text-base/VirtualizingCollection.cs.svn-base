using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using ZachJohnson.Promptu.UIModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Windows;

namespace ZachJohnson.Promptu.WpfUI.Collections
{
    internal class VirtualizingCollection : IList, INotifyCollectionChanged
    {
        private int count;
        private Dictionary<int, object> items = new Dictionary<int, object>();

        public VirtualizingCollection()
        {
        }

        public event EventHandler<CellValueEventArgs> CellValueNeeded;

        public event EventHandler<CellValueEventArgs> CellValuePushed;

        public event EventHandler<CellValidatingEventArgs> CellValidating;

        public event NotifyCollectionChangedEventHandler CollectionChanged;

        public int Count
        {
            get 
            { 
                return this.count; 
            }

            set 
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException("value");
                }

                this.count = value;

                this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
            }
        }

        public void RefreshBinding()
        {
            this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        public void RefreshRow(int index)
        {
            this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
            //object item = this[index];
            //this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace, item, item));
        }

        public int Add(object value)
        {
            throw new NotSupportedException();
        }

        public void Clear()
        {
            throw new NotSupportedException();
        }

        public bool Contains(object value)
        {
            return this.items.ContainsValue(value);
        }

        public int IndexOf(object value)
        {
            foreach (KeyValuePair<int, object> entry in this.items)
            {
                if (entry.Value == value)
                {
                    return entry.Key;
                }
            }

            return -1;
        }

        public void Insert(int index, object value)
        {
            throw new NotSupportedException();
        }

        public bool IsFixedSize
        {
            get { return false; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public void Remove(object value)
        {
        }

        public void RemoveAt(int index)
        {
        }

        public object this[int index]
        {
            get
            {
                if (index < 0 || index >= this.Count)
                {
                    throw new ArgumentOutOfRangeException("index");
                }

                object item;
                if (this.items.TryGetValue(index, out item))
                {
                    return item;
                }
                else
                {
                    item = new VirtualItem(this, index);
                    this.items.Add(index, item);
                    return item;
                }
            }
            set
            {
                throw new NotSupportedException();
            }
        }

        public void CopyTo(Array array, int index)
        {
            object[] realArray = array as object[];

            if (realArray == null)
            {
                return;
            }

            for (int i = index; i < this.Count; i++)
            {
                realArray[i] = this[i];
            }
        }

        public bool IsSynchronized
        {
            get { return false; }
        }

        public object SyncRoot
        {
            get { return this; }
        }

        public IEnumerator GetEnumerator()
        {
            for (int i = 0; i < this.count; i++)
            {
                yield return this[i];
            }
        }

        protected virtual void OnCellValueNeeded(CellValueEventArgs e)
        {
            EventHandler<CellValueEventArgs> handler = this.CellValueNeeded;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        protected virtual void OnCellValuePushed(CellValueEventArgs e)
        {
            EventHandler<CellValueEventArgs> handler = this.CellValuePushed;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        protected virtual void OnCellValidating(CellValidatingEventArgs e)
        {
            EventHandler<CellValidatingEventArgs> handler = this.CellValidating;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        protected virtual void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            NotifyCollectionChangedEventHandler handler = this.CollectionChanged;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        public void SetButtonVisibility(int index, int buttonIndex, Visibility value)
        {
            VirtualItem item = (VirtualItem)this[index];
            while (buttonIndex >= item.ButtonVisibility.Count)
            {
                item.ButtonVisibility.Add(new VisibilityWrapper());
            }

            item.ButtonVisibility[buttonIndex].Visibility = value;
        }

        public Visibility GetButtonVisibility(int index, int buttonIndex)
        {
            VirtualItem item = (VirtualItem)this[index];
            if (buttonIndex >= item.ButtonVisibility.Count)
            {
                return Visibility.Visible;
            }

            return item.ButtonVisibility[buttonIndex].Visibility;
        }

        internal class VisibilityWrapper : INotifyPropertyChanged
        {
            private Visibility visibility;

            public VisibilityWrapper()
            {
                this.visibility = Visibility.Visible;
            }

            public event PropertyChangedEventHandler PropertyChanged;

            public Visibility Visibility
            {
                get 
                { 
                    return this.visibility; 
                }

                set 
                { 
                    this.visibility = value;
                    this.OnPropertyChanged(new PropertyChangedEventArgs("Visibility"));
                }
            }

            protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)
            {
                PropertyChangedEventHandler handler = this.PropertyChanged;
                if (handler != null)
                {
                    handler(this, e);
                }
            }
        }

        internal class VirtualItem : INotifyPropertyChanged
        {
            private VirtualizingCollection parentCollection;
            private ObservableCollection<VisibilityWrapper> buttonVisibility = new ObservableCollection<VisibilityWrapper>();
            private int rowIndex;

            internal VirtualItem(VirtualizingCollection parentCollection, int rowIndex)
            {
                if (parentCollection == null)
                {
                    throw new ArgumentNullException("parentCollection");
                }

                this.parentCollection = parentCollection;
                this.rowIndex = rowIndex;
            }

            public event PropertyChangedEventHandler PropertyChanged;

            public object this[int index]
            {
                get
                {
                    CellValueEventArgs cellValueEventArgs = new CellValueEventArgs(index, this.rowIndex, null);
                    this.parentCollection.OnCellValueNeeded(cellValueEventArgs);
                    return cellValueEventArgs.Value;
                }

                set
                {
                    CellValidatingEventArgs cellValidatingEventArgs = new CellValidatingEventArgs(index, this.rowIndex, value);

                    this.parentCollection.OnCellValidating(cellValidatingEventArgs);

                    if (cellValidatingEventArgs.Cancel)
                    {
                        this.RaisePropertyChanged("Item[]");
#if DEBUG
                        return;
#else
                        throw new ArgumentException();
#endif

                    }
                    else
                    {
                        CellValueEventArgs cellValueEventArgs = new CellValueEventArgs(index, this.rowIndex, value);
                        this.parentCollection.OnCellValuePushed(cellValueEventArgs);
                    }
                }
            }

            public ObservableCollection<VisibilityWrapper> ButtonVisibility
            {
                get { return this.buttonVisibility; }
            }

            internal void RaisePropertyChanged(string name)
            {
                this.OnPropertyChanged(new PropertyChangedEventArgs(name));
            }

            protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)
            {
                PropertyChangedEventHandler handler = this.PropertyChanged;
                if (handler != null)
                {
                    handler(this, e);
                }
            }
        }
    }
}
