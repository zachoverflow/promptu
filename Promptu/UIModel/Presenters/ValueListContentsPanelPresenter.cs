using System;
using System.Collections.Generic;
using System.Text;
using ZachJohnson.Promptu.UserModel;
using ZachJohnson.Promptu.UIModel.Interfaces;
using System.Globalization;

namespace ZachJohnson.Promptu.UIModel.Presenters
{
    internal class ValueListContentsPanelPresenter : CollectionEditorPresenter<ValueList, ValueListItem>
    {
        public ValueListContentsPanelPresenter(ICollectionEditor nativeInterface)
            : base(nativeInterface, false)
        {
            this.NativeInterface.AddButton.ToolTipText = Localization.UIResources.ValueListItemAddToolTip;
            this.NativeInterface.DeleteButton.ToolTipText = Localization.UIResources.ValueListItemDeleteToolTip;
            this.NativeInterface.Text = Localization.UIResources.ValueListContentsPanelText;
            this.NativeInterface.PasteNewRowsButton.ToolTipText = Localization.UIResources.ValueListItemPasteNewRowsToolTip;

            this.AllowPaste = true;

            this.NativeInterface.MoveDownButton.Available = false;
            this.NativeInterface.MoveUpButton.Available = false;

            this.NativeInterface.InsertColumn(
                0, 
                new TextColumn(Localization.UIResources.ValueListContentsValueHeaderText));

            this.NativeInterface.InsertColumn(
                1,
                new TextColumn(Localization.UIResources.ValueListContentsTranslationHeaderText));

            this.NativeInterface.SetColumnVisibility(1, false);

            //this.NativeInterface.InsertColumn(
            //    2,
            //    new TextColumn(null));
            //fillColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            //fillColumn.ReadOnly = true;
            //fillColumn.MinimumWidth = 2;
        }

        public bool ShowTranslationColumn
        {
            get
            {
                return this.NativeInterface.IsVisibleColumn(1);
            }

            set
            {
                if (value != ShowTranslationColumn)
                {
                    this.NativeInterface.SetColumnVisibility(1, value);
                }
            }
        }

        protected override void PostAssignItems()
        {
            this.Collection.Sort();
        }

        protected override ValueList CloneCollection(ValueList collection)
        {
            return collection.Clone();
        }

        protected override void TranslateItem(int index, int indexTo)
        {
            throw new NotSupportedException("TranslateItem is not supported.");
        }

        //protected override bool AutoEndEditForCurrentCell()
        //{
        //    return false;
        //}

        protected override void InsertItem(ref int index, ValueListItem item)
        {
            this.Collection.Add(item);
            //this.Collection.Sort();
            index = this.Collection.IndexOf(item.Value);
        }

        protected override void RemoveItemAt(int index)
        {
            this.Collection.RemoveAt(index);
        }

        protected override ValueListItem CreateDefault()
        {
            string defaultValue = GeneralUtilities.GetAvailableIncrementingName(this.Collection, new NameGetter<ValueListItem>(this.GetValueFrom), "Value{+}", "({number})", false, InsertBase.Two);
            return new ValueListItem(defaultValue, String.Empty, true, -1);
        }

        protected override ValueListItem CreateFromValues(string[] values, out string errorCode)
        {
            string value = values.Length > 0 ? values[0] : GeneralUtilities.GetAvailableIncrementingName(this.Collection, new NameGetter<ValueListItem>(this.GetValueFrom), "Value{+}", "({number})", false, InsertBase.Two);
            string translation = values.Length > 1 ? values[1] : String.Empty;

            if (this.Collection.ContainsValue(value))
            {
                errorCode = "ValueListValuesAlreadyExist";
                return null;
            }

            errorCode = null;

            return new ValueListItem(value, translation, true, -1);
        }

        protected override object RequestInformation(int row, int column)
        {
            if (row < 0 || column < 0 || row >= this.Collection.Count || column >= this.NativeInterface.ColumnCount)
            {
                return null;
            }

            ValueListItem item = this.Collection[row];

            switch (column)
            {
                case 0:
                    return item.Value;
                case 1:
                    if (this.ShowTranslationColumn)
                    {
                        return item.Translation;
                    }

                    break;
                default:
                    return null;
            }

            return null;
        }

        protected override void ChangeInformation(int row, int column, object value, List<string> errorCodes, out int? newSelectedIndex)
        {
            newSelectedIndex = null;

            if ((value == null && column != 1) || row < 0 || column < 0 || row >= this.Collection.Count || column >= this.NativeInterface.ColumnCount)
            {
                return;
            }

            ValueListItem old = this.Collection[row];

            string itemValue = old.Value;
            string translation = old.Translation;

            switch (column)
            {
                case 0:
                    string newValue = value.ToString();
                    if (itemValue != newValue)
                    {
                        if (itemValue.ToUpperInvariant() != newValue.ToUpperInvariant() && this.Collection.ContainsValue(newValue))
                        {
                            if (errorCodes == null)
                            {
                                UIMessageBox.Show(
                                    String.Format(CultureInfo.CurrentCulture, Localization.UIResources.ValueListValueAlreadyExists, newValue),
                                    Localization.Promptu.AppName,
                                    UIMessageBoxButtons.OK,
                                    UIMessageBoxIcon.Error,
                                    UIMessageBoxResult.OK);
                            }
                            else
                            {
                                errorCodes.Add("ValueListValuesAlreadyExist");
                            }

                            return;
                        }

                        itemValue = newValue;
                    }

                    break;
                case 1:
                    if (this.ShowTranslationColumn)
                    {
                        if (value == null)
                        {
                            translation = String.Empty;
                        }
                        else
                        {
                            translation = value.ToString();
                        }
                    }

                    break;
                default:
                    return;
            }

            this.Collection.RemoveAt(row);
            this.Collection.Insert(row, new ValueListItem(itemValue, translation, true, -1));

            newSelectedIndex = this.Collection.IndexOf(itemValue);
        }

        private string GetValueFrom(ValueListItem item)
        {
            return item.Value;
        }
    }
}
