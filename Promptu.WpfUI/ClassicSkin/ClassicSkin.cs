using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZachJohnson.Promptu.SkinApi;

namespace ZachJohnson.Promptu.WpfUI.ClassicSkin
{
    internal class ClassicSkin : PromptuSkin
    {
        public ClassicSkin()
            : base(
            Localization.Promptu.ClassicSkinName,
            "www.promptulauncher.com/classicskin",
            "Zach Johnson",
            //null,
            "http://www.zachjohnson.net/",
            Localization.Promptu.ClassicSkinDescription,
            null,
            new ClassicSkinFactory())
        {
        }
    }
}
