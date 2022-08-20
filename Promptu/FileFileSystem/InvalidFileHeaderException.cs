//-----------------------------------------------------------------------
// <copyright file="InvalidFileHeaderException.cs" company="ZachJohnson">
//     Copyright (c) Zach Johnson. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace ZachJohnson.Promptu.FileFileSystem
{
    using System;

    [global::System.Serializable]
    internal class InvalidFileHeaderException : FileFileSystemException
    {
        public InvalidFileHeaderException()
        {
        }

        public InvalidFileHeaderException(string message)
            : base(message)
        {
        }

        public InvalidFileHeaderException(string message, Exception inner)
            : base(message, inner)
        {
        }

        protected InvalidFileHeaderException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context)
        {
        }
    }
}
