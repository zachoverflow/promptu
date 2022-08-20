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

namespace ZachJohnson.Promptu.Itl.AbstractSyntaxTree
{
    internal abstract class ArgumentSubstitution : Expression
    {
        private int? argumentNumber;
        private int? lastArgumentNumber;
        private bool singularSubstitution;

        public ArgumentSubstitution(int? argumentNumber, int? lastArgumentNumber, bool singularSubstitution)
        {
            this.argumentNumber = argumentNumber;
            this.lastArgumentNumber = lastArgumentNumber;
            this.singularSubstitution = singularSubstitution;
        }

        public int? ArgumentNumber
        {
            get { return this.argumentNumber; }
        }

        public int? LastArgumentNumber
        {
            get { return this.lastArgumentNumber; }
        }

        public bool SingularSubstitution
        {
            get { return this.singularSubstitution; }
        }
    }
}
