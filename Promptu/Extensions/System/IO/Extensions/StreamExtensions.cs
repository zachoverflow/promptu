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

namespace System.IO.Extensions
{
    using System;
    using System.IO;
    using System.Text;
    using ZachJohnson.Promptu.UI;
    using ZachJohnson.Promptu.UIModel.Presenters;

    internal static class StreamExtensions
    {
        private delegate void TransferToVisibleMethod(
            Stream stream, 
            Stream streamTo, 
            string statusFormat, 
            DownloadProgressDialogPresenter statusDialog);

        public static void Write(this Stream stream, string value, Encoding encoding)
        {
            if (stream == null)
            {
                throw new ArgumentNullException("stream");
            }
            else if (value == null)
            {
                throw new ArgumentNullException("value");
            }
            else if (encoding == null)
            {
                throw new ArgumentNullException("encoding");
            }

            byte[] buffer = encoding.GetBytes(value);
            stream.Write(buffer, 0, buffer.Length);
        }

        public static void Write(this Stream stream, byte[] bytes)
        {
            if (stream == null)
            {
                throw new ArgumentNullException("stream");
            }

            stream.Write(bytes, 0, bytes.Length);
        }

        public static MemoryStream ToMemoryStream(this Stream stream)
        {
            if (stream == null)
            {
                throw new ArgumentNullException("stream");
            }

            if (stream.CanSeek)
            {
                stream.Position = 0;
            }

            MemoryStream memoryStream = new MemoryStream();
            stream.TransferTo(memoryStream);
            return memoryStream;
        }

        public static bool IsExactCopyOf(this Stream thisStream, Stream otherStream)
        {
            if (thisStream.Length != otherStream.Length)
            {
                return false;
            }

            if (thisStream.CanSeek)
            {
                thisStream.Position = 0;
            }

            if (otherStream.CanSeek)
            {
                otherStream.Position = 0;
            }

            byte[] thisBuffer = new byte[4096];
            byte[] otherBuffer = new byte[4096];

            int thisBytesRead = 0;
            int otherBytesRead = 0;

            do
            {
                thisBytesRead = thisStream.Read(thisBuffer);
                otherBytesRead = otherStream.Read(otherBuffer);

                if (thisBytesRead != otherBytesRead)
                {
                    return false;
                }

                for (int i = 0; i < thisBytesRead; i++)
                {
                    if (thisBuffer[i] != otherBuffer[i])
                    {
                        return false;
                    }
                }
            }
            while (thisBytesRead > 0 && thisBytesRead > 0);

            return true;
        }

        public static byte[] ReadAllBytes(this Stream stream)
        {
            if (stream == null)
            {
                throw new ArgumentNullException("stream");
            }

            using (MemoryStream memoryStream = new MemoryStream())
            {
                byte[] buffer = new byte[4096];

                int bytesRead = 0;

                do
                {
                    stream.Read(buffer, 0, buffer.Length);

                    if (bytesRead > 0)
                    {
                        memoryStream.Write(buffer, 0, bytesRead);
                    }
                }
                while (bytesRead > 0);

                return memoryStream.ToArray();
            }
        }

        public static int Read(this Stream stream, byte[] buffer)
        {
            if (stream == null)
            {
                throw new ArgumentNullException("stream");
            }

            return stream.Read(buffer, 0, buffer.Length);
        }

        public static void TransferTo(this Stream stream, Stream transferTo, int byteCount)
        {
            if (stream == null)
            {
                throw new ArgumentNullException("stream");
            }
            else if (transferTo == null)
            {
                throw new ArgumentNullException("transferTo");
            }

            byte[] buffer = new byte[4096];

            int bytesRead = 0;
            int bytesLeft = byteCount;

            do
            {
                int numberOfBytesToRead;
                if (bytesLeft > buffer.Length)
                {
                    numberOfBytesToRead = buffer.Length;
                }
                else
                {
                    numberOfBytesToRead = bytesLeft;
                }

                bytesRead = stream.Read(buffer, 0, numberOfBytesToRead);

                if (bytesRead > 0)
                {
                    bytesLeft -= bytesRead;
                    transferTo.Write(buffer, 0, bytesRead);
                }
            }
            while (bytesRead > 0 && bytesLeft > 0);
        }

        public static void TransferTo(this Stream stream, Stream transferTo)
        {
            if (stream == null)
            {
                throw new ArgumentNullException("stream");
            }
            else if (transferTo == null)
            {
                throw new ArgumentNullException("transferTo");
            }

            byte[] buffer = new byte[4096];

            int bytesRead = 0;

            do
            {
                bytesRead = stream.Read(buffer, 0, buffer.Length);

                if (bytesRead > 0)
                {
                    transferTo.Write(buffer, 0, bytesRead);
                }
            }
            while (bytesRead > 0);
        }
    }
}
