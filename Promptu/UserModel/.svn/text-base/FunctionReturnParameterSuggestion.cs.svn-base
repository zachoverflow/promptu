using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using ZachJohnson.Promptu.Itl.AbstractSyntaxTree;
using ZachJohnson.Promptu.Itl;
using System.Globalization;

namespace ZachJohnson.Promptu.UserModel
{
    internal class FunctionReturnParameterSuggestion : ParameterSuggestion
    {
        public const string TypeAttributeValue = "FunctionReturn";
        private string expression;

        public FunctionReturnParameterSuggestion(string expression)
        {
            if (expression == null)
            {
                throw new ArgumentNullException("expression");
            }

            this.expression = expression;
        }

        public string Expression
        {
            get { return this.expression; }
        }

        public static string FormatExpression(string expression)
        {
            if (expression == null)
            {
                throw new ArgumentNullException("expression");
            }

            return String.Format(CultureInfo.InvariantCulture, "<{0}>", expression);
        }

        public override ParameterSuggestion Clone()
        {
            return new FunctionReturnParameterSuggestion(this.expression);
        }

        public FunctionCall TryCompile()
        {
            ItlCompiler compiler = new ItlCompiler();
            FeedbackCollection feedback;
            Expression expressionObject = compiler.Compile(ItlType.SingleFunction, FormatExpression(this.Expression), out feedback);

            if (!feedback.Has(FeedbackType.Error))
            {
                ExpressionGroup group = expressionObject as ExpressionGroup;
                if (group != null)
                {
                    group = group.Expressions[0] as ExpressionGroup;
                    if (group != null)
                    {
                        FunctionCall functionCall = null;

                        foreach (Expression innerExpression in group.Expressions)
                        {
                            functionCall = innerExpression as FunctionCall;
                            if (functionCall != null)
                            {
                                break;
                            }
                        }

                        if (functionCall != null)
                        {
                            return functionCall;
                        }
                    }
                }
            }

            return null;
        }

        public ValueList TryCompile(List priorityList)
        {
            ItlCompiler compiler = new ItlCompiler();
            FeedbackCollection feedback;
            Expression expressionObject = compiler.Compile(ItlType.SingleFunction, FormatExpression(this.Expression), out feedback);

            if (!feedback.Has(FeedbackType.Error))
            {
                ExpressionGroup group = expressionObject as ExpressionGroup;
                if (group != null)
                {
                    group = group.Expressions[0] as ExpressionGroup;
                    if (group != null)
                    {
                        FunctionCall functionCall = null;

                        foreach (Expression innerExpression in group.Expressions)
                        {
                            functionCall = innerExpression as FunctionCall;
                            if (functionCall != null)
                            {
                                break;
                            }
                        }

                        if (functionCall != null)
                        {
                            try
                            {
                                ExecutionData data = new ExecutionData(new string[0], priorityList, InternalGlobals.CurrentProfile.Lists);
                                return functionCall.ConvertToValueList(data);
                            }
                            catch (ConversionException)
                            {
                            }
                        }
                    }
                }
            }

            return null;
        }

        protected override System.Xml.XmlNode ToXmlCore(string name, System.Xml.XmlDocument document)
        {
            XmlNode node = document.CreateElement(name);

            node.Attributes.Append(XmlUtilities.CreateAttribute("type", TypeAttributeValue, document));
            node.Attributes.Append(XmlUtilities.CreateAttribute("expression", this.expression, document));

            return node;
        }

        public static new FunctionReturnParameterSuggestion FromXml(XmlNode node)
        {
            if (node == null)
            {
                throw new ArgumentNullException("node");
            }

            string expression = null;

            foreach (XmlAttribute attribute in node.Attributes)
            {
                switch (attribute.Name.ToUpperInvariant())
                {
                    case "EXPRESSION":
                        expression = attribute.Value;
                        break;
                    default:
                        break;
                }
            }

            if (expression == null)
            {
                throw new LoadException("Missing 'expression' attribute.");
            }

            return new FunctionReturnParameterSuggestion(expression);
        }
    }
}
