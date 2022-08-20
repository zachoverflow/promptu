using System;
using System.Collections.Generic;
using System.Text;
//using ZachJohnson.Promptu.DynamicEntryModel;

namespace ZachJohnson.Promptu.UserModel.Collections
{
    internal class AssemblyReferenceCollectionComposite
    {
        private ListCollection lists;
        private List priorityList;

        public AssemblyReferenceCollectionComposite(ListCollection lists)
            : this(lists, null)
        {
        }

        public AssemblyReferenceCollectionComposite(ListCollection lists, List priorityList)
        {
            if (lists == null)
            {
                throw new ArgumentNullException("lists");
            }

            this.lists = lists;
            this.priorityList = priorityList;
        }

        public List PriorityList
        {
            get { return this.priorityList; }
        }

        public ListCollection Lists
        {
            get { return this.lists; }
        }

        public CompositeItem<AssemblyReference, List> this[string name]
        {
            get
            {
                CompositeItem<AssemblyReference, List> compositeItem = null;
                this.Itterate(new LoopAction<List>(delegate(List list)
                    {
                        using (DdMonitor.Lock(list.AssemblyReferences))
                        {
                            foreach (AssemblyReference reference in list.AssemblyReferences)
                            {
                                if (reference.Name == name)
                                {
                                    compositeItem = new CompositeItem<AssemblyReference, List>(reference, list);
                                    return false;
                                }
                            }
                        }

                        return true;
                    }));

                if (compositeItem != null)
                {
                    return compositeItem;
                }

                throw new ArgumentOutOfRangeException("No AssemblyReference with that name was found in the list.");
            }
        }

        public bool Contains(string name)
        {
            bool found = false;

            this.Itterate(new LoopAction<List>(delegate(List list)
            {
                using (DdMonitor.Lock(list.AssemblyReferences))
                {
                    foreach (AssemblyReference item in list.AssemblyReferences)
                    {
                        if (name == item.Name)
                        {
                            found = true;
                            return false;
                        }
                    }
                }

                return true;
            }));

            return found;
        }

        private void Itterate(LoopAction<List> action)
        {
            int startingIndex = 0;

            if (this.priorityList != null)
            {
                startingIndex = -1;
            }

            for (int i = startingIndex; i < this.lists.Count; i++)
            {
                List list;

                if (i < 0)
                {
                    list = this.priorityList;
                }
                else
                {
                    list = this.lists[0];
                }

                if (!list.Enabled)
                {
                    continue;
                }

                if (!action(list))
                {
                    break;
                }
            }
        }
    }
}
