using System;
using System.Collections.Generic;
using System.Text;
using ZachJohnson.Promptu.UserModel;
using System.Windows.Forms;
using System.Diagnostics;

namespace ZachJohnson.Promptu.UserModel.Differencing
{
    internal class CommandDiff : Diff<Command, CommandDiff>
    {
        private static Comparison<ProcessWindowStyle> processWindowStyleComparison = new Comparison<ProcessWindowStyle>(CompareProcessWindowStyle);
        private DiffEntry<string> name;
        private DiffEntry<string> executionPath;
        private DiffEntry<string> arguments;
        private DiffEntry<bool> runAsAdministrator;
        private DiffEntry<bool> showParameterHistory;
        private DiffEntry<string> notes;
        private DiffEntry<ProcessWindowStyle> startingWindowState;
        private DiffEntry<string> startupDirectory;
        private List<DiffEntry<CommandParameterMetaInfo>> parametersMetaInfo = new List<DiffEntry<CommandParameterMetaInfo>>();
        private bool parametersMetaInfoHasChanged;

        public CommandDiff(Command baseItem, Command revisedItem)
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
                this.executionPath = new DiffEntry<string>(baseItem.ExecutionPath, revisedItem.ExecutionPath, ValueComparisons.StringComparison);
                this.arguments = new DiffEntry<string>(baseItem.Arguments, revisedItem.Arguments, ValueComparisons.StringComparison);
                this.runAsAdministrator = new DiffEntry<bool>(baseItem.RunAsAdministrator, revisedItem.RunAsAdministrator, ValueComparisons.BooleanComparison);
                this.showParameterHistory = new DiffEntry<bool>(baseItem.ShowParameterHistory, revisedItem.ShowParameterHistory, ValueComparisons.BooleanComparison);
                this.notes = new DiffEntry<string>(baseItem.Notes, revisedItem.Notes, ValueComparisons.StringComparison);
                this.startingWindowState = new DiffEntry<ProcessWindowStyle>(baseItem.StartingWindowState, revisedItem.StartingWindowState, processWindowStyleComparison);
                this.startupDirectory = new DiffEntry<string>(baseItem.StartupDirectory, revisedItem.StartupDirectory, ValueComparisons.StringComparison);

                int largestParameterCount = baseItem.ParametersMetaInfo.Count;

                if (revisedItem.ParametersMetaInfo.Count > largestParameterCount)
                {
                    largestParameterCount = revisedItem.ParametersMetaInfo.Count;
                }

                for (int i = 0; i < largestParameterCount; i++)
                {
                    CommandParameterMetaInfo baseParameterInfo = null;
                    CommandParameterMetaInfo revisedParameterInfo = null;

                    if (i < baseItem.ParametersMetaInfo.Count)
                    {
                        baseParameterInfo = baseItem.ParametersMetaInfo[i];
                    }

                    if (i < revisedItem.ParametersMetaInfo.Count)
                    {
                        revisedParameterInfo = revisedItem.ParametersMetaInfo[i];
                    }

                    DiffEntry<CommandParameterMetaInfo> diffEntry = new DiffEntry<CommandParameterMetaInfo>(baseParameterInfo, revisedParameterInfo, ValueComparisons.CommandParameterMetaInfoComparison);

                    this.parametersMetaInfo.Add(diffEntry);

                    if (diffEntry.HasChanged)
                    {
                        this.parametersMetaInfoHasChanged = true;
                    }
                }

                this.HasChanges = this.name.HasChanged
                        || this.executionPath.HasChanged
                        || this.arguments.HasChanged
                        || this.runAsAdministrator.HasChanged
                        || this.showParameterHistory.HasChanged
                        || this.startingWindowState.HasChanged
                        || this.startupDirectory.HasChanged
                        || this.parametersMetaInfoHasChanged;
            }
        }

        //public CommandDiff(CommandDiff baseDiff, CommandDiff latestDiff)
        //    : this(baseDiff.AsCommand(), latestDiff.AsCommand())
        //{
        //}

        public List<DiffEntry<CommandParameterMetaInfo>> ParametersMetaInfo
        {
            get { return this.parametersMetaInfo; }
        }

        public DiffEntry<string> Name
        {
            get { return this.name; }
        }

        public DiffEntry<string> ExecutionPath
        {
            get { return this.executionPath; }
        }

        public DiffEntry<string> Arguments
        {
            get { return this.arguments; }
        }

        public DiffEntry<bool> RunAsAdministrator
        {
            get { return this.runAsAdministrator; }
        }

        public DiffEntry<bool> ShowParameterHistory
        {
            get { return this.showParameterHistory; }
        }

        public DiffEntry<string> Notes
        {
            get { return this.notes; }
        }

        public DiffEntry<ProcessWindowStyle> StartingWindowState
        {
            get { return this.startingWindowState; }
        }

        public DiffEntry<string> StartupDirectory
        {
            get { return this.startupDirectory; }
        }

        //public Command AsCommand()
        //{
        //    return new Command(
        //        this.name.LatestValue,
        //        this.executionPath.LatestValue,
        //        this.arguments.LatestValue,
        //        this.runAsAdministrator.LatestValue,
        //        this.startingWindowState.LatestValue,
        //        this.notes.LatestValue,
        //        this.startupDirectory.LatestValue,
        //        this.saveParameterHistory.LatestValue);
        //}

        private static int CompareProcessWindowStyle(ProcessWindowStyle x, ProcessWindowStyle y)
        {
            return x.CompareTo(y);
        }

        protected override bool ConflictsWithCore(CommandDiff diff)
        {
            if (this.RevisedItem != null && diff.RevisedItem != null)
            {
                string[] thisNamesAndAliases = this.RevisedItem.GetAllPossibleNames();
                string[] otherNamesAndAliases = diff.RevisedItem.GetAllPossibleNames();

                foreach (string thisNameOrAlias in thisNamesAndAliases)
                {
                    foreach (string otherNameOrAlias in otherNamesAndAliases)
                    {
                        if (thisNameOrAlias.ToUpperInvariant() == otherNameOrAlias.ToUpperInvariant())
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        protected override bool GetIdentifierHasChangedCore()
        {
            return this.Name.HasChanged;
        }

        //protected override bool IsSimilarToCore(CommandDiff diff)
        //{
        //    Id thisId;
        //    Id diffId;

        //    if (this.Status == DiffStatus.Deleted)
        //    {
        //        thisId = this.BaseItem.Id;
        //    }
        //    else
        //    {
        //        thisId = this.RevisedItem.Id;
        //    }

        //    if (diff.Status == DiffStatus.Deleted)
        //    {
        //        diffId = diff.BaseItem.Id;
        //    }
        //    else
        //    {
        //        diffId = diff.RevisedItem.Id;
        //    }

        //    return thisId == diffId;
        //}
    }
}
