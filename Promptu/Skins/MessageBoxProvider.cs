//-----------------------------------------------------------------------
// <copyright file="MessageBoxProvider.cs" company="ZachJohnson">
//     Copyright (c) Zach Johnson. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace ZachJohnson.Promptu.Skins
{
    using System.Drawing;
    using ZachJohnson.Promptu.SkinApi;
    using ZachJohnson.Promptu.UIModel.RichText;

    internal static class MessageBoxProvider
    {
        public static void GiveError(string message, PromptHandler.SeparateSuggestionHandler suggestionHandler, ISuggestionProvider suggester, int resetsToDestroy)
        {
            ITextInfoBox informationBox = InternalGlobals.CurrentSkinInstance.CreateTextInfoBox();
            informationBox.InfoType = InfoType.Error;
            informationBox.MaxWidth = 500;
            informationBox.Content = new Text(message, TextStyle.Normal);

            Size preferredSize;

            InternalGlobals.CurrentSkinInstance.LayoutManager.PositionInfoBox(
                new PositioningContext(InternalGlobals.CurrentSkin, InternalGlobals.CurrentSkinInstance, new InfoBoxes(suggestionHandler.InformationBoxMananger)),
                informationBox,
                out preferredSize);

            suggestionHandler.InformationBoxMananger.RegisterAndShow(informationBox, true, suggester);

            if (resetsToDestroy > 0)
            {
                suggestionHandler.InformationBoxMananger.SetResetsTillDestroy(informationBox, resetsToDestroy);
            }
        }
    }
}
