//-----------------------------------------------------------------------
// <copyright file="BadLineException.cs" company="ZachJohnson">
//     Copyright (c) Zach Johnson. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace ZachJohnson.Promptu.FileFileSystem
{
    using System;
    [global::System.Serializable]
    internal class BadLineException : FileFileSystemException
    {
        public BadLineException()
        {
        }

        public BadLineException(string message)
            : base(message)
        {
        }

        public BadLineException(string message, Exception inner)
            : base(message, inner)
        {
        }

        protected BadLineException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context)
        {
        }
    }
}
