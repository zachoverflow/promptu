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

namespace ZachJohnson.Promptu.SkinApi
{
    using System.Drawing;

    public interface ILayoutManager
    {
        void PositionPrompt(PositioningContext context, PositioningMode mode, Point? suggestedPosition);

        void PositionSuggestionProvider(PositioningContext context);

        void PositionItemInfoBox(PositioningContext context, IInfoBox itemInfoBox, out Size preferredSize);

        void PositionParameterHelpBox(PositioningContext context, IInfoBox parameterHelpBox, out Size preferredSize);

        void PositionProgressBox(PositioningContext context, IInfoBox progressBox, out Size preferredSize);

        void PositionInfoBox(PositioningContext context, IInfoBox infoBox, out Size preferredSize);
    }
}
