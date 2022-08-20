using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZachJohnson.Promptu.SkinApi;

namespace ZachJohnson.Promptu.WpfUI.Dummy
{
    class DummySkinInstanceFactory : ISkinInstanceFactory
    {
        public PromptuSkinInstance CreateNewInstance()
        {
            return new DummySkinInstance();
        }
    }
}
