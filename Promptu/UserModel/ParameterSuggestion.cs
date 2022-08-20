using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace ZachJohnson.Promptu.UserModel
{
    internal abstract class ParameterSuggestion
    {
        public ParameterSuggestion()
        {
        }

        public abstract ParameterSuggestion Clone();

        public static ParameterSuggestion FromXml(XmlNode node)
        {
            if (node == null)
            {
                throw new ArgumentNullException("node");
            }

            foreach (XmlAttribute attribute in node.Attributes)
            {
                switch (attribute.Name.ToUpperInvariant())
                {
                    case "TYPE":
                        switch (attribute.Value)
                        {
                            case ValueListParameterSuggestion.TypeAttributeValue:
                                return ValueListParameterSuggestion.FromXml(node);
                            case FunctionReturnParameterSuggestion.TypeAttributeValue:
                                return FunctionReturnParameterSuggestion.FromXml(node);
                            case FileSystemParameterSuggestion.TypeAttributeValue:
                                return FileSystemParameterSuggestion.FromXml(node);
                            default:
                                throw new LoadException("Unknown \"type\" attribute value.");
                        }
                }
            }

            throw new LoadException("Missing \"type\" attribute.");
        }

        public static string ResolvePath(string text)
        {
            int indexOfFirstDirectorySeparatorChar = text.IndexOf(System.IO.Path.DirectorySeparatorChar);

            if (indexOfFirstDirectorySeparatorChar < 0)
            {
                indexOfFirstDirectorySeparatorChar = text.Length;
            }

            if (indexOfFirstDirectorySeparatorChar > -1)
            {
                string beginningFolderName = text.Substring(0, indexOfFirstDirectorySeparatorChar);

                bool found;
                GroupedCompositeItem groupedCompositeItem = InternalGlobals.CurrentProfile.CompositeFunctionsAndCommands.TryGetItem(beginningFolderName, out found);
                CompositeItem<Command, List> parameterlessCommandNamedLikeFirstFolder;
                if (found && groupedCompositeItem != null && (parameterlessCommandNamedLikeFirstFolder = groupedCompositeItem.TryGetCommand(beginningFolderName, 0)) != null)
                {
                    try
                    {
                        ExecutionData executionData = new ExecutionData(
                            new string[0],
                            parameterlessCommandNamedLikeFirstFolder.ListFrom,
                            InternalGlobals.CurrentProfile.Lists);
                        FileSystemDirectory proposedDirectory = parameterlessCommandNamedLikeFirstFolder.Item.GetSubstitutedExecutionPath(executionData);
                        if (proposedDirectory.LooksValid)
                        {
                            return proposedDirectory + text.Substring(indexOfFirstDirectorySeparatorChar);
                        }
                    }
                    catch (ParseException)
                    {
                    }
                    catch (ConversionException)
                    {
                    }
                    catch (SelfReferencingCommandException)
                    {
                    }
                }
            }

            return text;
        }

        public static string TranslateArgument(string argument, List listFrom, ParameterSuggestion parameterSuggestion)
        {
            if (parameterSuggestion != null)
            {
                if (parameterSuggestion is FileSystemParameterSuggestion)
                {
                    return ResolvePath(argument);
                }
                else
                {
                    string setValue = argument;
                    ValueListParameterSuggestion valueListParameterSuggestion = parameterSuggestion as ValueListParameterSuggestion;
                    FunctionReturnParameterSuggestion functionReturnParameterSuggestion;
                    ValueList valueList = null;
                    if (listFrom != null)
                    {
                        if (valueListParameterSuggestion != null)
                        {
                            valueList = listFrom.ValueLists.TryGet(valueListParameterSuggestion.ValueListName);
                        }
                        else if ((functionReturnParameterSuggestion = parameterSuggestion as FunctionReturnParameterSuggestion) != null)
                        {
                            valueList = functionReturnParameterSuggestion.TryCompile(listFrom);
                        }

                        if (valueList != null)
                        {
                            if (valueList.UseItemTranslations && valueList.ContainsValue(argument))
                            {
                                ValueListItem item = valueList[argument];
                                if (item.AllowTranslation)
                                {
                                    setValue = item.Translation;
                                }
                            }
                        }
                    }

                    return setValue;
                }
            }

            return argument;
        }

        public int CompareTo(ParameterSuggestion item)
        {
            return Compare(this, item);
        }

        public static int Compare(ParameterSuggestion x, ParameterSuggestion y)
        {
            int result = 0;

            if (x == y)
            {
                result = 0;
            }
            else
            {
                ValueListParameterSuggestion item1ValueListParameterSuggestion = x as ValueListParameterSuggestion;
                ValueListParameterSuggestion item2ValueListParameterSuggestion = y as ValueListParameterSuggestion;

                if (item1ValueListParameterSuggestion != null)
                {
                    if (item2ValueListParameterSuggestion != null)
                    {
                        result = item2ValueListParameterSuggestion.ValueListName.CompareTo(item1ValueListParameterSuggestion.ValueListName);
                    }
                    else
                    {
                        result = 1;
                    }
                }
                else if (item2ValueListParameterSuggestion != null)
                {
                    result = 1;
                }

                FunctionReturnParameterSuggestion item1FunctionReturnParameterSuggestion = x as FunctionReturnParameterSuggestion;
                FunctionReturnParameterSuggestion item2FunctionReturnParameterSuggestion = y as FunctionReturnParameterSuggestion;

                if (item1FunctionReturnParameterSuggestion != null)
                {
                    if (item2FunctionReturnParameterSuggestion != null)
                    {
                        result = item2FunctionReturnParameterSuggestion.Expression.CompareTo(item1FunctionReturnParameterSuggestion.Expression);
                    }
                    else
                    {
                        result = 1;
                    }
                }
                else if (item2FunctionReturnParameterSuggestion != null)
                {
                    result = 1;
                }

                FileSystemParameterSuggestion item1FileSystemParameterSuggestion = x as FileSystemParameterSuggestion;
                FileSystemParameterSuggestion item2FileSystemParameterSuggestion = y as FileSystemParameterSuggestion;

                if (item1FileSystemParameterSuggestion != null)
                {
                    if (item2FileSystemParameterSuggestion != null)
                    {
                        if (item1FileSystemParameterSuggestion.Filter == null)
                        {
                            if (item2FileSystemParameterSuggestion.Filter == null)
                            {
                                return 0;
                            }
                            else
                            {
                                return 1;
                            }
                        }
                        else if (item2FileSystemParameterSuggestion.Filter == null)
                        {
                            return -1;
                        }

                        result = item2FileSystemParameterSuggestion.Filter.CompareTo(item1FileSystemParameterSuggestion.Filter);
                    }
                    else
                    {
                        result = 1;
                    }
                }
                else if (item2FileSystemParameterSuggestion != null)
                {
                    result = 1;
                }
            }

            return result;
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

            return this.ToXmlCore(name, document);
        }

        protected abstract XmlNode ToXmlCore(string name, XmlDocument document);
    }
}
