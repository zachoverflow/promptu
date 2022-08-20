using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using ZachJohnson.Promptu.UIModel.Interfaces;
using System.Windows;

namespace ZachJohnson.Promptu.WpfUI.UIComponents
{
    internal class WpfComboInput : ComboBox, IComboInput
    {
        public WpfComboInput()
        {
            this.AddHandler(TextBox.TextChangedEvent, new RoutedEventHandler(this.HandleTextChanged));
        }

        public event EventHandler TextChanged;

        public event EventHandler SelectedIndexChanged;

        public object[] Values
        {
            set 
            {
                foreach (object item in value)
                {
                    this.Items.Add(item);
                }
            }
        }

        public void AddValue(object value)
        {
            this.Items.Add(value);
        }

        public int ValueCount
        {
            get { return this.Items.Count; }
        }

        private void HandleTextChanged(object sender, RoutedEventArgs e)
        {
            this.OnTextChanged(EventArgs.Empty);
        }

        protected override void OnSelectionChanged(SelectionChangedEventArgs e)
        {
            if (e.Source == this)
            {
                this.OnSelectedIndexChanged(EventArgs.Empty);
            }

            base.OnSelectionChanged(e);
        }

        protected virtual void OnTextChanged(EventArgs e)
        {
            EventHandler handler = this.TextChanged;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        protected virtual void OnSelectedIndexChanged(EventArgs e)
        {
            EventHandler handler = this.SelectedIndexChanged;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        public void AddRange<T>(IEnumerable<T> values)
        {
            foreach (T item in values)
            {
                this.Items.Add(item);
            }
        }

        public bool Enabled
        {
            get { return this.IsEnabled; }
            set { this.IsEnabled = value; }
        }

        public void Clear()
        {
            this.Items.Clear();
        }
    }
}
