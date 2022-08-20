//-----------------------------------------------------------------------
// <copyright file="FileFileDirectory.cs" company="ZachJohnson">
//     Copyright (c) Zach Johnson. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace ZachJohnson.Promptu.FileFileSystem
{
    using System;
    using System.Globalization;
    using System.IO;
    using System.IO.Compression;
    using System.IO.Extensions;
    using System.Text;
    using System.Text.RegularExpressions;

    internal class FileFileDirectory : IDisposable
    {
        private static Regex directoryHeaderRegex = new Regex(
            @"^\s*\[\s*Directory\s+name\s*=\s*""(?<name>.*)""\s+itemCount\s*=\s*""(?<itemCount>.*)""\]\s*$", 
            RegexOptions.IgnoreCase);

        private static Regex endOfHeaderRegex = new Regex(
            @"^\s*\[\s*EndOfHeader\s*\]\s*$",
            RegexOptions.IgnoreCase);

        private string name;
        private FileFileCollection files = new FileFileCollection();
        private FileFileDirectoryCollection directories = new FileFileDirectoryCollection();
        private bool disposed;

        public FileFileDirectory(string name)
        {
            if (name == null)
            {
                throw new ArgumentNullException("name");
            }

            this.name = name;
        }

        ~FileFileDirectory()
        {
            this.Dispose(false);
        }

        public string Name
        {
            get 
            { 
                return this.name; 
            }

            set 
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }

                this.name = value; 
            }
        }

        public FileFileCollection Files
        {
            get 
            {
                this.ValidateNotDisposed();
                return this.files; 
            }
        }

        public FileFileDirectoryCollection Directories
        {
            get 
            {
                this.ValidateNotDisposed();
                return this.directories; 
            }
        }

        public static DateTime ReadContainerTimestamp(Stream stream)
        {
            stream.Position = 1;

            byte[] timestamp = new byte[8];
            int bytesRead = stream.Read(timestamp);

            if (bytesRead < 8)
            {
                throw new BadFileFormatException("The file is too short.");
            }

            return DateTime.FromBinary(BitConverter.ToInt64(timestamp, 0));
        }

        public static DateTime ReadContainerTimestamp(FileSystemFile file)
        {
            using (FileStream fileStream = new FileStream(file, FileMode.Open))
            {
                return ReadContainerTimestamp(fileStream);
            }
        }

        public static FileFileDirectory FromContainer(FileSystemFile file)
        {
            using (FileStream fileStream = new FileStream(file, FileMode.Open))
            {
                FileFileDirectory directory = FileFileDirectory.FromContainer(fileStream);
                fileStream.Close();
                return directory;
            }
        }

        public static FileFileDirectory FromContainer(Stream stream)
        {
            if (stream.Length <= 0)
            {
                throw new BadFileFormatException("File cannot have a length of zero bytes.");
            }

            using (MemoryStream memoryStream = new MemoryStream())
            {
                byte timestampLength = 9; // version byte + 8 bytes for timestamp (64 bit)
                stream.Position = timestampLength;
                using (OffsetStream offsetStream = new OffsetStream(stream, timestampLength))
                using (DeflateStream uncompressingStream = new DeflateStream(offsetStream, CompressionMode.Decompress, true))
                {
                    try
                    {
                        uncompressingStream.TransferTo(memoryStream);
                    }
                    catch (InvalidDataException ex)
                    {
                        throw new FileFileSystemException("Invalid file.", ex);
                    }
                }

                using (MemoryStream readMemoryStream = new MemoryStream(memoryStream.ToArray()))
                using (MemoryStream headerStream = new MemoryStream())
                {
                    StreamWriter writer = new StreamWriter(headerStream);
                    CustomStreamReader reader = new CustomStreamReader(readMemoryStream);
                    long headerLength = 0;
                    while (!reader.EndOfStream)
                    {
                        StringBuilder builder = new StringBuilder();

                        while (!reader.EndOfStream)
                        {
                            int read = reader.Read();
                            if (read == -1)
                            {
                                break;
                            }

                            char c = (char)read;

                            builder.Append(c);

                            headerLength += reader.CurrentEncoding.GetByteCount(new char[] { c });

                            if (c == '\n')
                            {
                                break;
                            }
                        }

                        string line = builder.ToString();

                        if (!endOfHeaderRegex.IsMatch(line))
                        {
                            writer.WriteLine(line);
                        }
                        else
                        {
                            break;
                        }
                    }

                    writer.Close();

                    using (MemoryStream readableHeaderStream = new MemoryStream(memoryStream.ToArray()))
                    {
                        if (readableHeaderStream.Length == 0)
                        {
                            throw new InvalidFileHeaderException("The header is invalid.");
                        }

                        CustomStreamReader headerReader = new CustomStreamReader(readableHeaderStream);

                        memoryStream.Position = headerLength;
                        headerReader.ReadLine(); // move position to start of next line (missing the header's header)

                        FileFileDirectory directory = FileFileDirectory.FromStream(headerReader, memoryStream);

                        headerReader.Close();
                        return directory;
                    }
                }
            }
        }

        public static bool IsDirectoryHeader(string text)
        {
            return directoryHeaderRegex.IsMatch(text);
        }

        public static FileFileDirectory FromStream(CustomStreamReader headerStreamReader, Stream wholeStream)
        {
            string directoryHeader = headerStreamReader.ReadLine();
            Match match = directoryHeaderRegex.Match(directoryHeader);
            if (match.Success && match.Groups.Count == 3)
            {
                int numberOfItems;
                try
                {
                    try
                    {
                        numberOfItems = Convert.ToInt32(match.Groups["itemCount"].Value, CultureInfo.InvariantCulture);
                    }
                    catch (FormatException)
                    {
                        numberOfItems = Convert.ToInt32(match.Groups["itemCount"].Value, CultureInfo.CurrentCulture);
                    }
                    catch (OverflowException)
                    {
                        numberOfItems = Convert.ToInt32(match.Groups["itemCount"].Value, CultureInfo.CurrentCulture);
                    }
                }
                catch (FormatException)
                {
                    throw new InvalidDirectoryHeaderException("The 'itemCount' attribute does not contain a valid 32 bit integer.");
                }
                catch (OverflowException)
                {
                    throw new InvalidDirectoryHeaderException("The 'itemCount' attribute does not contain a valid 32 bit integer.");
                }

                FileFileDirectory directory = new FileFileDirectory(match.Groups["name"].Value);

                for (int i = 0; i < numberOfItems; i++)
                {
                    string line = headerStreamReader.PeekLine();

                    if (IsDirectoryHeader(line))
                    {
                        directory.Directories.Add(FileFileDirectory.FromStream(headerStreamReader, wholeStream));
                    }
                    else if (FileFile.IsFileHeader(line))
                    {
                        directory.Files.Add(FileFile.FromStream(headerStreamReader, wholeStream));
                    }
                    else
                    {
                        throw new BadLineException("Line cannot be interpreted.");
                    }
                }

                return directory;
            }
            else
            {
                throw new InvalidDirectoryHeaderException("Directory header is invalid.");
            }
        }

        public static FileFileDirectory FromDirectory(FileSystemDirectory directory)
        {
            FileFileDirectory fileFileDirectory = new FileFileDirectory(directory.Name);
            foreach (FileSystemDirectory childDirectory in directory.GetDirectories())
            {
                fileFileDirectory.Directories.Add(FileFileDirectory.FromDirectory(childDirectory));
            }

            foreach (FileSystemFile file in directory.GetFiles())
            {
                fileFileDirectory.Files.Add(FileFile.FromFile(file));
            }

            return fileFileDirectory;
        }

        public void SaveInContainer(FileSystemFile file)
        {
            Stream stream = new FileStream(file, FileMode.Create);
            this.SaveInContainer(stream);
        }

        public DateTime SaveInContainer(Stream stream)
        {
            if (!stream.CanWrite)
            {
                throw new ArgumentException("Cannot write to stream.");
            }

            DeflateStream compressedStream = new DeflateStream(stream, CompressionMode.Compress, true);
            MemoryStream contentStream = new MemoryStream();
            StreamWriter writer = new StreamWriter(stream);

            stream.WriteByte((byte)FileFileSystemVersion.InitialVersion);
            DateTime fileTimestamp = System.DateTime.Now.ToUniversalTime();
            byte[] timestamp = BitConverter.GetBytes(fileTimestamp.ToBinary());
            stream.Write(timestamp); // header bytes = version|...timestamp...
            compressedStream.Write("[FileFileSystem version=\"0.1\"]\n", writer.Encoding);
            this.ToStream(compressedStream, contentStream, writer.Encoding);
            compressedStream.Write("[EndOfHeader]\n", writer.Encoding);
            compressedStream.Write(contentStream.ToArray());

            contentStream.Close();
            compressedStream.Close();
            compressedStream.Dispose();
            writer.Close();
            stream.Dispose();
            contentStream.Dispose();

            return fileTimestamp;
        }

        public void ToStream(Stream headerStream, Stream contentStream, Encoding encoding)
        {
            if (encoding == null)
            {
                throw new ArgumentNullException("encoding");
            }
            
            headerStream.Write(
                String.Format(
                    CultureInfo.InvariantCulture,
                    "[Directory name=\"{0}\" itemCount=\"{1}\"]\n",
                    this.name,
                    this.Files.Count + this.Directories.Count), 
                encoding);

            foreach (FileFileDirectory directory in this.Directories)
            {
                directory.ToStream(headerStream, contentStream, encoding);
            }

            foreach (FileFile file in this.Files)
            {
                file.ToStream(headerStream, contentStream, encoding);
            }
        }

        public void SaveIn(FileSystemDirectory directory)
        {
            FileSystemDirectory fileSystemDirectory = directory.CreateDirectory(this.name);
            foreach (FileFileDirectory child in this.Directories)
            {
                child.SaveIn(fileSystemDirectory);
            }

            foreach (FileFile file in this.Files)
            {
                file.SaveIn(fileSystemDirectory);
            }
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (this.directories != null)
            {
                foreach (FileFileDirectory directory in this.directories)
                {
                    if (directory != null)
                    {
                        directory.Dispose();
                    }
                }

                this.directories = null;
            }

            if (this.files != null)
            {
                foreach (FileFile file in this.files)
                {
                    if (file != null)
                    {
                        file.Dispose();
                    }
                }

                this.files = null;
            }

            this.disposed = true;
        }

        protected void ValidateNotDisposed()
        {
            if (this.disposed)
            {
                throw new ObjectDisposedException("FileFileDirectory");
            }
        }
    }
}
