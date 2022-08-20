using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Collections;
using ZachJohnson.Promptu.UserModel;
using System.Windows.Forms;

namespace ZachJohnson.Promptu.UIModel.Interfaces
{
    internal interface IListSelector : IBatchUpdatable, IThreadingInvoke
    {
        IList<List> Lists { get; }

        void RefreshItems();

        event KeyEventHandler ListsKeyDown;

        event EventHandler SelectedIndexChanged;

        event EventHandler<ListDragDropEventArgs> ListDragEnter;

        event EventHandler<ListDragDropEventArgs> ListDrop;

        int SelectedIndex { get; set; }

        //bool ListDisplayVisible { get; set; }

        //bool ShowListPrecedenceHint { get; set; }

        bool ShowNoListsMessage { set; }

        string NoListsMessageText { set; }

        //string ListInfoName { get; set; }

        //string ListInfoDescription { get; set; }

        INativeContextMenu ListContextMenu { set; }

        ISplitToolbarButton NewListButton { get; }

        IToolbarButton DeleteButton { get; }

        IToolbarButton MoveListUpButton { get; }

        IToolbarButton MoveListDownButton { get; }

        IToolbarButton RenameButton { get; }

        IToolbarButton SyncButton { get; }

        IToolbarButton UnsubscribeButton { get; }

        IToolbarButton EnableButton { get; }

        IToolbarButton DisableButton { get; }

        IToolbarMenuItem PublishMenuItem { get; }

        IToolbarMenuItem ImportMenuItem { get; }

        IToolbarMenuItem LinkMenuItem { get; }

        IToolbarMenuItem SlickRunImportMenuItem { get; }

        IToolbarMenuItem ExportMenuItem { get; }

        IToolbarMenuItem NewEmptyListMenuItem { get; }

        IToolbarMenuItem NewDefaultListMenuItem { get; }

        IMenuButton ListInButton { get; }

        IMenuButton ListOutButton { get; }

        //object SaveScrollPositions();

        //void UpdateScrollPositions(object token);

        void SetCursorToWait();

        void SetCursorToDefault();

        //string ListPrecedenceHintToolTipText { get; set; }

        string Title { get; set; }

        void NotifySyncStarting();

        void NotifySyncEnded();
    }
}
