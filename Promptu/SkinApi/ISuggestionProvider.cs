//-----------------------------------------------------------------------
// <copyright file="ISuggestionProvider.cs" company="ZachJohnson">
//     Copyright (c) Zach Johnson. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace ZachJohnson.Promptu.SkinApi
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;
    using ZachJohnson.Promptu.UIModel;

    public interface ISuggestionProvider : IUIElement, IHasOptionsAndProperties
    {
        event EventHandler SelectedIndexChanged;

        event EventHandler UserInteractionFinished;

        event EventHandler VisibleChanged;

        event MouseEventHandler MouseDoubleClick;

        event EventHandler DesiredIconSizeChanged;

        event CancelEventHandler SelectedItemContextMenuOpening;

        UIContextMenu SelectedItemContextMenu { get; }

        int DesiredIconSize { get; }

        IList<object> Images { get; }

        bool Visible { get; }

        bool ContainsFocus { get; }

        int SelectedIndex { get; set; }

        bool SuppressItemInfoToolTips { get; }

        int ItemCount { get; }

        Point Location { get; set; }

        Size Size { get; set; }

        Size SaveSize { get; set; }

        int MinimumWidth { get; }

        Padding Padding { get; }

        int ItemHeight { get; }

        SuggestionItem GetItem(int index);

        void EnsureCreated();

        void ClearItems();

        void AddItem(SuggestionItem item);

        void CenterSelectedItem();

        void ScrollSelectedItemIntoView();

        void DoPageUpOrDown(Direction direction);

        void EnsureVisible(int index);

        void ScrollToTop();

        void ScrollToEnd();

        void Hide();

        void Show();

        void Activate();

        void RefreshThreadSafe();

        void ScrollSuggestions(Direction direction);

        Rectangle GetItemBounds(int index);

        void BringToFront();   
    }
}
