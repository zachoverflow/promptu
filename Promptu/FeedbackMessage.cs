//-----------------------------------------------------------------------
// <copyright file="FeedbackMessage.cs" company="Wynamee">
//     Copyright (c) Wynamee. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace ZachJohnson.Promptu
{
    using System;
    using System.Globalization;

    internal class FeedbackMessage
    {
        private string description;
        private FeedbackType typeOfMessage;
        private string location;
        private int start;
        private int length;
        private bool canLocate;

        public FeedbackMessage(string description, FeedbackType typeOfMessage, int start, int length, bool canLocate)
            : this(description, typeOfMessage, null)
        {
            this.start = start;
            this.length = length;
            this.canLocate = canLocate;
        }

        public FeedbackMessage(string description, FeedbackType typeOfMessage) : this(description, typeOfMessage, null) 
        { 
        }

        public FeedbackMessage(string description, FeedbackType typeOfMessage, string location)
        {
            this.description = description;
            this.typeOfMessage = typeOfMessage;
            this.location = location;
        }

        public string Location
        {
            get { return this.location; }
            set { this.location = value; }
        }

        public int Start
        {
            get { return this.start; }
            internal set { this.start = value; }
        }

        public int Length
        {
            get { return this.length; }
        }

        public bool CanLocate
        {
            get { return this.canLocate; }
        }

        public string Description
        {
            get { return this.description; }
        }

        public FeedbackType MessageType
        {
            get { return this.typeOfMessage; }
        }

        public override string ToString()
        {
            string format;
            switch (this.MessageType)
            {
                case FeedbackType.Error:
                    format = Localization.Promptu.FeedbackMessageErrorToStringFormat;
                    break;
                case FeedbackType.Warning:
                    format = Localization.Promptu.FeedbackMessageWarningToStringFormat;
                    break;
                case FeedbackType.Message:
                default:
                    format = Localization.Promptu.FeedbackMessageMessageToStringFormat;
                    break;
            }

            string location;
            if (this.CanLocate)
            {
                location = String.Format(CultureInfo.CurrentCulture, Localization.Promptu.FeedbackMessageLocationFormat, this.start);
            }
            else
            {
                location = String.Empty;
            }

            return String.Format(CultureInfo.CurrentCulture, format, this.description, location);
        }
    }
}
