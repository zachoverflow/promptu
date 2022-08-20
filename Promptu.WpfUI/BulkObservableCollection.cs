using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace ZachJohnson.Promptu.WpfUI
{
    internal class BulkObservableCollection<T> : ObservableCollection<T>
    {
        private bool ignoreCollectionChanged;

        protected override void OnCollectionChanged(System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (this.ignoreCollectionChanged)
            {
                return;
            }

            base.OnCollectionChanged(e);
        }

        public void Repopulate(IEnumerable<T> collection)
        {
            if (collection == null)
            {
                throw new ArgumentNullException("collection");
            }

            try
            {
                this.ignoreCollectionChanged = true;

                this.Clear();
                foreach (T item in collection)
                {
                    this.Add(item);
                }
            }
            finally
            {
                this.ignoreCollectionChanged = false;
            }

            this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        public void Refresh()
        {
            this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        public void AddRange(IEnumerable<T> collection)
        {
            if (collection == null)
            {
                throw new ArgumentNullException("collection");
            }

            try
            {
                this.ignoreCollectionChanged = true;

                foreach (T item in collection)
                {
                    this.Add(item);
                }
            }
            finally
            {
                this.ignoreCollectionChanged = false;
            }

            this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }
    }
}
