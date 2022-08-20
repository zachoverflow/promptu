using System;
using System.Collections.Generic;
using System.Text;
using ZachJohnson.Promptu.SkinApi;

namespace ZachJohnson.Promptu.UIModel.Interfaces
{
    internal interface IAppearanceTabPanel
    {
        event EventHandler<ObjectEventArgs<PromptuSkin>> CreatorContactLinkClicked;

        event EventHandler<ObjectEventArgs<PromptuSkin>> ConfigureSkinClicked;

        event EventHandler SelectedSkinChanged;

        string MainInstructions { set; }

        void ClearSkins();

        void AddSkins(IEnumerable<PromptuSkin> skins);

        PromptuSkin SelectedSkin { get; set; }
    }
}
