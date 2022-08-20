using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;

namespace ZachJohnson.Promptu.UserModel
{
    internal class Id
    {
        private readonly int value;

        public Id(int value)
        {
            this.value = value;
        }

        public Id(string numberString)
        {
            try
            {
                this.value = Convert.ToInt32(numberString, CultureInfo.InvariantCulture);
            }
            catch (FormatException)
            {
                this.value = Convert.ToInt32(numberString, CultureInfo.CurrentCulture);
            }
            catch (OverflowException)
            {
                this.value = Convert.ToInt32(numberString, CultureInfo.CurrentCulture);
            }
        }

        //public Id(string s, int indexOfTwoChars)
        //{
        //    byte[] bytes = new byte[4];
        //    if (s.Length < indexOfTwoChars + 2)
        //    {
        //        throw new ArgumentOutOfRangeException("'index' does not refer to a position in the string which has two following characters.");
        //    }

        //    for (int i = indexOfTwoChars; i < 2; i++)
        //    {
        //        byte[] charBytes = BitConverter.GetBytes(s[i]);
        //        for (int j = 0; j < 2; j++)
        //        {
        //            bytes[(i * 2) + j] = charBytes[j];
        //        }
        //    }

        //    this.value = BitConverter.ToInt32(bytes, 0);
        //}

        public int Value
        {
            get { return this.value; }
        }

        //public string ToTwoChars()
        //{
        //    byte[] bytes = new byte[4];
        //    bytes = BitConverter.GetBytes(this.value);
        //    StringBuilder builder = new StringBuilder();

        //    for (int i = 0; i < 2; i++)
        //    {
        //        builder.Append(BitConverter.ToChar(bytes, i * 2));
        //    }

        //    return builder.ToString();
        //}

        public static bool operator ==(Id id1, Id id2)
        {
            if (object.ReferenceEquals(id1, id2))
            {
                return true;
            }

            if (((object)id1 == null) || ((object)id2 == null))
            {
                return false;
            }

            return id1.Value == id2.Value;
        }

        public static bool operator !=(Id id1, Id id2)
        {
            return !(id1 == id2);
        }

        public override int GetHashCode()
        {
            return this.value.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            Id id = obj as Id;

            if (id != null)
            {
                return this == id;
            }
            else
            {
                return false;
            }
        }

        public override string ToString()
        {
            return this.value.ToString(CultureInfo.InvariantCulture);
        }
    }
}
