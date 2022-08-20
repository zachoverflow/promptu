// Copyright 2022 Zach Johnson
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

namespace ZachJohnson.Promptu.PluginModel
{
    using System;
    using System.Collections.Generic;
    using ZachJohnson.Promptu.UIModel;

    public class PromptuPluginFactory
    {
        private List<WeakReference<UIMenuItem>> ownedMenuItems = new List<WeakReference<UIMenuItem>>();
        private bool forceHideAll;

        public UIMenuItem CreateMenuItem(string id)
        {
            UIMenuItem item = new UIMenuItem(id);
            this.Initialize(item);
            this.ownedMenuItems.Add(new WeakReference<UIMenuItem>(item));
            return item;
        }

        public UIMenuItem CreateMenuItem(string id, string text)
        {
            UIMenuItem item = new UIMenuItem(id, text);
            this.Initialize(item);
            this.ownedMenuItems.Add(new WeakReference<UIMenuItem>(item));
            return item;
        }

        public UIMenuItem CreateMenuItem(string id, string text, EventHandler clickEventHandler)
        {
            UIMenuItem item = new UIMenuItem(id, text, clickEventHandler);
            this.Initialize(item);
            this.ownedMenuItems.Add(new WeakReference<UIMenuItem>(item));
            return item;
        }

        public UIMenuItem CreateMenuItem(string id, string text, string toolTipText, object image)
        {
            UIMenuItem item = new UIMenuItem(id, text, toolTipText, image);
            this.Initialize(item);
            this.ownedMenuItems.Add(new WeakReference<UIMenuItem>(item));
            return item;
        }

        public UIMenuItem CreateMenuItem(string id, string text, object image, EventHandler clickEventHandler)
        {
            UIMenuItem item = new UIMenuItem(id, text, image, clickEventHandler);
            this.Initialize(item);
            this.ownedMenuItems.Add(new WeakReference<UIMenuItem>(item));
            return item;
        }

        public UIMenuItem CreateMenuItem(string id, string text, string toolTipText, object image, EventHandler clickEventHandler)
        {
            UIMenuItem item = new UIMenuItem(id, text, toolTipText, image, clickEventHandler);
            this.Initialize(item);
            this.ownedMenuItems.Add(new WeakReference<UIMenuItem>(item));
            return item;
        }

        public void SetForceHideAll(bool value)
        {
            if (this.forceHideAll == value)
            {
                return;
            }

            this.forceHideAll = value;

            List<WeakReference<UIMenuItem>> itemsToRemove = new List<WeakReference<UIMenuItem>>();
            foreach (WeakReference<UIMenuItem> item in this.ownedMenuItems)
            {
                UIMenuItem realItem = item.Target;
                if (realItem == null)
                {
                    itemsToRemove.Add(item);
                }

                realItem.OverrideAsUnavailable = value;
            }

            foreach (var item in itemsToRemove)
            {
                this.ownedMenuItems.Remove(item);
            }
        }

        private void Initialize(UIMenuItemBase item)
        {
            item.OverrideAsUnavailable = this.forceHideAll;
        }
    }
}
