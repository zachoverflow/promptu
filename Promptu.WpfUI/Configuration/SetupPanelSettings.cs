using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZachJohnson.Promptu.UIModel.Configuration;
using ZachJohnson.Promptu.UIModel.Interfaces;
using System.Xml;
using System.Windows.Controls;
using System.ComponentModel;
using ZachJohnson.Promptu.Configuration;

namespace ZachJohnson.Promptu.WpfUI.Configuration
{
    internal class SetupPanelSettings : ObjectSettings<ISetupPanel>
    {
        private List<double> columnWidths = new List<double>();

        public SetupPanelSettings()
            : this(null)
        {
        }

        public SetupPanelSettings(List<double> columnWidths)
        {
            this.columnWidths = columnWidths ?? new List<double>();
        }

        protected override void ImpartToCore(ISetupPanel obj)
        {
            SetupPanel setupPanel = (SetupPanel)obj;
            GridView grid = (GridView)setupPanel.collectionViewer.View;

            for (int i = 0; i < this.columnWidths.Count; i++)
            {
                if (i >= grid.Columns.Count)
                {
                    break;
                }

                grid.Columns[i].Width = columnWidths[i];
            }
        }

        protected override void UpdateFromCore(ISetupPanel obj, ref bool anythingChanged)
        {
            SetupPanel setupPanel = (SetupPanel)obj;
            GridView grid = (GridView)setupPanel.collectionViewer.View;

            this.columnWidths.Clear();

            foreach (GridViewColumn column in grid.Columns)
            {
                this.columnWidths.Add(column.Width);
            }

            anythingChanged = true;
        }

        protected override void ToXmlCore(XmlNode node)
        {
            DoubleConverter converter = new DoubleConverter();
            foreach (double width in this.columnWidths)
            {
                XmlNode columnNode = node.OwnerDocument.CreateElement("Column");
                XmlUtilities.AppendAttribute(columnNode, "width", converter.ConvertToInvariantString(width));
                node.AppendChild(columnNode);
            }
        }

        protected override void UpdateFromCore(XmlNode node)
        {
            DoubleConverter converter = new DoubleConverter();
            this.columnWidths.Clear();

            foreach (XmlNode childNode in node.ChildNodes)
            {
                switch (childNode.Name.ToUpperInvariant())
                {
                    case "COLUMN":
                        foreach (XmlAttribute attribute in childNode.Attributes)
                        {
                            if (attribute.Name.ToUpperInvariant() == "WIDTH")
                            {
                                try
                                {
                                    this.columnWidths.Add(
                                        (double)converter.ConvertFromInvariantString(attribute.Value));
                                    break;
                                }
                                catch (NotSupportedException)
                                {
                                }
                            }
                        }

                        break;
                    default:
                        break;
                }
            }
        }
    }
}
