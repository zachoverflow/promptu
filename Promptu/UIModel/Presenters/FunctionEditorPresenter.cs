using System;
using System.Collections.Generic;
using System.Text;
using ZachJohnson.Promptu.UIModel.Interfaces;
using ZachJohnson.Promptu.UserModel;
using System.Windows.Forms;
using System.Extensions;
using ZachJohnson.Promptu.Skins;
using System.Globalization;

namespace ZachJohnson.Promptu.UIModel.Presenters
{
    internal class FunctionEditorPresenter : PresenterBase<IFunctionEditor>
    {
        private readonly List list;
        private readonly Function function;
        private readonly ParameterlessVoid updateAllCallback;
        private readonly FunctionParameterPanelPresenter functionParameterPanel;
        private bool nameIsInvalid;

        public FunctionEditorPresenter(
            Function function,
            List list,
            ParameterlessVoid updateAllCallback)
            : this(
                InternalGlobals.GuiManager.ToolkitHost.Factory.ConstructFunctionEditor(),
                function,
                list,
                updateAllCallback)
        {
        }

        public FunctionEditorPresenter(
            IFunctionEditor nativeInterface, 
            Function function, 
            List list, 
            ParameterlessVoid updateAllCallback)
            : base(nativeInterface)
        {
            if (function == null)
            {
                throw new ArgumentNullException("function");
            }
            else if (list == null)
            {
                throw new ArgumentNullException("list");
            }
            else if (updateAllCallback == null)
            {
                throw new ArgumentNullException("updateAllCallback");
            }

            this.list = list;
            this.function = function;
            this.updateAllCallback = updateAllCallback;

            this.NativeInterface.Text = Localization.UIResources.FunctionEditorText;
            this.NativeInterface.OkButton.Text = Localization.UIResources.OkButtonText;
            this.NativeInterface.CancelButton.Text = Localization.UIResources.CancelButtonText;
            this.NativeInterface.MethodLabelText = Localization.UIResources.FunctionEditorMethodLabelText;
            this.NativeInterface.NameLabelText = Localization.UIResources.FunctionEditorNameLabelText;
            this.NativeInterface.ClassLabelText = Localization.UIResources.FunctionEditorClassLabelText;
            this.NativeInterface.AssemblyLabelText = Localization.UIResources.FunctionEditorAssemblyLabelText;
            this.NativeInterface.TestFunctionButton.Text = Localization.UIResources.FunctionEditorTestButtonText;
            this.NativeInterface.ReturnsLabelText = Localization.UIResources.FunctionEditorReturnsLabelText;
            this.NativeInterface.MainInstructions = Localization.UIResources.FunctionEditorMainInstructions;

            this.NativeInterface.ReturnValue.Values = new object[] { 
                Localization.UIResources.StringDataType, 
                Localization.UIResources.StringArrayDataType, 
                Localization.UIResources.ValueListDataType };

            this.functionParameterPanel = new FunctionParameterPanelPresenter(
                this.NativeInterface.FunctionParameterPanel,
                updateAllCallback,
                new ZachJohnson.Promptu.UserModel.Collections.FunctionCollectionComposite(InternalGlobals.CurrentProfile.Lists, list));

            this.NativeInterface.Assembly.Values = list.AssemblyReferences.ToArray();

            AssemblyReference reference = null;
            if (list.AssemblyReferences.Contains(function.AssemblyReferenceName))
            {
                reference = list.AssemblyReferences[function.AssemblyReferenceName];
            }

            this.NativeInterface.Name.Text = function.Name;
            this.NativeInterface.Assembly.SelectedValue = reference;
            if (this.NativeInterface.Assembly.SelectedIndex < 0)
            {
                this.NativeInterface.Assembly.Text = function.AssemblyReferenceName;
            }

            this.NativeInterface.Class.Text = function.InvocationClass;
            this.NativeInterface.Method.Text = function.MethodName;

            this.NativeInterface.ReturnValue.SelectedIndex = (int)Math.Log((int)function.ReturnValue, 2);

            this.NativeInterface.TestFunctionButton.Click += this.TestFunctionButtonClick;

            this.NativeInterface.Name.TextChanged += this.ValidateAndUpdate;
            this.NativeInterface.Name.TextValidator = this.ValidateName;
            //this.NativeInterface.Name.KeyDown += this.HandleNameKeyDown;
            this.NativeInterface.Class.TextChanged += this.ValidateAndUpdate;
            this.NativeInterface.Method.TextChanged += this.ValidateAndUpdate;
            this.NativeInterface.Assembly.TextChanged += this.ValidateAndUpdate;

            this.functionParameterPanel.AssignItems(function.Parameters);

            this.ValidateAndUpdate();

            this.function = function;

            this.NativeInterface.OkButton.Click += this.HandleOkButtonClick;
        }

        public UIDialogResult ShowDialog()
        {
            return this.NativeInterface.ShowModal();
        }

        public Function AssembleFunction()
        {
            return new Function(
                this.NativeInterface.Name.Text, 
                this.NativeInterface.Class.Text, 
                this.NativeInterface.Method.Text, 
                this.NativeInterface.Assembly.Text, 
                (ReturnValue)Math.Pow(2, this.NativeInterface.ReturnValue.SelectedIndex), 
                this.functionParameterPanel.AssembleItems(), 
                function.Id,
                true);
        }

        private ValueValidationResult ValidateName(string value)
        {
            for (int i = 0; i < value.Length; i++)
            {
                char c = value[i];
                if (i == 0)
                {
                    if (!char.IsLetter(c) && c != '_')
                    {
                        if (!char.IsDigit(c) && c != '.')
                        {
                            this.SetNameIsInvalid(true);
                            return new ValueValidationResult(
                                false,
                                String.Format(CultureInfo.CurrentCulture, Localization.MessageFormats.FunctionEditorAlwaysInvalidChar, c));
                        }
                        else
                        {
                            this.SetNameIsInvalid(true);
                            return new ValueValidationResult(
                                false,
                                String.Format(CultureInfo.CurrentCulture, Localization.MessageFormats.FunctionEditorInvalidFirstChar, c));
                        }
                    }
                }
                else
                {
                    if (!char.IsLetterOrDigit(c) && c != '_' && c != '.')
                    {
                        this.SetNameIsInvalid(true);
                        return new ValueValidationResult(
                            false,
                            String.Format(CultureInfo.CurrentCulture, Localization.MessageFormats.FunctionEditorAlwaysInvalidChar, c));
                    }
                }
            }

            this.SetNameIsInvalid(false);
            return new ValueValidationResult(true, null);
        }

        private void SetNameIsInvalid(bool value)
        {
            this.nameIsInvalid = value;
            this.ValidateAndUpdate();
        }

        //private void HandleNameKeyDown(object sender, KeyEventArgs e)
        //{
        //    switch (e.KeyCode)
        //    {
        //        case Keys.Back:
        //        case Keys.Escape:
        //        case Keys.Tab:
        //            return;
        //        case Keys.Enter:
        //        case Keys.Space:
        //            e.SuppressKeyPress = true;
        //            break;
        //        default:
        //            if (e.Control)
        //            {
        //                return;
        //            }

        //            char c;

        //            try
        //            {
        //                c = Globals.GuiManager.ToolkitHost.Keyboard.ConvertToChar(e.KeyCode);
        //            }
        //            catch (ArgumentException)
        //            {
        //                return;
        //            }

        //            string message = null;
        //            bool valid = false;
        //            if (this.NativeInterface.Name.SelectionStart == 0)
        //            {
        //                valid = char.IsLetter(c) || c == '_';
        //                if (!valid)
        //                {
        //                    message = Localization.MessageFormats.FunctionEditorInvalidFirstChar;
        //                }
        //            }
        //            else
        //            {
        //                valid = char.IsLetterOrDigit(c) || c == '_' || c == '.';
        //                if (!valid)
        //                {
        //                    message = Localization.MessageFormats.FunctionEditorAlwaysInvalidChar;
        //                }
        //            }

        //            if (!valid)
        //            {
        //                UIMessageBox.Show(
        //                    message,
        //                    Localization.Promptu.AppName,
        //                    UIMessageBoxButtons.OK,
        //                    UIMessageBoxIcon.Error,
        //                    UIMessageBoxResult.OK);
        //            }

        //            e.SuppressKeyPress = !valid;
        //            break;
        //    }
        //}

        private void ValidateAndUpdate(object sender, EventArgs e)
        {
            this.ValidateAndUpdate();
        }

        private void ValidateAndUpdate()
        {
            if (
                this.NativeInterface.Name.Text.Length == 0
                || this.NativeInterface.Assembly.Text.Length == 0
                || this.NativeInterface.Class.Text.Length == 0
                || this.NativeInterface.Method.Text.Length == 0)
            {
                this.NativeInterface.OkButton.Enabled = false;
                this.NativeInterface.TestFunctionButton.Enabled = false;
            }
            else
            {
                this.NativeInterface.OkButton.Enabled = true;
                this.NativeInterface.TestFunctionButton.Enabled = true;
            }
        }

        private void TestFunctionButtonClick(object sender, EventArgs e)
        {
            Function function = this.AssembleFunction();
            string arguments = String.Empty;
            while (true)
            {
                if (function.Parameters.Count > 0)
                {
                    ArgumentDialogPresenter dialog = new ArgumentDialogPresenter();
                    dialog.Arguments = arguments;
                    if (dialog.ShowDialog() == UIDialogResult.Cancel)
                    {
                        return;
                    }

                    arguments = dialog.Arguments;
                }

                string[] args;
                if (arguments.Length > 0)
                {
                    args = arguments.ConvertToArguments();
                }
                else
                {
                    args = new string[0];
                }


                if (!PromptHandler.GetInstance().ExecuteFunction(function, this.list, args, false, ExecuteMode.Default))
                {
                    if (function.Parameters.Count != args.Length)
                    {
                        continue;
                    }
                }

                break;
            }
        }

        private void HandleOkButtonClick(object sender, EventArgs e)
        {
            if (this.NativeInterface.Name.Text.EndsWith("."))
            {
                UIMessageBox.Show(
                    Localization.MessageFormats.FunctionNameInvalidDotAtEnd,
                    Localization.Promptu.AppName,
                    UIMessageBoxButtons.OK,
                    UIMessageBoxIcon.Error,
                    UIMessageBoxResult.OK);
            }
            else
            {
                try
                {
                    Function.ValidateName(this.NativeInterface.Name.Text);
                }
                catch (ArgumentException ex)
                {
                    UIMessageBox.Show(
                    ex.Message,
                    Localization.Promptu.AppName,
                    UIMessageBoxButtons.OK,
                    UIMessageBoxIcon.Error,
                    UIMessageBoxResult.OK);

                    return;
                }

                this.NativeInterface.CloseWithOK();
                //this.DialogResult = DialogResult.OK;
            }
        }
    }
}
