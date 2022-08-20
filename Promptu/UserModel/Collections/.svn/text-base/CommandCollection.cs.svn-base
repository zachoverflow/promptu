using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.ObjectModel;
using System.Collections;
using ZachJohnson.Promptu;
using System.Xml;
using System.IO;
using ZachJohnson.Promptu.Collections;
using ZachJohnson.Promptu.UserModel.Differencing;

namespace ZachJohnson.Promptu.UserModel.Collections
{
    internal class CommandCollection : ItemWithIdChangeNotifiedList<Command>
    {
        internal const string XmlAlias = "commands";
        //private List<Command> commands;
        //private CommandNameCollection names;

        public CommandCollection()
        {
            //this.commands = new List<Command>();
            //this.names = new CommandNameCollection(this.commands);
        }

        public CommandCollection Clone()
        {
            return this.CloneCore();
        }

        protected virtual CommandCollection CloneCore()
        {
            CommandCollection clone = new CommandCollection();
            using (DdMonitor.Lock(this))
            {
                foreach (Command item in this)
                {
                    clone.Add(item.Clone());
                }
            }

            return clone;
        }

        public void RemoveEntriesFromHistory(HistoryCollection history)
        {
            using (DdMonitor.Lock(this))
            {
                foreach (Command command in this)
                {
                    command.RemoveEntriesFromHistory(history);
                }
            }
        }

        //public CommandNameCollection Names
        //{
        //    get { return this.names; }
        //}

        //public bool IsReadOnly
        //{
        //    get { return false; }
        //}

        //public int Count
        //{
        //    get { return this.commands.Count; }
        //}

        public Command this[string name]
        {
            get
            {
                using (DdMonitor.Lock(this))
                {
                    foreach (Command command in this)
                    {
                        string[] aliases = command.GetAliases();
                        for (int i = -1; i < aliases.Length; i++)
                        {
                            string alias;

                            if (i < 0)
                            {
                                alias = command.Name;
                            }
                            else
                            {
                                alias = aliases[i];
                            }

                            if (alias == name)
                            {
                                return command;
                            }
                        }
                    }
                }

                throw new ArgumentOutOfRangeException("Command not found.");
            }
        }

        public Command TryGetItemNameExact(string name)
        {
            using (DdMonitor.Lock(this))
            {
                foreach (Command command in this)
                {
                    if (command.Name == name)
                    {
                        return command;
                    }
                }
            }

            return null;
        }

        public Command this[string name, int numberOfParametersAccepts]
        {
            get
            {
                using (DdMonitor.Lock(this))
                {
                    foreach (Command command in this)
                    {
                        string[] aliases = command.GetAliases();
                        for (int i = -1; i < aliases.Length; i++)
                        {
                            string alias;

                            if (i < 0)
                            {
                                alias = command.Name;
                            }
                            else
                            {
                                alias = aliases[i];
                            }

                            if (alias == name)
                            {
                                if (command.TakesParameterCountOf(numberOfParametersAccepts))
                                {
                                    return command;
                                }
                                else
                                {
                                    break;
                                }
                            }
                        }
                    }
                }

                throw new ArgumentOutOfRangeException("Command not found.");
            }
        }

        //public Command TryGet(string name)
        //{
        //    foreach (Command command in this)
        //    {
        //        string[] aliases = command.Name.Split(Command.AliasSplitChars, StringSplitOptions.RemoveEmptyEntries);
        //        for (int i = -1; i < aliases.Length; i++)
        //        {
        //            string alias;

        //            if (i < 0)
        //            {
        //                alias = command.Name;
        //            }
        //            else
        //            {
        //                alias = aliases[i];
        //            }

        //            if (alias == name)
        //            {
        //                return command;
        //            }
        //        }
        //    }

        //    return null;
        //}

        //public Command this[int index]
        //{
        //    get
        //    {
        //        return this.commands[index];
        //    }
        //}

        //public void Add(Command item)
        //{
        //    this.commands.Add(item);
        //}

        //public void AddRange(IEnumerable<Command> collection)
        //{
        //    foreach (Command item in collection)
        //    {
        //        this.Add(item);
        //    }
        //}

        //public void CopyTo(Command[] array, int arrayIndex)
        //{
        //    this.commands.CopyTo(array, arrayIndex);
        //}

        //public bool Remove(Command item)
        //{
        //    return this.commands.Remove(item);
        //}

        public bool Remove(string name)
        {
            Command found = null;

            using (DdMonitor.Lock(this))
            {
                foreach (Command command in this)
                {
                    if (command.Name == name)
                    {
                        found = command;
                        break;
                    }
                }
            }

            if (found != null)
            {
                return this.Remove(found);
            }

            return false;
        }

        public static TrieList GetConflicts(string name, TrieDictionary<string> flattenedNames)
        {
            if (name == null)
            {
                throw new ArgumentNullException("name");
            }
            else if (flattenedNames == null)
            {
                throw new ArgumentNullException("flattenedNames");
            }

            string[] aliases = Command.GetAliasesFromName(name);

            TrieList conflicts = new TrieList(SortMode.Alphabetical);

            foreach (string alias in aliases)
            {
                if (flattenedNames.Contains(alias, CaseSensitivity.Insensitive))
                {
                    string conflictingName = flattenedNames[alias, CaseSensitivity.Insensitive];
                    if (!conflicts.Contains(conflictingName, CaseSensitivity.Insensitive))
                    {
                        conflicts.Add(conflictingName);
                    }
                }
            }

            return conflicts;
        }

        //public bool Contains(Command item)
        //{
        //    return this.commands.Contains(item);
        //}

        public TrieList GetConflicts(string name, CaseSensitivity caseSensitivity, GetConflictsMode mode, string nameToIgnore)
        {
            int startIndex;
            if (mode == GetConflictsMode.NoAliases)
            {
                startIndex = 0;
            }
            else
            {
                startIndex = -1;
            }

            string wholeNameCorrectCasing;
            if (caseSensitivity == CaseSensitivity.Insensitive)
            {
                wholeNameCorrectCasing = name.ToUpperInvariant();
            }
            else
            {
                wholeNameCorrectCasing = name;
            }

            TrieList conflicts = new TrieList(SortMode.DecendingFromLastAdded);
            string[] nameAliases = Command.GetAliasesFromName(wholeNameCorrectCasing);
            string[] realAliases;
            if (caseSensitivity == CaseSensitivity.Insensitive)
            {
                realAliases = Command.GetAliasesFromName(name);
            }
            else
            {
                realAliases = nameAliases;
            }

            string nameToIgnoreCorrectCasing;

            if (caseSensitivity == CaseSensitivity.Insensitive && nameToIgnore != null)
            {
                nameToIgnoreCorrectCasing = nameToIgnore.ToUpperInvariant();
            }
            else
            {
                nameToIgnoreCorrectCasing = nameToIgnore;
            }

            using (DdMonitor.Lock(this))
            {
                foreach (Command command in this)
                {
                    string compareCommandName;
                    if (caseSensitivity == CaseSensitivity.Insensitive)
                    {
                        compareCommandName = command.Name.ToUpperInvariant();
                    }
                    else
                    {
                        compareCommandName = command.Name;
                    }

                    if (compareCommandName == nameToIgnoreCorrectCasing)
                    {
                        continue;
                    }

                    string[] aliases = command.GetAliases();
                    for (int i = startIndex; i < aliases.Length; i++)
                    {
                        string alias;
                        string realName;

                        if (i < 0)
                        {
                            realName = command.Name;
                            if (caseSensitivity == CaseSensitivity.Insensitive)
                            {
                                alias = realName.ToUpperInvariant();
                            }
                            else
                            {
                                alias = realName;
                            }
                        }
                        else
                        {
                            realName = aliases[i];
                            if (caseSensitivity == CaseSensitivity.Insensitive)
                            {
                                alias = realName.ToUpperInvariant();
                            }
                            else
                            {
                                alias = realName;
                            }
                        }

                        for (int j = startIndex; j < nameAliases.Length; j++)
                        {
                            string compareName;
                            string realCompareName;

                            if (j < 0)
                            {
                                compareName = wholeNameCorrectCasing;
                                realCompareName = name;
                            }
                            else
                            {
                                compareName = nameAliases[j];
                                realCompareName = realAliases[j];
                            }

                            if (alias == compareName)
                            {
                                if (mode == GetConflictsMode.ReturnOnlyAliases)
                                {
                                    if (!conflicts.Contains(realName, CaseSensitivity.Insensitive))
                                    {
                                        conflicts.Add(realName);
                                        break;
                                    }
                                }
                                else
                                {
                                    if (!conflicts.Contains(realCompareName, CaseSensitivity.Insensitive))
                                    {
                                        conflicts.Add(realCompareName);
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return conflicts;
        }

        public TrieList ConstructFindOptimizedStringCollection()
        {
            TrieList collection = new TrieList(SortMode.DecendingFromLastAdded);
            using (DdMonitor.Lock(this))
            {
                foreach (Command command in this)
                {
                    collection.AddRange(command.GetAliases());
                }
            }

            return collection;
        }

        public bool Contains(string name, CaseSensitivity caseSensitivity)
        {
            string wholeNameCorrectCasing;
            if (caseSensitivity == CaseSensitivity.Insensitive)
            {
                wholeNameCorrectCasing = name.ToUpperInvariant();
            }
            else
            {
                wholeNameCorrectCasing = name;
            }

            string[] nameAliases = Command.GetAliasesFromName(name);
            using (DdMonitor.Lock(this))
            {
                foreach (Command command in this)
                {
                    string[] aliases = command.GetAliases();
                    for (int i = -1; i < aliases.Length; i++)
                    {
                        string alias;

                        if (i < 0)
                        {
                            alias = command.Name;
                        }
                        else
                        {
                            alias = aliases[i];
                        }

                        for (int j = -1; j < aliases.Length; j++)
                        {
                            string compareName;

                            if (j < 0)
                            {
                                compareName = name;
                            }
                            else
                            {
                                compareName = nameAliases[j];
                            }

                            if (alias == compareName)
                            {
                                return true;
                            }
                        }
                    }
                }
            }

            return false;
        }

        //public bool Contains(string name, int numberOfParametersAccepts)
        //{
        //    foreach (Command command in this)
        //    {
        //        string[] aliases = command.Name.Split(Command.AliasSplitChars, StringSplitOptions.RemoveEmptyEntries);
        //        for (int i = -1; i < aliases.Length; i++)
        //        {
        //            string alias;

        //            if (i < 0)
        //            {
        //                alias = command.Name;
        //            }
        //            else
        //            {
        //                alias = aliases[i];
        //            }

        //            if (alias == name)
        //            {
        //                if (command.TakesParameterCountOf(numberOfParametersAccepts))
        //                {
        //                    return true;
        //                }
        //                else
        //                {
        //                    break;
        //                }
        //            }
        //        }
        //    }

        //    return false;
        //}

        public Command TryGet(string name)
        {
            int? index;
            return this.TryGet(name, out index);
            //foreach (Command command in this)
            //{
            //    string[] aliases = command.Name.Split(Command.AliasSplitChars, StringSplitOptions.RemoveEmptyEntries);
            //    for (int i = -1; i < aliases.Length; i++)
            //    {
            //        string alias;

            //        if (i < 0)
            //        {
            //            alias = command.Name;
            //        }
            //        else
            //        {
            //            alias = aliases[i];
            //        }

            //        if (alias == name)
            //        {
            //            return command;
            //        }
            //    }
            //}

            //return null;
        }

        public Command TryGet(string name, out int? index)
        {
            return TryGet(name, false, out index);
        }

        public Command TryGet(string name, bool noAliases, out int? index)
        {
            for (int i = 0; i < this.Count; i++)
            {
                Command command = this[i];
                string[] aliases = command.GetAliases();
                for (int j = -1; j < aliases.Length; j++)
                {
                    string alias;

                    if (j < 0)
                    {
                        alias = command.Name;
                    }
                    else
                    {
                        if (noAliases)
                        {
                            break;
                        }

                        alias = aliases[j];
                    }

                    if (alias == name)
                    {
                        index = i;
                        return command;
                    }
                }
            }

            index = null;
            return null;
        }

        //public Command TryGet(string name, CommandIdentifierChangeCollection identifierChanges, bool noAliases, out int? index)
        //{
        //    if (identifierChanges == null)
        //    {
        //        return this.TryGet(name, noAliases, out index);
        //    }

        //    for (int i = 0; i < this.Count; i++)
        //    {
        //        Command command = this[i];
        //        string[] aliases = command.GetAliases();
        //        for (int j = -1; j < aliases.Length; j++)
        //        {
        //            string alias;

        //            if (j < 0)
        //            {
        //                ICommand compareAs;

        //                CommandIdentifierChange change = identifierChanges.TryGetIdentifierChangeFromRevisedItem(command);

        //                if (change != null)
        //                {
        //                    compareAs = change.CreateFilter(ItemType.Base);
        //                }
        //                else
        //                {
        //                    compareAs = command;
        //                }

        //                alias = compareAs.Name;
        //            }
        //            else
        //            {
        //                if (noAliases)
        //                {
        //                    break;
        //                }

        //                alias = aliases[j];
        //            }

        //            if (alias == name)
        //            {
        //                index = i;
        //                return command;
        //            }
        //        }
        //    }

        //    index = null;
        //    return null;
        //}

        protected override List<Command> GetConflictsWithCore(Command item)
        {
            List<Command> conflicts = new List<Command>();
            if (item != null)
            {
                TrieList conflictingNames = this.GetConflicts(item.Name, CaseSensitivity.Insensitive, GetConflictsMode.ReturnOnlyAliases, null);
                foreach (string name in conflictingNames)
                {
                    Command conflict = this[name];
                    if (item.Id != conflict.Id && !conflicts.Contains(conflict))
                    {
                        conflicts.Add(conflict);
                    }
                }
            }

            return conflicts;
        }

        //protected override Command TryGetSimilarCore(Command item)
        //{
        //    if (item != null)
        //    {
        //        return this.TryGet(item.Name);
        //    }

        //    return null;
        //}

        public Command TryGet(string name, int numberOfParametersAccepts)
        {
            using (DdMonitor.Lock(this))
            {
                foreach (Command command in this)
                {
                    string[] aliases = command.GetAliases();
                    for (int i = -1; i < aliases.Length; i++)
                    {
                        string alias;

                        if (i < 0)
                        {
                            alias = command.Name;
                        }
                        else
                        {
                            alias = aliases[i];
                        }

                        if (alias == name)
                        {
                            if (command.TakesParameterCountOf(numberOfParametersAccepts))
                            {
                                return command;
                            }
                            else
                            {
                                break;
                            }
                            //int? mininumNumberOfParameters = command.GetMininumNumberOfParameters();
                            //int? maximumNumberOfParameters = command.GetTotalNumberOfPossibleParameters();
                            //if (mininumNumberOfParameters == null)
                            //{
                            //    if (numberOfParametersAccepts == 0)
                            //    {
                            //        return command;
                            //    }
                            //    else
                            //    {
                            //        break;
                            //    }
                            //}
                            //else if (mininumNumberOfParameters <= numberOfParametersAccepts)
                            //{
                            //    if (maximumNumberOfParameters == null || maximumNumberOfParameters >= numberOfParametersAccepts)
                            //    {
                            //        return command;
                            //    }
                            //}
                        }
                    }
                }
            }

            return null;
        }

        //public void Clear()
        //{
        //    this.commands.Clear();
        //}

        //public IEnumerator<Command> GetEnumerator()
        //{
        //    return this.commands.GetEnumerator();
        //}

        //IEnumerator IEnumerable.GetEnumerator()
        //{
        //    return this.commands.GetEnumerator();
        //}

        //public class CommandNameCollection : IEnumerable<string>
        //{
        //    private Dictionary<string, Command> commands;

        //    public CommandNameCollection(Dictionary<string, Command> commands)
        //    {
        //        this.commands = commands;
        //    }

        //    public IEnumerator<string> GetEnumerator()
        //    {
        //        return this.commands.Keys.GetEnumerator();
        //    }

        //    IEnumerator IEnumerable.GetEnumerator()
        //    {
        //        return this.commands.Keys.GetEnumerator();
        //    }
        //}
        public TrieDictionary<string> GetFlattenedNames()
        {
            TrieDictionary<string> flattenedNames = new TrieDictionary<string>(SortMode.Alphabetical);

            using (DdMonitor.Lock(this))
            {
                foreach (Command command in this)
                {
                    string[] aliases = command.GetAliases();
                    foreach (string alias in aliases)
                    {
                        flattenedNames.Add(alias, command.Name);
                    }
                }
            }

            return flattenedNames;
        }

        public static CommandCollection FromXml(XmlNode node)
        {
            TrieList loadedAliases = new TrieList(SortMode.DecendingFromLastAdded);
            List<int> loadedIds = new List<int>();
            CommandCollection commands = new CommandCollection();
            if (node.Name.ToLowerInvariant() == XmlAlias)
            {
                foreach (XmlNode commandNode in node.ChildNodes)
                {
                    if (commandNode.Name.ToLowerInvariant() == Command.XmlAlias)
                    {
                        try
                        {
                            Command newCommand = Command.FromXml(commandNode);
                            string[] aliases = newCommand.GetAliases();
                            if (!loadedAliases.ContainsAny(aliases, CaseSensitivity.Insensitive) 
                                && Command.IsValidCommand(newCommand))
                            {
                                if (newCommand.Id != null)
                                {
                                    if (!loadedIds.Contains(newCommand.Id.Value))
                                    {
                                        loadedIds.Add(newCommand.Id.Value);
                                    }
                                    else
                                    {
                                        newCommand.Id = null;
                                    }
                                }

                                commands.Add(newCommand);
                                loadedAliases.AddRange(aliases);
                            }
                        }
                        catch (LoadException)
                        {
                        }
                    }
                }
            }
            else
            {
                throw new ArgumentException("The node is not named " + XmlAlias + ".");
            }

            return commands;
        }

        public XmlNode ToXml(XmlDocument document)
        {
            XmlNode node = document.CreateElement("Commands");
            using (DdMonitor.Lock(this))
            {
                foreach (Command command in this)
                {
                    node.AppendChild(command.ToXml(document));
                }
            }

            return node;
        }
    }
}
