using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace ZachJohnson.Promptu
{
    internal static class Log
    {
        private static string log = "Log:";
        private static string logPath = Application.StartupPath + "\\log.txt";

        static Log()
        {
            if (File.Exists(logPath))
            {
                log = File.ReadAllText(logPath);
            }
        }

        public static void RecordMessage(string message)
        {
            log += System.Environment.NewLine + System.DateTime.Now.ToString() + ": " + message;
            File.WriteAllText(logPath, log);
        }
    }
}
