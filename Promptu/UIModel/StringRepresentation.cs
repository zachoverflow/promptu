using System;
using System.Collections.Generic;
using System.Text;

namespace ZachJohnson.Promptu.UIModel
{
    internal class StringRepresentation<T>
    {
        private T value;
        private string representation;

        public StringRepresentation(T value, string representation)
        {
            this.value = value;
            this.representation = representation;
        }

        public T Value
        {
            get { return this.value; }
        }

        public string Representation
        {
            get { return this.representation; }
        }

        public override string ToString()
        {
            return this.Representation;
        }
    }
}
