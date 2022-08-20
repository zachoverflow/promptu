using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace ZachJohnson.Promptu.UserModel
{
    internal class ValueListParameterSuggestion : ParameterSuggestion
    {
        public const string TypeAttributeValue = "ValueList";
        private string valueListName;

        public ValueListParameterSuggestion(string valueListName)
        {
            if (valueListName == null)
            {
                throw new ArgumentNullException("valueListName");
            }

            this.valueListName = valueListName;
        }

        public string ValueListName
        {
            get { return this.valueListName; }
        }

        public override ParameterSuggestion Clone()
        {
            return new ValueListParameterSuggestion(this.valueListName);
        }

        protected override System.Xml.XmlNode ToXmlCore(string name, System.Xml.XmlDocument document)
        {
            XmlNode node = document.CreateElement(name);

            node.Attributes.Append(XmlUtilities.CreateAttribute("type", TypeAttributeValue, document));
            node.Attributes.Append(XmlUtilities.CreateAttribute("valueListName", this.valueListName, document));

            return node;
        }

        public static new ValueListParameterSuggestion FromXml(XmlNode node)
        {
            if (node == null)
            {
                throw new ArgumentNullException("node");
            }

            string valueListName = null;

            foreach (XmlAttribute attribute in node.Attributes)
            {
                switch (attribute.Name.ToUpperInvariant())
                {
                    case "VALUELISTNAME":
                        valueListName = attribute.Value;
                        break;
                    default:
                        break;
                }
            }

            if (valueListName == null)
            {
                throw new LoadException("Missing 'valueListName' attribute.");
            }

            return new ValueListParameterSuggestion(valueListName);
        }
    }
}
