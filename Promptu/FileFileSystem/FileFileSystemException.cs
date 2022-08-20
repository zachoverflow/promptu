//-----------------------------------------------------------------------
// <copyright file="FileFileSystemException.cs" company="ZachJohnson">
//     Copyright (c) Zach Johnson. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace ZachJohnson.Promptu.FileFileSystem
{
    using System;

    [global::System.Serializable]
    internal class FileFileSystemException : Exception
    {
        public FileFileSystemException()
        {
        }

        public FileFileSystemException(string message)
            : base(message)
        {
        }

        public FileFileSystemException(string message, Exception inner)
            : base(message, inner)
        {
        }

        protected FileFileSystemException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context)
        {
        }
    }
}
