using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace ZachJohnson.Promptu.UserModel
{
    internal class LoadedFunctionInfo
    {
        private AssemblyReference assemblyReference;
        private object classInstance;
        private MethodInfo invokingMethod;
        private ParameterInfo[] parameters;

        public LoadedFunctionInfo(AssemblyReference assemblyReference, object classInstance, MethodInfo invokingMethod)
        {
            if (assemblyReference == null)
            {
                throw new ArgumentNullException("assemblyReference");
            }
            else if (classInstance == null)
            {
                throw new ArgumentNullException("classInstance");
            }
            else if (invokingMethod == null)
            {
                throw new ArgumentNullException("invokingMethod");
            }

            this.assemblyReference = assemblyReference;
            this.classInstance = classInstance;
            this.invokingMethod = invokingMethod;
        }

        public AssemblyReference AssemblyReference
        {
            get { return this.assemblyReference; }
        }

        public object ClassInstance
        {
            get { return this.classInstance; }
        }

        public MethodInfo InvokingMethod
        {
            get { return this.invokingMethod; }
        }

        public ParameterInfo[] Parameters
        {
            get
            {
                if (this.parameters == null)
                {
                    this.parameters = invokingMethod.GetParameters();
                }

                return this.parameters;
            }
        }
    }
}
