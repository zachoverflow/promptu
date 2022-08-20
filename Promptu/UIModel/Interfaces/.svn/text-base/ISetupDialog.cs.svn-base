using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace ZachJohnson.Promptu.UIModel.Interfaces
{
    internal interface ISetupDialog : IBatchUpdatable, IThreadingInvoke
    {
        string Text { get; set; }

        ITabControl MainTabs { get; }

        void Refresh();

        void ShowThreadSafe();

        void UnMinimizeIfNecessary();

        void ActivateThreadSafe();

        void ShowOrBringToFront();

        void Hide();

        event CancelEventHandler Closing;

        event EventHandler Closed;

        bool IsShown { get; }

        bool AllowSaveSettings { get; }

        void SetCursorToWait();

        void SetCursorToDefault();

        ParameterlessVoid SettingChangedCallback { set; }
    }
}
