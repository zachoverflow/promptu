//-----------------------------------------------------------------------
// <copyright file="FileFile.cs" company="ZachJohnson">
//     Copyright (c) Zach Johnson. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace ZachJohnson.Promptu.FileFileSystem
{
    using System;
    using System.Globalization;
    using System.IO;
    using System.IO.Extensions;
    using System.Text;
    using System.Text.RegularExpressions;

    internal class FileFile : IDisposable
    {
        private static Regex fileHeaderRegex = new Regex(
                @"^\s*\[\s*File\s+name\s*=\s*""(?<name>.*)""\s+bytes\s*=\s*""(?<bytesCount>.*)""\]\s*$",
                RegexOptions.IgnoreCase);

        private string name;
        private Stream contents;
        private bool disposed;

        public FileFile(string name, Stream contents)
        {
            if (name == null)
            {
                throw new ArgumentNullException("name");
            }
            else if (contents == null)
            {
                throw new ArgumentNullException("contents");
            }

            this.name = name;
            this.contents = contents;
        }

        ~FileFile()
        {
            this.Dispose(false);
        }

        public string Name
        {
            get { return this.name; }
        }

        public Stream Contents
        {
            get
            {
                this.ValidateNotDisposed();
                return this.contents;
            }
        }

        public static bool IsFileHeader(string text)
        {
            return fileHeaderRegex.IsMatch(text);
        }

        public static FileFile FromFile(FileSystemFile file)
        {
            return new FileFile(file.Name, new FileStream(file, FileMode.Open));
        }

        public static FileFile FromStream(CustomStreamReader headerStreamReader, Stream wholeStream)
        {
            string fileHeader = headerStreamReader.ReadLine();
            Match match = fileHeaderRegex.Match(fileHeader);
            if (match.Success && match.Groups.Count == 3)
            {
                int numberOfBytes;
                try
                {
                    try
                    {
                        numberOfBytes = Convert.ToInt32(match.Groups["bytesCount"].Value, CultureInfo.InvariantCulture);
                    }
                    catch (FormatException)
                    {
                        numberOfBytes = Convert.ToInt32(match.Groups["bytesCount"].Value, CultureInfo.CurrentCulture);
                    }
                    catch (OverflowException)
                    {
                        numberOfBytes = Convert.ToInt32(match.Groups["bytesCount"].Value, CultureInfo.CurrentCulture);
                    }
                }
                catch (FormatException)
                {
                    throw new InvalidFileHeaderException("The 'bytes' attribute does not contain a valid 32 bit integer.");
                }
                catch (OverflowException)
                {
                    throw new InvalidFileHeaderException("The 'bytes' attribute does not contain a valid 32 bit integer.");
                }

                MemoryStream stream = new MemoryStream();
                wholeStream.TransferTo(stream, numberOfBytes);
                stream.Position = 0;

                return new FileFile(match.Groups["name"].Value, stream);
            }
            else
            {
                throw new InvalidFileHeaderException("File header is invalid.");
            }
        }

        public void ToStream(Stream headerStream, Stream contentStream, Encoding encoding)
        {
            if (headerStream == null)
            {
                throw new ArgumentNullException("headerStream");
            }

            headerStream.Write(encoding.GetBytes(String.Format(
                CultureInfo.InvariantCulture,
                "[File name=\"{0}\" bytes=\"{1}\"]\n",
                this.name,
                this.Contents.Length)));

            this.Contents.Position = 0;
            this.Contents.TransferTo(contentStream);
        }

        public void SaveIn(FileSystemDirectory directory)
        {
            FileSystemFile file = directory + this.name;
            FileStream outStream = new FileStream(file, FileMode.Create);
            this.Contents.Position = 0;
            this.Contents.TransferTo(outStream);
            outStream.Close();
            outStream.Dispose();
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (this.contents != null)
            {
                this.contents.Dispose();
                this.contents = null;
            }

            this.disposed = true;
        }

        protected void ValidateNotDisposed()
        {
            if (this.disposed)
            {
                throw new ObjectDisposedException("FileFile");
            }
        }
    }
}
