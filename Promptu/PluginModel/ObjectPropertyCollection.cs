//-----------------------------------------------------------------------
// <copyright file="ObjectPropertyCollection.cs" company="ZachJohnson">
//     Copyright (c) Zach Johnson. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace ZachJohnson.Promptu.PluginModel
{
    using System;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Xml;

    public class ObjectPropertyCollection : Collection<ObjectPropertyBase>
    {
        public ObjectPropertyCollection()
        {
        }

        public event EventHandler<ObjectPropertyChangedEventArgs> PropertyChanged;

        public ObjectPropertyBase TryGet(string id, out bool found)
        {
            foreach (ObjectPropertyBase childItem in this)
            {
                if (childItem.Id == id)
                {
                    found = true;
                    return childItem;
                }
            }

            found = false;
            return null;
        }

        internal void LoadSettingsFrom(XmlNode node)
        {
            if (node == null)
            {
                throw new ArgumentNullException("node");
            }

            foreach (XmlNode innerNode in node.ChildNodes)
            {
                if (innerNode.Name.ToUpperInvariant() != "PROPERTY")
                {
                    continue;
                }

                string id = null;
                string value = null;

                foreach (XmlAttribute attribute in innerNode.Attributes)
                {
                    switch (attribute.Name.ToUpperInvariant())
                    {
                        case "ID":
                            id = attribute.Value;
                            break;
                        case "VALUE":
                            value = attribute.Value;
                            break;
                        default:
                            break;
                    }
                }

                if (id == null || value == null)
                {
                    continue;
                }

                bool found;
                ObjectPropertyBase associatedProperty = this.TryGet(id, out found);

                if (!found)
                {
                    return;
                }

                Type valueType = associatedProperty.ValueType;

                TypeConverter converter = TypeDescriptor.GetConverter(valueType);

                if (converter.CanConvertFrom(typeof(string)))
                {
                    try
                    {
                        associatedProperty.ObjectValue = ConvertFrom(converter, value);
                    }
                    catch (Exception)
                    {
                    }
                }
            }
        }

        internal void SerializeTo(XmlNode node)
        {
            if (node == null)
            {
                throw new ArgumentNullException("node");
            }

            foreach (ObjectPropertyBase property in this)
            {
                Type valueType = property.ValueType;
                TypeConverter converter = TypeDescriptor.GetConverter(valueType);

                if (converter.CanConvertTo(typeof(string)))
                {
                    XmlNode propertyNode = node.OwnerDocument.CreateElement("Property");

                    propertyNode.Attributes.Append(XmlUtilities.CreateAttribute(
                        "id", 
                        property.Id, 
                        node.OwnerDocument));

                    propertyNode.Attributes.Append(XmlUtilities.CreateAttribute(
                        "value", 
                        converter.ConvertTo(null, System.Globalization.CultureInfo.InvariantCulture, property.ObjectValue, typeof(string)),
                        node.OwnerDocument));

                    node.AppendChild(propertyNode);
                }
            }
        }

        protected override void InsertItem(int index, ObjectPropertyBase item)
        {
            base.InsertItem(index, item);

            if (item != null)
            {
                item.PropertyChanged += this.HandlePropertyChanged;
            }
        }

        protected override void RemoveItem(int index)
        {
            this.RemoveHandlerOn(index);
            base.RemoveItem(index);
        }

        protected override void ClearItems()
        {
            foreach (ObjectPropertyBase item in this)
            {
                item.PropertyChanged -= this.HandlePropertyChanged;
            }

            base.ClearItems();
        }

        protected override void SetItem(int index, ObjectPropertyBase item)
        {
            this.RemoveHandlerOn(index);
            
            base.SetItem(index, item);

            if (item != null)
            {
                item.PropertyChanged += this.HandlePropertyChanged;
            }
        }

        protected virtual void OnPropertyChanged(ObjectPropertyChangedEventArgs e)
        {
            EventHandler<ObjectPropertyChangedEventArgs> handler = this.PropertyChanged;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        private static object ConvertFrom(TypeConverter converter, object value)
        {
            try
            {
                return converter.ConvertFrom(null, System.Globalization.CultureInfo.InvariantCulture, value);
            }
            catch (Exception)
            {
                // Argh. Not everyone follows the design guidelines.  Hence the need for a global catch.
            }

            return converter.ConvertFrom(null, System.Globalization.CultureInfo.CurrentCulture, value);
        }

        private void RemoveHandlerOn(int index)
        {
            if (index >= 0 && index < this.Count)
            {
                ObjectPropertyBase item = this[index];
                item.PropertyChanged -= this.HandlePropertyChanged;
            }
        }

        private void HandlePropertyChanged(object sender, EventArgs e)
        {
            ObjectPropertyBase property = sender as ObjectPropertyBase;
            if (property != null)
            {
                this.OnPropertyChanged(new ObjectPropertyChangedEventArgs(property));
            }
        }
    }
}
