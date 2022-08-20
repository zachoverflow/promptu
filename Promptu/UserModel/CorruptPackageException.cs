using System;
using System.Collections.Generic;
using System.Text;

namespace ZachJohnson.Promptu.UserModel
{
    [global::System.Serializable]
    internal class CorruptPackageException : Exception
    {
        public CorruptPackageException()
        {
        }

        public CorruptPackageException(string message)
            : base(message)
        {
        }

        public CorruptPackageException(string message, Exception inner)
            : base(message, inner)
        {
        }

        protected CorruptPackageException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context)
        {
        }
    }
}
