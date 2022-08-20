using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Globalization;

namespace ZachJohnson.Promptu.UserModel
{
    internal class CommandParameterMetaInfo
    {
        // TODO fix this for turkish locale
        private static Regex numberOrRangeRegex = new Regex("^(?:(?<first>(?:\\d+|[nN]))|(?<first>\\d+)\\-(?<last>(?:\\d+|[nN])))?$");
        private string display;
        private int firstParameter;
        private int? lastParameter;
        private bool isRange;
        private string description;
        private ParameterSuggestion parameterSuggestion;
        private bool showHistory;

        public CommandParameterMetaInfo(string display, int firstParameter, bool isRange, int? lastParameter, string description, ParameterSuggestion parameterSuggestion, bool showHistory)
        {
            this.isRange = isRange;
            this.firstParameter = firstParameter;
            this.lastParameter = lastParameter;
            this.description = description;
            this.parameterSuggestion = parameterSuggestion;
            this.showHistory = showHistory;

            if (display == null)
            {
                this.display = this.GetParameterDisplay();
            }
            else
            {
                this.display = display;
            }
        }

        public string Display
        {
            get { return this.display; }
        }

        public int FirstParameter
        {
            get { return this.firstParameter; }
        }

        public bool IsRange
        {
            get { return this.isRange; }
        }

        public int? LastParameter
        {
            get { return this.lastParameter; }
        }

        public string Description
        {
            get { return this.description; }
        }

        public ParameterSuggestion ParameterSuggestion
        {
            get { return this.parameterSuggestion; }
        }

        public bool ShowHistory
        {
            get { return this.showHistory; }
        }

        public CommandParameterMetaInfo Clone()
        {
            return new CommandParameterMetaInfo(this.display, this.firstParameter, this.isRange, this.lastParameter, this.description, this.parameterSuggestion == null ? null : this.parameterSuggestion.Clone(), this.showHistory);
        }

        public bool Encompasses(int parameterNumber)
        {
            if (!this.IsRange)
            {
                return parameterNumber == this.firstParameter;
            }
            else
            {
                return parameterNumber >= this.firstParameter && (this.lastParameter == null || parameterNumber <= this.lastParameter.Value);
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

            node.Attributes.Append(XmlUtilities.CreateAttribute("parameter", this.display, document));

            if (!String.IsNullOrEmpty(this.Description))
            {
                node.Attributes.Append(XmlUtilities.CreateAttribute("description", this.Description, document));
            }

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

            builder.AppendFormat("[{0}] Suggestion: ", this.Display);

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
                    builder.Append(" (No Filter);");
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

            if (!String.IsNullOrEmpty(this.description))
            {
                builder.AppendLine();
                builder.AppendFormat("\tDescription: {0}", this.Description);
            }

            return builder.ToString();
        }

        public int CompareTo(CommandParameterMetaInfo info)
        {
            return Compare(this, info);
        }

        public static int Compare(CommandParameterMetaInfo item1, CommandParameterMetaInfo item2)
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

            int result = item1.Display.CompareTo(item2.Display);

            if (result != 0)
            {
                return result;
            }

            result = item1.Description.CompareTo(item2.Description);

            if (result != 0)
            {
                return result;
            }

            result = ParameterSuggestion.Compare(item1.ParameterSuggestion, item2.ParameterSuggestion);

            if (result != 0)
            {
                return result;
            }

            return item1.ShowHistory.CompareTo(item2.ShowHistory);
        }

        public static CommandParameterMetaInfo FromXml(XmlNode node)
        {
            if (node == null)
            {
                throw new ArgumentNullException("node");
            }

            int? firstParameter = null;
            bool isRange = false;
            int? lastParameter = null;
            string description = String.Empty;
            string display = null;
            ParameterSuggestion suggestion = null;
            bool showHistory = false;

            foreach (XmlAttribute attribute in node.Attributes)
            {
                switch (attribute.Name.ToUpperInvariant())
                {
                    case "PARAMETER":
                        try
                        {
                            int firstNumber;
                            int? lastNumber;
                            bool valueIsRange;
                            CommandParameterMetaInfo.ConvertParameterNumberOrRange(attribute.Value, out firstNumber, out lastNumber, out valueIsRange);
                            firstParameter = firstNumber;
                            lastParameter = lastNumber;
                            isRange = valueIsRange;
                            display = attribute.Value;
                        }
                        catch (FormatException)
                        {
                        }
                        catch (ArgumentException)
                        {
                        }

                        break;
                    case "DESCRIPTION":
                        description = attribute.Value;
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

            if (firstParameter == null)
            {
                throw new LoadException("Missing or invalid 'parameter' attribute.");
            }

            return new CommandParameterMetaInfo(display, firstParameter.Value, isRange, lastParameter, description, suggestion, showHistory);
        }

        public static void ConvertParameterNumberOrRange(string numberOrRange, out int firstParameter, out int? lastParameter, out bool isRange)
        {
            Match match = numberOrRangeRegex.Match(numberOrRange);

            if (!match.Success)
            {
                throw new FormatException("The number or range must be in the format: \"[number] or 'n'\" or \"[number]-[number or 'n']\".");
            }

            Group lastNumberGroup = match.Groups["last"];

            string firstNumberOrN = match.Groups["first"].Value;

            if (firstNumberOrN.ToUpperInvariant() == "N")
            {
                firstParameter = 1;
                lastParameter = null;
                isRange = true;
                return;
            }
            else
            {
                try
                {
                    firstParameter = Convert.ToInt32(firstNumberOrN, CultureInfo.InvariantCulture);
                }
                catch (FormatException)
                {
                    throw new FormatException("The number or range must be in the format: \"[number] or 'n'\" or \"[number]-[number or 'n']\".");
                }
                catch (OverflowException)
                {
                    if (lastNumberGroup.Success)
                    {
                        throw new ArgumentException("The first number in the range is too big.");
                    }
                    else
                    {
                        throw new ArgumentException("The number is too big.");
                    }
                }
            }

            if (firstParameter <= 0)
            {
                if (lastNumberGroup.Success)
                {
                    throw new ArgumentException("The first number must be greater than zero.");
                }
                else
                {
                    throw new ArgumentException("The number must be greater than zero.");
                }
            }

            if (lastNumberGroup.Success)
            {
                isRange = true;

                if (lastNumberGroup.Value.ToUpperInvariant() == "N")
                {
                    lastParameter = null;
                }
                else
                {
                    try
                    {
                        lastParameter = Convert.ToInt32(lastNumberGroup.Value, CultureInfo.InvariantCulture);
                    }
                    catch (FormatException)
                    {
                        throw new FormatException("The number or range must be in the format: \"[number] or 'n'\" or \"[number]-[number or 'n']\".");
                    }
                    catch (OverflowException)
                    {
                        throw new ArgumentException("The last number in the range is too big.");
                    }

                    if (lastParameter <= firstParameter)
                    {
                        throw new ArgumentException("The last number must be greater than the first number.");
                    }
                }
            }
            else
            {
                isRange = false;
                lastParameter = null;
            }
        }

        public string GetParameterDisplay()
        {
            if (this.IsRange)
            {
                return String.Format(CultureInfo.InvariantCulture, "{0}-{1}", this.firstParameter.ToString(CultureInfo.InvariantCulture), this.lastParameter == null ? "n" : this.lastParameter.Value.ToString(CultureInfo.InvariantCulture));
            }
            else
            {
                return this.firstParameter.ToString(CultureInfo.InvariantCulture);
            }
        }
    }
}
