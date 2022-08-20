using System;
using System.Collections.Generic;
using System.Text;
using ZachJohnson.Promptu.UIModel.Interfaces;

namespace ZachJohnson.Promptu.UIModel.Presenters
{
    internal class SimpleCollectionPresenter
    {
        private ISimpleCollectionViewer viewer;
        private HeaderCollection headers;
        private SelectedIndexCollection selectedIndexes;

        public SimpleCollectionPresenter(ISimpleCollectionViewer viewer)
        {
            if (viewer == null)
            {
                throw new ArgumentNullException("viewer");
            }

            this.viewer = viewer;
            this.headers = new HeaderCollection(this);
            this.selectedIndexes = new SelectedIndexCollection(this);
            this.viewer.SelectedIndexChanged += this.HandleSelectedIndexChanged;
        }

        public event EventHandler SelectedIndexChanged;

        public object this[int index]
        {
            get { return this.viewer[index]; }
        }

        public HeaderCollection Headers
        {
            get { return this.headers; }
        }

        public SelectedIndexCollection SelectedIndexes
        {
            get { return this.selectedIndexes; }
        }

        public int Count
        {
            get { return this.viewer.ItemCount; }
        }

        public void Clear()
        {
            this.viewer.Clear();
        }

        public void Select()
        {
            this.viewer.Select();
        }

        public void Add(object item)
        {
            this.Insert(this.viewer.ItemCount, item);
        }

        public void Insert(int index, object item)
        {
            this.viewer.Insert(index, item);
        }

        public bool MultiSelect
        {
            get { return this.viewer.MultiSelect; }
            set { this.viewer.MultiSelect = value; }
        }

        public void RemoveAt(int index)
        {
            this.viewer.RemoveAt(index);
        }

        //public object GetTag(int index)
        //{
        //    return this.viewer.GetTag(index);
        //}

        public int IndexOf(object item)
        {
            return this.viewer.IndexOf(item);
            //for (int i = 0; i < this.Count; i++)
            //{
            //    if (this.GetTag(i) == tag)
            //    {
            //        return i;
            //    }
            //}

            //return -1;
        }

        //public string GetTextAt(int row, int column)
        //{
        //    return this.viewer.GetTextAt(row, column);
        //}

        public void EnsureVisible(int index)
        {
            this.viewer.EnsureVisible(index);
        }

        public int TopIndex
        {
            get { return this.viewer.TopIndex; }
            set { this.viewer.TopIndex = value; }
        }

        public int PrimarySelectedIndex
        {
            get { return this.viewer.PrimarySelectedIndex; }
        }

        protected virtual void OnSelectedIndexChanged(EventArgs e)
        {
            EventHandler handler = this.SelectedIndexChanged;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        private void HandleSelectedIndexChanged(object sender, EventArgs e)
        {
            this.OnSelectedIndexChanged(EventArgs.Empty);
        }

        internal class SelectedIndexCollection : IEnumerable<int>
        {
            private SimpleCollectionPresenter presenter;

            public SelectedIndexCollection(SimpleCollectionPresenter presenter)
            {
                this.presenter = presenter;
            }

            public int Count
            {
                get { return this.presenter.viewer.SelectedIndexesCount; }
            }

            public void Clear()
            {
                this.presenter.viewer.ClearSelectedIndexes();
            }

            public void Add(int index)
            {
                this.presenter.viewer.SelectIndex(index);
            }

            public void Remove(int index)
            {
                this.presenter.viewer.UnselectIndex(index);
            }

            public IEnumerator<int> GetEnumerator()
            {
                return this.presenter.viewer.SelectedIndexes.GetEnumerator();
            }

            System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
            {
                return this.GetEnumerator();
            }
        }

        internal class HeaderCollection
        {
            private SimpleCollectionPresenter presenter;

            public HeaderCollection(SimpleCollectionPresenter presenter)
            {
                this.presenter = presenter;
            }

            public int Count
            {
                get { return this.presenter.viewer.HeaderCount; }
            }

            public void Clear()
            {
                this.presenter.viewer.ClearHeaders();
            }

            public void Add(string text, CellValueGetter cellValueGetter)
            {
                this.Insert(this.presenter.viewer.HeaderCount, text, cellValueGetter);
            }

            public void Insert(int index, string text, CellValueGetter cellValueGetter)
            {
                this.presenter.viewer.InsertHeader(index, text, cellValueGetter);
            }

            public void RemoveAt(int index)
            {
                this.presenter.viewer.RemoveHeaderAt(index);
            }
        }
    }
}
