//-----------------------------------------------------------------------
// <copyright file="NotEnoughArgumentsConversionException.cs" company="ZachJohnson">
//     Copyright (c) Zach Johnson. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace ZachJohnson.Promptu.Itl.AbstractSyntaxTree
{
    using System;

    [global::System.Serializable]
    internal class NotEnoughArgumentsConversionException : ConversionException
    {
        public NotEnoughArgumentsConversionException()
        {
        }

        public NotEnoughArgumentsConversionException(string message)
            : base(message)
        {
        }

        public NotEnoughArgumentsConversionException(string message, Exception inner)
            : base(message, inner)
        {
        }

        protected NotEnoughArgumentsConversionException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context)
        {
        }
    }
}
