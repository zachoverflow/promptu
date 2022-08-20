using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZachJohnson.Promptu.UIModel.Configuration;
using System.Xml;
using ZachJohnson.Promptu.Configuration;
using System.Globalization;

namespace ZachJohnson.Promptu.WpfUI.Configuration
{
    internal class ErrorPanelSettings : ObjectSettings<UIComponents.ErrorPanel>
    {
        private bool? showErrors;
        private bool? showWarnings;
        private bool? showMessages;
        private DataGridSettings dataGridSettings;

        public ErrorPanelSettings()
            : this(
            null,
            null,
            null,
            null)
        {
        }

        public ErrorPanelSettings(
            bool? showErrors,
            bool? showWarnings,
            bool? showMessages,
            DataGridSettings dataGridSettings)
        {
            this.showErrors = showErrors;
            this.showMessages = showMessages;
            this.showWarnings = showWarnings;
            this.dataGridSettings = dataGridSettings ?? new DataGridSettings();
            this.dataGridSettings.IsStarSized = true;

            this.Register(this.dataGridSettings);
        }

        protected override void ImpartToCore(UIComponents.ErrorPanel obj)
        {
            if (this.showErrors != null)
            {
                obj.ErrorsButton.Checked = this.showErrors.Value;
            }

            if (this.showWarnings != null)
            {
                obj.WarningsButton.Checked = this.showWarnings.Value;
            }

            if (this.showMessages != null)
            {
                obj.MessagesButton.Checked = this.showMessages.Value;
            }

            this.dataGridSettings.ImpartTo(obj.messagesGrid);
        }

        protected override void UpdateFromCore(UIComponents.ErrorPanel obj, ref bool anythingChanged)
        {
            if (obj.ErrorsButton.Checked != this.showErrors)
            {
                this.showErrors = obj.ErrorsButton.Checked;
                anythingChanged = true;
            }

            if (obj.WarningsButton.Checked != this.showWarnings)
            {
                this.showWarnings = obj.WarningsButton.Checked;
                anythingChanged = true;
            }

            if (obj.MessagesButton.Checked != this.showMessages)
            {
                this.showMessages = obj.MessagesButton.Checked;
                anythingChanged = true;
            }

            this.dataGridSettings.UpdateFrom(obj.messagesGrid, ref anythingChanged);
        }

        protected override void ToXmlCore(XmlNode node)
        {
            if (this.showErrors != null)
            {
                XmlUtilities.AppendAttribute(node, "showErrors", this.showErrors.Value);
            }

            if (this.showWarnings != null)
            {
                XmlUtilities.AppendAttribute(node, "showWarnings", this.showWarnings.Value);
            }

            if (this.showMessages != null)
            {
                XmlUtilities.AppendAttribute(node, "showMessages", this.showMessages.Value);
            }

            this.dataGridSettings.AppendAsXmlOn(node, "DataGrid");
        }

        private static bool? TryParseBoolean(string value, bool? defaultValue)
        {
            try
            {
                return Convert.ToBoolean(value, CultureInfo.InvariantCulture);
            }
            catch (FormatException)
            {
            }

            try
            {
                return Convert.ToBoolean(value, CultureInfo.CurrentCulture);
            }
            catch (FormatException)
            {
            }

            return defaultValue;
        }

        protected override void UpdateFromCore(XmlNode node)
        {
            this.showErrors = null;
            this.showWarnings = null;
            this.showMessages = null;

            foreach (XmlAttribute attribute in node.Attributes)
            {
                switch (attribute.Name.ToUpperInvariant())
                {
                    case "SHOWERRORS":
                        this.showErrors = TryParseBoolean(attribute.Value, this.showErrors);
                        //try
                        //{
                        //    this.showErrors = Convert.ToBoolean(attribute.Value);
                        //}
                        //catch (FormatException)
                        //{
                        //}

                        break;
                    case "SHOWWARNINGS":
                        this.showWarnings = TryParseBoolean(attribute.Value, this.showWarnings);
                        //try
                        //{
                        //    this.showWarnings = Convert.ToBoolean(attribute.Value);
                        //}
                        //catch (FormatException)
                        //{
                        //}

                        break;
                    case "SHOWMESSAGES":
                        this.showMessages = TryParseBoolean(attribute.Value, this.showMessages);
                        //try
                        //{
                        //    this.showMessages = Convert.ToBoolean(attribute.Value);
                        //}
                        //catch (FormatException)
                        //{
                        //}

                        break;
                    default:
                        break;
                }
            }

            foreach (XmlNode childNode in node.ChildNodes)
            {
                switch (childNode.Name.ToUpperInvariant())
                {
                    case "DATAGRID":
                        this.dataGridSettings.UpdateFrom(childNode);
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
