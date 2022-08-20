using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace ZachJohnson.Promptu.WpfUI.UIComponents
{
    internal class NotBinding : Binding
    {
        private Binding binding;
        private object notEqualTo;

        public NotBinding(Binding binding, object notEqualTo)
        {
            this.binding = binding;
            this.notEqualTo = notEqualTo;
            this.Converter = new NotEqualConverter(this);
        }

        public Binding Binding
        {
            get { return this.binding; }
        }

        public object NotEqualTo
        {
            get { return this.notEqualTo; }
        }
    }
}
