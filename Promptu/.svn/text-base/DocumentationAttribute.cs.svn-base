using System;
using System.Collections.Generic;
using System.Text;

namespace ZachJohnson.Promptu
{
    [global::System.AttributeUsage(AttributeTargets.All, Inherited = false, AllowMultiple = false)]
    public class DocumentationAttribute : Attribute
    {
        private string documentation;

        public DocumentationAttribute(string documentation)
        {
            this.documentation = documentation;
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
