//-----------------------------------------------------------------------
// <copyright file="ISkinInstanceFactory.cs" company="ZachJohnson">
//     Copyright (c) Zach Johnson. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace ZachJohnson.Promptu.SkinApi
{
    internal interface ISkinInstanceFactory
    {
        PromptuSkinInstance CreateNewInstance();
    }
}
