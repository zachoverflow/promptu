using System;
using System.Collections.Generic;
using System.Text;
using ZachJohnson.Promptu.Collections;
using System.Xml;

namespace ZachJohnson.Promptu.UserModel.Collections
{
    internal class CommandParameterMetaInfoCollection : List<CommandParameterMetaInfo>, IHasCount
    {
        public CommandParameterMetaInfoCollection()
        {
        }

        public CommandParameterMetaInfoCollection Clone()
        {
            CommandParameterMetaInfoCollection clone = new CommandParameterMetaInfoCollection();
            foreach (CommandParameterMetaInfo item in this)
            {
                clone.Add(item.Clone());
            }

            return clone;
        }

        public string GetDescriptionFor(int parameterNumber)
        {
            CommandParameterMetaInfo match = this.Find(parameterNumber, new Predicate<CommandParameterMetaInfo>(this.HasValidDescription));
            if (match != null)
            {
                return match.Description;
            }

            return null;
        }

        public CommandParameterMetaInfo GetItemContainingParameterSuggestionFor(int parameterNumber)
        {
            CommandParameterMetaInfo match = this.Find(parameterNumber, new Predicate<CommandParameterMetaInfo>(this.HasMoreThanDefaultSuggestion));
            if (match == null)
            {
                match = this.Find(parameterNumber, null);
            }

            return match;
        }

        private bool HasValidDescription(CommandParameterMetaInfo item)
        {
            return !string.IsNullOrEmpty(item.Description);
        }

        private bool HasMoreThanDefaultSuggestion(CommandParameterMetaInfo item)
        {
            return item.ParameterSuggestion != null;
        }

        public CommandParameterMetaInfo Find(int parameterNumber, Predicate<CommandParameterMetaInfo> match)
        {
            foreach (CommandParameterMetaInfo item in this)
            {
                if (item.Encompasses(parameterNumber) && (match == null || match(item)))
                {
                    return item;
                }
            }

            return null;
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

            foreach (CommandParameterMetaInfo parameter in this)
            {
                node.AppendChild(parameter.ToXml("Info", document));
            }

            return node;
        }

        public static CommandParameterMetaInfoCollection FromXml(XmlNode node)
        {
            if (node == null)
            {
                throw new ArgumentNullException("node");
            }

            CommandParameterMetaInfoCollection collection = new CommandParameterMetaInfoCollection();

            foreach (XmlNode innerNode in node.ChildNodes)
            {
                if (innerNode.Name.ToUpperInvariant() == "INFO")
                {
                    try
                    {
                        collection.Add(CommandParameterMetaInfo.FromXml(innerNode));
                    }
                    catch (LoadException)
                    {
                    }
                }
            }

            return collection;
        }
    }
}
