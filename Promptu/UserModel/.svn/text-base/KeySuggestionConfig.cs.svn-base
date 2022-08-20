using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace ZachJohnson.Promptu.UserModel
{
    class KeySuggestionConfig
    {
        private bool eatCharacter;
        private bool acceptsSuggestion;

        public KeySuggestionConfig(bool acceptsSuggestion, bool eatCharacter)
        {
            this.acceptsSuggestion = acceptsSuggestion;
            this.eatCharacter = eatCharacter;
        }

        public bool EatCharacter
        {
            get { return this.eatCharacter; }
        }

        public bool AcceptsSuggestion
        {
            get { return this.acceptsSuggestion; }
        }

        public static KeySuggestionConfig FromXml(XmlNode node, KeySuggestionConfig defaultValues)
        {
            return FromXml(node, defaultValues.AcceptsSuggestion, defaultValues.EatCharacter);
        }

        public static KeySuggestionConfig FromXml(XmlNode node, bool defaultEatCharacter, bool defaultAcceptsSuggestion)
        {
            if (node == null)
            {
                throw new ArgumentNullException("node");
            }

            bool eatCharacter = defaultEatCharacter;
            bool acceptsSuggestion = defaultAcceptsSuggestion;

            foreach (XmlAttribute attribute in node.Attributes)
            {
                switch (attribute.Name.ToUpperInvariant())
                {
                    case "ACCEPTS":
                        acceptsSuggestion = Utilities.TryParseBoolean(attribute.Value, acceptsSuggestion);
                        //try
                        //{
                        //    acceptsSuggestion = Convert.ToBoolean(attribute.Value);
                        //}
                        //catch (FormatException)
                        //{
                        //}
                        
                        break;
                    case "EATCHARACTER":
                        eatCharacter = Utilities.TryParseBoolean(attribute.Value, acceptsSuggestion);
                        //try
                        //{
                        //    eatCharacter = Convert.ToBoolean(attribute.Value);
                        //}
                        //catch (FormatException)
                        //{
                        //}

                        break;
                    default:
                        break;
                }
            }

            return new KeySuggestionConfig(acceptsSuggestion, eatCharacter);
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

            node.Attributes.Append(XmlUtilities.CreateAttribute("accepts", this.acceptsSuggestion, document));
            node.Attributes.Append(XmlUtilities.CreateAttribute("eatCharacter", this.eatCharacter, document));
        }
    }
}
