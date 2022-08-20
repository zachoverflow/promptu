using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using ZachJohnson.Promptu.UserModel.Differencing;

namespace ZachJohnson.Promptu.UIModel.Interfaces
{
    internal interface ICollisionResolvingDialog : IDialog
    {
        event EventHandler ResolveToUsersClick;

        event EventHandler ResolveToExternalClick;

        event CancelEventHandler Closing;

        string MainInstructions { set; }

        //string YourChangeAlsoDeletesLabelText { set; }

        //string ConflictingChangeAlsoDeletesLabelText { set; }

        //string YourChangeLabelText { set; }

        //string ConflictingChangeLabelText { set; }

        //string SubText { set; }

        void CloseWithOk();

        ICheckBox DoForTheNextNConflicts { get; }

        string UsersChangeLabel { set; }

        string UsersChangeSupplement { set; }

        string ExternalChangeLabel { set; }

        string ExternalChangeSupplement { set; }

        void SetUsersChangeInfo(ObjectConflictInfo info);

        void SetExternalChangeInfo(ObjectConflictInfo info);

        //void SetPriorityAlsoDeletesInfo(VisualDisplayInfo info);

        //void SetSecondaryAlsoDeletesInfo(VisualDisplayInfo info);

        //void NotifyUpdateFinished();

        IButton CancelButton { get; }

        UIDialogResult ShowModal(object owner);
    }
}
