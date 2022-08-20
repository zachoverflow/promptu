//-----------------------------------------------------------------------
// <copyright file="AssemblyReferenceSetupPanelPresenter.cs" company="ZachJohnson">
//     Copyright (c) Zach Johnson. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace ZachJohnson.Promptu.UIModel.Presenters
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using ZachJohnson.Promptu.UserModel;
    using ZachJohnson.Promptu.UI;
    using System.IO;
    using System.Globalization;

    internal class AssemblyReferenceSetupPanelPresenter : SetupPanelPresenter<AssemblyReference>
    {
        public AssemblyReferenceSetupPanelPresenter(ParameterlessVoid updateAllCallback, ParameterlessVoid settingChangedCallback)
            : this(
            InternalGlobals.GuiManager.ToolkitHost.Factory.ConstructSetupPanel(settingChangedCallback),
            updateAllCallback)
        {
        }

        public AssemblyReferenceSetupPanelPresenter(Interfaces.ISetupPanel nativeInterface, ParameterlessVoid updateAllCallback)
            : base(nativeInterface, updateAllCallback)
        {
            this.CollectionPresenter.Headers.Add(
                Localization.UIResources.AssemblyReferenceNameColumnHeaderText,
                this.GetAssemblyReferenceName);

            this.CollectionPresenter.Headers.Add(
                Localization.UIResources.AssemblyNameColumnHeaderText,
                this.GetAssemblyReferenceAssemblyName);

            this.CollectionPresenter.Headers.Add(
                Localization.UIResources.AssemblyPathColumnHeaderText,
                this.GetAssemblyReferencePath);

            //this.fileColumnHeader = new System.Windows.Forms.ColumnHeader();
            //this.filepathColumnHeader = new System.Windows.Forms.ColumnHeader();
            //this.referenceNameHeader = new System.Windows.Forms.ColumnHeader();

            //this.ItemsListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            //this.referenceNameHeader,
            //this.fileColumnHeader,
            //this.filepathColumnHeader});

            //this.fileColumnHeader.Text = "Assembly Name";
            //this.fileColumnHeader.Width = 116;

            //this.filepathColumnHeader.Text = "Path";
            //this.filepathColumnHeader.Width = 312;

            //this.referenceNameHeader.Text = "Reference Name";
            //this.referenceNameHeader.Width = 104;

            //this.NewButton.Text = "New Assembly Reference";

            //this.NativeInterface.NewButton.ToolTipText = Localization.UIResources.AssemblyReferenceSetupPanelNewToolTip;
            this.NativeInterface.NewButton.Image = InternalGlobals.GuiManager.ToolkitHost.Images.NewAssemblyReference;
            this.ItemCode = ItemCode.AssemblyReference;
        }

        public void EditAssemblyReference(string name)
        {
            bool edited;
            AssemblyReference reference = this.EditItem(this.ListUsing.AssemblyReferences[name], name, out edited);
            if (edited)
            {
                this.OnItemEdited(new ItemEventArgs<AssemblyReference>(reference));
            }
        }

        private object GetAssemblyReferenceName(object obj)
        {
            AssemblyReference reference = (AssemblyReference)obj;
            return reference.Name;
        }

        private object GetAssemblyReferenceAssemblyName(object obj)
        {
            AssemblyReference reference = (AssemblyReference)obj;
            if (reference.OwnedByUser && !reference.Orphaned)
            {
                return reference.Filename;
            }
            else
            {
                return String.Empty;
            }
        }

        private object GetAssemblyReferencePath(object obj)
        {
            AssemblyReference reference = (AssemblyReference)obj;
            if (reference.OwnedByUser)
            {
                if (reference.Orphaned)
                {
                    return Localization.UIResources.OrphanedAssemblyReference;
                }
                else
                {
                    return reference.Filepath;
                }
            }
            else
            {
                return Localization.UIResources.ExternallySharedReference;
            }
        }

        protected override void UpdateItemsListViewCore()
        {
            //ListViewItem item;
            List<AssemblyReference> sorted = new List<AssemblyReference>();
            using (DdMonitor.Lock(this.ListUsing.AssemblyReferences))
            {
                sorted.AddRange(this.ListUsing.AssemblyReferences);
            }

            sorted.Sort(new Comparison<AssemblyReference>(this.AssemblyReferenceComparison));
            foreach (AssemblyReference reference in sorted)
            {
                //List<string> values = new List<string>(this.CollectionPresenter.Headers.Count);
                //values.Add(reference.Name);
                ////item = new ListViewItem();
                ////item.IndentCount = 0;
                ////item.Text = reference.Name;
                //if (reference.OwnedByUser)
                //{
                //    if (reference.Orphaned)
                //    {
                //        values.Add(String.Empty);
                //        values.Add(Localization.UIResources.OrphanedAssemblyReference);
                //    }
                //    else
                //    {
                //        values.Add(reference.Filename);
                //        values.Add(reference.Filepath);
                //    }
                //}
                //else
                //{
                //    values.Add(String.Empty);
                //    values.Add(Localization.UIResources.ExternallySharedReference);
                //}

                this.CollectionPresenter.Add(reference);
            }
        }

        protected override AssemblyReference CreateNewItemCore(AssemblyReference contents)
        {
            bool edited;
            return this.EditItem(new AssemblyReference(String.Empty, String.Empty, null, null, null, InternalGlobals.CurrentProfile.GetSyncCallback(), false), null, out edited);
        }

        protected override string GetPluralItemDisplayFormat()
        {
            return Localization.UIResources.AssemblyReferenceCountPluralFormat;
        }

        protected override string GetSingularItemDisplayFormat()
        {
            return Localization.UIResources.AssemblyReferenceCountSingularFormat;
        }

        protected override List<AssemblyReference> DeleteSelectedItemsCore(bool silent)
        {
            List<AssemblyReference> selectedItems = this.GetSelectedItems();

            if (selectedItems.Count > 0)
            {
                StringBuilder message = new StringBuilder();
                if (selectedItems.Count == 1)
                {
                    message = message.AppendFormat(CultureInfo.CurrentCulture, Localization.MessageFormats.ConfirmDeleteAssemblyReference, selectedItems[0].Name);
                }
                else
                {
                    message.AppendLine(Localization.MessageFormats.ConfirmDeleteAssemblyReferences);
                    for (int i = 0; i < selectedItems.Count; i++)
                    {
                        message.AppendLine(String.Format(CultureInfo.CurrentCulture, Localization.MessageFormats.AssemblyReferenceNameDisplayFormat, selectedItems[i].Name));
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
                    foreach (AssemblyReference reference in selectedItems)
                    {
                        this.ListUsing.AssemblyReferences.Remove(reference);
                        this.ListUsing.AssemblyReferences.Save();
                        reference.UninstallCachedAssemblyIfNoReferences();
                        this.ListUsing.AssemblyReferencesManifest.Remove(reference.Name);
                        this.ListUsing.AssemblyReferencesManifest.Save();
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

            return selectedItems;
        }

        protected override AssemblyReference EditSelectedItemCore()
        {
            if (this.CollectionPresenter.SelectedIndexes.Count > 0)
            {
                bool edited;
                AssemblyReference reference = this.GetItem(this.CollectionPresenter.PrimarySelectedIndex);
                return this.EditItem(reference, reference.Name, out edited);
            }

            return null;
        }

        protected override AssemblyReference GetItem(int index)
        {
            if (index >= 0 && index < this.CollectionPresenter.Count)
            {
                return (AssemblyReference)this.CollectionPresenter[index];
            }

            return null;
        }

        private int AssemblyReferenceComparison(AssemblyReference reference1, AssemblyReference reference2)
        {
            return reference1.Name.CompareTo(reference2.Name);
        }

        private AssemblyReference EditItem(AssemblyReference reference, string originalName, out bool edited)
        {
            edited = false;

            AssemblyReference assembled = reference;
            while (true)
            {
                AssemblyReferenceEditorPresenter editor = new AssemblyReferenceEditorPresenter(assembled);
                InternalGlobals.UISettings.AssemblyReferenceEditorSettings.ImpartTo(editor.NativeInterface);
                if (editor.ShowDialog() == UIDialogResult.OK)
                {
                    assembled = editor.AssembleReference();
                    InternalGlobals.UISettings.AssemblyReferenceEditorSettings.UpdateFrom(editor.NativeInterface);

                    if (this.ListUsing.AssemblyReferences.Contains(assembled.Name) && assembled.Name != originalName)
                    {
                        UIMessageBox.Show(
                            Localization.Promptu.AssemblyReferenceAlreadyExistsMessage,
                            Localization.Promptu.AppName,
                            UIMessageBoxButtons.OK,
                            UIMessageBoxIcon.Error,
                            UIMessageBoxResult.OK);
                        continue;
                    }
                    else
                    {
                        if (assembled.OwnedByUser && !assembled.Orphaned && !assembled.Filepath.Exists)
                        {
                            ConfirmDialogPresenter confirmDialog = new ConfirmDialogPresenter(
                                UIMessageBoxIcon.Warning,
                                Localization.Promptu.AppName,
                                Localization.UIResources.AssemblyReferenceFileNotFound,
                                Localization.UIResources.AssemblyReferenceFileNotFoundSupplement,
                                Localization.UIResources.ContinueAnywayButtonText,
                                Localization.UIResources.CancelButtonText);

                            if (confirmDialog.ShowDialog() == UIDialogResult.Cancel)
                            {
                                continue;
                            }

                            //UIMessageBox.Show(
                            //    String.Format(Localization.MessageFormats.PathNotFoundError, assembled.Filepath),
                            //    Localization.Promptu.AppName,
                            //    UIMessageBoxButtons.OK,
                            //    UIMessageBoxIcon.Error,
                            //    UIMessageBoxResult.OK);
                        }

                        //if (assembled.Name != originalName && originalName != null)
                        //{
                        //    this.ListUsing.SyncInfo.AssemblyReferenceIdentifierChanges.RemoveAllWithRevisedItem(reference);
                        //    this.ListUsing.SyncInfo.AssemblyReferenceIdentifierChanges.Add(new ZachJohnson.Promptu.UserModel.Differencing.AssemblyReferenceIdentifierChange(assembled, originalName));
                        //}
                        //try
                        //{
                        InternalGlobals.SyncSynchronizer.CancelSyncsAndWait();
                        //PromptuSettings.SyncSynchronizer.PauseSyncs();

                        if (assembled.Id == null)
                        {
                            assembled.Id = this.ListUsing.AssemblyReferences.IdGenerator.GenerateId();
                        }

                        bool notTheSameFile = assembled.Filepath != reference.Filepath;

                        this.ListUsing.AssemblyReferences.Remove(originalName);
                        this.ListUsing.AssemblyReferences.Add(assembled);

                        if ((!reference.OwnedByUser && notTheSameFile) || !this.ListUsing.AssemblyReferencesManifest.Contains(assembled.Name))
                        {
                            if (this.ListUsing.AssemblyReferencesManifest.Contains(reference.Name))
                            {
                                this.ListUsing.AssemblyReferencesManifest.Remove(reference.Name);
                            }

                            this.ListUsing.AssemblyReferencesManifest.Add(assembled.Name);
                            assembled.OwnedByUser = true;
                        }

                        if (assembled.Name != reference.Name && !String.IsNullOrEmpty(reference.Name))
                        {
                            if (UIMessageBox.Show(
                                String.Format(CultureInfo.CurrentCulture, Localization.MessageFormats.AssemblyReferenceNameChangeUpdateReferences, reference.Name),
                                Localization.Promptu.AppName,
                                UIMessageBoxButtons.YesNo,
                                UIMessageBoxIcon.Information,
                                UIMessageBoxResult.Yes) == UIMessageBoxResult.Yes)
                            {
                                using (DdMonitor.Lock(this.ListUsing.Functions))
                                {
                                    foreach (Function function in this.ListUsing.Functions)
                                    {
                                        if (function.AssemblyReferenceName == reference.Name)
                                        {
                                            function.AssemblyReferenceName = assembled.Name;
                                        }
                                    }
                                }

                                this.ListUsing.PublishOnChildrenSaved = false;
                                this.ListUsing.Functions.Save();
                                this.ListUsing.PublishOnChildrenSaved = true;

                                this.UpdateAllCallback();
                            }
                        }

                        if (assembled.CachedName == null)
                        {
                            try
                            {
                                this.ListUsing.PublishOnChildrenSaved = false;
                                assembled.InstallAssemblyInCache();
                            }
                            catch (FileNotFoundException)
                            {
                            }
                            catch (IOException ex)
                            {
                                UIMessageBox.Show(
                                    String.Format(CultureInfo.CurrentCulture, Localization.MessageFormats.AssemblyReferenceIOException, assembled.Filepath, ex.Message),
                                    Localization.Promptu.AppName,
                                    UIMessageBoxButtons.OK,
                                    UIMessageBoxIcon.Error,
                                    UIMessageBoxResult.OK);
                            }
                            finally
                            {
                                this.ListUsing.PublishOnChildrenSaved = true;
                            }
                        }

                        this.ListUsing.AssemblyReferences.Save();

                        reference.UninstallCachedAssemblyIfNoReferences();
                        //this.DeleteCachedAssemblyIfNoReferences(reference.CachedName);

                        this.UpdateItemsListView();
                        //ListViewItem item = this.ItemsListView.FindItemWithText(assembled.Name);
                        this.ClearSelectedIndices();
                        int indexOfItem = this.CollectionPresenter.IndexOf(assembled);
                        if (indexOfItem > -1)
                        {
                            this.CollectionPresenter.SelectedIndexes.Add(indexOfItem);
                            this.CollectionPresenter.EnsureVisible(indexOfItem);
                        }
                        //if (item != null)
                        //{
                        //    item.Selected = true;
                        //    this.ItemsListView.EnsureVisible(item.Index);
                        //}

                        edited = true;
                        InternalGlobals.UISettings.AssemblyReferenceEditorSettings.UpdateFrom(editor.NativeInterface);
                        return assembled;
                        //}
                        //finally
                        //{
                        //    PromptuSettings.SyncSynchronizer.UnPauseSyncs();
                        //}
                    }
                }

                InternalGlobals.UISettings.AssemblyReferenceEditorSettings.UpdateFrom(editor.NativeInterface);
                break;
            }

            return null;
        }
    }
}
