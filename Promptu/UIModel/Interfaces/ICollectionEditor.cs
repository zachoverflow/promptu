using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;

namespace ZachJohnson.Promptu.UIModel.Interfaces
{
    internal interface ICollectionEditor : IBatchUpdatable
    {
        event EventHandler<CellValueEventArgs> CellValueNeeded;

        event EventHandler<CellValueEventArgs> CellValuePushed;

        event EventHandler<CellValidatingEventArgs> CellValidating;

        event CancelEventHandler DataContextMenuOpening;

        event EventHandler SelectionChanged;

        event EventHandler<CellEventArgs> CellClicked;

        event KeyEventHandler EditorKeyDown;

        IButton AddButton { get; }

        IButton DeleteButton { get; }

        IToolbarButton MoveUpButton { get; }

        IToolbarButton MoveDownButton { get; }

        IButton PasteNewRowsButton { get; }

        UIContextMenu DataContextMenu { get; }

        bool MessageVisible { set; }

        bool AllowPaste { get; set; }

        bool DoMinimalCellValidating { get; }

        string Message { set; }

        int RowCount { get; set; }

        void EditCurrentCell();

        void Refresh();

        void ClearSelection();

        int SelectedCellsCount { get; }

        CellAddress? CurrentCell { get; set; }

        bool IsSelected(int row, int column);

        IEnumerable<CellAddress> SelectedCells { get; }

        void InsertColumn(int index, Column column);

        int ColumnCount { get; }

        void RemoveColumnAt(int index);

        string Text { set; }

        bool IsVisibleButton(CellAddress cell);

        void SetButtonVisibility(CellAddress cell, bool value);

        void InvalidateCell(CellAddress cell);

        void InvalidateRow(int row);

        bool IsVisibleColumn(int index);

        void SetColumnVisibility(int index, bool value);

        void DoCopy();
    }
}
