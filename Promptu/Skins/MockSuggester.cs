//-----------------------------------------------------------------------
// <copyright file="MockSuggester.cs" company="ZachJohnson">
//     Copyright (c) Zach Johnson. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable 0067

namespace ZachJohnson.Promptu.Skins
{
    using System;
    using System.Collections.Generic;
    using System.Windows.Forms;
    using ZachJohnson.Promptu.SkinApi;

    internal class MockSuggester : ISuggestionProvider
    {
        private List<object> images = new List<object>();

        public MockSuggester()
        {
        }

        public event EventHandler DesiredIconSizeChanged;

        public event System.ComponentModel.CancelEventHandler SelectedItemContextMenuOpening;

        public event EventHandler UserInteractionFinished;

        public event EventHandler VisibleChanged;

        public event System.Windows.Forms.MouseEventHandler MouseDoubleClick;

        public event EventHandler SelectedIndexChanged;

        public IList<object> Images
        {
            get { return this.images; }
        }

        public System.Drawing.Point Location
        {
            get
            {
                return new System.Drawing.Point();
            }

            set
            {
            }
        }

        public System.Drawing.Size Size
        {
            get
            {
                return new System.Drawing.Size();
            }

            set
            {
            }
        }

        public System.Drawing.Size SaveSize
        {
            get { return new System.Drawing.Size(); }
            set { }
        }

        public IntPtr Handle
        {
            get { return IntPtr.Zero; }
        }

        public int SelectedIndex
        {
            get
            {
                return -1;
            }

            set
            {
            }
        }

        public bool TopMost
        {
            get { return false; }
            set { }
        }

        public PluginModel.OptionPage Options
        {
            get { return null; }
        }

        public PluginModel.ObjectPropertyCollection SavingProperties
        {
            get { return null; }
        }

        public int DesiredIconSize
        {
            get { return 16; }
        }

        public UIModel.UIContextMenu SelectedItemContextMenu
        {
            get { throw new NotImplementedException(); }
        }

        public int MinimumWidth
        {
            get { return 0; }
        }

        public Padding Padding
        {
            get { return new Padding(); }
        }

        public int ItemHeight
        {
            get { return 1; }
        }

        public object UIObject
        {
            get { throw new NotSupportedException(); }
        }

        public int ItemCount
        {
            get { return 0; }
        }

        public bool SuppressItemInfoToolTips
        {
            get { return true; }
        }

        public bool Visible
        {
            get { return false; }
        }

        public bool ContainsFocus
        {
            get { return false; }
        }

        public void DoPageUpOrDown(Direction direction)
        {
        }

        public void ScrollToTop()
        {
        }

        public void ScrollToEnd()
        {
        }

        public System.Drawing.Rectangle GetItemBounds(int index)
        {
            return new System.Drawing.Rectangle();
        }

        public void BringToFront()
        {
        }

        public void AddItem(SuggestionItem item)
        {
        }

        public void CenterSelectedItem()
        {
        }

        public SuggestionItem GetItem(int index)
        {
            return null;
        }

        public void ClearItems()
        {
        }

        public void Hide()
        {
        }

        public void Show()
        {
        }

        public void Show(IWin32Window owner)
        {
        }

        public void Activate()
        {
        }

        public void RefreshThreadSafe()
        {
        }

        public void EnsureCreated()
        {
        }

        public void ScrollSuggestions(Direction direction)
        {
        }

        public void EnsureVisible(int index)
        {
        }

        public void ScrollSelectedItemIntoView()
        {
        }
    }
}

#pragma warning restore 0067
