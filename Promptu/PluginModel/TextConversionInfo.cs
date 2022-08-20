//-----------------------------------------------------------------------
// <copyright file="TextConversionInfo.cs" company="ZachJohnson">
//     Copyright (c) Zach Johnson. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

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
