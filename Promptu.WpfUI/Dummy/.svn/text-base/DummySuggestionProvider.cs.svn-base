using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZachJohnson.Promptu.SkinApi;

namespace ZachJohnson.Promptu.WpfUI.Dummy
{
    class DummySuggestionProvider : ISuggestionProvider
    {
        private List<object> images = new List<object>();

        public event EventHandler SelectedIndexChanged;

        public event EventHandler UserInteractionFinished;

        public event EventHandler VisibleChanged;

        public event System.Windows.Forms.MouseEventHandler MouseDoubleClick;

        public event EventHandler DesiredIconSizeChanged;

        public event System.ComponentModel.CancelEventHandler SelectedItemContextMenuOpening;

        public UIModel.UIContextMenu SelectedItemContextMenu
        {
            get { return new UIModel.UIContextMenu(); }
        }

        public int DesiredIconSize
        {
            get { return 16; }
        }

        public IList<object> Images
        {
            get { return this.images; }
        }

        public bool Visible
        {
            get { return true; }
        }

        public bool ContainsFocus
        {
            get { return false; }
        }

        public int SelectedIndex
        {
            get;
            set;
        }

        public SuggestionItem GetItem(int index)
        {
            return null;
        }

        public void EnsureCreated()
        {
        }

        public void ClearItems()
        {
        }

        public void AddItem(SuggestionItem item)
        {
        }

        public void CenterSelectedItem()
        {
        }

        public bool SuppressItemInfoToolTips
        {
            get { return false; }
        }

        public int ItemCount
        {
            get { return 0; }
        }

        public void DoPageUpOrDown(Direction direction)
        {
        }

        public void EnsureVisible(int index)
        {
        }

        public void ScrollToTop()
        {
        }

        public void ScrollToEnd()
        {
        }

        public void Hide()
        {
        }

        public void Show()
        {
        }

        public void Activate()
        {
        }

        public void RefreshThreadSafe()
        {
        }

        public void ScrollSuggestions(Direction direction)
        {
        }

        public System.Drawing.Point Location
        {
            get;
            set;
        }

        public System.Drawing.Size Size
        {
            get;
            set;
        }

        public System.Drawing.Size SaveSize
        {
            get;
            set;
        }

        public int MinimumWidth
        {
            get { return 16; }
        }

        public System.Windows.Forms.Padding Padding
        {
            get { return new System.Windows.Forms.Padding(); }
        }

        public int ItemHeight
        {
            get { return 16; }
        }

        public System.Drawing.Rectangle GetItemBounds(int index)
        {
            return new System.Drawing.Rectangle();
        }

        public void BringToFront()
        {
        }

        public object UIObject
        {
            get { return this; }
        }

        public PluginModel.OptionPage Options
        {
            get { return null; }
        }

        public PluginModel.ObjectPropertyCollection SavingProperties
        {
            get { return null; }
        }


        public void ScrollSelectedItemIntoView()
        {
        }
    }
}
