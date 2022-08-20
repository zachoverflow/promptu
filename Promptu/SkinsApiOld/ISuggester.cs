//using System;
//using System.Collections.Generic;
//using System.Text;
//using System.Drawing;
//using ZachJohnson.Promptu.SkinsApi;
//using System.Windows.Forms;

//namespace ZachJohnson.Promptu.SkinsApi
//{
//    internal interface ISuggester : ISelfSizingAndPositioning, IWin32Window
//    {
//        //Point Location { get; set; }

//        //Size Size { get; set; }

//        //Size SaveSize { get; }

//        //int SelectedIndex { get; set; }

//        Rectangle GetItemRectangle(int index);

//        //void BringToFront();

//        //event EventHandler SelectedIndexChanged;

//        //int ItemCount { get; }

//        //bool SuppressItemInfoToolTips { get; }

//        //event EventHandler UserInteractionFinished;

//        //event EventHandler VisibleChanged;

//        //event MouseEventHandler MouseDoubleClick;

//        event EventHandler<PerformOperationEventArgs> PerformOperation;

//        //bool Visible { get; }

//        //bool ContainsFocus { get; }

//        //void AddItem(SuggestionItem item);

//        //void CenterSelectedItem();

//        //void DoPageUp();

//        //void DoPageDown();

//        //void ScrollToTop();

//        //void ScrollToEnd();

//        //SuggestionItem GetItem(int index);

//        SuggestionItem SelectedItem { get; }

//        ///void ClearItems();

//        //void Hide();

//        //void Show();

//        void Activate();

//        //void RefreshThreadSafe();

//        //void EnsureHandleCreated();

//        //void ScrollSuggestionsThreeLines(Direction direction);
//    }
//}
