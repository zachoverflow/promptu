// Copyright 2022 Zach Johnson
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

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
