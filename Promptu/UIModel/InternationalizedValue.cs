using System;
using System.Collections.Generic;
using System.Text;

namespace ZachJohnson.Promptu.UIModel
{
    internal class InternationalizedValue<T> 
    {
        private string representation;
        private T value;

        public InternationalizedValue(T value, string representation)
        {
            this.value = value;
            this.representation = representation;
        }

        public string Representation
        {
            get { return this.representation; }
        }

        public T Value
        {
            get { return this.value; }
        }

        public override string ToString()
        {
            return representation;
        }
    }
}
