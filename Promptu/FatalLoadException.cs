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

namespace ZachJohnson.Promptu
{
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    /// The exception that is thrown when a method fails to load a type from XML.
    /// </summary>
    [Serializable]
    public class LoadException : Exception, ISerializable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LoadException"/> class.
        /// </summary>
        public LoadException() : this(String.Empty)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LoadException"/> class.
        /// </summary>
        /// <param name="messageFormat">The message format.</param>
        /// <param name="args">The args.</param>
        public LoadException(IFormatProvider formatProvider, string messageFormat, params object[] args)
            : this(String.Format(formatProvider, messageFormat, args))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LoadException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public LoadException(string message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LoadException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="innerException">The inner exception.</param>
        public LoadException(string message, Exception innerException) 
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LoadException"/> class.
        /// </summary>
        /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo"/> that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext"/> that contains contextual information about the source or destination.</param>
        /// <exception cref="T:System.ArgumentNullException">
        /// The <paramref name="info"/> parameter is null.
        /// </exception>
        /// <exception cref="T:System.Runtime.Serialization.SerializationException">
        /// The class name is null or <see cref="P:System.Exception.HResult"/> is zero (0).
        /// </exception>
        protected LoadException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
