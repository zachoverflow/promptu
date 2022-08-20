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
