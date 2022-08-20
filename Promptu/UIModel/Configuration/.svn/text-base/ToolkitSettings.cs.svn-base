//-----------------------------------------------------------------------
// <copyright file="ToolkitSettings.cs" company="ZachJohnson">
//     Copyright (c) Zach Johnson. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace ZachJohnson.Promptu.UIModel.Configuration
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using ZachJohnson.Promptu.UIModel.Interfaces;
    using System.Xml;
    using ZachJohnson.Promptu.Configuration;

    internal abstract class ToolkitSettings : SettingsBase
    {
        private ObjectSettings<IAssemblyReferenceEditor> assemblyReferenceEditorSettings;
        private ObjectSettings<IFunctionEditor> functionEditorSettings;
        private ObjectSettings<IValueListEditor> valueListEditorSettings;
        private ObjectSettings<IValueListSelectorWindow> valueListSelectorSettings;
        private ObjectSettings<IFunctionInvocationEditor> functionInvocationEditorSettings;
        private ObjectSettings<IFileSystemParameterSuggestionEditor> fileSystemParameterSuggestionEditorSettings;
        private ObjectSettings<IFunctionViewer> functionViewerSettings;
        private ObjectSettings<ICommandEditor> commandEditorSettings;
        //private ObjectSettings<ICollisionResolvingDialog> collisionResolvingDialogSettings;
        private ObjectSettings<ISetupPanel> commandSetupPanelSettings;
        private ObjectSettings<ISetupPanel> valueListSetupPanelSettings;
        private ObjectSettings<ISetupPanel> functionSetupPanelSettings;
        private ObjectSettings<ISetupPanel> assemblyReferenceSetupPanelSettings;
        private ObjectSettings<ISetupDialog> setupDialogSettings;
        private ObjectSettings<IProfileTabPanel> profileTabSettings;
        private ObjectSettings<IOptionsDialog> skinOptionsDialogSettings;
        private ObjectSettings<IOptionsDialog> pluginOptionsDialogSettings;
        //private ObjectSettings<IProfileTabPanel> profileTabPanelSettings;

        public ToolkitSettings(
            ObjectSettings<IAssemblyReferenceEditor> assemblyReferenceEditorSettings,
            ObjectSettings<IFunctionEditor> functionEditorSettings,
            ObjectSettings<IValueListEditor> valueListEditorSettings,
            ObjectSettings<IValueListSelectorWindow> valueListSelectorSettings,
            ObjectSettings<IFunctionInvocationEditor> functionInvocationEditorSettings,
            ObjectSettings<IFileSystemParameterSuggestionEditor> fileSystemParameterSuggestionEditorSettings,
            ObjectSettings<IFunctionViewer> functionViewerSettings,
            ObjectSettings<ICommandEditor> commandEditorSettings,
            //ObjectSettings<ICollisionResolvingDialog> collisionResolvingDialogSettings,
            ObjectSettings<ISetupPanel> commandSetupPanelSettings,
            ObjectSettings<ISetupPanel> valueListSetupPanelSettings,
            ObjectSettings<ISetupPanel> functionSetupPanelSettings,
            ObjectSettings<ISetupPanel> assemblyReferenceSetupPanelSettings,
            ObjectSettings<ISetupDialog> setupDialogSettings,
            ObjectSettings<IProfileTabPanel> profileTabSettings,
            ObjectSettings<IOptionsDialog> skinOptionsDialogSettings,
            ObjectSettings<IOptionsDialog> pluginOptionsDialogSettings)
        {
            if (assemblyReferenceEditorSettings == null)
            {
                throw new ArgumentNullException("assemblyReferenceEditorSettings");
            }
            else if (functionEditorSettings == null)
            {
                throw new ArgumentNullException("functionEditorSettings");
            }
            else if (valueListEditorSettings == null)
            {
                throw new ArgumentNullException("valueListEditorSettings");
            }
            else if (valueListSelectorSettings == null)
            {
                throw new ArgumentNullException("valueListSelectorSettings");
            }
            else if (functionInvocationEditorSettings == null)
            {
                throw new ArgumentNullException("functionInvocationEditorSettings");
            }
            else if (fileSystemParameterSuggestionEditorSettings == null)
            {
                throw new ArgumentNullException("fileSystemParameterSuggestionEditorSettings");
            }
            else if (functionViewerSettings == null)
            {
                throw new ArgumentNullException("functionViewerSettings");
            }
            else if (commandEditorSettings == null)
            {
                throw new ArgumentNullException("commandEditorSettings");
            }
            //else if (collisionResolvingDialogSettings == null)
            //{
            //    throw new ArgumentNullException("collisionResolvingDialogSettings");
            //}
            else if (commandSetupPanelSettings == null)
            {
                throw new ArgumentNullException("commandSetupPanelSettings");
            }
            else if (valueListSetupPanelSettings == null)
            {
                throw new ArgumentNullException("valueListSetupPanelSettings");
            }
            else if (functionSetupPanelSettings == null)
            {
                throw new ArgumentNullException("functionSetupPanelSettings");
            }
            else if (assemblyReferenceSetupPanelSettings == null)
            {
                throw new ArgumentNullException("assemblyReferenceSetupPanelSettings");
            }
            else if (setupDialogSettings == null)
            {
                throw new ArgumentNullException("setupDialogSettings");
            }
            else if (profileTabSettings == null)
            {
                throw new ArgumentNullException("profileTabSettings");
            }
            else if (skinOptionsDialogSettings == null)
            {
                throw new ArgumentNullException("skinOptionsDialogSettings");
            }
            else if (pluginOptionsDialogSettings == null)
            {
                throw new ArgumentNullException("pluginOptionsDialogSettings");
            }

            this.assemblyReferenceEditorSettings = assemblyReferenceEditorSettings;
            this.functionEditorSettings = functionEditorSettings;
            this.valueListEditorSettings = valueListEditorSettings;
            this.valueListSelectorSettings = valueListSelectorSettings;
            this.functionInvocationEditorSettings = functionInvocationEditorSettings;
            this.fileSystemParameterSuggestionEditorSettings = fileSystemParameterSuggestionEditorSettings;
            this.functionViewerSettings = functionViewerSettings;
            this.commandEditorSettings = commandEditorSettings;
            //this.collisionResolvingDialogSettings = collisionResolvingDialogSettings;
            this.commandSetupPanelSettings = commandSetupPanelSettings;
            this.valueListSetupPanelSettings = valueListSetupPanelSettings;
            this.functionSetupPanelSettings = functionSetupPanelSettings;
            this.assemblyReferenceSetupPanelSettings = assemblyReferenceSetupPanelSettings;
            this.setupDialogSettings = setupDialogSettings;
            this.profileTabSettings = profileTabSettings;
            this.skinOptionsDialogSettings = skinOptionsDialogSettings;
            this.pluginOptionsDialogSettings = pluginOptionsDialogSettings;
            
            this.Register(this.assemblyReferenceEditorSettings);
            this.Register(this.functionEditorSettings);
            this.Register(this.valueListEditorSettings);
            this.Register(this.valueListSelectorSettings);
            this.Register(this.functionInvocationEditorSettings);
            this.Register(this.fileSystemParameterSuggestionEditorSettings);
            this.Register(this.functionViewerSettings);
            this.Register(this.commandEditorSettings);
            //this.Register(this.collisionResolvingDialogSettings);
            this.Register(this.commandSetupPanelSettings);
            this.Register(this.valueListSetupPanelSettings);
            this.Register(this.functionSetupPanelSettings);
            this.Register(this.assemblyReferenceSetupPanelSettings);
            this.Register(this.setupDialogSettings);
            this.Register(this.profileTabSettings);
            this.Register(this.skinOptionsDialogSettings);
            this.Register(this.pluginOptionsDialogSettings);
        }

        public ObjectSettings<IAssemblyReferenceEditor> AssemblyReferenceEditorSettings
        {
            get { return this.assemblyReferenceEditorSettings; }
        }

        public ObjectSettings<IFunctionEditor> FunctionEditorSettings
        {
            get { return this.functionEditorSettings; }
        }

        public ObjectSettings<IValueListEditor> ValueListEditorSettings
        {
            get { return this.valueListEditorSettings; }
        }

        public ObjectSettings<IValueListSelectorWindow> ValueListSelectorSettings
        {
            get { return this.valueListSelectorSettings; }
        }

        public ObjectSettings<IFunctionInvocationEditor> FunctionInvocationEditorSettings
        {
            get { return this.functionInvocationEditorSettings; }
        }

        public ObjectSettings<IFileSystemParameterSuggestionEditor> FileSystemParameterSuggestionEditorSettings
        {
            get { return this.fileSystemParameterSuggestionEditorSettings; }
        }

        public ObjectSettings<IFunctionViewer> FunctionViewerSettings
        {
            get { return this.functionViewerSettings; }
        }

        public ObjectSettings<ICommandEditor> CommandEditorSettings
        {
            get { return this.commandEditorSettings; }
        }

        //public ObjectSettings<ICollisionResolvingDialog> CollisionResolvingDialogSettings
        //{
        //    get { return this.collisionResolvingDialogSettings; }
        //}

        public ObjectSettings<ISetupPanel> CommandSetupPanelSettings
        {
            get { return this.commandSetupPanelSettings; }
        }

        public ObjectSettings<ISetupPanel> ValueListSetupPanelSettings
        {
            get { return this.valueListSetupPanelSettings; }
        }

        public ObjectSettings<ISetupPanel> FunctionSetupPanelSettings
        {
            get { return this.functionSetupPanelSettings; }
        }

        public ObjectSettings<ISetupPanel> AssemblyReferenceSetupPanelSettings
        {
            get { return this.assemblyReferenceSetupPanelSettings; }
        }

        public ObjectSettings<ISetupDialog> SetupDialogSettings
        {
            get { return this.setupDialogSettings; }
        }

        public ObjectSettings<IProfileTabPanel> ProfileTabSettings
        {
            get { return this.profileTabSettings; }
        }

        public ObjectSettings<IOptionsDialog> SkinOptionsDialogSettings
        {
            get { return this.skinOptionsDialogSettings; }
        }

        public ObjectSettings<IOptionsDialog> PluginOptionsDialogSettings
        {
            get { return this.pluginOptionsDialogSettings; }
        }

        protected override void ToXmlCore(XmlNode node)
        {
            this.assemblyReferenceEditorSettings.AppendAsXmlOn(node, "AssemblyReferenceEditor");
            this.functionEditorSettings.AppendAsXmlOn(node, "FunctionEditor");
            this.valueListEditorSettings.AppendAsXmlOn(node, "ValueListEditor");
            this.valueListSelectorSettings.AppendAsXmlOn(node, "ValueListSelector");
            this.functionInvocationEditorSettings.AppendAsXmlOn(node, "FunctionInvocationEditor");
            this.fileSystemParameterSuggestionEditorSettings.AppendAsXmlOn(node, "FileSystemParameterSuggestionEditor");
            this.functionViewerSettings.AppendAsXmlOn(node, "FunctionViewer");
            this.commandEditorSettings.AppendAsXmlOn(node, "CommandEditor");
            //this.collisionResolvingDialogSettings.AppendAsXmlOn(node, "CollisionResolvingDialog");
            this.commandSetupPanelSettings.AppendAsXmlOn(node, "CommandSetupPanel");
            this.valueListSetupPanelSettings.AppendAsXmlOn(node, "ValueListSetupPanel");
            this.functionSetupPanelSettings.AppendAsXmlOn(node, "FunctionSetupPanel");
            this.assemblyReferenceSetupPanelSettings.AppendAsXmlOn(node, "AssemblyReferenceSetupPanel");
            this.setupDialogSettings.AppendAsXmlOn(node, "SetupDialog");
            this.profileTabSettings.AppendAsXmlOn(node, "ProfileTab");
            this.skinOptionsDialogSettings.AppendAsXmlOn(node, "SkinOptionsDialog");
            this.pluginOptionsDialogSettings.AppendAsXmlOn(node, "PluginOptionsDialog");
        }

        protected override void UpdateFromCore(XmlNode node)
        {
            foreach (XmlNode innerNode in node.ChildNodes)
            {
                switch (innerNode.Name.ToUpperInvariant())
                {
                    case "ASSEMBLYREFERENCEEDITOR":
                        this.AssemblyReferenceEditorSettings.UpdateFrom(innerNode);
                        break;
                    case "FUNCTIONEDITOR":
                        this.functionEditorSettings.UpdateFrom(innerNode);
                        break;
                    case "VALUELISTEDITOR":
                        this.valueListEditorSettings.UpdateFrom(innerNode);
                        break;
                    case "VALUELISTSELECTOR":
                        this.valueListSelectorSettings.UpdateFrom(innerNode);
                        break;
                    case "FUNCTIONINVOCATIONEDITOR":
                        this.functionInvocationEditorSettings.UpdateFrom(innerNode);
                        break;
                    case "FILESYSTEMPARAMETERSUGGESTIONEDITOR":
                        this.fileSystemParameterSuggestionEditorSettings.UpdateFrom(innerNode);
                        break;
                    case "FUNCTIONVIEWER":
                        this.functionViewerSettings.UpdateFrom(innerNode);
                        break;
                    case "COMMANDEDITOR":
                        this.commandEditorSettings.UpdateFrom(innerNode);
                        break;
                    //case "COLLISIONRESOLVINGDIALOG":
                    //    this.collisionResolvingDialogSettings.UpdateFrom(innerNode);
                    //    break;
                    case "COMMANDSETUPPANEL":
                        this.commandSetupPanelSettings.UpdateFrom(innerNode);
                        break;
                    case "VALUELISTSETUPPANEL":
                        this.valueListSetupPanelSettings.UpdateFrom(innerNode);
                        break;
                    case "FUNCTIONSETUPPANEL":
                        this.functionSetupPanelSettings.UpdateFrom(innerNode);
                        break;
                    case "ASSEMBLYREFERENCESETUPPANEL":
                        this.assemblyReferenceSetupPanelSettings.UpdateFrom(innerNode);
                        break;
                    case "SETUPDIALOG":
                        this.setupDialogSettings.UpdateFrom(innerNode);
                        break;
                    case "PROFILETAB":
                        this.profileTabSettings.UpdateFrom(innerNode);
                        break;
                    case "SKINOPTIONSDIALOG":
                        this.skinOptionsDialogSettings.UpdateFrom(innerNode);
                        break;
                    case "PLUGINOPTIONSDIALOG":
                        this.pluginOptionsDialogSettings.UpdateFrom(innerNode);
                        break;
                    default:
                        break;
                }
            }
        }

        //public static UISettings FromXml(XmlNode node)
        //{
        //    if (node == null)
        //    {
        //        throw new ArgumentNullException("node");
        //    }

        //    CommandEditorSettings commandEditorSettings = null;
        //    FunctionEditorSettings functionEditorSettings = null;
        //    ValueListEditorSettings valueListEditorSettings = null;
        //    WindowSettings<Form> assemblyReferenceEditorSettings = null;
        //    SetupDialogSettings setupDialogSettings = null;
        //    SetupPanelSettings commandSetupPanelSettings = null;
        //    SetupPanelSettings functionSetupPanelSettings = null;
        //    SetupPanelSettings assemblyReferenceSetupPanelSettings = null;
        //    SetupPanelSettings valueListSetupPanelSettings = null;
        //    WindowSettings<Form> collisionResolvingDialogSettings = null;
        //    FunctionViewerSettings functionViewerSettings = null;
        //    FunctionInvocationEditorSettings functionInvocationEditorSettings = null;
        //    ValueListSelectorSettings valueListSelectorSettings = null;
        //    FileSystemParameterSuggestionEditorSettings fileSystemParameterSuggestionEditorSettings = null;
        //    NotesSettings notesSettings = null;

        //    foreach (XmlNode innerNode in node.ChildNodes)
        //    {
        //        switch (innerNode.Name.ToUpperInvariant())
        //        {
        //            case "COMMANDEDITOR":
        //                commandEditorSettings = CommandEditorSettings.FromXml(innerNode);
        //                break;
        //            case "FUNCTIONEDITOR":
        //                functionEditorSettings = FunctionEditorSettings.FromXml(innerNode);
        //                break;
        //            case "ASSEMBLYREFERENCEEDITOR":
        //                assemblyReferenceEditorSettings = WindowSettings<Form>.FromXml(innerNode);
        //                break;
        //            case "SETUPDIALOG":
        //                setupDialogSettings = SetupDialogSettings.FromXml(innerNode);
        //                break;
        //            case "COMMANDSETUPPANEL":
        //                commandSetupPanelSettings = SetupPanelSettings.FromXml(innerNode);
        //                break;
        //            case "FUNCTIONSETUPPANEL":
        //                functionSetupPanelSettings = SetupPanelSettings.FromXml(innerNode);
        //                break;
        //            case "ASSEMBLYREFERENCESETUPPANEL":
        //                assemblyReferenceSetupPanelSettings = SetupPanelSettings.FromXml(innerNode);
        //                break;
        //            case "COLLISIONRESOLVINGDIALOG":
        //                collisionResolvingDialogSettings = WindowSettings<Form>.FromXml(innerNode);
        //                break;
        //            case "FUNCTIONVIEWERSETTINGS":
        //                functionViewerSettings = FunctionViewerSettings.FromXml(innerNode);
        //                break;
        //            case "VALUELISTEDITOR":
        //                valueListEditorSettings = ValueListEditorSettings.FromXml(innerNode);
        //                break;
        //            case "VALUELISTSETUPPANEL":
        //                valueListSetupPanelSettings = SetupPanelSettings.FromXml(innerNode);
        //                break;
        //            case "FUNCTIONINVOCATIONEDITOR":
        //                functionInvocationEditorSettings = FunctionInvocationEditorSettings.FromXml(innerNode);
        //                break;
        //            case "VALUELISTSELECTOR":
        //                valueListSelectorSettings = ValueListSelectorSettings.FromXml(innerNode);
        //                break;
        //            case "FILESYSTEMPARAMETERSUGGESTIONEDITOR":
        //                fileSystemParameterSuggestionEditorSettings = FileSystemParameterSuggestionEditorSettings.FromXml(innerNode);
        //                break;
        //            case "NOTES":
        //                notesSettings = NotesSettings.FromXml(innerNode);
        //                break;
        //            default:
        //                break;
        //        }
        //    }

        //    return new UISettings(
        //        commandEditorSettings,
        //        functionEditorSettings,
        //        valueListEditorSettings,
        //        assemblyReferenceEditorSettings,
        //        setupDialogSettings,
        //        commandSetupPanelSettings, 
        //        functionSetupPanelSettings, 
        //        assemblyReferenceSetupPanelSettings,
        //        valueListSetupPanelSettings,
        //        collisionResolvingDialogSettings,
        //        functionViewerSettings,
        //        functionInvocationEditorSettings,
        //        valueListSelectorSettings,
        //        fileSystemParameterSuggestionEditorSettings,
        //        notesSettings);
        //}

        //protected override XmlNode ToXmlCore(XmlDocument document)
        //{
        //    XmlNode node = document.CreateElement("UI");

        //    node.AppendChild(this.setupDialogSettings.ToXml("SetupDialog", document));
        //    node.AppendChild(this.commandEditorSettings.ToXml("CommandEditor", document));
        //    node.AppendChild(this.functionEditorSettings.ToXml("FunctionEditor", document));
        //    node.AppendChild(this.assemblyReferenceEditorSettings.ToXml("AssemblyReferenceEditor", document));
        //    node.AppendChild(this.commandSetupPanelSettings.ToXml("CommandSetupPanel", document));
        //    node.AppendChild(this.functionSetupPanelSettings.ToXml("FunctionSetupPanel", document));
        //    node.AppendChild(this.assemblyReferenceSetupPanelSettings.ToXml("AssemblyReferenceSetupPanel", document));
        //    node.AppendChild(this.valueListSetupPanelSettings.ToXml("ValueListSetupPanel", document));
        //    node.AppendChild(this.collisionResolvingDialogSettings.ToXml("CollisionResolvingDialog", document));
        //    node.AppendChild(this.functionViewerSettings.ToXml("FunctionViewerSettings", document));
        //    node.AppendChild(this.valueListEditorSettings.ToXml("ValueListEditor", document));
        //    node.AppendChild(this.functionInvocationEditorSettings.ToXml("FunctionInvocationEditor", document));
        //    node.AppendChild(this.valueListSelectorSettings.ToXml("ValueListSelector", document));
        //    node.AppendChild(this.fileSystemParameterSuggestionEditorSettings.ToXml("FileSystemParameterSuggestionEditor", document));
        //    node.AppendChild(this.notesSettings.ToXml("Notes", document));

        //    return node;
        //}
    }
}
