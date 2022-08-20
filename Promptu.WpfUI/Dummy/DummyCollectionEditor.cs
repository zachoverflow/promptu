using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZachJohnson.Promptu.UIModel.Interfaces;

namespace ZachJohnson.Promptu.WpfUI.Dummy
{
    internal class DummyCollectionEditor : ICollectionEditor
    {
        public event EventHandler<UIModel.CellValueEventArgs> CellValueNeeded;

        public event EventHandler<UIModel.CellValueEventArgs> CellValuePushed;

        public event EventHandler<UIModel.CellValidatingEventArgs> CellValidating;

        public event EventHandler SelectionChanged;

        public event EventHandler<UIModel.CellEventArgs> CellClicked;

        public ParameterlessVoid DeleteDelegate
        {
            set { }
        }

        public ParameterlessVoid AddDelegate
        {
            set { }
        }

        public ParameterlessVoid PasteDelegate
        {
            set { }
        }

        public Question AutoEndEditForCurrentCell
        {
            set { }
        }

        public IButton AddButton
        {
            get { return new DummyButton(); }
        }

        public IButton DeleteButton
        {
            get { return new DummyButton(); }
        }

        public IToolbarButton MoveUpButton
        {
            get { return new DummyButton(); }
        }

        public IToolbarButton MoveDownButton
        {
            get { return new DummyButton(); }
        }

        public IButton PasteNewRowsButton
        {
            get { return new DummyButton(); }
        }

        public UIModel.UIContextMenu DataContextMenu
        {
            get { return new UIModel.UIContextMenu(); }
        }

        public bool MessageVisible
        {
            set { }
        }

        public bool AllowPaste
        {
            get
            {
                return true;
            }
            set
            {
            }
        }

        public bool DoMinimalCellValidating
        {
            get { return false; }
        }

        public string Message
        {
            get
            {
                return String.Empty;
            }
            set
            {
            }
        }

        public int RowCount
        {
            get
            {
                return 0;
            }
            set
            {
            }
        }

        public void EditCurrentCell()
        {
        }

        public void Refresh()
        {
        }

        public void ClearSelection()
        {
        }

        public int SelectedCellsCount
        {
            get { return 0;}
        }

        public UIModel.CellAddress? CurrentCell
        {
            get
            {
                return null;
            }
            set
            {
            }
        }

        public bool IsSelected(int row, int column)
        {
            return false;
        }

        public IEnumerable<UIModel.CellAddress> SelectedCells
        {
            get { return new List<UIModel.CellAddress>(); }
        }

        public void InsertColumn(int index, UIModel.Column column)
        {
        }

        public int ColumnCount
        {
            get { return 0; }
        }

        public void RemoveColumnAt(int index)
        {
        }

        public string Text
        {
            get
            {
                return String.Empty;
            }
            set
            {
            }
        }

        public bool IsVisibleButton(UIModel.CellAddress cell)
        {
            return false;
        }

        public void SetButtonVisibility(UIModel.CellAddress cell, bool value)
        {
        }

        public void InvalidateCell(UIModel.CellAddress cell)
        {
        }

        public void InvalidateRow(int row)
        {
        }

        public bool IsVisibleColumn(int index)
        {
            return true;
        }

        public void SetColumnVisibility(int index, bool value)
        {
        }

        public object GetClipboardData()
        {
            return null;
        }

        public void BeginUpdate()
        {
        }

        public void EndUpdate()
        {
        }


        public event System.Windows.Forms.KeyEventHandler EditorKeyDown;


        public void DoCopy()
        {
            throw new NotImplementedException();
        }
    }
}
