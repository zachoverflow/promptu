using System;
using System.Collections.Generic;
using System.Text;
using ZachJohnson.Promptu.UIModel.Interfaces;

namespace ZachJohnson.Promptu.UIModel
{
    public abstract class UIMenuItemBase : UIComponent<UIMenuItemBase, IGenericMenuItem>
    {
        //private string id;
        //private UIMenuItemCollection parentCollection;
        //private object tag;
        //private GenericMenuItemGetter itemGetter;
        private bool available = true;
        private bool overrideAsUnavailable;

        internal UIMenuItemBase(string id)
            : base(id)
        {
            //if (id == null)
            //{
            //    throw new ArgumentNullException("id");
            //}
            //else if (itemGetter == null)
            //{
            //    throw new ArgumentNullException("itemGetter");
            //}

            //this.id = id;
            //this.itemGetter = itemGetter;
        }

        public event EventHandler AvailableChanged;

        //public bool Available
        //{
        //    get
        //    {
        //        return this.available;
        //    }

        //    set
        //    {
        //        //this.ValidateNotInternal();
        //        this.AvailableInternal = value;
        //    }
        //}

        public bool Available
        {
            get
            {
                return this.available;
            }

            set
            {
                this.available = value;
                this.UpdateAvailable();
            }
        }

        internal bool OverrideAsUnavailable
        {
            get
            {
                return this.overrideAsUnavailable;
            }

            set
            {
                if (this.overrideAsUnavailable != value)
                {
                    this.overrideAsUnavailable = value;
                    this.UpdateAvailable();
                }
            }
        }

        private void UpdateAvailable()
        {
            this.SetAvailable(this.overrideAsUnavailable ? false : this.available);
            this.OnAvailableChanged(EventArgs.Empty);
        }

        private void OnAvailableChanged(EventArgs e)
        {
            EventHandler handler = this.AvailableChanged;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        protected abstract void SetAvailable(bool value);

        //public string Id
        //{
        //    get { return this.id; }
        //}

        //public bool IsInternal
        //{
        //    get { return this is Interfaces.ILockedInternal; }
        //}

        //public object Tag
        //{
        //    get
        //    {
        //        return this.tag;
        //    }

        //    set
        //    {
        //        this.ValidateNotInternal();
        //        this.SetTagInternal(value);
        //    }
        //}

        //internal abstract IGenericMenuItem GenericMenuItem
        //{
        //    get;
        //}

        //internal UIMenuItemCollection ParentCollection
        //{
        //    get { return this.parentCollection; }
        //    set { this.parentCollection = value; }
        //}

        //internal void SetTagInternal(object value)
        //{
        //    this.tag = value;
        //}

        //protected internal void ValidateNotInternal()
        //{
        //    if (this.IsInternal)
        //    {
        //        throw new InvalidOperationException("Cannot modify this UIMenuItem because it is internal to Promptu and you are not allowed to modify it.");
        //    }
        //}
    }
}
