﻿//-----------------------------------------------------------------------
// <copyright file="WidgetCollectionWidget.cs" company="ZachJohnson">
//     Copyright (c) Zach Johnson. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace ZachJohnson.Promptu.PTK
{
    using System;
    using System.Collections.Generic;
    using ZachJohnson.Promptu.UIModel.Interfaces;

    internal abstract class WidgetCollectionWidget<TNativeInterface>
        : GenericWidget<TNativeInterface>, IEnumerable<Widget>, IWidgetHost
        where TNativeInterface : ICollectionWidget
    {
        private List<Widget> hostedWidgets = new List<Widget>();

        public WidgetCollectionWidget(string id, TNativeInterface nativeInterface) 
            : base(id, nativeInterface)
        {
        }

        public event EventHandler<WidgetEventArgs> WidgetAdded;

        public event EventHandler<WidgetEventArgs> WidgetRemoved;

        public int WidgetCount
        {
            get { return this.hostedWidgets.Count; }
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public void Add(Widget widget)
        {
            if (widget == null)
            {
                throw new ArgumentNullException("widget");
            }

            this.Insert(this.WidgetCount, widget);
        }

        public void Insert(int index, Widget widget)
        {
            if (widget == null)
            {
                throw new ArgumentNullException("widget");
            }
            else if (index < 0 || index > this.WidgetCount)
            {
                throw new ArgumentOutOfRangeException("index");
            }

            widget.UnhostIfNecessary();
            this.NativeInterface.Insert(index, widget.NativeObject);
            this.hostedWidgets.Insert(index, widget);
            widget.CurrentHost = this;
            this.OnWidgetAdded(new WidgetEventArgs(widget));
        }

        public void Remove(Widget widget)
        {
            if (widget == null)
            {
                throw new ArgumentNullException("widget");
            }

            if (widget.CurrentHost == this)
            {
                this.hostedWidgets.Remove(widget);
                this.NativeInterface.Remove(widget.NativeObject);
                widget.CurrentHost = null;
                this.OnWidgetRemoved(new WidgetEventArgs(widget));
            }
        }

        public IEnumerator<Widget> GetEnumerator()
        {
            return this.hostedWidgets.GetEnumerator();
        }

        protected virtual void OnWidgetAdded(WidgetEventArgs e)
        {
            EventHandler<WidgetEventArgs> handler = this.WidgetAdded;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        protected virtual void OnWidgetRemoved(WidgetEventArgs e)
        {
            EventHandler<WidgetEventArgs> handler = this.WidgetRemoved;
            if (handler != null)
            {
                handler(this, e);
            }
        }
    }
}