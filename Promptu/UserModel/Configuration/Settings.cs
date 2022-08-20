using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace ZachJohnson.Promptu.UserModel.Configuration
{
    internal abstract class Settings : SettingsBase
    {
        public Settings()
        {
        }

        public XmlNode ToXml(XmlDocument document)
        {
            if (document == null)
            {
                throw new ArgumentNullException("document");
            }

            return this.ToXmlCore(document);
        }

        protected abstract XmlNode ToXmlCore(XmlDocument document);
    }
}
