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

namespace ZachJohnson.Promptu.PTK
{
    using System;

    internal abstract class Widget
    {
        private string id;
        private IWidgetHost currentHost;

        public Widget(string id)
        {
            if (id == null)
            {
                throw new ArgumentNullException("id");
            }

            this.id = id;
        }

        public string Id
        {
            get { return this.id; }
        }

        internal abstract object NativeObject { get; }

        internal IWidgetHost CurrentHost
        {
            get { return this.currentHost; }
            set { this.currentHost = value; }
        }

        internal void UnhostIfNecessary()
        {
            if (this.currentHost != null)
            {
                this.currentHost.Remove(this);
            }
        }
    }
}
