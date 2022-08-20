//-----------------------------------------------------------------------
// <copyright file="InformationBoxManager.cs" company="ZachJohnson">
//     Copyright (c) Zach Johnson. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace ZachJohnson.Promptu.Skins
{
    using System;
    using System.Collections.Generic;
    using ZachJohnson.Promptu.SkinApi;
    using ZachJohnson.Promptu.UIModel;

    internal class InformationBoxManager : IEnumerable<IInfoBox>
    {
        private List<IInfoBox> informationBoxes = new List<IInfoBox>();
        private WindowManager windowManager;
        private IInfoBox itemInfoBox;
        private IInfoBox parameterHelpBox;
        private ParameterlessVoid activateDefaultWindow;
        private List<IInfoBox> allowHideAndDestroy = new List<IInfoBox>();
        private Dictionary<IInfoBox, int> resetsTillDestroy = new Dictionary<IInfoBox, int>();

        public InformationBoxManager(WindowManager windowManager, ParameterlessVoid activateDefaultWindow)
        {
            if (windowManager == null)
            {
                throw new ArgumentNullException("windowManager");
            }
            else if (activateDefaultWindow == null)
            {
                throw new ArgumentNullException("activateDefaultWindow");
            }

            this.windowManager = windowManager;
            this.activateDefaultWindow = activateDefaultWindow;
        }

        public event EventHandler<ImageClickEventArgs> ParameterHelpImageClick;

        public IInfoBox ItemInfoBox
        {
            get { return this.itemInfoBox; }
        }

        public IInfoBox ParameterHelpBox
        {
            get { return this.parameterHelpBox; }
        }

        public void RegisterAndShow(IInfoBox box, bool hideAndDestroyWhenActivated, InformationBoxType type, object owner)
        {
            this.Register(box, hideAndDestroyWhenActivated, type);
            InternalGlobals.GuiManager.ToolkitHost.TrySetOwner(box, owner);

            box.Show();
            if (hideAndDestroyWhenActivated)
            {
                this.SetAllowHideAndDestory(box, true);
            }
        }

        public void Register(IInfoBox box, bool hideAndDestroyWhenActivated, InformationBoxType type)
        {
            this.informationBoxes.Add(box);
            if (!hideAndDestroyWhenActivated)
            {
                this.windowManager.TryRegister(box, false);
            }
            else
            {
                this.windowManager.TryRegister(box, box, new Action<IInfoBox>(this.HideAndDestroyInformationBox), false);
            }

            switch (type)
            {
                case InformationBoxType.ItemInfo:
                    this.itemInfoBox = box;
                    break;
                case InformationBoxType.ParameterHelp:
                    this.parameterHelpBox = box;

                    // REVISIT
                    ITextInfoBox textualInfoBox = box as ITextInfoBox;
                    if (textualInfoBox != null)
                    {
                        textualInfoBox.ImageClick += this.HandleParameterHelpImageClick;
                    }

                    break;
                default:
                    break;
            }

            box.TopMost = true;
        }

        public void SetAllowHideAndDestory(IInfoBox box, bool value)
        {
            if (value)
            {
                if (!this.allowHideAndDestroy.Contains(box))
                {
                    this.allowHideAndDestroy.Add(box);
                }
            }
            else
            {
                this.allowHideAndDestroy.Remove(box);
            }
        }

        public void RegisterAndShow(IInfoBox box, bool hideAndDestroyWhenActivated, object owner)
        {
            this.RegisterAndShow(box, hideAndDestroyWhenActivated, InformationBoxType.Default, owner);
        }

        public void Register(IInfoBox box, bool hideAndDestroyWhenActivated)
        {
            this.Register(box, hideAndDestroyWhenActivated, InformationBoxType.Default);
        }

        public void SetResetsTillDestroy(IInfoBox box, int value)
        {
            if (this.informationBoxes.Contains(box))
            {
                this.resetsTillDestroy.Add(box, value);
            }
        }

        public void Unregister(IInfoBox box)
        {
            this.informationBoxes.Remove(box);
            this.windowManager.Unregister(box);
            if (this.itemInfoBox == box)
            {
                this.itemInfoBox = null;
            }
            else if (this.parameterHelpBox == box)
            {
                ITextInfoBox textualInfoBox = box as ITextInfoBox;
                if (textualInfoBox != null)
                {
                    textualInfoBox.ImageClick += this.HandleParameterHelpImageClick;
                }

                this.parameterHelpBox = null;
            }

            this.allowHideAndDestroy.Remove(box);
            this.resetsTillDestroy.Remove(box);
        }

        public void HideAndDestroy(IInfoBox box)
        {
            if (box != null)
            {
                this.activateDefaultWindow();
                box.Hide();
                this.Unregister(box);
            }
        }

        public void HideAndDestroyAll()
        {
            foreach (IInfoBox box in this.informationBoxes.ToArray())
            {
                box.Hide();
                this.Unregister(box);
            }
        }

        public void HideAndDestroyAllExceptParameterHelp()
        {
            foreach (IInfoBox box in this.informationBoxes.ToArray())
            {
                if (box != this.parameterHelpBox)
                {
                    int value;
                    if (this.resetsTillDestroy.TryGetValue(box, out value))
                    {
                        if (value > 0)
                        {
                            this.resetsTillDestroy[box]--;
                            continue;
                        }
                    }

                    box.Hide();
                    this.Unregister(box);
                }
            }
        }

        public IEnumerator<IInfoBox> GetEnumerator()
        {
            return this.informationBoxes.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        protected virtual void OnParameterHelpMouseDown(ImageClickEventArgs e)
        {
            EventHandler<ImageClickEventArgs> handler = this.ParameterHelpImageClick;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        private void HideAndDestroyInformationBox(IInfoBox box)
        {
            this.activateDefaultWindow();
            if (this.allowHideAndDestroy.Contains(box))
            {
                box.Hide();
                this.Unregister(box);
            }
        }

        private void HandleParameterHelpImageClick(object sender, ImageClickEventArgs e)
        {
            this.OnParameterHelpMouseDown(e);
        }
    }
}