using System;
using System.Collections.Generic;
using System.Text;
using ZachJohnson.Promptu.UserModel;
using System.Windows.Forms;
using System.Drawing;
using System.IO;
using System.Drawing.Extensions;
using ZachJohnson.Promptu.Collections;
using ZachJohnson.Promptu.UserModel.Differencing;
using ZachJohnson.Promptu.UserModel.Collections;
using ZachJohnson.Promptu.UIModel.Presenters;
using System.Globalization;

namespace ZachJohnson.Promptu.UIModel
{
    internal static class ListDataManager
    {
        public static bool GetIfCanPaste(ItemCode itemCode)
        {
            object clipboardData = InternalGlobals.GuiManager.ToolkitHost.Clipboard.GetData("Promptu-Cut/Copy");
            if (clipboardData != null && clipboardData.ToString() == "0")
            {
                ClipboardCopyData data = ClipboardCopyData.LastSetData;
                if (data != null && data.Info.ItemCode == itemCode)
                {
                    return true;
                }
            }

            return false;
        }

        public static List<Id> DownloadClipboardData(ItemCode itemCode)
        {
            List<Id> idsToSelect;
            object clipboardData = InternalGlobals.GuiManager.ToolkitHost.Clipboard.GetData("Promptu-Cut/Copy");
            if (clipboardData != null && clipboardData.ToString() == "0")
            {
                ClipboardCopyData data = ClipboardCopyData.LastSetData;
                if (data != null && data.Info.ItemCode == itemCode)
                {
                    Cursor.Current = Cursors.WaitCursor;
                    List listFrom = InternalGlobals.CurrentProfile.Lists.TryGet(data.ListId);
                    idsToSelect = ListDataManager.PasteData(InternalGlobals.CurrentProfile.SelectedList, listFrom, data.Info, false, data.CutNotCopy);

                    Skins.PromptHandler.GetInstance().SetupDialog.UpdateToCurrentList();
                    Cursor.Current = Cursors.Default;
                    return idsToSelect;
                }
            }

            return new List<Id>();
        }

        private static string GetCommandRenameName(string name, TrieDictionary<string> flattenedNames)
        {
            string[] aliases = Command.GetAliasesFromName(name);

            StringBuilder renameName = new StringBuilder();

            bool doneFirst = false;
            foreach (string alias in aliases)
            {
                if (!doneFirst)
                {
                    doneFirst = true;
                }
                else
                {
                    renameName.Append(",");
                }

                renameName.Append(GeneralUtilities.GetAvailableIncrementingNameOptimized(flattenedNames, alias + "{+}", "{number}", false, InsertBase.Two));
            }

            return renameName.ToString();
        }

        public static string GetGenericRenameName(string name, ITrie names)
        {
            return GeneralUtilities.GetAvailableIncrementingNameOptimized(names, name + "{+}", "{number}", false, InsertBase.Two);
        }

        private static void RemoveCommandName(TrieDictionary<string> flattenedNames, string name)
        {
            foreach (string alias in Command.GetAliasesFromName(name))
            {
                flattenedNames.Remove(alias);
            }
        }

        private static void AddCommandName(TrieDictionary<string> flattenedNames, string name)
        {
            foreach (string alias in Command.GetAliasesFromName(name))
            {
                flattenedNames.Add(alias, name);
            }
        }

        private static string GetFunctionRenameName(Function function, TrieList fullStringIds)
        {
            string availableSignature = GeneralUtilities.GetAvailableIncrementingNameOptimized(
                fullStringIds, 
                "[" + function.Name + "{+}]" + function.ParameterSignature, 
                "{number}", 
                false, 
                InsertBase.Two);

            return Function.ExtractNameFromStringId(availableSignature, function.ParameterSignature);
        }

        public static List<Id> PasteData(List listTo, List listFrom, ItemCopyInfo info, bool moveNotCopy, bool dragging)
        {
            List<Id> idsToSelect = new List<Id>(); 
            //List<string> namesToSelect = new List<string>();
            MoveConflictAction action = MoveConflictAction.Skip;
            bool doActionForRemaining = false;

            bool intoSameList = listFrom == listTo;

            try
            {
                listTo.PublishOnChildrenSaved = false;
                if (listFrom != null)
                {
                    listFrom.PublishOnChildrenSaved = false;
                }

                InternalGlobals.SyncSynchronizer.CancelSyncsAndWait();
                switch (info.ItemCode)
                {
                    case ItemCode.Command:
                        {
                            List<Command> commands = info.Item as List<Command>;

                            //listTo.Commands.BlockSave = true;

                            if (commands != null)
                            {
                                TrieDictionary<string> flattenedCommandNames = listTo.Commands.GetFlattenedNames();

                                int index = -1;
                                foreach (Command command in commands)
                                {
                                    index++;
                                    bool goAhead = true;
                                    TrieList conflicts = CommandCollection.GetConflicts(command.Name, flattenedCommandNames);//listTo.Commands.GetConflicts(command.Name, CaseSensitivity.Insensitive, GetConflictsMode.ReturnOnlyAliases, null);

                                    string renameName = null;
                                    if (conflicts.Count > 0)
                                    {
                                        renameName = GetCommandRenameName(command.Name, flattenedCommandNames);
                                        if (intoSameList)
                                        {
                                            action = MoveConflictAction.Rename;
                                        }
                                        else if (!doActionForRemaining)
                                        {
                                            List<ItemInfo> conflictInfos = new List<ItemInfo>();

                                            foreach (string conflictName in conflicts)
                                            {
                                                Command conflictingCommand = listTo.Commands.TryGetItemNameExact(conflictName);
                                                if (conflictingCommand != null)
                                                {
                                                    conflictInfos.Add(conflictingCommand.GetItemInfo());
                                                }
                                            }

                                            action = GetUserConflictDecision(
                                                ConflictObjectType.Command,
                                                conflictInfos,
                                                command.GetItemInfo(),
                                                renameName,
                                                moveNotCopy,
                                                index < commands.Count - 1,
                                                out doActionForRemaining);
                                        }

                                        if (action == MoveConflictAction.Replace)
                                        {
                                            renameName = null;
                                            goAhead = true;
                                            foreach (string name in conflicts)
                                            {
                                                listTo.Commands.Remove(name);

                                                RemoveCommandName(flattenedCommandNames, name);
                                            }
                                        }
                                        else if (action == MoveConflictAction.Cancel)
                                        {
                                            return idsToSelect;
                                        }
                                        else if (action == MoveConflictAction.Rename)
                                        {
                                            goAhead = true;
                                        }
                                        else
                                        {
                                            goAhead = false;
                                        }
                                    }

                                    if (goAhead)
                                    {
                                        Command clonedCommand = command.Clone(renameName);
                                        clonedCommand.Id = listTo.Commands.IdGenerator.GenerateId();

                                        listTo.Commands.Add(clonedCommand);

                                        idsToSelect.Add(clonedCommand.Id);

                                        AddCommandName(flattenedCommandNames, clonedCommand.Name);

                                        //namesToSelect.Add(renameName == null ? command.Name : renameName);

                                        if (listFrom != null)
                                        {
                                            FunctionCollection functionDependencies = command.GetFunctionDependecies(new FunctionCollectionComposite(
                                                InternalGlobals.CurrentProfile.Lists,
                                                listFrom));

                                            foreach (Function function in functionDependencies)
                                            {
                                                MoveOrCopyFunction(listTo, listFrom, null, function, false, null, null);
                                            }

                                            ValueListCollection valueListDependencies = command.GetValueListDependencies(listFrom.ValueLists);

                                            foreach (ValueList valueList in valueListDependencies)
                                            {
                                                MoveOrCopyValueList(listTo, listFrom, null, valueList, false, null, null);
                                            }
                                        }

                                        listTo.Commands.Save();

                                        if (dragging && moveNotCopy && listFrom != null)
                                        {
                                            listFrom.Commands.Remove(command);
                                            listFrom.Commands.Save();
                                        }
                                    }
                                }
                            }

                            //listTo.Commands.BlockSave = false;
                            //listTo.Commands.Save();

                            //if (listFrom != null)
                            //{
                            //    listFrom.Commands.BlockSave = false;
                            //    listFrom.Commands.Save();
                            //}
                        }

                        break;
                    case ItemCode.ValueList:
                        {
                            List<ValueList> valueLists = info.Item as List<ValueList>;

                            if (valueLists != null)
                            {
                                TrieList names = listTo.ValueLists.GetNames();

                                int index = -1;
                                foreach (ValueList valueList in valueLists)
                                {
                                    index++;
                                    bool goAhead = true;
                                    bool confict = names.Contains(valueList.Name, CaseSensitivity.Insensitive);//listTo.ValueLists.TryGet(valueList.Name);
                                    string renameName = null;

                                    if (confict)
                                    {
                                        string conflictName = names.GetCorrectCasing(valueList.Name);
                                        renameName = GetGenericRenameName(valueList.Name, names);

                                        if (intoSameList)
                                        {
                                            action = MoveConflictAction.Rename;
                                        }
                                        else if (!doActionForRemaining)
                                        {
                                            ValueList conflict = listTo.ValueLists[conflictName];
                                            action = GetUserConflictDecision(
                                                ConflictObjectType.ValueList,
                                                conflict.GetItemInfo(),
                                                valueList.GetItemInfo(),
                                                renameName,
                                                moveNotCopy,
                                                index < valueLists.Count - 1,
                                                out doActionForRemaining);
                                        }

                                        if (action == MoveConflictAction.Replace)
                                        {
                                            renameName = null;
                                            goAhead = true;
                                            listTo.ValueLists.Remove(conflictName);
                                            names.Remove(conflictName);
                                        }
                                        else if (action == MoveConflictAction.Cancel)
                                        {
                                            return idsToSelect;
                                        }
                                        else if (action == MoveConflictAction.Rename)
                                        {
                                            goAhead = true;
                                        }
                                        else
                                        {
                                            goAhead = false;
                                        }
                                    }

                                    if (goAhead)
                                    {
                                        //names.Add(renameName == null ? valueList.Name : renameName);
                                        MoveOrCopyValueList(listTo, listFrom, renameName, valueList, moveNotCopy, names, idsToSelect);
                                    }
                                }
                            }
                        }

                        break;
                    case ItemCode.Function:
                        {
                            List<Function> functions = info.Item as List<Function>;

                            if (functions != null)
                            {
                                TrieList stringIds = listTo.Functions.GetStringIds();

                                int index = -1;
                                foreach (Function function in functions)
                                {
                                    index++;
                                    bool goAhead = true;
                                    string fullStringIdForFunction = function.GetStringId();
                                    bool confict = stringIds.Contains(fullStringIdForFunction, CaseSensitivity.Insensitive);//listTo.ValueLists.TryGet(valueList.Name);
                                    string renameName = null;

                                    if (confict)
                                    {
                                        string conflictName = Function.ExtractNameFromStringId(stringIds.GetCorrectCasing(fullStringIdForFunction), function.ParameterSignature);
                                        renameName = GetFunctionRenameName(function, stringIds);//GetGenericRenameName(function.Name, names);

                                        if (intoSameList)
                                        {
                                            action = MoveConflictAction.Rename;
                                        }
                                        else if (!doActionForRemaining)
                                        {
                                            Function conflictingFuction = listTo.Functions[conflictName, function.ParameterSignature];
                                            action = GetUserConflictDecision(
                                                ConflictObjectType.Function,
                                                conflictingFuction.GetItemInfo(),
                                                function.GetItemInfo(),
                                                renameName,
                                                moveNotCopy,
                                                index < functions.Count - 1,
                                                out doActionForRemaining);
                                        }

                                        if (action == MoveConflictAction.Replace)
                                        {
                                            renameName = null;
                                            goAhead = true;
                                            listTo.Functions.Remove(conflictName, function.ParameterSignature);
                                            stringIds.Remove(Function.CreateStringId(conflictName, function.ParameterSignature));
                                        }
                                        else if (action == MoveConflictAction.Cancel)
                                        {
                                            return idsToSelect;
                                        }
                                        else if (action == MoveConflictAction.Rename)
                                        {
                                            goAhead = true;
                                        }
                                        else
                                        {
                                            goAhead = false;
                                        }
                                    }

                                    //bool goAhead = true;
                                    //if (listTo.Functions.Contains(function.Name, function.ParameterSignature))
                                    //{
                                    //    DialogResult dialogResult = DialogResult.No; //GetUserConflictDecision(function.Name, function.Name, listTo.Name);
                                    //    if (dialogResult == DialogResult.Yes)
                                    //    {
                                    //        goAhead = true;
                                    //        listTo.Functions.Remove(function.Name, function.ParameterSignature);
                                    //    }
                                    //    else if (dialogResult == DialogResult.Cancel)
                                    //    {
                                    //        return idsToSelect;
                                    //    }
                                    //    else
                                    //    {
                                    //        goAhead = false;
                                    //    }
                                    //}

                                    if (goAhead)
                                    {
                                        MoveOrCopyFunction(listTo, listFrom, renameName, function, moveNotCopy, stringIds, idsToSelect);
                                    }
                                }
                            }
                        }

                        break;
                    case ItemCode.AssemblyReference:
                        {
                            List<AssemblyReference> assemblyReferences = info.Item as List<AssemblyReference>;

                            TrieList names = listTo.AssemblyReferences.GetNames();

                            if (assemblyReferences != null)
                            {
                                int index = -1;
                                foreach (AssemblyReference assemblyReference in assemblyReferences)
                                {
                                    index++;
                                    bool goAhead = true;
                                    bool confict = names.Contains(assemblyReference.Name, CaseSensitivity.Insensitive);//listTo.ValueLists.TryGet(valueList.Name);
                                    string renameName = null;

                                    if (confict)
                                    {
                                        string conflictName = names.GetCorrectCasing(assemblyReference.Name);
                                        renameName = GetGenericRenameName(assemblyReference.Name, names);

                                        if (intoSameList)
                                        {
                                            action = MoveConflictAction.Rename;
                                        }
                                        else if (!doActionForRemaining)
                                        {
                                            AssemblyReference conflictingReference = listTo.AssemblyReferences[conflictName];
                                            action = GetUserConflictDecision(
                                                ConflictObjectType.AssemblyReference,
                                                conflictingReference.GetItemInfo(),
                                                assemblyReference.GetItemInfo(),
                                                renameName,
                                                moveNotCopy,
                                                index < assemblyReferences.Count - 1,
                                                out doActionForRemaining);
                                        }

                                        if (action == MoveConflictAction.Replace)
                                        {
                                            renameName = null;
                                            goAhead = true;
                                            listTo.AssemblyReferences.Remove(conflictName);
                                            names.Remove(conflictName);
                                        }
                                        else if (action == MoveConflictAction.Cancel)
                                        {
                                            return idsToSelect;
                                        }
                                        else if (action == MoveConflictAction.Rename)
                                        {
                                            goAhead = true;
                                        }
                                        else
                                        {
                                            goAhead = false;
                                        }
                                    }

                                    //if (listTo.AssemblyReferences.Contains(assemblyReference.Name))
                                    //{
                                    //    DialogResult dialogResult = DialogResult.No; //GetUserConflictDecision(assemblyReference.Name, assemblyReference.Name, listTo.Name);
                                    //    if (dialogResult == DialogResult.Yes)
                                    //    {
                                    //        goAhead = true;
                                    //        listTo.AssemblyReferences.Remove(assemblyReference.Name);
                                    //    }
                                    //    else if (dialogResult == DialogResult.Cancel)
                                    //    {
                                    //        return idsToSelect;
                                    //    }
                                    //    else
                                    //    {
                                    //        goAhead = false;
                                    //    }
                                    //}

                                    if (goAhead)
                                    {
                                        MoveOrCopyAssemblyReference(listTo, listFrom, renameName, assemblyReference, moveNotCopy, names, idsToSelect);
                                        //AssemblyReference clone = assemblyReference.Clone(renameName, assemblyReference.SyncCallback);
                                        //names.Add(clone.Name);
                                        //clone.Id = listTo.AssemblyReferences.IdGenerator.GenerateId();
                                        //idsToSelect.Add(clone.Id);
                                        //listTo.AssemblyReferences.Add(clone);
                                        //if (!assemblyReference.OwnedByUser)
                                        //{
                                        //    clone.OwnedByUser = true;
                                        //    clone.Orphaned = true;
                                        //    listTo.AssemblyReferencesManifest.Add(clone.Name);
                                        //    listTo.AssemblyReferencesManifest.Save();
                                        //}
                                        //else
                                        //{
                                        //    listTo.AssemblyReferencesManifest.Add(clone.Name);
                                        //    listTo.AssemblyReferencesManifest.Save();
                                        //}

                                        //if (moveNotCopy && listFrom != null)
                                        //{
                                        //    listFrom.AssemblyReferences.Remove(assemblyReference);
                                        //    listFrom.AssemblyReferences.Save();
                                        //}

                                        //listTo.AssemblyReferences.Save();
                                    }
                                }
                            }
                        }

                        break;
                    default:
                        break;
                }

                if (moveNotCopy)
                {
                    Skins.PromptHandler.GetInstance().SetupDialog.UpdateListSelectorUI();
                }

            }
            finally
            {
                listTo.PublishOnChildrenSaved = true;
                listTo.PublishChangesAsync(false);

                if (listFrom != null)
                {
                    listFrom.PublishOnChildrenSaved = true;
                    listFrom.PublishChangesAsync(false);
                }
            }

            //return namesToSelect;
            return idsToSelect;
        }

        private static void MoveOrCopyValueList(List listTo, List listFrom, string renameName, ValueList valueList, bool moveNotCopy, TrieList names, List<Id> idsToSelect)
        {
            if (!listTo.ValueLists.Contains(renameName == null ? valueList.Name : renameName))
            {
                ValueList clone = valueList.Clone(renameName);
                if (names != null)
                {
                    names.Add(clone.Name);
                }

                Id newId = listTo.ValueLists.IdGenerator.GenerateId();
                ((IHasId)clone).Id = newId;
                listTo.ValueLists.Add(clone);

                if (idsToSelect != null)
                {
                    idsToSelect.Add(newId);
                }

                listTo.ValueLists.Save();

                if (moveNotCopy && listFrom != null)
                {
                    listFrom.ValueLists.Remove(valueList);
                    listFrom.ValueLists.Save();
                }
            }
        }

        private static void MoveOrCopyAssemblyReference(List listTo, List listFrom, string renameName, AssemblyReference assemblyReference, bool moveNotCopy, TrieList names, List<Id> idsToSelect)
        {
            if (!listTo.AssemblyReferences.Contains(renameName == null ? assemblyReference.Name : renameName))
            {
                AssemblyReference clone = assemblyReference.Clone(renameName, assemblyReference.SyncCallback);
                if (names != null)
                {
                    names.Add(clone.Name);
                }

                clone.Id = listTo.AssemblyReferences.IdGenerator.GenerateId();
                if (idsToSelect != null)
                {
                    idsToSelect.Add(clone.Id);
                }

                listTo.AssemblyReferences.Add(clone);
                if (!assemblyReference.OwnedByUser)
                {
                    clone.OwnedByUser = true;
                    clone.Orphaned = true;
                    listTo.AssemblyReferencesManifest.Add(clone.Name);
                    listTo.AssemblyReferencesManifest.Save();
                }
                else
                {
                    listTo.AssemblyReferencesManifest.Add(clone.Name);
                    listTo.AssemblyReferencesManifest.Save();
                }

                if (moveNotCopy && listFrom != null)
                {
                    listFrom.AssemblyReferences.Remove(assemblyReference);
                    listFrom.AssemblyReferences.Save();
                }

                listTo.AssemblyReferences.Save();
            }
        }

        private static void MoveOrCopyFunction(List listTo, List listFrom, string renameName, Function function, bool moveNotCopy, TrieList stringIds, List<Id> idsToSelect)
        {
            if (!listTo.Functions.Contains(renameName == null ? function.Name : renameName, function.ParameterSignature))
            {
                Function functionClone = function.Clone(renameName);
                functionClone.Id = listTo.Functions.IdGenerator.GenerateId();
                if (idsToSelect != null)
                {
                    idsToSelect.Add(functionClone.Id);
                }

                listTo.Functions.Add(functionClone);

                if (stringIds != null)
                {
                    stringIds.Add(functionClone.GetStringId());
                }

                if (listFrom != null)
                {
                    FunctionCollection functionDependencies = function.GetFunctionDependecies(new FunctionCollectionComposite(
                        InternalGlobals.CurrentProfile.Lists,
                        listFrom));

                    foreach (Function dependency in functionDependencies)
                    {
                        MoveOrCopyFunction(listTo, listFrom, null, dependency, false, null, null);
                    }

                    ValueListCollection valueListDependencies = function.GetValueListDependencies(listFrom.ValueLists);

                    foreach (ValueList valueList in valueListDependencies)
                    {
                        MoveOrCopyValueList(listTo, listFrom, null, valueList, false, null, null);
                    }
                }

                AssemblyReferenceCollectionComposite assemblyReferences = new AssemblyReferenceCollectionComposite(
                    InternalGlobals.CurrentProfile.Lists,
                    listFrom);
                if (!listTo.AssemblyReferences.Contains(function.AssemblyReferenceName) && assemblyReferences.Contains(function.AssemblyReferenceName))
                {
                    CompositeItem<AssemblyReference, List> compositeItem = assemblyReferences[function.AssemblyReferenceName];

                    MoveOrCopyAssemblyReference(listTo, listFrom, null, compositeItem.Item, false, null, null);

                    //AssemblyReference clone = compositeItem.Item.Clone(renameName, compositeItem.Item.SyncCallback);
                    ////names.Add(clone.Name);
                    //clone.Id = listTo.AssemblyReferences.IdGenerator.GenerateId();
                    ////idsToSelect.Add(clone.Id);
                    //listTo.AssemblyReferences.Add(clone);
                    //if (!compositeItem.Item.OwnedByUser)
                    //{
                    //    clone.OwnedByUser = true;
                    //    clone.Orphaned = true;
                    //    listTo.AssemblyReferencesManifest.Add(clone.Name);
                    //    listTo.AssemblyReferencesManifest.Save();
                    //}
                    //else
                    //{
                    //    listTo.AssemblyReferencesManifest.Add(clone.Name);
                    //    listTo.AssemblyReferencesManifest.Save();
                    //}

                    //if (moveNotCopy && listFrom != null)
                    //{
                    //    listFrom.AssemblyReferences.Remove(assemblyReference);
                    //    listFrom.AssemblyReferences.Save();
                    //}

                    //listTo.AssemblyReferences.Save();


                    //AssemblyReference clone = compositeItem.Item.Clone(compositeItem.Item.SyncCallback);
                    //listTo.AssemblyReferences.Add(clone);

                    //if (!compositeItem.Item.OwnedByUser)
                    //{
                    //    clone.OwnedByUser = true;
                    //    clone.Orphaned = true;
                    //    listTo.AssemblyReferencesManifest.Add(compositeItem.Item.Name);
                    //    listTo.AssemblyReferencesManifest.Save();
                    //}
                    //else
                    //{
                    //    listTo.AssemblyReferencesManifest.Add(compositeItem.Item.Name);
                    //    listTo.AssemblyReferencesManifest.Save();
                    //}

                    //listTo.AssemblyReferences.Save();
                }

                listTo.Functions.Save();

                if (moveNotCopy && listFrom != null)
                {
                    listFrom.Functions.Remove(function);
                    listFrom.Functions.Save();
                }
            }
        }

        //public static DialogResult GetUserConflictDecision(string oldName, string nameOfNew, string renameName)
        //{
        //    return GetUserConflictDecision(String.Format(Localization.MessageFormats.ItemAlreadyExists, oldName, nameOfNew, nameOfList));
        //}

        //public static DialogResult GetUserConflictDecision(IEnumerable<string> names, string nameOfNew, string nameOfList)
        //{
        //    StringBuilder builder = new StringBuilder();
        //    bool singular = true;
        //    bool seenFirst = false;
        //    foreach (string name in names)
        //    {
        //        if (!seenFirst)
        //        {
        //            builder.Append(name);
        //            seenFirst = true;
        //        }
        //        else if (singular)
        //        {
        //            singular = false;
        //            builder.AppendLine();
        //            builder.AppendLine(name);
        //        }
        //        else
        //        {
        //            builder.AppendLine(name);
        //        }
        //    }

        //    if (!seenFirst)
        //    {
        //        throw new ArgumentException("There must be at least one name in 'names'.");
        //    }

        //    string messageFormat;
        //    if (singular)
        //    {
        //        messageFormat = Localization.MessageFormats.ItemAlreadyExists;
        //    }
        //    else
        //    {
        //        messageFormat = Localization.MessageFormats.ItemAlreadyExistsMulitpleConflicts;
        //    }

        //    return GetUserConflictDecision(String.Format(messageFormat, builder.ToString(), nameOfNew, nameOfList));
        //}

        public static MoveConflictAction GetUserConflictDecision(
            ConflictObjectType objectType,
            ItemInfo oldItem,
            ItemInfo newItem, 
            string renameName, 
            bool moveNotCopy, 
            bool couldBeMore, 
            out bool doForAll)
        {
            return GetUserConflictDecision(
                objectType,
                new ItemInfo[] { oldItem }, 
                newItem, 
                renameName, 
                moveNotCopy, 
                couldBeMore, 
                out doForAll);
        }

        //I18N
        public static MoveConflictAction GetUserConflictDecision(
            ConflictObjectType objectType, 
            IEnumerable<ItemInfo> oldItems, 
            ItemInfo newItem, 
            string renameName, 
            bool moveNotCopy, 
            bool couldBeMore, 
            out bool doForAll)
        {
            OverwriteDialogPresenter dialog = new OverwriteDialogPresenter(couldBeMore);

            List<ItemInfo> oldItemsList = new List<ItemInfo>(oldItems);

            string mainInstructions;
            string replaceSupplement;
            string skipSupplement;
            string renameLabel;
            string renameSupplement;
            string dialogText;

            ItemInfo oldItemInfo;

            if (objectType == ConflictObjectType.Command)
            {
                if (oldItemsList.Count == 1)
                {
                    mainInstructions = Localization.UIResources.OverwriteCommandMainInstructionsSingular;
                    replaceSupplement = moveNotCopy ?
                        Localization.UIResources.CommandReplaceSupplementMoveSingular :
                        Localization.UIResources.CommandReplaceSupplementCopySingular;

                    skipSupplement = Localization.UIResources.CommandSkipSupplementSingular;
                    renameLabel = moveNotCopy ?
                        Localization.UIResources.MoveKeepBothCommands :
                        Localization.UIResources.CopyKeepBothCommands;

                    oldItemInfo = oldItemsList[0];
                }
                else
                {
                    mainInstructions = Localization.UIResources.OverwriteCommandMainInstructionsPlural;
                    replaceSupplement = moveNotCopy ?
                        Localization.UIResources.CommandReplaceSupplementMovePlural :
                        Localization.UIResources.CommandReplaceSupplementCopyPlural;

                    skipSupplement = Localization.UIResources.CommandSkipSupplementPlural;
                    renameLabel = moveNotCopy ?
                        Localization.UIResources.MoveKeepAllCommands :
                        Localization.UIResources.CopyKeepAllCommands;

                    StringBuilder commands = new StringBuilder();

                    for (int i = 0; i < oldItemsList.Count; i++)
                    {
                        // I18N list
                        if (i > 0)
                        {
                            commands.Append(" ");
                        }
                        
                        commands.AppendFormat(
                            Localization.UIResources.CommandNameFormat, 
                            oldItemsList[i].Name);
                    }

                    List<string> attributes = new List<string>();
                    attributes.Add(commands.ToString());

                    oldItemInfo = new ItemInfo(
                        Localization.UIResources.MultipleCommandsOverwriteName,
                        attributes);
                }

                renameSupplement = moveNotCopy ?
                    Localization.UIResources.RenameCommandMove :
                    Localization.UIResources.RenameCommandCopy;

                dialogText = moveNotCopy ?
                    Localization.UIResources.MoveCommandTitle :
                    Localization.UIResources.CopyCommandTitle;
            }
            else if (objectType == ConflictObjectType.AssemblyReference)
            {
                if (oldItemsList.Count != 1)
                {
                    throw new ArgumentException("If assembly references now support aliases, this code needs to be fixed.");
                }

                mainInstructions = Localization.UIResources.OverwriteAssemblyReferenceMainInstructionsSingular;
                replaceSupplement = moveNotCopy ?
                        Localization.UIResources.AssemblyReferenceReplaceSupplementMove :
                        Localization.UIResources.AssemblyReferenceReplaceSupplementCopy;

                skipSupplement = Localization.UIResources.AssemblyReferenceSkipSupplement;
                renameLabel = moveNotCopy ?
                        Localization.UIResources.MoveKeepBothAssemblyReferences :
                        Localization.UIResources.CopyKeepBothAssemblyReferences;

                renameSupplement = moveNotCopy ?
                    Localization.UIResources.RenameAssemblyReferenceMove :
                    Localization.UIResources.RenameAssemblyReferenceCopy;

                dialogText = moveNotCopy ?
                    Localization.UIResources.MoveAssemblyReferenceTitle :
                    Localization.UIResources.CopyAssemblyReferenceTitle;

                oldItemInfo = oldItemsList[0];
            }
            else if (objectType == ConflictObjectType.Function)
            {
                if (oldItemsList.Count != 1)
                {
                    throw new ArgumentException("If functions now support aliases, this code needs to be fixed.");
                }

                mainInstructions = Localization.UIResources.OverwriteFunctionMainInstructionsSingular;
                replaceSupplement = moveNotCopy ?
                        Localization.UIResources.FunctionReplaceSupplementMove :
                        Localization.UIResources.FunctionReplaceSupplementCopy;

                skipSupplement = Localization.UIResources.FunctionSkipSupplement;
                renameLabel = moveNotCopy ?
                        Localization.UIResources.MoveKeepBothFunctions :
                        Localization.UIResources.CopyKeepBothFunctions;

                renameSupplement = moveNotCopy ?
                    Localization.UIResources.RenameFunctionMove :
                    Localization.UIResources.RenameFunctionCopy;

                dialogText = moveNotCopy ?
                    Localization.UIResources.MoveFunctionTitle :
                    Localization.UIResources.CopyFunctionTitle;

                oldItemInfo = oldItemsList[0];
            }
            else if (objectType == ConflictObjectType.ValueList)
            {
                if (oldItemsList.Count != 1)
                {
                    throw new ArgumentException("If value lists now support aliases, this code needs to be fixed.");
                }

                mainInstructions = Localization.UIResources.OverwriteValueListMainInstructionsSingular;
                replaceSupplement = moveNotCopy ?
                        Localization.UIResources.ValueListReplaceSupplementMove :
                        Localization.UIResources.ValueListReplaceSupplementCopy;

                skipSupplement = Localization.UIResources.ValueListSkipSupplement;
                renameLabel = moveNotCopy ?
                        Localization.UIResources.MoveKeepBothValueLists :
                        Localization.UIResources.CopyKeepBothValueLists;

                renameSupplement = moveNotCopy ?
                    Localization.UIResources.RenameValueListMove:
                    Localization.UIResources.RenameValueListCopy;

                dialogText = moveNotCopy ?
                    Localization.UIResources.MoveValueListTitle :
                    Localization.UIResources.CopyValueListTitle;

                oldItemInfo = oldItemsList[0];
            }
            else
            {
                throw new ArgumentException("'objectType' not understood.");
            }

            dialog.MainInstructions = mainInstructions;
            dialog.Text = dialogText;

            string replaceLabel = moveNotCopy ?
                Localization.UIResources.MoveAndReplace :
                Localization.UIResources.CopyAndReplace;

            string skipLabel = moveNotCopy ?
                Localization.UIResources.DontMove :
                Localization.UIResources.DontCopy;

            dialog.Replace = new OverwriteOption(
                replaceLabel,
                replaceSupplement,
                new OverwriteOptionExtraInfo(objectType, newItem, 1));

            dialog.Skip = new OverwriteOption(
                skipLabel,
                skipSupplement,
                new OverwriteOptionExtraInfo(objectType, oldItemInfo, oldItemsList.Count));

            dialog.Rename = new OverwriteOption(
                renameLabel,
                String.Format(CultureInfo.CurrentCulture, renameSupplement, renameName),
                null);
                

            //string moveOrCopy = moveNotCopy ? "Move" : "Copy";
            //string movingOrCopying = moveNotCopy ? "moving" : "copying";

            //string replaceText = ;
            //dialog.Replace = new OverwriteOption(
            //    String.Format("{0} and Replace", moveOrCopy),
            //    "All conflicting items will be removed");
            ////dialog.ReplaceButton.GraphicalEntries.Add(new GraphicalTextEntry(replaceText, PromptuFonts.CopyActionMainFont, PromptuColors.CopyActionColor), 0, 0);
            ////dialog.ReplaceButton.GraphicalEntries.Add(new GraphicalTextEntry("All conflicting items will be removed", PromptuFonts.CopyActionDetailFont, PromptuColors.CopyActionColor), 0, 1);

            ////string skipText = String.Format("Don't {0}", moveOrCopy);
            //dialog.Skip = new OverwriteOption(
            //    String.Format("Don't {0}", moveOrCopy),
            //    "Skip this item");

            ////dialog.SkipButton.GraphicalEntries.Add(new GraphicalTextEntry(skipText, PromptuFonts.CopyActionMainFont, PromptuColors.CopyActionColor), 0, 0);
            //// dialog.SkipButton.GraphicalEntries.Add(new GraphicalTextEntry("Skip this item", PromptuFonts.CopyActionDetailFont, PromptuColors.CopyActionColor), 0, 1);

            //string renameText = String.Format("{0}, but keep {1}", moveOrCopy, count == 1 ? "both" : "all");
            //string renameDetail = String.Format("The item you are {0} will be renamed '{1}'", movingOrCopying, renameName);

            //dialog.Rename = new OverwriteOption(
            //    renameText,
            //    renameDetail);
            ////dialog.RenameButton.GraphicalEntries.Add(new GraphicalTextEntry(renameText, PromptuFonts.CopyActionMainFont, PromptuColors.CopyActionColor), 0, 0);
            ////dialog.RenameButton.GraphicalEntries.Add(new GraphicalTextEntry(renameDetail, PromptuFonts.CopyActionDetailFont, PromptuColors.CopyActionColor), 0, 1);

            if (dialog.ShowDialog() == UIDialogResult.OK)
            {
                doForAll = dialog.DoActionForRemaining;
                return dialog.Action;
            }
            else
            {
                doForAll = false;
                return MoveConflictAction.Cancel;
            } 




            //StringBuilder message = new StringBuilder();

            //message.AppendFormat("'{0}' conficts with ", nameOfNew);
            ////bool singular = true;
            ////bool seenFirst = false;
            //int count = 0;

            //foreach (string name in oldNames)
            //{
            //    count++;
            //}

            //if (count == 0)
            //{
            //    throw new ArgumentException("There must be at least one name in 'names'.");
            //}

            //int index = 0;
            //foreach (string name in oldNames)
            //{
            //    if (index > 0)
            //    {
            //        if (count > 2)
            //        {
            //            message.Append(", ");
            //        }
            //        else
            //        {
            //            message.Append(" ");
            //        }

            //        if (index == count - 1)
            //        {
            //            message.Append("and ");
            //        }
            //    }

            //    message.AppendFormat("'{0}'", name);
            //    index++;
            //    //else if (singular)
            //    //{
            //    //    singular = false;
            //    //    builder.AppendLine();
            //    //    builder.AppendLine(name);
            //    //}
            //    //else
            //    //{
            //    //    builder.AppendLine(name);
            //    //}
            //}

            ////string messageFormat;
            ////if (singular)
            ////{
            ////    messageFormat = Localization.MessageFormats.ItemAlreadyExists;
            ////}
            ////else
            ////{
            ////    messageFormat = Localization.MessageFormats.ItemAlreadyExistsMulitpleConflicts;
            ////}

            //dialog.Message = message.ToString();

           // string moveOrCopy = moveNotCopy ? "Move" : "Copy";
           // string movingOrCopying = moveNotCopy ? "moving" : "copying";

           // //string replaceText = ;
           // dialog.Replace = new OverwriteOption(
           //     String.Format("{0} and Replace", moveOrCopy),
           //     "All conflicting items will be removed");
           // //dialog.ReplaceButton.GraphicalEntries.Add(new GraphicalTextEntry(replaceText, PromptuFonts.CopyActionMainFont, PromptuColors.CopyActionColor), 0, 0);
           // //dialog.ReplaceButton.GraphicalEntries.Add(new GraphicalTextEntry("All conflicting items will be removed", PromptuFonts.CopyActionDetailFont, PromptuColors.CopyActionColor), 0, 1);

           // //string skipText = String.Format("Don't {0}", moveOrCopy);
           // dialog.Skip = new OverwriteOption(
           //     String.Format("Don't {0}", moveOrCopy),
           //     "Skip this item");

           // //dialog.SkipButton.GraphicalEntries.Add(new GraphicalTextEntry(skipText, PromptuFonts.CopyActionMainFont, PromptuColors.CopyActionColor), 0, 0);
           //// dialog.SkipButton.GraphicalEntries.Add(new GraphicalTextEntry("Skip this item", PromptuFonts.CopyActionDetailFont, PromptuColors.CopyActionColor), 0, 1);

           // string renameText = String.Format("{0}, but keep {1}", moveOrCopy, count == 1 ? "both" : "all");
           // string renameDetail = String.Format("The item you are {0} will be renamed '{1}'", movingOrCopying, renameName);

           // dialog.Rename = new OverwriteOption(
           //     renameText,
           //     renameDetail);
           // //dialog.RenameButton.GraphicalEntries.Add(new GraphicalTextEntry(renameText, PromptuFonts.CopyActionMainFont, PromptuColors.CopyActionColor), 0, 0);
           // //dialog.RenameButton.GraphicalEntries.Add(new GraphicalTextEntry(renameDetail, PromptuFonts.CopyActionDetailFont, PromptuColors.CopyActionColor), 0, 1);

           // if (dialog.ShowDialog() == UIDialogResult.OK)
           // {
           //     doForAll = dialog.DoActionForRemaining;
           //     return dialog.Action;
           // }
           // else
           // {
           //     doForAll = false;
           //     return MoveConfictAction.Cancel;
           // } 

            //doForAll = true;
            //return MoveConfictAction.Overwrite;
           
            //return MessageBox.Show(
            //    message,
            //    Localization.Promptu.AppName,
            //    MessageBoxButtons.YesNoCancel,
            //    MessageBoxIcon.Exclamation,
            //    MessageBoxDefaultButton.Button2,
            //    Skins.PromptHandler.GetInstance().SetupDialog.GetMessageBoxOptions());
        }
    }
}
