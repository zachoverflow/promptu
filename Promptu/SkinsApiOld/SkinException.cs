using System;
using System.Collections.Generic;
using System.Text;

namespace ZachJohnson.Promptu.SkinsApi
{
    [global::System.Serializable]
    internal class SkinException : Exception
    {
        public SkinException() { }
        public SkinException(string message) : base(message) { }
        public SkinException(string message, Exception inner) : base(message, inner) { }
        protected SkinException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
}
