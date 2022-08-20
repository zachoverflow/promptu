using System;
using System.Collections.Generic;
using System.Text;

namespace ZachJohnson.Promptu.UI
{
    [global::System.Serializable]
    public class InvalidValueException : Exception
    {
        private string uiResourceKey;

        public InvalidValueException()
        {
        }

        public InvalidValueException(string message, string uiResourceKey)
            : base(message)
        {
            this.uiResourceKey = uiResourceKey;
        }

        public InvalidValueException(string message, Exception inner)
            : base(message, inner)
        {
        }

        protected InvalidValueException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context)
        {
        }

        public string UIResourceKey
        {
            get { return this.uiResourceKey; }
        }
    }
}
