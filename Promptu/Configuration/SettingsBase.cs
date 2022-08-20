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

namespace ZachJohnson.Promptu.Configuration
{
    using System;
    using System.Xml;

    public abstract class SettingsBase
    {
        private bool raiseSettingChanged = true;

        public SettingsBase()
        {
        }

        public event EventHandler SettingChanged;

        protected bool RaiseSettingChanged
        {
            get { return this.raiseSettingChanged; }
            set { this.raiseSettingChanged = value; }
        }

        public XmlNode ToXml(string name, XmlDocument document)
        {
            if (name == null)
            {
                throw new ArgumentNullException("name");
            }
            else if (document == null)
            {
                throw new ArgumentNullException("document");
            }

            XmlNode node = document.CreateElement(name);
            this.ToXmlCore(node);
            return node;
        }

        public void AppendAsXmlOn(XmlNode node, string name)
        {
            if (name == null)
            {
                throw new ArgumentNullException("name");
            }
            else if (node == null)
            {
                throw new ArgumentNullException("node");
            }

            node.AppendChild(this.ToXml(name, node.OwnerDocument));
        }

        public void UpdateFrom(XmlNode node)
        {
            if (node == null)
            {
                throw new ArgumentNullException("node");
            }

            this.UpdateFromCore(node);
        }

        protected void Register(SettingsBase childSettings)
        {
            if (childSettings == null)
            {
                throw new ArgumentNullException("childSettings");
            }

            childSettings.SettingChanged += this.RaiseSettingChangedEvent;
        }

        protected void Unregister(SettingsBase childSettings)
        {
            if (childSettings == null)
            {
                throw new ArgumentNullException("childSettings");
            }

            childSettings.SettingChanged -= this.RaiseSettingChangedEvent;
        }       

        protected virtual void OnSettingChanged(EventArgs e)
        {
            if (!this.raiseSettingChanged)
            {
                return;
            }

            EventHandler handler = this.SettingChanged;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        protected void RaiseSettingChangedEvent(object sender, EventArgs e)
        {
            this.OnSettingChanged(EventArgs.Empty);
        }

        protected abstract void ToXmlCore(XmlNode node);

        protected abstract void UpdateFromCore(XmlNode node);
    }
}
