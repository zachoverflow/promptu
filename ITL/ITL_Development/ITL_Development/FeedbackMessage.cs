//-----------------------------------------------------------------------
// <copyright file="FeedbackMessage.cs" company="Wynamee">
//     Copyright (c) Wynamee. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace ZachJohnson.Promptu.Itl
{
    using System;

    public class FeedbackMessage
    {
        private string description;
        private FeedbackType typeOfMessage;
        private object associatedObject;
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

        public FeedbackMessage(string description, FeedbackType typeOfMessage, object associatedObject)
        {
            this.description = description;
            this.typeOfMessage = typeOfMessage;
            this.associatedObject = associatedObject;
        }

        public object AssociatedObject
        {
            get { return this.associatedObject; }
        }

        public int Start
        {
            get { return this.start; }
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
    }
}
