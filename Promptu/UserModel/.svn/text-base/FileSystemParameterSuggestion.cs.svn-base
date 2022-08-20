using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Globalization;

namespace ZachJohnson.Promptu.UserModel
{
    internal class FileSystemParameterSuggestion : ParameterSuggestion
    {
        public const string TypeAttributeValue = "FileSystem";
        //private static FileSystemParameterSuggestion value = new FileSystemParameterSuggestion();
        private string filter;

        public FileSystemParameterSuggestion(string filter)
        {
            this.filter = filter;
        }

        public string Filter
        {
            get { return this.filter; }
        }

        public string GetDisplay()
        {
            if (String.IsNullOrEmpty(this.filter))
            {
                return Localization.UIResources.FileSystemNoFilter;
            }
            else
            {
                return String.Format(CultureInfo.CurrentCulture, Localization.UIResources.FileSystemFilterFormat, this.filter);
            }
        }

        public override ParameterSuggestion Clone()
        {
            return new FileSystemParameterSuggestion(this.filter);
        }

        protected override XmlNode ToXmlCore(string name, XmlDocument document)
        {
            XmlNode node = document.CreateElement(name);

            node.Attributes.Append(XmlUtilities.CreateAttribute("type", TypeAttributeValue, document));
            if (this.filter != null)
            {
                node.Attributes.Append(XmlUtilities.CreateAttribute("filter", this.filter, document));
            }

            return node;
        }

        public static new FileSystemParameterSuggestion FromXml(XmlNode node)
        {
            if (node == null)
            {
                throw new ArgumentNullException("node");
            }

            string filter = string.Empty;

            foreach (XmlAttribute attribute in node.Attributes)
            {
                switch (attribute.Name.ToUpperInvariant())
                {
                    case "FILTER":
                        filter = attribute.Value;
                        break;
                    default:
                        break;
                }
            }

            return new FileSystemParameterSuggestion(filter);
        }
    }
}
