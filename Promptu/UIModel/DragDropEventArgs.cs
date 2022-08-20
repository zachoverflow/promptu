using System;
using System.Collections.Generic;
using System.Text;

namespace ZachJohnson.Promptu.UIModel
{
    internal delegate object DataGetter(Type dataType);

    internal class DragDropEventArgs : EventArgs
    {
        private UIDragDropEffects effects = UIDragDropEffects.None;
        private UIDragDropEffects allowedEffects;
        private DataGetter dataGetter;

        public DragDropEventArgs(UIDragDropEffects allowedEffects, DataGetter dataGetter)
        {
            this.allowedEffects = allowedEffects;
            this.dataGetter = dataGetter;
        }

        public UIDragDropEffects AllowedEffects
        {
            get { return this.allowedEffects; }
            set { this.allowedEffects = value; }
        }

        public UIDragDropEffects Effects
        {
            get { return this.effects; }
            set { this.effects = value; }
        }

        public object GetData(Type dataType)
        {
            return this.dataGetter(dataType);
        }
    }
}
