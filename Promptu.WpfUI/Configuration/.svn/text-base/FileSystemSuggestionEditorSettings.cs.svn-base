using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZachJohnson.Promptu.UIModel.Interfaces;
using System.Xml;
using ZachJohnson.Promptu.WpfUI.UIComponents;

namespace ZachJohnson.Promptu.WpfUI.Configuration
{
    internal class FileSystemSuggestionEditorSettings : WindowSettings<IFileSystemParameterSuggestionEditor>
    {
        private double? errorPanelHeight;

        public FileSystemSuggestionEditorSettings()
            : this(null)
        {
        }

        public FileSystemSuggestionEditorSettings(double? errorPanelHeight)
            : base()
        {
            this.errorPanelHeight = errorPanelHeight;
        }

        protected override void ImpartToCore(IFileSystemParameterSuggestionEditor obj)
        {
            FileSystemParameterSuggestionEditor window = (FileSystemParameterSuggestionEditor)obj;

            double? errorPanelHeight = this.errorPanelHeight;
            if (errorPanelHeight != null)
            {
                window.errorPanel.Height = errorPanelHeight.Value;
            }

            base.ImpartToCore(obj);
        }

        protected override void UpdateFromCore(IFileSystemParameterSuggestionEditor obj, ref bool anythingChanged)
        {
            FileSystemParameterSuggestionEditor window = (FileSystemParameterSuggestionEditor)obj;

            double errorPanelHeight = window.errorPanel.Height;
            if (errorPanelHeight != this.errorPanelHeight)
            {
                this.errorPanelHeight = errorPanelHeight;
                anythingChanged = true;
            }

            base.UpdateFromCore(obj, ref anythingChanged);
        }

        protected override void ToXmlCore(System.Xml.XmlNode node)
        {
            double? errorPanelHeight = this.errorPanelHeight;
            if (errorPanelHeight != null)
            {
                XmlUtilities.AppendAttribute(node, "errorPanelHeight", errorPanelHeight.Value);
            }

            base.ToXmlCore(node);
        }

        protected override void UpdateFromCore(System.Xml.XmlNode node)
        {
            foreach (XmlAttribute attribute in node.Attributes)
            {
                switch (attribute.Name.ToUpperInvariant())
                {
                    case "ERRORPANELHEIGHT":
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
                    default:
                        break;
                }
            }

            base.UpdateFromCore(node);
        }
    }
}
