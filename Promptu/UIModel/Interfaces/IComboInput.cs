using System;
using System.Collections.Generic;
using System.Text;

namespace ZachJohnson.Promptu.UIModel.Interfaces
{
    internal interface IComboInput
    {
        event EventHandler TextChanged;

        event EventHandler SelectedIndexChanged;

        string Text { get; set; }

        object[] Values { set; }

        void AddValue(object value);

        void AddRange<T>(IEnumerable<T> values);

        void Clear();

        //bool Freeform { get; set; }

        object SelectedValue { get; set; }

        int SelectedIndex { get; set; }

        int ValueCount { get; }

        bool Enabled { get; set; }
    }
}
