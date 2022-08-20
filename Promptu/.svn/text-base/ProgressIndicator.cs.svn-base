using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace ZachJohnson.Promptu
{
    internal class ProgressIndicator : IIndicatesProgress, INotifyPropertyChanged
    {
        private string statusMessage;
        private int progress;

        public ProgressIndicator()
        {
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public string StatusMessage
        {
            get
            {
                return this.statusMessage;
            }

            set
            {
                this.statusMessage = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs("StatusMessage"));
            }
        }

        public int ProgressPercentage
        {
            get
            {
                return this.progress;
            }

            set
            {
                this.progress = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs("ProgressPercentage"));
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
