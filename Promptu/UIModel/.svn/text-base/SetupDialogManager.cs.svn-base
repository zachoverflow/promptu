using System;
using System.Collections.Generic;
using System.Text;
using ZachJohnson.Promptu.UIModel.Presenters;
using ZachJohnson.Promptu.UserModel;

namespace ZachJohnson.Promptu.UIModel
{
    internal class SetupDialogManager
    {
        private SetupDialogPresenter setupDialog;

        public SetupDialogManager()
        {
        }

        //public event EventHandler HotkeyChanged;

        public void EditCommand(string name)
        {
            this.GetSetupDialog().EditCommand(name);
        }

        public void UpdateCurrentListCommands()
        {
            SetupDialogPresenter setupDialog = this.setupDialog;
            if (setupDialog != null)
            {
                setupDialog.CommandSetupPanel.UpdateItemsListView();
            }
        }

        public void UpdateCurrentListFunctions()
        {
            SetupDialogPresenter setupDialog = this.setupDialog;
            if (setupDialog != null)
            {
                setupDialog.FunctionSetupPanel.UpdateItemsListView();
            }
        }

        public void EditCommand(string name, List listFrom, Command contents)
        {
            this.GetSetupDialog().EditCommand(name, listFrom, contents);
        }

        public void EditCommand(string name, List listFrom)
        {
            this.GetSetupDialog().EditCommand(name, listFrom, null);
        }

        public void EditNewCommand(List list, Command contents)
        {
            this.GetSetupDialog().EditNewCommand(list, contents);
        }

        public void EditFunction(string name, string parameterSignature)
        {
            this.GetSetupDialog().EditFunction(name, parameterSignature);
        }

        public void EditFunction(string name, string parameterSignature, List listFrom)
        {
            this.GetSetupDialog().EditFunction(name, parameterSignature, listFrom);
        }

        public void ShowUserToAbout()
        {
            this.GetSetupDialog().ShowUserToAbout();
        }

        public void SetSelectedListTab(int index)
        {
            SetupDialogPresenter setupDialog = this.setupDialog;
            if (setupDialog != null)
            {
                setupDialog.ListTabs.SelectedIndex = index;
            }
        }

        public void UpdateToCurrentList()
        {
            SetupDialogPresenter setupDialog = this.setupDialog;
            if (setupDialog != null)
            {
                setupDialog.UpdateToCurrentList();
            }
        }

        public void UpdateInstalledPlugins()
        {
            SetupDialogPresenter setupDialog = this.setupDialog;
            if (setupDialog != null)
            {
                setupDialog.UpdateInstalledPlugins();
            }
        }

        public void UpdatePluginDisplays()
        {
            SetupDialogPresenter setupDialog = this.setupDialog;
            if (setupDialog != null)
            {
                setupDialog.UpdatePluginDisplays();
            }
        }

        public void SyncLists()
        {
            SetupDialogPresenter setupDialog = this.setupDialog;
            if (setupDialog != null)
            {
                setupDialog.ListSelector.SyncLists();
            }
            else
            {
                ListSelectorPresenter.SyncListsStatic();
            }
        }

        public object DialogOwner
        {
            get
            {
                SetupDialogPresenter setupDialog = this.setupDialog;
                if (setupDialog != null)
                {
                    return setupDialog.NativeInterface;
                }

                return null;
            }
        }

        public void UpdateToCurrentListThreadSafe()
        {
            SetupDialogPresenter setupDialog = this.setupDialog;
            if (setupDialog != null)
            {
                ParameterlessVoid action = new ParameterlessVoid(delegate
                {
                    setupDialog.UpdateToCurrentList();
                });

                if (InternalGlobals.GuiManager.ToolkitHost.MainThreadDispatcher.InvokeRequired)
                {
                    InternalGlobals.GuiManager.ToolkitHost.MainThreadDispatcher.Invoke(action, null);
                }
                else
                {
                    action();
                }
            }
        }

        public void Show()
        {
            this.GetSetupDialog().Show();
        }

        public void CreateNewCommand(string listFolderName)
        {
            this.GetSetupDialog().CreateNewCommand(listFolderName);
        }

        public void DoItemPaste()
        {
            this.GetSetupDialog().DoItemPaste();
        }

        public void UpdateListSelectorLists()
        {
            SetupDialogPresenter setupDialog = this.setupDialog;
            if (setupDialog != null)
            {
                setupDialog.ListSelector.UpdateLists();
            }
        }

        public void UpdateListSelectorUI()
        {
            SetupDialogPresenter setupDialog = this.setupDialog;
            if (setupDialog != null)
            {
                setupDialog.ListSelector.UpdateUI();
            }
        }

        private SetupDialogPresenter GetSetupDialog()
        {
            using (DdMonitor.Lock(this))
            {
                if (this.setupDialog == null)
                {
                    ParameterlessVoid action = delegate 
                    {
                        this.setupDialog = new SetupDialogPresenter();
                        //this.setupDialog.HotkeyChanged += this.HandleHotkeyChanged;
                        //this.setupDialog.Closed += this.HandleSetupDialogClosed;
                    };

                    if (InternalGlobals.GuiManager.ToolkitHost.MainThreadDispatcher.InvokeRequired)
                    {
                        InternalGlobals.GuiManager.ToolkitHost.MainThreadDispatcher.Invoke(action, null);
                    }
                    else
                    {
                        action();
                    }
                }

                return this.setupDialog;
            }
        }

        //private void HandleHotkeyChanged(object sender, EventArgs e)
        //{
        //    this.OnHotkeyChanged(EventArgs.Empty);
        //}

        //protected virtual void OnHotkeyChanged(EventArgs e)
        //{
        //    EventHandler handler = this.HotkeyChanged;
        //    if (handler != null)
        //    {
        //        handler(this, e);
        //    }
        //}

        //private void HandleSetupDialogClosed(object sender, EventArgs e)
        //{
        //    using (DdMonitor.Lock(this))
        //    {
        //        this.setupDialog.Closed -= this.HandleSetupDialogClosed;
        //        this.setupDialog.Dispose();
        //        this.setupDialog = null;

        //        //GC.Collect();
        //    }
        //}
    }
}
