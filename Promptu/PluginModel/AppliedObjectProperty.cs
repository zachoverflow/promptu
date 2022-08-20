//-----------------------------------------------------------------------
// <copyright file="AppliedObjectProperty.cs" company="ZachJohnson">
//     Copyright (c) Zach Johnson. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace ZachJohnson.Promptu.PluginModel
{
    using System;
    using System.ComponentModel;
    using ZachJohnson.Promptu.PluginModel.Internals;

    public class AppliedObjectProperty<TValue> : ObjectProperty<TValue>, IAppliable
    {
        private object backingField;
        private BindingExpression binding;
        private TypeConverter typeConverter;

        public AppliedObjectProperty(
            string id,
            string label,
            object startingValue,
            string bindingPath,
            IPropertyEditorFactory editorFactory,
            object conversionInfo,
            TypeConverter typeConverter)
            : base(
                id,
                label,
                editorFactory,
                conversionInfo)
        {
            if (bindingPath == null)
            {
                throw new ArgumentNullException("bindingPath");
            }
            else if (bindingPath.Length < 0)
            {
                throw new ArgumentException("'bindingPath' cannot be empty.");
            }

            this.binding = new BindingExpression(bindingPath);
            this.typeConverter = typeConverter;

            this.backingField = startingValue;
        }

        public void ApplyTo(object obj)
        {
            this.binding.SetValue(obj, this.backingField);
        }

        protected override TValue GetValueCore()
        {
            object value = this.backingField;

            try
            {
                if (this.typeConverter != null)
                {
                    return (TValue)this.typeConverter.ConvertTo(value, typeof(TValue));
                }
            }
            catch (ArgumentException)
            {
            }
            catch (NotSupportedException)
            {
            }
            catch (InvalidCastException)
            {
            }

            return (TValue)value;
        }

        protected override void SetValueCore(TValue value)
        {
            object setValue = value;

            try
            {
                if (this.typeConverter != null)
                {
                    setValue = this.typeConverter.ConvertFrom(value);
                }
            }
            catch (ArgumentException)
            {
            }
            catch (NotSupportedException)
            {
            }
            catch (InvalidCastException)
            {
            }

            this.backingField = setValue;
        }
    }
}
