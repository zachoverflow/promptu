using System;
using System.Collections.Generic;
using System.Text;
using ZachJohnson.Promptu.Collections;

namespace ZachJohnson.Promptu.UserModel.Collections
{
    // TODO remove if not needed
    internal class CommandCollectionComposite
    {
        private ListCollection lists;
        private TrieDictionary<CompositeItem<Command, List>> composite;

        public CommandCollectionComposite(ListCollection lists)
        {
            if (lists == null)
            {
                throw new ArgumentNullException("lists");
            }

            this.composite = new TrieDictionary<CompositeItem<Command, List>>(SortMode.Alphabetical);
            this.lists = lists;
            foreach (List list in lists)
            {
                this.AttachToList(list);
            }

            this.lists.ItemAdded += this.ListAdded;
            this.lists.ItemRemoved += this.ListRemoved;
            this.Regenerate();
        }

        public ListCollection Lists
        {
            get { return this.lists; }
        }

        public CompositeItem<Command, List> this[string name]
        {
            get
            {
                return this.composite[name, CaseSensitivity.Insensitive];
                //foreach (List list in this.lists)
                //{
                //    foreach (Command command in list.Commands)
                //    {
                //        if (command.Name == name)
                //        {
                //            return new CompositeItem<Command,List>(command, list);
                //        }
                //    }
                //}

                //throw new ArgumentOutOfRangeException("Command not found.");
            }
        }

        public bool Contains(string name)
        {
            return this.composite.Contains(name, CaseSensitivity.Insensitive);
            //foreach (List list in this.lists)
            //{
            //    foreach (Command command in list.Commands)
            //    {
            //        if (command.Name == name)
            //        {
            //            return true;
            //        }
            //    }
            //}

            //return false;
        }

        public string Find(string nameStartsWith)
        {
            return this.composite.TryFindKey(nameStartsWith, CaseSensitivity.Insensitive);
        }

        //public CommandCollection GetComposite()
        //{
        //    CommandCollection composite = new CommandCollection();

        //    foreach (List list in this.lists)
        //    {
        //        foreach (Command command in list.Commands)
        //        {
        //            if (!composite.Contains(command.Name))
        //            {
        //                composite.Add(command);
        //            }
        //        }
        //    }

        //    return composite;
        //}

        public void Regenerate()
        {
            this.composite.Clear();
            this.composite.Add("setup", null);
            this.composite.Add("quit", null);
            for (int i = 0; i < this.lists.Count; i++)
            {
                List list = this.lists[this.lists.Count - i - 1];
                if (!list.Enabled)
                {
                    continue;
                }

                using (DdMonitor.Lock(list.Commands))
                {
                    System.Threading.Thread.Sleep(5000);
                    foreach (Command command in list.Commands)
                    {
                        if (!this.composite.Contains(command.Name, CaseSensitivity.Insensitive))
                        {
                            string[] aliases = command.GetAliases();
                            for (int j = 0; j < aliases.Length; j++)
                            {
                                this.composite.Add(aliases[j], new CompositeItem<Command, List>(command, list));
                            }
                        }
                    }
                }
            }
        }

        private void ListAdded(object sender, ItemAndIndexEventArgs<List> e)
        {
            this.Regenerate();
            this.AttachToList(e.Item);
        }

        private void ListRemoved(object sender, ItemAndIndexEventArgs<List> e)
        {
            this.Regenerate();
            e.Item.Commands.ItemAdded -= this.CommandsChanged;
            e.Item.Commands.ItemRemoved -= this.CommandsChanged;
        }

        private void AttachToList(List list)
        {
            list.Commands.ItemAdded += this.CommandsChanged;
            list.Commands.ItemRemoved += this.CommandsChanged;
        }

        private void CommandsChanged(object sender, EventArgs e)
        {
            this.Regenerate();
        }
    }
}
