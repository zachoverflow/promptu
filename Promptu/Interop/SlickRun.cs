//-----------------------------------------------------------------------
// <copyright file="SlickRun.cs" company="ZachJohnson">
//     Copyright (c) Zach Johnson. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace ZachJohnson.Promptu.Interop
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using ZachJohnson.Promptu.Collections;
    using ZachJohnson.Promptu.UserModel;
    using ZachJohnson.Promptu.UserModel.Collections;

    internal static class SlickRun
    {
        public static CommandCollection ImportMagicWords(string path, IdGenerator commandIdGenerator)
        {
            if (path == null)
            {
                throw new ArgumentNullException("path");
            }
            else if (commandIdGenerator == null)
            {
                throw new ArgumentNullException("commandIdGenerator");
            }

            TrieList alreadyImportedAliases = new TrieList(SortMode.DecendingFromLastAdded);
            CommandCollection importedCommands = new CommandCollection();
            string[] lines = File.ReadAllLines(path);

            for (int i = 0; i < lines.Length; i++)
            {
                string line = lines[i].Trim();
                if (line.StartsWith("[") && line.EndsWith("]"))
                {
                    string name = line.Substring(1, line.Length - 2);
                    string executes = null;
                    string startupPath = String.Empty;
                    string arguments = String.Empty;
                    string notes = String.Empty;
                    ProcessWindowStyle startupWindowState = ProcessWindowStyle.Normal;
                    bool runAsAdministrator = false;

                    i++;
                    for (int j = i; i < lines.Length; i++)
                    {
                        line = lines[i].Trim();
                        if (line.StartsWith("[") && line.EndsWith("]"))
                        {
                            i--;
                            break;
                        }

                        int indexOfEqualSign = line.IndexOf("=");
                        if (indexOfEqualSign == -1)
                        {
                            continue;
                        }

                        string key = line.Substring(0, indexOfEqualSign);
                        string value = line.Substring(indexOfEqualSign + 1, line.Length - indexOfEqualSign - 1);
                        value = value.Trim();
                        if (value.StartsWith("\"") && value.EndsWith("\""))
                        {
                            value = value.Substring(1, value.Length - 2);
                        }

                        switch (key.ToUpperInvariant())
                        {
                            case "FILENAME":
                                executes = value;
                                break;
                            case "PATH":
                                startupPath = value;
                                break;
                            case "PARAMS":
                                arguments = value;
                                break;
                            case "NOTES":
                                notes = value;
                                break;
                            case "STARTMODE":
                                switch (value.ToUpperInvariant())
                                {
                                    case "5":
                                        startupWindowState = ProcessWindowStyle.Normal;
                                        break;
                                    case "7":
                                        startupWindowState = ProcessWindowStyle.Minimized;
                                        break;
                                    case "3":
                                        startupWindowState = ProcessWindowStyle.Maximized;
                                        break;
                                    default:
                                        break;
                                }

                                break;
                            case "USERUNAS":
                                if (value.ToUpperInvariant() == "-1")
                                {
                                    runAsAdministrator = true;
                                }
                                else
                                {
                                    runAsAdministrator = false;
                                }

                                break;
                            default:
                                break;
                        }
                    }

                    if (executes == null)
                    {
                        continue;
                    }

                    name = Command.CleanName(name).ToLowerInvariant();

                    if (name == null)
                    {
                        continue;
                    }

                    executes = executes.Replace("$W$", "<!n!>");
                    arguments = arguments.Replace("$W$", "<!n!>");
                    executes = executes.Replace("$I$", "<!n!>");
                    arguments = arguments.Replace("$I$", "<!n!>");

                    switch (executes.Trim().ToUpperInvariant())
                    {
                        case "@MULTI@":
                            arguments = arguments.Replace('@', '&');
                            break;
                        case "@SCREENSAVE@":
                            executes = "@SCREENSAVER@";
                            break;
                        default:
                            break;
                    }

                    Command newCommand = new Command(
                        name, 
                        executes, 
                        arguments, 
                        runAsAdministrator, 
                        startupWindowState, 
                        notes, 
                        startupPath, 
                        false, 
                        Command.DefaultUseExecutionDirectoryAsStartupDirectory, 
                        commandIdGenerator.GenerateId(),
                        null);

                    string[] aliases = newCommand.GetAliases();

                    if (!alreadyImportedAliases.ContainsAny(aliases, CaseSensitivity.Insensitive))
                    {
                        importedCommands.Add(newCommand);
                        alreadyImportedAliases.AddRange(aliases);
                    }
                }
            }

            return importedCommands;
        }
    }
}
