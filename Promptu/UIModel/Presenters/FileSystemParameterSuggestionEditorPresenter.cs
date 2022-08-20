using System;
using System.Collections.Generic;
using System.Text;
using ZachJohnson.Promptu.UIModel.Interfaces;
using ZachJohnson.Promptu.UserModel.Collections;
using ZachJohnson.Promptu.UserModel;
using ZachJohnson.Promptu.Itl.AbstractSyntaxTree;
using ZachJohnson.Promptu.Itl;
using System.Globalization;

namespace ZachJohnson.Promptu.UIModel.Presenters
{
    internal class FileSystemParameterSuggestionEditorPresenter : DialogPresenterBase<IFileSystemParameterSuggestionEditor>
    {
        private const int SecondsToValidationAfterLastChange = 2;
        private ValidationManager validationManager;
        private FunctionCollectionComposite prioritizedFunctions;
        private int parameterNumber;
        private ErrorPanelPresenter errorPanel;

        public FileSystemParameterSuggestionEditorPresenter(
            string filter,
            FunctionCollectionComposite prioritizedFunctions,
            int parameterNumber)
            : this(
            InternalGlobals.GuiManager.ToolkitHost.Factory.ConstructFileSystemParameterSuggestionEditor(),
            filter,
            prioritizedFunctions,
            parameterNumber)
        {
        }

        public FileSystemParameterSuggestionEditorPresenter(
            IFileSystemParameterSuggestionEditor nativeInterface,
            string filter,
            FunctionCollectionComposite prioritizedFunctions,
            int parameterNumber)
            : base(nativeInterface)
        {
            this.NativeInterface.CancelButton.Text = Localization.UIResources.CancelButtonText;
            this.NativeInterface.OkButton.Text = Localization.UIResources.OkButtonText;
            this.NativeInterface.MainInstructions = Localization.UIResources.FileSystemParameterMainInstructions;
            this.NativeInterface.FilterSupplementalInstructions = Localization.UIResources.MultipleFilterHint;
            this.NativeInterface.Text = Localization.UIResources.FileSystemSuggestionEditorText;

            this.errorPanel = new ErrorPanelPresenter(this.NativeInterface.ErrorPanel);

            this.NativeInterface.Filter.Text = filter;
            this.parameterNumber = parameterNumber;

            this.validationManager = new ValidationManager(SecondsToValidationAfterLastChange);
            this.validationManager.TimeToValidate += this.ValidateItl;

            this.prioritizedFunctions = prioritizedFunctions;

            this.ValidateItl();

            this.errorPanel.MessageSelected += this.HandleMessageSelected;

            this.NativeInterface.Filter.TextChanged += this.HandleFilterTextChanged;

            //this.NativeInterface.Filter.Select();
        }

        public string Filter
        {
            get { return this.NativeInterface.Filter.Text; }
        }

        private void HandleFilterTextChanged(object sender, EventArgs e)
        {
            this.validationManager.NotifyChangeHappened();
        }

        public void ValidateExpression(Expression expression, FeedbackCollection feedback)
        {
            ExpressionGroup group;
            FunctionCall functionCall;
            ArgumentSubstitution argumentSubstitution;
            if ((group = expression as ExpressionGroup) != null)
            {
                foreach (Expression innerExpression in group.Expressions)
                {
                    this.ValidateExpression(innerExpression, feedback);
                }
            }
            else if ((functionCall = expression as FunctionCall) != null)
            {
                if (!this.prioritizedFunctions.ContainsAnyNamed(functionCall.Identifier.Name, ReturnValue.String))
                {
                    feedback.AddError(String.Format(CultureInfo.CurrentCulture, Localization.MessageFormats.MissingStringFunction, functionCall.Identifier.Name));
                }
                else if (!this.prioritizedFunctions.Contains(functionCall.Identifier.Name, ReturnValue.String, functionCall.GetParameterSignature()))
                {
                    if (functionCall.Parameters.Count == 1)
                    {
                        feedback.AddError(String.Format(CultureInfo.CurrentCulture, Localization.MessageFormats.InvalidParameterCountSingular, functionCall.Identifier.Name, functionCall.Parameters.Count));
                    }
                    else
                    {
                        feedback.AddError(String.Format(CultureInfo.CurrentCulture, Localization.MessageFormats.InvalidParameterCountPlural, functionCall.Identifier.Name, functionCall.Parameters.Count));
                    }
                }

                foreach (Expression innerExpression in functionCall.Parameters)
                {
                    this.ValidateExpression(innerExpression, feedback);
                }
            }
            else if ((argumentSubstitution = expression as ArgumentSubstitution) != null)
            {
                bool optional = false;
                if (expression is OptionalSubsitution)
                {
                    optional = true;
                    OptionalSubsitution substitution = expression as OptionalSubsitution;

                    if (substitution.DefaultValue != null)
                    {
                        this.ValidateExpression(substitution.DefaultValue, feedback);
                    }
                }

                if (this.parameterNumber == 1)
                {
                    feedback.Add(Localization.MessageFormats.ArgumentSubstitutionsCannotCaptureArguments, optional ? FeedbackType.Warning : FeedbackType.Error);
                    return;
                }

                if (argumentSubstitution.ArgumentNumber != null)
                {
                    if (argumentSubstitution.ArgumentNumber.Value >= this.parameterNumber)
                    {
                        if (!optional)
                        {
                            feedback.AddError(String.Format(CultureInfo.CurrentCulture, Localization.MessageFormats.CannotUseParametersGreaterThanOrEqualTo, this.parameterNumber));
                            return;
                        }
                    }

                    if (!argumentSubstitution.SingularSubstitution)
                    {
                        if (argumentSubstitution.LastArgumentNumber != null && argumentSubstitution.LastArgumentNumber.Value >= this.parameterNumber)
                        {
                            if (!optional)
                            {
                                feedback.AddError(String.Format(CultureInfo.CurrentCulture, Localization.MessageFormats.CannotUseParametersGreaterThanOrEqualTo, this.parameterNumber));
                                return;
                            }
                        }
                    }
                }
            }
        }

        private void HandleMessageSelected(object sender, MessageSelectedEventArgs e)
        {
            if (e.Message.CanLocate)
            {
                this.NativeInterface.Filter.SelectionStart = e.Message.Start;
                this.NativeInterface.Filter.SelectionLength = e.Message.Length;
                this.NativeInterface.Filter.Select();
            }
        }

        private void ValidateItl(object sender, EventArgs e)
        {
            this.ValidateItl();
        }

        private bool ValidateItl()
        {
            ItlCompiler compiler = new ItlCompiler();

            FeedbackCollection feedback = null;

            bool justText = true;

            if (this.NativeInterface.Filter.Text.Length > 0)
            {
                Expression expression = compiler.Compile(ItlType.Standard, this.NativeInterface.Filter.Text, out feedback, out justText);
                this.ValidateExpression(expression, feedback);
            }

            FeedbackCollection totalFeedback = new FeedbackCollection();

            if (feedback != null)
            {
                foreach (FeedbackMessage message in feedback)
                {
                    message.Location = Localization.UIResources.FileSystemSuggestionEditorFilterLocation;
                    totalFeedback.Add(message);
                }
            }

            bool hasErrors = totalFeedback.Has(FeedbackType.Error);

            if (this.NativeInterface.IsCreatedAndNotDisposing)
            {
                try
                {
                    this.NativeInterface.Invoke(new ParameterlessVoid(delegate
                    {
                        this.errorPanel.SetMessages(totalFeedback, false);
                    }),
                    null);
                }
                catch (ObjectDisposedException)
                {
                }
            }

            return !hasErrors;
        }
    }
}
