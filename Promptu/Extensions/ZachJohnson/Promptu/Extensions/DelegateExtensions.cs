//-----------------------------------------------------------------------
// <copyright file="DelegateExtensions.cs" company="ZachJohnson">
//     Copyright (c) Zach Johnson. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace ZachJohnson.Promptu.Extensions
{
    internal static class DelegateExtensions
    {
        public static void InvokeIfNotNull(this ParameterlessVoid method)
        {
            if (method != null)
            {
                method();
            }
        }
    }
}
