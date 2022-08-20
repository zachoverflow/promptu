//-----------------------------------------------------------------------
// <copyright file="OptionsTextEntry.cs" company="ZachJohnson">
//     Copyright (c) Zach Johnson. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

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
