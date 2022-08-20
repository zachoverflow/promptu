//-----------------------------------------------------------------------
// <copyright file="ObjectProperty.cs" company="ZachJohnson">
//     Copyright (c) Zach Johnson. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace ZachJohnson.Promptu.PluginModel
{
    using System;

    public abstract class ObjectProperty<T> : ObjectPropertyBase
    {
        public ObjectProperty(
            string id,
            string label,
            IPropertyEditorFactory editorFactory,
            object conversionInfo)
            : base(
                id,
                label,
                editorFactory,
                conversionInfo)
        {
        }

        public T Value
        {
            get 
            {
                return this.GetValueCore(); 
            }

            set
            {
                this.SetValueCore(value);
                this.NotifyValueChanged();
            }
        }

        protected abstract T GetValueCore();

        protected abstract void SetValueCore(T value);

        protected override object GetObjectValueCore()
        {
            return this.GetValueCore();
        }

        protected override void SetObjectValueCore(object value)
        {
            this.SetValueCore((T)value);
        }

        protected override Type GetValueTypeCore()
        {
            return typeof(T);
        }
    }
}
