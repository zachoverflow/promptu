//-----------------------------------------------------------------------
// <copyright file="BadFileFormatException.cs" company="ZachJohnson">
//     Copyright (c) Zach Johnson. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace ZachJohnson.Promptu.FileFileSystem
{
    using System;

    [global::System.Serializable]
    internal class BadFileFormatException : FileFileSystemException
    {
        public BadFileFormatException()
        {
        }

        public BadFileFormatException(string message)
            : base(message)
        {
        }

        public BadFileFormatException(string message, Exception inner)
            : base(message, inner)
        {
        }

        protected BadFileFormatException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context)
        {
        }
    }
}
