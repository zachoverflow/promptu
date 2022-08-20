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
    using ZachJohnson.Promptu.UIModel;

    public class OptionsGroup : BindingCollection<object>
    {
        private string id;
        private string label;

        public OptionsGroup(string id, string label)
        {
            if (id == null)
            {
                throw new ArgumentNullException("id");
            }
            else if (label == null)
            {
                throw new ArgumentNullException("label");
            }

            this.id = id;
            this.label = label;
        }

        public string Id
        {
            get { return this.id; }
        }

        public string Label
        {
            get
            {
                return this.label;
            }

            set
            {
                this.label = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs("Label"));
            }
        }
    }
}
