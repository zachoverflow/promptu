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

namespace System.Drawing.Extensions
{
    using System;

    internal static class PointExtensions
    {
        public static Point Invert(this Point pt)
        {
            return new Point(-pt.X, -pt.Y);
        }

        public static int DistanceTo(this Point thisPt, Point pt)
        {
            return (int)Math.Sqrt(Math.Pow((thisPt.X - pt.X), 2) + Math.Pow((thisPt.Y - pt.Y), 2));
        }
    }
}
