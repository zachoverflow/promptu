using System;
using System.Collections.Generic;
using System.Text;

namespace ZachJohnson.Promptu.UIModel.Presenters
{
    internal abstract class PresenterBase<T> : IDisposable
    {
        private readonly T nativeInterface;

        public PresenterBase(T nativeInterface)
        {
            if (nativeInterface == null)
            {
                throw new ArgumentNullException("nativeInterface");
            }

            this.nativeInterface = nativeInterface;
        }

        public T NativeInterface
        {
            get { return this.nativeInterface; }
        }

        public void Dispose()
        {
            this.Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            IDisposable disposableNativeInterface = this.NativeInterface as IDisposable;

            if (disposableNativeInterface != null)
            {
                disposableNativeInterface.Dispose();
            }
        }
    }
}
