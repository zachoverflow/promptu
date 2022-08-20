using System;
using System.Collections.Generic;
using System.Text;

namespace ZachJohnson.Promptu.AssemblyCaching
{
    [global::System.Serializable]
    internal class CachedAssemblyNotFoundException : Exception
    {
        public CachedAssemblyNotFoundException()
        {
        }

        public CachedAssemblyNotFoundException(string message)
            : base(message)
        {
        }

        public CachedAssemblyNotFoundException(string message, Exception inner)
            : base(message, inner)
        {
        }

        protected CachedAssemblyNotFoundException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context)
        {
        }
    }
}
