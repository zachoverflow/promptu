//using System;
//using System.Collections.Generic;
//using System.Text;
//using ZachJohnson.Promptu.SkinApi;
//using System.Xml;

//namespace ZachJohnson.Promptu.Skins
//{
//    internal class Skin
//    {
//        private string name;
//        private string imagePath;
//        private string creator;
//        private string description;
//        private PromptuSkin instance;
//        private ExternalCodeObject<PromptuSkin> externalObject;

//        public Skin(string name, string imagePath, string creator, string description, ExternalCodeObject<PromptuSkin> externalObject, object instance)
//        {
//            this.name = name;
//            this.imagePath = imagePath;
//            this.creator = creator;
//            this.description = description;
//            this.externalObject = externalObject;
//        }

//        //public string Name
//        //{
//        //    get { return this.name; }
//        //}

//        //public string ImagePath
//        //{
//        //    get { return this.imagePath; }
//        //}

//        //public string Creator
//        //{
//        //    get { return this.creator; }
//        //}

//        //public string Description
//        //{
//        //    get { return this.description; }
//        //}

//        public PromptuSkin GetCore()
//        {
//            if (this.instance == null)
//            {
//                this.instance = this.externalObject.GetNewInstance();
//            }
            
//            return this.instance;
//        }

//        public static Skin LoadFrom(XmlNode node)
//        {
//            if (node == null)
//            {
//                throw new ArgumentNullException("node");
//            }

//            string name = null;
//            bool currentToolkitIsSupported = false;

//            foreach (XmlAttribute attribute in node.Attributes)
//            {
//                switch (attribute.Name.ToUpperInvariant())
//                {
//                    case "SUPPORTEDTOOLKITS":
//                        foreach (string toolkit in attribute.Value.Split(','))
//                        {
//                            if (toolkit.Trim().ToUpperInvariant() == Globals.GuiManager.ToolkitHost.ToolkitName)
//                            {
//                                currentToolkitIsSupported = true;
//                                break;
//                            }
//                        }

//                        break;
//                    case "NAME":
//                        name = attribute.Value;
//                        break;
//                    default:
//                        break;
//                }
//            }

//            if (!currentToolkitIsSupported)
//            {
//                return null;
//            }

//            string assembly = null;
//            string fullyQualifiedTypeName = null;
//            string creator = null;
//            string description = null;
//            string imagePath = null;

//            foreach (XmlNode innerNode in node.ChildNodes)
//            {
//                if (innerNode.Name.ToUpperInvariant() == Globals.GuiManager.ToolkitHost.ToolkitName)
//                {
//                    foreach (XmlNode actualInnerNode in innerNode.ChildNodes)
//                    {
//                        switch (actualInnerNode.Name.ToUpperInvariant())
//                        {
//                            case "ABOUT":
//                                foreach (XmlNode aboutNode in actualInnerNode.ChildNodes)
//                                {
//                                    switch (aboutNode.Name.ToUpperInvariant())
//                                    {
//                                        case "CREATOR":
//                                            creator = aboutNode.InnerText;
//                                            break;
//                                        case "DESCRIPTION":
//                                            description = aboutNode.InnerText;
//                                            break;
//                                        case "IMAGE":
//                                            imagePath = aboutNode.InnerText;
//                                            break;
//                                        default:
//                                            break;
//                                    }
//                                }

//                                break;
//                            case "CODE":
//                                foreach (XmlAttribute attribute in actualInnerNode.Attributes)
//                                {
//                                    switch (attribute.Name.ToUpperInvariant())
//                                    {
//                                        case "ASSEMBLY":
//                                            assembly = attribute.Value;
//                                            break;
//                                        case "CLASS":
//                                            fullyQualifiedTypeName = attribute.Value;
//                                            break;
//                                        default:
//                                            break;
//                                    }
//                                }

//                                break;
//                            default:
//                                break;
//                        }
//                    }
//                }
//            }

//            if (assembly == null || fullyQualifiedTypeName == null)
//            {
//                return null;
//            }

//            if (name == null)
//            {
//                name = Localization.UIResources.NoNameSkin;
//            }

//            return new Skin(name, imagePath, creator, description, new ExternalCodeObject<PromptuSkin>(assembly, fullyQualifiedTypeName));
//        }
//    }
//}
