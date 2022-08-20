// Copyright 2022 Zach Johnson
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

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
