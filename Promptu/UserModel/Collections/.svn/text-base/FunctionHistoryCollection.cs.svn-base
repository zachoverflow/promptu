using System;
using System.Collections.Generic;
using System.Text;
using ZachJohnson.Promptu.Collections;
using System.Extensions;

namespace ZachJohnson.Promptu.UserModel.Collections
{
    internal class FunctionHistoryCollection : TrieDictionary<FunctionHistory>
    {
        public FunctionHistoryCollection()
            : base(SortMode.DecendingFromLastAdded)
        {
        }

        public void Add(string historyItem)
        {
            if (Function.IsInFunctionSyntax(historyItem))
            {
                string[] nameAndParameters = Function.ExtractNameAndParametersFrom(historyItem);
                if (nameAndParameters.Length > 0)
                {
                    string id = Function.CreateStringId(nameAndParameters[0], Function.CreateAllStringParameterSignature(nameAndParameters.Length - 1));
                    bool found;
                    FunctionHistory historyEntry = this.TryGetItem(id, CaseSensitivity.Insensitive, out found);

                    if (!found)
                    {
                        historyEntry = new FunctionHistory();
                        this.Add(id, historyEntry);
                    }

                    for (int i = 1; i < nameAndParameters.Length; i++)
                    {
                        string parameter = nameAndParameters[i];

                        if (parameter.Length > 1)
                        {
                            //parameter = parameter.Substring(1, parameter.Length - 2).Unescape();
                            try
                            {
                                historyEntry.Add(i - 1, parameter, parameter);
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

            foreach (string historyItem in history.Keys)
            {
                this.Add(history[historyItem, CaseSensitivity.Sensitive].EntryValue);
            }
        }
    }
}
