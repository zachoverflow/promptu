using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZachJohnson.Promptu.SkinApi;

namespace ZachJohnson.Promptu.WpfUI.ClassicSkin
{
    internal class ClassicSkinInstance : PromptuSkinInstance
    {
        public ClassicSkinInstance()
            : base(
                null,
                new ClassicPrompt(),
                null,
                null)
        {
        }
    }
}
