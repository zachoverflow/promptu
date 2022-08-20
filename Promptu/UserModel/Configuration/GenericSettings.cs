using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace ZachJohnson.Promptu.UserModel.Configuration
{
    internal abstract class GenericSettings : SettingsBase
    {
        public GenericSettings()
        {
        }

        public XmlNode ToXml(string nodeName, XmlDocument document)
        {
            if (document == null)
            {
                throw new ArgumentNullException("document");
            }
            else if (nodeName == null)
            {
                throw new ArgumentNullException("nodeName");
            }

            return this.ToXmlCore(nodeName, document);
        }

        protected abstract XmlNode ToXmlCore(string nodeName, XmlDocument document);
    }
}
