using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;

namespace ZachJohnson.Promptu.UIModel.Interfaces
{
    internal interface ISetupPanel : IDragSource, IBatchUpdatable
    {
        //event EventHandler UISettingChanged;

        event EventHandler ItemDoubleClick;

        event CancelEventHandler ItemContextMenuOpening;

        event KeyEventHandler ItemKeyDown;

        event EventHandler ItemDrag;

        UIContextMenu ItemContextMenu { get; }

        bool MouseIsOverAnItem(out bool notOnHeaders);

        IToolbarButton EditButton { get; }

        IToolbarButton NewButton { get; }

        IToolbarButton DeleteButton { get; }

        string CountDisplayText { get; set; }

        bool Enabled { get; set; }

        ISimpleCollectionViewer CollectionViewer { get; }

        void FocusCollectionViewer();

        void SelectAndUpdate();
    }
}
