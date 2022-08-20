//-----------------------------------------------------------------------
// <copyright file="SkinsSettings.cs" company="ZachJohnson">
//     Copyright (c) Zach Johnson. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace ZachJohnson.Promptu.SkinApi
{
    using System.IO;
    using System.Xml;
    using ZachJohnson.Promptu.PluginModel;

    internal class SkinsSettings
    {
        private FileSystemFile settingFile;

        public SkinsSettings(FileSystemFile settingFile)
        {
            this.settingFile = settingFile;
        }

        public void TrySerialize(PromptuSkinInstance skin, string skinId)
        {
            XmlDocument document = new XmlDocument();

            try
            {
                document.Load(this.settingFile);
            }
            catch (IOException)
            {
            }
            catch (XmlException)
            {
            }

            XmlNode skinSettingsRoot = null;

            foreach (XmlNode rootNode in document.ChildNodes)
            {
                if (rootNode.Name.ToUpperInvariant() == "SKINSSETTINGS")
                {
                    skinSettingsRoot = rootNode;
                }
            }

            if (skinSettingsRoot == null)
            {
                document.RemoveAll();
                skinSettingsRoot = document.CreateElement("SkinsSettings");
                document.AppendChild(skinSettingsRoot);
            }

            XmlNode thisSkinNode = null;
            foreach (XmlNode skinNode in skinSettingsRoot.ChildNodes)
            {
                if (skinNode.Name.ToUpperInvariant() == "SKIN")
                {
                    bool isForThisSkin = false;

                    foreach (XmlAttribute attribute in skinNode.Attributes)
                    {
                        switch (attribute.Name.ToUpperInvariant())
                        {
                            case "ID":
                                if (attribute.Value == skinId)
                                {
                                    isForThisSkin = true;
                                }

                                break;
                            default:
                                break;
                        }

                        if (isForThisSkin)
                        {
                            break;
                        }
                    }

                    if (!isForThisSkin)
                    {
                        continue;
                    }

                    thisSkinNode = skinNode;
                    break;
                }
            }

            if (thisSkinNode == null)
            {
                thisSkinNode = document.CreateElement("Skin");
                thisSkinNode.Attributes.Append(XmlUtilities.CreateAttribute("id", skinId, document));
                skinSettingsRoot.AppendChild(thisSkinNode);
            }

            XmlNode promptNode = null;
            XmlNode suggestionProviderNode = null;
            XmlNode tooltipsNode = null;

            foreach (XmlNode objectNode in thisSkinNode.ChildNodes)
            {
                switch (objectNode.Name.ToUpperInvariant())
                {
                    case "PROMPT":
                        promptNode = objectNode;
                        break;
                    case "SUGGESTIONPROVIDER":
                        suggestionProviderNode = objectNode;
                        break;
                    case "TOOLTIPS":
                        tooltipsNode = objectNode;
                        break;
                    default:
                        break;
                }
            }

            SerializeSettingGroup(
                ref promptNode,
                "Prompt",
                skin.Prompt.SavingProperties,
                thisSkinNode);

            SerializeSettingGroup(
                ref suggestionProviderNode,
                "SuggestionProvider",
                skin.SuggestionProvider.SavingProperties,
                thisSkinNode);

            SerializeSettingGroup(
                ref tooltipsNode,
                "Tooltips",
                skin.InformationBoxPropertiesAndOptions.Properties,
                thisSkinNode);
            
            document.Save(this.settingFile);
        }

        public void TryRehydrate(PromptuSkinInstance skin, string skinId)
        {
            XmlDocument document = new XmlDocument();
            try
            {
                document.Load(this.settingFile);
            }
            catch (IOException)
            {
                return;
            }
            catch (XmlException)
            {
                return;
            }

            foreach (XmlNode root in document)
            {
                if (root.Name.ToUpperInvariant() == "SKINSSETTINGS")
                {
                    foreach (XmlNode skinNode in root.ChildNodes)
                    {
                        if (skinNode.Name.ToUpperInvariant() == "SKIN")
                        {
                            bool isForThisSkin = false;

                            foreach (XmlAttribute attribute in skinNode.Attributes)
                            {
                                switch (attribute.Name.ToUpperInvariant())
                                {
                                    case "ID":
                                        if (attribute.Value == skinId)
                                        {
                                            isForThisSkin = true;
                                        }

                                        break;
                                    default:
                                        break;
                                }

                                if (isForThisSkin)
                                {
                                    break;
                                }
                            }

                            if (!isForThisSkin)
                            {
                                continue;
                            }

                            foreach (XmlNode objectNode in skinNode.ChildNodes)
                            {
                                ObjectPropertyCollection properties = null;

                                switch (objectNode.Name.ToUpperInvariant())
                                {
                                    case "PROMPT":
                                        properties = skin.Prompt.SavingProperties;
                                        break;
                                    case "SUGGESTIONPROVIDER":
                                        properties = skin.SuggestionProvider.SavingProperties;
                                        break;
                                    case "TOOLTIPS":
                                        properties = skin.InformationBoxPropertiesAndOptions.Properties;
                                        break;
                                    default:
                                        break;
                                }

                                if (properties != null)
                                {
                                    properties.LoadSettingsFrom(objectNode);
                                }
                            }
                        }
                    }
                }
            }
        }

        private static void SerializeSettingGroup(ref XmlNode groupNode, string groupName, ObjectPropertyCollection settings, XmlNode parentNode)
        {
            if (groupNode == null)
            {
                groupNode = parentNode.OwnerDocument.CreateElement(groupName);
                parentNode.AppendChild(groupNode);
            }
            else
            {
                groupNode.RemoveAll();
            }

            if (settings != null)
            {
                settings.SerializeTo(groupNode);
            }
        }
    }
}
