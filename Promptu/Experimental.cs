//using System;
//using System.Collections.Generic;
//using System.Text;
//using System.ComponentModel;
//using ZachJohnson.Promptu.UserModel;
//using System.IO;
//using System.IO.Extensions;
//using System.Drawing;
//using System.Net;

//namespace ZachJohnson.Promptu
//{
//    internal static class Experimental
//    {
//        //public static void Test()
//        //{
//        //    BackgroundWorker worker = new BackgroundWorker();
//        //    worker.DoWork += CacheIcons;
//        //    worker.RunWorkerAsync();
//        //}

//        //private static void CacheIcons(object sender, DoWorkEventArgs e)
//        //{
//        //    foreach (List list in InternalGlobals.CurrentProfile.Lists)
//        //    {
//        //        foreach (Command command in list.Commands)
//        //        {
//        //            command.UpdateCacheIcon(list);
//        //            //if (command.TakesParameterCountOf(0))
//        //            //{
//        //            //    string executionPath = command.GetSubstitutedExecutionPath(new ExecutionData(new string[0], list, InternalGlobals.CurrentProfile.Lists));
//        //            //    CacheIcon(executionPath, command.GetIconId(list));
//        //            //}
//        //        }
//        //    }
//        //}

//        //private static void CacheIcon(string command, string id)
//        //{
//        //    //if (command != "www.facebook.com")
//        //    //{
//        //    //    return;
//        //    //}

//        //    if (command.StartsWith("www", StringComparison.InvariantCultureIgnoreCase))
//        //    {
//        //        command = "http://" + command;
//        //    }

//        //    //System.Diagnostics.Debug.WriteLine(command);
//        //    Uri uri;
//        //    if (Uri.TryCreate(command, UriKind.Absolute, out uri))
//        //    {
//        //        if (uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps)
//        //        {
//        //            UriBuilder builder = new UriBuilder();
//        //            builder.Host = uri.Host;
//        //            builder.Scheme = uri.Scheme;
//        //            builder.Path = "favicon.ico";

//        //            PromptuWebClient client = Updater.GetDefaultWebClient();

//        //            FileSystemFile cacheFile = InternalGlobals.CurrentProfile.IconCacheDirectory + (id + ".png");

//        //            DateTime? lastModified = null;

//        //            try
//        //            {
//        //                lastModified = File.GetLastWriteTime(cacheFile);
//        //            }
//        //            catch (UnauthorizedAccessException)
//        //            {
//        //            }
//        //            catch (ArgumentException)
//        //            {
//        //            }
//        //            catch (IOException)
//        //            {
//        //            }
//        //            catch (NotSupportedException)
//        //            {
//        //            }

//        //            try
//        //            {
//        //                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(builder.Uri);
//        //                request.UserAgent = client.UserAgent;
//        //                request.IfModifiedSince = lastModified ?? DateTime.MinValue;

//        //                HttpWebResponse response = (HttpWebResponse)request.GetResponse();

//        //                using (Stream s = response.GetResponseStream())
//        //                using (MemoryStream localStream = new MemoryStream())
//        //                {
//        //                    s.TransferTo(localStream);
//        //                    try
//        //                    {
//        //                        Icon icon = new Icon(localStream);
//        //                        icon.ToBitmap().Save(cacheFile);
//        //                    }
//        //                    catch (ArgumentException)
//        //                    {
//        //                        try
//        //                        {
//        //                            localStream.Position = 0;
//        //                            Bitmap bitmap = (Bitmap)Bitmap.FromStream(localStream);
//        //                            bitmap.Save(cacheFile);
//        //                        }
//        //                        catch (ArgumentException)
//        //                        {
//        //                        }
//        //                    }
//        //                }
//        //            }
//        //            catch (WebException)
//        //            {
//        //            }
//        //            catch (Exception)
//        //            {
//        //            }
//        //        }
//        //    }

//        //    InternalGlobals.CurrentProfile.IconCacheDirectory.CreateIfDoesNotExist();
//        //    if (File.Exists(command))
//        //    {
//        //        Icon icon = InternalGlobals.GuiManager.ToolkitHost.ExtractFileIcon(command, IconSize.Large);
//        //        if (icon == null)
//        //        {
//        //            return;
//        //        }

//        //        icon.ToBitmap().Save(InternalGlobals.CurrentProfile.IconCacheDirectory + (id + ".png"));

//        //        //using (FileStream stream = new FileStream(iconCacheDirectory + (id + ".ico"), FileMode.Create))
//        //        //{
//        //        //    icon.ToBitmap.Sa.Save(stream);
//        //        //}
//        //    }
//        //    else if (Directory.Exists(command))
//        //    {
//        //        Icon icon = InternalGlobals.GuiManager.ToolkitHost.ExtractDirectoryIcon(command, IconSize.Large);
//        //        if (icon == null)
//        //        {
//        //            return;
//        //        }

//        //        icon.ToBitmap().Save(InternalGlobals.CurrentProfile.IconCacheDirectory + (id + ".png"));

//        //        //using (FileStream stream = new FileStream(iconCacheDirectory + (id + ".ico"), FileMode.Create))
//        //        //{
//        //        //    icon.Save(stream);
//        //        //}
//        //    }
//        //}
//    }
//}
