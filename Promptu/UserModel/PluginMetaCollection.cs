using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using ZachJohnson.Promptu.PluginModel;
using System.IO;
using ZachJohnson.Promptu.Configuration;

namespace ZachJohnson.Promptu.UserModel
{
    internal class PluginMetaCollection : List<string>
    {
        private FileSystemFile metaFile;
        private bool blockSave;

        public PluginMetaCollection(FileSystemFile metaFile)
        {
            this.metaFile = metaFile;
        }

        //internal void Reload()
        //{
        //    bool needToSave = false;
        //    XmlDocument document = new XmlDocument();

        //    if (!this.metaFile.Exists)
        //    {
        //        return;
        //    }

        //    try
        //    {
        //        document.Load(this.metaFile);
        //    }
        //    catch (XmlException)
        //    {
        //        return;
        //    }
        //    catch (IOException)
        //    {
        //        return;
        //    }

        //    Dictionary<string, XmlNode> nodeMap = new Dictionary<string, XmlNode>();

        //    foreach (XmlNode root in document.ChildNodes)
        //    {
        //        if (root.Name.ToUpperInvariant() == "PLUGINS")
        //        {
        //            foreach (XmlNode node in root.ChildNodes)
        //            {
        //                if (node.Name.ToUpperInvariant() != "PLUGIN")
        //                {
        //                    continue;
        //                }

        //                foreach (XmlAttribute attribute in node.Attributes)
        //                {
        //                    if (attribute.Name.ToUpperInvariant() == "ID")
        //                    {
        //                        if (!nodeMap.ContainsKey(attribute.Value))
        //                        {
        //                            nodeMap.Add(attribute.Value, node);
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //    }
        //}

        //internal void FilterInto(PromptuPluginCollection plugins, Action<PromptuPlugin> add)
        //{
        //    foreach (PromptuPlugin plugin in plugins)
        //    {
        //        if (plugin.IsInstalled)
        //        {
        //            add(plugin);
        //        }
        //    }
        //}

        internal void SaveMetadata(PromptuPluginCollection plugins)
        {
            if (this.blockSave)
            {
                return;
            }

            XmlDocument document = new XmlDocument();
            XmlNode root = document.CreateElement("Plugins");
            document.AppendChild(root);

            foreach (string pluginId in this)
            {
                PromptuPlugin plugin = plugins.TryGet(pluginId);
                if (plugin == null)
                {
                    continue;
                }

                XmlNode pluginNode = document.CreateElement("Plugin");
                XmlUtilities.AppendAttribute(pluginNode, "id", plugin.Id);

                if (!plugin.Enabled && plugin.IsCompatible)
                {
                    XmlUtilities.AppendAttribute(pluginNode, "disabled", !plugin.Enabled);
                }

                if (plugin.CheckedCompatibleWith != null)
                {
                    XmlUtilities.AppendAttribute(pluginNode, "checkedCompatibleWith", plugin.CheckedCompatibleWith);
                }

                if (!plugin.IsCompatible)
                {
                    XmlUtilities.AppendAttribute(pluginNode, "compatible", plugin.IsCompatible);
                }

                PromptuPluginEntryPoint entryPoint;

                if (plugin.HasLoaded && ((entryPoint = plugin.EntryPoint) != null))
                {
                    XmlNode propertiesNode = document.CreateElement("Properties");
                    entryPoint.SavingProperties.SerializeTo(propertiesNode);

                    SettingsBase settings = entryPoint.SavingSettings;
                    if (settings != null)
                    {
                        settings.AppendAsXmlOn(pluginNode, "Settings");
                    }

                    pluginNode.AppendChild(propertiesNode);
                }
                else
                {
                    XmlDocument tempDocument = new XmlDocument();
                    string propertyPersistXml = plugin.PropertyPersistXml;

                    if (propertyPersistXml != null)
                    {
                        tempDocument.LoadXml(propertyPersistXml);
                        XmlNode node = tempDocument.ChildNodes[0];
                        XmlNode converted = document.ImportNode(node, true);
                        pluginNode.AppendChild(converted);
                    }

                    string settingPersistXml = plugin.SettingPersistXml;

                    if (settingPersistXml != null)
                    {
                        tempDocument.LoadXml(settingPersistXml);
                        XmlNode node = tempDocument.ChildNodes[0];
                        XmlNode converted = document.ImportNode(node, true);
                        pluginNode.AppendChild(converted);
                    }
                }

                root.AppendChild(pluginNode);
            }

            // TODO check for not saving, resave later?
            document.Save(this.metaFile);
        }

        public void InstallPlugin(PromptuPlugin plugin, PromptuPluginCollection plugins)
        {
            plugin.IsCompatible = true;
            if (InternalGlobals.LastPluginApiChangeVersion.IsAfter(plugin.PromptuVersionCompiledAgainst)
                        || plugin.PromptuVersionCompiledAgainst.IsAfter(InternalGlobals.CurrentPromptuVersion))
            {
                plugin.IsCompatible = ZachJohnson.Promptu.PluginModel.Internals.CompatibilityAnalyzer.PluginAssemblyIsCompatible(plugin.AssemblyPath, plugin.Id);
                plugin.CheckedCompatibleWith = InternalGlobals.LastPluginApiChangeVersion;
            }

            this.Add(plugin.Id);
            plugin.IsInstalled = true;
            plugin.Enabled = true;
            this.SaveMetadata(plugins);
        }

        internal void LoadIds()
        {
            XmlDocument document = new XmlDocument();

            if (!this.metaFile.Exists)
            {
                return;
            }

            try
            {
                document.Load(this.metaFile);
            }
            catch (XmlException)
            {
                return;
            }
            catch (IOException)
            {
                return;
            }

            List<string> ids = new List<string>();

            foreach (XmlNode root in document.ChildNodes)
            {
                if (root.Name.ToUpperInvariant() == "PLUGINS")
                {
                    foreach (XmlNode node in root.ChildNodes)
                    {
                        if (node.Name.ToUpperInvariant() != "PLUGIN")
                        {
                            continue;
                        }

                        foreach (XmlAttribute attribute in node.Attributes)
                        {
                            if (attribute.Name.ToUpperInvariant() == "ID")
                            {
                                if (!ids.Contains(attribute.Value))
                                {
                                    ids.Add(attribute.Value);
                                }
                            }
                        }
                    }
                }
            }

            this.Clear();
            this.AddRange(ids);
        }

        internal void LoadMetadata(PromptuPluginCollection plugins)
        {
            bool needToSave = false;
            XmlDocument document = new XmlDocument();

            if (!this.metaFile.Exists)
            {
                return;
            }

            try
            {
                document.Load(this.metaFile);
            }
            catch (XmlException)
            {
                return;
            }
            catch (IOException)
            {
                return;
            }

            Dictionary<string, XmlNode> nodeMap = new Dictionary<string, XmlNode>();

            foreach (XmlNode root in document.ChildNodes)
            {
                if (root.Name.ToUpperInvariant() == "PLUGINS")
                {
                    foreach (XmlNode node in root.ChildNodes)
                    {
                        if (node.Name.ToUpperInvariant() != "PLUGIN")
                        {
                            continue;
                        }

                        foreach (XmlAttribute attribute in node.Attributes)
                        {
                            if (attribute.Name.ToUpperInvariant() == "ID")
                            {
                                if (!nodeMap.ContainsKey(attribute.Value))
                                {
                                    nodeMap.Add(attribute.Value, node);
                                }
                            }
                        }
                    }
                }
            }

            plugins.DisableAndResetAll();

            List<string> loadedIds = new List<string>();

            foreach (KeyValuePair<string, XmlNode> pluginEntry in nodeMap)
            {
                PromptuPlugin plugin = plugins.TryGet(pluginEntry.Key);

                if (plugin == null)
                {
                    continue;
                }

                loadedIds.Add(pluginEntry.Key);

                bool disabled = false;
                bool isCompatible = true;

                XmlNode correspondingNode;

                if (nodeMap.TryGetValue(plugin.Id, out correspondingNode))
                {
                    foreach (XmlAttribute attribute in correspondingNode.Attributes)
                    {
                        switch (attribute.Name.ToUpperInvariant())
                        {
                            case "COMPATIBLE":
                                isCompatible = Utilities.TryParseBoolean(attribute.Value, isCompatible);
                                //try
                                //{
                                //    isCompatible = Convert.ToBoolean(attribute.Value);
                                //}
                                //catch (FormatException)
                                //{
                                //}

                                break;
                            case "DISABLED":
                                disabled = Utilities.TryParseBoolean(attribute.Value, disabled);
                                //try
                                //{
                                //    disabled = Convert.ToBoolean(attribute.Value);
                                //}
                                //catch (FormatException)
                                //{
                                //}

                                break;
                            default:
                                break;
                        }
                    }

                    if (InternalGlobals.LastPluginApiChangeVersion.IsAfter(plugin.PromptuVersionCompiledAgainst)
                        || plugin.PromptuVersionCompiledAgainst.IsAfter(InternalGlobals.CurrentPromptuVersion))
                    //&& (plugin.CheckedCompatibleWith == null || InternalGlobals.LastPluginApiChangeVersion.IsAfter(plugin.CheckedCompatibleWith))
                    {
                        ReleaseVersion checkedCompatibleWith = null;
                        foreach (XmlAttribute attribute in correspondingNode.Attributes)
                        {
                            switch (attribute.Name.ToUpperInvariant())
                            {
                                case "CHECKEDCOMPATIBLEWITH":
                                    try
                                    {
                                        checkedCompatibleWith = new ReleaseVersion(attribute.Value);
                                    }
                                    catch (FormatException)
                                    {
                                    }

                                    break;
                                default:
                                    break;
                            }
                        }

                        plugin.CheckedCompatibleWith = checkedCompatibleWith;

                        if (plugin.CheckedCompatibleWith == null
                            || InternalGlobals.LastPluginApiChangeVersion.IsAfter(plugin.CheckedCompatibleWith))
                        {
                            isCompatible = ZachJohnson.Promptu.PluginModel.Internals.CompatibilityAnalyzer.PluginAssemblyIsCompatible(plugin.AssemblyPath, plugin.Id);
                            plugin.CheckedCompatibleWith = InternalGlobals.LastPluginApiChangeVersion;
                            needToSave = true;
                        }
                    }

                    foreach (XmlNode childNode in correspondingNode.ChildNodes)
                    {
                        switch (childNode.Name.ToUpperInvariant())
                        {
                            case "PROPERTIES":
                                if (plugin.HasLoaded && plugin.EntryPoint != null)
                                {
                                    plugin.EntryPoint.SavingProperties.LoadSettingsFrom(childNode);
                                }
                                else
                                {
                                    plugin.PropertyPersistXml = childNode.OuterXml;
                                }

                                break;
                            case "SETTINGS":
                                if (plugin.HasLoaded && plugin.EntryPoint != null)
                                {
                                    SettingsBase settings = plugin.EntryPoint.SavingSettings;
                                    if (settings != null)
                                    {
                                        settings.UpdateFrom(childNode);
                                    }
                                }
                                else
                                {
                                    plugin.SettingPersistXml = childNode.OuterXml;
                                }

                                break;
                        }
                    }
                }

                try
                {
                    this.blockSave = true;
                    plugin.IsCompatible = isCompatible;
                    plugin.Enabled = !disabled;
                    plugin.IsInstalled = true;
                }
                finally
                {
                    this.blockSave = false;
                }
            }

            this.Clear();
            this.AddRange(loadedIds);

            if (needToSave)
            {
                this.SaveMetadata(plugins);
            }
        }
    }
}
