using System;
using System.Collections.Generic;
using System.Text;

namespace ZachJohnson.Promptu.UIModel
{
    public abstract class UIComponent<TThis, TNativeInterface>
        where TThis : UIComponent<TThis, TNativeInterface>
    {
        private string id;
        private object tag;
        private UIComponentCollection<TThis, TNativeInterface> parentCollection;

        public UIComponent(string id)
        {
            if (id == null)
            {
                throw new ArgumentNullException("id");
            }

            this.id = id;
        }

        public string Id
        {
            get { return this.id; }
        }

        //public bool IsInternal
        //{
        //    get { return this is Interfaces.ILockedInternal; }
        //}

        public object Tag
        {
            get
            {
                return this.tag;
            }

            set
            {
                //this.ValidateNotInternal();
                this.tag = value;
                //this.TagInternal = value;
            }
        }

        //internal object TagInternal
        //{
        //    get { return this.tag; }
        //    set { this.tag = value; }
        //}

        internal abstract TNativeInterface NativeInterface
        {
            get;
        }

        internal UIComponentCollection<TThis, TNativeInterface> ParentCollection
        {
            get { return this.parentCollection; }
            set { this.parentCollection = value; }
        }

        //protected internal void ValidateNotInternal()
        //{
        //    if (this.IsInternal)
        //    {
        //        throw new InvalidOperationException("Cannot modify this UIComponent because it is internal to Promptu and you are not allowed to modify it.");
        //    }
        //}
    }
}
