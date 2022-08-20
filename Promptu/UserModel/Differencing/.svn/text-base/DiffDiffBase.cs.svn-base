using System;
using System.Collections.Generic;
using System.Text;
using ZachJohnson.Promptu.UIModel;
using System.Globalization;

namespace ZachJohnson.Promptu.UserModel.Differencing
{
    internal abstract class DiffDiffBase
    {
        private bool hasConflictingChanges;
        private bool hasChanges;

        public DiffDiffBase()
        {
        }

        public bool HasConflictingChanges
        {
            get { return this.hasConflictingChanges; }
            protected set { this.hasConflictingChanges = value; }
        }

        public bool ImplicityResolved
        {
            get { return this.GetImplicitlyResolved(); }
        }

        public int PriorityDiffIdentifierConflictsCount
        {
            get { return this.GetPriorityDiffIdentifierConflictsCount(); }
        }

        public int SecondaryDiffIdentifierConflictsCount
        {
            get { return this.GetSecondaryDiffIdentifierConflictsCount(); }
        }

        public bool HasChanges
        {
            get { return this.hasChanges; }
            protected set { this.hasChanges = value; }
        }

        public ObjectConflictInfo GetInfoForPriority()
        {
            return this.GetInfoForPriorityCore();
        }

        public ObjectConflictInfo GetInfoForSecondary()
        {
            return this.GetInfoForSecondaryCore();
        }

        public VisualDisplayInfo GetInfoForPriorityIdentifierConflicts()
        {
            return this.GetInfoForPriorityIdentifierConflictsCore();   
        }

        public VisualDisplayInfo GetInfoForSecondaryIdentifierConflicts()
        {
            return this.GetInfoForSecondaryIdentifierConflictsCore();
        }

        public void ResolveTo(DiffVersion version)
        {
            this.ResolveToCore(version);
        }

        public string ObjectTypeName
        {
            get { return this.GetObjectTypeNameCore(); }
        }

        protected abstract ObjectConflictInfo GetInfoForPriorityCore();

        protected abstract ObjectConflictInfo GetInfoForSecondaryCore();

        protected abstract VisualDisplayInfo GetInfoForPriorityIdentifierConflictsCore();

        protected abstract VisualDisplayInfo GetInfoForSecondaryIdentifierConflictsCore();

        protected abstract string GetObjectTypeNameCore();

        protected abstract void ResolveToCore(DiffVersion version);

        protected abstract bool GetImplicitlyResolved();

        protected abstract int GetPriorityDiffIdentifierConflictsCount();

        protected abstract int GetSecondaryDiffIdentifierConflictsCount();

        public override string ToString()
        {
            return String.Format(CultureInfo.InvariantCulture, "Conflicting Change: {0}", this.HasConflictingChanges);
        }
    }
}
