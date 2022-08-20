using System;
using System.Collections.Generic;
using System.Text;

namespace ZachJohnson.Promptu.UserModel
{
    internal class CommandDeletedEventArgs : EventArgs
    {
        private Command deletedCommand;

        public CommandDeletedEventArgs(Command deletedCommand)
        {
            this.deletedCommand = deletedCommand;
        }

        public Command DeletedCommand
        {
            get { return this.deletedCommand; }
        }
    }
}
