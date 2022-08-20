using System;
using System.Collections.Generic;
using System.Text;

namespace ZachJohnson.Promptu.UserModel
{
    public class ValueListItem
    {
        private string value;
        private string translation;
        private bool allowTranslation;
        private int imageIndex;

        /// <summary>
        /// Initializes a new instance of the <see cref="ValueListItem"/> class.
        /// </summary>
        /// <param name="value">The ValueListItem's value.</param>
        public ValueListItem(string value)
            : this(value, -1)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ValueListItem"/> class.
        /// </summary>
        /// <param name="value">The ValueListItem's value.</param>
        /// <param name="imageIndex">Index of the ValueListItem's image.</param>
        public ValueListItem(string value, int imageIndex)
            : this(value, null, false, imageIndex)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ValueListItem"/> class.
        /// </summary>
        /// <param name="value">The ValueListItem's value.</param>
        /// <param name="translation">The ValueListItem's translation.</param>
        public ValueListItem(string value, string translation)
            : this(value, translation, -1)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ValueListItem"/> class.
        /// </summary>
        /// <param name="value">The ValueListItem's value.</param>
        /// <param name="translation">The ValueListItem's translation.</param>
        /// <param name="imageIndex">Index of the ValueListItem's image.</param>
        public ValueListItem(string value, string translation, int imageIndex)
            : this(value, translation, true, imageIndex)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ValueListItem"/> class.
        /// </summary>
        /// <param name="value">The ValueListItem's value.</param>
        /// <param name="translation">The ValueListItem's translation.</param>
        /// <param name="allowTranslation">if set to <c>true</c> the ValueListItem's translation will be used if the parent ValueList allows translation.</param>
        public ValueListItem(string value, string translation, bool allowTranslation)
            : this(value, translation, allowTranslation, -1)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ValueListItem"/> class.
        /// </summary>
        /// <param name="value">The ValueListItem's value.</param>
        /// <param name="translation">The ValueListItem's translation.</param>
        /// <param name="allowTranslation">if set to <c>true</c> the ValueListItem's translation will be used if the parent ValueList allows translation.</param>
        /// <param name="imageIndex">Index of the ValueListItem's image.</param>
        public ValueListItem(string value, string translation, bool allowTranslation, int imageIndex)
        {
            if (value == null)
            {
                throw new ArgumentNullException("value");
            }

            this.value = value;
            this.translation = translation;
            this.allowTranslation = allowTranslation;
            this.imageIndex = imageIndex;
        }

        /// <summary>
        /// Gets a value indicating whether translation is allowed for this ValueListItem.
        /// </summary>
        /// <value><c>true</c> translation is allowed; otherwise, <c>false</c>.</value>
        public bool AllowTranslation
        {
            get { return this.allowTranslation; }
        }

        /// <summary>
        /// Gets the index of the image in the parent ValueList's Images to use for this ValueListItem.
        /// </summary>
        /// <value>The index of the image.</value>
        public int ImageIndex
        {
            get { return this.imageIndex; }
        }

        /// <summary>
        /// Gets the ValueListItem's value.
        /// </summary>
        /// <value>The value.</value>
        public string Value
        {
            get { return this.value; }
        }

        /// <summary>
        /// Gets the translation for this ValueListItem's Value.
        /// </summary>
        /// <value>The translation.</value>
        public string Translation
        {
            get { return this.translation; }
        }

        /// <summary>
        /// Clones this instance.
        /// </summary>
        /// <returns></returns>
        public ValueListItem Clone()
        {
            return this.CloneCore();
        }

        internal static int CompareFromUI(ValueListItem x, ValueListItem y)
        {
            if (x == null)
            {
                if (y == null)
                {
                    return 0;
                }
                else
                {
                    return -1;
                }
            }
            else if (y == null)
            {
                return 1;
            }

            int result = x.Value.CompareTo(y.Value);
            if (result == 0)
            {
                if (x.Translation == null)
                {
                    if (y.translation != null)
                    {
                        result = 1;
                    }
                }
                else if (y.translation == null)
                {
                    result = -1;
                }
                else
                {
                    result = x.translation.CompareTo(y.translation);
                }
            }

            return result;
        }

        protected virtual ValueListItem CloneCore()
        {
            return new ValueListItem(this.value, this.translation, this.allowTranslation, this.imageIndex); 
        }
    }
}
