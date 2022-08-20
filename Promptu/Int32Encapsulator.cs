using System;
using System.Collections.Generic;
using System.Text;

namespace ZachJohnson.Promptu
{
    internal class Int32Encapsulator
    {
        private int value;

        public Int32Encapsulator(int value)
        {
            this.value = value;
        }

        public static implicit operator int(Int32Encapsulator intEncapsulator)
        {
            if (intEncapsulator == null)
            {
                throw new ArgumentNullException("intEncapsulator");
            }

            return intEncapsulator.value;
        }

        public static implicit operator Int32Encapsulator(int i)
        {
            return new Int32Encapsulator(i);
        }
    }
}
