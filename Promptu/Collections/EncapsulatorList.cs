//using System;
//using System.Collections.Generic;
//using System.Text;
//using System.Collections;

//namespace ZachJohnson.Promptu.Collections
//{
//    internal class EncapsulatorList<T> : IEnumerable<T>
//    {
//        private Predicate<T> predicate;
//        private IEnumerable<T> collection;

//        public EncapsulatorList(IEnumerable<T> collection, Predicate<T> predicate)
//        {
//            if (predicate == null)
//            {
//                throw new ArgumentNullException("predicate");
//            }
//            else if (collection == null)
//            {
//                throw new ArgumentNullException("collection");
//            }

//            this.collection = collection;
//            this.predicate = predicate;
//        }

//        public int Count
//        {
//            get
//            {
//                int count = 0;

//                foreach (T item in this.collection)
//                {
//                    if (predicate(item))
//                    {
//                        count++;
//                    }
//                }

//                return count;
//            }
//        }

//        public IEnumerator<T> GetEnumerator()
//        {
//            foreach (T item in this.collection)
//            {
//                if (predicate(item))
//                {
//                    yield return item;
//                }
//            }
//        }

//        IEnumerator IEnumerable.GetEnumerator()
//        {
//            return this.GetEnumerator();
//        }
//    }
//}
