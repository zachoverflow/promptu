using System;
using System.Collections.Generic;
using System.Text;
using ZachJohnson.Promptu.UserModel;
using ZachJohnson.Promptu.UIModel.Interfaces;
using System.Globalization;

namespace ZachJohnson.Promptu.UIModel.Presenters
{
    internal class ValueListSetupPanelPresenter : SetupPanelPresenter<ValueList>
    {
        public ValueListSetupPanelPresenter(ParameterlessVoid updateAllCallback, ParameterlessVoid settingChangedCallback)
            : this(
            InternalGlobals.GuiManager.ToolkitHost.Factory.ConstructSetupPanel(settingChangedCallback),
            updateAllCallback)
        {
        }

        public ValueListSetupPanelPresenter(ISetupPanel uiObject, ParameterlessVoid updateAllCallback)
            : base(uiObject, updateAllCallback)
        {
            //this.NativeInterface.NewButton.ToolTipText = Localization.UIResources.ValueListSetupPanelNewToolTip;
            this.NativeInterface.NewButton.Image = InternalGlobals.GuiManager.ToolkitHost.Images.NewValueList;

            this.CollectionPresenter.Headers.Add(
                Localization.UIResources.ValueListNameHeaderText,
                this.GetValueListName);

            this.CollectionPresenter.Headers.Add(
                Localization.UIResources.ValueListContentsHeaderText,
                this.GetValueListContents);

            this.CollectionPresenter.Headers.Add(
                Localization.UIResources.ValueListTranslationHeaderText,
                this.GetValueListItemTranslations);

            this.CollectionPresenter.Headers.Add(
                Localization.UIResources.ValueListNamespaceInterpretationHeaderText,
                this.GetValueListNamespaceInterpretation);

            //this.nameColumn.Text = "Name";
            //this.nameColumn.Width = 181;

            //this.contentsColumn.Text = "Contents";
            //this.contentsColumn.Width = 300;

            //this.translateColumn.Text = "Value Translation";
            //this.translateColumn.Width = 100;

            //this.namespaceInterpretationColumn.Text = "Namespace Interpretation";
            //this.namespaceInterpretationColumn.Width = 140;

            this.ItemCode = UIModel.ItemCode.ValueList;
        }

        private object GetValueListName(object obj)
        {
            ValueList item = (ValueList)obj;
            return item.Name;
        }

        private object GetValueListContents(object obj)
        {
            ValueList item = (ValueList)obj;
            return FormatContents(item);
        }

        private object GetValueListItemTranslations(object obj)
        {
            ValueList item = (ValueList)obj;
            return ConvertToYesNo(item.UseItemTranslations);
        }

        private object GetValueListNamespaceInterpretation(object obj)
        {
            ValueList item = (ValueList)obj;
            return ConvertToYesNo(item.UseNamespaceInterpretation);
        }

        protected override void UpdateItemsListViewCore()
        {
            List<ValueList> sorted = new List<ValueList>();
            using (DdMonitor.Lock(this.ListUsing.ValueLists))
            {
                sorted.AddRange(this.ListUsing.ValueLists);
            }

            sorted.Sort(new Comparison<ValueList>(this.ValueListComparison));
            foreach (ValueList valueList in sorted)
            {
                //List<string> values = new List<string>(this.CollectionPresenter.Headers.Count);
                ////item = new ListViewItem();
                ////item.IndentCount = 0;
                //values.Add(valueList.Name);
                ////*item.Name = valueList.Name;
                //values.Add(FormatContents(valueList));
                //values.Add(ConvertToYesNo(valueList.UseItemTranslations));
                //values.Add(ConvertToYesNo(valueList.UseNamespaceInterpretation));
                this.CollectionPresenter.Add(valueList);
            }
        }

        public int IndexOf(string name)
        {
            return this.CollectionPresenter.IndexOf(this.ListUsing.ValueLists.TryGet(name));
        }

        protected override string GetPluralItemDisplayFormat()
        {
            return Localization.UIResources.ValueListCountPluralFormat;
        }

        protected override string GetSingularItemDisplayFormat()
        {
            return Localization.UIResources.ValueListCountSingularFormat;
        }

        protected override ValueList GetItem(int index)
        {
            if (index >= 0 && index < this.CollectionPresenter.Count)
            {
                return (ValueList)this.CollectionPresenter[index];
            }

            return null;
        }

        protected override ValueList EditSelectedItemCore()
        {
            if (this.CollectionPresenter.SelectedIndexes.Count > 0)
            {
                bool edited;
                ValueList valueList = this.GetSelectedItem();
                return this.EditItem(valueList, out edited);
            }

            return null;
        }

        protected override ValueList CreateNewItemCore(ValueList contents)
        {
            bool edited;
            return this.EditItem(new ValueList(null, String.Empty, false, false), out edited);
        }

        protected override List<ValueList> DeleteSelectedItemsCore(bool silent)
        {
            List<ValueList> selectedItems = this.GetSelectedItems();

            if (selectedItems.Count > 0)
            {
                StringBuilder message = new StringBuilder();
                if (selectedItems.Count == 1)
                {
                    message = message.AppendFormat(CultureInfo.CurrentCulture, Localization.MessageFormats.ConfirmDeleteValueList, selectedItems[0].Name);
                }
                else
                {
                    message.AppendLine(Localization.MessageFormats.ConfirmDeleteValueLists);
                    for (int i = 0; i < selectedItems.Count; i++)
                    {
                        message.Append(String.Format(CultureInfo.CurrentCulture, Localization.MessageFormats.ValueListNameDisplayFormat, selectedItems[i].Name));
                        if (i < selectedItems.Count - 1)
                        {
                            message.Append(", ");
                        }
                    }
                }

                if (silent || UIMessageBox.Show(
                    message.ToString(),
                    Localization.Promptu.AppName,
                    UIMessageBoxButtons.YesNo,
                    UIMessageBoxIcon.Warning,
                    UIMessageBoxResult.Yes) == UIMessageBoxResult.Yes)
                {
                    //try
                    //{
                    InternalGlobals.SyncSynchronizer.CancelSyncsAndWait();
                    //PromptuSettings.SyncSynchronizer.PauseSyncs();
                    int selectedIndex = this.CollectionPresenter.PrimarySelectedIndex;
                    foreach (ValueList valueList in selectedItems)
                    {
                        this.ListUsing.ValueLists.Remove(valueList);
                        this.ListUsing.ValueLists.Save();
                    }

                    this.UpdateItemsListView();
                    if (this.CollectionPresenter.Count > 0)
                    {
                        if (selectedIndex >= this.CollectionPresenter.Count)
                        {
                            selectedIndex = this.CollectionPresenter.Count - 1;
                        }

                        this.CollectionPresenter.SelectedIndexes.Add(selectedIndex);
                        this.CollectionPresenter.EnsureVisible(selectedIndex);
                    }
                    //}
                    //finally
                    //{
                    //    //PromptuSettings.SyncSynchronizer.UnPauseSyncs();
                    //}
                }
            }

            this.UpdateAllCallback();
            return selectedItems;
        }

        private ValueList EditItem(ValueList valueList, out bool edited)
        {
            edited = false;
            ValueListEditorPresenter editor = new ValueListEditorPresenter(valueList);
            InternalGlobals.UISettings.ValueListEditorSettings.ImpartTo(editor.NativeInterface);
            while (true)
            {
                if (editor.ShowDialog() == UIDialogResult.OK)
                {
                    InternalGlobals.UISettings.ValueListEditorSettings.UpdateFrom(editor.NativeInterface);
                    ValueList assembled = editor.AssembleValueList();
                    if (assembled.Name.ToUpperInvariant() != valueList.Name.ToUpperInvariant() &&
                        this.ListUsing.ValueLists.Contains(assembled.Name))
                    {
                        UIMessageBox.Show(
                            Localization.Promptu.ValueListAlreadyExistsMessage,
                            Localization.Promptu.AppName,
                            UIMessageBoxButtons.OK,
                            UIMessageBoxIcon.Error,
                            UIMessageBoxResult.OK);
                        continue;
                    }
                    else
                    {
                        //try
                        //{
                        InternalGlobals.SyncSynchronizer.CancelSyncsAndWait();
                        IHasId castValueList = (IHasId)assembled;

                        if (castValueList.Id == null)
                        {
                            castValueList.Id = this.ListUsing.Functions.IdGenerator.GenerateId();
                        }

                        assembled.Sort();

                        this.ListUsing.ValueLists.Remove(valueList);
                        this.ListUsing.ValueLists.Add(assembled);
                        this.ListUsing.ValueLists.Save();
                        this.UpdateItemsListView();
                        //ListViewItem item = this.ItemsListView.FindItemWithText(assembled.Name);
                        this.ClearSelectedIndices();
                        int indexOfItem = this.CollectionPresenter.IndexOf(assembled);
                        if (indexOfItem > -1)
                        {
                            this.CollectionPresenter.SelectedIndexes.Add(indexOfItem);
                            this.CollectionPresenter.EnsureVisible(indexOfItem);
                        }

                        edited = true;
                        this.UpdateAllCallback();
                        InternalGlobals.UISettings.ValueListEditorSettings.UpdateFrom(editor.NativeInterface);
                        return assembled;
                        //}
                        //finally
                        //{
                        //    PromptuSettings.SyncSynchronizer.UnPauseSyncs();
                        //}
                    }
                }

                break;
            }

            this.UpdateAllCallback();
            InternalGlobals.UISettings.ValueListEditorSettings.UpdateFrom(editor.NativeInterface);

            return null;
        }

        private static string ConvertToYesNo(bool value)
        {
            if (value)
            {
                return "Yes";
            }

            return "No";
        }

        private static string FormatContents(ValueList valueList)
        {
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < valueList.Count; i++)
            {
                builder.AppendFormat("{0}{1}", i > 0 ? " | " : String.Empty, valueList[i].Value);
            }

            return builder.ToString();
        }

        private int ValueListComparison(ValueList item1, ValueList item2)
        {
            return item1.Name.CompareTo(item2.Name);
        }
    }
}
