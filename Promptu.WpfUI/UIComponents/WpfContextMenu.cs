using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using ZachJohnson.Promptu.UIModel.Interfaces;
using ZachJohnson.Promptu.UIModel;
using System.ComponentModel;
using System.Windows.Media;

namespace ZachJohnson.Promptu.WpfUI.UIComponents
{
    internal class WpfContextMenu : ContextMenu, INativeContextMenu
    {
        private NativeUICollectionBinding<IGenericMenuItem> itemsBinding;

        public WpfContextMenu()
        {
            this.itemsBinding = new NativeUICollectionBinding<IGenericMenuItem>(
                new NativeUICollectionBinding<IGenericMenuItem>.InsertMethod(this.InsertIntoItems),
                new NativeUICollectionBinding<IGenericMenuItem>.ClearMethod(this.ClearItems),
                new NativeUICollectionBinding<IGenericMenuItem>.RemoveMethod(this.RemoveItem));

            TextOptions.SetTextFormattingMode(this, TextFormattingMode.Display);
        }

        public event CancelEventHandler Opening;

        INativeUICollection<IGenericMenuItem> INativeContextMenu.Items
        {
            get { return this.itemsBinding; }
        }

        private void InsertIntoItems(int index, IGenericMenuItem item)
        {
            this.Items.Insert(index, item);
        }

        private void ClearItems()
        {
            this.Items.Clear();
        }

        private void RemoveItem(IGenericMenuItem item)
        {
            this.Items.Remove(item);
        }

        protected virtual void OnOpening(CancelEventArgs e)
        {
            CancelEventHandler handler = this.Opening;
            if (handler != null)
            {
                handler(this, e);
            }
        }
    }
}
