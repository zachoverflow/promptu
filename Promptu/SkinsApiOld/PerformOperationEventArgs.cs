//using System;
//using System.Collections.Generic;
//using System.Text;

//namespace ZachJohnson.Promptu.SkinsApi
//{
//    internal class PerformOperationEventArgs : EventArgs
//    {
//        private SuggestionItemOperation operation;
//        private SuggestionItem item;

//        public PerformOperationEventArgs(SuggestionItemOperation operation, SuggestionItem item)
//        {
//            if (item == null)
//            {
//                throw new ArgumentNullException("item");
//            }

//            this.operation = operation;
//            this.item = item;
//        }

//        public SuggestionItemOperation Operation
//        {
//            get { return this.operation; }
//        }

//        public SuggestionItem Item
//        {
//            get { return this.item; }
//        }
//    }
//}
