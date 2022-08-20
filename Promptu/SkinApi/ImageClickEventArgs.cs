//-----------------------------------------------------------------------
// <copyright file="ImageClickEventArgs.cs" company="ZachJohnson">
//     Copyright (c) Zach Johnson. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace ZachJohnson.Promptu.SkinApi
{
    using System;

    public class ImageClickEventArgs : EventArgs
    {
        private string key;

        public ImageClickEventArgs(string key)
        {
            this.key = key;
        }

        public string Key
        {
            get { return this.key; }
        }
    }
}
