//using System;
//using System.Collections.Generic;
//using System.Text;
//using ZachJohnson.Promptu.Skins;
//using System.Xml;
//using System.ComponentModel;
//using System.Windows.Forms;
//using System.IO;
//using ZachJohnson.Promptu.Collections;
//using ZachJohnson.Promptu.SkinApi;

//namespace ZachJohnson.Promptu.UserModel
//{
//    internal class SkinData
//    {
//        private Skin skin;
//        private PersistingProperties<IPrompt> promptProperties;
//        private PersistingProperties<ISuggester> suggesterProperties;
//        private PersistingProperties<ToolTipSettings> toolTipSettings;
//        private bool restored;

//        public SkinData(Skin skin)
//            : this(skin, new PersistingProperties<IPrompt>(), new PersistingProperties<ISuggester>(), new PersistingProperties<ToolTipSettings>())
//        {
//        }

//        public SkinData(
//            Skin skin,
//            PersistingProperties<IPrompt> promptProperties,
//            PersistingProperties<ISuggester> suggesterProperties,
//            PersistingProperties<ToolTipSettings> toolTipSettings)
//        {
//            if (skin == null)
//            {
//                throw new ArgumentNullException("skin");
//            }
//            else if (promptProperties == null)
//            {
//                throw new ArgumentNullException("promptProperties");
//            }
//            else if (suggesterProperties == null)
//            {
//                throw new ArgumentNullException("suggesterProperties");
//            }
//            else if (toolTipSettings == null)
//            {
//                throw new ArgumentNullException("toolTipSettings");
//            }

//            this.skin = skin;
//            this.promptProperties = promptProperties;
//            this.suggesterProperties = suggesterProperties;
//            this.toolTipSettings = toolTipSettings;
//        }

//        public Skin Skin
//        {
//            get { return this.skin; }
//        }

//        public void TakeSnapshot()
//        {
//            //this.properties.Clear();
//            this.promptProperties.TakeSnapshot(this.skin.GetInstance());
//            this.suggesterProperties.TakeSnapshot(this.skin.GetSuggester());
//            this.toolTipSettings.TakeSnapshot(this.skin.ToolTipSettings);
//            //PropertyDescriptorCollection properties = new PropertiesMiddleMan(skin).GetPersistingProperties();
//            //foreach (PropertyDescriptor property in properties)
//            //{
//            //    this.properties.Add(property.Name, property.Converter.ConvertTo(property.GetValue(skin.GetInstance()), typeof(string)).ToString());
//            //}

//            this.restored = false;
//        }

//        public void Restore()
//        {
//            if (!this.restored)
//            {
//                this.promptProperties.Restore(this.skin.GetInstance());
//                this.suggesterProperties.Restore(this.skin.GetSuggester());
//                this.toolTipSettings.Restore(this.skin.ToolTipSettings);
//                //PropertyDescriptorCollection properties = new PropertiesMiddleMan(skin).GetPersistingProperties();
//                //foreach (PropertyDescriptor property in properties)
//                //{
//                //    if (this.properties.ContainsKey(property.Name))
//                //    {
//                //        Type propertyType = property.GetType();
//                //        if (property.Converter.CanConvertFrom(typeof(string)))
//                //        {
//                //            property.SetValue(skin.GetInstance(), property.Converter.ConvertFromString(this.properties[property.Name]));
//                //        }
//                //    }
//                //}

//                this.restored = true;
//            }
//        }

//        public void LoadDataFromXml(XmlNode node)
//        {
//            if (node == null)
//            {
//                throw new ArgumentNullException("node");
//            }

//            //this.properties.Clear();

//            foreach (XmlNode innerNode in node.ChildNodes)
//            {
//                switch (innerNode.Name.ToUpperInvariant())
//                {
//                    case "PROMPT":
//                        this.promptProperties.LoadDataFromXml(innerNode);
//                        break;
//                    case "SUGGESTER":
//                        this.suggesterProperties.LoadDataFromXml(innerNode);
//                        break;
//                    case "TOOLTIPS":
//                        this.toolTipSettings.LoadDataFromXml(innerNode);
//                        break;
//                    default:
//                        break;
//                }
//            }
//        }

//        public XmlNode ToXml(XmlDocument document)
//        {
//            XmlNode skinNode = document.CreateElement("Skin");
//            skinNode.AppendChild(this.promptProperties.ToXml(document, "Prompt"));
//            skinNode.AppendChild(this.suggesterProperties.ToXml(document, "Suggester"));
//            skinNode.AppendChild(this.toolTipSettings.ToXml(document, "ToolTips"));
//            //foreach (KeyValuePair<string, string> property in this.properties)
//            //{
//            //    XmlNode propertyNode = XmlUtilites.CreateNode("Property", property.Value, document);
//            //    propertyNode.Attributes.Append(XmlUtilites.CreateAttribute("name", property.Key, document));
//            //    skinNode.AppendChild(propertyNode);
//            //}


//            return skinNode;
//        }
//    }
//}
