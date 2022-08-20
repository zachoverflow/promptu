using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace ZachJohnson.Promptu.UserModel
{
    internal class FunctionParameter
    {
        private FunctionParameterValueType valueType;
        private ParameterSuggestion parameterSuggestion;
        private bool showHistory;

        public FunctionParameter(FunctionParameterValueType valueType, ParameterSuggestion parameterSuggestion, bool showHistory)
        {
            this.valueType = valueType;
            this.parameterSuggestion = parameterSuggestion;
            this.showHistory = showHistory;
        }

        public FunctionParameterValueType ValueType
        {
            get { return this.valueType; }
        }

        public ParameterSuggestion ParameterSuggestion
        {
            get { return this.parameterSuggestion; }
        }

        public bool ShowHistory
        {
            get { return this.showHistory; }
        }

        public string GetSignature()
        {
            return new string(((char)this.valueType), 1);
        }

        public FunctionParameter Clone()
        {
            return new FunctionParameter(this.ValueType, this.parameterSuggestion == null ? null : this.parameterSuggestion.Clone(), this.showHistory);
        }

        public int CompareTo(FunctionParameter item)
        {
            return Compare(this, item);
        }

        public static int Compare(FunctionParameter item1, FunctionParameter item2)
        {
            if (item1 == null)
            {
                if (item2 == null)
                {
                    return 0;
                }
                else
                {
                    return -1;
                }
            }
            else if (item2 == null)
            {
                return 1;
            }
            else if (item1 == item2)
            {
                return 0;
            }

            int result = ParameterSuggestion.Compare(item1.ParameterSuggestion, item2.ParameterSuggestion);

            if (result != 0)
            {
                return result;
            }
            else
            {
                return item1.ShowHistory.CompareTo(item2.ShowHistory);
            }
        }

        public XmlNode ToXml(string name, XmlDocument document)
        {
            if (name == null)
            {
                throw new ArgumentNullException("name");
            }
            else if (document == null)
            {
                throw new ArgumentNullException("document");
            }

            XmlNode node = document.CreateElement(name);

            node.Attributes.Append(XmlUtilities.CreateAttribute("valueType", this.ValueType.ToString(), document));
            if (this.showHistory)
            {
                node.Attributes.Append(XmlUtilities.CreateAttribute("showHistory", this.showHistory.ToString(), document));
            }

            if (this.ParameterSuggestion != null)
            {
                node.AppendChild(this.ParameterSuggestion.ToXml("Suggestion", document));
            }

            return node;
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();

            builder.Append("Suggestion: ");

            ValueListParameterSuggestion valueListParameterSuggestion;
            FunctionReturnParameterSuggestion functionReturnParameterSuggestion;
            FileSystemParameterSuggestion fileSystemParameterSuggestion;

            if ((fileSystemParameterSuggestion = this.parameterSuggestion as FileSystemParameterSuggestion) != null)
            {
                builder.Append("'File System'");
                if (!String.IsNullOrEmpty(fileSystemParameterSuggestion.Filter))
                {
                    builder.AppendFormat(" (Filter: '{0}');", fileSystemParameterSuggestion.Filter);
                }
                else
                {
                    builder.Append(";");
                }
            }
            else if ((valueListParameterSuggestion = this.parameterSuggestion as ValueListParameterSuggestion) != null)
            {
                builder.AppendFormat("'Value List ('{0}')';", valueListParameterSuggestion.ValueListName);
            }
            else if ((functionReturnParameterSuggestion = this.parameterSuggestion as FunctionReturnParameterSuggestion) != null)
            {
                builder.AppendFormat("'Function Return Value ('{0}')';", functionReturnParameterSuggestion.Expression);
            }
            else
            {
                builder.Append("'Default';");
            }

            builder.AppendFormat(" Show History: '{0}'", this.ShowHistory);

            return builder.ToString();
        }

        public static FunctionParameter FromXml(XmlNode node)
        {
            if (node == null)
            {
                throw new ArgumentNullException("node");
            }

            FunctionParameterValueType valueType = FunctionParameterValueType.String;
            ParameterSuggestion suggestion = null;
            bool showHistory = false;

            foreach (XmlAttribute attribute in node.Attributes)
            {
                switch (attribute.Name.ToUpperInvariant())
                {
                    case "VALUETYPE":
                        try
                        {
                            valueType = (FunctionParameterValueType)Enum.Parse(typeof(FunctionParameterValueType), attribute.Value);
                        }
                        catch (ArgumentException)
                        {
                        }

                        break;
                    case "SHOWHISTORY":
                        showHistory = Utilities.TryParseBoolean(attribute.Value, showHistory);
                        //try
                        //{
                        //    showHistory = Convert.ToBoolean(attribute.Value);
                        //}
                        //catch (FormatException)
                        //{
                        //}

                        break;
                    default:
                        break;
                }
            }

            foreach (XmlNode innerNode in node.ChildNodes)
            {
                switch (innerNode.Name.ToUpperInvariant())
                {
                    case "SUGGESTION":
                        try
                        {
                            suggestion = ParameterSuggestion.FromXml(innerNode);
                        }
                        catch (LoadException)
                        {
                        }

                        break;
                    default:
                        break;
                }
            }

            return new FunctionParameter(valueType, suggestion, showHistory);
        }
    }
}
