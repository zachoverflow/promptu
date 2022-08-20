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
    using System.Extensions;

    internal class ImperativeSubstitution : ArgumentSubstitution
    {
        public ImperativeSubstitution(int? argumentNumber, int? lastArgumentNumber, bool singularSubstitution)
            : base(argumentNumber, lastArgumentNumber, singularSubstitution)
        {
        }

        public override string ConvertToString(ExecutionData data)
        {
            if (data.Arguments.Length == 0)
            {
                throw new NotEnoughArgumentsConversionException("Not enough arguments were supplied.");
            }

            if (this.SingularSubstitution)
            {
                if (this.ArgumentNumber != null)
                {
                    if (data.Arguments.Length < this.ArgumentNumber)
                    {
                        throw new NotEnoughArgumentsConversionException("Not enough arguments were supplied.");
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
                        throw new NotEnoughArgumentsConversionException("Not enough arguments were supplied.");
                    }

                    return data.Arguments.ConcatenateAllBetween(this.ArgumentNumber.Value - 1, this.LastArgumentNumber.Value - 1, " ");
                }
            }
        }
    }
}
