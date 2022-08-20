using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZachJohnson.Promptu.UIModel.Interfaces;
using ZachJohnson.Promptu.WpfUI.UIComponents;
using System.Xml;

namespace ZachJohnson.Promptu.WpfUI.Configuration
{
    internal class CommandEditorSettings : WindowSettings<ICommandEditor>
    {
        private ErrorPanelSettings errorPanelSettings;
        private double? errorPanelHeight;
        private ErrorPanelDisplayMode? errorPanelLastVisibleDisplayMode;
        private DataGridSettings parameterInfoDataGridSettings;

        public CommandEditorSettings()
            : this(null, null, null, null, null)
        {
        }

        public CommandEditorSettings(
            NativeMethods.WINDOWPLACEMENT? placement, 
            ErrorPanelSettings errorPanelSettings,
            double? errorPanelHeight,
            ErrorPanelDisplayMode? errorPanelLastVisibleDisplayMode,
            DataGridSettings parameterInfoDataGridSettings)
            : base(placement)
        {
            this.errorPanelSettings = errorPanelSettings ?? new ErrorPanelSettings();
            this.errorPanelHeight = errorPanelHeight;
            this.errorPanelLastVisibleDisplayMode = errorPanelLastVisibleDisplayMode;

            this.parameterInfoDataGridSettings = parameterInfoDataGridSettings ?? new DataGridSettings();

            this.Register(this.errorPanelSettings);
            this.Register(this.parameterInfoDataGridSettings);
        }

        public ErrorPanelSettings ErrorPanelSettings
        {
            get { return this.errorPanelSettings; }
        }

        protected override void ImpartToCore(ICommandEditor obj)
        {
            CommandEditor editor = (CommandEditor)obj;
            ErrorPanel errorPanel = obj.ErrorPanel as ErrorPanel;

            base.ImpartToCore(obj);
            this.errorPanelSettings.ImpartTo(errorPanel);

            if (this.errorPanelHeight != null && errorPanel != null)
            {
                errorPanel.Height = this.errorPanelHeight.Value;
            }

            if (this.errorPanelLastVisibleDisplayMode != null)
            {
                editor.LastVisibleErrorPanelDisplayMode = this.errorPanelLastVisibleDisplayMode.Value;
            }

            this.parameterInfoDataGridSettings.ImpartTo(editor.commandParameterPanel.dataGrid);
        }

        protected override void UpdateFromCore(ICommandEditor obj, ref bool anythingChanged)
        {
            CommandEditor editor = (CommandEditor)obj;
            ErrorPanel errorPanel = obj.ErrorPanel as ErrorPanel;

            base.UpdateFromCore(obj, ref anythingChanged);
            this.errorPanelSettings.UpdateFrom(errorPanel, ref anythingChanged);

            if (errorPanel != null)
            {
                this.errorPanelHeight = (double)errorPanel.GetAnimationBaseValue(ErrorPanel.HeightProperty);
            }

            this.errorPanelLastVisibleDisplayMode = editor.LastVisibleErrorPanelDisplayMode;

            this.parameterInfoDataGridSettings.UpdateFrom(editor.commandParameterPanel.dataGrid);
        }

        protected override void ToXmlCore(XmlNode node)
        {
            base.ToXmlCore(node);
            XmlNode errorPanelNode = this.errorPanelSettings.ToXml("ErrorPanel", node.OwnerDocument);
            node.AppendChild(errorPanelNode);

            if (this.errorPanelHeight != null)
            {
                XmlUtilities.AppendAttribute(errorPanelNode, "height", this.errorPanelHeight.Value);
            }

            if (this.errorPanelLastVisibleDisplayMode != null)
            {
                XmlUtilities.AppendAttribute(errorPanelNode, "displayMode", this.errorPanelLastVisibleDisplayMode);
            }

            this.parameterInfoDataGridSettings.AppendAsXmlOn(node, "ParameterInfoDataGrid");
        }

        protected override void UpdateFromCore(XmlNode node)
        {
            base.UpdateFromCore(node);

            this.errorPanelHeight = null;
            this.errorPanelLastVisibleDisplayMode = null;

            foreach (XmlNode childNode in node.ChildNodes)
            {
                switch (childNode.Name.ToUpperInvariant())
                {
                    case "ERRORPANEL":
                        this.errorPanelSettings.UpdateFrom(childNode);

                        foreach (XmlAttribute attribute in childNode.Attributes)
                        {
                            switch (attribute.Name.ToUpperInvariant())
                            {
                                case "HEIGHT":
                                    this.errorPanelHeight = WpfUtilities.TryParseDouble(attribute.Value, this.errorPanelHeight);
                                    //try
                                    //{
                                    //    this.errorPanelHeight = Convert.ToDouble(attribute.Value);
                                    //}
                                    //catch (FormatException)
                                    //{
                                    //}
                                    //catch (OverflowException)
                                    //{
                                    //}

                                    break;
                                case "DISPLAYMODE":
                                    try
                                    {
                                        this.errorPanelLastVisibleDisplayMode = (ErrorPanelDisplayMode)Enum.Parse(
                                            typeof(ErrorPanelDisplayMode),
                                            attribute.Value);
                                    }
                                    catch (OverflowException)
                                    {
                                    }
                                    catch (ArgumentException)
                                    {
                                    }

                                    break;
                                default:
                                    break;
                            }
                        }

                        break;
                    case "PARAMETERINFODATAGRID":
                        this.parameterInfoDataGridSettings.UpdateFrom(childNode);
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
