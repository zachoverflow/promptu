using System;
using System.Collections.Generic;
using System.Text;
using ZachJohnson.Promptu.UIModel.Interfaces;
using ZachJohnson.Promptu.UserModel.Differencing;
using System.ComponentModel;

namespace ZachJohnson.Promptu.UIModel.Presenters
{
    internal class CollisionResolvingDialogPresenter : DialogPresenterBase<ICollisionResolvingDialog>
    {
        // TODO test more than one conflict scenario
        private int index;
        private List<DiffDiffBase> conflicts;
        //bool promptuUserOnClose = true;

        public CollisionResolvingDialogPresenter(
            List<DiffDiffBase> conflicts)
            : this(
            InternalGlobals.GuiManager.ToolkitHost.Factory.ConstructCollisionResolvingDialog(),
            conflicts)
        {
        }

        public CollisionResolvingDialogPresenter(
            ICollisionResolvingDialog nativeInterface,
            List<DiffDiffBase> conflicts)
            : base(nativeInterface)
        {
            if (conflicts == null)
            {
                throw new ArgumentNullException("conflicts");
            }

            this.conflicts = conflicts;
            this.NativeInterface.ResolveToUsersClick += this.ResolveConflictToPriority;
            this.NativeInterface.ResolveToExternalClick += this.ResolveConflictToSecondary;

            this.NativeInterface.Text = Localization.UIResources.CollisionResolvingDialogText;
            this.NativeInterface.MainInstructions = Localization.UIResources.CollisionResolvingDialogMainInstructions;

            //this.NativeInterface.ConflictingChangeAlsoDeletesLabelText = Localization.UIResources.ConflictingChangeAlsoDeletesNotificationText;
            //this.NativeInterface.YourChangeAlsoDeletesLabelText = Localization.UIResources.YourChangeAlsoDeletesNotificationText;

            this.NativeInterface.CancelButton.Text = Localization.UIResources.CancelButtonText;

            this.NativeInterface.ExternalChangeLabel = Localization.UIResources.ConflictKeepOutsideChangeLabel;
            this.NativeInterface.ExternalChangeSupplement = Localization.UIResources.ConflictKeepOutsideChangeSupplement;
            this.NativeInterface.UsersChangeLabel = Localization.UIResources.ConflictKeepMyChangeLabel;
            this.NativeInterface.UsersChangeSupplement = Localization.UIResources.ConflictKeepMyChangeSupplement;

            //this.NativeInterface.YourChangeLabelText = Localization.UIResources.YourChangeLabelText;
            //this.NativeInterface.ConflictingChangeLabelText = Localization.UIResources.ConflictingChangeLabelText;

            this.NativeInterface.Closing += this.HandleClosing;
            
            if (conflicts.Count <= 0)
            {
                //this.CloseAndSetDialogResult(UIDialogResult.OK);
                this.NativeInterface.CloseWithOk();
            }

            this.RefreshConflictDisplay();
        }

        private void CloseWithOk()
        {
            //this.promptuUserOnClose = false;
            this.NativeInterface.CloseWithOk();
        }

        public UIDialogResult ShowDialog(object owner)
        {
            return this.NativeInterface.ShowModal(owner);
        }

        private void HandleClosing(object sender, CancelEventArgs e)
        {
        //    if (this.promptuUserOnClose)
        //    {
        //        if (UIMessageBox.Show(
        //            Localization.UIResources.ConfirmExitCollisionResolvingDialog,
        //            Localization.Promptu.AppName,
        //            UIMessageBoxButtons.YesNo,
        //            UIMessageBoxIcon.Information,
        //            UIMessageBoxResult.No) == UIMessageBoxResult.Yes)
        //        {
                    this.NativeInterface.DoForTheNextNConflicts.Checked = true;
                    this.ResolveConflict(DiffVersion.Priority);
            //    }
            //    else
            //    {
            //        e.Cancel = true;
            //    }
            //}
        }

        private void RefreshConflictDisplay()
        {
            if (this.index < this.conflicts.Count)
            {
                // I18N needed
                DiffDiffBase diffDiff = this.conflicts[this.index];
                //this.NativeInterface.SubText = String.Format(Localization.UIResources.CollisionResolvingDialogSubTextFormat, diffDiff.ObjectTypeName);
                int numberOfConflictsLeft = this.conflicts.Count - this.index - 1;
                if (numberOfConflictsLeft < 1)
                {
                    //this.doForTheNextNConflicts.Visible = false;
                    //this.doForTheNextNConflicts.Checked = false;
                    this.NativeInterface.DoForTheNextNConflicts.Visible = false;
                    this.NativeInterface.DoForTheNextNConflicts.Checked = false;
                }
                //else if (numberOfConflictsLeft == 1)
                //{
                //    this.NativeInterface.DoForTheNextNConflicts.Text = Localization.UIResources.DoThisForTheNextConflictFormat;
                //}
                else
                {
                    this.NativeInterface.DoForTheNextNConflicts.Text = Localization.UIResources.DoThisForAllRemainingConflicts;//String.Format(Localization.UIResources.DoThisForTheNextNConflictsFormat, numberOfConflictsLeft);
                }

                ObjectConflictInfo priorityInfo = diffDiff.GetInfoForPriority();
                ObjectConflictInfo secondaryInfo = diffDiff.GetInfoForSecondary();

                //if (diffDiff.PriorityDiffIdentifierConflictsCount > 0)
                //{
                //    //this.NativeInterface.ShowYourChangeAlsoDeletesArea = true;

                //    //this.NativeInterface.SetPriorityAlsoDeletesInfo(diffDiff.GetInfoForPriorityIdentifierConflicts());
                //}
                //else
                //{
                //    //this.NativeInterface.ShowYourChangeAlsoDeletesArea = false;
                //}

                //if (diffDiff.SecondaryDiffIdentifierConflictsCount > 0)
                //{
                //    //this.NativeInterface.ShowConflictingChangeAlsoDeletesArea = true;

                //    //this.NativeInterface.SetSecondaryAlsoDeletesInfo(diffDiff.GetInfoForSecondaryIdentifierConflicts());
                    
                //}
                //else
                //{
                //    //this.NativeInterface.ShowConflictingChangeAlsoDeletesArea = false;
                //}

                this.NativeInterface.SetUsersChangeInfo(priorityInfo);
                this.NativeInterface.SetExternalChangeInfo(secondaryInfo);

                //this.NativeInterface.NotifyUpdateFinished();
            }
        }

        private void ResolveConflictToPriority(object sender, EventArgs e)
        {
            this.ResolveConflict(DiffVersion.Priority);
        }

        private void ResolveConflictToSecondary(object sender, EventArgs e)
        {
            this.ResolveConflict(DiffVersion.Secondary);
        }

        private void ResolveConflict(DiffVersion version)
        {
            if (this.NativeInterface.DoForTheNextNConflicts.Checked)
            {
                for (int i = this.index; i < this.conflicts.Count; i++)
                {
                    this.conflicts[i].ResolveTo(version);
                }

                this.CloseWithOk();
            }
            else
            {
                this.conflicts[this.index].ResolveTo(version);

                List<int> conflictIndexesToRemove = new List<int>();
                for (int i = this.index + 1; i < this.conflicts.Count; i++)
                {
                    if (this.conflicts[i].ImplicityResolved)
                    {
                        conflictIndexesToRemove.Add(i);
                    }
                }

                foreach (int index in conflictIndexesToRemove)
                {
                    this.conflicts.RemoveAt(index);
                }

                this.NextConflict();
            }
        }

        private void NextConflict()
        {
            if (++this.index < this.conflicts.Count)
            {
                this.RefreshConflictDisplay();
            }
            else
            {
                this.CloseWithOk();
            }
        }
    }
}
