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

namespace ZachJohnson.Promptu.PTK
{
    using System;
    using ZachJohnson.Promptu.UIModel.Interfaces;

    internal class TabPageBase<TThis, TTabPage> : IWidgetHost where TThis : TabPageBase<TThis, TTabPage> where TTabPage : ITabPage
    {
        private string text;
        private string id;
        private TabWidgetBase<TThis, TTabPage> currentOwner;
        private TTabPage nativeInterface;
        private Widget hostedWidget;

        public TabPageBase(string id)
        {
            if (id == null)
            {
                throw new ArgumentNullException("id");
            }

            this.id = id;
        }

        public event EventHandler<WidgetEventArgs> HostedWidgetChanged;

        public string Id
        {
            get { return this.id; }
        }

        public string Text
        {
            get 
            {
                return this.text; 
            }

            set
            {
                this.text = value;
                this.NativeInterface.Text = value;
            }
        }

        public Widget HostedWidget
        {
            get 
            { 
                return this.hostedWidget; 
            }

            set
            {
                if (value != this.hostedWidget)
                {
                    if (value == null)
                    {
                        ((IWidgetHost)this).Remove(this.hostedWidget);
                    }
                    else
                    {
                        value.UnhostIfNecessary();
                        this.NativeInterface.SetContent(value.NativeObject);
                        this.hostedWidget = value;
                        this.hostedWidget.CurrentHost = this;
                    }

                    this.OnHostedWidgetChanged(new WidgetEventArgs(value));
                }
            }
        }

        internal TTabPage NativeInterface
        {
            get { return this.nativeInterface; }
            set { this.nativeInterface = value; }
        }

        internal TabWidgetBase<TThis, TTabPage> CurrentOwner
        {
            get { return this.currentOwner; }
            set { this.currentOwner = value; }
        }

        void IWidgetHost.Remove(Widget widget)
        {
            if (widget == null)
            {
                throw new ArgumentNullException("widget");
            }

            if (widget == this.hostedWidget)
            {
                this.hostedWidget.CurrentHost = null;
                this.NativeInterface.SetContent(null);
                this.hostedWidget = null;
            }
        }

        protected virtual void OnHostedWidgetChanged(WidgetEventArgs e)
        {
            EventHandler<WidgetEventArgs> handler = this.HostedWidgetChanged;
            if (handler != null)
            {
                handler(this, e);
            }
        }
    }
}
