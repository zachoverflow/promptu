using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZachJohnson.Promptu.UIModel.Configuration;
using ZachJohnson.Promptu.UIModel.Interfaces;
using ZachJohnson.Promptu.WpfUI.UIComponents;
using System.Xml;
using ZachJohnson.Promptu.Configuration;

namespace ZachJohnson.Promptu.WpfUI.Configuration
{
    internal class ProfileTabSettings : ObjectSettings<IProfileTabPanel>
    {
        private double? listSelectorWidth;

        public ProfileTabSettings()
            : this(null)
        {
        }

        public ProfileTabSettings(double? listSelectorWidth)
        {
            this.listSelectorWidth = listSelectorWidth;
        }

        protected override void ImpartToCore(IProfileTabPanel obj)
        {
            ProfileTabPanel panel = (ProfileTabPanel)obj;

            double? listSelectorWidth = this.listSelectorWidth;
            if (listSelectorWidth != null)
            {
                panel.ListSelectorWidth = listSelectorWidth.Value;
            }
        }

        protected override void UpdateFromCore(IProfileTabPanel obj, ref bool anythingChanged)
        {
            ProfileTabPanel panel = (ProfileTabPanel)obj;

            double listSelectorWidth = panel.ListSelectorWidth;
            if (listSelectorWidth != this.listSelectorWidth)
            {
                this.listSelectorWidth = listSelectorWidth;
                anythingChanged = true;
            }
        }

        protected override void ToXmlCore(System.Xml.XmlNode node)
        {
            double? listSelectorWidth = this.listSelectorWidth;
            if (listSelectorWidth != null)
            {
                XmlUtilities.AppendAttribute(node, "listSelectorWidth", listSelectorWidth.Value);
            }
        }

        protected override void UpdateFromCore(System.Xml.XmlNode node)
        {
            foreach (XmlAttribute attribute in node.Attributes)
            {
                switch (attribute.Name.ToUpperInvariant())
                {
                    case "LISTSELECTORWIDTH":
                        this.listSelectorWidth = WpfUtilities.TryParseDouble(attribute.Value, this.listSelectorWidth);
                        //try
                        //{
                        //    this.listSelectorWidth = Convert.ToDouble(attribute.Value);
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
        }
    }
}
