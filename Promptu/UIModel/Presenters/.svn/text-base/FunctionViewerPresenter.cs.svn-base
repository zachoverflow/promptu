using System;
using System.Collections.Generic;
using System.Text;
using ZachJohnson.Promptu.UIModel.Interfaces;
using ZachJohnson.Promptu.UserModel;
using ZachJohnson.Promptu.UserModel.Collections;

namespace ZachJohnson.Promptu.UIModel.Presenters
{
    internal class FunctionViewerPresenter : DialogPresenterBase<IFunctionViewer>
    {
        private Dictionary<List, AssemblyReferenceCollectionComposite> prioritizedLists = new Dictionary<List, AssemblyReferenceCollectionComposite>();
        private SimpleCollectionPresenter functionsList;
        private Profile profile;
        private Dictionary<CompositeItem<Function, List>, string> functionSignatureCache = new Dictionary<CompositeItem<Function, List>, string>();

        public FunctionViewerPresenter(Profile profile, List priorityList)
            : this(
            InternalGlobals.GuiManager.ToolkitHost.Factory.ConstructFunctionViewer(),
            profile,
            priorityList)
        {
        }

        public FunctionViewerPresenter(IFunctionViewer nativeInterface, Profile profile, List priorityList)
            : base(nativeInterface)
        {
            if (profile == null)
            {
                throw new ArgumentNullException("profile");
            }

            this.functionsList = new SimpleCollectionPresenter(nativeInterface.FunctionsList);

            this.functionsList.Headers.Add(
                Localization.UIResources.FunctionViewerFunctionHeader,
                this.GetFunctionSignature);

            this.functionsList.Headers.Add(
                Localization.UIResources.FunctionViewerListFromHeader,
                this.GetFunctionListName);

            this.NativeInterface.Text = Localization.UIResources.FunctionViewerText;
            this.NativeInterface.MainInstructions = Localization.UIResources.FunctionViewerMainInstructions;
            this.NativeInterface.OkButton.Text = Localization.UIResources.OkButtonText;

            List<string> sortedKeys = new List<string>(profile.CompositeFunctionsAndCommands.AllKeys);
            sortedKeys.Sort();

            this.profile = profile;

            //Dictionary<List, AssemblyReferenceCollectionComposite> prioritizedLists = new Dictionary<List, AssemblyReferenceCollectionComposite>();

            foreach (string key in sortedKeys)
            {
                GroupedCompositeItem compositeItem = profile.CompositeFunctionsAndCommands[key];
                if (compositeItem != null && compositeItem.StringFunctions.Count > 0)
                {
                    foreach (CompositeItem<Function, List> function in this.SimplifyFunctionComposite(compositeItem, priorityList))
                    {
                        //AssemblyReferenceCollectionComposite prioritizedList;

                        //if (prioritizedLists.ContainsKey(function.ListFrom))
                        //{
                        //    prioritizedList = prioritizedLists[function.ListFrom];
                        //}
                        //else
                        //{
                        //    prioritizedList = new AssemblyReferenceCollectionComposite(profile.Lists, function.ListFrom);
                        //    prioritizedLists.Add(function.ListFrom, prioritizedList);
                        //}

                        ////ListViewItem mainItem = new ListViewItem();
                        //List<string> values = new List<string>(2);
                        //values.Add(function.Item.GetNamedSignatureIfPossible(prioritizedList));
                        //values.Add(function.ListFrom.Name);
                        this.functionsList.Add(function);
                        //this.GetFunctionSignature(function);
                        //mainItem.Tag = function;
                        //this.functionsListView.Items.Add(mainItem);
                    }
                }
            }

            this.UpdateInterfaceToMatchSelectedFunction();

            this.functionsList.SelectedIndexChanged += this.HandleSelectedFunctionChanged;

            this.NativeInterface.Closed += this.HandleClosed;
        }

        private object GetFunctionSignature(object obj)
        {
            CompositeItem<Function, List> function = (CompositeItem<Function, List>)obj;

            string cachedSignature;
            if (this.functionSignatureCache.TryGetValue(function, out cachedSignature))
            {
                return cachedSignature;
            }

            AssemblyReferenceCollectionComposite prioritizedList;

            if (prioritizedLists.ContainsKey(function.ListFrom))
            {
                prioritizedList = prioritizedLists[function.ListFrom];
            }
            else
            {
                prioritizedList = new AssemblyReferenceCollectionComposite(profile.Lists, function.ListFrom);
                prioritizedLists.Add(function.ListFrom, prioritizedList);
            }

            string signature = function.Item.GetNamedSignatureIfPossible(prioritizedList);
            this.functionSignatureCache.Add(function, signature);
            return signature;
        }

        private object GetFunctionListName(object obj)
        {
            CompositeItem<Function, List> function = (CompositeItem<Function, List>)obj;
            return function.ListFrom.Name;
        }

        public event EventHandler Closed;

        public void Show(object owner)
        {
            this.NativeInterface.Show(owner);
        }

        protected virtual void OnClosed(EventArgs e)
        {
            EventHandler handler = this.Closed;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        private void HandleClosed(object sender, EventArgs e)
        {
            this.OnClosed(EventArgs.Empty);
        }

        private List<CompositeItem<Function, List>> SimplifyFunctionComposite(GroupedCompositeItem compositeItem, List priorityList)
        {
            List<CompositeItem<Function, List>> simplfied = new List<CompositeItem<Function, List>>();

            List<string> alreadyAddedParameterSignatures = new List<string>();

            if (priorityList != null)
            {
                foreach (CompositeItem<Function, List> item in compositeItem.StringFunctions)
                {
                    if (item.ListFrom == priorityList)
                    {
                        simplfied.Add(item);
                        alreadyAddedParameterSignatures.Add(item.Item.ParameterSignature);
                    }
                }
            }

            foreach (CompositeItem<Function, List> item in compositeItem.StringFunctions)
            {
                string normalizedName = item.Item.Name.ToUpperInvariant();
                if (!alreadyAddedParameterSignatures.Contains(item.Item.ParameterSignature))
                {
                    simplfied.Add(item);
                    alreadyAddedParameterSignatures.Add(item.Item.ParameterSignature);
                }
            }

            return simplfied;
        }

        private void HandleSelectedFunctionChanged(object sender, EventArgs e)
        {
            this.UpdateInterfaceToMatchSelectedFunction();
        }

        private void UpdateInterfaceToMatchSelectedFunction()
        {
            if (this.functionsList.SelectedIndexes.Count > 0)
            {
                CompositeItem<Function, List> function = (CompositeItem<Function, List>)this.functionsList[this.functionsList.PrimarySelectedIndex];
                //ListViewItem selectedItem = this.functionsListView.SelectedItems[0];
                this.NativeInterface.SelectedFunctionName = (string)this.GetFunctionSignature(function);//this.functionsList.GetTextAt(this.functionsList.PrimarySelectedIndex, 0);
                
                if (function != null)
                {
                    string documentation = function.Item.TryConstructFullDocumentation(new AssemblyReferenceCollectionComposite(this.profile.Lists, function.ListFrom));
                    if (documentation == null)
                    {
                        this.NativeInterface.SelectedFunctionDescription = String.Empty;
                    }
                    else
                    {
                        this.NativeInterface.SelectedFunctionDescription = documentation;
                    }
                }
                else
                {
                    this.NativeInterface.SelectedFunctionDescription = String.Empty;
                }
            }
            else
            {
                this.NativeInterface.SelectedFunctionName = String.Empty;
                this.NativeInterface.SelectedFunctionDescription = String.Empty;
            }
        }
    }
}
