//-----------------------------------------------------------------------
// <copyright file="CallbackObjectProperty.cs" company="ZachJohnson">
//     Copyright (c) Zach Johnson. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace ZachJohnson.Promptu.PluginModel
{
    using System;

    public class CallbackObjectProperty<T> : ObjectProperty<T>
    {
        private Getter<T> getter;
        private Setter<T> setter;

        public CallbackObjectProperty(
            string id,
            string label,
            Getter<T> getter,
            Setter<T> setter,
            IPropertyEditorFactory editorFactory,
            object conversionInfo)
            : base(
                id,
                label,
                editorFactory,
                conversionInfo)
        {
            if (getter == null)
            {
                throw new ArgumentNullException("getter");
            }
            else if (setter == null)
            {
                throw new ArgumentNullException("setter");
            }

            this.getter = getter;
            this.setter = setter;
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            this.getter = null;
            this.setter = null;
        }

        protected override T GetValueCore()
        {
            return this.getter();
        }

        protected override void SetValueCore(T value)
        {
            this.setter(value);
        }
    }
}
