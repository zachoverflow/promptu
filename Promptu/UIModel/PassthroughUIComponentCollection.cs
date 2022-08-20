//using System;
//using System.Collections.Generic;
//using System.Text;

//namespace ZachJohnson.Promptu.UIModel
//{
//    internal class PassthroughUIComponentCollection<TUIComponent, TComponentNativeInterface> 
//        : UIComponentCollection<TUIComponent, TComponentNativeInterface>
//        where TUIComponent : UIComponent<TUIComponent, TComponentNativeInterface>
//    {
//        private UIComponentCollection<TUIComponent, TComponentNativeInterface> collection;

//        public PassthroughUIComponentCollection()
//        {
//        }

//        public UIComponentCollection<TUIComponent, TComponentNativeInterface> PassthroughCollection
//        {
//            get
//            {
//                return this.collection;
//            }

//            set
//            {
//                var oldValue = this.collection;

//                if (oldValue != null)
//                {
//                    foreach (var item in this)
//                    {
//                        oldValue.Remove(item);
//                    }
//                }

//                this.collection = value;

//                if (value != null)
//                {
//                    foreach (var item in this)
//                    {
//                        value.Add(item);
//                    }
//                }
//            }
//        }

//        protected override void InsertIntoUnderlyingCollection(int index, TUIComponent item)
//        {
//            var collection = this.collection;
//            if (collection != null)
//            {
//                collection.Insert(index, item);
//            }
//        }

//        protected override void RemoveFromUnderlyingCollection(TUIComponent item)
//        {
//            var collection = this.collection;
//            if (collection != null)
//            {
//                collection.Remove(item);
//            }
//        }
//    }
//}
