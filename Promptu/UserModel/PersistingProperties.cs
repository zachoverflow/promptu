//using System;
//using System.Collections.Generic;
//using System.Text;
//using ZachJohnson.Promptu.Skins;
//using System.Xml;
//using System.ComponentModel;
//using System.Windows.Forms;
//using System.IO;
//using ZachJohnson.Promptu.Collections;

//namespace ZachJohnson.Promptu.UserModel
//{
//    internal class PersistingProperties<TObjectFor>
//    {
//        private Dictionary<string, string> properties;

//        public PersistingProperties()
//            : this(new Dictionary<string,string>())
//        {
//        }

//        public PersistingProperties(Dictionary<string, string> properties)
//        {
//            if (properties == null)
//            {
//                throw new ArgumentNullException("properties");
//            }

//            this.properties = properties;
//        }

//        public void TakeSnapshot(TObjectFor objectFrom)
//        {
//            this.properties.Clear();
//            PropertyDescriptorCollection properties = new PropertiesMiddleMan(objectFrom).GetPersistingProperties();
//            foreach (PropertyDescriptor property in properties)
//            {
//                this.properties.Add(property.Name, property.Converter.ConvertTo(property.GetValue(objectFrom), typeof(string)).ToString());
//            }
//        }

//        public void Restore(TObjectFor objectTo)
//        {
//            PropertyDescriptorCollection properties = new PropertiesMiddleMan(objectTo).GetPersistingProperties();
//            foreach (PropertyDescriptor property in properties)
//            {
//                if (this.properties.ContainsKey(property.Name))
//                {
//                    Type propertyType = property.GetType();
//                    if (property.Converter.CanConvertFrom(typeof(string)))
//                    {
//                        property.SetValue(objectTo, property.Converter.ConvertFromString(this.properties[property.Name]));
//                    }
//                }
//            }
//        }

//        public void LoadDataFromXml(XmlNode node)
//        {
//            if (node == null)
//            {
//                throw new ArgumentNullException("node");
//            }

//            this.properties.Clear();

//            foreach (XmlNode propertyNode in node.ChildNodes)
//            {
//                if (propertyNode.Name == "Property")
//                {
//                    string propertyName = null;
//                    foreach (XmlAttribute attribute in propertyNode.Attributes)
//                    {
//                        if (attribute.Name == "name")
//                        {
//                            propertyName = attribute.Value;
//                        }
//                    }

//                    if (propertyName != null)
//                    {
//                        properties.Add(propertyName, propertyNode.InnerText);
//                    }
//                }
//            }
//        }

//        public XmlNode ToXml(XmlDocument document, string name)
//        {
//            if (name == null)
//            {
//                throw new ArgumentNullException("name");
//            }

//            XmlNode propertiesNode = document.CreateElement(name);
//            //skinNode.Attributes.Append(XmlUtilites.CreateAttribute("name", this.skin.Name, document));
//            foreach (KeyValuePair<string, string> property in this.properties)
//            {
//                XmlNode propertyNode = XmlUtilities.CreateNode("Property", property.Value, document);
//                propertyNode.Attributes.Append(XmlUtilities.CreateAttribute("name", property.Key, document));
//                propertiesNode.AppendChild(propertyNode);
//            }

//            return propertiesNode;
//        }
//    }
//}
