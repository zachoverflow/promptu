using System;
using System.Collections.Generic;
using System.Text;
using ZachJohnson.Promptu.UIModel.Interfaces;
using System.ComponentModel;

namespace ZachJohnson.Promptu.UIModel
{
    public class UIContextMenu
    {
        private INativeContextMenu nativeContextMenu;
        private UIComponentCollection<UIMenuItemBase, IGenericMenuItem> items;

        internal UIContextMenu(UIComponentCollection<UIMenuItemBase, IGenericMenuItem> items)
        {
            if (items == null)
            {
                throw new ArgumentNullException("items");
            }

            this.items = items;
        }

        internal UIContextMenu(INativeContextMenu nativeContextMenu)
        {
            if (nativeContextMenu == null)
            {
                throw new ArgumentNullException("nativeContextMenu");
            }

            this.nativeContextMenu = nativeContextMenu;
            this.items = new UIMenuItemCollection(nativeContextMenu.Items);
            //this.nativeContextMenu.Opening += this.HandleNativeContextMenuOpening;
        }

        public UIContextMenu()
            : this(InternalGlobals.GuiManager.ToolkitHost.Factory.ConstructContextMenu())
        {
        }

        //public event CancelEventHandler Opening;

        public UIComponentCollection<UIMenuItemBase, IGenericMenuItem> Items
        {
            get { return this.items; }
        }

        internal INativeContextMenu NativeContextMenuInterface
        {
            get { return this.nativeContextMenu; }
        }

        public object NativeContextMenu
        {
            get { return this.nativeContextMenu; }
        }

        //public object NativeObject
        //{
        //    get { return this.nativeContextMenu; }
        //}

        //protected virtual void OnOpening(CancelEventArgs e)
        //{
        //    CancelEventHandler handler = this.Opening;
        //    if (handler != null)
        //    {
        //        handler(this, e);
        //    }
        //}

        //private void HandleNativeContextMenuOpening(object sender, CancelEventArgs e)
        //{
        //    this.OnOpening(e);
        //}
    }
}
