using System;
using System.Collections.Generic;
using System.Text;

namespace ZachJohnson.Promptu.UserModel
{
    [global::System.Serializable]
    internal class LockedProfileException : Exception
    {
        public LockedProfileException()
        {
        }

        public LockedProfileException(string message)
            : base(message)
        {
        }

        public LockedProfileException(string message, Exception inner)
            : base(message, inner)
        {
        }

        protected LockedProfileException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context)
        {
        }
    }
}
