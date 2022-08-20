using System;
using System.Collections.Generic;
using System.Text;
using ZachJohnson.Promptu.UserModel.Collections;
using ZachJohnson.Promptu.UserModel;
using ZachJohnson.Promptu.UIModel.Interfaces;
using System.Globalization;

namespace ZachJohnson.Promptu.UIModel.Presenters
{
    internal class CommandParameterMetaInfoPanelPresenter : CollectionEditorPresenter<CommandParameterMetaInfoCollection, CommandParameterMetaInfo>
    {
        private List<InternationalizedValue<SuggestionModeTypes>> suggestionModes =
            new List<InternationalizedValue<SuggestionModeTypes>>();

        private const int DefaultSuggestionModeIndex = 0;
        private const int FileSystemSuggestionModeIndex = 1;
        private const int ValueListSuggestionModeIndex = 2;
        private const int FunctionReturnSuggestionModeIndex = 3;
        private const int SuggestionColumnIndex = 2;
        private const int ConfigureColumnIndex = 3;
        public const int HistoryColumnIndex = 4;
        private ParameterlessVoid updateAllCallback;
        FunctionCollectionComposite prioritizedFunctions;
        private bool defaultShowHistory;

        // allow reorder
        public CommandParameterMetaInfoPanelPresenter(
            ICollectionEditor nativeInterface,
            ParameterlessVoid updateAllCallback,
            FunctionCollectionComposite prioritizedFunctions)
            : base(nativeInterface, true)
        {
            this.updateAllCallback = updateAllCallback;
            this.prioritizedFunctions = prioritizedFunctions;

            this.NativeInterface.Message = Localization.UIResources.CommandParameterInfoMessage;
            this.NativeInterface.AddButton.ToolTipText = Localization.UIResources.CommandParameterInfoAddToolTip;
            this.NativeInterface.DeleteButton.ToolTipText = Localization.UIResources.CommandParameterInfoDeleteToolTip;
            this.NativeInterface.MoveDownButton.ToolTipText = Localization.UIResources.CommandParameterInfoMoveDownToolTip;
            this.NativeInterface.MoveUpButton.ToolTipText = Localization.UIResources.CommandParameterInfoMoveUpToolTip;
            this.NativeInterface.Text = Localization.UIResources.CommandParameterInfoText;

            TextColumn numberOrRangeColumn = new TextColumn(
                Localization.UIResources.CommandParameterInfoNumberOrRange);

            suggestionModes.Add(new InternationalizedValue<SuggestionModeTypes>(
                SuggestionModeTypes.None,
                Localization.UIResources.SuggestionModeNone));
            suggestionModes.Add(new InternationalizedValue<SuggestionModeTypes>(
                SuggestionModeTypes.FileSystem,
                Localization.UIResources.SuggestionModeFileSystem));
            suggestionModes.Add(new InternationalizedValue<SuggestionModeTypes>(
                SuggestionModeTypes.ValueList,
                Localization.UIResources.SuggestionModeValueList));
            suggestionModes.Add(new InternationalizedValue<SuggestionModeTypes>(
                SuggestionModeTypes.FunctionReturnValue,
                Localization.UIResources.SuggestionModeFunctionReturn));

            TextColumn descriptionColumn = new TextColumn(
                Localization.UIResources.CommandParameterInfoDescription);

            ComboBoxColumn suggestionColumn = new ComboBoxColumn(
                Localization.UIResources.CommandParameterInfoSuggestionMode);
            suggestionColumn.Values.AddRange(this.suggestionModes.ToArray());

            CheckBoxColumn showHistoryColumn = new CheckBoxColumn(
                Localization.UIResources.CommandParameterInfoShowHistory);

            ButtonColumn configureSuggestionColumn = new ButtonColumn(String.Empty);

            //TextColumn fillColumn = new TextColumn(String.Empty);
            //fillColumn.ReadOnly = true;

            //DataGridViewTextBoxColumn numberOrRangeColumn = new DataGridViewTextBoxColumn();
            //numberOrRangeColumn.HeaderText = "Parameter Number or Range";

            //suggestionModes.Add("None");
            //suggestionModes.Add("File System");
            //suggestionModes.Add("Value List");
            //suggestionModes.Add("Function Return Value");

            //CustomDataGridViewTextBoxColumn descriptionColumn = new CustomDataGridViewTextBoxColumn();
            //descriptionColumn.HeaderText = "Description";
            //descriptionColumn.DefaultCellStyle.WrapMode = DataGridViewTriState.True;

            //DataGridViewComboBoxColumn suggestionColumn = new DataGridViewComboBoxColumn();
            //suggestionColumn.HeaderText = "Suggestion Mode";
            //suggestionColumn.Items.AddRange(suggestionModes.ToArray());

            //DataGridViewCheckBoxColumn showHistoryColumn = new DataGridViewCheckBoxColumn();
            //showHistoryColumn.HeaderText = "Show History?";
            //showHistoryColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader;

            //DataGridViewButtonColumn configureSuggestionColumn = new DataGridViewButtonColumn();
            //configureSuggestionColumn.CellTemplate = new CustomDataGridViewButtonCell();

            //DataGridViewTextBoxColumn fillColumn = new DataGridViewTextBoxColumn();
            //fillColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            //fillColumn.ReadOnly = true;
            //fillColumn.MinimumWidth = 2;

            this.NativeInterface.InsertColumn(0, numberOrRangeColumn);
            this.NativeInterface.InsertColumn(1, descriptionColumn);
            this.NativeInterface.InsertColumn(2, suggestionColumn);
            this.NativeInterface.InsertColumn(3, configureSuggestionColumn);
            this.NativeInterface.InsertColumn(4, showHistoryColumn);
            //this.NativeInterface.InsertColumn(5, fillColumn);

            this.NativeInterface.CellClicked += this.HandleCellClick;
            this.NativeInterface.CellValidating += this.HandleCellValidating;
        }

        public bool DefaultShowHistory
        {
            get { return this.defaultShowHistory; }
            set { this.defaultShowHistory = value; }
        }

        protected override CommandParameterMetaInfo CreateFromValues(string[] values, out string errorCode)
        {
            throw new NotImplementedException();
        }

        private void HandleCellValidating(object sender, CellValidatingEventArgs e)
        {
            if (e.Column == 0)
            {
                try
                {
                    int firstNumber;
                    int? lastNumber;
                    bool valueIsRange;
                    CommandParameterMetaInfo.ConvertParameterNumberOrRange(e.FormattedValue.ToString(), out firstNumber, out lastNumber, out valueIsRange);
                }
                catch (FormatException ex)
                {
                    if (!this.NativeInterface.DoMinimalCellValidating)
                    {
                        UIMessageBox.Show(
                            ex.Message,
                            Localization.Promptu.AppName,
                            UIMessageBoxButtons.OK,
                            UIMessageBoxIcon.Error,
                            UIMessageBoxResult.OK);
                    }

                    e.Cancel = true;
                }
                catch (ArgumentException ex)
                {
                    if (!this.NativeInterface.DoMinimalCellValidating)
                    {
                        UIMessageBox.Show(
                            ex.Message,
                            Localization.Promptu.AppName,
                            UIMessageBoxButtons.OK,
                            UIMessageBoxIcon.Error,
                            UIMessageBoxResult.OK);
                    }

                    e.Cancel = true;
                }
            }
        }

        //protected override bool AutoEndEditForCurrentCell()
        //{
        //    CellAddress? cell = this.NativeInterface.CurrentCell;
        //    if (cell != null)
        //    {
        //        switch (cell.Value.Column)
        //        {
        //            case 0:
        //            case 1:
        //                return false;
        //            default:
        //                break;
        //        }
        //    }

        //    return true;
        //}

        protected override void RemoveItemAt(int index)
        {
            this.Collection.RemoveAt(index);
        }

        protected override CommandParameterMetaInfo CreateDefault()
        {
            return new CommandParameterMetaInfo(
                null, 
                1, 
                false, 
                null, 
                String.Empty, 
                null, 
                this.defaultShowHistory);
        }

        protected override void TranslateItem(int index, int indexTo)
        {
            CommandParameterMetaInfo item = this.Collection[index];
            this.Collection.RemoveAt(index);
            this.Collection.Insert(indexTo, item);
            this.UpdateEnabledStates();
        }

        protected override void InsertItem(ref int index, CommandParameterMetaInfo item)
        {
            this.Collection.Insert(index, item);
        }

        protected override CommandParameterMetaInfoCollection CloneCollection(
            CommandParameterMetaInfoCollection collection)
        {
            return collection.Clone();
        }

        protected override void ChangeInformation(int row, int column, object value, List<string> errorCatcher, out int? newSelectedIndex)
        {
            newSelectedIndex = null;
            if (value == null || row < 0 || column < 0 || row >= this.Collection.Count || column >= this.NativeInterface.ColumnCount)
            {
                return;
            }

            CommandParameterMetaInfo old = this.Collection[row];

            int firstParameter = old.FirstParameter;
            int? lastParameter = old.LastParameter;
            bool isRange = old.IsRange;
            string description = old.Description;
            string display = old.Display;
            ParameterSuggestion suggestion = old.ParameterSuggestion;
            bool showHistory = old.ShowHistory;

            switch (column)
            {
                case 0:
                    try
                    {
                        int firstNumber;
                        int? lastNumber;
                        bool valueIsRange;
                        CommandParameterMetaInfo.ConvertParameterNumberOrRange(value.ToString(), out firstNumber, out lastNumber, out valueIsRange);
                        firstParameter = firstNumber;
                        lastParameter = lastNumber;
                        isRange = valueIsRange;
                        display = value.ToString();
                    }
                    catch (FormatException)
                    {
                    }
                    catch (ArgumentException)
                    {
                    }

                    break;
                case 1:
                    description = value.ToString();
                    break;
                case 2:
                    CellAddress buttonCellAddress = new CellAddress(row, ConfigureColumnIndex);
                    //CustomDataGridViewButtonCell cell = (CustomDataGridViewButtonCell)this.DataGridView[ConfigureColumnIndex, row];
                    bool showButton = this.NativeInterface.IsVisibleButton(buttonCellAddress);;
                    if (CastParameterSuggestion(suggestion) != value)
                    {
                        switch (((InternationalizedValue<SuggestionModeTypes>)value).Value)
                        {
                            case SuggestionModeTypes.ValueList:
                                ValueListSelectorWindowPresenter valueListSelector = new ValueListSelectorWindowPresenter(this.updateAllCallback);
                                InternalGlobals.UISettings.ValueListSelectorSettings.ImpartTo(valueListSelector.NativeInterface);

                                if (valueListSelector.ShowDialog() == UIDialogResult.OK)
                                {
                                    suggestion = new ValueListParameterSuggestion(valueListSelector.SelectedValueListName);
                                    showButton = true;
                                }

                                InternalGlobals.UISettings.ValueListSelectorSettings.UpdateFrom(valueListSelector.NativeInterface);
                                break;
                            case SuggestionModeTypes.FunctionReturnValue:
                                FunctionInvocationEditorPresenter functionInvocationEditor = new FunctionInvocationEditorPresenter(String.Empty, this.prioritizedFunctions, firstParameter);
                                InternalGlobals.UISettings.FunctionInvocationEditorSettings.ImpartTo(functionInvocationEditor.NativeInterface);
                                if (functionInvocationEditor.ShowDialog() == UIDialogResult.OK)
                                {
                                    suggestion = new FunctionReturnParameterSuggestion(functionInvocationEditor.Expression);
                                    showButton = true;
                                }

                                InternalGlobals.UISettings.FunctionInvocationEditorSettings.UpdateFrom(functionInvocationEditor.NativeInterface);

                                break;
                            case SuggestionModeTypes.FileSystem:
                                //FileSystemParameterSuggestionEditor fileSystemParameterSuggestionEditor = new FileSystemParameterSuggestionEditor(String.Empty, this.prioritizedFunctions, firstParameter);
                                //PromptuSettings.CurrentProfile.UISettings.FileSystemParameterSuggestionEditorSettings.ImpartTo(fileSystemParameterSuggestionEditor);
                                //if (fileSystemParameterSuggestionEditor.ShowDialog() == DialogResult.OK)
                                //{
                                suggestion = new FileSystemParameterSuggestion(string.Empty);
                                showButton = true;
                                //}

                                //PromptuSettings.CurrentProfile.UISettings.FileSystemParameterSuggestionEditorSettings.UpdateFrom(fileSystemParameterSuggestionEditor);

                                break;
                            default:
                                suggestion = null;
                                showButton = false;
                                break;
                        }
                    }

                    this.NativeInterface.SetButtonVisibility(buttonCellAddress, showButton);
                    this.NativeInterface.InvalidateRow(row);

                    break;
                case 4:
                    try
                    {
                        showHistory = Convert.ToBoolean(value, CultureInfo.InvariantCulture);
                    }
                    catch (FormatException)
                    {
                    }
                    catch (InvalidCastException)
                    {
                    }

                    break;
                default:
                    return;
            }

            this.Collection.RemoveAt(row);
            this.Collection.Insert(row, new CommandParameterMetaInfo(display, firstParameter, isRange, lastParameter, description, suggestion, showHistory));
        }

        private InternationalizedValue<SuggestionModeTypes> CastParameterSuggestion(ParameterSuggestion suggestion)
        {
            if (suggestion is FileSystemParameterSuggestion)
            {
                return suggestionModes[FileSystemSuggestionModeIndex];
            }
            else if (suggestion is FunctionReturnParameterSuggestion)
            {
                return suggestionModes[FunctionReturnSuggestionModeIndex];
            }
            else if (suggestion is ValueListParameterSuggestion)
            {
                return suggestionModes[ValueListSuggestionModeIndex];
            }

            return suggestionModes[DefaultSuggestionModeIndex];
        }

        protected override object RequestInformation(int row, int column)
        {
            if (row < 0 || column < 0 || row >= this.Collection.Count || column >= this.NativeInterface.ColumnCount)
            {
                return null;
            }

            CommandParameterMetaInfo parameter = this.Collection[row];

            switch (column)
            {
                case 0:
                    return parameter.Display;
                case 1:
                    return parameter.Description;
                case 2:
                    return CastParameterSuggestion(parameter.ParameterSuggestion);
                case 4:
                    return parameter.ShowHistory;
                case 3:
                    CellAddress buttonCellAddress = new CellAddress(row, ConfigureColumnIndex);
                    //DataGridViewRow physicalRow = this.DataGridView.Rows[row];
                    switch (((InternationalizedValue<SuggestionModeTypes>)this.RequestInformation(row, SuggestionColumnIndex)).Value)
                    {
                        case SuggestionModeTypes.ValueList:
                            //((CustomDataGridViewButtonCell)physicalRow.Cells[ConfigureColumnIndex]).Show = true;
                            this.NativeInterface.SetButtonVisibility(buttonCellAddress, true);

                            return String.Format(
                                CultureInfo.CurrentCulture, 
                                Localization.UIResources.ValueListSuggestionModeDisplayFormat,
                                ((ValueListParameterSuggestion)this.Collection[row].ParameterSuggestion).ValueListName);
                        case SuggestionModeTypes.FunctionReturnValue:
                            //((CustomDataGridViewButtonCell)physicalRow.Cells[ConfigureColumnIndex]).Show = true;
                            this.NativeInterface.SetButtonVisibility(buttonCellAddress, true);

                            return ((FunctionReturnParameterSuggestion)this.Collection[row].ParameterSuggestion).Expression;
                        case SuggestionModeTypes.FileSystem:
                            //((CustomDataGridViewButtonCell)physicalRow.Cells[ConfigureColumnIndex]).Show = true;
                            this.NativeInterface.SetButtonVisibility(buttonCellAddress, true);

                            return ((FileSystemParameterSuggestion)this.Collection[row].ParameterSuggestion).GetDisplay();
                        default:
                            bool buttonVisible = this.NativeInterface.IsVisibleButton(buttonCellAddress);
                            if (buttonVisible)
                            {
                                this.NativeInterface.SetButtonVisibility(buttonCellAddress, false);
                                this.NativeInterface.InvalidateCell(buttonCellAddress);
                            }

                            break;
                    }

                    return Localization.UIResources.DefaultConfigureSuggestionMode;
                default:
                    return null;
            }
        }

        private void HandleCellClick(object sender, CellEventArgs e)
        {
            if (e.Row >= 0 && e.Column == ConfigureColumnIndex)
            {
                //CustomDataGridViewButtonCell cell = this.DataGridView[e.ColumnIndex, e.RowIndex] as CustomDataGridViewButtonCell;

                //if (cell != null && cell.Show)
                if (this.NativeInterface.IsVisibleButton(new CellAddress(e.Row, e.Column)))
                {
                    CommandParameterMetaInfo parameter = this.Collection[e.Row];

                    ValueListParameterSuggestion valueListParameterSuggestion = parameter.ParameterSuggestion as ValueListParameterSuggestion;
                    FunctionReturnParameterSuggestion functionReturnParameterSuggestion;
                    FileSystemParameterSuggestion fileSystemParameterSuggestion;

                    if (valueListParameterSuggestion != null)
                    {
                        ValueListSelectorWindowPresenter selectorWindow = new ValueListSelectorWindowPresenter(this.updateAllCallback);
                        InternalGlobals.UISettings.ValueListSelectorSettings.ImpartTo(selectorWindow.NativeInterface);
                        selectorWindow.SelectedValueListName = valueListParameterSuggestion.ValueListName;
                        //selectorWindow.Select();
                        //selectorWindow.ValueListSetupPanel.ItemsListView.Focus();
                        selectorWindow.NativeInterface.ValueListSetupPanel.CollectionViewer.Select();

                        if (selectorWindow.ShowDialog() == UIDialogResult.OK)
                        {
                            this.Collection.RemoveAt(e.Row);
                            this.Collection.Insert(e.Row, new CommandParameterMetaInfo(parameter.Display, parameter.FirstParameter, parameter.IsRange, parameter.LastParameter, parameter.Description, new ValueListParameterSuggestion(selectorWindow.SelectedValueListName), parameter.ShowHistory));
                            this.NativeInterface.InvalidateCell(new CellAddress(e.Row, e.Column));
                        }

                        InternalGlobals.UISettings.ValueListSelectorSettings.UpdateFrom(selectorWindow.NativeInterface);
                    }
                    else if ((functionReturnParameterSuggestion = parameter.ParameterSuggestion as FunctionReturnParameterSuggestion) != null)
                    {
                        FunctionInvocationEditorPresenter selectorWindow = new FunctionInvocationEditorPresenter(functionReturnParameterSuggestion.Expression, this.prioritizedFunctions, parameter.FirstParameter);
                        InternalGlobals.UISettings.FunctionInvocationEditorSettings.ImpartTo(selectorWindow.NativeInterface);

                        if (selectorWindow.ShowDialog() == UIDialogResult.OK)
                        {
                            this.Collection.RemoveAt(e.Row);
                            this.Collection.Insert(e.Row, new CommandParameterMetaInfo(parameter.Display, parameter.FirstParameter, parameter.IsRange, parameter.LastParameter, parameter.Description, new FunctionReturnParameterSuggestion(selectorWindow.Expression), parameter.ShowHistory));
                            this.NativeInterface.InvalidateCell(new CellAddress(e.Row, e.Column));
                        }

                        InternalGlobals.UISettings.FunctionInvocationEditorSettings.UpdateFrom(selectorWindow.NativeInterface);
                    }
                    else if ((fileSystemParameterSuggestion = parameter.ParameterSuggestion as FileSystemParameterSuggestion) != null)
                    {
                        FileSystemParameterSuggestionEditorPresenter fileSystemParameterSuggestionEditor = new FileSystemParameterSuggestionEditorPresenter(fileSystemParameterSuggestion.Filter, this.prioritizedFunctions, parameter.FirstParameter);
                        InternalGlobals.UISettings.FileSystemParameterSuggestionEditorSettings.ImpartTo(fileSystemParameterSuggestionEditor.NativeInterface);
                        if (fileSystemParameterSuggestionEditor.ShowDialog() == UIDialogResult.OK)
                        {
                            this.Collection.RemoveAt(e.Row);
                            this.Collection.Insert(e.Row, new CommandParameterMetaInfo(parameter.Display, parameter.FirstParameter, parameter.IsRange, parameter.LastParameter, parameter.Description, new FileSystemParameterSuggestion(fileSystemParameterSuggestionEditor.Filter), parameter.ShowHistory));
                            this.NativeInterface.InvalidateCell(new CellAddress(e.Row, e.Column));
                        }

                        InternalGlobals.UISettings.FileSystemParameterSuggestionEditorSettings.UpdateFrom(fileSystemParameterSuggestionEditor.NativeInterface);
                    }
                }
            }
        }
    }
}
