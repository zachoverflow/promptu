using System;
using System.Collections.Generic;
using System.Text;
using ZachJohnson.Promptu.UIModel.Interfaces;

namespace ZachJohnson.Promptu.PTK
{
    internal class LabelWidget : GenericWidget<ILabel>
    {
        public LabelWidget(string id)
            : base(id, Globals.GuiManager.ToolkitHost.Factory.ConstructLabel())
        {
        }

        public event EventHandler TextChanged;

        public string Text
        {
            get 
            { 
                return this.NativeInterface.Text;
            }

            set 
            {
                this.NativeInterface.Text = value;
                this.OnTextChanged(EventArgs.Empty);
            }
        }

        protected virtual void OnTextChanged(EventArgs e)
        {
            EventHandler handler = this.TextChanged;
            if (handler != null)
            {
                handler(this, e);
            }
        }
    }
}
