using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZachJohnson.Promptu.UIModel.Configuration;
using System.Windows.Controls;
using System.Xml;
using ZachJohnson.Promptu.Configuration;

namespace ZachJohnson.Promptu.WpfUI.Configuration
{
    internal class DataGridSettings : ObjectSettings<DataGrid>
    {
        private bool isStarSized;
        private List<DataGridLength> columnWidths;

        public DataGridSettings()
            : this(null)
        {
        }

        public bool IsStarSized
        {
            get { return this.isStarSized; }
            set { this.isStarSized = value; }
        }

        public DataGridSettings(List<DataGridLength> columnWidths)
        {
            this.columnWidths = columnWidths ?? new List<DataGridLength>();
        }

        protected override void UpdateFromCore(DataGrid obj, ref bool anythingChanged)
        {
            this.columnWidths.Clear();

            for (int i = 0; i < obj.Columns.Count; i++)
            {
                DataGridColumn column = obj.Columns[i];
                DataGridLength width = column.Width;
                if (!this.isStarSized)
                {
                    this.columnWidths.Add(width);
                }
                else
                {
                    if (width.IsStar)
                    {
                        this.columnWidths.Add(width);
                    }
                    else// if (i > 0 && obj.Columns[i - 1].Width.IsStar)
                    {
                        this.columnWidths.Add(new DataGridLength(column.ActualWidth, DataGridLengthUnitType.Pixel));
                    }
                    //else
                    //{
                    //    this.columnWidths.Add(width);
                    //}
                }
            }

            anythingChanged = true;
        }

        protected override void ImpartToCore(DataGrid obj)
        {
            for (int i = 0; i < this.columnWidths.Count; i++)
            {
                if (i >= obj.Columns.Count)
                {
                    break;
                }

                obj.Columns[i].Width = this.columnWidths[i];
            }
        }

        protected override void ToXmlCore(XmlNode node)
        {
            DataGridLengthConverter converter = new DataGridLengthConverter();
            foreach (DataGridLength width in this.columnWidths)
            {
                XmlNode columnNode = node.OwnerDocument.CreateElement("Column");
                XmlUtilities.AppendAttribute(columnNode, "width", converter.ConvertToInvariantString(width));
                node.AppendChild(columnNode);
            }
        }

        protected override void UpdateFromCore(XmlNode node)
        {
            DataGridLengthConverter converter = new DataGridLengthConverter();
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
                                        (DataGridLength)converter.ConvertFromInvariantString(attribute.Value));
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
