using System;
using System.Collections.Generic;
using System.Text;
using ZachJohnson.Promptu.UIModel.Interfaces;
using ZachJohnson.Promptu.UserModel;

namespace ZachJohnson.Promptu.UIModel.Presenters
{
    internal class AssemblyReferenceEditorPresenter : DialogPresenterBase<IAssemblyReferenceEditor>
    {
        private IOpenFileDialog pathBrowser;
        private AssemblyReference original;
        private bool initialized = false;


        public AssemblyReferenceEditorPresenter(AssemblyReference reference)
            : this(
            InternalGlobals.GuiManager.ToolkitHost.Factory.ConstructAssemblyReferenceEditor(),
            reference)
        {
        }

        public AssemblyReferenceEditorPresenter(IAssemblyReferenceEditor nativeInterface, AssemblyReference reference)
            : base(nativeInterface)
        {
            this.NativeInterface.OkButton.Text = Localization.UIResources.OkButtonText;
            this.NativeInterface.CancelButton.Text = Localization.UIResources.CancelButtonText;
            this.NativeInterface.Text = Localization.UIResources.AssemblyReferenceEditorText;
            this.NativeInterface.BrowseButton.Text = Localization.UIResources.BrowseButtonText;
            this.NativeInterface.NameLabelText = Localization.UIResources.AssemblyReferenceNameLabelText;
            this.NativeInterface.PathLabelText = Localization.UIResources.AssemblyReferencePathLabelText;
            this.NativeInterface.MainInstructions = Localization.UIResources.AssemblyReferenceEditorMainInstructions;

            this.pathBrowser = InternalGlobals.GuiManager.ToolkitHost.Factory.ConstructOpenFileDialog();
            this.pathBrowser.CheckFileExists = true;
            this.pathBrowser.CheckPathExists = true;
            this.pathBrowser.Filter = Localization.UIResources.AssemblyReferencePathBrowserFilter;
            this.pathBrowser.Multiselect = false;
            this.pathBrowser.Text = Localization.UIResources.AssemblyReferencePathBrowserTitle;

            this.NativeInterface.Name.TextChanged += this.ValidateOkButtonShouldBeEnabled;
            this.NativeInterface.Path.TextChanged += this.ValidateOkButtonShouldBeEnabled;
            this.NativeInterface.BrowseButton.Click += this.HandleBrowseButtonClick;

            this.NativeInterface.Name.Text = reference.Name;
            if (reference.OwnedByUser)
            {
                if (reference.Orphaned)
                {
                    this.NativeInterface.Path.Cue = Localization.UIResources.OrphanedAssemblyReference;
                }
                else
                {
                    this.NativeInterface.Path.Text = reference.Filepath;
                }
            }
            else
            {
                this.NativeInterface.Path.Cue = Localization.UIResources.ExternallySharedReference;
            }

            this.original = reference;

            this.initialized = true;
            this.ValidateOkButtonShouldBeEnabled();
        }

        public AssemblyReference AssembleReference()
        {
            string cachedName;
            DateTime? lastUpdatedTime;
            FileSystemFile filepath = this.original.Filepath;
            bool orphaned = false;

            bool keepOriginalValues = false;
            if (!this.original.OwnedByUser)
            {
                if (this.NativeInterface.Path.CueDisplayed || string.IsNullOrEmpty(this.NativeInterface.Path.Text))
                {
                    keepOriginalValues = true;
                }
                else
                {
                    filepath = this.NativeInterface.Path.Text;
                }
            }
            else if (!this.NativeInterface.Path.CueDisplayed && this.NativeInterface.Path.Text.ToUpperInvariant() != this.original.Filepath.Path.ToUpperInvariant())
            {
                filepath = this.NativeInterface.Path.Text;
            }
            else
            {
                keepOriginalValues = true;
            }

            if (!keepOriginalValues)
            {
                cachedName = null;
                lastUpdatedTime = null;
            }
            else
            {
                cachedName = this.original.CachedName;
                lastUpdatedTime = this.original.LastUpdatedTimestamp;
                orphaned = this.original.Orphaned;
            }

            AssemblyReference reference = new AssemblyReference(
                this.NativeInterface.Name.Text,
                filepath,
                cachedName,
                lastUpdatedTime,
                this.original.Id,
                this.original.SyncCallback,
                orphaned);
            reference.OwnedByUser = original.OwnedByUser;
            return reference;
        }

        private void ValidateOkButtonShouldBeEnabled(object sender, EventArgs e)
        {
            this.ValidateOkButtonShouldBeEnabled();
        }

        private void ValidateOkButtonShouldBeEnabled()
        {
            if (!this.initialized)
            {
                return;
            }

            this.NativeInterface.OkButton.Enabled = (this.NativeInterface.Name.Text.Length > 0 && this.NativeInterface.Path.Text.Length > 0
                && (this.NativeInterface.Name.Text != this.original.Name || !this.NativeInterface.Path.CueDisplayed));
        }

        private void HandleBrowseButtonClick(object sender, EventArgs e)
        {
            if (this.pathBrowser.ShowModal() == UIDialogResult.OK)
            {
                this.NativeInterface.Path.Text = this.pathBrowser.Path;
            }
        }
    }
}
