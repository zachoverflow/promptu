//using System;
//using System.Collections.Generic;
//using System.Text;
//using System.ComponentModel;
//using ZachJohnson.Promptu.SkinApi;

//namespace ZachJohnson.Promptu.Skins
//{
//    internal class PropertiesMiddleMan : ICustomTypeDescriptor
//    {
//        private object obj;

//        public PropertiesMiddleMan(object obj)
//        {
//            this.obj = obj;
//        }

//        private object PropertyObject
//        {
//            get
//            {
//                object propertyObject = this.obj;
//                IInstanceOnDemand instanceOnDemandObject = propertyObject as IInstanceOnDemand;
//                if (instanceOnDemandObject != null)
//                {
//                    propertyObject = instanceOnDemandObject.GetInstance();
//                }

//                return propertyObject;
//            }
//        }

//        public AttributeCollection GetAttributes()
//        {
//            return TypeDescriptor.GetAttributes(this.PropertyObject, true);
//        }

//        public string GetClassName()
//        {
//            return TypeDescriptor.GetClassName(this.PropertyObject, true);
//        }

//        public string GetComponentName()
//        {
//            return TypeDescriptor.GetComponentName(this.PropertyObject, true);
//        }

//        public TypeConverter GetConverter()
//        {
//            return TypeDescriptor.GetConverter(this.PropertyObject, true);//new SortingTypeConverter(this);
//        }

//        public EventDescriptor GetDefaultEvent()
//        {
//            return TypeDescriptor.GetDefaultEvent(this.PropertyObject, true);
//        }

//        public PropertyDescriptor GetDefaultProperty()
//        {
//            return TypeDescriptor.GetDefaultProperty(this.PropertyObject, true);
//        }

//        public object GetEditor(Type editorBaseType)
//        {
//            return TypeDescriptor.GetEditor(this.PropertyObject, editorBaseType, true);
//        }

//        public EventDescriptorCollection GetEvents(Attribute[] attributes)
//        {
//            return TypeDescriptor.GetEvents(this.PropertyObject, attributes, true);
//        }

//        public EventDescriptorCollection GetEvents()
//        {
//            return TypeDescriptor.GetEvents(this.PropertyObject, true);
//        }

//        public PropertyDescriptorCollection GetProperties()
//        {
//            PropertyDescriptorCollection skinProperties = TypeDescriptor.GetProperties(this.PropertyObject, new Attribute[] { new UserEditableAttribute(true) });
//            List<PropertyDescriptor> properties = new List<PropertyDescriptor>();
//            foreach (PropertyDescriptor property in skinProperties)
//            {
//                if (((UserEditableAttribute)property.Attributes[typeof(UserEditableAttribute)]).EditableByUser)
//                {
//                    properties.Add(property);
//                }
//            }

//            return new PropertyDescriptorCollection(properties.ToArray());
//        }

//        public PropertyDescriptorCollection GetPersistingProperties()
//        {
//            PropertyDescriptorCollection skinProperties = TypeDescriptor.GetProperties(this.PropertyObject, new Attribute[] { new PersistValueAttribute(true) });
//            List<PropertyDescriptor> properties = new List<PropertyDescriptor>();
//            foreach (PropertyDescriptor property in skinProperties)
//            {
//                if (((PersistValueAttribute)property.Attributes[typeof(PersistValueAttribute)]).PersistValue)
//                {
//                    properties.Add(property);
//                }
//            }

//            return new PropertyDescriptorCollection(properties.ToArray());
//        }

//        public PropertyDescriptorCollection GetProperties(Attribute[] attributes)
//        {
//            return this.GetProperties();
//        }

//        public object GetPropertyOwner(PropertyDescriptor pd)
//        {
//            return this.PropertyObject;
//        }
//    }
//}
