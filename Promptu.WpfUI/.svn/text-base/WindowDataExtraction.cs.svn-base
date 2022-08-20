using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;

namespace ZachJohnson.Promptu.WpfUI
{
    internal class WindowDataExtraction
    {
        internal const string XmlAlias = "EXTRACTION";
        private string processName;
        private Regex regex;
        private string className;
        private int returnCaptureNumber;

        public WindowDataExtraction(string processName, string className, string regex, int returnCaptureNumber)
        {
            if (processName == null)
            {
                throw new ArgumentNullException("processName");
            }
            else if (className == null)
            {
                throw new ArgumentNullException("className");
            }
            else if (regex == null)
            {
                throw new ArgumentNullException("regex");
            }
            else if (returnCaptureNumber < 0)
            {
                throw new ArgumentOutOfRangeException("'returnCaptureNumber cannot be less than zero.");
            }

            this.processName = processName;
            this.regex = new Regex(regex);
            this.className = className;
            this.returnCaptureNumber = returnCaptureNumber;
        }

        public string ProcessName
        {
            get { return this.processName; }
        }

        public string ClassName
        {
            get { return this.className; }
        }

        public string Extract(string text)
        {
            if (this.returnCaptureNumber == 0)
            {
                return text;
            }

            Match m = this.regex.Match(text);
            if (this.returnCaptureNumber < m.Groups.Count)
            {
                return m.Groups[(int)this.returnCaptureNumber].Value;
            }

            return null;
        }

        public static WindowDataExtraction FromXml(XmlNode node)
        {
            if (node.Name.ToUpperInvariant() != XmlAlias)
            {
                throw new ArgumentException("The node is not named " + XmlAlias + ".");
            }

            string process = null;
            string regex = null;
            string className = null;
            int returnCaptureNumber = 0;

            foreach (XmlAttribute attribute in node.Attributes)
            {
                switch (attribute.Name.ToUpperInvariant())
                {
                    case "PROCESS":
                        process = attribute.Value;
                        break;
                    case "REGEX":
                        regex = attribute.Value;
                        break;
                    case "CLASSNAME":
                        className = attribute.Value;
                        break;
                    case "RETURNCAPTURE":
                        returnCaptureNumber = WpfUtilities.TryParseInt32(attribute.Value, returnCaptureNumber);
                        //try
                        //{
                        //    returnCaptureNumber = Convert.ToInt32(attribute.Value);
                        //}
                        //catch (FormatException)
                        //{
                        //}
                        //catch (OverflowException)
                        //{
                        //}

                        break;
                    default:
                        break;
                }
            }

            if (process == null)
            {
                throw new LoadException("Missing 'process' attribute.");
            }
            else if (regex == null)
            {
                throw new LoadException("Missing 'regex' attribute.");
            }
            else if (className == null)
            {
                throw new LoadException("Missing 'className' attribute.");
            }

            try
            {
                return new WindowDataExtraction(process, className, regex, returnCaptureNumber);
            }
            catch (ArgumentException ex)
            {
                throw new LoadException(ex.Message);
            }
        }
    }
}
