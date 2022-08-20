using System;
using System.Collections.Generic;
using System.Text;
using ZachJohnson.Promptu.Collections;
using System.Extensions;

namespace ZachJohnson.Promptu.UserModel.Collections
{
    internal class CommandParameterHistoryCollection : TrieDictionary<CommandParameterHistory>
    {
        public CommandParameterHistoryCollection()
            : base(SortMode.DecendingFromLastAdded)
        {
        }

        public void Add(string historyItem, HistoryDetails details)
        {
            if (details.ItemId != null && !Function.IsInFunctionSyntax(details.EntryValue))
            {
                string[] nameAndParameters = Command.ExtractNameAndParametersFrom(details.EntryValue);
                if (nameAndParameters.Length > 0)
                {
                    string id = details.ItemId;
                    //string id = Function.CreateStringId(nameAndParameters[0], Function.CreateAllStringParameterSignature(nameAndParameters.Length - 1));
                    bool found;
                    CommandParameterHistory historyEntry = this.TryGetItem(id, CaseSensitivity.Insensitive, out found);

                    if (!found)
                    {
                        historyEntry = new CommandParameterHistory();
                        this.Add(id, historyEntry);
                    }

                    for (int i = 1; i < nameAndParameters.Length; i++)
                    {
                        string parameter = nameAndParameters[i];
                        string unescapedParameter = null;

                        if (!parameter.StartsWith("\""))
                        {
                            return;
                        }
                        else
                        {
                            unescapedParameter = parameter.Substring(1, parameter.Length - 2).Unescape();
                        }

                        if (parameter.Length > 1)
                        {
                            try
                            {
                                historyEntry.Add(i - 1, parameter, unescapedParameter);
                            }
                            catch (ArgumentException)
                            {
                            }
                        }
                    }
                }
            }
        }

        public void RegenerateFrom(HistoryCollection history)
        {
            if (history == null)
            {
                throw new ArgumentNullException("history");
            }

            this.Clear();

            foreach (string historyEntry in history)
            {
                this.Add(historyEntry, history[historyEntry, CaseSensitivity.Sensitive]);
            }
        }
    }
}
