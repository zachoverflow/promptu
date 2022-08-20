//-----------------------------------------------------------------------
// <copyright file="BackedObjectProperty.cs" company="ZachJohnson">
//     Copyright (c) Zach Johnson. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace ZachJohnson.Promptu.PluginModel
{
    public class BackedObjectProperty<T> : ObjectProperty<T>
    {
        private T backingField;

        public BackedObjectProperty(
            string id,
            string label,
            T startingValue,
            IPropertyEditorFactory editorFactory,
            object conversionInfo)
            : base(
                id,
                label,
                editorFactory,
                conversionInfo)
        {
            this.backingField = startingValue;
        }

        protected override T GetValueCore()
        {
            return this.backingField;
        }

        protected override void SetValueCore(T value)
        {
            this.backingField = value;
        }
    }
}
