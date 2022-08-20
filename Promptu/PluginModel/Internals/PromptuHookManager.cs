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
    using System.Globalization;
    using ZachJohnson.Promptu.PluginModel.Hooks;

    internal static class PromptuHookManager
    {
        internal static CommandExecutingHook CommandExecuting { get; set; }

        internal static CommandFullyResolvedExecutingHook CommandFullyResolvedExecuting { get; set; }

        internal static BasicHook ShowingSuggestionProvider { get; set; }

        internal static BasicHook CurrentProfileChanged { get; set; }

        public static HookAction RaiseCommandFullyResolvedExecuting(
            ExecuteMode mode,
            string resolvedTarget,
            string resolvedArguments,
            string resolvedWorkingDirectory)
        {
            return RaiseHookEvent(
                CommandFullyResolvedExecuting, 
                mode, 
                resolvedTarget, 
                resolvedArguments, 
                resolvedWorkingDirectory);
        }

        public static HookAction RaiseCommandExecuting(string textToExecute, ExecuteMode mode)
        {
            return RaiseHookEvent(CommandExecuting, textToExecute, mode);
        }

        public static HookAction RaiseShowingSuggestionProvider()
        {
            return RaiseHookEvent(ShowingSuggestionProvider);
        }

        public static HookAction RaiseCurrentProfileChanged()
        {
            return RaiseHookEvent(CurrentProfileChanged);
        }

        private static HookAction RaiseHookEvent(Delegate hook, params object[] args)
        {
            if (hook == null)
            {
                return HookAction.Continue;
            }
            
            foreach (Delegate d in hook.GetInvocationList())
            {
                try
                {
                    HookAction result = (HookAction)d.DynamicInvoke(args);

                    if (result == HookAction.Return)
                    {
                        return result;
                    }
                }
                catch (Exception ex)
                {
                    string id = "unknown plugin";

                    foreach (PromptuPlugin plugin in InternalGlobals.AvailablePlugins)
                    {
                        if (!plugin.HasLoaded || !plugin.Enabled)
                        {
                            continue;
                        }

                        if (plugin.EntryPoint.Hooks.Contains(d))
                        {
                            id = plugin.Id;
                            plugin.EntryPoint.Hooks.Detach(d);
                            break;
                        }
                    }

                    ErrorConsole.WriteLine(id, "Unhandled exception in plugin via a hook.  Details logged in exceptions.log");
                    ExceptionLogger.LogException(ex, String.Format(CultureInfo.InvariantCulture, "unknown, \"{0}\" at fault", id));
                }
            }

            return HookAction.Continue;
        }
    }
}
