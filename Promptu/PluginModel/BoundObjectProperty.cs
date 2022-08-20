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

    public class BoundObjectProperty<T> : ObjectProperty<T>
    {
        private BindingExpression binding;
        private BindingContextManager bindingContextManager;
        private TypeConverter typeConverter;

        public BoundObjectProperty(
            string id,
            string label,
            string path,
            object dataContext)
            : this(
                id,
                label,  
                path, 
                dataContext, 
                null,
                null,
                null)
        {
        }

        public BoundObjectProperty(
            string id,
            string label, 
            string path, 
            object dataContext,
            IPropertyEditorFactory editorFactory,
            object conversionInfo,
            TypeConverter typeConverter)
            : base(id, label, editorFactory, conversionInfo)
        {
            if (path == null)
            {
                throw new ArgumentNullException("path");
            }
            else if (path.Length <= 0)
            {
                throw new ArgumentException("'path' cannot be empty.");
            }

            this.binding = new BindingExpression(path);
            this.bindingContextManager = new BindingContextManager(this.binding);
            this.bindingContextManager.PropertyChanged += this.HandleDataContextPropertyChanged;
            this.DataContext = dataContext;
            this.typeConverter = typeConverter;
        }

        public object DataContext
        {
            get 
            { 
                return this.bindingContextManager.BaseContext; 
            }

            set
            {
                this.bindingContextManager.BaseContext = value;
                this.NotifyValueChanged();
            }
        }

        protected override T GetValueCore()
        {
            object value = this.binding.GetValue(this.bindingContextManager.BaseContext);

            try
            {
                if (this.typeConverter != null)
                {
                    return (T)this.typeConverter.ConvertTo(value, typeof(T));
                }

                return (T)value;
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

            return default(T);
        }

        protected override void SetValueCore(T value)
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

            this.binding.SetValue(this.bindingContextManager.BaseContext, setValue);
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            this.bindingContextManager.Dispose();
        }

        private void HandleDataContextPropertyChanged(object sender, EventArgs e)
        {
            this.NotifyValueChanged();
        }
    }
}
