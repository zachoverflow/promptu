using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using ZachJohnson.Promptu.UIModel.Interfaces;
using ZachJohnson.Promptu.UIModel;

namespace ZachJohnson.Promptu.WpfUI.UIComponents
{
    internal class TextValidationRule : ValidationRule
    {
        private PromptuTextBox owner;

        public TextValidationRule(PromptuTextBox owner)
        {
            this.owner = owner;
        }

        public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
        {
            Validator<string> validator = owner.TextValidator;
            if (!this.owner.CueDisplayed && validator != null)
            {
                ValueValidationResult result = validator(value.ToString());
                return new ValidationResult(result.IsValid, result.ErrorMessage);
            }

            return ValidationResult.ValidResult;
        }
    }
}
