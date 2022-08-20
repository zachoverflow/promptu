//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//namespace ZachJohnson.Promptu.WpfUI.DefaultSkin
//{
//    internal class WpfConvertedImages : IList<object>
//    {
//        private IList<object> converted = new List<object>();

//        public WpfConvertedImages()
//        {
//        }

//        public IList<object> Converted
//        {
//            get { return this.converted; }
//        }

//        public int IndexOf(object item)
//        {
//            return converted.IndexOf(item);
//        }

//        public void Insert(int index, object item)
//        {
//            this.converted.Insert(this.Convert(item), item);
//        }

//        private object Convert(object from)
//        {
//            return from;
//        }

//        public void RemoveAt(int index)
//        {
//            this.converted.RemoveAt(index);
//        }

//        public object this[int index]
//        {
//            get
//            {
//                return 
//            }
//            set
//            {
//                throw new NotImplementedException();
//            }
//        }

//        public void Add(object item)
//        {
//            throw new NotImplementedException();
//        }

//        public void Clear()
//        {
//            throw new NotImplementedException();
//        }

//        public bool Contains(object item)
//        {
//            throw new NotImplementedException();
//        }

//        public void CopyTo(object[] array, int arrayIndex)
//        {
//            throw new NotImplementedException();
//        }

//        public int Count
//        {
//            get { throw new NotImplementedException(); }
//        }

//        public bool IsReadOnly
//        {
//            get { throw new NotImplementedException(); }
//        }

//        public bool Remove(object item)
//        {
//            throw new NotImplementedException();
//        }

//        public IEnumerator<object> GetEnumerator()
//        {
//            throw new NotImplementedException();
//        }

//        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
//        {
//            throw new NotImplementedException();
//        }
//    }
//}
