using System;
using System.Collections.Generic;
using System.Text;
using ZachJohnson.Promptu.Collections;
using System.Diagnostics;
using System.Threading;

namespace ZachJohnson.Promptu.UserModel.Collections
{
    internal class FunctionsCommandsComposite
    {
        private ListCollection lists;
        private TrieDictionary<GroupedCompositeItem> composite;
        private bool onlyEnabledLists;
        protected FunctionsCommandsCompositeMediator mediator;
        private object compositeLock = new object();

        public FunctionsCommandsComposite(ListCollection lists, bool onlyEnabledLists)
        {
            if (lists == null)
            {
                throw new ArgumentNullException("lists");
            }

            this.onlyEnabledLists = onlyEnabledLists;

            this.composite = new TrieDictionary<GroupedCompositeItem>(SortMode.Alphabetical);
            this.lists = lists;
            this.Regenerate();
        }

        public FunctionsCommandsCompositeMediator Mediator
        {
            get 
            { 
                return this.mediator; 
            }

            set
            {
                if (this.mediator != null)
                {
                    this.mediator.RemoveClient(this);
                }

                this.mediator = value;

                if (this.mediator != null)
                {
                    this.mediator.AddClient(this);
                }
            }
        }

        //public event EventHandler Regenerated;

        public ListCollection Lists
        {
            get 
            {
                return this.lists; 
            }
        }

        public IKeyAbstractionCollection AllKeys
        {
            get 
            {
                return this.SynchronizedComposite.Keys;
            }
        }

        //public string[] GetAllKeysToArray()
        //{
        //    return this.composite.GetAllKeysToArray();
        //}

        public GroupedCompositeItem this[string name]
        {
            get
            {
                return this.SynchronizedComposite[name, CaseSensitivity.Insensitive];
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

        public Dictionary<string, GroupedCompositeItem> FindAllThatStartWith(string filter)
        {
            return this.SynchronizedComposite.FindAllThatStartWith(filter, CaseSensitivity.Insensitive);
        }

        public GroupedCompositeItem TryGetItem(string name, out bool found)
        {
            return this.SynchronizedComposite.TryGetItem(name, CaseSensitivity.Insensitive, out found);
        }

        public bool Contains(string name)
        {
            return this.SynchronizedComposite.Contains(name, CaseSensitivity.Insensitive);
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

        //private void SyncWithMediator(ParameterlessVoid action)
        //{
        //    FunctionsCommandsCompositeMediator mediator = this.mediator;
        //    if (mediator != null)
        //    {
        //        lock (mediator.RegenerationSyncToken)
        //        {
        //            action();
        //        }
        //    }
        //    else
        //    {
        //        action();
        //    }
        //}

        public string TryFind(string nameStartsWith)
        {
            return this.SynchronizedComposite.TryFindKey(nameStartsWith, CaseSensitivity.Insensitive);
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

        protected TrieDictionary<GroupedCompositeItem> SynchronizedComposite
        {
            get
            {
                //FunctionsCommandsCompositeMediator mediator = this.mediator;
                //if (mediator != null)
                //{
                    //DateTime current = DateTime.Now;
                    //Trace.WriteLine("going to access the composite on thread:" + Thread.CurrentThread.ManagedThreadId.ToString());
                    using (DdMonitor.Lock(this.compositeLock))
                    {
                        //TimeSpan accessWaitLength = DateTime.Now - current;
                        //if (accessWaitLength.TotalMilliseconds > 20)
                        //{
                            //Trace.WriteLine(String.Format("thread {0} had to wait for {1} milliseconds", Thread.CurrentThread.ManagedThreadId.ToString(), accessWaitLength.TotalMilliseconds));
                        //}
                        //Trace.WriteLine("got access to the composite on thread:" + Thread.CurrentThread.ManagedThreadId.ToString());
                        return this.composite;
                    }
                //}
                //else
                //{
                    //return this.composite;
                //}
            }
        }

        public void Regenerate()
        {
            //if (allowRegenerate)
            //{
            TrieDictionary<GroupedCompositeItem> newComposite = new TrieDictionary<GroupedCompositeItem>(SortMode.Alphabetical);
            newComposite.Add("setup", null);
            newComposite.Add("quit", null);
            newComposite.Add("synchronize", null);
            newComposite.Add("help", null);
            newComposite.Add("about", null);
            for (int i = 0; i < this.lists.Count; i++)
            {
                List list = this.lists[i];
                if (this.onlyEnabledLists && !list.Enabled)
                {
                    continue;
                }

                using (DdMonitor.Lock(list.Commands))
                {
                    foreach (Command command in list.Commands)
                    {
                        string[] aliases = command.GetAliases();
                        for (int j = 0; j < aliases.Length; j++)
                        {
                            string alias = aliases[j];
                            if (!newComposite.Contains(alias, CaseSensitivity.Insensitive))
                            {
                                newComposite.Add(alias, new GroupedCompositeItem());
                            }

                            newComposite[alias, CaseSensitivity.Insensitive].Commands.Add(new CompositeItem<Command, List>(command, list));
                        }
                    }
                }

                using (DdMonitor.Lock(list.Functions))
                {
                    foreach (Function function in list.Functions)
                    {
                        if (!newComposite.Contains(function.Name, CaseSensitivity.Insensitive))
                        {
                            newComposite.Add(function.Name, new GroupedCompositeItem());
                        }

                        if (function.ReturnValue == ReturnValue.String)
                        {
                            newComposite[function.Name, CaseSensitivity.Insensitive].StringFunctions.Add(new CompositeItem<Function, List>(function, list));
                        }
                        else
                        {
                            newComposite[function.Name, CaseSensitivity.Insensitive].NonStringFunctions.Add(new CompositeItem<Function, List>(function, list));
                        }
                    }
                }
            }

            using (DdMonitor.Lock(this.compositeLock))
            {
                this.composite = newComposite;
            }
            //}
        }
    }
}
