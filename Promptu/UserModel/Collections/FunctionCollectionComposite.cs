using System;
using System.Collections.Generic;
using System.Text;
//using ZachJohnson.Promptu.DynamicEntryModel;

namespace ZachJohnson.Promptu.UserModel.Collections
{
    internal class FunctionCollectionComposite
    {
        private ListCollection lists;
        private List priorityList;

        public FunctionCollectionComposite(ListCollection lists)
            : this(lists, null)
        {
        }

        public FunctionCollectionComposite(ListCollection lists, List priorityList)
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

        public CompositeItem<Function, List> this[string name, ReturnValue? returnValue, string parameterSignature]
        {
            get
            {
                string uppercaseName = name.ToUpperInvariant();

                CompositeItem<Function, List> compositeItem = null;
                this.Itterate(new LoopAction<List>(delegate(List list)
                    {
                        using (DdMonitor.Lock(list.Functions))
                        {
                            foreach (Function item in list.Functions)
                            {
                                if (item.ParameterSignature == parameterSignature && (returnValue == null || (returnValue.Value & item.ReturnValue) != 0) && uppercaseName == item.Name.ToUpperInvariant())
                                {
                                    compositeItem = new CompositeItem<Function, List>(item, list);
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

                throw new ArgumentException("No function has that name.");
            }
        }

        public CompositeItem<Function, List> TryGet(string name, ReturnValue? returnValue, string parameterSignature)
        {
            string uppercaseName = name.ToUpperInvariant();

            CompositeItem<Function, List> compositeItem = null;
            this.Itterate(new LoopAction<List>(delegate(List list)
            {
                using (DdMonitor.Lock(list.Functions))
                {
                    foreach (Function item in list.Functions)
                    {
                        if (item.ParameterSignature == parameterSignature && (returnValue == null || (returnValue.Value & item.ReturnValue) != 0) && uppercaseName == item.Name.ToUpperInvariant())
                        {
                            compositeItem = new CompositeItem<Function, List>(item, list);
                            return false;
                        }
                    }
                }

                return true;
            }));

            return compositeItem;
        }

        public bool Contains(string name, ReturnValue? returnValue, string parameterSignature)
        {
            bool found = false;

            string uppercaseName = name.ToUpperInvariant();

            this.Itterate(new LoopAction<List>(delegate(List list)
                    {
                        using (DdMonitor.Lock(list.Functions))
                        {
                            foreach (Function item in list.Functions)
                            {
                                if (item.ParameterSignature == parameterSignature && (returnValue == null || (returnValue.Value & item.ReturnValue) != 0) && uppercaseName == item.Name.ToUpperInvariant())
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

        public bool ContainsAnyNamed(string name, ReturnValue? returnValue)
        {
            bool found = false;

            string uppercaseName = name.ToUpperInvariant();

            this.Itterate(new LoopAction<List>(delegate(List list)
            {
                using (DdMonitor.Lock(list.Functions))
                {
                    foreach (Function item in list.Functions)
                    {
                        if (uppercaseName == item.Name.ToUpperInvariant() && (returnValue == null || (returnValue.Value & item.ReturnValue) != 0))
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
                    list = this.lists[i];
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
