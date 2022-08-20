//-----------------------------------------------------------------------
// <copyright file="IProgressInfoBox.cs" company="ZachJohnson">
//     Copyright (c) Zach Johnson. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace ZachJohnson.Promptu.SkinApi
{
    public interface IProgressInfoBox : IInfoBox
    {
        string Text { set; }

        int Mininum { set; }

        int Maximum { set; }

        int Value { get; set; }
    }
}
