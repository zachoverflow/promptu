using System;
using System.Collections.Generic;
using System.Text;

namespace ZachJohnson.Promptu.UserModel.Differencing
{
    internal abstract class DiffDiffEntryBase
    {
        private bool hasConflictingChanges;
        private bool hasChanges;

        public DiffDiffEntryBase()
        {
        }

        public bool HasConflictingChanges
        {
            get { return this.hasConflictingChanges; }
            protected set { this.hasConflictingChanges = value; }
        }

        public bool HasChanges
        {
            get { return this.hasChanges; }
            protected set { this.hasChanges = value; }
        }
    }
}
