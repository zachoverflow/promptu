using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.IO;
using System.Globalization;

namespace ZachJohnson.Promptu
{
    internal class ExternalCodeObject<T>
    {
        private string assembly;
        private string fullyQualifiedTypeName;
        private Type type;

        public ExternalCodeObject(string assembly, string fullyQualifiedTypeName)
        {
            if (assembly == null)
            {
                throw new ArgumentNullException("assembly");
            }
            else if (fullyQualifiedTypeName == null)
            {
                throw new ArgumentNullException("fullyQualifiedTypeName");
            }

            this.assembly = assembly;
            this.fullyQualifiedTypeName = fullyQualifiedTypeName;
        }

        public T GetNewInstance()
        {
            if (this.type == null)
            {
                Assembly reflectionAssembly = null;
                try
                {
                    reflectionAssembly = Assembly.LoadFrom(assembly);
                }
                catch (IOException ex)
                {
                    throw new LoadException("Unable to load the external object.", ex);
                }
                catch (BadImageFormatException ex)
                {
                    throw new LoadException("Unable to load the external object.", ex);
                }

                if (reflectionAssembly == null)
                {
                    throw new LoadException("Unable to load the external object.");
                }

                Type codeObjectType = null;
                try
                {
                    codeObjectType = reflectionAssembly.GetType(this.fullyQualifiedTypeName);
                }
                catch (Exception ex)
                {
                    throw new LoadException("Unable to load the external object.", ex);
                }

                if (codeObjectType == null)
                {
                    throw new LoadException("Unable to load the external object.");
                }

                Type typeofT = typeof(T);

                if (!typeofT.IsInterface)
                {
                    if (!codeObjectType.IsSubclassOf(typeofT))
                    {
                        throw new LoadException(String.Format(CultureInfo.CurrentCulture, "The external object does not derive from '{0}'.", typeofT.Name));
                    }
                }
                else
                {
                    Type iface = null;
                    try
                    {
                        iface = type.GetInterface(typeofT.Name);
                    }
                    catch (AmbiguousMatchException ex)
                    {
                        throw new LoadException("Unable to load the external object.", ex);
                    }

                    if (iface == null)
                    {
                        throw new LoadException(String.Format(CultureInfo.CurrentCulture, "The external object does not derive from '{0}'.", typeofT.Name));
                    }
                }

                this.type = codeObjectType;
            }

            try
            {
                return (T)Activator.CreateInstance(this.type);
            }
            catch (TargetInvocationException ex)
            {
                throw new LoadException("Unable to create an instance the external object.", ex);
            }
            catch (MissingMethodException ex)
            {
                throw new LoadException("Unable to create an instance the external object.", ex);
            }
            catch (MemberAccessException ex)
            {
                throw new LoadException("Unable to create an instance the external object.", ex);
            }
        }
    }
}
