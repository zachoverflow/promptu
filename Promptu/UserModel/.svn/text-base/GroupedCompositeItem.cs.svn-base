using System;
using System.Collections.Generic;
using System.Text;
using ZachJohnson.Promptu.Collections;

namespace ZachJohnson.Promptu.UserModel
{
    internal class GroupedCompositeItem
    {
        private List<CompositeItem<Command, List>> commands;
        private List<CompositeItem<Function, List>> stringFunctions;
        private List<CompositeItem<Function, List>> nonStringFunctions;
        //private ObjectNamespace namespaceItem;

        public GroupedCompositeItem()
        {
            //if (name == null)
            //{
            //    throw new ArgumentNullException("name");
            //}

            this.commands = new List<CompositeItem<Command, List>>();
            this.stringFunctions = new List<CompositeItem<Function, List>>();
            this.nonStringFunctions = new List<CompositeItem<Function, List>>();
        }

        //public string Name
        //{
        //    get { return this.name; }
        //}

        public List<CompositeItem<Function, List>> StringFunctions
        {
            get { return this.stringFunctions; }
        }

        public List<CompositeItem<Function, List>> NonStringFunctions
        {
            get { return this.nonStringFunctions; }
        }

        public List<CompositeItem<Command, List>> Commands
        {
            get { return this.commands; }
        }

        public GroupedCompositeItem GetUniqueItems()
        {
            GroupedCompositeItem uniqueItems = new GroupedCompositeItem();
            uniqueItems.Commands.AddRange(this.GetUniqueCommands());
            uniqueItems.StringFunctions.AddRange(this.GetUniqueStringFunctions());
            uniqueItems.NonStringFunctions.AddRange(this.GetUniqueNonStringFunctions());
            return uniqueItems;
        }

        public List<CompositeItem<Command, List>> GetUniqueCommands()
        {
            IntRangeCollection takenRanges = new IntRangeCollection();
            //List<int?> takenMaximums = new List<int?>();

            List<CompositeItem<Command, List>> uniqueCommands = new List<CompositeItem<Command, List>>();

            foreach (CompositeItem<Command, List> command in this.Commands)
            {
                IntRange range = command.Item.GetParameterRange();
                if (!takenRanges.IsTaken(range))
                {
                    uniqueCommands.Add(command);
                    takenRanges.Add(range);
                }
            }

            return uniqueCommands;
        }

        public List<CompositeItem<Function, List>> GetUniqueStringFunctions()
        {
            List<string> takenSignatures = new List<string>();

            List<CompositeItem<Function, List>> uniqueFunctions = new List<CompositeItem<Function, List>>();

            foreach (CompositeItem<Function, List> function in this.StringFunctions)
            {
                if (!takenSignatures.Contains(function.Item.ParameterSignature))
                {
                    uniqueFunctions.Add(function);
                    takenSignatures.Add(function.Item.ParameterSignature);
                }
            }

            return uniqueFunctions;
        }

        public List<CompositeItem<Function, List>> GetUniqueNonStringFunctions()
        {
            List<string> takenSignatures = new List<string>();

            List<CompositeItem<Function, List>> uniqueFunctions = new List<CompositeItem<Function, List>>();

            foreach (CompositeItem<Function, List> function in this.NonStringFunctions)
            {
                if (!takenSignatures.Contains(function.Item.ParameterSignature))
                {
                    uniqueFunctions.Add(function);
                    takenSignatures.Add(function.Item.ParameterSignature);
                }
            }

            return uniqueFunctions;
        }

        public CompositeItem<Function, List> TryGetFunction(string name, string parameterSignature)
        {
            string uppercaseName = name.ToUpperInvariant();
            foreach (CompositeItem<Function, List> entry in this.stringFunctions)
            {
                if (entry.Item.ParameterSignature == parameterSignature && entry.Item.Name.ToUpperInvariant() == uppercaseName)
                {
                    return entry;
                }
            }

            foreach (CompositeItem<Function, List> entry in this.nonStringFunctions)
            {
                if (entry.Item.ParameterSignature == parameterSignature && entry.Item.Name.ToUpperInvariant() == uppercaseName)
                {
                    return entry;
                }
            }

            return null;
        }

        public CompositeItem<Function, List> TryGetStringFunction(string name, string parameterSignature)
        {
            string uppercaseName = name.ToUpperInvariant();
            foreach (CompositeItem<Function, List> entry in this.stringFunctions)
            {
                if (entry.Item.ParameterSignature == parameterSignature && entry.Item.Name.ToUpperInvariant() == uppercaseName)
                {
                    return entry;
                }
            }

            return null;
        }

        public CompositeItem<Function, List> TryGetNonStringFunction(string name, string parameterSignature)
        {
            string uppercaseName = name.ToUpperInvariant();
            foreach (CompositeItem<Function, List> entry in this.nonStringFunctions)
            {
                if (entry.Item.ParameterSignature == parameterSignature && entry.Item.Name.ToUpperInvariant() == uppercaseName)
                {
                    return entry;
                }
            }

            return null;
        }

        public bool ContainsStringFunctionThatTakesAtLeastOneParameter
        {
            get
            {
                foreach (CompositeItem<Function, List> function in this.stringFunctions)
                {
                    if (function.Item.ParameterSignature.Length > 0)
                    {
                        return true;
                    }
                }

                return false;
            }
        }

        public bool ContainsStringFunctionThatTakesZeroParameters
        {
            get
            {
                foreach (CompositeItem<Function, List> function in this.stringFunctions)
                {
                    if (function.Item.ParameterSignature.Length == 0)
                    {
                        return true;
                    }
                }

                return false;
            }
        }

        public bool ContainsCommandThatTakesAtLeastOneParameter
        {
            get
            {
                foreach (CompositeItem<Command, List> command in this.commands)
                {
                    if (command.Item.GetMininumNumberOfParameters() > 0)
                    {
                        return true;
                    }
                }

                return false;
            }
        }

        public bool ContainsCommandThatTakesZeroParameters
        {
            get
            {
                foreach (CompositeItem<Command, List> command in this.commands)
                {
                    int? numberOfParameters = command.Item.GetMininumNumberOfParameters();
                    if (numberOfParameters == 0)
                    {
                        return true;
                    }
                }

                return false;
            }
        }

        public bool ContainsFileSystemCommand
        {
            get
            {
                foreach (CompositeItem<Command, List> command in this.commands)
                {
                    if (command.Item.IsFileSystemCommand(command.ListFrom, false))
                    {
                        return true;
                    }
                }

                return false;
            }
        }

        public CompositeItem<Command, List> TryGetCommand(string name)
        {
            int? index;
            return this.TryGetCommand(name, false, out index);
        }

        public CompositeItem<Command, List> TryGetCommand(string name, int parameterCount)
        {
            string uppercaseName = name.ToUpperInvariant();
            for (int i = 0; i < this.commands.Count; i++)
            {
                CompositeItem<Command, List> command = this.commands[i];
                if (command.Item.TakesParameterCountOf(parameterCount))
                {
                    string[] aliases = command.Item.GetAliases();
                    for (int j = -1; j < aliases.Length; j++)
                    {
                        string alias;

                        if (j < 0)
                        {
                            alias = command.Item.Name.ToUpperInvariant();
                        }
                        else
                        {
                            //if (noAliases)
                            //{
                            //    break;
                            //}

                            alias = aliases[j].ToUpperInvariant();
                        }

                        if (alias == uppercaseName)
                        {
                            //index = i;
                            return command;
                        }
                    }
                }
            }

            //index = null;
            return null;
        }

        public CompositeItem<Command, List> TryGetCommand(string name, bool noAliases, out int? index)
        {
            string uppercaseName = name.ToUpperInvariant();
            for (int i = 0; i < this.commands.Count; i++)
            {
                CompositeItem<Command, List> command = this.commands[i];
                string[] aliases = command.Item.GetAliases();
                for (int j = -1; j < aliases.Length; j++)
                {
                    string alias;

                    if (j < 0)
                    {
                        alias = command.Item.Name.ToUpperInvariant();
                    }
                    else
                    {
                        if (noAliases)
                        {
                            break;
                        }

                        alias = aliases[j].ToUpperInvariant();
                    }

                    if (alias == uppercaseName)
                    {
                        index = i;
                        return command;
                    }
                }
            }

            index = null;
            return null;
        }

        //public ObjectNamespace NamespaceItem
        //{
        //    get { return this.namespaceItem; }
        //    set { this.namespaceItem = value; }
        //}

        //public GroupedCompositeItem TryGetCompositeItemFor(string name)
        //{
        //    string name = 
        //}

        //private GetNamespaceFor(string[] namesToDefine;
    }
}
