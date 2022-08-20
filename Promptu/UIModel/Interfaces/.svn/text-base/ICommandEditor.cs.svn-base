using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace ZachJohnson.Promptu.UIModel.Interfaces
{
    interface ICommandEditor : IDialog, IThreadingInvoke
    {
        event CancelEventHandler Closing;

        ITextInput Name { get; }

        ITextInput Target { get; }

        ITextInput Arguments { get; }

        ITextInput WorkingDirectory { get; }

        string MainInstructions { set; }

        string NameLabelText { set; }

        string ExecutesLabelText { set; }

        string ArgumentsLabelText { set; }

        string StartupStateLabelText { set; }

        ICheckBox RunAsAdmin { get; }

        IComboInput StartupState { get; }

        string WorkingDirectoryLabelText { set; }

        string NotesLabelText { set; }

        ITextInput Notes { get; }

        ICheckBox ShowParamHistory { get; }

        IButton TestButton { get; }

        ICheckBox GuessWorkingDirectory { get; }

        IButton ViewAvailableFunctionsButton { get; }

        IButton OkButton { get; }

        IButton CancelButton { get; }

        ICollectionEditor CommandParameterMetaInfoPanel { get; }

        string EyedropperToolTip { set; }

        IErrorPanel ErrorPanel { get; }

        void CloseWithOK();

        bool IsCreatedAndNotDisposing { get; }

        void EnsureErrorListVisible();

        bool MustShowErrorList { set; }

        void SetUIForParameters(bool thereAreParameters);

        ParameterlessVoid RestartPromptu { set; }
    }
}
