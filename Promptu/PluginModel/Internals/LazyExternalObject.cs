//-----------------------------------------------------------------------
// <copyright file="LazyExternalObject.cs" company="ZachJohnson">
//     Copyright (c) Zach Johnson. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace ZachJohnson.Promptu.PluginModel.Internals
{
    using System;
    using System.IO;
    using System.Reflection;
    using System.Runtime.InteropServices;

    internal class LazyExternalObject<T> where T : class
    {
        private T instance;
        private FileSystemFile assemblyPath;
        private string className;
        private string traceCategory;
        private bool hasLoaded;
        private bool loadIsEnabled;

        public LazyExternalObject(FileSystemFile assemblyPath, string className, string traceCategory)
        {
            this.assemblyPath = assemblyPath;
            this.className = className;
            this.traceCategory = traceCategory;
        }

        public event EventHandler Loaded;

        public bool HasLoaded
        {
            get { return this.hasLoaded; }
        }

        public FileSystemFile AssemblyPath
        {
            get { return this.assemblyPath; }
        }

        public bool LoadIsEnabled
        {
            get { return this.loadIsEnabled; }
            set { this.loadIsEnabled = value; }
        }

        public T Instance
        {
            get
            {
                if (!this.hasLoaded && this.loadIsEnabled)
                {
                    this.instance = LoadInstance(this.assemblyPath, this.className, this.traceCategory);
                    this.hasLoaded = true;
                    this.OnLoaded(EventArgs.Empty);
                }

                return this.instance;
            }
        }

        protected virtual void OnLoaded(EventArgs e)
        {
            EventHandler handler = this.Loaded;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        private static T LoadInstance(FileSystemFile assemblyFile, string className, string traceCategory)
        {
            if (!assemblyFile.Exists)
            {
                ErrorConsole.WriteLineFormat(traceCategory, "Error: \"{0}\" does not exist.", assemblyFile);
                return null;
            }

            Assembly assembly;

            try
            {
                assembly = Assembly.LoadFile(assemblyFile);
            }
            catch (FileNotFoundException ex)
            {
                ErrorConsole.WriteLineFormat(traceCategory, "Error loading \"{0}\".  Message: {1}.", assemblyFile.Name, ex.Message);
                return null;
            }
            catch (FileLoadException ex)
            {
                ErrorConsole.WriteLineFormat(traceCategory, "Error loading \"{0}\".  Message: {1}.", assemblyFile.Name, ex.Message);
                return null;
            }
            catch (BadImageFormatException ex)
            {
                ErrorConsole.WriteLineFormat(traceCategory, "Error loading \"{0}\".  Message: {1}.", assemblyFile.Name, ex.Message);
                return null;
            }

            Type classType;

            try
            {
                classType = assembly.GetType(className);
            }
            catch (ArgumentException ex)
            {
                ErrorConsole.WriteLineFormat(traceCategory, "Error loading the class \"{0}\".  Message: {1}.", className, ex.Message);
                return null;
            }
            catch (FileNotFoundException ex)
            {
                ErrorConsole.WriteLineFormat(traceCategory, "Error loading the class \"{0}\".  Message: {1}.", className, ex.Message);
                return null;
            }
            catch (FileLoadException ex)
            {
                ErrorConsole.WriteLineFormat(traceCategory, "Error loading the class \"{0}\".  Message: {1}.", className, ex.Message);
                return null;
            }
            catch (BadImageFormatException ex)
            {
                ErrorConsole.WriteLineFormat(traceCategory, "Error loading the class \"{0}\".  Message: {1}.", className, ex.Message);
                return null;
            }

            if (classType == null)
            {
                ErrorConsole.WriteLineFormat(traceCategory, "The class \"{0}\" was not found in the assembly \"{0}\".", className, assemblyFile.Name);
                return null;
            }

            if (!classType.IsSubclassOf(typeof(T)))
            {
                ErrorConsole.WriteLineFormat(traceCategory, "\"{0}\" does not inherit from \"{1}\".", className, typeof(T).Name);
                return null;
            }

            object instance;

            try
            {
                instance = Activator.CreateInstance(classType);
            }
            catch (ArgumentException ex)
            {
                ErrorConsole.WriteLineFormat(traceCategory, "Error loading the class \"{0}\".  Message: {1}.", className, ex.Message);
                return null;
            }
            catch (NotSupportedException ex)
            {
                ErrorConsole.WriteLineFormat(traceCategory, "Error loading the class \"{0}\".  Message: {1}.", className, ex.Message);
                return null;
            }
            catch (TargetInvocationException ex)
            {
                ErrorConsole.WriteLineFormat(traceCategory, "Error loading the class \"{0}\".  Message: {1}.", className, ex.Message);
                return null;
            }
            catch (MethodAccessException ex)
            {
                ErrorConsole.WriteLineFormat(traceCategory, "Error loading the class \"{0}\".  Message: {1}.", className, ex.Message);
                return null;
            }
            catch (MemberAccessException ex)
            {
                ErrorConsole.WriteLineFormat(traceCategory, "Error loading the class \"{0}\".  Message: {1}.", className, ex.Message);
                return null;
            }
            catch (InvalidComObjectException ex)
            {
                ErrorConsole.WriteLineFormat(traceCategory, "Error loading the class \"{0}\".  Message: {1}.", className, ex.Message);
                return null;
            }
            catch (COMException ex)
            {
                ErrorConsole.WriteLineFormat(traceCategory, "Error loading the class \"{0}\".  Message: {1}.", className, ex.Message);
                return null;
            }
            catch (TypeLoadException ex)
            {
                ErrorConsole.WriteLineFormat(traceCategory, "Error loading the class \"{0}\".  Message: {1}.", className, ex.Message);
                return null;
            }

            return (T)instance;
        }
    }
}
