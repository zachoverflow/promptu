using System;
using System.Collections.Generic;
using System.Text;
using ZachJohnson.Promptu.SkinApi;

namespace ZachJohnson.Promptu.UIModel
{
    internal class ObjectEventArgs<T> : EventArgs
    {
        private T obj;

        public ObjectEventArgs(T obj)
        {
            this.obj = obj;
        }

        public T Object
        {
            get { return this.obj; }
        }
    }
}