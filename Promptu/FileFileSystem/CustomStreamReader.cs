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

namespace ZachJohnson.Promptu.FileFileSystem
{
    using System.IO;

    internal class CustomStreamReader : StreamReader
    {
        private string peekedLine;

        public CustomStreamReader(Stream stream)
            : base(stream)
        {
        }

        public string PeekLine()
        {
            this.peekedLine = this.ReadLine();
            return this.peekedLine;
        }

        public override string ReadLine()
        {
            if (this.peekedLine != null)
            {
                string line = this.peekedLine;
                this.peekedLine = null;
                return line;
            }

            return base.ReadLine();
        }
    }
}
