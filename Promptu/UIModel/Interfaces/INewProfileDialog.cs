//using System;
//using System.Collections.Generic;
//using System.Text;
//using System.ComponentModel;

//namespace ZachJohnson.Promptu.UIModel.Interfaces
//{
//    interface INewProfileDialog : IDialog
//    {
//        event CancelEventHandler ClosingWithCancel;

//        event ParameterlessVoid NextClicked;

//        event ParameterlessVoid BackClicked;

//        string BackButtonText { get; set; }

//        string NextButtonText { get; set; }

//        bool BackButtonEnabled { get; set; }

//        bool MoveForwardAndBackOnInit { get; }

//        string NameInstructions { get; set; }

//        string FurtherInstructions { get; set; }

//        string ProfileName { get; set; }

//        IHotkeyControl HotkeyControl { get; }

//        void MoveToNextPanel(bool willGoToStep3);

//        void MoveToPreviousPanel();

//        void CloseWithOk();
//    }
//}
