//using System;
//using System.Collections.Generic;
//using System.Text;
//using System.Windows.Forms;
//using ZachJohnson.Promptu.SkinModel;

//namespace ZachJohnson.Promptu.SkinsApi
//{
//    internal interface ITextualInput
//    {
//        event EventHandler<KeyPressedEventArgs> KeyPressedByUser;

//        event MouseEventHandler MouseWheel;

//        event MouseEventHandler MouseDown;

//        event MouseEventHandler MouseUp;

//        string InputText { get; set; }

//        int StartOfSelection { get; set; }

//        int LengthOfSelection { get; set; }

//        void SuspendDrawing();

//        void ResumeDrawing();

//        bool Focus();

//        void ShowCaret();
//    }
//}
