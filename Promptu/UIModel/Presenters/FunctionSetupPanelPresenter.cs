//-----------------------------------------------------------------------
// <copyright file="FunctionSetupPanelPresenter.cs" company="ZachJohnson">
//     Copyright (c) Zach Johnson. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace ZachJohnson.Promptu.UIModel.Presenters
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using ZachJohnson.Promptu.UserModel;
    using ZachJohnson.Promptu.UIModel.Interfaces;
    using ZachJohnson.Promptu.UI;
    using ZachJohnson.Promptu.UserModel.Collections;
    using System.Globalization;

    internal class FunctionSetupPanelPresenter : SetupPanelPresenter<Function>
    {
        private AssemblyReferenceCollectionComposite prioritizedReferences;

        public FunctionSetupPanelPresenter(ParameterlessVoid updateAllCallback, ParameterlessVoid settingChangedCallback)
            : this(
            InternalGlobals.GuiManager.ToolkitHost.Factory.ConstructSetupPanel(settingChangedCallback),
            updateAllCallback)
        {
        }

        public FunctionSetupPanelPresenter(ISetupPanel uiObject, ParameterlessVoid updateAllCallback)
            : base(uiObject, updateAllCallback)
        {
            this.prioritizedReferences = new AssemblyReferenceCollectionComposite(InternalGlobals.CurrentProfile.Lists, this.ListUsing);

            this.CollectionPresenter.Headers.Add(
                Localization.UIResources.FunctionNameHeaderText,
                this.GetFunctionName);

            this.CollectionPresenter.Headers.Add
                (Localization.UIResources.FunctionMethodHeaderText,
                this.GetFunctionMethod);

            this.CollectionPresenter.Headers.Add(
                Localization.UIResources.FunctionClassHeaderText,
                this.GetFunctionClass);

            this.CollectionPresenter.Headers.Add(
                Localization.UIResources.FunctionAssemblyHeaderText,
                this.GetFunctionAssemblyReference);

            //TODO put widths in settings for all setup panels
            //this.nameHeader.Text = "Function";
            //this.nameHeader.Width = 116;

            //this.assemblyHeader.Text = "Assembly";
            //this.assemblyHeader.Width = 104;

            //this.methodHeader.Text = "Method";
            //this.methodHeader.Width = 104;

            //this.classHeader.Text = "Class";
            //this.classHeader.Width = 104;

            //this.NativeInterface.NewButton.ToolTipText = Localization.UIResources.FunctionSetupPanelNewToolTip;
            this.NativeInterface.NewButton.Image = InternalGlobals.GuiManager.ToolkitHost.Images.NewFunction;

            this.ItemCode = ItemCode.Function;
        }

        private object GetFunctionName(object obj)
        {
            Function function = (Function)obj;
            object retVal = null;
            //try
            //{
            retVal = function.GetSignature();//function.GetNamedSignatureIfPossible(this.prioritizedReferences);
            //}
            //catch (Exception ex)
            //{
            //}
            //try
            //{
            return retVal;
            //}
            //catch (Exception e)
            //{
            //    return "I'm the problem";
            //}
        }

        private object GetFunctionMethod(object obj)
        {
            Function function = (Function)obj;
            return function.MethodName;
        }

        private object GetFunctionClass(object obj)
        {
            Function function = (Function)obj;
            return function.InvocationClass;
        }

        private object GetFunctionAssemblyReference(object obj)
        {
            Function function = (Function)obj;
            return function.AssemblyReferenceName;
        }

        protected override void UpdateItemsListViewCore()
        {
            //ListViewItem listViewItem;
            List<Function> sorted = new List<Function>();
            using (DdMonitor.Lock(this))
            {
                sorted.AddRange(this.ListUsing.Functions);
            }

            sorted.Sort(new Comparison<Function>(this.FunctionComparison));
            this.prioritizedReferences = new AssemblyReferenceCollectionComposite(InternalGlobals.CurrentProfile.Lists, this.ListUsing);
            foreach (Function item in sorted)
            {
                //List<string> values = new List<string>(this.CollectionPresenter.Headers.Count);
                //values.Add(item.GetNamedSignatureIfPossible(prioritizedReferences));
                //values.Add(item.MethodName);
                //values.Add(item.InvocationClass);
                //values.Add(item.AssemblyReferenceName);
                
                this.CollectionPresenter.Add(item);
            }
        }

        protected override Function GetItem(int index)
        {
            if (index >= 0 && index < this.CollectionPresenter.Count)
            {
                return (Function)this.CollectionPresenter[index];
            }

            return null;
        }

        protected override Function CreateNewItemCore(Function contents)
        {
            bool edited;
            return this.EditItem(new Function(String.Empty, String.Empty, String.Empty, String.Empty, ReturnValue.String, new FunctionParameterCollection(), null, true), out edited);
        }

        protected override List<Function> DeleteSelectedItemsCore(bool silent)
        {
            List<Function> selectedItems = this.GetSelectedItems();

            if (selectedItems.Count > 0)
            {
                StringBuilder message = new StringBuilder();
                if (selectedItems.Count == 1)
                {
                    message = message.AppendFormat(CultureInfo.CurrentCulture, Localization.MessageFormats.ConfirmDeleteFunction, selectedItems[0].Name);
                }
                else
                {
                    message.AppendLine(Localization.MessageFormats.ConfirmDeleteFunctions);
                    for (int i = 0; i < selectedItems.Count; i++)
                    {
                        message.AppendLine(String.Format(CultureInfo.CurrentCulture, Localization.MessageFormats.FunctionNameDisplayFormat, selectedItems[i].Name));
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
                    //List<int> selectedIndices = this.GetSelectedIndices();
                    int selectedIndex = this.CollectionPresenter.PrimarySelectedIndex;
                    foreach (Function function in selectedItems)
                    {
                        this.ListUsing.Functions.Remove(function);
                        function.RemoveEntriesFromHistory(InternalGlobals.CurrentProfile.History);
                    }

                    this.ListUsing.Functions.Save();
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
                    //    PromptuSettings.SyncSynchronizer.UnPauseSyncs();
                    //}
                }
            }

            return selectedItems;
        }

        protected override string GetPluralItemDisplayFormat()
        {
            return Localization.UIResources.FunctionCountPluralFormat;
        }

        protected override string GetSingularItemDisplayFormat()
        {
            return Localization.UIResources.FunctionCountSingularFormat;
        }

        protected override Function EditSelectedItemCore()
        {
            if (this.CollectionPresenter.SelectedIndexes.Count > 0)
            {
                bool edited;
                Function function = this.GetSelectedItem();
                return this.EditItem(function, out edited);
            }

            return null;
        }

        public void EditFunction(string name, string parameterSignature)
        {
            bool edited;
            Function function = this.EditItem(this.ListUsing.Functions[name, parameterSignature], out edited);
            if (edited)
            {
                this.OnItemEdited(new ItemEventArgs<Function>(function));
            }
        }

        private Function EditItem(Function function, out bool edited)
        {
            edited = false;
            FunctionEditorPresenter editor = new FunctionEditorPresenter(
                function, 
                this.ListUsing, 
                this.UpdateAllCallback);

            InternalGlobals.UISettings.FunctionEditorSettings.ImpartTo(editor.NativeInterface);
            while (true)
            {
                string oldParameterSignature = function.ParameterSignature;
                if (editor.ShowDialog() == UIDialogResult.OK)
                {
                    InternalGlobals.UISettings.FunctionEditorSettings.UpdateFrom(editor.NativeInterface);
                    Function assembled = editor.AssembleFunction();
                    if ((assembled.Name.ToUpperInvariant() != function.Name.ToUpperInvariant()
                        || assembled.ParameterSignature != oldParameterSignature) &&
                        this.ListUsing.Functions.Contains(assembled.Name, assembled.ParameterSignature))
                    {
                        UIMessageBox.Show(
                            Localization.Promptu.FunctionAlreadyExistsMessage,
                            Localization.Promptu.AppName,
                            UIMessageBoxButtons.OK,
                            UIMessageBoxIcon.Error,
                            UIMessageBoxResult.OK);
                        continue;
                    }
                    else if (Command.ReservedNames.Contains(assembled.Name.ToUpperInvariant()))
                    {
                        UIMessageBox.Show(
                            String.Format(CultureInfo.CurrentCulture, Localization.MessageFormats.NameIsReserved, assembled.Name),
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
                        //PromptuSettings.SyncSynchronizer.PauseSyncs();
                        if ((function.Name != assembled.Name || function.ParameterSignature != assembled.ParameterSignature
                            || (function.ReturnValue == ReturnValue.String && assembled.ReturnValue != ReturnValue.String)))
                        {
                            function.RemoveEntriesFromHistory(InternalGlobals.CurrentProfile.History);
                            //this.ListUsing.SyncInfo.FunctionIdentifierChanges.RemoveAllWithRevisedItem(function);
                            //this.ListUsing.SyncInfo.FunctionIdentifierChanges.Add(new ZachJohnson.Promptu.UserModel.Differencing.FunctionIdentifierChange(assembled, originalName, function.NumberOfParameters));
                        }

                        if (assembled.Id == null)
                        {
                            assembled.Id = this.ListUsing.Functions.IdGenerator.GenerateId();
                        }

                        this.ListUsing.Functions.Remove(function.Name, oldParameterSignature);
                        this.ListUsing.Functions.Add(assembled);
                        this.ListUsing.Functions.Save();
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
                        InternalGlobals.UISettings.FunctionEditorSettings.UpdateFrom(editor.NativeInterface);
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

            InternalGlobals.UISettings.FunctionEditorSettings.UpdateFrom(editor.NativeInterface);

            return null;
        }

        private int FunctionComparison(Function item1, Function item2)
        {
            return item1.Name.CompareTo(item2.Name);
        }
    }
}
