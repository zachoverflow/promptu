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

namespace ZachJohnson.Promptu.UIModel.Presenters
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using ZachJohnson.Promptu.UIModel.Interfaces;
    using ZachJohnson.Promptu.Collections;
    using System.Windows.Forms;
    using System.IO;
    using System.Drawing;

    internal abstract class CollectionEditorPresenter<TCollection, TItem> : PresenterBase<ICollectionEditor>
        where TCollection : IHasCount, new()
    {
        private readonly UIMenuItemInternal contextCopy;
        private readonly UIMenuItemInternal contextPaste;
        private TCollection collection = new TCollection();
        private bool showMessageIfNoItems;
        //private bool ignoreCellValuePushed;

        public CollectionEditorPresenter(ICollectionEditor nativeInterface, bool showMessageIfNoItems)
            : base(nativeInterface)
        {
            this.showMessageIfNoItems = showMessageIfNoItems;

            this.NativeInterface.CellValueNeeded += this.HandleCellValueNeeded;
            this.NativeInterface.CellValuePushed += this.HandleCellValuePushed;
            this.NativeInterface.SelectionChanged += this.HandleSelectionChanged;

            this.NativeInterface.EditorKeyDown += this.HandleEditorKeyDown;
            //this.NativeInterface.AddDelegate = new ParameterlessVoid(this.AddDefault);
            //this.NativeInterface.DeleteDelegate = new ParameterlessVoid(this.DoDelete);
            //this.NativeInterface.PasteDelegate = new ParameterlessVoid(this.HandlePaste);

            this.NativeInterface.AddButton.Text = Localization.UIResources.Add;
            //this.NativeInterface.AddButton.Image = EmbeddedImages.GreenPlus;
            this.NativeInterface.AddButton.Click += this.HandleAddMouseDown;

            this.NativeInterface.DeleteButton.Text = Localization.UIResources.Delete;
            //this.NativeInterface.DeleteButton.Image = EmbeddedImages.RedX;
            this.NativeInterface.DeleteButton.Click += this.HandleDeleteMouseDown;

            this.NativeInterface.PasteNewRowsButton.Text = Localization.UIResources.PasteNewRows;
            //this.NativeInterface.PasteNewRowsButton.Image = EmbeddedImages.Paste;
            this.NativeInterface.PasteNewRowsButton.Click += this.HandlePasteNewRows;

            //this.NativeInterface.MoveDownButton.Image = EmbeddedImages.DownArrow;
            this.NativeInterface.MoveDownButton.Click += this.HandleMoveDownMouseDown;

            //this.NativeInterface.MoveUpButton.Image = EmbeddedImages.UpArrow;
            this.NativeInterface.MoveUpButton.Click += this.HandleMoveUpMouseDown;

            this.NativeInterface.DataContextMenuOpening += this.HandleContextMenuOpening;

            this.contextCopy = new UIMenuItemInternal(
                "Promptu.Copy", 
                Localization.UIResources.CopyMenuItemText,
                InternalGlobals.GuiManager.ToolkitHost.Images.Copy,
                new EventHandler(this.HandleCopy));

            this.contextPaste = new UIMenuItemInternal(
                "Promptu.Paste",
                Localization.UIResources.PasteMenuItemText,
                InternalGlobals.GuiManager.ToolkitHost.Images.Paste,
                new EventHandler(this.HandlePaste));

            this.NativeInterface.DataContextMenu.Items.Add(this.contextCopy);
            this.NativeInterface.DataContextMenu.Items.Add(this.contextPaste);

            this.AllowPaste = false;

            this.UpdateEnabledStates();
        }

        public void AssignItems(TCollection collection)
        {
            this.collection = this.CloneCollection(collection);
            this.NativeInterface.RowCount = this.collection.Count;
            this.UpdateMessageVisible();
            this.PostAssignItems();
            this.NativeInterface.Refresh();
            this.UpdateEnabledStates();
        }

        public TCollection AssembleItems()
        {
            return this.CloneCollection(this.collection);
        }

        protected bool AllowPaste
        {
            get { return this.NativeInterface.AllowPaste; }
            set { this.NativeInterface.AllowPaste = value; }
        }

        protected TCollection Collection
        {
            get { return this.collection; }
        }

        protected abstract object RequestInformation(int row, int column);

        protected abstract void ChangeInformation(int row, int column, object value, List<string> errorCatcher, out int? newSelectedIndex);

        protected abstract void InsertItem(ref int index, TItem item);

        //protected abstract bool AutoEndEditForCurrentCell();

        protected abstract void TranslateItem(int index, int indexTo);

        protected abstract void RemoveItemAt(int index);

        protected abstract TCollection CloneCollection(TCollection collection);

        protected abstract TItem CreateDefault();

        protected abstract TItem CreateFromValues(string[] values, out string errorCode);

        protected virtual void PostAssignItems()
        {
        }
        
        protected void UpdateEnabledStates()
        {
            if (this.NativeInterface.SelectedCellsCount > 0)
            {
                int row = -1;
                foreach (CellAddress cell in this.NativeInterface.SelectedCells)
                {
                    row = cell.Row;
                    break;
                }

                this.NativeInterface.MoveUpButton.Enabled = row > 0;
                this.NativeInterface.MoveDownButton.Enabled = row < this.Collection.Count - 1;
                this.NativeInterface.DeleteButton.Enabled = true;
            }
            else
            {
                this.NativeInterface.MoveDownButton.Enabled = false;
                this.NativeInterface.MoveUpButton.Enabled = false;
                this.NativeInterface.DeleteButton.Enabled = false;
            }
        }
        
        private void HandleContextMenuOpening(object sender, EventArgs e)
        {
            this.contextPaste.Enabled = InternalGlobals.GuiManager.ToolkitHost.Clipboard.ContainsText;
        }

        private void UpdateMessageVisible()
        {
            if (this.showMessageIfNoItems)
            {
                this.NativeInterface.MessageVisible = this.collection.Count <= 0;
            }
        }

        private void HandleCopy(object sender, EventArgs e)
        {
            this.NativeInterface.DoCopy();
        }

        private void HandleCellValueNeeded(object sender, CellValueEventArgs e)
        {
            e.Value = this.RequestInformation(e.Row, e.Column);
        }

        private void HandleCellValuePushed(object sender, CellValueEventArgs e)
        {
            //if (this.ignoreCellValuePushed)
            //{
            //    return;
            //}

            int? newSelectedIndex; //TODO remove if never used
            this.ChangeInformation(e.Row, e.Column, e.Value, null, out newSelectedIndex);

            //if (newSelectedIndex != null)
            //{
            //    this.ignoreCellValuePushed = true;
            //    this.dataGridView.Refresh();
            //    this.MoveSelectionToRow(newSelectedIndex.Value);
            //    this.ignoreCellValuePushed = false;
            //}
        }

        private void Insert(int index, TItem item, bool select)
        {
            int previousCount = this.Collection.Count;
            this.InsertItem(ref index, item);
            this.NativeInterface.RowCount++;
            if (select)
            {
                this.NativeInterface.CurrentCell = new CellAddress(index, 0);
                this.NativeInterface.EditCurrentCell();
                //this.dataGridView.CurrentCell = this.dataGridView.Rows[index].Cells[0];
                //this.dataGridView.BeginEdit(true);
            }
            //this.dataGridView.Rows.Insert(index, parameter.ValueType.ToString(), this.CastParameterSuggestion(parameter.ParameterSuggestion));
            //this.NativeInterface.Refresh();
            if (previousCount >= 0)
            {
                this.UpdateMessageVisible();
            }
        }

        private void HandlePasteNewRows(object sender, EventArgs e)
        {
            if (!this.AllowPaste)
            {
                return;
            }

            string text = InternalGlobals.GuiManager.ToolkitHost.Clipboard.GetText();

            StringReader reader = new StringReader(text);

            List<string> errors = new List<string>();

            this.NativeInterface.BeginUpdate();

            while (reader.Peek() != -1)
            {
                string[] rowValues = reader.ReadLine().Split('\t');

                string errorCode;
                TItem item = this.CreateFromValues(rowValues, out errorCode);
                if (errorCode != null)
                {
                    if (!errors.Contains(errorCode))
                    {
                        errors.Add(errorCode);
                    }
                }
                else
                {
                    this.Add(item, false);
                }
            }

            this.NativeInterface.EndUpdate();

            if (errors.Count > 0)
            {
                StringBuilder builder = new StringBuilder();
                if (errors.Count != 1)
                {
                    builder.AppendLine(Localization.UIResources.FollowingErrorsOccurred);
                    builder.AppendLine();
                }

                foreach (string error in errors)
                {
                    if (error != null)
                    {
                        builder.AppendLine(Localization.UIResources.ResourceManager.GetString(error));
                    }
                }

                UIMessageBox.Show(
                    builder.ToString(),
                    Localization.Promptu.AppName,
                    UIMessageBoxButtons.OK,
                    UIMessageBoxIcon.Error,
                    UIMessageBoxResult.OK);
            }
        }

        private void Add(TItem item, bool select)
        {
            this.Insert(this.NativeInterface.RowCount, item, select);
        }

        private void HandleMoveUpMouseDown(object sender, EventArgs e)
        {
            this.TranslateSelectedItem(-1);
        }

        private void HandleMoveDownMouseDown(object sender, EventArgs e)
        {
            this.TranslateSelectedItem(1);
        }

        private void HandleAddMouseDown(object sender, EventArgs e)
        {
            this.AddDefault();
        }

        private void AddDefault()
        {
            this.Add(this.CreateDefault(), true);
        }

        private void HandleDeleteMouseDown(object sender, EventArgs e)
        {
            this.DoDelete();
        }

        private void MoveSelectionToRow(int row)
        {
            int column;

            if (this.NativeInterface.CurrentCell != null)
            {
                column = this.NativeInterface.CurrentCell.Value.Column;
            }
            else
            {
                column = 0;
            }

            this.NativeInterface.CurrentCell = new CellAddress(row, column);
        }

        private void DoDelete()
        {
            CellAddress? currentCell = this.NativeInterface.CurrentCell;
            if (currentCell != null)
            {
                List<int> rowsToDelete = new List<int>();
                foreach (CellAddress cell in this.NativeInterface.SelectedCells)
                {
                    if (!rowsToDelete.Contains(cell.Row))
                    {
                        rowsToDelete.Add(cell.Row);
                    }
                }

                this.DeleteRange(rowsToDelete);
                //for (int i
                //this.DeleteParameterAt(this.dataGridView.CurrentCell.RowIndex);
            }
        }

        private void TranslateSelectedItem(int distance)
        {
            CellAddress currentCell = this.NativeInterface.CurrentCell.Value;
            int currentLocation = currentCell.Row;
            int newLocation = currentLocation + distance;
            this.TranslateItem(currentLocation, newLocation);
            this.NativeInterface.ClearSelection();
            this.NativeInterface.CurrentCell = new CellAddress(newLocation, currentCell.Column);
            this.NativeInterface.Refresh();
        }

        private void HandleSelectionChanged(object sender, EventArgs e)
        {
            this.UpdateEnabledStates();
        }

        private void DeleteRange(List<int> indexes)
        {
            if (indexes.Count <= 0)
            {
                return;
            }

            indexes.Sort();
            indexes.Reverse();

            foreach (int index in indexes)
            {
                this.NativeInterface.RowCount--;
                this.RemoveItemAt(index);
            }

            //if (indexes.Count > 
            //this.NativeInterface.Refresh();

            if (this.collection.Count <= 0)
            {
                this.UpdateMessageVisible();
            }

            CellAddress? currentCell = this.NativeInterface.CurrentCell;

            int newIndex = currentCell == null ? 0 : currentCell.Value.Row;

            if (newIndex >= this.collection.Count && this.collection.Count > 0)
            {
                newIndex = this.collection.Count - 1;
            }
            else
            {
                return;
            }

            int column;

            if (currentCell != null)
            {
                column = currentCell.Value.Column;
            }
            else
            {
                column = 0;
            }

            this.NativeInterface.CurrentCell = new CellAddress(newIndex, column);
        }

        private void HandlePaste()
        {
            if (!this.AllowPaste || this.NativeInterface.SelectedCellsCount <= 0)
            {
                return;
            }

            string text = InternalGlobals.GuiManager.ToolkitHost.Clipboard.GetText();
            int? originColumn = null;
            int? originRow = null;
            int? endColumn = null;
            int? endRow = null;

            foreach (CellAddress cell in this.NativeInterface.SelectedCells)
            {
                if (originColumn == null || cell.Column < originColumn)
                {
                    originColumn = cell.Column;
                }

                if (originRow == null || cell.Row < originRow)
                {
                    originRow = cell.Row;
                }

                if (endColumn == null || cell.Column > endColumn)
                {
                    endColumn = cell.Column;
                }

                if (endRow == null || cell.Row > endRow)
                {
                    endRow = cell.Row;
                }
            }

            List<string> errors = new List<string>();

            StringReader reader = new StringReader(text);

            for (int row = originRow.Value; row <= endRow.Value; row++)
            {
                if (reader.Peek() == -1)
                {
                    break;
                }

                string[] rowValues = reader.ReadLine().Split('\t');

                //DataGridViewRow underlyingRow = this.DataGridView.Rows[row];

                for (int col = originColumn.Value; col <= endColumn.Value; col++)
                {
                    if (col < rowValues.Length)
                    {
                        //DataGridViewCell cell = underlyingRow.Cells[col];
                        if (this.NativeInterface.IsSelected(row, col))
                        {
                            //cell.Value = rowValues[col - originColumn.Value];
                            int? newSelectedIndex;
                            this.ChangeInformation(row, col, rowValues[col - originColumn.Value], errors, out newSelectedIndex);
                        }
                    }
                }
            }

            this.NativeInterface.Refresh();

            if (errors.Count > 0)
            {
                StringBuilder builder = new StringBuilder();
                if (errors.Count != 1)
                {
                    builder.AppendLine(Localization.UIResources.FollowingErrorsOccurred);
                    builder.AppendLine();
                }

                foreach (string error in errors)
                {
                    if (error != null)
                    {
                        builder.AppendLine(Localization.UIResources.ResourceManager.GetString(error));
                    }
                }

                UIMessageBox.Show(
                    builder.ToString(),
                    Localization.Promptu.AppName,
                    UIMessageBoxButtons.OK,
                    UIMessageBoxIcon.Error,
                    UIMessageBoxResult.OK);
            }
        }

        private void HandlePaste(object sender, EventArgs e)
        {
            this.HandlePaste();
        }

        private void HandleEditorKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control)
            {
                switch (e.KeyCode)
                {
                    case Keys.V:
                        this.HandlePaste();
                        e.Handled = true;
                        break;
                    default:
                        break;
                }
            }
            else if (e.Shift)
            {
                switch (e.KeyCode)
                {
                    case Keys.Delete:
                        this.DoDelete();
                        e.Handled = true;
                        break;
                    default:
                        break;
                }
            }
            else
            {
                switch (e.KeyCode)
                {
                    case Keys.Insert:
                        this.AddDefault();
                        e.Handled = true;
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
