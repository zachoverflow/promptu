using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using ZachJohnson.Promptu.Collections;

namespace ZachJohnson.Promptu.UserModel.Collections
{
    internal class FunctionParameterCollection : List<FunctionParameter>, IHasCount
    {
        public FunctionParameterCollection()
        {
        }

        public FunctionParameterCollection Clone()
        {
            FunctionParameterCollection clone = new FunctionParameterCollection();
            foreach (FunctionParameter parameter in this)
            {
                clone.Add(parameter.Clone());
            }

            return clone;
        }

        public XmlNode ToXml(string name, XmlDocument document)
        {
            if (name == null)
            {
                throw new ArgumentNullException("name");
            }
            else if (document == null)
            {
                throw new ArgumentNullException("document");
            }

            XmlNode node = document.CreateElement(name);

            foreach (FunctionParameter parameter in this)
            {
                node.AppendChild(parameter.ToXml("Parameter", document));
            }

            return node;
        }

        public static FunctionParameterCollection FromXml(XmlNode node)
        {
            if (node == null)
            {
                throw new ArgumentNullException("node");
            }

            FunctionParameterCollection collection = new FunctionParameterCollection();

            foreach (XmlNode innerNode in node.ChildNodes)
            {
                if (innerNode.Name.ToUpperInvariant() == "PARAMETER")
                {
                    collection.Add(FunctionParameter.FromXml(innerNode));   
                }
            }

            return collection;
        }
    }
}
