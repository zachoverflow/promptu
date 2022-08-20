//-----------------------------------------------------------------------
// <copyright file="OptionsGroup.cs" company="ZachJohnson">
//     Copyright (c) Zach Johnson. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace ZachJohnson.Promptu.PluginModel
{
    using System;
    using System.ComponentModel;
    using ZachJohnson.Promptu.UIModel;

    public class OptionsGroup : BindingCollection<object>
    {
        private string id;
        private string label;

        public OptionsGroup(string id, string label)
        {
            if (id == null)
            {
                throw new ArgumentNullException("id");
            }
            else if (label == null)
            {
                throw new ArgumentNullException("label");
            }

            this.id = id;
            this.label = label;
        }

        public string Id
        {
            get { return this.id; }
        }

        public string Label
        {
            get
            {
                return this.label;
            }

            set
            {
                this.label = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs("Label"));
            }
        }
    }
}
