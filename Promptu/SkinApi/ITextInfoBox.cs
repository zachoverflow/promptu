//-----------------------------------------------------------------------
// <copyright file="ITextInfoBox.cs" company="ZachJohnson">
//     Copyright (c) Zach Johnson. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace ZachJohnson.Promptu.SkinApi
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using ZachJohnson.Promptu.UIModel.RichText;

    public interface ITextInfoBox : IInfoBox
    {
        event EventHandler<ImageClickEventArgs> ImageClick;

        InfoType InfoType { get; set; }

        RTElement Content { get; set; }

        List<Bitmap> Images { get; }
    }
}
