using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace ZachJohnson.Promptu.UIModel.Presenters
{
    internal class ListConversionInfo
    {
        private IList values;
        private bool readOnly;

        public ListConversionInfo(IList values, bool readOnly)
        {
            if (values == null)
            {
                throw new ArgumentNullException("values");
            }

            this.values = values;
            this.readOnly = readOnly;
        }

        public IList Values
        {
            get { return this.values; }
        }

        public bool ReadOnly
        {
            get { return this.readOnly; }
        }
    }
}
