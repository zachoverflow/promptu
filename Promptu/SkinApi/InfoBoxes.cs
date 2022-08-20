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

namespace ZachJohnson.Promptu.SkinApi
{
    using System.Collections.Generic;
    using ZachJohnson.Promptu.Skins;

    public sealed class InfoBoxes : IEnumerable<IInfoBox>
    {
        private InformationBoxManager manager;

        internal InfoBoxes(InformationBoxManager manager)
        {
            this.manager = manager;
        }

        public IInfoBox ItemInfoBox
        {
            get { return this.manager.ItemInfoBox; }
        }

        public IInfoBox ParameterHelpBox
        {
            get { return this.manager.ParameterHelpBox; }
        }

        public IEnumerator<IInfoBox> GetEnumerator()
        {
            return this.manager.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
