using System;
using System.Collections.Generic;
using System.Text;

namespace ZachJohnson.Promptu.UserModel
{
    [global::System.Serializable]
    internal class NewerPackageVersionException : Exception
    {
        public NewerPackageVersionException()
        {
        }

        public NewerPackageVersionException(string message)
            : base(message)
        {
        }

        public NewerPackageVersionException(string message, Exception inner)
            : base(message, inner)
        {
        }

        protected NewerPackageVersionException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context)
        {
        }
    }
}
