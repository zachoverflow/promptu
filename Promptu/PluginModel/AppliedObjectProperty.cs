// Copyright 2022 Zach Johnson
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

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
