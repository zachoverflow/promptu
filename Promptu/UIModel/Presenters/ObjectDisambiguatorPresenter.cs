using System;
using System.Collections.Generic;
using System.Text;
using ZachJohnson.Promptu.UIModel.Interfaces;
using ZachJohnson.Promptu.UserModel;

namespace ZachJohnson.Promptu.UIModel.Presenters
{
    internal class ObjectDisambiguatorPresenter : DialogPresenterBase<IObjectDisambiguator>
    {
        public ObjectDisambiguatorPresenter(
            string mainInstructions,
            List<object> ambiguousObjects)
            : this(
            InternalGlobals.GuiManager.ToolkitHost.Factory.ConstructObjectDisambiguator(),
            mainInstructions,
            ambiguousObjects)
        {
        }

        public ObjectDisambiguatorPresenter(
            IObjectDisambiguator nativeInterface,
            string mainInstructions,
            List<object> ambiguousObjects)
            : base(nativeInterface)
        {
            this.NativeInterface.Text = Localization.Promptu.AppName;

            this.NativeInterface.MainInstructions = mainInstructions;
            this.NativeInterface.OkButton.Text = Localization.UIResources.OkButtonText;
            this.NativeInterface.CancelButton.Text = Localization.UIResources.CancelButtonText;

            if (ambiguousObjects.Count > 0)
            {
                this.NativeInterface.SelectedObject = ambiguousObjects[0];
            }

            //foreach (Function function in functions)
            //{
            //    string format;
            //    int parameterCount = function.Parameters.Count;

            //    if (parameterCount == 1)
            //    {
            //        format = Localization.UIResources.ParameterCountFormatSingular;
            //    }
            //    else
            //    {
            //        format = Localization.UIResources.ParameterCountFormatPlural;
            //    }

            //    this.NativeInterface.ParameterCountComboInput.AddValue(new StringRepresentation<Function>(function, String.Format(format, parameterCount)));
            //}

            //if (this.NativeInterface.ParameterCountComboInput.ValueCount > 0)
            //{
            //    this.NativeInterface.ParameterCountComboInput.SelectedIndex = 0;
            //}

            this.NativeInterface.SetAmbiguousObjects(ambiguousObjects);
        }

        public object SelectedObject
        {
            get
            {
                //StringRepresentation<Function> representation = this.NativeInterface.ParameterCountComboInput.SelectedValue as StringRepresentation<Function>;
                //if (representation != null)
                //{
                //    return representation.Value;
                //}

                //return null;

                return this.NativeInterface.SelectedObject;
            }
        }
    }
}
