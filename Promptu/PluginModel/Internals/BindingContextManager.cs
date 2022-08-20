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

namespace ZachJohnson.Promptu.PluginModel.Internals
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;

    internal class BindingContextManager : IDisposable
    {
        private BindingExpression bindingExpression;
        private List<object> bindingContextChain = new List<object>();

        public BindingContextManager(BindingExpression bindingExpression)
        {
            if (bindingExpression == null)
            {
                throw new ArgumentNullException("bindingExpression");
            }

            this.bindingExpression = bindingExpression;
        }

        ~BindingContextManager()
        {
            this.Dispose(false);
        }

        public event EventHandler PropertyChanged;

        public object BaseContext
        {
            get
            {
                return this.bindingContextChain.Count <= 0 ? null : this.bindingContextChain[0];
            }

            set
            {
                this.UnhookChangedHandlers(0);

                if (this.bindingContextChain.Count <= 0)
                {
                    this.bindingContextChain.Add(value);
                }
                else
                {
                    this.bindingContextChain[0] = value;
                }

                this.HookChangedHandlers(-1);
            }
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            this.UnhookChangedHandlers(0);
            this.bindingContextChain.Clear();
        }

        protected virtual void OnPropertyChanged(EventArgs e)
        {
            EventHandler handler = this.PropertyChanged;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        private void HookChangedHandlers(int offset)
        {
            if (this.bindingContextChain.Count <= 0)
            {
                throw new ArgumentException("No binding context chain base is set.");
            }

            for (int i = this.bindingContextChain.Count + offset; 
                i < this.bindingExpression.Count; 
                i++)
            {
                object currentContext;

                if (i >= this.bindingContextChain.Count)
                {
                    currentContext = this.bindingExpression[i - 1].GetValue(this.bindingContextChain[i - 1]);
                    this.bindingContextChain.Add(currentContext);
                }
                else
                {
                    currentContext = this.bindingContextChain[i];
                }

                INotifyPropertyChanged notifyInterface = currentContext as INotifyPropertyChanged;

                if (notifyInterface != null)
                {
                    notifyInterface.PropertyChanged += this.HandleContextPropertyChanged;
                }
            }
        }

        private void UnhookChangedHandlers(int startingFrom)
        {
            if (startingFrom < 0)
            {
                throw new ArgumentOutOfRangeException("startingFrom");
            }

            for (int i = startingFrom; i < this.bindingContextChain.Count; i++)
            {
                object context = this.bindingContextChain[i];
                INotifyPropertyChanged notifyInterface;
                if (context != null)
                {
                    notifyInterface = context as INotifyPropertyChanged;

                    if (notifyInterface != null)
                    {
                        notifyInterface.PropertyChanged -= this.HandleContextPropertyChanged;
                    }
                }
            }

            this.bindingContextChain.RemoveRange(
                startingFrom,
                this.bindingContextChain.Count - startingFrom);
        }

        private void ReevaulateContext(int startingFrom)
        {
            if (startingFrom <= 0)
            {
                throw new ArgumentOutOfRangeException("startingFrom");
            }

            this.UnhookChangedHandlers(startingFrom);
            this.HookChangedHandlers(0);
        }

        private void HandleContextPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            int index = this.bindingContextChain.IndexOf(sender);

            if (index < 0)
            {
                return;
            }

            BindingNode node = this.bindingExpression[index];

            if (node.PropertyName == e.PropertyName)
            {
                this.ReevaulateContext(index + 1);
            }

            this.OnPropertyChanged(EventArgs.Empty);
        }
    }
}
