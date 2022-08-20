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
