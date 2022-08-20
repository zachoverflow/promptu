using System;
using System.Collections.Generic;
using System.Text;

namespace ZachJohnson.Promptu.UserModel.Differencing
{
    internal class FunctionDiff : Diff<Function, FunctionDiff>
    {
        private DiffEntry<string> name;
        private DiffEntry<string> invocationClass;
        private DiffEntry<string> methodName;
        private DiffEntry<string> assemblyReferenceName;
        private List<DiffEntry<FunctionParameter>> parameters = new List<DiffEntry<FunctionParameter>>();
        private DiffEntry<ReturnValue> returnValue;
        private bool parameterCountHasChanged;

        public FunctionDiff(Function baseItem, Function revisedItem)
            : base(baseItem, revisedItem)
        {
            if (baseItem == null && revisedItem == null)
            {
                throw new ArgumentNullException("'baseItem' and 'revisedItem' cannot both be null.");
            }
            else if (baseItem == null)
            {
                this.Status = DiffStatus.New;
                this.HasChanges = true;
            }
            else if (revisedItem == null)
            {
                this.Status = DiffStatus.Deleted;
                this.HasChanges = true;
            }
            else
            {
                this.name = new DiffEntry<string>(baseItem.Name, revisedItem.Name, ValueComparisons.StringComparison);
                this.invocationClass = new DiffEntry<string>(baseItem.InvocationClass, revisedItem.InvocationClass, ValueComparisons.StringComparison);
                this.methodName = new DiffEntry<string>(baseItem.MethodName, revisedItem.MethodName, ValueComparisons.StringComparison);
                this.assemblyReferenceName = new DiffEntry<string>(baseItem.AssemblyReferenceName, revisedItem.AssemblyReferenceName, ValueComparisons.StringComparison);
                this.returnValue = new DiffEntry<ReturnValue>(baseItem.ReturnValue, revisedItem.ReturnValue, ValueComparisons.ReturnValueComparison);

                int largestParameterCount = baseItem.Parameters.Count; 

                if (revisedItem.Parameters.Count > largestParameterCount)
                {
                    largestParameterCount = revisedItem.Parameters.Count;
                }

                bool parametersHaveChanged = false;
                
                for (int i = 0; i < largestParameterCount; i++)
                {
                    FunctionParameter baseParameter = null;
                    FunctionParameter revisedParameter = null;

                    if (i < baseItem.Parameters.Count)
                    {
                        baseParameter = baseItem.Parameters[i];
                    }

                    if (i < revisedItem.Parameters.Count)
                    {
                        revisedParameter = revisedItem.Parameters[i];
                    }

                    DiffEntry<FunctionParameter> diffEntry = new DiffEntry<FunctionParameter>(baseParameter, revisedParameter, ValueComparisons.FunctionParameterComparison);

                    this.parameters.Add(diffEntry);

                    if (diffEntry.HasChanged)
                    {
                        parametersHaveChanged = true;
                    }

                    if (diffEntry.BaseValue == null || diffEntry.RevisedValue == null)
                    {
                        this.parameterCountHasChanged = true;
                    }
                }

                //this.parameterSignature = new DiffEntry<string>(baseItem.ParameterSignature, revisedItem.ParameterSignature, ValueComparisons.StringComparison);
                this.HasChanges = this.name.HasChanged
                    || this.invocationClass.HasChanged
                    || this.methodName.HasChanged
                    || this.assemblyReferenceName.HasChanged
                    || this.returnValue.HasChanged
                    || parametersHaveChanged;
            }
        }

        public DiffEntry<string> Name
        {
            get { return this.name; }
        }

        public DiffEntry<string> InvocationClass
        {
            get { return this.invocationClass; }
        }

        public DiffEntry<string> MethodName
        {
            get { return this.methodName; }
        }

        public DiffEntry<string> AssemblyReferenceName
        {
            get { return this.assemblyReferenceName; }
        }

        public List<DiffEntry<FunctionParameter>> Parameters
        {
            get { return this.parameters; }
        }

        public DiffEntry<ReturnValue> ReturnValue
        {
            get { return this.returnValue; }
        }

        //public DiffEntry<string> ParameterSignature
        //{
        //    get { return this.parameterSignature; }
        //}

        protected override bool ConflictsWithCore(FunctionDiff diff)
        {
            if (this.RevisedItem != null && diff.RevisedItem != null)
            {
                return (this.RevisedItem.ParameterSignature == diff.RevisedItem.ParameterSignature
                    && this.RevisedItem.Name.ToUpperInvariant() == diff.RevisedItem.Name.ToUpperInvariant());
            }

            return false;
        }

        //protected override bool IsSimilarToCore(FunctionDiff diff)
        //{
        //    string thisName;
        //    int thisParameterCount;

        //    string diffName;
        //    int diffParameterCount;

        //    if (this.Status == DiffStatus.Deleted)
        //    {
        //        thisName = this.BaseItem.Name;
        //        thisParameterCount = this.BaseItem.NumberOfParameters;
        //    }
        //    else
        //    {
        //        thisName = this.RevisedItem.Name;
        //        thisParameterCount = this.RevisedItem.NumberOfParameters;
        //    }

        //    if (diff.Status == DiffStatus.Deleted)
        //    {
        //        diffName = diff.BaseItem.Name;
        //        diffParameterCount = diff.BaseItem.NumberOfParameters;
        //    }
        //    else
        //    {
        //        diffName = diff.RevisedItem.Name;
        //        diffParameterCount = diff.RevisedItem.NumberOfParameters;
        //    }

        //    return thisName == diffName && thisParameterCount == diffParameterCount;
        //}

        protected override bool GetIdentifierHasChangedCore()
        {
            return this.Name.HasChanged || this.parameterCountHasChanged;
        }
    }
}
