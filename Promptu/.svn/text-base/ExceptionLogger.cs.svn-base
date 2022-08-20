//-----------------------------------------------------------------------
// <copyright file="ExceptionLogger.cs" company="Wynamee">
//     Copyright (c) Wynamee. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace ZachJohnson.Promptu
{
    using System;
    using System.Collections;
    using System.IO;
    using System.Reflection;
    using System.Threading;
    using System.Windows.Forms;
    using System.Xml;
    using System.Diagnostics;
    using System.Text;

    internal static class ExceptionLogger
    {
        internal static void LogCurrentThreadStack(string effect)
        {
            XmlDocument document = new XmlDocument();
            XmlNode threadSnapshot = document.CreateElement("StackTrace");

//            foreach (ProcessThread thread in Process.GetCurrentProcess().Threads)
//            {
//                //Thread.Get
//#pragma warning disable 0618

//                bool resume = false;

//                if (thread != Thread.CurrentThread)
//                {
//                    thread.Suspend();
//                    resume = true;
//                }

                StackTrace threadStackTrace = new StackTrace(true);

                //if (resume)
                //{
                //    thread.Resume();
                //}

                //XmlNode threadNode = document.CreateElement("Thread");

                //threadStackTrace.ToString();

                //StringBuilder callstack = new StringBuilder();

                //foreach (StackFrame frame in threadStackTrace.GetFrames())
                //{
                //    callstack.AppendLine(frame.ToString());
                //}

                //XmlUtilities.AppendAttribute(threadNode, "id", thread.ManagedThreadId);
                XmlUtilities.AppendAttribute(threadSnapshot, "callstack", threadStackTrace);

//#pragma warning restore 0618
            //}

            AppendXmlToLog(threadSnapshot, true, effect);
        }

        internal static void LogException(Exception ex, string effect)
        {
            XmlDocument document = new XmlDocument();
            XmlNode node = SerializeException(ex, document);

            AppendXmlToLog(node, true, effect);
        }

        private static void AppendXmlToLog(XmlNode node, bool addAdditionalInfo, string effect)
        {
            if (addAdditionalInfo)
            {
                XmlNode additionalInfo = node.OwnerDocument.CreateElement("AdditionalInfo");

                //additionalInfo.AppendChild(XmlUtilities.CreateNode("MachineName", Environment.MachineName, document));
                additionalInfo.AppendChild(XmlUtilities.CreateNode("OSVersion", Environment.OSVersion, node.OwnerDocument));
                additionalInfo.AppendChild(XmlUtilities.CreateNode("ProcessorCount", Environment.ProcessorCount, node.OwnerDocument));
                additionalInfo.AppendChild(XmlUtilities.CreateNode("UserInteractive", Environment.UserInteractive, node.OwnerDocument));
                additionalInfo.AppendChild(XmlUtilities.CreateNode("CLR_Version", Environment.Version, node.OwnerDocument));
                additionalInfo.AppendChild(XmlUtilities.CreateNode("WorkingSet", Environment.WorkingSet, node.OwnerDocument));
                additionalInfo.AppendChild(XmlUtilities.CreateNode("TimeStamp", DateTime.Now.ToString(), node.OwnerDocument));
                additionalInfo.AppendChild(XmlUtilities.CreateNode("ManagedThreadId", Thread.CurrentThread.ManagedThreadId, node.OwnerDocument));
                additionalInfo.AppendChild(XmlUtilities.CreateNode("AppDomain", AppDomain.CurrentDomain.ToString().Replace(System.Environment.NewLine, " \\n "), node.OwnerDocument));

                additionalInfo.AppendChild(XmlUtilities.CreateNode("Effect", effect, node.OwnerDocument));

                node.AppendChild(additionalInfo);
            }

            string path = Application.StartupPath + "\\exceptions.log";

            if (File.Exists(path))
            {
                try
                {
                    node.OwnerDocument.LoadXml(File.ReadAllText(path));
                    foreach (XmlNode logNode in node.OwnerDocument.ChildNodes)
                    {
                        if (logNode.Name.ToUpperInvariant() == "EXCEPTIONLOG")
                        {
                            logNode.AppendChild(node);
                            node.OwnerDocument.Save(path);
                            return;
                        }
                    }
                }
                catch (XmlException)
                {
                }
                catch (IOException)
                {
                }

                node.OwnerDocument.RemoveAll();
            }

            node.OwnerDocument.AppendChild(node.OwnerDocument.CreateProcessingInstruction("xml", "version=\"1.0\""));
            XmlNode exceptionLog = node.OwnerDocument.CreateElement("ExceptionLog");

            exceptionLog.AppendChild(node);

            node.OwnerDocument.AppendChild(exceptionLog);

            try
            {
                node.OwnerDocument.Save(path);
            }
            catch (IOException)
            {
            }
        }

        private static XmlNode SerializeException(Exception ex, XmlDocument document)
        {
            Type exceptionType = ex.GetType();
            XmlNode node = document.CreateElement("Exception");
            XmlNode properties = document.CreateElement("Properties");

            node.Attributes.Append(XmlUtilities.CreateAttribute("type", exceptionType.ToString(), document));

            foreach (PropertyInfo info in exceptionType.GetProperties())
            {
                object value = info.GetValue(ex, null);
                XmlNode propertyNode = document.CreateElement("Property");

                if (value is Exception)
                {
                    propertyNode = SerializeException(value as Exception, document);
                }
                else if (value is IEnumerable && !(value is string))
                {
                    foreach (object innerValue in (IEnumerable)value)
                    {
                        if (value == null)
                        {
                            value = String.Empty;
                        }

                        propertyNode.AppendChild(XmlUtilities.CreateNode("Value", innerValue.ToString().Trim(), document));
                        properties.AppendChild(propertyNode);
                    }
                }
                else
                {
                    if (value == null)
                    {
                        value = String.Empty;
                    }

                    propertyNode.Attributes.Append(XmlUtilities.CreateAttribute("value", value.ToString().Trim(), document));
                }

                propertyNode.Attributes.Prepend(XmlUtilities.CreateAttribute("name", info.Name, document));

                properties.AppendChild(propertyNode);
            }

            node.AppendChild(properties);

            return node;
        }
    }
}
