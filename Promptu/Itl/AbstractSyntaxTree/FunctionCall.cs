//-----------------------------------------------------------------------
// <copyright file="FunctionCall.cs" company="ZachJohnson">
//     Copyright (c) Zach Johnson. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace ZachJohnson.Promptu.Itl.AbstractSyntaxTree
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Text;
    using ZachJohnson.Promptu.UserModel;
    using ZachJohnson.Promptu.UserModel.Collections;

    internal class FunctionCall : Expression
    {
        private Identifier identifier;
        private List<Expression> parameters;

        public FunctionCall(Identifier identifier, List<Expression> parameters)
        {
            if (identifier == null)
            {
                throw new ArgumentNullException("identifier");
            }
            else if (parameters == null)
            {
                throw new ArgumentNullException("parameters");
            }

            this.identifier = identifier;
            this.parameters = parameters;
        }

        public Identifier Identifier
        {
            get { return this.identifier; }
        }

        public List<Expression> Parameters
        {
            get { return this.parameters; }
        }

        public string GetParameterSignature()
        {
            return Function.CreateAllStringParameterSignature(this.parameters.Count);
        }

        public object ConvertToObject(ExecutionData data, ReturnValue? allowedReturnValues)
        {
            string parameterSignature = this.GetParameterSignature();
            if (!data.PrioritizedCompositeFunctions.ContainsAnyNamed(this.Identifier.Name, null))
            {
                throw new ConversionException(String.Format(CultureInfo.CurrentCulture, "Missing function: {0}.", this.Identifier.Name));
            }
            else if (!data.PrioritizedCompositeFunctions.Contains(this.Identifier.Name, allowedReturnValues, parameterSignature))
            {
                StringBuilder allowedSuffix = new StringBuilder();

                if (allowedReturnValues != null)
                {
                    List<string> allowedValues = new List<string>();

                    if ((allowedReturnValues.Value & ReturnValue.String) != 0)
                    {
                        allowedValues.Add("'String'");
                    }

                    if ((allowedReturnValues.Value & ReturnValue.StringArray) != 0)
                    {
                        allowedValues.Add("'String[]'");
                    }

                    if ((allowedReturnValues.Value & ReturnValue.ValueList) != 0)
                    {
                        allowedValues.Add("'ValueList'");
                    }

                    if (allowedValues.Count >= 0)
                    {
                        allowedSuffix.Append(" and returns a ");

                        for (int i = 0; i < allowedValues.Count; i++)
                        {
                            if (i > 0)
                            {
                                if (i >= allowedValues.Count - 1)
                                {
                                    if (i > 1)
                                    {
                                        allowedSuffix.Append(",");
                                    }

                                    allowedSuffix.Append(" or ");
                                }
                                else
                                {
                                    allowedSuffix.Append(", ");
                                }
                            }

                            allowedSuffix.Append(allowedValues[i]);
                        }
                    }
                }

                string parametersFormat;
                if (this.parameters.Count == 1)
                {
                    parametersFormat = Localization.MessageFormats.StringParameterFormatSingular;
                }
                else
                {
                    parametersFormat = Localization.MessageFormats.StringParameterFormatPlural;
                }

                throw new ConversionException(
                    String.Format(
                    CultureInfo.CurrentCulture,
                    "No function named '{0}' has {1}{2}.",
                    this.Identifier.Name,
                    String.Format(CultureInfo.CurrentCulture, parametersFormat, this.parameters.Count),
                    allowedSuffix));
            }

            try
            {
                List<string> parameters = new List<string>();
                foreach (Expression parameter in this.parameters)
                {
                    parameters.Add(parameter.ConvertToString(data));
                }

                CompositeItem<Function, List> foundFunction = data.PrioritizedCompositeFunctions[this.Identifier.Name, allowedReturnValues, parameterSignature];

                return foundFunction.Item.Invoke(
                    parameters.ToArray(),
                    data, 
                    new AssemblyReferenceCollectionComposite(data.PrioritizedCompositeFunctions.Lists, foundFunction.ListFrom));
            }
            catch (Exception ex)
            {
                if (ex is ConversionException)
                {
                    throw;
                }

                throw new ConversionException(ex.Message);
            }
        }

        public override string ConvertToString(ExecutionData data)
        {
            object converted = this.ConvertToObject(data, ReturnValue.String);

            if (converted != null)
            {
                return converted.ToString();
            }

            return String.Empty;
        }

        public ValueList ConvertToValueList(ExecutionData data)
        {
            object converted = this.ConvertToObject(data, ReturnValue.StringArray | ReturnValue.ValueList);

            if (converted != null)
            {
                ValueList valueList = converted as ValueList;
                if (valueList != null)
                {
                    return valueList;
                }
                else
                {
                    string[] array = converted as string[];
                    if (array != null)
                    {
                        valueList = new ValueList();
                        foreach (string element in array)
                        {
                            valueList.Add(new ValueListItem(element, null, false, -1));
                        }

                        return valueList;
                    }
                }
            }

            return new ValueList();
        }
    }
}
