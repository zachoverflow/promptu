using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZachJohnson.Promptu.UIModel.Interfaces;
using ZachJohnson.Promptu.UIModel;

namespace ZachJohnson.Promptu.WpfUI.Dummy
{
    internal class DummyMenuItem : IMenuItem
    {
        private INativeUICollection<IGenericMenuItem> subItems;
        
        public DummyMenuItem()
        {
            this.subItems = new NativeUICollectionBinding<IGenericMenuItem>(
                new NativeUICollectionBinding<IGenericMenuItem>.InsertMethod(this.InsertIntoItems),
                new NativeUICollectionBinding<IGenericMenuItem>.ClearMethod(this.ClearItems),
                new NativeUICollectionBinding<IGenericMenuItem>.RemoveMethod(this.RemoveItem));
        }

        public event EventHandler Click;

        public string Text
        {
            get;
            set;
        }

        public string ToolTipText
        {
            get;
            set;
        }

        public bool Enabled
        {
            get;
            set;
        }

        public bool Available
        {
            get;
            set;
        }

        public UIModel.TextStyle TextStyle
        {
            get;
            set;
        }

        public object Image
        {
            get;
            set;
        }

        public INativeUICollection<IGenericMenuItem> SubItems
        {
            get { return this.subItems; }
        }

        private void InsertIntoItems(int index, IGenericMenuItem item)
        {
        }

        private void ClearItems()
        {
        }

        private void RemoveItem(IGenericMenuItem item)
        {
        }
    }
}
