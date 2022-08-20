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
    using System.ComponentModel;

    public class OptionsTextEntry : INotifyPropertyChanged
    {
        private string text;
        private int indent;
        private TextEntryType entryType;
        private bool visible = true;

        public OptionsTextEntry(string text, TextEntryType entryType, int indent)
        {
            this.text = text;
            this.entryType = entryType;
            this.indent = indent;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public bool Visible
        {
            get 
            { 
                return this.visible; 
            }

            set
            {
                this.visible = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs("Visible"));
            }
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
                this.OnPropertyChanged(new PropertyChangedEventArgs("Text"));
            }
        }

        public int Indent
        {
            get
            {
                return this.indent;
            }

            set
            {
                this.indent = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs("Indent"));
            }
        }

        public TextEntryType EntryType
        {
            get
            {
                return this.entryType;
            }

            set
            {
                this.entryType = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs("EntryType"));
            }
        }

        protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            PropertyChangedEventHandler handler = this.PropertyChanged;
            if (handler != null)
            {
                handler(this, e);
            }
        }
    }
}
