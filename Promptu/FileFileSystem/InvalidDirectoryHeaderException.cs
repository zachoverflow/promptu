//-----------------------------------------------------------------------
// <copyright file="InvalidDirectoryHeaderException.cs" company="ZachJohnson">
//     Copyright (c) Zach Johnson. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace ZachJohnson.Promptu.FileFileSystem
{
    using System;

    [global::System.Serializable]
    internal class InvalidDirectoryHeaderException : FileFileSystemException
    {
        public InvalidDirectoryHeaderException()
        {
        }

        public InvalidDirectoryHeaderException(string message)
            : base(message)
        {
        }

        public InvalidDirectoryHeaderException(string message, Exception inner)
            : base(message, inner)
        {
        }

        protected InvalidDirectoryHeaderException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context)
        {
        }
    }
}
