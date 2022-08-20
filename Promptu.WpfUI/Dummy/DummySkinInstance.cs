using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZachJohnson.Promptu.SkinApi;

namespace ZachJohnson.Promptu.WpfUI.Dummy
{
    class DummySkinInstance : PromptuSkinInstance
    {
        public DummySkinInstance()
            : base(
#if WPF_MINIMAL
                new ZachJohnson.Promptu.WpfUI.DefaultSkin.DefaultLayoutManager(),
                new ZachJohnson.Promptu.WpfUI.DefaultSkin.DefaultPrompt(),
                new ZachJohnson.Promptu.WpfUI.DefaultSkin.DefaultSuggestionProvider(),
#else
                new ZachJohnson.Promptu.WpfUI.DefaultSkin.DefaultLayoutManager(),
                new DummyPrompt(),
                new DummySuggestionProvider(),
#endif
                null,
                true)
        {
        }
    }
}
