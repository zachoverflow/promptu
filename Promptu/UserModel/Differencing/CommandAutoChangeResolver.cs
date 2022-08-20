using System;
using System.Collections.Generic;
using System.Text;
using ZachJohnson.Promptu.UserModel.Collections;

namespace ZachJohnson.Promptu.UserModel.Differencing
{
    internal class CommandAutoChangeResolver 
        : AutoChangeResolver<CommandDiffDiff, CommandDiff, Command, CommandCollection>
    {
        public CommandAutoChangeResolver(CommandCollection synthesisCollection, IdGenerator synthesisIdGenerator, HistoryCollection history, DiffVersion ignoreHistoryRemovalForVersion)
            : base(synthesisCollection, synthesisIdGenerator, history, ignoreHistoryRemovalForVersion)
        {
        }

        protected override void AddCloneToCollection(Command itemToClone, CommandCollection collection)
        {
            collection.Add(itemToClone.Clone());
        }

        protected override void RemoveItemEntriesFromHistory(HistoryCollection history, Command item)
        {
            item.RemoveEntriesFromHistory(history);
        }

        //protected override void RemoveCorrespondingFromCollection(Command item, CommandCollection collection)
        //{
        //    collection.Remove(item.Name);
        //}
    }
}
