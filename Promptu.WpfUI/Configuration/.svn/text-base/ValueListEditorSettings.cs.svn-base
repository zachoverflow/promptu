using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZachJohnson.Promptu.UIModel.Interfaces;
using ZachJohnson.Promptu.WpfUI.UIComponents;
using System.Xml;

namespace ZachJohnson.Promptu.WpfUI.Configuration
{
    internal class ValueListEditorSettings : WindowSettings<IValueListEditor>
    {
        DataGridSettings valuesDataGridSettings;

        public ValueListEditorSettings()
            : this(null, null)
        {
        }

        public ValueListEditorSettings(NativeMethods.WINDOWPLACEMENT? placement, DataGridSettings valuesDataGridSettings)
            : base(placement)
        {
            this.valuesDataGridSettings = valuesDataGridSettings ?? new DataGridSettings();

            this.Register(this.valuesDataGridSettings);
        }

        protected override void ImpartToCore(IValueListEditor obj)
        {
            ValueListEditor editor = (ValueListEditor)obj;
            base.ImpartToCore(obj);

            this.valuesDataGridSettings.ImpartTo(editor.valuesCollectionEditor.dataGrid);
        }

        protected override void UpdateFromCore(IValueListEditor obj, ref bool anythingChanged)
        {
            ValueListEditor editor = (ValueListEditor)obj;
            base.UpdateFromCore(obj, ref anythingChanged);

            this.valuesDataGridSettings.UpdateFrom(editor.valuesCollectionEditor.dataGrid, ref anythingChanged);
        }

        protected override void ToXmlCore(XmlNode node)
        {
            base.ToXmlCore(node);
            this.valuesDataGridSettings.AppendAsXmlOn(node, "ValuesDataGrid");
        }

        protected override void UpdateFromCore(XmlNode node)
        {
            base.UpdateFromCore(node);

            foreach (XmlNode childNode in node.ChildNodes)
            {
                switch (childNode.Name.ToUpperInvariant())
                {
                    case "VALUESDATAGRID":
                        this.valuesDataGridSettings.UpdateFrom(childNode);
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
