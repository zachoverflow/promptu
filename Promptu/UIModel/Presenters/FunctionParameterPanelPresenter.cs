using System;
using System.Collections.Generic;
using System.Text;
using ZachJohnson.Promptu.UserModel.Collections;
using ZachJohnson.Promptu.UserModel;
using ZachJohnson.Promptu.UIModel.Interfaces;
using System.Globalization;

namespace ZachJohnson.Promptu.UIModel.Presenters
{
    internal class FunctionParameterPanelPresenter : CollectionEditorPresenter<FunctionParameterCollection, FunctionParameter>
    {
        private List<InternationalizedValue<SuggestionModeTypes>> suggestionModes = 
            new List<InternationalizedValue<SuggestionModeTypes>>();

        private const int DefaultSuggestionModeIndex = 0;
        private const int FileSystemSuggestionModeIndex = 1;
        private const int ValueListSuggestionModeIndex = 2;
        private const int FunctionReturnSuggestionModeIndex = 3;
        private const int SuggestionColumnIndex = 1;
        private const int ConfigureColumnIndex = 2;
        internal const int HistoryColumnIndex = 3;
        private ParameterlessVoid updateAllCallback;
        FunctionCollectionComposite prioritizedFunctions;

        public FunctionParameterPanelPresenter(ICollectionEditor nativeInterface, ParameterlessVoid updateAllCallback, FunctionCollectionComposite prioritizedFunctions)
            : base(nativeInterface, false)
        {
            this.updateAllCallback = updateAllCallback;
            this.prioritizedFunctions = prioritizedFunctions;

            this.NativeInterface.AddButton.ToolTipText = Localization.UIResources.AddParameterToolTip;
            this.NativeInterface.DeleteButton.ToolTipText = Localization.UIResources.DeleteParameterToolTip;
            this.NativeInterface.MoveDownButton.ToolTipText = Localization.UIResources.MoveParameterDownToolTip;
            this.NativeInterface.MoveUpButton.ToolTipText = Localization.UIResources.MoveParameterUpToolTip;
            this.NativeInterface.Text = Localization.UIResources.FunctionParameterPanelText;

            TextColumn valueColumn = new TextColumn(
                Localization.UIResources.FunctionParameterPanelValueTypeHeaderText);
            valueColumn.ReadOnly = true;

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

            ComboBoxColumn suggestionColumn = new ComboBoxColumn(
                Localization.UIResources.SuggestionModeColumnHeaderText);
            suggestionColumn.Values.AddRange(this.suggestionModes.ToArray());

            CheckBoxColumn showHistoryColumn = new CheckBoxColumn(
                Localization.UIResources.ShowHistoryColumnHeaderText);

            ButtonColumn configureSuggestionColumn = new ButtonColumn(null);

            //TextColumn fillColumn = new TextColumn(null);
            //fillColumn.ReadOnly = true;
            //// 

            this.NativeInterface.InsertColumn(0, valueColumn);
            this.NativeInterface.InsertColumn(SuggestionColumnIndex, suggestionColumn);
            this.NativeInterface.InsertColumn(ConfigureColumnIndex, configureSuggestionColumn);
            this.NativeInterface.InsertColumn(HistoryColumnIndex, showHistoryColumn);
            //this.NativeInterface.InsertColumn(4, fillColumn);

            this.NativeInterface.CellClicked += this.HandleCellClick;
        }

        protected override FunctionParameter CreateFromValues(string[] values, out string errorCode)
        {
            throw new NotImplementedException();
        }

        private void HandleCellClick(object sender, CellEventArgs e)
        {
            if (e.Row >= 0 && e.Column == ConfigureColumnIndex)
            {
                //CustomDataGridViewButtonCell cell = this.DataGridView[e.ColumnIndex, e.RowIndex] as CustomDataGridViewButtonCell;

                //if (cell != null && cell.Show)
                if (this.NativeInterface.IsVisibleButton(new CellAddress(e.Row, e.Column)))
                {
                    FunctionParameter parameter = this.Collection[e.Row];

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
                            this.Collection.Insert(e.Row, new FunctionParameter(parameter.ValueType, new ValueListParameterSuggestion(selectorWindow.SelectedValueListName), parameter.ShowHistory));
                            this.NativeInterface.InvalidateCell(new CellAddress(e.Row, e.Column));
                        }

                        InternalGlobals.UISettings.ValueListSelectorSettings.UpdateFrom(selectorWindow.NativeInterface);
                    }
                    else if ((functionReturnParameterSuggestion = parameter.ParameterSuggestion as FunctionReturnParameterSuggestion) != null)
                    {
                        FunctionInvocationEditorPresenter selectorWindow = new FunctionInvocationEditorPresenter(functionReturnParameterSuggestion.Expression, this.prioritizedFunctions, e.Row + 1);
                        InternalGlobals.UISettings.FunctionInvocationEditorSettings.ImpartTo(selectorWindow.NativeInterface);

                        if (selectorWindow.ShowDialog() == UIDialogResult.OK)
                        {
                            this.Collection.RemoveAt(e.Row);
                            this.Collection.Insert(e.Row, new FunctionParameter(parameter.ValueType, new FunctionReturnParameterSuggestion(selectorWindow.Expression), parameter.ShowHistory));
                            this.NativeInterface.InvalidateCell(new CellAddress(e.Row, e.Column));
                        }

                        InternalGlobals.UISettings.FunctionInvocationEditorSettings.UpdateFrom(selectorWindow.NativeInterface);
                    }
                    else if ((fileSystemParameterSuggestion = parameter.ParameterSuggestion as FileSystemParameterSuggestion) != null)
                    {
                        FileSystemParameterSuggestionEditorPresenter fileSystemParameterSuggestionEditor = new FileSystemParameterSuggestionEditorPresenter(fileSystemParameterSuggestion.Filter, this.prioritizedFunctions, e.Row + 1);
                        InternalGlobals.UISettings.FileSystemParameterSuggestionEditorSettings.ImpartTo(fileSystemParameterSuggestionEditor.NativeInterface);
                        if (fileSystemParameterSuggestionEditor.ShowDialog() == UIDialogResult.OK)
                        {
                            this.Collection.RemoveAt(e.Row);
                            this.Collection.Insert(e.Row, new FunctionParameter(parameter.ValueType, new FileSystemParameterSuggestion(fileSystemParameterSuggestionEditor.Filter), parameter.ShowHistory));
                            this.NativeInterface.InvalidateCell(new CellAddress(e.Row, e.Column));
                        }

                        InternalGlobals.UISettings.FileSystemParameterSuggestionEditorSettings.UpdateFrom(fileSystemParameterSuggestionEditor.NativeInterface);
                    }
                }
            }
        }

        //protected override bool AutoEndEditForCurrentCell()
        //{
        //    return true;
        //}

        protected override FunctionParameter CreateDefault()
        {
            return new FunctionParameter(FunctionParameterValueType.String, null, false);
        }

        protected override FunctionParameterCollection CloneCollection(FunctionParameterCollection collection)
        {
            return collection.Clone();
        }

        protected override void RemoveItemAt(int index)
        {
            this.Collection.RemoveAt(index);
        }

        protected override void TranslateItem(int index, int indexTo)
        {
            FunctionParameter parameter = this.Collection[index];
            this.Collection.RemoveAt(index);
            this.Collection.Insert(indexTo, parameter);
        }

        protected override void InsertItem(ref int index, FunctionParameter item)
        {
            this.Collection.Insert(index, item);
        }

        protected override void ChangeInformation(int row, int column, object value, List<string> errorCatcher, out int? newSelectedIndex)
        {
            newSelectedIndex = null;
            if (value == null || row < 0 || column < 0 || row >= this.Collection.Count || column >= this.NativeInterface.ColumnCount)
            {
                return;
            }

            FunctionParameter old = this.Collection[row];

            FunctionParameterValueType valueType = old.ValueType;
            ParameterSuggestion suggestion = old.ParameterSuggestion;
            bool showHistory = old.ShowHistory;

            switch (column)
            {
                case 0:
                    valueType = (FunctionParameterValueType)Enum.Parse(typeof(FunctionParameterValueType), value.ToString());
                    break;
                case 1:
                    CellAddress buttonCellAddress = new CellAddress(row, ConfigureColumnIndex);
                    //CustomDataGridViewButtonCell cell = (CustomDataGridViewButtonCell)this.DataGridView[ConfigureColumnIndex, row];
                    bool showButton = this.NativeInterface.IsVisibleButton(buttonCellAddress);//cell.Show;
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
                                FunctionInvocationEditorPresenter functionInvocationEditor = new FunctionInvocationEditorPresenter(String.Empty, this.prioritizedFunctions, row + 1);
                                InternalGlobals.UISettings.FunctionInvocationEditorSettings.ImpartTo(functionInvocationEditor.NativeInterface);

                                if (functionInvocationEditor.ShowDialog() == UIDialogResult.OK)
                                {
                                    suggestion = new FunctionReturnParameterSuggestion(functionInvocationEditor.Expression);
                                    showButton = true;
                                }

                                InternalGlobals.UISettings.FunctionInvocationEditorSettings.UpdateFrom(functionInvocationEditor.NativeInterface);

                                break;
                            case SuggestionModeTypes.FileSystem:
                                //FileSystemParameterSuggestionEditor fileSystemParameterSuggestionEditor = new FileSystemParameterSuggestionEditor(String.Empty, this.prioritizedFunctions, row + 1);
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
                case 3:
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
            this.Collection.Insert(row, new FunctionParameter(valueType, suggestion, showHistory));

            //FunctionParameter parameter = this.parameters[row];
            //FunctionParameter parameter = this.AssembleParameterFromRow(row);
            //this.parameters.RemoveAt(row);
            //this.parameters.Insert(row, parameter);

            //FunctionParameterValueType valueType = FunctionParameterValueType.String;

            //switch (column)
            //{
            //    case 0:
            //        return enum.;
            //    case 1:
            //        return CastParameterSuggestion(parameter.ParameterSuggestion);
            //    default:
            //        return;
            //}
        }

        protected override object RequestInformation(int row, int column)
        {
            if (row < 0 || column < 0 || row >= this.Collection.Count || column >= this.NativeInterface.ColumnCount)
            {
                return null;
            }

            FunctionParameter parameter = this.Collection[row];

            switch (column)
            {
                case 0:
                    return parameter.ValueType.ToString();
                case 1:
                    return CastParameterSuggestion(parameter.ParameterSuggestion);
                case 3:
                    return parameter.ShowHistory;
                case 2:
                    CellAddress buttonCellAddress = new CellAddress(row, ConfigureColumnIndex);
                    //DataGridViewRow physicalRow = this.DataGridView.Rows[row];
                    switch (((InternationalizedValue<SuggestionModeTypes>)this.RequestInformation(row, 1)).Value)
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
                            //CustomDataGridViewButtonCell buttonCell = (CustomDataGridViewButtonCell)physicalRow.Cells[ConfigureColumnIndex];
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
    }
}
