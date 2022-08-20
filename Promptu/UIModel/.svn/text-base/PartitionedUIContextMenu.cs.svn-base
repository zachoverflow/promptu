using System;
using System.Collections.Generic;
using System.Text;
using ZachJohnson.Promptu.UIModel.Interfaces;

namespace ZachJohnson.Promptu.UIModel
{
    public class PartitionedUIContextMenu
    {
        private UIContextMenu top;
        private UIContextMenu center;
        private UIContextMenu bottom;
        
        private INativeContextMenu menu;

        internal PartitionedUIContextMenu(INativeContextMenu menu)
        {
            if (menu == null)
            {
                throw new ArgumentNullException("menu");
            }

            this.menu = menu;

            NativeUICollectionBinding<IGenericMenuItem> topBinding = new NativeUICollectionBinding<IGenericMenuItem>(
                new NativeUICollectionBinding<IGenericMenuItem>.InsertMethod(this.InsertTop),
                new NativeUICollectionBinding<IGenericMenuItem>.ClearMethod(this.ClearTop),
                new NativeUICollectionBinding<IGenericMenuItem>.RemoveMethod(this.RemoveTop));

            NativeUICollectionBinding<IGenericMenuItem> centerBinding = new NativeUICollectionBinding<IGenericMenuItem>(
                new NativeUICollectionBinding<IGenericMenuItem>.InsertMethod(this.InsertCenter),
                new NativeUICollectionBinding<IGenericMenuItem>.ClearMethod(this.ClearCenter),
                new NativeUICollectionBinding<IGenericMenuItem>.RemoveMethod(this.RemoveCenter));

            NativeUICollectionBinding<IGenericMenuItem> bottomBinding = new NativeUICollectionBinding<IGenericMenuItem>(
                new NativeUICollectionBinding<IGenericMenuItem>.InsertMethod(this.InsertBottom),
                new NativeUICollectionBinding<IGenericMenuItem>.ClearMethod(this.ClearBottom),
                new NativeUICollectionBinding<IGenericMenuItem>.RemoveMethod(this.RemoveBottom));

            this.top = new UIContextMenu(new PassthroughNativeContextMenu(topBinding));
            this.center = new UIContextMenu(new PassthroughNativeContextMenu(centerBinding));
            this.bottom = new UIContextMenu(new PassthroughNativeContextMenu(bottomBinding));
        }

        public PartitionedUIContextMenu()
            : this(InternalGlobals.GuiManager.ToolkitHost.Factory.ConstructContextMenu())
        {
        }

        public object NativeContextMenu
        {
            get { return this.menu; }
        }

        public UIContextMenu Top
        {
            get { return this.top; }
        }

        public UIContextMenu Center
        {
            get { return this.center; }
        }

        public UIContextMenu Bottom
        {
            get { return this.bottom; }
        }

        private void InsertTop(int index, IGenericMenuItem item)
        {
            this.menu.Items.Insert(index, item);
        }

        private void ClearTop()
        {
            foreach (UIMenuItemBase item in this.top.Items)
            {
                this.menu.Items.Remove(item.NativeInterface);
            }
        }

        private void RemoveTop(IGenericMenuItem item)
        {
            this.menu.Items.Remove(item);
        }

        private void InsertCenter(int index, IGenericMenuItem item)
        {
            this.menu.Items.Insert(this.top.Items.Count + index, item);
        }

        private void ClearCenter()
        {
            foreach (UIMenuItemBase item in this.center.Items)
            {
                this.menu.Items.Remove(item.NativeInterface);
            }
        }

        private void RemoveCenter(IGenericMenuItem item)
        {
            this.menu.Items.Remove(item);
        }

        private void InsertBottom(int index, IGenericMenuItem item)
        {
            this.menu.Items.Insert(this.top.Items.Count + this.center.Items.Count + index, item);
        }

        private void ClearBottom()
        {
            foreach (UIMenuItemBase item in this.bottom.Items)
            {
                this.menu.Items.Remove(item.NativeInterface);
            }
        }

        private void RemoveBottom(IGenericMenuItem item)
        {
            this.menu.Items.Remove(item);
        }
    }
}
