using System;
using System.Collections.Generic;
using System.Text;
using ZachJohnson.Promptu.UserModel.Collections;

namespace ZachJohnson.Promptu.UserModel.Differencing
{
    internal class CommandChangeFinder : ChangeFinder<
        Command,
        CommandDiff,
        CommandDiffMaker,
        CommandDiffDiff,
        CommandDiffDiffMaker,
        CommandCollection>
    {
        public CommandChangeFinder(CommandCollection baseCollection,
            //CommandIdentifierChangeCollection baseCollectionIdentifierChanges,
            CommandCollection priorityCollection,
            //CommandIdentifierChangeCollection priorityCollectionIdentifierChanges,
            CommandCollection secondaryCollection)
           // CommandIdentifierChangeCollection secondaryCollectionIdentifierChanges)
            : base(baseCollection,
            //baseCollectionIdentifierChanges,
            priorityCollection,
           // priorityCollectionIdentifierChanges,
            secondaryCollection)
            //secondaryCollectionIdentifierChanges)
        {
        }
    }
}
