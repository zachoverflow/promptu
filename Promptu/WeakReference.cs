using System;
using System.Collections.Generic;
using System.Text;

namespace ZachJohnson.Promptu
{
    internal class WeakReference<T>
    {
        private WeakReference weakReference;

        public WeakReference(T target)
        {
            this.weakReference = new WeakReference(target);
        }

        public WeakReference(T target, bool trackRessurection)
        {
            this.weakReference = new WeakReference(target, trackRessurection);
        }

        public bool IsAlive
        {
            get { return this.weakReference.IsAlive; }
        }

        public T Target
        {
            get { return (T)this.weakReference.Target; }
        }

        public bool TrackRessurection
        {
            get { return this.weakReference.TrackResurrection; }
        }
    }
}
