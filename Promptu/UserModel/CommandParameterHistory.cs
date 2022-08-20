using System;
using System.Collections.Generic;
using System.Text;
using ZachJohnson.Promptu.Collections;

namespace ZachJohnson.Promptu.UserModel
{
    internal class CommandParameterHistory
    {
        private List<TrieDictionary<HistoryDetails>> parameterHistory = new List<TrieDictionary<HistoryDetails>>();

        public CommandParameterHistory()
        {
        }

        public List<TrieDictionary<HistoryDetails>> ParameterHistory
        {
            get { return this.parameterHistory; }
        }

        public void Add(int parameterIndex, string keyValue, string detailsValue)
        {
            while (parameterIndex >= this.ParameterHistory.Count)
            {
                this.ParameterHistory.Add(new TrieDictionary<HistoryDetails>(SortMode.DecendingFromLastAdded));
            }

            if (keyValue.Length > 0)
            {
                this.ParameterHistory[parameterIndex].Add(keyValue, new HistoryDetails(detailsValue, null));
            }
        }
    }
}
