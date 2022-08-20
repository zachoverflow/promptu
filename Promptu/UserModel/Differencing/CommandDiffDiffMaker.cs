using System;
using System.Collections.Generic;
using System.Text;
using ZachJohnson.Promptu.UserModel.Collections;

namespace ZachJohnson.Promptu.UserModel.Differencing
{
    internal class CommandDiffDiffMaker : DiffDiffMaker<CommandDiffDiff, CommandDiff, Command, CommandCollection>
    {
        public CommandDiffDiffMaker()
        {
        }

        protected override CommandDiffDiff CreateDiffDiff(
            CommandDiff priorityDiff, 
            CommandDiff secondaryDiff)
        {
            return new CommandDiffDiff(priorityDiff, secondaryDiff);
        }
    }
}
