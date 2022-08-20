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
    using System;
    using System.IO;

    internal class OffsetStream : Stream
    {
        private Stream underlyingStream;
        private long offset;

        public OffsetStream(Stream stream, long offset)
        {
            if (stream == null)
            {
                throw new ArgumentNullException("stream");
            }

            this.underlyingStream = stream;
            this.offset = offset;
        }

        public override bool CanRead
        {
            get { return this.underlyingStream.CanRead; }
        }

        public override bool CanSeek
        {
            get { return this.underlyingStream.CanSeek; }
        }

        public override bool CanWrite
        {
            get { return this.underlyingStream.CanWrite; }
        }

        public override long Length
        {
            get { return this.underlyingStream.Length - this.offset; }
        }

        public override long Position
        {
            get
            {
                return this.underlyingStream.Position - this.offset;
            }

            set
            {
                this.underlyingStream.Position = value + this.offset;
            }
        }

        public override void Flush()
        {
            this.underlyingStream.Flush();
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            return this.underlyingStream.Read(buffer, offset, count);
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            long realOffset = offset;

            switch (origin)
            {
                case SeekOrigin.Begin:
                    realOffset -= this.offset;
                    break;
                default:
                    break;
            }

            return this.underlyingStream.Seek(realOffset, origin);
        }

        public override void SetLength(long value)
        {
            this.underlyingStream.SetLength(value + this.offset);
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            this.underlyingStream.Write(buffer, offset, count);
        }
    }
}
