using System;
using System.Collections.Generic;
using System.Text;
using ZachJohnson.Promptu.UI;
using ZachJohnson.Promptu.UIModel;
using System.Globalization;

namespace ZachJohnson.Promptu.UserModel.Differencing
{
    internal class FunctionDiffDiff : DiffDiff<FunctionDiff, Function, FunctionDiffDiff>
    {
        private DiffDiffEntry<string> name;
        private DiffDiffEntry<string> invocationClass;
        private DiffDiffEntry<string> methodName;
        private DiffDiffEntry<string> assemblyReferenceName;
        private DiffDiffEntry<ReturnValue> returnValue;
        private List<DiffDiffEntry<FunctionParameter>> parameters = new List<DiffDiffEntry<FunctionParameter>>();
        //private bool parametersHaveChanges;

        public FunctionDiffDiff(
            FunctionDiff priorityDiff, 
            FunctionDiff secondaryDiff)
            : base(priorityDiff, secondaryDiff)
        {
            if (priorityDiff == null && secondaryDiff == null)
            {
                throw new ArgumentException("'priorityDiff' and 'secondaryDiff' cannot both be null.");
            }
            else if (priorityDiff != null && secondaryDiff != null && priorityDiff.BaseItem != secondaryDiff.BaseItem)
            {
                throw new ArgumentException("The two diffs do not share the same base.  Cannot diff diffs of difference bases.");
            }

            this.name = new DiffDiffEntry<string>(
                priorityDiff == null ? null : priorityDiff.Name,
                secondaryDiff == null ? null : secondaryDiff.Name);

            this.invocationClass = new DiffDiffEntry<string>(
                priorityDiff == null ? null : priorityDiff.InvocationClass,
                secondaryDiff == null ? null : secondaryDiff.InvocationClass);

            this.methodName = new DiffDiffEntry<string>(
                priorityDiff == null ? null : priorityDiff.MethodName,
                secondaryDiff == null ? null : secondaryDiff.MethodName);

            this.assemblyReferenceName = new DiffDiffEntry<string>(
                priorityDiff == null ? null : priorityDiff.AssemblyReferenceName,
                secondaryDiff == null ? null : secondaryDiff.AssemblyReferenceName);

            this.returnValue = new DiffDiffEntry<ReturnValue>(
                priorityDiff == null ? null : priorityDiff.ReturnValue,
                secondaryDiff == null ? null : secondaryDiff.ReturnValue);

            int largestParameterCount = 0;
            
            if (priorityDiff != null)
            {
                largestParameterCount = priorityDiff.Parameters.Count;
            }

            if (secondaryDiff != null && secondaryDiff.Parameters.Count > largestParameterCount)
            {
                largestParameterCount = secondaryDiff.Parameters.Count;
            }

            for (int i = 0; i < largestParameterCount; i++)
            {
                DiffEntry<FunctionParameter> priorityParameter = null;
                DiffEntry<FunctionParameter> secondaryParameter = null;

                if (priorityDiff != null && i < priorityDiff.Parameters.Count)
                {
                    priorityParameter = priorityDiff.Parameters[i];
                }

                if (priorityDiff != null && i < secondaryDiff.Parameters.Count)
                {
                    secondaryParameter = secondaryDiff.Parameters[i];
                }

                DiffDiffEntry<FunctionParameter> diffDiffEntry = new DiffDiffEntry<FunctionParameter>(priorityParameter, secondaryParameter);

                this.parameters.Add(diffDiffEntry);

                //if (diffDiffEntry.HasChanges)
                //{
                //    this.parametersHaveChanges = true;
                //}
            }

            //this.parameterSignature = new DiffDiffEntry<string>(
            //    priorityDiff == null ? null : priorityDiff.ParameterSignature,
            //    secondaryDiff == null ? null : secondaryDiff.ParameterSignature);

            if (priorityDiff == null)
            {
                this.HasConflictingChanges = false;
                this.HasChanges = secondaryDiff.HasChanges;
            }
            else if (secondaryDiff == null)
            {
                this.HasConflictingChanges = false;
                this.HasChanges = priorityDiff.HasChanges;
            }
            else if (priorityDiff.Status != DiffStatus.Default)
            {
                this.HasChanges = secondaryDiff.HasChanges;
                this.HasConflictingChanges = this.HasChanges;
            }
            else if (secondaryDiff.Status != DiffStatus.Default)
            {
                this.HasChanges = priorityDiff.HasChanges;
                this.HasConflictingChanges = this.HasChanges;
            }
            else
            {
                this.HasConflictingChanges = secondaryDiff.HasChanges && priorityDiff.HasChanges;
                this.HasChanges = secondaryDiff.HasChanges || priorityDiff.HasChanges;
            }
            //this.HasConflictingChanges = this.name.IsConflictingChange
            //    || this.invocationClass.IsConflictingChange
            //    || this.methodName.IsConflictingChange
            //    || this.assemblyReferenceName.IsConflictingChange
            //    || this.numberOfParameters.IsConflictingChange;
        }

        public DiffDiffEntry<string> Name
        {
            get { return this.name; }
        }

        public DiffDiffEntry<string> InvocationClass
        {
            get { return this.invocationClass; }
        }

        public DiffDiffEntry<string> MethodName
        {
            get { return this.methodName; }
        }

        public DiffDiffEntry<string> AssemblyReferenceName
        {
            get { return this.assemblyReferenceName; }
        }

        public List<DiffDiffEntry<FunctionParameter>> Parameters
        {
            get { return this.parameters; }
        }

        public DiffDiffEntry<ReturnValue> ReturnValue
        {
            get { return this.returnValue; }
        }

        //public DiffDiffEntry<string> ParameterSignature
        //{
        //    get { return this.parameterSignature; }
        //}

        protected override ObjectConflictInfo GetDisplayCore(DiffVersion diffVersion, FunctionDiff diff)
        {
            ObjectConflictInfo info = new ObjectConflictInfo(new ItemCompareEntry(
                diff.RevisedItem.Name,
                this.GetCorrectState(this.name.HasChanges)),
                ConflictObjectType.Function);

            info.Attributes.Add(new ItemCompareEntry(String.Format(
                CultureInfo.CurrentCulture,
                Localization.UIResources.FunctionDetailsAssemblyReferenceNameFormat,
                diff.RevisedItem.AssemblyReferenceName),
                this.GetCorrectState(this.AssemblyReferenceName.HasChanges)));

            info.Attributes.Add(new ItemCompareEntry(String.Format(
                CultureInfo.CurrentCulture,
                Localization.UIResources.FunctionDetailsClassFormat,
                diff.RevisedItem.InvocationClass),
                this.GetCorrectState(this.InvocationClass.HasChanges)));

            info.Attributes.Add(new ItemCompareEntry(String.Format(
                CultureInfo.CurrentCulture,
                Localization.UIResources.FunctionDetailsMethodFormat,
                diff.RevisedItem.MethodName),
                this.GetCorrectState(this.MethodName.HasChanges)));

            int parameterCount = diff.RevisedItem.Parameters.Count;

            if (parameterCount == 1)
            {
                info.Attributes.Add(new ItemCompareEntry(String.Format(
                    CultureInfo.CurrentCulture,
                    Localization.UIResources.FunctionDetailsParameterCountSingular,
                    parameterCount),
                    EntryState.Normal));
            }
            else
            {
                info.Attributes.Add(new ItemCompareEntry(String.Format(
                    CultureInfo.CurrentCulture,
                    Localization.UIResources.FunctionDetailsParameterCountPlural,
                    parameterCount),
                    EntryState.Normal));
            }

            //if (diff.Parameters.Count > 0)
            //{
            //    bool parameterInfoAdded = false;

            //    for (int i = 0; i < diff.Parameters.Count; i++)
            //    {
            //        DiffEntry<FunctionParameter> parameterEntry = diff.Parameters[i];
            //        if (parameterEntry.RevisedValue != null)
            //        {
            //            if (!parameterInfoAdded)
            //            {
            //                info.Attributes.Add(new ItemCompareEntry(
            //                    Localization.UIResources.FunctionParameterLabelText,
            //                    this.GetCorrectState(this.parametersHaveChanges)));
            //                parameterInfoAdded = true;
            //            }

            //            // I18N
            //            info.Attributes.Add(new ItemCompareEntry(String.Format(
            //                "[{1}] {0}",
            //                parameterEntry.RevisedValue.ToString(),
            //                i+1),
            //                this.GetCorrectState(this.parametersHaveChanges)));
            //        }
            //    }
            //}

            return info;
        }

        protected override ConflictObjectType GetConflictObjectType()
        {
            return ConflictObjectType.Function;
        }

        //protected override VisualDisplayInfo GetInfoForPriorityCore()
        //{
        //    FunctionDiff diff = this.GetDiff(DiffVersion.Priority);
        //    if (diff != null)
        //    {
        //        return new VisualDisplayInfo(this.Name.PriorityDiffEntry.RevisedValue,
        //            this.ConcatenateDetails(DiffVersion.Priority));
        //    }
        //    else
        //    {
        //        return new VisualDisplayInfo(string.Empty, Localization.UIResources.NotPresent);
        //    }
        //}

        //protected override bool GetConflictingIdentifiersCouldExistCore(DiffVersion version)
        //{
        //    if (version == DiffVersion.Priority)
        //    {
        //        switch (this.PriorityDiff.Status)
        //        {
        //            case DiffStatus.New:
        //                return true;
        //            case DiffStatus.Deleted:
        //                return false;
        //            default:
        //                return this.PriorityDiff.Name.HasChanged || this.PriorityDiff.;
        //        }
        //    }
        //    else
        //    {
        //        return this.SecondaryDiff.Name.HasChanged;
        //    }
        //}

        //protected override VisualDisplayInfo GetInfoForSecondaryCore()
        //{
        //    FunctionDiff diff = this.GetDiff(DiffVersion.Priority);
        //    if (diff != null)
        //    {
        //        return new VisualDisplayInfo(this.Name.PriorityDiffEntry.RevisedValue,
        //            this.ConcatenateDetails(DiffVersion.Priority));
        //    }
        //    else
        //    {
        //        return new VisualDisplayInfo(string.Empty, Localization.UIResources.NotPresent);
        //    }
        //}

        protected override string GetObjectTypeNameCore()
        {
            return Localization.UIResources.FunctionTypeName;
        }
    }
}
