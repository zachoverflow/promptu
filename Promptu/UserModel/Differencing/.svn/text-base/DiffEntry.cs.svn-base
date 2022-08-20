using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;

namespace ZachJohnson.Promptu.UserModel.Differencing
{
    internal class DiffEntry<T>
    {
        private Comparison<T> valueComparison;
        private T baseValue;
        private T revisedValue;
        private bool hasChanged;

        public DiffEntry(T baseValue, T revisedValue, Comparison<T> valueComparison)
        {
            this.valueComparison = valueComparison;
            this.baseValue = baseValue;
            this.revisedValue = revisedValue;
            this.hasChanged = valueComparison(baseValue, revisedValue) != 0;
        }

        public T BaseValue
        {
            get { return this.baseValue; }
        }

        public T RevisedValue
        {
            get { return this.revisedValue; }
        }

        public bool HasChanged
        {
            get { return this.hasChanged; }
        }

        public Comparison<T> ValueComparison
        {
            get { return this.valueComparison; }
        }

        public override string ToString()
        {
            return String.Format(CultureInfo.InvariantCulture, "Changed: {0} | Base: {1} | Revised: {2}", this.hasChanged, this.baseValue.ToString(), this.revisedValue.ToString());
        }
    }
}
