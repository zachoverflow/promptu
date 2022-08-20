using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using ZachJohnson.Promptu.Collections;
using System.Collections;
using System.Drawing;
using ZachJohnson.Promptu.UIModel;
using System.ComponentModel;
using System.Globalization;

namespace ZachJohnson.Promptu.UserModel
{
    /// <summary>
    /// Represents a list of values and optional translation values to providing as suggestions for Promptu function and command parameters.
    /// </summary>
    public class ValueList : IEnumerable<ValueListItem>, IDiffable, IHasCount, INotifyPropertyChanged
    {
        private string name;
        private Id id;
        private TrieDictionary<ValueListItem> collection = new TrieDictionary<ValueListItem>(SortMode.Alphabetical, CaseSensitivity.Sensitive, true);
        private bool useNamespaceInterpretation;
        private bool useItemTranslations;
        private List<Image> images = new List<Image>();

        /// <summary>
        /// Initializes a new instance of the <see cref="ValueList"/> class.
        /// </summary>
        public ValueList()
            : this(false, false)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ValueList"/> class.
        /// </summary>
        /// <param name="useNamespaceInterpretation">If set to <c>true</c>, the ValueList uses namespace interpretation.</param>
        /// <param name="useItemTranslations">If set to <c>true</c>, the ValueList uses item translations.</param>
        public ValueList(bool useNamespaceInterpretation, bool useItemTranslations)
            : this(null, String.Empty, useNamespaceInterpretation, useItemTranslations)
        {
        }

        internal ValueList(Id id, string name, bool useNamespaceInterpretation, bool useItemTranslations)
        {
            if (name == null)
            {
                throw new ArgumentNullException("name");
            }

            this.name = name;
            this.id = id;
            this.useNamespaceInterpretation = useNamespaceInterpretation;
            this.useItemTranslations = useItemTranslations;
        }

        Id IHasId.Id
        {
            get { return this.id; }
            set { this.id = value; }
        }

        internal string Name
        {
            get { return this.name; }
            set { this.name = value; }
        }

        internal ItemInfo GetItemInfo()
        {
            List<string> attributes = new List<string>();

            attributes.Add(String.Format(
                CultureInfo.CurrentCulture, 
                Localization.UIResources.ValueListDetailsUseNamespaceInterpretationFormat,
                this.UseNamespaceInterpretation));

            attributes.Add(String.Format(
                CultureInfo.CurrentCulture, 
                Localization.UIResources.ValueListDetailsUseItemTranslationsFormat,
                this.UseItemTranslations));

            if (this.Count == 1)
            {
                attributes.Add(String.Format(
                    CultureInfo.CurrentCulture, 
                    Localization.UIResources.ValueListItemsCountSingular, 
                    this.Count));
            }
            else
            {
                attributes.Add(String.Format(
                    CultureInfo.CurrentCulture, 
                    Localization.UIResources.ValueListItemsCountPlural,
                    this.Count));
            }


            return new ItemInfo(this.Name, attributes);
        }

        /// <summary>
        /// Gets the list of images to use when displaying the ValueList's items.
        /// </summary>
        /// <value>The images.</value>
        public List<Image> Images
        {
            get { return this.images; }
        }

        /// <summary>
        /// Gets a value indicating whether to use namespace interpretation.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if to uses namespace interpretation; otherwise, <c>false</c>.
        /// </value>
        public bool UseNamespaceInterpretation
        {
            get { return this.useNamespaceInterpretation; }
            internal set { this.useNamespaceInterpretation = value; }
        }

        /// <summary>
        /// Gets a value indicating whether to use item translations.
        /// </summary>
        /// <value><c>true</c> if to uses item translations; otherwise, <c>false</c>.</value>
        public bool UseItemTranslations
        {
            get { return this.useItemTranslations; }
            internal set { this.useItemTranslations = value; }
        }

        string IHasNonIdIdentifier.GetFormattedIdentifier()
        {
            return this.Name;
        }

        /// <summary>
        /// Clones this instance.
        /// </summary>
        /// <returns></returns>
        public ValueList Clone()
        {
            return this.Clone(null);
        }

        /// <summary>
        /// Clones this instance.
        /// </summary>
        /// <returns></returns>
        internal ValueList Clone(string renameName)
        {
            ValueList clone = new ValueList(this.id, renameName == null ? this.name : renameName, this.UseNamespaceInterpretation, this.UseItemTranslations);
            foreach (ValueListItem entry in this)
            {
                clone.Add(entry.Clone());
            }

            return clone;
        }

        /// <summary>
        /// Adds the specified ValueListItem to the ValueList.
        /// </summary>
        /// <param name="valueListItem">The ValueListItem.</param>
        public void Add(ValueListItem valueListItem)
        {
            if (valueListItem == null)
            {
                throw new ArgumentNullException("valueListItem");
            }

            try
            {
                this.collection.Add(valueListItem.Value, valueListItem);
            }
            catch (ArgumentException)
            {
                throw new ArgumentException("The specified ValueListItem's value is already present in the ValueList.");
            }
        }

        internal void Insert(int index, ValueListItem valueListItem)
        {
            if (valueListItem == null)
            {
                throw new ArgumentNullException("valueListItem");
            }

            try
            {
                this.collection.Insert(index, valueListItem.Value, valueListItem);
            }
            catch (ArgumentException)
            {
                throw new ArgumentException("The specified ValueListItem's value is already present in the ValueList.");
            }
        }

        /// <summary>
        /// Clears this ValueList's items.
        /// </summary>
        public void Clear()
        {
            this.collection.Clear();
        }

        /// <summary>
        /// Removes the specified value from the ValueList.
        /// </summary>
        /// <param name="value">The value to remove.</param>
        /// <returns></returns>
        public bool Remove(string value)
        {
            if (value == null)
            {
                throw new ArgumentNullException("value");
            }

            return this.collection.Remove(value);
        }

        /// <summary>
        /// Gets the <see cref="ZachJohnson.Promptu.UserModel.ValueListItem"/> with the specified value, case-insensitive.
        /// </summary>
        /// <value>The value to find.</value>
        public ValueListItem this[string value]
        {
            get 
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }

                return this.collection[value, CaseSensitivity.Insensitive]; 
            }
        }

        /// <summary>
        /// Tries to get the <see cref="ZachJohnson.Promptu.UserModel.ValueListItem"/> with the specified value.
        /// </summary>
        /// <param name="value">The value to find.</param>
        /// <param name="found">if set to <c>true</c> the item was found.</param>
        /// <returns></returns>
        public ValueListItem TryGet(string value, out bool found)
        {
            if (value == null)
            {
                throw new ArgumentNullException("value");
            }

            return this.collection.TryGetItem(value, CaseSensitivity.Insensitive, out found);
        }

        internal string TryFindValue(string startsWith)
        {
            return this.collection.TryFindKey(startsWith, CaseSensitivity.Insensitive);
        }

        /// <summary>
        /// Gets the item count.
        /// </summary>
        /// <value>The item count.</value>
        public int Count
        {
            get { return this.collection.Count; }
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Collections.Generic.IEnumerator{ZachJohnson.Promptu.UserModel.ValueListItem}"/> that can be used to iterate through the collection.
        /// </returns>
        public IEnumerator<ValueListItem> GetEnumerator()
        {
            foreach (string key in this.collection.Keys)
            {
                yield return this.collection[key, CaseSensitivity.Insensitive];
            }
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate through the collection.
        /// </returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        /// <summary>
        /// Sorts the ValueList.
        /// </summary>
        public void Sort()
        {
            this.collection.SortKeys();
        }

        /// <summary>
        /// Gets the <see cref="ZachJohnson.Promptu.UserModel.ValueListItem"/> at the specified index.
        /// </summary>
        /// <value></value>
        public ValueListItem this[int index]
        {
            get { return this.collection[index]; }
        }

        /// <summary>
        /// Determines whether the ValueList contains the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// 	<c>true</c> if the specified value is in the ValueList; otherwise, <c>false</c>.
        /// </returns>
        public bool ContainsValue(string value)
        {
            if (value == null)
            {
                throw new ArgumentNullException("value");
            }

            return this.collection.Contains(value, CaseSensitivity.Insensitive);
        }

        /// <summary>
        /// Removes the item at the specified index.
        /// </summary>
        /// <param name="index">The index.</param>
        public void RemoveAt(int index)
        {
            this.collection.Remove(this.collection.Keys[index]);
        }

        /// <summary>
        /// Returns the index of the specified value if found, otherwise, -1.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public int IndexOf(string value)
        {
            if (value == null)
            {
                throw new ArgumentNullException("value");
            }

            return this.collection.Keys.IndexOf(value);
        }

        /// <summary>
        /// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </returns>
        public override string ToString()
        {
            //StringBuilder builder = new StringBuilder();
            //builder.AppendLine("ValueList:");
            ////bool doneFirst = false;
            //foreach (ValueListItem item in this)
            //{
            //    //if (doneFirst)
            //    //{
            //    //    builder.Append(", ");
            //    //}

            //    builder.AppendFormat("\"{0}\"", item.Value);
            //    if (this.UseItemTranslations && item.AllowTranslation)
            //    {
            //        builder.Append(" Translation: ");
            //        if (item.Translation == null)
            //        {
            //            builder.Append("null");
            //        }
            //        else
            //        {
            //            builder.AppendFormat("\"{0}\"", item.Translation);
            //        }
            //    }

            //    builder.AppendLine();

            //    //doneFirst = true;
            //}

            //return builder.ToString();

            return this.Name;
        }

        /// <summary>
        /// Constructs a ValueList from its XML form.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <returns></returns>
        public static ValueList FromXml(XmlNode node)
        {
            if (node == null)
            {
                throw new ArgumentNullException("node");
            }

            string name = null;
            Id id = null;
            bool useItemTranslations = false;
            bool useNamespaceInterpretation = false;

            foreach (XmlAttribute attribute in node.Attributes)
            {
                switch (attribute.Name.ToUpperInvariant())
                {
                    case "ID":
                        try
                        {
                            id = new Id(attribute.Value);
                        }
                        catch (FormatException)
                        {
                        }
                        catch (OverflowException)
                        {
                        }

                        break;
                    case "NAME":
                        name = attribute.Value;
                        break;
                    case "TRANSLATE":
                        useItemTranslations = Utilities.TryParseBoolean(attribute.Value, useItemTranslations);
                        //try
                        //{
                        //    useItemTranslations = Convert.ToBoolean(attribute.Value);
                        //}
                        //catch (FormatException)
                        //{
                        //}

                        break;
                    case "USENAMESPACEINTERPRETATION":
                        useNamespaceInterpretation = Utilities.TryParseBoolean(attribute.Value, useNamespaceInterpretation);
                        //try
                        //{
                        //    useNamespaceInterpretation = Convert.ToBoolean(attribute.Value);
                        //}
                        //catch (FormatException)
                        //{
                        //}

                        break;
                    default:
                        break;
                }
            }

            if (name == null)
            {
                throw new LoadException("Missing 'name' attribute.");
            }
            else if (name.Length == 0)
            {
                throw new LoadException("Empty 'name' attribute.");
            }

            ValueList valueList = new ValueList(id, name, useNamespaceInterpretation, useItemTranslations);

            foreach (XmlNode innerNode in node.ChildNodes)
            {
                switch (innerNode.Name.ToUpperInvariant())
                {
                    case "ITEM":
                        string value = null;
                        string translation = null;
                        bool useTranslation = false;
                        int imageIndex = -1;

                        foreach (XmlAttribute itemAttribute in innerNode.Attributes)
                        {
                            switch (itemAttribute.Name.ToUpperInvariant())
                            {
                                case "VALUE":
                                    value = itemAttribute.Value;
                                    break;
                                case "TRANSLATION":
                                    translation = itemAttribute.Value;
                                    break;
                                case "IMAGEINDEX":
                                    imageIndex = Utilities.TryParseInt32(itemAttribute.Value, imageIndex);
                                    //try
                                    //{
                                    //    imageIndex = Convert.ToInt32(itemAttribute.Value);
                                    //}
                                    //catch (FormatException)
                                    //{
                                    //}
                                    //catch (OverflowException)
                                    //{
                                    //}

                                    break;
                                case "USETRANSLATION":
                                    useTranslation = Utilities.TryParseBoolean(itemAttribute.Value, useTranslation);
                                    //try
                                    //{
                                    //    useTranslation = Convert.ToBoolean(itemAttribute.Value);
                                    //}
                                    //catch (FormatException)
                                    //{
                                    //}

                                    break;
                                default:
                                    break;
                            }
                        }

                        if (!String.IsNullOrEmpty(value))
                        {
                            valueList.Add(new ValueListItem(value, translation, useTranslation, imageIndex));
                        }

                        break;
                    default:
                        break;
                }
            }

            return valueList;
        }

        /// <summary>
        /// Converts the ValueList into its XML form.
        /// </summary>
        /// <param name="name">The name to name the XmlNode.</param>
        /// <param name="document">The document to use in creating the XML.</param>
        /// <returns></returns>
        public XmlNode ToXml(string name, XmlDocument document)
        {
            if (name == null)
            {
                throw new ArgumentNullException("name");
            }
            else if (document == null)
            {
                throw new ArgumentNullException("document");
            }

            XmlNode node = document.CreateElement(name);

            if (this.id != null)
            {
                node.Attributes.Append(XmlUtilities.CreateAttribute("id", this.id, document));
            }

            node.Attributes.Append(XmlUtilities.CreateAttribute("name", this.name, document));

            if (this.useNamespaceInterpretation)
            {
                node.Attributes.Append(XmlUtilities.CreateAttribute("useNamespaceInterpretation", this.useNamespaceInterpretation, document));
            }

            if (this.useItemTranslations)
            {
                node.Attributes.Append(XmlUtilities.CreateAttribute("translate", this.useItemTranslations, document));
            }

            foreach (ValueListItem item in this)
            {
                XmlNode valueNode = document.CreateElement("Item");
                valueNode.Attributes.Append(XmlUtilities.CreateAttribute("value", item.Value, document));
                if (item.Translation != null)
                {
                    valueNode.Attributes.Append(XmlUtilities.CreateAttribute("translation", item.Translation, document));
                }

                if (item.AllowTranslation)
                {
                    valueNode.Attributes.Append(XmlUtilities.CreateAttribute("useTranslation", item.AllowTranslation, document));
                }

                if (item.ImageIndex != -1)
                {
                    valueNode.Attributes.Append(XmlUtilities.CreateAttribute("imageIndex", item.ImageIndex, document));
                }

                node.AppendChild(valueNode);
            }

            return node;
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
