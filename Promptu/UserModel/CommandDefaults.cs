using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace ZachJohnson.Promptu.UserModel
{
    internal class CommandDefaults
    {
        private bool saveParameterHistory;

        public CommandDefaults(bool saveParameterHistory)
        {
            this.saveParameterHistory = saveParameterHistory;
        }

        public bool SaveParameterHistory
        {
            get { return this.saveParameterHistory; }
        }

        public static CommandDefaults FromXml(XmlNode node, CommandDefaults defaultValues)
        {
            return FromXml(node, defaultValues.SaveParameterHistory);
        }

        public static CommandDefaults FromXml(XmlNode node, bool defaultSaveParameterHistory)
        {
            if (node == null)
            {
                throw new ArgumentNullException("node");
            }

            bool saveParameterHistory = defaultSaveParameterHistory;

            foreach (XmlAttribute attribute in node.Attributes)
            {
                switch (attribute.Name.ToUpperInvariant())
                {
                    case "SAVEPARAMETERHISTORY":
                        saveParameterHistory = Utilities.TryParseBoolean(attribute.Value, saveParameterHistory);
                        //try
                        //{
                        //    saveParameterHistory = Convert.ToBoolean(attribute.Value);
                        //}
                        //catch (FormatException)
                        //{
                        //}

                        break;
                    default:
                        break;
                }
            }

            return new CommandDefaults(saveParameterHistory);
        }

        public void ToXml(XmlNode node, XmlDocument document)
        {
            if (node == null)
            {
                throw new ArgumentNullException("node");
            }
            else if (document == null)
            {
                throw new ArgumentNullException("document");
            }

            node.Attributes.Append(XmlUtilities.CreateAttribute("saveParameterHistory", this.saveParameterHistory, document));
        }
    }
}
