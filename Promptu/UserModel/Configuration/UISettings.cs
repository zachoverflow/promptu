//using System;
//using System.Collections.Generic;
//using System.Text;
//using System.Xml;
//using System.Windows.Forms;

//namespace ZachJohnson.Promptu.UserModel.Configuration
//{
//    internal class UISettings : Settings
//    {
//        private CommandEditorSettings commandEditorSettings;
//        private FunctionEditorSettings functionEditorSettings;
//        private ValueListEditorSettings valueListEditorSettings;
//        private WindowSettings<Form> assemblyReferenceEditorSettings;
//        private SetupDialogSettings setupDialogSettings;
//        private SetupPanelSettings commandSetupPanelSettings;
//        private SetupPanelSettings functionSetupPanelSettings;
//        private SetupPanelSettings assemblyReferenceSetupPanelSettings;
//        private SetupPanelSettings valueListSetupPanelSettings;
//        private WindowSettings<Form> collisionResolvingDialogSettings;
//        private FunctionViewerSettings functionViewerSettings;
//        private FunctionInvocationEditorSettings functionInvocationEditorSettings;
//        private ValueListSelectorSettings valueListSelectorSettings;
//        private FileSystemParameterSuggestionEditorSettings fileSystemParameterSuggestionEditorSettings;
//        private NotesSettings notesSettings;

//        public UISettings()
//            : this(null, null, null, null, null, null, null, null, null, null, null, null, null, null, null)
//        {
//        }

//        public UISettings(
//            CommandEditorSettings commandEditorSettings,
//            FunctionEditorSettings functionEditorSettings,
//            ValueListEditorSettings valueListEditorSettings,
//            WindowSettings<Form> assemblyReferenceEditorSettings,
//            SetupDialogSettings setupDialogSettings,
//            SetupPanelSettings commandSetupPanelSettings, 
//            SetupPanelSettings functionSetupPanelSettings,
//            SetupPanelSettings assemblyReferenceSetupPanelSettings,
//            SetupPanelSettings valueListSetupPanelSettings,
//            WindowSettings<Form> collisionResolvingDialogSettings,
//            FunctionViewerSettings functionViewerSettings,
//            FunctionInvocationEditorSettings functionInvocationEditorSettings,
//            ValueListSelectorSettings valueListSelectorSettings,
//            FileSystemParameterSuggestionEditorSettings fileSystemParameterSuggestionEditorSettings,
//            NotesSettings notesSettings)
//        {
//            if (commandEditorSettings == null)
//            {
//                this.commandEditorSettings = new CommandEditorSettings();
//            }
//            else
//            {
//                this.commandEditorSettings = commandEditorSettings;
//            }

//            if (functionEditorSettings == null)
//            {
//                this.functionEditorSettings = new FunctionEditorSettings();
//            }
//            else
//            {
//                this.functionEditorSettings = functionEditorSettings;
//            }

//            if (assemblyReferenceEditorSettings == null)
//            {
//                this.assemblyReferenceEditorSettings = new WindowSettings<Form>();
//            }
//            else
//            {
//                this.assemblyReferenceEditorSettings = assemblyReferenceEditorSettings;
//            }

//            if (setupDialogSettings == null)
//            {
//                this.setupDialogSettings = new SetupDialogSettings();
//            }
//            else
//            {
//                this.setupDialogSettings = setupDialogSettings;
//            }

//            if (commandSetupPanelSettings == null)
//            {
//                this.commandSetupPanelSettings = new SetupPanelSettings();
//            }
//            else
//            {
//                this.commandSetupPanelSettings = commandSetupPanelSettings;
//            }

//            if (functionSetupPanelSettings == null)
//            {
//                this.functionSetupPanelSettings = new SetupPanelSettings();
//            }
//            else
//            {
//                this.functionSetupPanelSettings = functionSetupPanelSettings;
//            }

//            if (assemblyReferenceSetupPanelSettings == null)
//            {
//                this.assemblyReferenceSetupPanelSettings = new SetupPanelSettings();
//            }
//            else
//            {
//                this.assemblyReferenceSetupPanelSettings = assemblyReferenceSetupPanelSettings;
//            }

//            if (collisionResolvingDialogSettings == null)
//            {
//                this.collisionResolvingDialogSettings = new WindowSettings<Form>();
//            }
//            else
//            {
//                this.collisionResolvingDialogSettings = collisionResolvingDialogSettings;
//            }

//            if (functionViewerSettings == null)
//            {
//                this.functionViewerSettings = new FunctionViewerSettings();
//            }
//            else
//            {
//                this.functionViewerSettings = functionViewerSettings;
//            }

//            if (valueListEditorSettings == null)
//            {
//                this.valueListEditorSettings = new ValueListEditorSettings();
//            }
//            else
//            {
//                this.valueListEditorSettings = valueListEditorSettings;
//            }

//            if (valueListSetupPanelSettings == null)
//            {
//                this.valueListSetupPanelSettings = new SetupPanelSettings();
//            }
//            else
//            {
//                this.valueListSetupPanelSettings = valueListSetupPanelSettings;
//            }

//            if (functionInvocationEditorSettings == null)
//            {
//                this.functionInvocationEditorSettings = new FunctionInvocationEditorSettings();
//            }
//            else
//            {
//                this.functionInvocationEditorSettings = functionInvocationEditorSettings;
//            }

//            if (valueListSelectorSettings == null)
//            {
//                this.valueListSelectorSettings = new ValueListSelectorSettings();
//            }
//            else
//            {
//                this.valueListSelectorSettings = valueListSelectorSettings;
//            }

//            if (fileSystemParameterSuggestionEditorSettings == null)
//            {
//                this.fileSystemParameterSuggestionEditorSettings = new FileSystemParameterSuggestionEditorSettings();
//            }
//            else
//            {
//                this.fileSystemParameterSuggestionEditorSettings = fileSystemParameterSuggestionEditorSettings;
//            }

//            if (notesSettings == null)
//            {
//                this.notesSettings = new NotesSettings();
//            }
//            else
//            {
//                this.notesSettings = notesSettings;
//            }

//            this.functionViewerSettings.SettingChanged += this.RaiseSettingChangedEvent;
//            this.commandEditorSettings.SettingChanged += this.RaiseSettingChangedEvent;
//            this.functionEditorSettings.SettingChanged += this.RaiseSettingChangedEvent;
//            this.assemblyReferenceEditorSettings.SettingChanged += this.RaiseSettingChangedEvent;
//            this.setupDialogSettings.SettingChanged += this.RaiseSettingChangedEvent;
//            this.commandSetupPanelSettings.SettingChanged += this.RaiseSettingChangedEvent;
//            this.functionSetupPanelSettings.SettingChanged += this.RaiseSettingChangedEvent;
//            this.assemblyReferenceSetupPanelSettings.SettingChanged += this.RaiseSettingChangedEvent;
//            this.collisionResolvingDialogSettings.SettingChanged += this.RaiseSettingChangedEvent;
//            this.valueListEditorSettings.SettingChanged += this.RaiseSettingChangedEvent;
//            this.valueListSetupPanelSettings.SettingChanged += this.RaiseSettingChangedEvent;
//            this.functionInvocationEditorSettings.SettingChanged += this.RaiseSettingChangedEvent;
//            this.valueListSelectorSettings.SettingChanged += this.RaiseSettingChangedEvent;
//            this.fileSystemParameterSuggestionEditorSettings.SettingChanged += this.RaiseSettingChangedEvent;
//            this.notesSettings.SettingChanged += this.RaiseSettingChangedEvent;
//        }

//        public NotesSettings NotesSettings
//        {
//            get { return this.notesSettings; }
//        }

//        public FileSystemParameterSuggestionEditorSettings FileSystemParameterSuggestionEditorSettings
//        {
//            get { return this.fileSystemParameterSuggestionEditorSettings; }
//        }

//        public FunctionInvocationEditorSettings FunctionInvocationEditorSettings
//        {
//            get { return this.functionInvocationEditorSettings; }
//        }

//        public ValueListSelectorSettings ValueListSelectorSettings
//        {
//            get { return this.valueListSelectorSettings; }
//        } 

//        public CommandEditorSettings CommandEditorSettings
//        {
//            get { return this.commandEditorSettings; }
//        }

//        public FunctionEditorSettings FunctionEditorSettings
//        {
//            get { return this.functionEditorSettings; }
//        }

//        public WindowSettings<Form> AssemblyReferenceEditorSettings
//        {
//            get { return this.assemblyReferenceEditorSettings; }
//        }

//        public ValueListEditorSettings ValueListEditorSettings
//        {
//            get { return this.valueListEditorSettings; }
//        }

//        public SetupDialogSettings SetupDialogSettings
//        {
//            get { return this.setupDialogSettings; }
//        }

//        public SetupPanelSettings CommandSetupPanelSettings
//        {
//            get { return this.commandSetupPanelSettings; }
//        }

//        public SetupPanelSettings FunctionSetupPanelSettings
//        {
//            get { return this.functionSetupPanelSettings; }
//        }

//        public SetupPanelSettings ValueListSetupPanelSettigns
//        {
//            get { return this.valueListSetupPanelSettings; }
//        }

//        public SetupPanelSettings AssemblyReferenceSetupPanelSettings
//        {
//            get { return this.assemblyReferenceSetupPanelSettings; }
//        }

//        public WindowSettings<Form> CollisionResolvingDialogSettings
//        {
//            get { return this.collisionResolvingDialogSettings; }
//        }

//        public FunctionViewerSettings FunctionViewerSettings
//        {
//            get { return this.functionViewerSettings; }
//        }

//        public static UISettings FromXml(XmlNode node)
//        {
//            if (node == null)
//            {
//                throw new ArgumentNullException("node");
//            }

//            CommandEditorSettings commandEditorSettings = null;
//            FunctionEditorSettings functionEditorSettings = null;
//            ValueListEditorSettings valueListEditorSettings = null;
//            WindowSettings<Form> assemblyReferenceEditorSettings = null;
//            SetupDialogSettings setupDialogSettings = null;
//            SetupPanelSettings commandSetupPanelSettings = null;
//            SetupPanelSettings functionSetupPanelSettings = null;
//            SetupPanelSettings assemblyReferenceSetupPanelSettings = null;
//            SetupPanelSettings valueListSetupPanelSettings = null;
//            WindowSettings<Form> collisionResolvingDialogSettings = null;
//            FunctionViewerSettings functionViewerSettings = null;
//            FunctionInvocationEditorSettings functionInvocationEditorSettings = null;
//            ValueListSelectorSettings valueListSelectorSettings = null;
//            FileSystemParameterSuggestionEditorSettings fileSystemParameterSuggestionEditorSettings = null;
//            NotesSettings notesSettings = null;

//            foreach (XmlNode innerNode in node.ChildNodes)
//            {
//                switch (innerNode.Name.ToUpperInvariant())
//                {
//                    case "COMMANDEDITOR":
//                        commandEditorSettings = CommandEditorSettings.FromXml(innerNode);
//                        break;
//                    case "FUNCTIONEDITOR":
//                        functionEditorSettings = FunctionEditorSettings.FromXml(innerNode);
//                        break;
//                    case "ASSEMBLYREFERENCEEDITOR":
//                        assemblyReferenceEditorSettings = WindowSettings<Form>.FromXml(innerNode);
//                        break;
//                    case "SETUPDIALOG":
//                        setupDialogSettings = SetupDialogSettings.FromXml(innerNode);
//                        break;
//                    case "COMMANDSETUPPANEL":
//                        commandSetupPanelSettings = SetupPanelSettings.FromXml(innerNode);
//                        break;
//                    case "FUNCTIONSETUPPANEL":
//                        functionSetupPanelSettings = SetupPanelSettings.FromXml(innerNode);
//                        break;
//                    case "ASSEMBLYREFERENCESETUPPANEL":
//                        assemblyReferenceSetupPanelSettings = SetupPanelSettings.FromXml(innerNode);
//                        break;
//                    case "COLLISIONRESOLVINGDIALOG":
//                        collisionResolvingDialogSettings = WindowSettings<Form>.FromXml(innerNode);
//                        break;
//                    case "FUNCTIONVIEWERSETTINGS":
//                        functionViewerSettings = FunctionViewerSettings.FromXml(innerNode);
//                        break;
//                    case "VALUELISTEDITOR":
//                        valueListEditorSettings = ValueListEditorSettings.FromXml(innerNode);
//                        break;
//                    case "VALUELISTSETUPPANEL":
//                        valueListSetupPanelSettings = SetupPanelSettings.FromXml(innerNode);
//                        break;
//                    case "FUNCTIONINVOCATIONEDITOR":
//                        functionInvocationEditorSettings = FunctionInvocationEditorSettings.FromXml(innerNode);
//                        break;
//                    case "VALUELISTSELECTOR":
//                        valueListSelectorSettings = ValueListSelectorSettings.FromXml(innerNode);
//                        break;
//                    case "FILESYSTEMPARAMETERSUGGESTIONEDITOR":
//                        fileSystemParameterSuggestionEditorSettings = FileSystemParameterSuggestionEditorSettings.FromXml(innerNode);
//                        break;
//                    case "NOTES":
//                        notesSettings = NotesSettings.FromXml(innerNode);
//                        break;
//                    default:
//                        break;
//                }
//            }

//            return new UISettings(
//                commandEditorSettings,
//                functionEditorSettings,
//                valueListEditorSettings,
//                assemblyReferenceEditorSettings,
//                setupDialogSettings,
//                commandSetupPanelSettings, 
//                functionSetupPanelSettings, 
//                assemblyReferenceSetupPanelSettings,
//                valueListSetupPanelSettings,
//                collisionResolvingDialogSettings,
//                functionViewerSettings,
//                functionInvocationEditorSettings,
//                valueListSelectorSettings,
//                fileSystemParameterSuggestionEditorSettings,
//                notesSettings);
//        }

//        protected override XmlNode ToXmlCore(XmlDocument document)
//        {
//            XmlNode node = document.CreateElement("UI");

//            node.AppendChild(this.setupDialogSettings.ToXml("SetupDialog", document));
//            node.AppendChild(this.commandEditorSettings.ToXml("CommandEditor", document));
//            node.AppendChild(this.functionEditorSettings.ToXml("FunctionEditor", document));
//            node.AppendChild(this.assemblyReferenceEditorSettings.ToXml("AssemblyReferenceEditor", document));
//            node.AppendChild(this.commandSetupPanelSettings.ToXml("CommandSetupPanel", document));
//            node.AppendChild(this.functionSetupPanelSettings.ToXml("FunctionSetupPanel", document));
//            node.AppendChild(this.assemblyReferenceSetupPanelSettings.ToXml("AssemblyReferenceSetupPanel", document));
//            node.AppendChild(this.valueListSetupPanelSettings.ToXml("ValueListSetupPanel", document));
//            node.AppendChild(this.collisionResolvingDialogSettings.ToXml("CollisionResolvingDialog", document));
//            node.AppendChild(this.functionViewerSettings.ToXml("FunctionViewerSettings", document));
//            node.AppendChild(this.valueListEditorSettings.ToXml("ValueListEditor", document));
//            node.AppendChild(this.functionInvocationEditorSettings.ToXml("FunctionInvocationEditor", document));
//            node.AppendChild(this.valueListSelectorSettings.ToXml("ValueListSelector", document));
//            node.AppendChild(this.fileSystemParameterSuggestionEditorSettings.ToXml("FileSystemParameterSuggestionEditor", document));
//            node.AppendChild(this.notesSettings.ToXml("Notes", document));

//            return node;
//        }
//    }
//}
