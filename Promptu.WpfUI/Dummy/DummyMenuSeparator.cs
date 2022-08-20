using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZachJohnson.Promptu.UIModel.Interfaces;

namespace ZachJohnson.Promptu.WpfUI.Dummy
{
    class DummyMenuSeparator : IMenuSeparator
    {
        public bool Available
        {
            get;
            set;
        }
    }
}
