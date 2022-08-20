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

    public class TextConversionInfo : GroupingConversionInfo, INotifyPropertyChanged
    {
        private string cue;
        private double? minEditWidth;

        public TextConversionInfo(string cue)
            : this(null, false, cue, null)
        {
        }

        public TextConversionInfo(string groupName, bool groupEditControl)
            : this(groupName, groupEditControl, null, null)
        {
        }

        public TextConversionInfo(string groupName, bool groupEditControl, string cue, double? minEditWidth)
            : base(groupName, groupEditControl)
        {
            this.cue = cue;
            this.minEditWidth = minEditWidth;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public string Cue
        {
            get 
            {
                return this.cue; 
            }

            set
            {
                this.cue = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs("Cue"));
            }
        }

        public double? MinEditWidth
        {
            get
            {
                return this.minEditWidth;
            }

            set
            {
                this.minEditWidth = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs("MinEditWidth"));
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
