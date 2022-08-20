using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Xml;
//using ZachJohnson.Promptu.DynamicEntryModel;
//using ZachJohnson.Promptu.DynamicEntryModel.Parsing;
using System.Diagnostics;
using ZachJohnson.Promptu.Itl.AbstractSyntaxTree;
using ZachJohnson.Promptu.Itl;
using ZachJohnson.Promptu.UserModel.Differencing;
using ZachJohnson.Promptu.Collections;
using ZachJohnson.Promptu.UserModel.Collections;
using System.Extensions;
using ZachJohnson.Promptu.UIModel;
using System.ComponentModel;
using System.Globalization;
using System.Drawing;
using System.IO;
using System.IO.Extensions;
using System.Net;
using System.Runtime.InteropServices;

namespace ZachJohnson.Promptu.UserModel
{
    internal class Command : IDiffable, INamed, INotifyPropertyChanged
    {
        private static readonly List<char> invalidPathChars = new List<char>(Path.GetInvalidFileNameChars());
        private static readonly ReadOnlyCollection<char> illegalNameChars = new List<char>(new char[] { '\\', '/', '(', ')', '"', '&', ' ' }).AsReadOnly();
        private static readonly char[] aliasSplitChars = new char[] { ',' };
        internal const bool DefaultUseExecutionDirectoryAsStartupDirectory = true;
        private static List<string> reservedNames = new List<string>();
        internal const string XmlAlias = "command";
        private string name;
        private string executionPath;
        private string arguments;
        private bool runAsAdministrator;
        private bool showParameterHistory;
        private bool useExecutionDirectoryAsStartupDirectory;
        private string notes;
        private Expression parsedExecutionPath;
        private Expression parsedArguments;
        private Expression parsedStartupDirectory;
        private Expression actualParsedExecutionPath;
        private Expression actualParsedArguments;
        private Expression actualParsedStartupDirectory;
        private ProcessWindowStyle startingWindowState;
        private string startupDirectory;
        private Id id;
        private string[] nameAliases;
        private bool? isFileSystemCommand;
        private CommandParameterMetaInfoCollection parametersMetaInfo;
        private FileSystemFile? iconPath;
        private bool cachedIconPath;

        static Command()
        {
            // when changing, update reserved name message for function editing.
            reservedNames.Add("SETUP");
            reservedNames.Add("QUIT");
            reservedNames.Add("SYNCHRONIZE");
            reservedNames.Add("HELP");
            reservedNames.Add("ABOUT");
        }

        public Command(string name, string executionPath, string arguments, bool runAsAdministrator)
            : this(
            name, 
            executionPath, 
            arguments, 
            runAsAdministrator, 
            ProcessWindowStyle.Normal, 
            String.Empty, 
            String.Empty, 
            InternalGlobals.CurrentProfile.CommandDefaults.SaveParameterHistory,
            DefaultUseExecutionDirectoryAsStartupDirectory,
            null,
            null)
        {
        }

        public Command(
            string name, 
            string executionPath, 
            string arguments, 
            bool runAsAdministrator, 
            ProcessWindowStyle startingWindowState, 
            string notes, 
            string startupDirectory,
            bool showParameterHistory,
            bool useExecutionDirectoryAsStartupDirectory,
            Id id,
            CommandParameterMetaInfoCollection parametersMetaInfo)
        {
            if (name == null)
            {
                throw new ArgumentNullException("name");
            }
            else if (executionPath == null)
            {
                throw new ArgumentNullException("executionPath");
            }
            else if (arguments == null)
            {
                arguments = String.Empty;
            }
            else if (reservedNames.Contains(executionPath))
            {
                throw new ArgumentException("The name is reserved and cannot be used.");
            }

            if (parametersMetaInfo == null)
            {
                this.parametersMetaInfo = new CommandParameterMetaInfoCollection();
            }
            else
            {
                this.parametersMetaInfo = parametersMetaInfo;
            }

            this.name = name;
            this.executionPath = executionPath;
            this.arguments = arguments;
            this.runAsAdministrator = runAsAdministrator;
            //this.parsedExecutionPath = //CommandParser.PreParse(this.executionPath);
            //this.parsedArguments = //CommandParser.PreParse(this.arguments);
            this.startingWindowState = startingWindowState;
            this.startupDirectory = startupDirectory ?? String.Empty;
            this.notes = notes;
            this.showParameterHistory = showParameterHistory;
            this.useExecutionDirectoryAsStartupDirectory = useExecutionDirectoryAsStartupDirectory;
            this.id = id;
        }

        internal ItemInfo GetItemInfo()
        {
            List<string> attributes = new List<string>();

            attributes.Add(String.Format(
                CultureInfo.CurrentCulture, 
                Localization.UIResources.CommandDetailsExecutionPathFormat,
                this.ExecutionPath));

            string arguments = this.Arguments;
            if (arguments.Length > 0)
            {
                attributes.Add(String.Format(
                    CultureInfo.CurrentCulture, 
                    Localization.UIResources.CommandDetailsArgumentsFormat,
                    arguments));
            }

            if (this.RunAsAdministrator)
            {
                attributes.Add(String.Format(
                    CultureInfo.CurrentCulture, 
                    Localization.UIResources.CommandDetailsRunAsAdministratorFormat,
                    this.RunAsAdministrator));
            }

            string startupDirectory = this.StartupDirectory;
            if (!String.IsNullOrEmpty(startupDirectory))
            {
                attributes.Add(String.Format(
                    CultureInfo.CurrentCulture, 
                    Localization.UIResources.CommandDetailsStartupDirectoryFormat,
                    startupDirectory));
            }

            return new ItemInfo(this.Name, attributes);
        }

        public bool IsFileSystemCommand(List listFrom, bool forceRegeneration)
        {
            if (forceRegeneration || this.isFileSystemCommand == null)
            {
                bool value = false;

                if (this.GetMininumNumberOfParameters() == 0)
                {
                    try
                    {
                        ExecutionData executionData = new ExecutionData(
                            new string[0],
                            listFrom,
                            InternalGlobals.CurrentProfile.Lists);
                        FileSystemDirectory proposedDirectory = this.GetSubstitutedExecutionPath(executionData);
                        if (proposedDirectory.Exists)
                        {
                            value = true;
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

                this.isFileSystemCommand = value;
            }

            return this.isFileSystemCommand.Value;
        }

        public static string GetPromptuCommandDocumentation(string commandName)
        {
            return Localization.Documentation.ResourceManager.GetString(commandName.ToUpperInvariant());
        }

        public static ReadOnlyCollection<string> ReservedNames
        {
            get { return reservedNames.AsReadOnly(); }
        }

        public static ReadOnlyCollection<char> IllegalNameChars
        {
            get { return illegalNameChars; }
        }

        public bool UseExecutionDirectoryAsStartupDirectory
        {
            get { return this.useExecutionDirectoryAsStartupDirectory; }
        }

        public CommandParameterMetaInfoCollection ParametersMetaInfo
        {
            get { return this.parametersMetaInfo; }
        }

        public string Name
        {
            get { return this.name; }
        }

        public Id Id
        {
            get { return this.id; }
            set { this.id = value; }
        }

        public static char[] AliasSplitChars
        {
            get { return aliasSplitChars; }
        }

        public string ExecutionPath
        {
            get { return this.executionPath; }
            //set { this.executionPath = value; }
        }

        public string Arguments
        {
            get { return this.arguments; }
        }

        public bool RunAsAdministrator
        {
            get { return this.runAsAdministrator; }
        }

        public bool ShowParameterHistory
        {
            get { return this.showParameterHistory; }
        }

        public string Notes
        {
            get { return this.notes; }
        }

        public ProcessWindowStyle StartingWindowState
        {
            get { return this.startingWindowState; }
        }

        public string StartupDirectory
        {
            get { return this.startupDirectory; }
        }

        public string[] GetAliases()
        {
            if (this.nameAliases == null)
            {
                this.nameAliases = Command.GetAliasesFromName(this.name);
            }

            return (string[])this.nameAliases.Clone();
        }

        public void RemoveEntriesFromHistory(HistoryCollection history)
        {
            if (history == null)
            {
                throw new ArgumentNullException("history");
            }

            history.RemoveAllThatStartWith(this.GetAllPossibleNames());
        }

        public void RemoveEntriesFromHistoryNotPresentIn(TrieList collection, HistoryCollection history)
        {
            if (collection == null)
            {
                throw new ArgumentNullException("commandCollection");
            }
            else if (history == null)
            {
                throw new ArgumentNullException("history");
            }

            List<string> stringsToRemove = new List<string>();
            foreach (string alias in this.GetAllPossibleNames())
            {
                if (!collection.Contains(alias, CaseSensitivity.Insensitive))
                {
                    stringsToRemove.Add(alias);
                }
            }

            history.RemoveAllThatStartWith(stringsToRemove);
        }

        public string GetIconId(List listFrom)
        {
            return String.Format(CultureInfo.InvariantCulture, "{0}.{1}", listFrom.FolderName, this.Id);
        }

        public string GetFormattedIdentifier()
        {
            return this.Name;
        }

        public static string[] ExtractNameAndParametersFrom(string text)
        {
            if (text == null)
            {
                throw new ArgumentNullException("text");
            }

            return text.BreakApart(Quotes.Include, Spaces.Break);
        }

        public static string[] ExtractSimpleNameAndParametersFrom(string text)
        {
            if (text == null)
            {
                throw new ArgumentNullException("text");
            }

            return text.BreakApart(Quotes.Eat, Spaces.Break);
        }

        public bool GetWhetherJustNParameters()
        {
            this.TryParseArguments();
            this.TryParseExecutionPath();
            this.TryParseStartupDirectory();

            bool isJustN = GetWhetherOnlyParameterSpecifedIsN(this.parsedExecutionPath);
            if (!isJustN)
            {
                return isJustN;
            }

            isJustN = GetWhetherOnlyParameterSpecifedIsN(this.parsedArguments);

            if (!isJustN)
            {
                return isJustN;
            }

            isJustN = GetWhetherOnlyParameterSpecifedIsN(this.parsedStartupDirectory);

            if (!isJustN)
            {
                return isJustN;
            }

            if (this.parametersMetaInfo.Count == 1)
            {
                CommandParameterMetaInfo metaInfo = this.parametersMetaInfo[0];
                return metaInfo.FirstParameter == 1 && metaInfo.LastParameter == null;
            }
            else if (this.parametersMetaInfo.Count == 0)
            {
                return true;
            }

            return false;
        }

        public FileSystemFile? GetIconPath(List listFrom)
        {
            FileSystemFile? iconPath = this.iconPath;
            if (!this.cachedIconPath)
            {
                string iconTargetBase = this.GetIconTargetBase(listFrom);

                Uri uri;
                if (Uri.TryCreate(iconTargetBase, UriKind.Absolute, out uri) 
                    && (uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps))
                {
                    StringBuilder fileName = new StringBuilder();
                    foreach (char c in uri.Host.ToUpperInvariant())
                    {
                        if (!invalidPathChars.Contains(c))
                        {
                            fileName.Append(c);
                        }
                    }

                    fileName.Append(".ico");

                    iconPath = InternalGlobals.CurrentProfile.IconCacheDirectory
                        + String.Format("{0}web{0}{1}", Path.DirectorySeparatorChar, fileName.ToString());
                }
                else
                {
                    iconPath = InternalGlobals.CurrentProfile.IconCacheDirectory + (this.GetIconId(listFrom) + ".png");
                }

                this.cachedIconPath = true;
                this.iconPath = iconPath;
            }

            return iconPath;
            //return InternalGlobals.CurrentProfile.IconCacheDirectory + (this.GetIconId(listFrom) + ".png");
            //if (iconFile.Exists)
            //{
            //    return (Bitmap)Bitmap.FromFile(iconFile);
            //}

            //return null;
        }

        private string GetIconTargetBase(List listFrom)
        {
            int? mininum = null;

            this.TryParseExecutionPath();

            GetMininumNumberOfParameters(this.parsedExecutionPath, ref mininum);

            int mininumNumberOfParameters = mininum == null ? 0 : mininum.Value;

            bool fuzzyEvaluate = true;

            if (mininumNumberOfParameters <= 0)
            {
                fuzzyEvaluate = false;
                try
                {
                    string executionPath = this.GetSubstitutedExecutionPath(
                        new ExecutionData(new string[0], listFrom, InternalGlobals.CurrentProfile.Lists));

                    executionPath = InternalGlobals.GuiManager.ToolkitHost.ResolvePath(executionPath);

                    // TODO add plugin filter call here

                    // TODO parse iexplore, firefox etc

                    if (executionPath.StartsWith("www", StringComparison.InvariantCultureIgnoreCase))
                    {
                        executionPath = "http://" + executionPath;
                    }

                    return executionPath;
                }
                catch (ParseException)
                {
                    fuzzyEvaluate = true;
                }
                catch (ConversionException)
                {
                    fuzzyEvaluate = true;
                }
                catch (SelfReferencingCommandException)
                {
                    fuzzyEvaluate = true;
                }
            }

            if (fuzzyEvaluate)
            {
                StringBuilder partialTarget = new StringBuilder();
                string target = this.executionPath;

                if (target.StartsWith("www", StringComparison.InvariantCultureIgnoreCase))
                {
                    target = "http://" + executionPath;
                }

                if (target.StartsWith("http://", StringComparison.InvariantCultureIgnoreCase)
                    || target.StartsWith("https://", StringComparison.InvariantCultureIgnoreCase))
                {
                    for (int i = 0; i < target.Length; i++)
                    {
                        char c = target[i];
                        if (c == '<' && i + 1 < target.Length && target[i + 1] != '<')
                        {
                            break;
                        }

                        partialTarget.Append(c);
                    }

                    return partialTarget.ToString();
                }
            }

            return null;
        }

        internal void UpdateCacheIconAsync(List listFrom)
        {
            if (!InternalGlobals.CurrentProfile.ShowCommandTargetIcons)
            {
                return;
            }

            BackgroundWorker worker = new BackgroundWorker();
            worker.DoWork += HandleUpdateCacheIconAsync;
            worker.RunWorkerAsync(listFrom);
        }

        private void HandleUpdateCacheIconAsync(object sender, DoWorkEventArgs e)
        {
            this.UpdateCacheIcon((List)e.Argument, false, Updater.GetFaviconWebClient());
            InternalGlobals.CurrentProfile.WebIconLastModifiedDictionary.Save();
        }

        internal void UpdateCacheIcon(List listFrom, bool userInitiated, PromptuWebClient webClient)
        {
            if (!InternalGlobals.CurrentProfile.ShowCommandTargetIcons)
            {
                return;
            }

            string iconTarget = this.GetIconTargetBase(listFrom);

            if (iconTarget == null)
            {
                return;
            }

            if (iconTarget.EndsWith(".lnk"))
            {
                try
                {
                    Shortcut shortcut = Shortcut.FromFile(iconTarget);
                    iconTarget = shortcut.Target;
                }
                catch (InvalidComObjectException)
                {
                }
            }

            //Debug.WriteLine(String.Format("Loading icon for {0}", iconTarget));

            Uri uri;
            if (Uri.TryCreate(iconTarget, UriKind.Absolute, out uri))
            {
                if (uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps)
                {
                    if (!userInitiated && !InternalGlobals.CurrentProfile.AutoFetchFavicons)
                    {
                        return;
                    }

                    UriBuilder builder = new UriBuilder();
                    builder.Host = uri.Host;
                    builder.Scheme = uri.Scheme;
                    builder.Path = "favicon.ico";

                    //PromptuWebClient client = Updater.GetDefaultWebClient();

                    FileSystemFile? cacheFile = this.GetIconPath(listFrom);//InternalGlobals.CurrentProfile.IconCacheDirectory + (id + ".png");

                    if (cacheFile == null)
                    {
                        return;
                    }

                    cacheFile.Value.GetParentDirectory().CreateIfDoesNotExist();

                    DateTime? lastModified = null;

                    try
                    {
                        lastModified = File.GetLastWriteTime(cacheFile);
                    }
                    catch (UnauthorizedAccessException)
                    {
                    }
                    catch (ArgumentException)
                    {
                    }
                    catch (IOException)
                    {
                    }
                    catch (NotSupportedException)
                    {
                    }

                    //if (lastModified == null)
                    //{
                        DateTime cachedModifiedTime;

                        if (InternalGlobals.CurrentProfile.WebIconLastModifiedDictionary.TryGetValue(
                            cacheFile.Value.NameWithoutExtension,
                            out cachedModifiedTime))
                        {
                            lastModified = cachedModifiedTime;
                        }
                    //}

                    if (lastModified != null)
                    {
                        if (lastModified.Value.AddDays(7) > DateTime.Now)
                        {
                            return;
                        }
                    }

                    try
                    {
                        HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(builder.Uri);
                        request.UserAgent = webClient.UserAgent;
                        request.Proxy = webClient.Proxy;
                        request.IfModifiedSince = lastModified ?? DateTime.MinValue;

                        if (userInitiated)
                        {
                            request.Timeout = 2000;
                        }

                        HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                        using (Stream s = response.GetResponseStream())
                        using (FileStream cacheFileStream = new FileStream(cacheFile, FileMode.Create, FileAccess.Write))
                        //using (MemoryStream localStream = new MemoryStream())
                        {
                            s.TransferTo(cacheFileStream);
                            //try
                            //{
                            //    localStream.Position = 0;
                            //    Icon icon = new Icon(localStream, new Size(32, 32));
                            //    icon.ToBitmap().Save(cacheFile);
                            //}
                            //catch (ArgumentException)
                            //{
                            //    try
                            //    {
                            //        localStream.Position = 0;
                            //        Bitmap bitmap = (Bitmap)Bitmap.FromStream(localStream);
                            //        cacheFile.Value.GetParentDirectory().CreateIfDoesNotExist();
                            //        bitmap.Save(cacheFile);
                            //    }
                            //    catch (ArgumentException)
                            //    {
                            //    }
                            //}
                        }

                        InternalGlobals.CurrentProfile.WebIconLastModifiedDictionary.Remove(cacheFile.Value.NameWithoutExtension);
                    }
                    catch (WebException ex)
                    {
                        HttpWebResponse response = ex.Response as HttpWebResponse;

                        if (response != null && 
                            (response.StatusCode == HttpStatusCode.NotFound || response.StatusCode == HttpStatusCode.NotModified))
                        {
                            if (cacheFile.Value.Exists)
                            {
                                cacheFile.Value.Touch();
                            }
                            else
                            {
                                InternalGlobals.CurrentProfile.WebIconLastModifiedDictionary.Set(cacheFile.Value.NameWithoutExtension, DateTime.Now);
                            }
                        }
                    }
                    catch (Exception)
                    {
                    }
                }
            }

            string id = this.GetIconId(listFrom);

            InternalGlobals.CurrentProfile.IconCacheDirectory.CreateIfDoesNotExist();
            if (File.Exists(iconTarget))
            {
                Icon icon = InternalGlobals.GuiManager.ToolkitHost.ExtractFileIcon(iconTarget, IconSize.Large);
                if (icon == null)
                {
                    return;
                }

                try
                {
                    icon.ToBitmap().Save(InternalGlobals.CurrentProfile.IconCacheDirectory + (id + ".png"));
                }
                catch (ExternalException)
                {
                }
                catch (IOException)
                {
                }
            }
            else if (Directory.Exists(iconTarget))
            {
                Icon icon = InternalGlobals.GuiManager.ToolkitHost.ExtractDirectoryIcon(iconTarget, IconSize.Large);
                if (icon == null)
                {
                    return;
                }

                try
                {
                    icon.ToBitmap().Save(InternalGlobals.CurrentProfile.IconCacheDirectory + (id + ".png"));
                }
                catch (ExternalException)
                {
                }
                catch (IOException)
                {
                }
            }
        }

        public ValueListCollection GetValueListDependencies(ValueListCollection allValueLists)
        {
            ValueListCollection valueLists = new ValueListCollection();

            foreach (CommandParameterMetaInfo metaInfo in this.ParametersMetaInfo)
            {
                ValueListParameterSuggestion suggestion = metaInfo.ParameterSuggestion as ValueListParameterSuggestion;
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

        public string[] GetAllPossibleNames()
        {
            string[] aliases = this.GetAliases();
            if (aliases.Length == 1)
            {
                return aliases;
            }

            string[] allNames = new string[aliases.Length + 1];
            for (int i = -1; i < aliases.Length; i++)
            {
                string name;
                if (i < 0)
                {
                    name = this.name;
                }
                else
                {
                    name = aliases[i];
                }

                allNames[i + 1] = name;
            }

            return allNames;
        }

        public Command Clone(string newName)
        {
            Command command = new Command(
                newName == null ? this.name : newName,
                this.executionPath,
                this.arguments,
                this.runAsAdministrator,
                this.startingWindowState,
                this.notes,
                this.startupDirectory,
                this.showParameterHistory,
                this.useExecutionDirectoryAsStartupDirectory,
                this.id,
                this.parametersMetaInfo.Clone());
            command.parsedArguments = this.parsedArguments;
            command.parsedExecutionPath = this.parsedExecutionPath;
            command.parsedStartupDirectory = this.parsedStartupDirectory;
            command.actualParsedArguments = this.actualParsedArguments;
            command.actualParsedExecutionPath = this.actualParsedExecutionPath;
            command.actualParsedStartupDirectory = this.actualParsedStartupDirectory;
            command.isFileSystemCommand = this.isFileSystemCommand;
            return command;
        }

        public Command Clone()
        {
            return this.Clone(null);
        }

        public string GetSubstitutedExecutionPath(ExecutionData data)
        {
            return this.GetSubstitutedExecutionPathInternal(data, new TrieList(SortMode.Default));
        }

        private string GetSubstitutedExecutionPathInternal(ExecutionData data, TrieList alreadyCalledCommandNames)
        {
            if (this.actualParsedExecutionPath == null)
            {
                ItlCompiler compiler = new ItlCompiler();
                FeedbackCollection feedback;
                this.actualParsedExecutionPath = compiler.Compile(ItlType.Standard, this.executionPath, out feedback);

                if (feedback.Has(FeedbackType.Error))
                {
                    this.actualParsedExecutionPath = null;
                    StringBuilder message = new StringBuilder();
                    message.AppendLine(String.Format(CultureInfo.CurrentCulture, Localization.MessageFormats.ItlParseErrorOnExecutionPath, this.Name, this.executionPath));
                    foreach (FeedbackMessage feedbackMessage in feedback)
                    {
                        message.AppendLine(String.Format(CultureInfo.CurrentCulture, Localization.MessageFormats.ItlParseErrorMessageFormat, feedbackMessage.MessageType, feedbackMessage.Description));
                    }

                    throw new ParseException(message.ToString());
                }
            }

            string executionPath = this.actualParsedExecutionPath.ConvertToString(data);
            executionPath = executionPath.ExpandEnvironmentVariables();
            executionPath = executionPath.ReplaceIntrinsics();

            try
            {
                string[] pathSplit;
                if (Utilities.LooksLikeValidPath(executionPath, out pathSplit) && pathSplit.Length > 1)
                {
                    string beginningFolderName = pathSplit[0];
                    bool found;
                    GroupedCompositeItem compositeItem = InternalGlobals.CurrentProfile.CompositeFunctionsAndCommands.TryGetItem(beginningFolderName, out found);
                    CompositeItem<Command, List> parameterlessCommandNamedLikeFirstFolder;
                    if (found && compositeItem != null && (parameterlessCommandNamedLikeFirstFolder = compositeItem.TryGetCommand(beginningFolderName, 0)) != null)
                    {
                        if (alreadyCalledCommandNames.Contains(beginningFolderName, CaseSensitivity.Insensitive))
                        {
                            throw new SelfReferencingCommandException(String.Format(
                                CultureInfo.CurrentCulture, 
                                Localization.MessageFormats.SelfReferencingRecursiveDirectoryResolvingReference, beginningFolderName),
                                beginningFolderName, parameterlessCommandNamedLikeFirstFolder.ListFrom);
                            //this.InvokeOnMainThread(new ParameterlessVoid(delegate
                            //{
                            //    MessageBox.Show(
                            //    Localization.MessageFormats.SelfReferencingRecursiveMultiCall,
                            //    Localization.Promptu.AppName,
                            //    MessageBoxButtons.OK,
                            //    MessageBoxIcon.Error,
                            //    MessageBoxDefaultButton.Button1,
                            //    this.GetMessageBoxOptions());
                            //}));
                            //return false;
                        }
                        else
                        {
                            alreadyCalledCommandNames.Add(beginningFolderName);
                            FileSystemDirectory proposedDirectory = parameterlessCommandNamedLikeFirstFolder.Item.GetSubstitutedExecutionPathInternal(
                                new ExecutionData(new string[0], parameterlessCommandNamedLikeFirstFolder.ListFrom, InternalGlobals.CurrentProfile.Lists),
                                alreadyCalledCommandNames);

                            if (proposedDirectory.LooksValid)
                            {
                                executionPath = proposedDirectory + executionPath.Substring(beginningFolderName.Length);
                            }
                        }
                        //try
                        //{
                        //    ExecutionData itemExecutionData = new ExecutionData(
                        //        new string[0],
                        //        parameterlessCommandNamedLikeFirstFolder.ListFrom,
                        //        PromptuSettings.CurrentProfile.Lists);
                        //    FileSystemDirectory proposedDirectory = parameterlessCommandNamedLikeFirstFolder.Item.GetSubstitutedExecutionPath(executionData);
                        //    if (proposedDirectory.Exists)
                        //    {
                        //        directory = proposedDirectory + populationFilter.Substring(indexOfFirstDirectorySeparatorChar, indexOfLastDirectorySeparatorChar - indexOfFirstDirectorySeparatorChar + 1);
                        //    }
                        //}
                        //catch (Itl.ParseException)
                        //{
                        //}
                        //catch (ConversionException)
                        //{
                        //}
                    }
                }
            }
            catch (ParseException)
            {
            }
            catch (ConversionException)
            {
            }

            return executionPath;
        }

        public FunctionCollection GetFunctionDependecies(FunctionCollectionComposite allFunctions)
        {
            List<FunctionCall> functionCalls = new List<FunctionCall>();
            this.TryParseArguments();
            this.TryParseExecutionPath();
            this.TryParseStartupDirectory();

            Expression.GetFunctionCalls(this.parsedArguments, functionCalls);
            Expression.GetFunctionCalls(this.parsedExecutionPath, functionCalls);
            Expression.GetFunctionCalls(this.parsedStartupDirectory, functionCalls);

            FunctionCollection functions = new FunctionCollection();

            foreach (CommandParameterMetaInfo metaInfo in this.ParametersMetaInfo)
            {
                FunctionReturnParameterSuggestion suggestion = metaInfo.ParameterSuggestion as FunctionReturnParameterSuggestion;

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

        

        public string GetSubstitutedArguments(ExecutionData data)
        {
            if (this.actualParsedArguments == null)
            {
                ItlCompiler compiler = new ItlCompiler();
                FeedbackCollection feedback;
                this.actualParsedArguments = compiler.Compile(ItlType.Standard, this.arguments, out feedback);

                if (feedback.Has(FeedbackType.Error))
                {
                    this.actualParsedArguments = null;
                    StringBuilder message = new StringBuilder();
                    message.AppendLine(String.Format(CultureInfo.CurrentCulture, Localization.MessageFormats.ItlParseErrorOnArguments, this.Name, this.arguments));
                    foreach (FeedbackMessage feedbackMessage in feedback)
                    {
                        message.AppendLine(String.Format(CultureInfo.CurrentCulture, Localization.MessageFormats.ItlParseErrorMessageFormat, feedbackMessage.MessageType, feedbackMessage.Description));
                    }

                    throw new ParseException(message.ToString());
                }
            }

            return this.actualParsedArguments.ConvertToString(data).ExpandEnvironmentVariables().ReplaceIntrinsics();
        }

        public string GetSubstitutedStartupDirectory(ExecutionData data)
        {
            if (this.actualParsedStartupDirectory == null)
            {
                ItlCompiler compiler = new ItlCompiler();
                FeedbackCollection feedback;
                this.actualParsedStartupDirectory = compiler.Compile(ItlType.Standard, this.startupDirectory, out feedback);

                if (feedback.Has(FeedbackType.Error))
                {
                    this.actualParsedStartupDirectory = null;
                    StringBuilder message = new StringBuilder();
                    message.AppendLine(String.Format(CultureInfo.CurrentCulture, Localization.MessageFormats.ItlParseErrorOnStartupDirectory, this.Name, this.startupDirectory));
                    foreach (FeedbackMessage feedbackMessage in feedback)
                    {
                        message.AppendLine(String.Format(CultureInfo.CurrentCulture, Localization.MessageFormats.ItlParseErrorMessageFormat, feedbackMessage.MessageType, feedbackMessage.Description));
                    }

                    throw new ParseException(message.ToString());
                }
            }

            return this.actualParsedStartupDirectory.ConvertToString(data).ExpandEnvironmentVariables().ReplaceIntrinsics();
        }

        public IntRange GetParameterRange()
        {
            return new IntRange(this.GetMininumNumberOfParameters(), this.GetMaximumNumberOfParameters());
        }

        public int? GetMaximumNumberOfParameters()
        {
            int? maximum = 0;

            this.TryParseArguments();
            this.TryParseExecutionPath();
            this.TryParseStartupDirectory();

            GetMaximumNumberOfParameters(this.parsedExecutionPath, ref maximum);
            GetMaximumNumberOfParameters(this.parsedArguments, ref maximum);
            GetMaximumNumberOfParameters(this.parsedStartupDirectory, ref maximum);

            return maximum;
        }

        public int GetMininumNumberOfParameters()
        {
            int? mininum = null;

            this.TryParseArguments();
            this.TryParseExecutionPath();
            this.TryParseStartupDirectory();

            GetMininumNumberOfParameters(this.parsedExecutionPath, ref mininum);
            GetMininumNumberOfParameters(this.parsedArguments, ref mininum);
            GetMininumNumberOfParameters(this.parsedStartupDirectory, ref mininum);

            return mininum == null ? 0 : mininum.Value;
        }

        public bool TakesParameterCountOf(int numberOfParameters)
        {
            int? mininumNumberOfParameters = this.GetMininumNumberOfParameters();
            int? maximumNumberOfParameters = this.GetMaximumNumberOfParameters();

            return mininumNumberOfParameters <= numberOfParameters
                && (maximumNumberOfParameters == null || maximumNumberOfParameters >= numberOfParameters);
        }

        private void TryParseExecutionPath()
        {
            if (this.parsedExecutionPath == null)
            {
                ItlCompiler compiler = new ItlCompiler();
                FeedbackCollection feedback;
                this.parsedExecutionPath = compiler.Compile(ItlType.Standard, this.executionPath, out feedback);
            }
        }

        private void TryParseArguments()
        {
            if (this.parsedArguments == null)
            {
                ItlCompiler compiler = new ItlCompiler();
                FeedbackCollection feedback;
                this.parsedArguments = compiler.Compile(ItlType.Standard, this.arguments, out feedback);
            }
        }

        private void TryParseStartupDirectory()
        {
            if (this.parsedStartupDirectory == null)
            {
                ItlCompiler compiler = new ItlCompiler();
                FeedbackCollection feedback;
                this.parsedStartupDirectory = compiler.Compile(ItlType.Standard, this.startupDirectory, out feedback);
            }
        }

        //public CommandDiff DoDiff(Command latestItem)
        //{
        //    return new CommandDiff(this, latestItem);
        //}

        private static bool GetWhetherOnlyParameterSpecifedIsN(Expression component)
        {
            ArgumentSubstitution argumentSubstitution;
            ExpressionGroup grouping;
            FunctionCall functionalSubstitution;

            if ((argumentSubstitution = component as ArgumentSubstitution) != null)
            {
                if (argumentSubstitution.SingularSubstitution)
                {
                    if (argumentSubstitution.ArgumentNumber != null)
                    {
                        return false;
                    }
                }
                else if (argumentSubstitution.ArgumentNumber != 1 || argumentSubstitution.LastArgumentNumber != null)
                {
                    return false;
                }

                OptionalSubsitution optionalSubstitution = component as OptionalSubsitution;

                if (optionalSubstitution != null && optionalSubstitution.DefaultValue != null)
                {
                    return GetWhetherOnlyParameterSpecifedIsN(optionalSubstitution.DefaultValue);
                }

                return true;
            }
            else if ((grouping = component as ExpressionGroup) != null)
            {
                foreach (Expression childComponent in grouping.Expressions)
                {
                    return GetWhetherOnlyParameterSpecifedIsN(childComponent);
                }
            }
            else if ((functionalSubstitution = component as FunctionCall) != null)
            {
                foreach (Expression childComponent in functionalSubstitution.Parameters)
                {
                    return GetWhetherOnlyParameterSpecifedIsN(childComponent);
                }
            }

            return true;
        }

        public static void GetMaximumNumberOfParameters(Expression component, ref int? max)
        {
            if (max == null)
            {
                return;
            }

            ArgumentSubstitution argumentSubstitution;
            ExpressionGroup grouping;
            FunctionCall functionalSubstitution;

            if ((argumentSubstitution = component as ArgumentSubstitution) != null)
            {
                if (argumentSubstitution is ImperativeSubstitution && max != null && max < 1)
                {
                    max = 1;
                }

                if (argumentSubstitution.SingularSubstitution)
                {
                    if (argumentSubstitution.ArgumentNumber == null || argumentSubstitution.ArgumentNumber > max)
                    {
                        max = argumentSubstitution.ArgumentNumber;
                    }
                }
                else if (argumentSubstitution.ArgumentNumber == null || argumentSubstitution.LastArgumentNumber == null)
                {
                    max = null;
                }
                else if (argumentSubstitution.LastArgumentNumber > max)
                {
                    max = argumentSubstitution.LastArgumentNumber;
                }
                else if (argumentSubstitution.ArgumentNumber > max)
                {
                    max = argumentSubstitution.ArgumentNumber;
                }

                OptionalSubsitution optionalSubstitution = component as OptionalSubsitution;

                if (optionalSubstitution != null && optionalSubstitution.DefaultValue != null)
                {
                    GetMaximumNumberOfParameters(optionalSubstitution.DefaultValue, ref max);
                }
            }
            else if ((grouping = component as ExpressionGroup) != null)
            {
                foreach (Expression childComponent in grouping.Expressions)
                {
                    GetMaximumNumberOfParameters(childComponent, ref max);
                }
            }
            else if ((functionalSubstitution = component as FunctionCall) != null)
            {
                foreach (Expression childComponent in functionalSubstitution.Parameters)
                {
                    GetMaximumNumberOfParameters(childComponent, ref max);
                }
            }
        }

        public static void GetMininumNumberOfParameters(Expression component, ref int? min)
        {
            if (min == 0)
            {
                return;
            }

            ImperativeSubstitution argumentSubstitution;
            ExpressionGroup grouping;
            FunctionCall functionalSubstitution;

            if ((argumentSubstitution = component as ImperativeSubstitution) != null)
            {
                if (argumentSubstitution.SingularSubstitution)
                {
                    if (min == null || argumentSubstitution.ArgumentNumber < min)
                    {
                        if (argumentSubstitution.ArgumentNumber != null)
                        {
                            min = argumentSubstitution.ArgumentNumber;
                        }
                        else
                        {
                            min = 1;
                        }
                    }
                }
                else if (argumentSubstitution.ArgumentNumber == null)
                {
                    min = 1;
                }
                else if (min == null || (argumentSubstitution.LastArgumentNumber != null && argumentSubstitution.LastArgumentNumber < min))
                {
                    min = argumentSubstitution.LastArgumentNumber;
                }
                else if (argumentSubstitution.ArgumentNumber > min)
                {
                    if (argumentSubstitution.ArgumentNumber != null)
                    {
                        min = argumentSubstitution.ArgumentNumber;
                    }
                    else
                    {
                        min = 1;
                    }
                }

                OptionalSubsitution optionalSubstitution = component as OptionalSubsitution;

                if (optionalSubstitution != null && optionalSubstitution.DefaultValue != null)
                {
                    GetMininumNumberOfParameters(optionalSubstitution.DefaultValue, ref min);
                }
            }
            else if ((grouping = component as ExpressionGroup) != null)
            {
                foreach (Expression childComponent in grouping.Expressions)
                {
                    GetMininumNumberOfParameters(childComponent, ref min);
                }
            }
            else if ((functionalSubstitution = component as FunctionCall) != null)
            {
                foreach (Expression childComponent in functionalSubstitution.Parameters)
                {
                    GetMininumNumberOfParameters(childComponent, ref min);
                }
            }
        }

        internal XmlNode ToXml(XmlDocument document)
        {
            XmlNode node = document.CreateElement("Command");

            if (this.id != null)
            {
                node.Attributes.Append(XmlUtilities.CreateAttribute("id", this.id.ToString(), document));
            }

            node.Attributes.Append(XmlUtilities.CreateAttribute("name", this.name, document));
            if (this.runAsAdministrator)
            {
                node.Attributes.Append(XmlUtilities.CreateAttribute("elevate", this.runAsAdministrator, document));
            }

            if (this.startingWindowState != ProcessWindowStyle.Normal)
            {
                node.Attributes.Append(XmlUtilities.CreateAttribute("startupWindowState", this.startingWindowState, document));
            }

            if (this.showParameterHistory)
            {
                node.Attributes.Append(XmlUtilities.CreateAttribute("showParamHistory", this.showParameterHistory, document));
            }

            node.AppendChild(XmlUtilities.CreateNode("Invokes", this.executionPath, document));

            if (this.arguments.Length > 0)
            {
                node.AppendChild(XmlUtilities.CreateNode("Arguments", this.arguments, document));
            }

            if (this.startupDirectory.Length > 0)
            {
                XmlNode startupDirectoryNode = XmlUtilities.CreateNode("StartupDirectory", this.StartupDirectory, document);
                if (this.UseExecutionDirectoryAsStartupDirectory != DefaultUseExecutionDirectoryAsStartupDirectory)
                {
                    startupDirectoryNode.Attributes.Append(
                        XmlUtilities.CreateAttribute("UseExecutionDirectory", this.UseExecutionDirectoryAsStartupDirectory, document));
                }

                node.AppendChild(startupDirectoryNode);
            }

            if (this.parametersMetaInfo.Count > 0)
            {
                node.AppendChild(this.parametersMetaInfo.ToXml("ParametersInfo", document));
            }

            if (this.notes.Length > 0)
            {
                node.AppendChild(XmlUtilities.CreateNode("Notes", this.notes, document));
            }

            return node;
        }

        public static string[] GetAliasesFromName(string name)
        {
            if (String.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException("name");
            }

            TrieList actualAliases = new TrieList(SortMode.DecendingFromLastAdded);
            foreach (string alias in name.Split(AliasSplitChars))
            {
                if (!actualAliases.Contains(alias, CaseSensitivity.Insensitive))
                {
                    actualAliases.Add(alias);
                }
            }

            return actualAliases.ToArray();
        }

        public string TranslateArgument(string suppliedValue, int parameterIndex, List listFrom)
        {
            CommandParameterMetaInfo info = this.ParametersMetaInfo.GetItemContainingParameterSuggestionFor(parameterIndex + 1);
            ParameterSuggestion suggestion = info == null ? null : info.ParameterSuggestion;
            return ParameterSuggestion.TranslateArgument(suppliedValue, listFrom, suggestion);
        }

        public static string CleanName(string name)
        {
            if (String.IsNullOrEmpty(name) || Command.ReservedNames.Contains(name.ToUpperInvariant()))
            {
                return null;
            }

            string[] aliases = Command.GetAliasesFromName(name);

            List<StringBuilder> cleanedAliases = new List<StringBuilder>();

            foreach (string alias in aliases)
            {
                if (alias.Length > 0)
                {
                    StringBuilder cleanedAlias = new StringBuilder();

                    for (int i = 0; i < alias.Length; i++)
                    {
                        bool valid = true;
                        char c = alias[i];
                        if (i == 0 || i == alias.Length - 1)
                        {
                            valid = c != '.';
                        }

                        if (valid)
                        {
                            valid = !IllegalNameChars.Contains(c);
                        }

                        if (valid)
                        {
                            cleanedAlias.Append(c);
                        }
                    }

                    cleanedAliases.Add(cleanedAlias);
                }
            }

            if (cleanedAliases.Count > 0)
            {
                StringBuilder cleanedName = new StringBuilder();
                for (int i = 0; i < cleanedAliases.Count; i++)
                {
                    cleanedName.Append(cleanedAliases[i]);
                    if (i < cleanedAliases.Count - 1)
                    {
                        cleanedName.Append(',');
                    }
                }

                return cleanedName.ToString();
            }

            return null;
        }

        public static bool IsValidCommand(Command command)
        {
            if (command == null)
            {
                return false;
            }

            return Command.IsValidName(command.name)
                && !String.IsNullOrEmpty(command.executionPath);
        }

        public static bool IsValidName(string name)
        {
            if (String.IsNullOrEmpty(name) || Command.ReservedNames.Contains(name.ToUpperInvariant()))
            {
                return false;
            }

            string[] aliases = Command.GetAliasesFromName(name);

            bool valid;
            foreach (string alias in aliases)
            {
                for (int i = 0; i < alias.Length; i++)
                {
                    char c = alias[i];
                    if (i == 0 || i == alias.Length - 1)
                    {
                        valid = c != '.' && c != ',';
                        if (!valid)
                        {
                            return false;
                        }
                    }

                    valid = !IllegalNameChars.Contains(c);
                    if (!valid)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        internal static Command FromXml(XmlNode node)
        {
            if (node.Name.ToLowerInvariant() != XmlAlias)
            {
                throw new ArgumentException("The node is not named " + XmlAlias + ".");
            }
            //else if (idGenerator == null)
            //{
            //    throw new ArgumentNullException("idGenerator");
            //}

            string name = null;
            string executionPath = null;
            string arguments = String.Empty;
            string startupDirectory = String.Empty;
            string notes = String.Empty;
            bool showParamHistory = false;
            bool runAsAdmin = false;
            ProcessWindowStyle startupWindowState = ProcessWindowStyle.Normal;
            Id id = null;
            bool useExecutionDirectoryAsStartupDirectory = DefaultUseExecutionDirectoryAsStartupDirectory;
            CommandParameterMetaInfoCollection parametersMetaInfo = null;

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
                    case "ELEVATE":
                        runAsAdmin = Utilities.TryParseBoolean(attribute.Value, runAsAdmin);
                        //try
                        //{
                        //    runAsAdmin = Convert.ToBoolean(attribute.Value);
                        //}
                        //catch (FormatException)
                        //{
                        //}

                        break;
                    case "SAVEPARAMHISTORY": // Backward compatability for 0.7 data
                    case "SHOWPARAMHISTORY":
                        showParamHistory = Utilities.TryParseBoolean(attribute.Value, showParamHistory);
                        //try
                        //{
                        //    showParamHistory = Convert.ToBoolean(attribute.Value);
                        //}
                        //catch (FormatException)
                        //{
                        //}

                        break;
                    case "STARTUPWINDOWSTATE":
                        try
                        {
                            startupWindowState = (ProcessWindowStyle)Enum.Parse(typeof(ProcessWindowStyle), attribute.Value);
                        }
                        catch (ArgumentException)
                        {
                        }

                        break;
                    default:
                        break;
                }
            }

            foreach (XmlNode infoNode in node.ChildNodes)
            {
                switch (infoNode.Name.ToUpperInvariant())
                {
                    case "PARAMETERSINFO":
                        parametersMetaInfo = CommandParameterMetaInfoCollection.FromXml(infoNode);
                        break;
                    case "INVOKES":
                        executionPath = infoNode.InnerText;
                        break;
                    case "ARGUMENTS":
                        arguments = infoNode.InnerText;
                        break;
                    case "STARTUPDIRECTORY":
                        foreach (XmlAttribute attribute in infoNode.Attributes)
                        {
                            switch (attribute.Name.ToUpperInvariant())
                            {
                                case "USEEXECUTIONDIRECTORY":
                                    useExecutionDirectoryAsStartupDirectory = Utilities.TryParseBoolean(
                                        attribute.Value,
                                        useExecutionDirectoryAsStartupDirectory);
                                    //try
                                    //{
                                    //    useExecutionDirectoryAsStartupDirectory = Convert.ToBoolean(attribute.Value);
                                    //}
                                    //catch (FormatException)
                                    //{
                                    //}

                                    break;
                                default:
                                    break;
                            }
                        }

                        startupDirectory = infoNode.InnerText;

                        break;
                    case "NOTES":
                        notes = infoNode.InnerText;
                        break;
                    default:
                        break;
                }
            }

            if (name == null)
            {
                throw new LoadException("The command's name was not found.");
            }
            else if (executionPath == null)
            {
                throw new LoadException("The commands's execution path was not found.");
            }
            else if (reservedNames.Contains(name))
            {
                throw new LoadException("the command's name is reserved and cannot be used.");
            }

            //if (id == null)
            //{
            //    id = idGenerator.GenerateId();
            //}

            return new Command(name, executionPath, arguments, runAsAdmin, startupWindowState, notes, startupDirectory, showParamHistory, useExecutionDirectoryAsStartupDirectory, id, parametersMetaInfo);
        }

        public static string GenerateItemId(Command command, List listFrom)
        {
            return String.Format(CultureInfo.InvariantCulture, "{0}:{1}", listFrom.FolderName, command.Id);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public override string ToString()
        {
            return this.Name;
        }
    }
}
