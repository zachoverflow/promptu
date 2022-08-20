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
    using System.ComponentModel;

    public class EnumConversionInfo : GroupingConversionInfo, INotifyPropertyChanged
    {
        private Type enumType;
        private List<EnumValueInfo> entries = new List<EnumValueInfo>();
        private double? minEditWidth;

        public EnumConversionInfo(Type enumType)
            : this(enumType, null, null, false)
        {
        }

        public EnumConversionInfo(Type enumType, double? minEditWidth, string groupName, bool groupEditControl)
            : base(groupName, groupEditControl)
        {
            if (enumType == null)
            {
                throw new ArgumentNullException("enumType");
            }
            else if (!enumType.IsEnum)
            {
                throw new ArgumentException("'enumType' is not derived from Enum.");
            }

            this.enumType = enumType;
            this.minEditWidth = minEditWidth;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public List<EnumValueInfo> Entries
        {
            get { return this.entries; }
        }

        public Type EnumType
        {
            get { return this.enumType; }
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
