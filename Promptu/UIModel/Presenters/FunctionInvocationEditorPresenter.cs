using System;
using System.Collections.Generic;
using System.Text;
using ZachJohnson.Promptu.UIModel.Interfaces;
using ZachJohnson.Promptu.UserModel.Collections;
using ZachJohnson.Promptu.UI;
using ZachJohnson.Promptu.Itl.AbstractSyntaxTree;
using ZachJohnson.Promptu.UserModel;
using ZachJohnson.Promptu.Itl;
using System.Globalization;

namespace ZachJohnson.Promptu.UIModel.Presenters
{
    internal class FunctionInvocationEditorPresenter : DialogPresenterBase<IFunctionInvocationEditor>
    {
        private const int SecondsToValidationAfterLastChange = 2;
        private ValidationManager validationManager;
        private FunctionCollectionComposite prioritizedFunctions;
        private int parameterNumber;
        private ErrorPanelPresenter errorPanel;

        public FunctionInvocationEditorPresenter(
            string currentInvocation,
            FunctionCollectionComposite prioritizedFunctions,
            int parameterNumber)
            : this(
            InternalGlobals.GuiManager.ToolkitHost.Factory.ConstructFunctionInvocationEditor(),
            currentInvocation,
            prioritizedFunctions,
            parameterNumber)
        {
        }

        public FunctionInvocationEditorPresenter(
            IFunctionInvocationEditor nativeInterface,
            string currentInvocation,
            FunctionCollectionComposite prioritizedFunctions,
            int parameterNumber)
            : base(nativeInterface)
        {
            this.parameterNumber = parameterNumber;

            this.NativeInterface.Text = Localization.UIResources.FunctionInvocationEditorText;
            this.NativeInterface.MainInstructions = Localization.UIResources.FunctionInvocationEditorMessage;
            this.NativeInterface.OkButton.Text = Localization.UIResources.OkButtonText;
            this.NativeInterface.CancelButton.Text = Localization.UIResources.CancelButtonText;

            this.errorPanel = new ErrorPanelPresenter(this.NativeInterface.ErrorPanel);

            this.validationManager = new ValidationManager(SecondsToValidationAfterLastChange);
            this.validationManager.TimeToValidate += this.ValidateItl;

            this.prioritizedFunctions = prioritizedFunctions;

            this.NativeInterface.Expression.Text = currentInvocation;

            this.errorPanel.MessageSelected += this.HandleMessageSelected;

            this.UpdateOkButtonEnabled();
            this.ValidateItl();

            this.NativeInterface.Expression.TextChanged += this.HandleExpressionTextChanged;

            this.NativeInterface.Expression.Select();
        }

        public string Expression
        {
            get { return this.NativeInterface.Expression.Text; }
        }

        public void ValidateExpression(Expression expression, FeedbackCollection feedback, bool withinFunctionCall)
        {
            ExpressionGroup group;
            FunctionCall functionCall;
            ArgumentSubstitution argumentSubstitution;
            if ((group = expression as ExpressionGroup) != null)
            {
                foreach (Expression innerExpression in group.Expressions)
                {
                    this.ValidateExpression(innerExpression, feedback, withinFunctionCall);
                }
            }
            else if ((functionCall = expression as FunctionCall) != null)
            {
                if (!withinFunctionCall)
                {
                    if (!this.prioritizedFunctions.ContainsAnyNamed(functionCall.Identifier.Name, ReturnValue.StringArray | ReturnValue.ValueList))
                    {
                        feedback.AddError(String.Format(CultureInfo.CurrentCulture, Localization.MessageFormats.MissingStringArrayOrValueListFunction, functionCall.Identifier.Name));
                    }
                    else if (!this.prioritizedFunctions.Contains(functionCall.Identifier.Name, ReturnValue.StringArray | ReturnValue.ValueList, functionCall.GetParameterSignature()))
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
                }
                else
                {
                    if (withinFunctionCall && !this.prioritizedFunctions.ContainsAnyNamed(functionCall.Identifier.Name, ReturnValue.String))
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
                }

                foreach (Expression innerExpression in functionCall.Parameters)
                {
                    this.ValidateExpression(innerExpression, feedback, true);
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
                        this.ValidateExpression(substitution.DefaultValue, feedback, withinFunctionCall);
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

        private void HandleExpressionTextChanged(object sender, EventArgs e)
        {
            this.validationManager.NotifyChangeHappened();
            this.UpdateOkButtonEnabled();
        }

        private void UpdateOkButtonEnabled()
        {
            this.NativeInterface.OkButton.Enabled = this.NativeInterface.Expression.Text.Length > 0;
        }

        private void HandleMessageSelected(object sender, MessageSelectedEventArgs e)
        {
            if (e.Message.CanLocate)
            {
                this.NativeInterface.Expression.SelectionStart = e.Message.Start;
                this.NativeInterface.Expression.SelectionLength = e.Message.Length;
                this.NativeInterface.Expression.Select();
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

            if (this.NativeInterface.Expression.Text.Length > 0)
            {
                Expression expression = compiler.Compile(
                    ItlType.SingleFunction, 
                    FunctionReturnParameterSuggestion.FormatExpression(this.NativeInterface.Expression.Text), 
                    out feedback, out justText);
                this.ValidateExpression(expression, feedback, false);
            }

            FeedbackCollection totalFeedback = new FeedbackCollection();

            if (feedback != null)
            {
                foreach (FeedbackMessage message in feedback)
                {
                    if (message.Description == String.Format(CultureInfo.CurrentCulture, Localization.ItlMessages.NotUnderstoodFormat, '>') && message.Start > this.NativeInterface.Expression.Text.Length)
                    {
                        continue;
                    }

                    message.Start--;
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
                    }), null);
                }
                catch (ObjectDisposedException)
                {
                }
            }

            return !hasErrors;
        }
    }
}
