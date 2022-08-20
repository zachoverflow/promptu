using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;

namespace ZachJohnson.Promptu
{
    internal class LastModifiedDictionary : Dictionary<string, DateTime>
    {
        private FileSystemFile file;

        public LastModifiedDictionary(FileSystemFile file)
        {
            this.file = file;
        }

        public void Save()
        {
            XmlDocument document = new XmlDocument();
            XmlNode rootNode = document.CreateElement("Items");

            foreach (KeyValuePair<string, DateTime> entry in this)
            {
                XmlNode node = document.CreateElement("Item");
                node.Attributes.Append(XmlUtilities.CreateAttribute("id", entry.Key, document));
                node.Attributes.Append(XmlUtilities.CreateAttribute("lastModified", entry.Value.ToBinary(), document));
                rootNode.AppendChild(node);
            }

            document.AppendChild(rootNode);
            this.file.GetParentDirectory().CreateIfDoesNotExist();
            document.Save(this.file);
        }

        public void Set(string key, DateTime value)
        {
            if (this.ContainsKey(key))
            {
                this.Remove(key);
            }

            this.Add(key, value);
        }

        public void Reload()
        {
            Dictionary<string, DateTime> newValues = new Dictionary<string, DateTime>();

            try
            {
                XmlDocument document = new XmlDocument();
                document.Load(this.file);

                foreach (XmlNode root in document.ChildNodes)
                {
                    if (root.Name.ToUpperInvariant() == "ITEMS")
                    {
                        foreach (XmlNode child in root.ChildNodes)
                        {
                            if (child.Name.ToUpperInvariant() == "ITEM")
                            {
                                string id = null;
                                DateTime? lastModified = null;

                                foreach (XmlAttribute attribute in child.Attributes)
                                {
                                    switch (attribute.Name.ToUpperInvariant())
                                    {
                                        case "ID":
                                            id = attribute.Value;
                                            break;
                                        case "LASTMODIFIED":
                                            try
                                            {
                                                lastModified = DateTime.FromBinary(Convert.ToInt64(attribute.Value));
                                            }
                                            catch (OverflowException)
                                            {
                                            }
                                            catch (FormatException)
                                            {
                                            }

                                            break;
                                        default:
                                            break;
                                    }
                                }

                                if (id != null && lastModified != null && !newValues.ContainsKey(id))
                                {
                                    newValues.Add(id, lastModified.Value);
                                }
                            }
                        }
                    }
                }
            }
            catch (XmlException)
            {
            }
            catch (IOException)
            {
            }

            this.Clear();
            foreach (var entry in newValues)
            {
                this.Add(entry.Key, entry.Value);
            }
        }
    }
}
