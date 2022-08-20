using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using ZachJohnson.Promptu.UI;
using ZachJohnson.Promptu.UIModel;
using System.Globalization;

namespace ZachJohnson.Promptu.UserModel.Differencing
{
    internal class CommandDiffDiff : DiffDiff<CommandDiff, Command, CommandDiffDiff>
    {
        private DiffDiffEntry<string> name;
        private DiffDiffEntry<string> executionPath;
        private DiffDiffEntry<string> arguments;
        private DiffDiffEntry<bool> runAsAdministrator;
        private DiffDiffEntry<bool> showParameterHistory;
        private DiffDiffEntry<string> notes;
        private DiffDiffEntry<ProcessWindowStyle> startingWindowState;
        private DiffDiffEntry<string> startupDirectory;
        private List<DiffDiffEntry<CommandParameterMetaInfo>> parametersMetaInfo = new List<DiffDiffEntry<CommandParameterMetaInfo>>();
        //private bool parametersMetaInfoHaveChanged;

        public CommandDiffDiff(
            CommandDiff priorityDiff, 
            CommandDiff secondaryDiff)
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

            this.executionPath = new DiffDiffEntry<string>(
                priorityDiff == null ? null : priorityDiff.ExecutionPath,
                secondaryDiff == null ? null : secondaryDiff.ExecutionPath);

            this.arguments = new DiffDiffEntry<string>(
                priorityDiff == null ? null : priorityDiff.Arguments,
                secondaryDiff == null ? null : secondaryDiff.Arguments);

            this.runAsAdministrator = new DiffDiffEntry<bool>(
                priorityDiff == null ? null : priorityDiff.RunAsAdministrator,
                secondaryDiff == null ? null : secondaryDiff.RunAsAdministrator);

            this.showParameterHistory = new DiffDiffEntry<bool>(
                priorityDiff == null ? null : priorityDiff.ShowParameterHistory,
                secondaryDiff == null ? null : secondaryDiff.ShowParameterHistory);

            this.notes = new DiffDiffEntry<string>(
                priorityDiff == null ? null : priorityDiff.Notes,
                secondaryDiff == null ? null : secondaryDiff.Notes);

            this.startingWindowState = new DiffDiffEntry<ProcessWindowStyle>(
                priorityDiff == null ? null : priorityDiff.StartingWindowState,
                secondaryDiff == null ? null : secondaryDiff.StartingWindowState);

            this.startupDirectory = new DiffDiffEntry<string>(
                priorityDiff == null ? null : priorityDiff.StartupDirectory,
                secondaryDiff == null ? null : secondaryDiff.StartupDirectory);

            int largestParameterCount = 0;

            if (priorityDiff != null)
            {
                largestParameterCount = priorityDiff.ParametersMetaInfo.Count;
            }

            if (secondaryDiff != null && secondaryDiff.ParametersMetaInfo.Count > largestParameterCount)
            {
                largestParameterCount = secondaryDiff.ParametersMetaInfo.Count;
            }

            for (int i = 0; i < largestParameterCount; i++)
            {
                DiffEntry<CommandParameterMetaInfo> priorityParameterInfo = null;
                DiffEntry<CommandParameterMetaInfo> secondaryParameterInfo = null;

                if (priorityDiff != null && i < priorityDiff.ParametersMetaInfo.Count)
                {
                    priorityParameterInfo = priorityDiff.ParametersMetaInfo[i];
                }

                if (priorityDiff != null && i < secondaryDiff.ParametersMetaInfo.Count)
                {
                    secondaryParameterInfo = secondaryDiff.ParametersMetaInfo[i];
                }

                DiffDiffEntry<CommandParameterMetaInfo> diffDiffEntry = new DiffDiffEntry<CommandParameterMetaInfo>(priorityParameterInfo, secondaryParameterInfo);

                this.parametersMetaInfo.Add(diffDiffEntry);

                //if (diffDiffEntry.HasChanges)
                //{
                //    this.parametersMetaInfoHaveChanged = true;
                //}
            }

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
            //        || this.executionPath.IsConflictingChange
            //        || this.arguments.IsConflictingChange
            //        || this.runAsAdministrator.IsConflictingChange
            //        || this.saveParameterHistory.IsConflictingChange
            //        || this.startingWindowState.IsConflictingChange
            //        || this.startupDirectory.IsConflictingChange;
        }

        public DiffDiffEntry<string> Name
        {
            get { return this.name; }
        }

        public DiffDiffEntry<string> ExecutionPath
        {
            get { return this.executionPath; }
        }

        public DiffDiffEntry<string> Arguments
        {
            get { return this.arguments; }
        }

        public DiffDiffEntry<bool> RunAsAdministrator
        {
            get { return this.runAsAdministrator; }
        }

        public DiffDiffEntry<bool> ShowParameterHistory
        {
            get { return this.showParameterHistory; }
        }

        public DiffDiffEntry<string> Notes
        {
            get { return this.notes; }
        }

        public List<DiffDiffEntry<CommandParameterMetaInfo>> ParametersMetaInfo
        {
            get { return this.parametersMetaInfo; }
        }

        public DiffDiffEntry<ProcessWindowStyle> StartingWindowState
        {
            get { return this.startingWindowState; }
        }

        public DiffDiffEntry<string> StartupDirectory
        {
            get { return this.startupDirectory; }
        }

        protected override ObjectConflictInfo GetDisplayCore(DiffVersion diffVersion, CommandDiff diff)
        {
            ObjectConflictInfo info = new ObjectConflictInfo(new ItemCompareEntry(
                diff.RevisedItem.Name,
                this.GetCorrectState(this.name.HasChanges)),
                ConflictObjectType.Command);

            info.Attributes.Add(new ItemCompareEntry(String.Format(
                CultureInfo.CurrentCulture,
                Localization.UIResources.CommandDetailsExecutionPathFormat,
                diff.RevisedItem.ExecutionPath),
                this.GetCorrectState(this.ExecutionPath.HasChanges)));

            string arguments = diff.RevisedItem.Arguments;
            if (arguments.Length > 0)
            {
                info.Attributes.Add(new ItemCompareEntry(String.Format(
                    CultureInfo.CurrentCulture,
                    Localization.UIResources.CommandDetailsArgumentsFormat,
                    arguments),
                this.GetCorrectState(this.Arguments.HasChanges)));
            }

            if (diff.RevisedItem.RunAsAdministrator)
            {
                info.Attributes.Add(new ItemCompareEntry(String.Format(
                    CultureInfo.CurrentCulture,
                    Localization.UIResources.CommandDetailsRunAsAdministratorFormat,
                    diff.RevisedItem.RunAsAdministrator),
                    this.GetCorrectState(this.RunAsAdministrator.HasChanges)));
            }

            //info.Attributes.Add(new ItemCompareEntry(String.Format(
            //    Localization.UIResources.CommandDetailsShowParameterHistoryFormat,
            //    diff.RevisedItem.ShowParameterHistory),
            //    this.GetCorrectState(this.ShowParameterHistory.HasChanges)));

            //ProcessWindowStyle startingWindowState = diff.RevisedItem.StartingWindowState;

            //if (startingWindowState != ProcessWindowStyle.Normal)
            //{
            //    info.Attributes.Add(new ItemCompareEntry(String.Format(
            //    Localization.UIResources.CommandDetailsStartingWindowStateFormat,
            //    startingWindowState),
            //    this.GetCorrectState(this.StartingWindowState.HasChanges)));
            //}

            string startupDirectory = diff.RevisedItem.StartupDirectory;
            if (!String.IsNullOrEmpty(startupDirectory))
            {
                info.Attributes.Add(new ItemCompareEntry(String.Format(
                    CultureInfo.CurrentCulture,
                    Localization.UIResources.CommandDetailsStartupDirectoryFormat,
                    startupDirectory),
                    this.GetCorrectState(this.StartupDirectory.HasChanges)));
            }

            //if (diff.ParametersMetaInfo.Count > 0)
            //{
            //    info.Attributes.Add(new ItemCompareEntry(String.Format(
            //    Localization.UIResources.CommandDetailsStartupDirectoryFormat,
            //    startupDirectory),
            //    EntryState.Normal));
            //}

            //if (diff.ParametersMetaInfo.Count > 0)
            //{
            //    bool parameterInfoAdded = false;

            //    for (int i = 0; i < diff.ParametersMetaInfo.Count; i++)
            //    {
            //        DiffEntry<CommandParameterMetaInfo> parameterInfoEntry = diff.ParametersMetaInfo[i];
            //        if (parameterInfoEntry.RevisedValue != null)
            //        {
            //            if (!parameterInfoAdded)
            //            {
            //                info.Attributes.Add(new ItemCompareEntry(
            //                    Localization.UIResources.CommandParameterMetaInfoLabelText,
            //                    this.GetCorrectState(this.parametersMetaInfoHaveChanged)));
            //                parameterInfoAdded = true;
            //            }

            //            info.Attributes.Add(new ItemCompareEntry(
            //                parameterInfoEntry.RevisedValue.ToString(),
            //                this.GetCorrectState(this.parametersMetaInfoHaveChanged)));
            //        }
            //    }
            //}

            return info;
        }

        protected override ConflictObjectType GetConflictObjectType()
        {
            return ConflictObjectType.Command;
        }

        //protected override VisualDisplayInfo GetInfoForPriorityCore()
        //{
        //    CommandDiff diff = this.GetDiff(DiffVersion.Priority);
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

        //protected override VisualDisplayInfo GetInfoForSecondaryCore()
        //{
        //    CommandDiff diff = this.GetDiff(DiffVersion.Priority);
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
            return Localization.UIResources.CommandTypeName;
        }
    }
}
