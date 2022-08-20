//using System;
//using System.Collections.Generic;
//using System.Text;
//using System.Windows.Forms;
//using System.Drawing;

//namespace ZachJohnson.Promptu.SkinsApi
//{
//    internal interface IPrompt : IWin32Window
//    {
//        event EventHandler PromptFocusLost;

//        ITextualInput TextualInput { get; }

//        //event EventHandler<MessageEventArgs> WM_ActivateRecieved;
//        ////IntPtr Handle { get; }

//        Point LocationOnScreen { get; set; }

//        event EventHandler LocationOnScreenChanged;

//        event MouseEventHandler MouseDownOnPrompt;

//        //TODO remove as soon as api is designed
//        void UpdateNewCommandItems();

//        Size PromptSize { get; }

//        int PromptHeight { get; }

//        bool ContainsFocus { get; }

//        void OpenPrompt(Point location);

//        void ClosePrompt();

//        void EnsureHandleCreated();

//        void Activate();

//        void BringToFront();

//        //void Show(IWin32Window owner);

//        ISuggester Suggester { get; }
//    }
//}
