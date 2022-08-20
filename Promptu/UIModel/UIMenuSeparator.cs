using System;
using System.Collections.Generic;
using System.Text;
using ZachJohnson.Promptu.UIModel.Interfaces;

namespace ZachJohnson.Promptu.UIModel
{
    internal class UIMenuSeparator : UIMenuItemBase
    {
        private IMenuSeparator correspondingSeparator;

        public UIMenuSeparator(string id)
            : base(id)
        {
            this.correspondingSeparator = InternalGlobals.GuiManager.ToolkitHost.Factory.ConstructNewMenuSeparator();
        }

        internal override IGenericMenuItem NativeInterface
        {
            get { return this.correspondingSeparator; }
        }

        protected override void SetAvailable(bool value)
        {
            this.correspondingSeparator.Available = value;
        }
    }
}
