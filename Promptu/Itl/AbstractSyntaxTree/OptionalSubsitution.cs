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
    using System;
    using System.Extensions;

    internal class OptionalSubsitution : ArgumentSubstitution
    {
        private Expression defaultValue;

        public OptionalSubsitution(int? argumentNumber, int? lastArgumentNumber, bool singularSubstitution, Expression defaultValue)
            : base(argumentNumber, lastArgumentNumber, singularSubstitution)
        {
            this.defaultValue = defaultValue;
        }

        public Expression DefaultValue
        {
            get { return this.defaultValue; }
        }

        public override string ConvertToString(ExecutionData data)
        {
            if (data.Arguments.Length == 0 || (this.ArgumentNumber != null && data.Arguments.Length < this.ArgumentNumber))
            {
                if (this.defaultValue == null)
                {
                    return String.Empty;
                }
                else
                {
                    return this.defaultValue.ConvertToString(data);
                }
            }

            if (this.SingularSubstitution)
            {
                if (this.ArgumentNumber != null)
                {
                    if (data.Arguments.Length < this.ArgumentNumber)
                    {
                        if (this.defaultValue == null)
                        {
                            return String.Empty;
                        }
                        else
                        {
                            return this.defaultValue.ConvertToString(data);
                        }
                    }
                    else
                    {
                        return data.Arguments[this.ArgumentNumber.Value - 1];
                    }
                }
                else
                {
                    return data.Arguments.ConcatenateAll(" ");
                }
            }
            else
            {
                if (this.LastArgumentNumber == null)
                {
                    return data.Arguments.ConcatenateAll(this.ArgumentNumber.Value - 1, " ");
                }
                else
                {
                    if (this.LastArgumentNumber >= data.Arguments.Length)
                    {
                        if (this.defaultValue == null)
                        {
                            return String.Empty;
                        }
                        else
                        {
                            return this.defaultValue.ConvertToString(data);
                        }
                    }

                    return data.Arguments.ConcatenateAllBetween(this.ArgumentNumber.Value - 1, this.LastArgumentNumber.Value - 1, " ");
                }
            }
        }
    }
}
