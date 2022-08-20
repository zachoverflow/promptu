using System;
using System.Collections.Generic;
using System.Text;

namespace ZachJohnson.Promptu
{
    [global::System.AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = true)]
    public class ParameterDocumentationAttribute : Attribute
    {
        private readonly string documentation;
        private readonly int index;

        public ParameterDocumentationAttribute(int index, string documentation)
        {
            this.index = index;
            this.documentation = documentation;
        }

        public int Index
        {
            get { return this.index; }
        }

        public string Documentation
        {
            get { return this.GetDocumentation(); }
        }

        protected virtual string GetDocumentation()
        {
            return this.documentation;
        }
    }
}
