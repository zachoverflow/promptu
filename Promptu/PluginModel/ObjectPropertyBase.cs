//-----------------------------------------------------------------------
// <copyright file="ObjectPropertyBase.cs" company="ZachJohnson">
//     Copyright (c) Zach Johnson. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace ZachJohnson.Promptu.PluginModel
{
    using System;
    using System.ComponentModel;

    public abstract class ObjectPropertyBase : INotifyPropertyChanged, IDisposable
    {
        private IPropertyEditorFactory editorFactory;
        private string label;
        private string id;
        private bool isEnabled = true;
        private object conversionInfo;
        private int indent;

        public ObjectPropertyBase(
            string id, 
            string label, 
            IPropertyEditorFactory editorFactory,
            object conversionInfo)
        {
            if (id == null)
            {
                throw new ArgumentNullException("id");
            }

            this.id = id;
            this.label = label;
            this.editorFactory = editorFactory;
            this.conversionInfo = conversionInfo;
        }

        ~ObjectPropertyBase()
        {
            this.Dispose(false);
            GC.SuppressFinalize(this);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public event EventHandler ValueChanged;

        public string Id
        {
            get { return this.id; }
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

        public object ConversionInfo
        {
            get
            {
                return this.conversionInfo;
            }

            set
            {
                this.conversionInfo = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs("ConversionInfo"));
            }
        }

        public bool IsEnabled
        {
            get
            {
                return this.isEnabled;
            }

            set
            {
                this.isEnabled = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs("IsEnabled"));
            }
        }

        public IPropertyEditorFactory EditorFactory
        {
            get { return this.editorFactory; }
        }

        public object ObjectValue
        {
            get 
            {
                return this.GetObjectValueCore(); 
            }

            set
            {
                this.SetObjectValueCore(value);
                this.NotifyValueChanged();
            }
        }

        public Type ValueType
        {
            get { return this.GetValueTypeCore(); }
        }

        public void NotifyValueChanged()
        {
            this.OnPropertyChanged(new PropertyChangedEventArgs("Value"));
            this.OnValueChanged(EventArgs.Empty);
        }

        public void Dispose()
        {
            this.Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            this.ValueChanged = null;
            this.PropertyChanged = null;
        }

        protected abstract object GetObjectValueCore();

        protected abstract void SetObjectValueCore(object value);

        protected abstract Type GetValueTypeCore();

        protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            PropertyChangedEventHandler handler = this.PropertyChanged;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        protected virtual void OnValueChanged(EventArgs e)
        {
            EventHandler handler = this.ValueChanged;
            if (handler != null)
            {
                handler(this, e);
            }
        }
    }
}
