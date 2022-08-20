//-----------------------------------------------------------------------
// <copyright file="BindingNode.cs" company="ZachJohnson">
//     Copyright (c) Zach Johnson. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace ZachJohnson.Promptu.PluginModel.Internals
{
    using System;
    using System.Reflection;

    internal class BindingNode
    {
        private string propertyName;

        public BindingNode(string propertyName)
        {
            this.propertyName = propertyName;
        }

        public string PropertyName
        {
            get { return this.propertyName; }
        }

        public object GetValue(object context)
        {
            PropertyInfo propertyInfo = this.GetPropertyInfo(context);

            if (propertyInfo == null)
            {
                return null;
            }

            try
            {
                return propertyInfo.GetValue(context, null);
            }
            catch (MethodAccessException)
            {
            }
            catch (ArgumentException)
            {
            }
            catch (TargetInvocationException)
            {
            }
            catch (TargetException)
            {
            }
            catch (TargetParameterCountException)
            {
            }
            catch (InvalidCastException)
            {
            }

            return null;
        }

        public void SetValue(object context, object value)
        {
            PropertyInfo propertyInfo = this.GetPropertyInfo(context);

            if (propertyInfo == null)
            {
                return;
            }

            try
            {
                propertyInfo.SetValue(context, value, null);
            }
            catch (MethodAccessException)
            {
            }
            catch (ArgumentException)
            {
            }
            catch (TargetInvocationException)
            {
            }
            catch (TargetException)
            {
            }
            catch (TargetParameterCountException)
            {
            }
            catch (InvalidCastException)
            {
            }
        }

        private PropertyInfo GetPropertyInfo(object context)
        {
            if (context == null)
            {
                return null;
            }

            Type objectType = context.GetType();
            PropertyInfo propertyInfo = null;

            try
            {
                propertyInfo = objectType.GetProperty(
                    this.propertyName,
                    BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
            }
            catch (AmbiguousMatchException)
            {
            }

            return propertyInfo;
        }
    }
}
