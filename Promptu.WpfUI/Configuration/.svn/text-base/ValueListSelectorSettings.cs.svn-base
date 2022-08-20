using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZachJohnson.Promptu.UIModel.Interfaces;
using ZachJohnson.Promptu.WpfUI.UIComponents;
using System.Xml;

namespace ZachJohnson.Promptu.WpfUI.Configuration
{
    internal class ValueListSelectorSettings : WindowSettings<IValueListSelectorWindow>
    {
        private SetupPanelSettings setupPanelSettings;

        public ValueListSelectorSettings()
            : this(null)
        {
        }

        public ValueListSelectorSettings(SetupPanelSettings setupPanelSettings)
        {
            this.setupPanelSettings = setupPanelSettings ?? new SetupPanelSettings();
            this.Register(this.setupPanelSettings);
        }

        protected override void ImpartToCore(IValueListSelectorWindow obj)
        {
            ValueListSelectorWindow window = (ValueListSelectorWindow)obj;
            this.setupPanelSettings.ImpartTo(window.setupPanel);

            base.ImpartToCore(obj);
        }

        protected override void UpdateFromCore(IValueListSelectorWindow obj, ref bool anythingChanged)
        {
            ValueListSelectorWindow window = (ValueListSelectorWindow)obj;
            this.setupPanelSettings.UpdateFrom(window.setupPanel, ref anythingChanged);

            base.UpdateFromCore(obj, ref anythingChanged);
        }

        protected override void ToXmlCore(System.Xml.XmlNode node)
        {
            this.setupPanelSettings.AppendAsXmlOn(node, "SetupPanel");
            base.ToXmlCore(node);
        }

        protected override void UpdateFromCore(System.Xml.XmlNode node)
        {
            foreach (XmlNode childNode in node.ChildNodes)
            {
                switch (childNode.Name.ToUpperInvariant())
                {
                    case "SETUPPANEL":
                        this.setupPanelSettings.UpdateFrom(childNode);
                        break;
                    default:
                        break;
                }
            }

            base.UpdateFromCore(node);
        }
    }
}
