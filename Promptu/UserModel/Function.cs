using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Globalization;
using System.Xml;
using ZachJohnson.Promptu.UserModel;
using ZachJohnson.Promptu.UserModel.Differencing;
using ZachJohnson.Promptu.Collections;
using ZachJohnson.Promptu.UserModel.Collections;
using System.Extensions;
using System.IO;
using ZachJohnson.Promptu.Itl.AbstractSyntaxTree;
using ZachJohnson.Promptu.UIModel;
using System.ComponentModel;

namespace ZachJohnson.Promptu.UserModel
{
    class Function : IDiffable, INotifyPropertyChanged
    {
        internal const string XmlAlias = "function";
        private string name;
        private string invocationClass;
        private string methodName;
        private string assemblyReferenceName;
        private FunctionParameterCollection parameters;
        private Id id;
        private string parameterSignature;
        private string stringId;
        private ReturnValue returnValue;
        private bool isCustom;

        public Function(string name, string invocationClass, string methodName, string assemblyReferenceName, ReturnValue returnValue, FunctionParameterCollection parameters, Id id, bool isCustom)
        {
            this.parameters = parameters;
            this.name = name;
            this.invocationClass = invocationClass;
            this.methodName = methodName;
            this.assemblyReferenceName = assemblyReferenceName;
            this.id = id;
            this.returnValue = returnValue;
        }

        public ReturnValue ReturnValue
        {
            get { return this.returnValue; }
        }

        public Id Id
        {
            get { return this.id; }
            set { this.id = value; }
        }

        public string Name
        {
            get { return this.name; }
        }

        public string InvocationClass
        {
            get { return this.invocationClass; }
        }

        public string MethodName
        {
            get { return this.methodName; }
        }

        public string AssemblyReferenceName
        {
            get { return this.assemblyReferenceName; }
            set { this.assemblyReferenceName = value; }
        }

        public bool IsCustom
        {
            get { return this.isCustom; }
            set { this.isCustom = value; }
        }

        //public int NumberOfParameters
        //{
        //    get { return this.numberOfParameters; }
        //}
        public FunctionParameterCollection Parameters
        {
            get { return this.parameters; }
        }

        public String StringId
        {
            get
            {
                if (this.stringId == null)
                {
                    this.stringId = Function.CreateStringId(this.name, this.ParameterSignature);
                }

                return this.stringId;
            }
        }

        public ItemInfo GetItemInfo()
        {
            List<string> attributes = new List<string>();

            attributes.Add(String.Format(
                CultureInfo.CurrentCulture, 
                Localization.UIResources.FunctionDetailsAssemblyReferenceNameFormat,
                this.AssemblyReferenceName));

            attributes.Add(String.Format(
                CultureInfo.CurrentCulture, 
                Localization.UIResources.FunctionDetailsClassFormat,
                this.InvocationClass));

            attributes.Add(String.Format(
                CultureInfo.CurrentCulture, 
                Localization.UIResources.FunctionDetailsMethodFormat,
                this.MethodName));

            int parameterCount = this.Parameters.Count;

            if (parameterCount == 1)
            {
                attributes.Add(String.Format(
                    CultureInfo.CurrentCulture, 
                    Localization.UIResources.FunctionDetailsParameterCountSingular,
                    parameterCount));
            }
            else
            {
                attributes.Add(String.Format(
                    CultureInfo.CurrentCulture, 
                    Localization.UIResources.FunctionDetailsParameterCountPlural,
                    parameterCount));;
            }

            return new ItemInfo(this.Name, attributes);
        }

        //public FunctionDiff DoDiff(Function latestItem)
        //{
        //    return new FunctionDiff(this, latestItem);
        //}

        public string GetFormattedIdentifier()
        {
            string format;
            if (this.Parameters.Count == 1)
            {
                format = Localization.Promptu.FunctionIdentifierFormatSingular;
                //return string.Format(Localization.Promptu.
            }
            else
            {
                format = Localization.Promptu.FunctionIdentifierFormatPlural;
            }

            return String.Format(CultureInfo.CurrentCulture, format, this.name, this.ParameterSignature.Length);
        }

        public Function Clone()
        {
            return this.Clone(null);
        }

        public Function Clone(string renameName)
        {
            return new Function(renameName == null ? this.name : renameName, this.invocationClass, this.methodName, this.assemblyReferenceName, this.ReturnValue, this.Parameters.Clone(), this.id, this.isCustom);
        }

        public ValueListCollection GetValueListDependencies(ValueListCollection allValueLists)
        {
            ValueListCollection valueLists = new ValueListCollection();

            foreach (FunctionParameter parameter in this.Parameters)
            {
                ValueListParameterSuggestion suggestion = parameter.ParameterSuggestion as ValueListParameterSuggestion;
                if (suggestion != null && !String.IsNullOrEmpty(suggestion.ValueListName))
                {
                    ValueList valueList = allValueLists.TryGet(suggestion.ValueListName);
                    if (valueList != null)
                    {
                        valueLists.Add(valueList);
                    }
                }
            }

            return valueLists;
        }

        public FunctionCollection GetFunctionDependecies(FunctionCollectionComposite allFunctions)
        {
            List<FunctionCall> functionCalls = new List<FunctionCall>();

            FunctionCollection functions = new FunctionCollection();

            foreach (FunctionParameter parameter in this.Parameters)
            {
                FunctionReturnParameterSuggestion suggestion = parameter.ParameterSuggestion as FunctionReturnParameterSuggestion;

                if (suggestion != null)
                {
                    FunctionCall expression = suggestion.TryCompile();
                    if (expression != null)
                    {
                        string parameterSignature = expression.GetParameterSignature();
                        CompositeItem<Function, List> functionAndList = allFunctions.TryGet(expression.Identifier.Name, ReturnValue.StringArray | ReturnValue.ValueList, parameterSignature);
                        if (functionAndList != null)
                        {
                            //Function function = allFunctions[expression.Identifier.Name, ReturnValue.StringArray | ReturnValue.ValueList, parameterSignature].Item;
                            if (!functions.Contains(functionAndList.Item))
                            {
                                functions.Add(functionAndList.Item);
                            }
                        }

                        foreach (Expression parameterExpression in expression.Parameters)
                        {
                            Expression.GetFunctionCalls(parameterExpression, functionCalls);
                        }
                    }
                }
            }

            foreach (FunctionCall functionCall in functionCalls)
            {
                string parameterSignature = functionCall.GetParameterSignature();
                CompositeItem<Function, List> functionAndList = allFunctions.TryGet(functionCall.Identifier.Name, ReturnValue.String, parameterSignature);
                if (functionAndList != null)
                {
                    //Function function = allFunctions[functionCall.Identifier.Name, ReturnValue.String, parameterSignature].Item;
                    if (!functions.Contains(functionAndList.Item))
                    {
                        functions.Add(functionAndList.Item);
                    }
                }
            }

            return functions;
        }

        public static bool IsValidFunction(Function function)
        {
            if (function == null)
            {
                return false;
            }

            return Function.IsValidName(function.name)
                //&& function.numberOfParameters >= 0
                && !String.IsNullOrEmpty(function.assemblyReferenceName)
                && !String.IsNullOrEmpty(function.invocationClass)
                && !String.IsNullOrEmpty(function.methodName);
        }

        //private static bool IsValidCharForPosition(char c, int index, bool isLast)
        //{
        //    bool valid;
        //    if (index == 0)
        //    {
        //        valid = char.IsLetter(c) || c == '_';
        //        if (!valid)
        //        {
        //            return false;
        //        }
        //    }
        //    else
        //    {
        //        valid = char.IsLetterOrDigit(c) || c == '_';
        //        if (!valid && !isLast)
        //        {
        //            valid = c == '.';
        //        }
        //    }

        //    return valid;
        //}

        public static bool IsValidName(string name)
        {
            if (String.IsNullOrEmpty(name) || Command.ReservedNames.Contains(name.ToUpperInvariant()))
            {
                return false;
            }

            bool valid;
            for (int i = 0; i < name.Length; i++)
            {
                char c = name[i];
                if (i == 0)
                {
                    valid = char.IsLetter(c) || c == '_';
                    if (!valid)
                    {
                        return false;
                    }
                }
                else
                {
                    valid = char.IsLetterOrDigit(c) || c == '_';
                    if (!valid && i < name.Length - 1)
                    {
                        valid = c == '.';
                    }

                    if (!valid)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        public static void ValidateName(string name)
        {
            if (name == null)
            {
                throw new ArgumentNullException("name");
            }
            if (name.Length <= 0)
            {
                throw new ArgumentException("Name cannot be empty.");
            }
            else if (Command.ReservedNames.Contains(name.ToUpperInvariant()))
            {
                //StringBuilder reservedNames = new StringBuilder();

                //for (int i = 0; i < Command.ReservedNames.Count; i++)
                //{
                //    if (i > 0)
                //    {
                //        if (i == Command.ReservedNames.Count - 1)
                //        {
                //            reservedNames.Append(Localization.Promptu.ReservedNamesCommandAndFormat);
                //        }
                //        else
                //        {
                //            reservedNames.Append(Localization.Promptu.ReservedNamesCommaFormat);
                //        }
                //    }
                    

                //    reservedNames.AppendFormat(Localization.Promptu.ReservedNameFormat, Command.ReservedNames[i]);
                //}

                throw new ArgumentException(String.Format(CultureInfo.InvariantCulture, Localization.ExceptionFormats.FunctionValidationNameIsReserved, name));
            }

            bool valid;
            for (int i = 0; i < name.Length; i++)
            {
                char c = name[i];
                if (i == 0)
                {
                    valid = char.IsLetter(c) || c == '_';
                    if (!valid)
                    {
                        throw new ArgumentException(Localization.ExceptionFormats.FunctionValidationFirstLetterInvalid);
                    }
                }
                else
                {
                    valid = char.IsLetterOrDigit(c) || c == '_';
                    if (!valid && i < name.Length - 1)
                    {
                        valid = c == '.';
                    }

                    if (!valid)
                    {
                        if (i < name.Length - 1)
                        {
                            throw new ArgumentException(String.Format(CultureInfo.CurrentCulture, Localization.ExceptionFormats.FunctionValidationMiddleLetterInvalid, c, i + 1));
                        }
                        else
                        {
                            throw new ArgumentException(Localization.ExceptionFormats.FunctionValidationLastLetterInvalid);
                        }
                    }
                }
            }
        }

        public void RemoveEntriesFromHistory(HistoryCollection history)
        {
            history.RemoveAllThatStartWith(this.Name);
        }

        public void RemoveEntriesFromHistoryNotPresentIn(TrieList collection, HistoryCollection history)
        {
            if (collection == null)
            {
                throw new ArgumentNullException("functionCollection");
            }
            else if (history == null)
            {
                throw new ArgumentNullException("history");
            }

            if (!collection.Contains(this.name, CaseSensitivity.Insensitive))
            {
                history.RemoveAllThatStartWith(this.name);
            }
        }

        public static string CreateAllStringParameterSignature(int numberOfStringParameters)
        {
            return new string(((char)FunctionParameterValueType.String), numberOfStringParameters);
        }

        //public string GetDocumentation(AssemblyReferenceCollectionComposite prioritizedAssemblyReferences)
        //{
        //    AssemblyReference assemblyReference;
        //    object classInstance;
        //    MethodInfo invokingMethod;

        //    this.GetInstance(prioritizedAssemblyReferences, out assemblyReference, out classInstance, out invokingMethod);

        //    DocumentationAttribute[] attributes = (DocumentationAttribute[])invokingMethod.GetCustomAttributes(typeof(DocumentationAttribute), false);

        //    if (attributes.Length > 0)
        //    {
        //        return attributes[0].Documentation;
        //    }
        //    else
        //    {
        //        return String.Empty;
        //    }
        //}

        public string TryGetDocumentation(AssemblyReferenceCollectionComposite prioritizedAssemblyReferences, bool needFast)
        {
            try
            {
                LoadedFunctionInfo info = this.GetInstance(prioritizedAssemblyReferences, needFast);

                DocumentationAttribute[] attributes = (DocumentationAttribute[])info.InvokingMethod.GetCustomAttributes(typeof(DocumentationAttribute), false);

                if (attributes.Length > 0)
                {
                    return attributes[0].Documentation;
                }
            }
            catch (LoadException)
            {
            }
            
            return String.Empty;
        }

        public string TryConstructFullDocumentation(AssemblyReferenceCollectionComposite prioritizedAssemblyReferences)
        {
            StringBuilder documentation = new StringBuilder();

            string mainDocumentation = this.TryGetDocumentation(prioritizedAssemblyReferences, false);

            if (!String.IsNullOrEmpty(mainDocumentation))
            {
                documentation.Append(mainDocumentation);
            }

            LoadedFunctionInfo info = null;

            try
            {
                info = this.GetInstance(prioritizedAssemblyReferences);
            }
            catch (LoadException)
            {
            }

            bool addedFirstParameterDocumentationYet = false;

            for (int i = 0; i < this.Parameters.Count; i++)
            {
                string parameterName = this.TryGetParameterName(i, info);
                string parameterDocumentation = this.TryGetParameterDocumentation(i, info);

                if (documentation.Length > 0 && !addedFirstParameterDocumentationYet)
                {
                    documentation.AppendLine();
                    documentation.AppendLine();
                }

                if (!String.IsNullOrEmpty(parameterDocumentation))
                {
                    addedFirstParameterDocumentationYet = true;
                    documentation.AppendFormat("{0}: {1}", parameterName, parameterDocumentation);
                    documentation.AppendLine();
                }
            }

            return documentation.ToString();
        }

        public string TryGetParameterDocumentation(int parameterIndex, LoadedFunctionInfo info)
        {
            if (info != null)
            {
                ParameterDocumentationAttribute[] attributes = (ParameterDocumentationAttribute[])info.InvokingMethod.GetCustomAttributes(
                    typeof(ParameterDocumentationAttribute), 
                    false);

                foreach (ParameterDocumentationAttribute attribute in attributes)
                {
                    if (attribute.Index == parameterIndex)
                    {
                        return attribute.Documentation;
                    }
                }
            }

            return String.Empty;
        }

        public string TryGetParameterDocumentation(int parameterIndex, AssemblyReferenceCollectionComposite prioritizedAssemblyReferences)
        {
            try
            {
                return this.TryGetParameterDocumentation(parameterIndex, this.GetInstance(prioritizedAssemblyReferences));
            }
            catch (LoadException)
            {
            }

            return String.Empty;
        }

        public LoadedFunctionInfo TryGetInstance(AssemblyReferenceCollectionComposite prioritizedAssemblyReferences)
        {
            try
            {
                return this.GetInstance(prioritizedAssemblyReferences);
            }
            catch (LoadException)
            {
            }

            return null;
        }

        public LoadedFunctionInfo GetInstance(AssemblyReferenceCollectionComposite prioritizedAssemblyReferences)
        {
            return this.GetInstance(prioritizedAssemblyReferences, false);
        }

        public LoadedFunctionInfo GetInstance(AssemblyReferenceCollectionComposite prioritizedAssemblyReferences, bool needFast)
        {
            if (!prioritizedAssemblyReferences.Contains(this.assemblyReferenceName))
            {
                throw new LoadException(
                    String.Format(
                    CultureInfo.CurrentCulture,
                    Localization.ExceptionFormats.MissingAssemblyReference,
                    this.assemblyReferenceName));
            }

            AssemblyReference assemblyReference = prioritizedAssemblyReferences[this.assemblyReferenceName].Item;

            Assembly assembly = assemblyReference.GetAssembly(needFast);

            Type invokingClass = null;

            try
            {
                invokingClass = assembly.GetType(this.InvocationClass);
            }
            catch (BadImageFormatException ex)
            {
                throw new LoadException(ex.Message);
            }
            catch (Exception ex)
            {
                throw new LoadException(ex.Message);
            }

            if (invokingClass == null)
            {
                throw new LoadException(
                    String.Format(
                    CultureInfo.CurrentCulture,
                    Localization.ExceptionFormats.MissingClassInAssembly,
                    this.InvocationClass,
                    assemblyReference.Filename));
            }

            MethodInfo invokingMethod = null;

            try
            {
                List<Type> parameterTypes = new List<Type>();
                foreach (FunctionParameter parameter in this.parameters)
                {
                    switch (parameter.ValueType)
                    {
                        default:
                            parameterTypes.Add(typeof(string));
                            break;
                    }
                }

                invokingMethod = invokingClass.GetMethod(this.MethodName, parameterTypes.ToArray());
            }
            catch (AmbiguousMatchException ex)
            {
                throw new LoadException(ex.Message);
            }

            if (invokingMethod == null)
            {
                string parameterSingularOrPlural;
                if (this.parameters.Count == 1)
                {
                    parameterSingularOrPlural = Localization.Promptu.ParameterSingular;
                }
                else
                {
                    parameterSingularOrPlural = Localization.Promptu.ParameterPlural;
                }

                throw new LoadException(
                    String.Format(
                    CultureInfo.CurrentCulture,
                    Localization.ExceptionFormats.MissingMethodInClass,
                    this.MethodName,
                    this.InvocationClass,
                    assemblyReference.Filename,
                    this.parameters.Count,
                    parameterSingularOrPlural));
            }

            object classInstance = null;

            try
            {
                classInstance = Activator.CreateInstance(invokingClass);
            }
            catch (Exception ex)
            {
                throw new LoadException(ex.Message);
            }

            if (classInstance == null)
            {
                throw new LoadException(
                    String.Format(
                    CultureInfo.CurrentCulture,
                    Localization.ExceptionFormats.CannotCreateInstanceOfClass,
                    this.InvocationClass,
                    assemblyReference.Filepath));
            }

            return new LoadedFunctionInfo(assemblyReference, classInstance, invokingMethod);
        }

        public string TranslateArgument(string suppliedValue, int parameterIndex, List listFrom)
        {
            FunctionParameter parameter = this.Parameters[parameterIndex];

            return ParameterSuggestion.TranslateArgument(suppliedValue, listFrom, parameter.ParameterSuggestion);
        }

        public object Invoke(string[] parameters, ExecutionData data, AssemblyReferenceCollectionComposite prioritizedAssemblyReferences)
        {
            LoadedFunctionInfo info = this.GetInstance(prioritizedAssemblyReferences);

            string[] realParameters = new string[parameters.Length];

            for (int i = 0; i < this.Parameters.Count; i++)
            {
                realParameters[i] = this.TranslateArgument(parameters[i], i, prioritizedAssemblyReferences.PriorityList);
            }

            object returnValue = null;

            try
            {
                returnValue = info.InvokingMethod.Invoke(info.ClassInstance, realParameters);
            }
            catch (Exception ex)
            {
                Exception exception = ex;
                if (ex.InnerException != null)
                {
                    exception = ex.InnerException;
                }

                throw new LoadException(
                    String.Format(
                    CultureInfo.CurrentCulture,
                    Localization.ExceptionFormats.UnableToInvokeMethod,
                    this.MethodName,
                    this.InvocationClass,
                    info.AssemblyReference.Filepath,
                    exception.Message));
            }

            if (returnValue == null)
            {
                return null;
            }
            else
            {
                switch (this.ReturnValue)
                {
                    case ReturnValue.String:
                        if (!(returnValue is string))
                        {
                            throw new LoadException(
                                String.Format(
                                CultureInfo.CurrentCulture,
                                Localization.ExceptionFormats.ReturnValueTypeMismatch,
                                this.MethodName,
                                this.InvocationClass,
                                info.AssemblyReference.Filepath,
                                returnValue.GetType(),
                                typeof(string)));
                        }

                        break;
                    case ReturnValue.StringArray:
                        if (!(returnValue is string[]))
                        {
                            throw new LoadException(
                                String.Format(
                                CultureInfo.CurrentCulture,
                                Localization.ExceptionFormats.ReturnValueTypeMismatch,
                                this.MethodName,
                                this.InvocationClass,
                                info.AssemblyReference.Filepath,
                                returnValue.GetType(),
                                typeof(string[])));
                        }

                        break;
                    case ReturnValue.ValueList:
                        if (!(returnValue is ValueList))
                        {
                            throw new LoadException(
                                String.Format(
                                CultureInfo.CurrentCulture,
                                Localization.ExceptionFormats.ReturnValueTypeMismatch,
                                this.MethodName,
                                this.InvocationClass,
                                info.AssemblyReference.Filepath,
                                returnValue.GetType(),
                                typeof(ValueList)));
                        }

                        break;
                }

                return returnValue;
            }
        }

        internal XmlNode ToXml(XmlDocument document)
        {
            XmlNode node = document.CreateElement("Function");

            if (this.id != null)
            {
                node.Attributes.Append(XmlUtilities.CreateAttribute("id", this.id.ToString(), document));
            }

            node.Attributes.Append(XmlUtilities.CreateAttribute("name", this.Name, document));
            node.Attributes.Append(XmlUtilities.CreateAttribute("invocationClass", this.InvocationClass, document));
            node.Attributes.Append(XmlUtilities.CreateAttribute("method", this.MethodName, document));
            node.Attributes.Append(XmlUtilities.CreateAttribute("assemblyReference", this.assemblyReferenceName, document));
            node.Attributes.Append(XmlUtilities.CreateAttribute("returns", this.returnValue.ToString(), document));
            node.AppendChild(this.Parameters.ToXml("Parameters", document));
            //node.Attributes.Append(XmlUtilities.CreateAttribute("numberOfParameters", this.numberOfParameters, document));
            return node;
        }

        internal static Function FromXml(XmlNode node)
        {
            if (node.Name.ToLowerInvariant() != XmlAlias)
            {
                throw new ArgumentException("The node is not named " + XmlAlias + ".");
            }

            string name = null;
            string invocationClass = null;
            string assemblyReferenceName = null;
            string methodName = null;
            FunctionParameterCollection parameters = null;
            Id id = null;
            ReturnValue returnValue = ReturnValue.String;

            foreach (XmlAttribute attribute in node.Attributes)
            {
                switch (attribute.Name.ToUpperInvariant())
                {
                    case "ID":
                        try
                        {
                            id = new Id(attribute.Value);
                        }
                        catch (FormatException)
                        {
                        }
                        catch (OverflowException)
                        {
                        }

                        break;
                    case "NAME":
                        name = attribute.Value;
                        break;
                    case "INVOCATIONCLASS":
                        invocationClass = attribute.Value;
                        break;
                    case "ASSEMBLYREFERENCE":
                        assemblyReferenceName = attribute.Value;
                        break;
                    case "METHOD":
                        methodName = attribute.Value;
                        break;
                    case "RETURNS":
                        try
                        {
                            returnValue = (ReturnValue)Enum.Parse(typeof(ReturnValue), attribute.Value);
                        }
                        catch (ArgumentException)
                        {
                        }

                        break;
                    case "NUMBEROFPARAMETERS": // Backward compatability for 0.7 data
                        //try
                        //{
                            parameters = new FunctionParameterCollection();
                            int? numberOfParameters = Utilities.TryParseInt32(attribute.Value, null);//Convert.ToInt32(attribute.Value);
                            if (numberOfParameters != null)
                            {
                                for (int i = 0; i < numberOfParameters.Value; i++)
                                {
                                    parameters.Add(new FunctionParameter(FunctionParameterValueType.String, null, false));
                                }
                            }
                        //}
                        //catch (FormatException)
                        //{
                        //}
                        //catch (OverflowException)
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
                     case "PARAMETERS":
                        parameters = FunctionParameterCollection.FromXml(innerNode);
                        break;
                    default:
                        break;
                }
            }

            if (invocationClass == null)
            {
                throw new LoadException("The function's invocation class name was not found.");
            }
            else if (name == null)
            {
                throw new LoadException("The functions's name was not found.");
            }
            else if (assemblyReferenceName == null)
            {
                throw new LoadException("The function's assembly reference name was not found.");
            }
            else if (methodName == null)
            {
                throw new LoadException("The function's method name was not found.");
            }
            
            if (parameters == null)
            {
                parameters = new FunctionParameterCollection();
            }

            // TODO Move isCustom around with xml?

            return new Function(name, invocationClass, methodName, assemblyReferenceName, returnValue, parameters, id, true);
        }

        public string ParameterSignature
        {
            get
            {
                if (this.parameterSignature == null)
                {
                    StringBuilder builder = new StringBuilder();
                    foreach (FunctionParameter parameter in this.Parameters)
                    {
                        builder.Append(parameter.GetSignature());
                    }

                    this.parameterSignature = builder.ToString();
                }

                return this.parameterSignature;
            }
        }

        public static string[] ExtractNameAndParametersFrom(string s)
        {
            if (s == null)
            {
                throw new ArgumentNullException("s");
            }

            List<string> parameters = new List<string>();
            int level = -1;

            for (int i = 0; i < s.Length; i++)
            {
                char? breakChar;
                int indexOfNextBreakChar = s.IndexOfNextBreakingChar(i, s.Length - i, true, out breakChar, '(', ',', ')');

                bool breakCharIsOpenParentheses = breakChar == '(';

                if (breakCharIsOpenParentheses)
                {
                    level++;
                }

                if (indexOfNextBreakChar == -1)
                {
                    indexOfNextBreakChar = s.Length;
                }

                if (level == 0)
                {
                    i--;

                    StringBuilder currentParameter = new StringBuilder();

                    while (i + 1 < indexOfNextBreakChar)
                    {
                        i++;
                        currentParameter.Append(s[i]);
                    }

                    i++;

                    string currentParameterValue = currentParameter.ToString().Trim();

                    if (level == 0 && (breakChar != ')' || (parameters.Count > 1 || currentParameterValue.Length > 0)))
                    {
                        parameters.Add(currentParameterValue);
                    }
                }
                else
                {
                    i = indexOfNextBreakChar;
                }

                if (breakChar == ')')
                {
                    level--;
                }
            }

            return parameters.ToArray();
        }

        public static bool IsInFunctionSyntax(string s)
        {
            if (s == null)
            {
                throw new ArgumentNullException("s");
            }

            for (int i = 0; i < s.Length; i++)
            {
                char c = s[i];
                if (Char.IsWhiteSpace(c) || c == Path.DirectorySeparatorChar)
                {
                    break;
                }
                //else if (!IsValidCharForPosition(c, i, (i + 1 >= s.Length || s[i + 1] == '(')))
                //{
                //    break;
                //}
                else if (c == '(')
                {
                    return true;
                }
            }

            return false;
        }

        public static string ConvertToFunctionSyntax(string s)
        {
            if (s == null)
            {
                throw new ArgumentNullException("s");
            }

            StringBuilder builder = new StringBuilder();
            string[] brokenApart = s.BreakApart();
            for (int i = 0; i < brokenApart.Length; i++)
            {
                if (i == 0)
                {
                    builder.Append(brokenApart[i]);
                }
                else
                {
                    builder.AppendFormat("\"{0}\"", brokenApart[i].Escape());
                }

                if (i <= 0)
                {
                    builder.Append("(");
                }
                else if (i < brokenApart.Length - 1)
                {
                    builder.Append(", ");
                }
                else
                {
                    builder.Append(")");
                }
            }

            return builder.ToString();
        }

        public static List<StringBuilder> Split(string s)
        {
            if (s == null)
            {
                throw new ArgumentNullException("s");
            }

            List<StringBuilder> splitFunctions = new List<StringBuilder>();
            int level = -1;

            for (int i = 0; i < s.Length; i++)
            {
                char? breakChar;
                int indexOfNextBreakChar = s.IndexOfNextBreakingChar(i, s.Length - i, true, out breakChar, '(', ',', ')');

                bool breakCharIsOpenParentheses = breakChar == '(';

                StringBuilder previousLevel = null;

                if (breakCharIsOpenParentheses)
                {
                    if (level >= 0)
                    {
                        previousLevel = splitFunctions[level];
                    }

                    level++;
                }
                else
                {
                }

                if (level >= splitFunctions.Count)
                {
                    splitFunctions.Add(new StringBuilder());
                }

                if (indexOfNextBreakChar == -1)
                {
                    indexOfNextBreakChar = s.Length - 1;
                }

                StringBuilder currentLevel = splitFunctions[level];

                bool seenAnythingNotWhitespace = false;
                i--;
                while (i + 1 <= indexOfNextBreakChar)
                {
                    i++;
                    char c = s[i];
                    bool haveNotStartedOnCurrentLevel = !seenAnythingNotWhitespace && previousLevel != null;
                    if (haveNotStartedOnCurrentLevel && char.IsWhiteSpace(c))
                    {
                        previousLevel.Append(c);
                    }
                    else
                    {
                        if (haveNotStartedOnCurrentLevel)
                        {
                            previousLevel.Append("&f:");
                        }

                        seenAnythingNotWhitespace = true;

                        currentLevel.Append(c);
                        if (breakCharIsOpenParentheses && previousLevel != null)
                        {
                            if (i < indexOfNextBreakChar)
                            {
                                previousLevel.Append(c);
                            }
                            else
                            {
                                previousLevel.Append(';');
                            }
                        }
                    }
                }

                if (breakChar == ')')
                {
                    level--;
                }
            }

            return splitFunctions;
        }

        //private static void EscapeIfNecessaryAndAppend(char c, StringBuilder builder)
        //{
        //    switch (c)
        //    {
        //        case '&':
        //            builder.Append("&amp;");
        //            break;
        //        case ';':
        //            builder.Append("&semi;");
        //            break;
        //        default:
        //            builder.Append(c);
        //            break;
        //    }
        //}

        //private static string Unescape(string s)
        //{
        //    return s.Replace("&amp;", "&").Replace("&semi;", ";");
        //}

        public string GetSignature()
        {
            return FormatSignature(this.returnValue, this.name, this.ParameterSignature);
        }

        public static string AppendImplicitCloseParentheses(string text)
        {
            if (text == null)
            {
                throw new ArgumentNullException("text");
            }
            else if (!Function.IsInFunctionSyntax(text))
            {
                return text;
            }

            char character;

            int level = -1;

            for (int i = 0; i < text.Length; i++)
            {
                character = text[i];

                //int levelOffset = 0;

                switch (character)
                {
                    case '(':
                        //levelOffset = -1;
                        level++;
                        break;
                    case ')':
                        //levelOffset = 1;
                        level--;
                        break;
                    case '"':
                        i++;

                        while (i < text.Length)
                        {
                            if (text[i] == '"' && (text.CountReverseCharRun('\\', i - 1) % 2) == 0)
                            {
                                break;
                            }

                            i++;
                        }

                        break;
                    default:
                        break;
                }

                //if (i == text.Length - 1)
                //{
                //    level = level + levelOffset;
                //    break;
                //}
            }

            if (level > -1)
            {
                StringBuilder builder = new StringBuilder(text);

                for (int i = level; i >= 0; i--)
                {
                    builder.Append(')');
                }

                return builder.ToString();
            }
            else
            {
                return text;
            }
        }

        public string GetNamedParametersIfPossible(AssemblyReferenceCollectionComposite prioritizedAssemblyReferences, bool needFast)
        {
            try
            {
                LoadedFunctionInfo info = this.GetInstance(prioritizedAssemblyReferences, needFast);

                ParameterInfo[] parameters = info.Parameters;

                StringBuilder parameterSignature = new StringBuilder();
                for (int i = 0; i < this.Parameters.Count; i++)
                {
                    if (i > 0)
                    {
                        parameterSignature.Append(", ");
                    }

                    parameterSignature.Append(this.parameters[i].ValueType.ToString());
                    parameterSignature.Append(" ");
                    parameterSignature.Append(parameters[i].Name);
                }

                return String.Format(CultureInfo.CurrentCulture, "({0})", parameterSignature.ToString());
            }
            catch (LoadException)
            {
            }

            return FormatParameterSignature(this.parameterSignature);
        }

        public string GetNamedSignatureIfPossible(AssemblyReferenceCollectionComposite prioritizedAssemblyReferences)
        {
            try
            {
                LoadedFunctionInfo info = this.GetInstance(prioritizedAssemblyReferences);

                ParameterInfo[] parameters = info.Parameters;

                StringBuilder parameterSignature = new StringBuilder();
                for (int i = 0; i < this.Parameters.Count; i++)
                {
                    if (i > 0)
                    {
                        parameterSignature.Append(", ");
                    }

                    parameterSignature.Append(this.parameters[i].ValueType.ToString());
                    parameterSignature.Append(" ");
                    parameterSignature.Append(parameters[i].Name);
                }

                return String.Format(CultureInfo.CurrentCulture, "{2} {0}({1})", name, parameterSignature.ToString(), ConvertReturnValueToString(this.returnValue));
            }
            catch (LoadException)
            {
            }

            return GetSignature();
        }

        public string GetStringId()
        {
            return CreateStringId(this.name, this.ParameterSignature);
        }

        //public string GetFullStringId()
        //{
        //    return CreateStringId(this.name, ((int)this.ReturnValue).ToString() + this.ParameterSignature);
        //}

        public static string ExtractNameFromStringId(string stringId, string parameterSignature)
        {
            return stringId.Substring(1, stringId.Length - 2 - parameterSignature.Length);
        }

        public static string CreateStringId(string name, string parameterSignature)
        {
            return String.Format(CultureInfo.InvariantCulture, "[{0}]{1}", name, parameterSignature);
        }

        // I18N
        public static string ConvertReturnValueToString(ReturnValue returnValue)
        {
            switch (returnValue)
            {
                case ReturnValue.StringArray:
                    return "String[]";
                case ReturnValue.ValueList:
                    return "ValueList";
                default:
                    return "String";
            }
        }

        public static string FormatSignature(ReturnValue returnValue, string name, string parameterSignature)
        {
            StringBuilder parameterPartOfSignature = new StringBuilder();
            for (int i = 0; i < parameterSignature.Length; i++)
            {
                if (i > 0)
                {
                    parameterPartOfSignature.Append(", ");
                }

                switch (parameterSignature[i])
                {
                    default:
                        parameterPartOfSignature.Append("String");
                        break;
                }
            }

            return String.Format(CultureInfo.CurrentCulture, "{2} {0}({1})", name, parameterPartOfSignature.ToString(), ConvertReturnValueToString(returnValue));
        }

        public string TryGetParameterName(int index, LoadedFunctionInfo info)
        {
            if (info != null && index < info.Parameters.Length)
            {
                return info.Parameters[index].Name;
            }

            return string.Format(CultureInfo.CurrentCulture, "param{0}", index + 1);
        }

        //public string TryGetParameterName(int index, AssemblyReferenceCollectionComposite prioritizedAssemblyReferences)

        public string TryGetParameterName(int index, AssemblyReferenceCollectionComposite prioritizedAssemblyReferences)
        {
            try
            {
                return TryGetParameterName(index, this.GetInstance(prioritizedAssemblyReferences));
            }
            catch (LoadException)
            {
            }

            return string.Format(CultureInfo.CurrentCulture, "param{0}", index + 1);
        }

        public static string FormatParameterSignature(string parameterSignature)
        {
            StringBuilder parameterPartOfSignature = new StringBuilder();
            for (int i = 0; i < parameterSignature.Length; i++)
            {
                if (i > 0)
                {
                    parameterPartOfSignature.Append(", ");
                }

                switch (parameterSignature[i])
                {
                    default:
                        parameterPartOfSignature.Append("String");
                        break;
                }
            }

            return String.Format(CultureInfo.CurrentCulture, "({0})", parameterPartOfSignature.ToString());
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public override string ToString()
        {
            return this.Name;
        }
    }
}
