using System;
using System.Collections.Generic;
using System.Text;

namespace ZachJohnson.Promptu.UIModel.Interfaces
{
    internal interface ISimpleCollectionViewer
    {
        event EventHandler SelectedIndexChanged;

        int ItemCount { get; }

        int HeaderCount { get; }

        int SelectedIndexesCount { get; }

        bool MultiSelect { get; set; }

        void Clear();

        object this[int index] { get; }

        void Insert(int index, object item);

        void RemoveAt(int index);

        int IndexOf(object item);

        void ClearHeaders();

        void InsertHeader(int index, string text, CellValueGetter cellValueGetter);

        void RemoveHeaderAt(int index);

        void SelectIndex(int index);

        void UnselectIndex(int index);

        IEnumerable<int> SelectedIndexes { get; }

        int PrimarySelectedIndex { get; }

        void ClearSelectedIndexes();

        int TopIndex { get; set; }

        void EnsureVisible(int index);

        void Select();

        //string GetTextAt(int row, int column);
    }
}
