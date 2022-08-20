using System;
using System.Collections.Generic;
using System.Text;
using ZachJohnson.Promptu.UserModel.Collections;

namespace ZachJohnson.Promptu.UserModel.Differencing
{
    internal class CommandDiffMaker : DiffMaker<
        Command, 
        CommandDiff, 
        CommandCollection>
    {
        public CommandDiffMaker()
        {
        }

        protected override CommandDiff CreateDiff(Command baseItem, Command latestItem)
        {
            return new CommandDiff(baseItem, latestItem);
        }

        //protected override Command GetCorrespondingItemFrom(ICommand item, CommandCollection collection, CommandIdentifierChangeCollection identifierChanges, out int? index)
        //{
        //    return collection.TryGet(item.Name, identifierChanges, true, out index);
        //}

        //protected override CommandDiff[]  MakeMultipleDiffsFromItemCore(Command item, ItemIs itemIs, params CommandCollection[] latestCollections)
        //{
        //    CommandDiff[] items = new CommandDiff[latestCollections.Length];

        //    for (int i = 0; i < latestCollections.Length; i++)
        //    {
        //        Command otherItem = null;
        //        CommandCollection commandCollection = latestCollections[i];
        //        if (commandCollection.Contains(item.Name))
        //        {
        //            otherItem = commandCollection[item.Name];
        //            //if (itemIs == ItemIs.Latest)
        //            //{
        //            //    items[i] = commandCollection[item.Name].DoDiff(item);
        //            //}
        //            //else
        //            //{
        //            //    items[i] = item.DoDiff(commandCollection[item.Name]);
        //            //}
        //        }

        //        if (itemIs == ItemIs.Latest)
        //        {
        //            items[i] = new CommandDiff(otherItem, item);
        //        }
        //        else
        //        {
        //            items[i] = new CommandDiff(item, otherItem);
        //        }
        //    }

        //    return items;
        //}

        //protected override DiffCollection<CommandDiff>[] MakeMultipleDiffsCore(CommandCollection baseCollection, params CommandCollection[] latestCollections)
        //{
        //    DiffCollection<CommandDiff>[] diffs = new DiffCollection<CommandDiff>[latestCollections.Length];
        //    List<int>[] diffedIndexes = new List<int>[latestCollections.Length];

        //    for (int i = 0; i < baseCollection.Count; i++)
        //    {
        //        Command baseCommand = baseCollection[i];
        //        for (int j = 0; j < latestCollections.Length; j++)
        //        {
        //            if (i == 0)
        //            {
        //                diffedIndexes[j] = new List<int>();
        //                diffs[j] = new DiffCollection<CommandDiff>();
        //            }

        //            Command latestItem = null;
        //            CommandCollection commandCollection = latestCollections[j];
        //            if (commandCollection.Contains(baseCommand.Name))
        //            {
        //                latestItem = commandCollection[baseCommand.Name];
        //                diffedIndexes[j].Add(commandCollection.IndexOf(latestItem));
        //            }

        //            diffs[i].Add(new CommandDiff(baseCommand, latestItem));
        //        }
        //    }

        //    for (int i = 0; i < latestCollections.Length; i++)
        //    {
        //        DiffCollection<CommandDiff> diffsWithThisCollection = diffs[i];
        //        CommandCollection collection = latestCollections[i];
        //        List<int> diffedIndexesForThisCollection = diffedIndexes[i];

        //        if (diffedIndexesForThisCollection.Count != collection.Count)
        //        {
        //            for (int j = 0; j < collection.Count; j++)
        //            {
        //                if (!diffedIndexesForThisCollection.Contains(j))
        //                {
        //                    diffsWithThisCollection.Add(new CommandDiff(null, collection[j]));
        //                }
        //            }
        //        }
        //    }

        //    return diffs;
        //}
    }
}
