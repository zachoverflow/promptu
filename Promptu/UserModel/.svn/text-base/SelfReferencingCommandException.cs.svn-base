using System;
using System.Collections.Generic;
using System.Text;

namespace ZachJohnson.Promptu.UserModel
{
    [global::System.Serializable]
    internal class SelfReferencingCommandException : Exception
    {
        private string commandName;
        private List listFrom;

        public SelfReferencingCommandException()
        {
        }

        public SelfReferencingCommandException(string message, string commandName, List listFrom)
            : base(message)
        {
            this.commandName = commandName;
            this.listFrom = listFrom;
        }

        public SelfReferencingCommandException(string message, Exception inner)
            : base(message, inner)
        {
        }

        protected SelfReferencingCommandException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context)
        {
        }

        public string CommandName
        {
            get { return this.commandName; }
        }

        public List ListFrom
        {
            get { return this.listFrom; }
        }
    }
}
