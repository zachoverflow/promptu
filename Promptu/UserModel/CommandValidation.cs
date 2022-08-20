using System;
using System.Collections.Generic;
using System.Text;

namespace ZachJohnson.Promptu.UserModel
{
    internal class CommandValidation
    {
        private Command command;
        private List list;
        private Getter<bool> cancelationCallback;

        public CommandValidation(Command command, List list, Getter<bool> cancelationCallback)
        {
            if (command == null)
            {
                throw new ArgumentNullException("command");
            }
            else if (list == null)
            {
                throw new ArgumentNullException("list");
            }

            this.command = command;
            this.list = list;
            this.cancelationCallback = cancelationCallback;
        }

        public event EventHandler ValidationCompleted;

        public void ValidateIsFileSystem()
        {
            if (this.cancelationCallback == null || !this.cancelationCallback())
            {
                this.command.IsFileSystemCommand(this.list, true);
                //if (this.cancelationCallback != null)
                //{
                //    System.Threading.Thread.Sleep(20);
                //}
            }

            this.OnValidationCompleted(EventArgs.Empty);
        }

        protected virtual void OnValidationCompleted(EventArgs e)
        {
            EventHandler handler = this.ValidationCompleted;
            if (handler != null)
            {
                handler(this, e);
            }
        }
    }
}
