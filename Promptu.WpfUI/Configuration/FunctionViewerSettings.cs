using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZachJohnson.Promptu.UIModel.Interfaces;
using ZachJohnson.Promptu.WpfUI.UIComponents;
using System.Xml;
using System.Globalization;

namespace ZachJohnson.Promptu.WpfUI.Configuration
{
    internal class FunctionViewerSettings : WindowSettings<IFunctionViewer>
    {
        private double? detailsHeight;

        public FunctionViewerSettings()
            : this(null)
        {
        }

        public FunctionViewerSettings(double? detailsHeight)
            : base()
        {
            this.detailsHeight = detailsHeight;
        }

        protected override void ImpartToCore(IFunctionViewer obj)
        {
            FunctionViewer window = (FunctionViewer)obj;

            double? detailsHeight = this.detailsHeight;
            if (detailsHeight != null)
            {
                window.details.Height = detailsHeight.Value;
            }

            base.ImpartToCore(obj);
        }

        protected override void UpdateFromCore(IFunctionViewer obj, ref bool anythingChanged)
        {
            FunctionViewer window = (FunctionViewer)obj;

            double detailsHeight = window.details.Height;
            if (detailsHeight != this.detailsHeight)
            {
                this.detailsHeight = detailsHeight;
                anythingChanged = true;
            }

            base.UpdateFromCore(obj, ref anythingChanged);
        }

        protected override void ToXmlCore(System.Xml.XmlNode node)
        {
            double? detailsHeight = this.detailsHeight;
            if (detailsHeight != null)
            {
                XmlUtilities.AppendAttribute(node, "detailsHeight", detailsHeight.Value);
            }

            base.ToXmlCore(node);
        }

        protected override void UpdateFromCore(System.Xml.XmlNode node)
        {
            foreach (XmlAttribute attribute in node.Attributes)
            {
                switch (attribute.Name.ToUpperInvariant())
                {
                    case "DETAILSHEIGHT":
                        this.detailsHeight = WpfUtilities.TryParseDouble(attribute.Value, this.detailsHeight);
                        //try
                        //{
                        //    this.detailsHeight = Convert.ToDouble(attribute.Value);
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
