using System;
using System.Collections.Generic;
using System.Text;
using ZachJohnson.Promptu.UIModel.Interfaces;

namespace ZachJohnson.Promptu.UIModel
{
    internal class NativeUICollectionBinding<TIObject> : INativeUICollection<TIObject>
    {
        private InsertMethod insertMethod;
        private ClearMethod clearMethod;
        private RemoveMethod removeMethod;

        public NativeUICollectionBinding(InsertMethod insertMethod, ClearMethod clearMethod, RemoveMethod removeMethod)
        {
            if (insertMethod == null)
            {
                throw new ArgumentNullException("insertMethod");
            }
            else if (clearMethod == null)
            {
                throw new ArgumentNullException("clearMethod");
            }
            else if (removeMethod == null)
            {
                throw new ArgumentNullException("removeMethod");
            }

            this.insertMethod = insertMethod;
            this.clearMethod = clearMethod;
            this.removeMethod = removeMethod;
        }

        public delegate void InsertMethod(int index, TIObject item);

        public delegate void ClearMethod();

        public delegate void RemoveMethod(TIObject item);

        public void Insert(int index, TIObject item)
        {
            this.insertMethod(index, item);
        }

        public void Clear()
        {
            this.clearMethod();
        }

        public void Remove(TIObject item)
        {
            this.removeMethod(item);
        }
    }
}
