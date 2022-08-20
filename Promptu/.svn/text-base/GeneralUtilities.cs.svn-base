using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;
using System.Collections.Generic.Extensions;

namespace ZachJohnson.Promptu
{
    internal static class GeneralUtilities
    {
        public static string GetAvailableIncrementingName(
            IEnumerable<string> collection,
            string basicFormat,
            string insertFormat,
            bool caseSensitive,
            InsertBase insertBase)
        {
            NameGetter<string> nameGetter = new NameGetter<string>(
                delegate(string objectToOperateOn)
                {
                    return objectToOperateOn;
                });

            return GetAvailableIncrementingName<string>(collection, nameGetter, basicFormat, insertFormat, caseSensitive, insertBase);
        }

        public static string GetAvailableIncrementingName<TObject>(
            IEnumerable<TObject> collection,
            string basicFormat,
            string insertFormat,
            bool caseSensitive,
            InsertBase insertBase) where TObject : INamed
        {
            NameGetter<TObject> nameGetter = new NameGetter<TObject>(
               delegate(TObject objectToOperateOn)
               {
                   return objectToOperateOn.Name;
               });

            return GetAvailableIncrementingName<TObject>(collection, nameGetter, basicFormat, insertFormat, caseSensitive, insertBase);
        }

        public static string GetAvailableIncrementingName<TObject>(
            IEnumerable<TObject> collection,
            NameGetter<TObject> nameGetter,
            string basicFormat,
            string insertFormat,
            bool caseSensitive,
            InsertBase insertBase)
        {
            if (basicFormat == null)
            {
                throw new ArgumentNullException("basicFormat");
            }
            else if (collection == null)
            {
                throw new ArgumentNullException("collection");
            }
            else if (nameGetter == null)
            {
                throw new ArgumentNullException("nameGetter");
            }
            else if (insertFormat == null)
            {
                throw new ArgumentNullException("insertFormat");
            }
            else if (!insertFormat.Contains("{number}"))
            {
                throw new ArgumentException("The insert format must contain \"{number}\".");
            }
            else if (!basicFormat.Contains("{+}"))
            {
                throw new ArgumentException("The basic format must contain \"{+}\".");
            }

            string substitutedBasicFormat = basicFormat.Replace("{+}", "{0}");
            string alternateNameFormat = String.Format(CultureInfo.CurrentCulture, substitutedBasicFormat, insertFormat.Replace("{number}", "{0}"));

            int collectionCount = 1;

            TObject[] array = collection as TObject[];
            if (array != null)
            {
                collectionCount = array.Length;
            }
            else
            {
                IList<TObject> list = collection as IList<TObject>;
                if (list != null)
                {
                    collectionCount = list.Count;
                }
                else
                {
                    foreach (TObject item in collection)
                    {
                        collectionCount++;
                    }
                }
            }

            int i = 0;
            do
            {
                string name;
                if (i == 0)
                {
                    name = basicFormat.Replace("{+}", String.Empty);
                }
                else
                {
                    int number = i;
                    if (insertBase == InsertBase.Zero)
                    {
                        number--;
                    }
                    else if (insertBase != InsertBase.One)
                    {
                        number++;
                    }

                    name = String.Format(CultureInfo.CurrentCulture, alternateNameFormat, (number).ToString(CultureInfo.InvariantCulture));
                }

                if (name.Length > 0 && !collection.Contains<TObject>(nameGetter, name, caseSensitive))
                {
                    return name;
                }

                i++;
            }
            while (i <= collectionCount + 1);

            throw new IncrementingValueCannotBeFoundException(
                "The number of attempts to locate an incrementing name have exceeded the number of items in the collection supplied.  "
            + "Make sure that you have supplied valid formats and that the name getter returns valid names.");
        }

        public static string GetAvailableIncrementingNameOptimized(
            Collections.ITrie collection,
            string basicFormat,
            string insertFormat,
            bool caseSensitive,
            InsertBase insertBase)
        {
            if (basicFormat == null)
            {
                throw new ArgumentNullException("basicFormat");
            }
            else if (collection == null)
            {
                throw new ArgumentNullException("collection");
            }
            else if (insertFormat == null)
            {
                throw new ArgumentNullException("insertFormat");
            }
            else if (!insertFormat.Contains("{number}"))
            {
                throw new ArgumentException("The insert format must contain \"{number}\".");
            }
            else if (!basicFormat.Contains("{+}"))
            {
                throw new ArgumentException("The basic format must contain \"{+}\".");
            }

            string substitutedBasicFormat = basicFormat.Replace("{+}", "{0}");
            string alternateNameFormat = String.Format(CultureInfo.CurrentCulture, substitutedBasicFormat, insertFormat.Replace("{number}", "{0}"));    

            int i = 0;
            do
            {
                string name;
                if (i == 0)
                {
                    name = basicFormat.Replace("{+}", String.Empty);
                }
                else
                {
                    int number = i;
                    if (insertBase == InsertBase.Zero)
                    {
                        number--;
                    }
                    else if (insertBase != InsertBase.One)
                    {
                        number++;
                    }

                    name = String.Format(CultureInfo.CurrentCulture, alternateNameFormat, (number).ToString(CultureInfo.InvariantCulture));
                }

                if (name.Length > 0 && !collection.Contains(name, ZachJohnson.Promptu.Collections.CaseSensitivity.Insensitive))
                {
                    return name;
                }

                i++;
            }
            while (i <= collection.Count);

            throw new IncrementingValueCannotBeFoundException(
                "The number of attempts to locate an incrementing name have exceeded the number of items in the collection supplied.  "
            + "Make sure that you have supplied valid formats and that the name getter returns valid names.");
        }     
    }
}
