using System;
using System.Collections.Generic;
using System.Text;

namespace ZachJohnson.Promptu.UIModel.Interfaces
{
    internal interface IErrorPanel
    {
        event EventHandler ItemActivated;

        string Caption { get; set; }

        ICheckBoxButton ErrorsButton { get; }

        ICheckBoxButton WarningsButton { get; }

        ICheckBoxButton MessagesButton { get; }

        int PrimarySelectedIndex { get; }

        bool SomethingIsSelected { get; }

        void SetMessages(FeedbackCollection messages);
    }
}
