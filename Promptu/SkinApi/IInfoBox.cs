//-----------------------------------------------------------------------
// <copyright file="IInfoBox.cs" company="ZachJohnson">
//     Copyright (c) Zach Johnson. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace ZachJohnson.Promptu.SkinApi
{
    using System.Drawing;

    public interface IInfoBox : IUIElement
    {
        bool Visible { get; }

        Size Size { get; set; }

        Size ActualSize { get; }

        Point Location { get; set; }

        bool TopMost { get; set; }

        int? MaxWidth { get; set; }

        Size GetPreferredSize(Size proposedSize);

        void Show();

        void Hide();

        void Refresh();

        void BringToFront();
    }
}
