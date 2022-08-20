//-----------------------------------------------------------------------
// <copyright file="ILayoutManager.cs" company="ZachJohnson">
//     Copyright (c) Zach Johnson. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

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
