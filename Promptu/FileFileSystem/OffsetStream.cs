//-----------------------------------------------------------------------
// <copyright file="OffsetStream.cs" company="ZachJohnson">
//     Copyright (c) Zach Johnson. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

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
