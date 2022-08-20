using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZachJohnson.Promptu.UIModel.Interfaces;
using ZachJohnson.Promptu.WpfUI.UIComponents;
using System.Xml;

namespace ZachJohnson.Promptu.WpfUI.Configuration
{
    internal class FunctionEditorSettings : WindowSettings<IFunctionEditor>
    {
        DataGridSettings parameterDataGridSettings;

        public FunctionEditorSettings()
            : this(null, null)
        {
        }

        public FunctionEditorSettings(NativeMethods.WINDOWPLACEMENT? placement, DataGridSettings parameterDataGridSettings)
            : base(placement)
        {
            this.parameterDataGridSettings = parameterDataGridSettings ?? new DataGridSettings();

            this.Register(this.parameterDataGridSettings);
        }

        protected override void ImpartToCore(IFunctionEditor obj)
        {
            FunctionEditor editor = (FunctionEditor)obj;
            base.ImpartToCore(obj);

            this.parameterDataGridSettings.ImpartTo(editor.parameterEditor.dataGrid);
        }

        protected override void UpdateFromCore(IFunctionEditor obj, ref bool anythingChanged)
        {
            FunctionEditor editor = (FunctionEditor)obj;
            base.UpdateFromCore(obj, ref anythingChanged);

            this.parameterDataGridSettings.UpdateFrom(editor.parameterEditor.dataGrid, ref anythingChanged);
        }

        protected override void ToXmlCore(XmlNode node)
        {
            base.ToXmlCore(node);
            this.parameterDataGridSettings.AppendAsXmlOn(node, "ParameterDataGrid");
        }

        protected override void UpdateFromCore(XmlNode node)
        {
            base.UpdateFromCore(node);

            foreach (XmlNode childNode in node.ChildNodes)
            {
                switch (childNode.Name.ToUpperInvariant())
                {
                    case "PARAMETERDATAGRID":
                        this.parameterDataGridSettings.UpdateFrom(childNode);
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
