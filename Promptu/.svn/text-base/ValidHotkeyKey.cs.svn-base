using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace ZachJohnson.Promptu
{
    internal class ValidHotkeyKey
    {
        private string display;
        private Keys associatedKey;

        public ValidHotkeyKey(string display, Keys associatedKey)
        {
            this.display = display;
            this.associatedKey = associatedKey;
        }

        public Keys AssociatedKey
        {
           get { return this.associatedKey; }
        }

        public override string ToString()
        {
            return display;
        }
    }
}
