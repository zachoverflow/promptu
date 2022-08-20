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
