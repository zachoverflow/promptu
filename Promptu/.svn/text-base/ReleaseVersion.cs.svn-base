using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;

namespace ZachJohnson.Promptu
{
    internal class ReleaseVersion
    {
        private int major;
        private int minor;
        private int build;
        private int revision;

        public ReleaseVersion(int major, int minor, int build, int revision)
        {
            this.major = major;
            this.minor = minor;
            this.build = build;
            this.revision = revision;
        }

        public ReleaseVersion(Version version)
        {
            this.major = version.Major;
            this.minor = version.Minor;
            this.build = version.Build;
            this.revision = version.Revision;
        }

        public ReleaseVersion(string version)
        {
            string[] numbers = version.Trim().Split(".".ToCharArray());
            if (numbers.Length != 4)
            {
                throw new FormatException("The version is not in a valid format.");
            }

            try
            {
                this.major = Convert.ToInt32(numbers[0], CultureInfo.InvariantCulture);
                this.minor = Convert.ToInt32(numbers[1], CultureInfo.InvariantCulture);
                this.build = Convert.ToInt32(numbers[2], CultureInfo.InvariantCulture);
                this.revision = Convert.ToInt32(numbers[3], CultureInfo.InvariantCulture);
            }
            catch (OverflowException ex)
            {
                throw new FormatException(ex.Message);
            }
        }

        public int Major
        {
            get { return this.major; }
        }

        public int Minor
        {
            get { return this.minor; }
        }

        public int Build
        {
            get { return this.build; }
        }

        public int Revision
        {
            get { return this.revision; }
        }

        public bool IsAfter(ReleaseVersion otherVersion)
        {
            if (otherVersion.major < this.major)
            {
                return true;
            }
            else if (otherVersion.major == this.major)
            {
                if (otherVersion.minor < this.minor)
                {
                    return true;
                }
                else if (otherVersion.minor == this.minor)
                {
                    if (otherVersion.build < this.build)
                    {
                        return true;
                    }
                    else if (otherVersion.build == this.build)
                    {
                        if (otherVersion.revision < this.revision)
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        public Version ToVersion()
        {
            return new Version(this.major, this.minor, this.build, this.revision);
        }

        public override string ToString()
        {
            return String.Format(CultureInfo.InvariantCulture, "{0}.{1}.{2}.{3}", this.major, this.minor, this.build, this.revision);
        }
    }
}
