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
    using System.Runtime.CompilerServices;
    using ZachJohnson.Promptu.PluginModel.Hooks;
    using ZachJohnson.Promptu.PluginModel.Internals;

    public delegate HookAction CommandExecutingHook(string textToExecute, ExecuteMode mode);

    public delegate HookAction CommandFullyResolvedExecutingHook(
            ExecuteMode mode,
            string resolvedTarget,
            string resolvedArguments,
            string resolvedWorkingDirectory);

    public delegate HookAction BasicHook();

    public class PromptuHooks
    {
        private Dictionary<Delegate, HookId> hooks = new Dictionary<Delegate, HookId>();
        private bool enabled;

        internal PromptuHooks()
        {
        }

        public event CommandExecutingHook CommandExecuting
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            add
            {
                this.AddHook(value, HookId.CommandExecuting);
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            remove
            {
                this.RemoveHook(value, HookId.CommandExecuting);
            }
        }

        public event CommandFullyResolvedExecutingHook CommandFullyResolvedExecuting
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            add
            {
                this.AddHook(value, HookId.CommandFullyResolvedExecuting);
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            remove
            {
                this.RemoveHook(value, HookId.CommandFullyResolvedExecuting);
            }
        }

        public event BasicHook ShowingSuggestionProvider
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            add
            {
                this.AddHook(value, HookId.ShowingSuggestionProvider);
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            remove
            {
                this.RemoveHook(value, HookId.ShowingSuggestionProvider);
            }
        }

        public event BasicHook CurrentProfileChanged
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            add
            {
                this.AddHook(value, HookId.CurrentProfileChanged);
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            remove
            {
                this.RemoveHook(value, HookId.CurrentProfileChanged);
            }
        }

        private enum HookId
        {
            CommandExecuting = 0,
            ShowingSuggestionProvider,
            CurrentProfileChanged,
            CommandFullyResolvedExecuting,
        }

        internal bool Enabled
        {
            get 
            { 
                return this.enabled; 
            }

            set
            {
                if (this.enabled != value)
                {
                    this.enabled = value;

                    foreach (var hookEntry in this.hooks)
                    {
                        this.SetAttached(hookEntry.Key, hookEntry.Value, value);
                    }
                }
            }
        }

        internal bool Contains(Delegate d)
        {
            return this.hooks.ContainsKey(d);
        }

        internal void Detach(Delegate d)
        {
            HookId hookId;
            if (this.hooks.TryGetValue(d, out hookId))
            {
                this.SetAttached(d, hookId, false);
            }
        }

        private void AddHook(Delegate d, HookId hookId)
        {
            this.hooks.Add(d, hookId);

            if (this.enabled)
            {
                this.SetAttached(d, hookId, true);
            }
        }

        private void RemoveHook(Delegate d, HookId hookId)
        {
            this.hooks.Remove(d);
            if (this.enabled)
            {
                this.SetAttached(d, hookId, false);
            }
        }

        private void SetAttached(Delegate d, HookId hookId, bool attach)
        {
            switch (hookId)
            {
                case HookId.CommandExecuting:
                    if (attach)
                    {
                        PromptuHookManager.CommandExecuting = 
                            (CommandExecutingHook)Delegate.Combine(PromptuHookManager.CommandExecuting, d);
                    }
                    else
                    {
                        PromptuHookManager.CommandExecuting = 
                            (CommandExecutingHook)Delegate.Remove(PromptuHookManager.CommandExecuting, d);
                    }

                    break;
                case HookId.CommandFullyResolvedExecuting:
                    if (attach)
                    {
                        PromptuHookManager.CommandFullyResolvedExecuting =
                            (CommandFullyResolvedExecutingHook)Delegate.Combine(PromptuHookManager.CommandFullyResolvedExecuting, d);
                    }
                    else
                    {
                        PromptuHookManager.CommandFullyResolvedExecuting =
                            (CommandFullyResolvedExecutingHook)Delegate.Remove(PromptuHookManager.CommandFullyResolvedExecuting, d);
                    }

                    break;
                case HookId.ShowingSuggestionProvider:
                    if (attach)
                    {
                        PromptuHookManager.ShowingSuggestionProvider =
                            (BasicHook)Delegate.Combine(PromptuHookManager.ShowingSuggestionProvider, d);
                    }
                    else
                    {
                        PromptuHookManager.ShowingSuggestionProvider =
                            (BasicHook)Delegate.Remove(PromptuHookManager.ShowingSuggestionProvider, d);
                    }

                    break;
                case HookId.CurrentProfileChanged:
                    if (attach)
                    {
                        PromptuHookManager.CurrentProfileChanged =
                            (BasicHook)Delegate.Combine(PromptuHookManager.CurrentProfileChanged, d);
                    }
                    else
                    {
                        PromptuHookManager.CurrentProfileChanged =
                            (BasicHook)Delegate.Remove(PromptuHookManager.CurrentProfileChanged, d);
                    }

                    break;
                default:
                    break;
            }
        }
    }
}
