//using System;
//using System.Collections.Generic;
//using System.Text;
//using ZachJohnson.Promptu.UIModel.Interfaces;

//namespace ZachJohnson.Promptu.UIModel
//{
//    internal class UIMenuItemCollectionBinding : INativeUICollection<IGenericMenuItem>
//    {
//        private InsertMethod insertMethod;
//        private ClearMethod clearMethod;
//        private RemoveMethod removeMethod;

//        public UIMenuItemCollectionBinding(InsertMethod insertMethod, ClearMethod clearMethod, RemoveMethod removeMethod)
//        {
//            if (insertMethod == null)
//            {
//                throw new ArgumentNullException("insertMethod");
//            }
//            else if (clearMethod == null)
//            {
//                throw new ArgumentNullException("clearMethod");
//            }
//            else if (removeMethod == null)
//            {
//                throw new ArgumentNullException("removeMethod");
//            }

//            this.insertMethod = insertMethod;
//            this.clearMethod = clearMethod;
//            this.removeMethod = removeMethod;
//        }

//        public delegate void InsertMethod(int index, IGenericMenuItem item);

//        public delegate void ClearMethod();

//        public delegate void RemoveMethod(IGenericMenuItem item);

//        public void Insert(int index, IGenericMenuItem item)
//        {
//            this.insertMethod(index, item);
//        }

//        public void Clear()
//        {
//            this.clearMethod();
//        }

//        public void Remove(IGenericMenuItem item)
//        {
//            this.removeMethod(item);
//        }
//    }
//}
