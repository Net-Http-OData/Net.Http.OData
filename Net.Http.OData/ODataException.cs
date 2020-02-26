// -----------------------------------------------------------------------
// <copyright file="ODataException.cs" company="Project Contributors">
// Copyright Project Contributors
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//    http://www.apache.org/licenses/LICENSE-2.0
//
// </copyright>
// -----------------------------------------------------------------------
using System;
using System.Net;

#if NET45
using System.Runtime.Serialization;
#endif

namespace Net.Http.OData
{
    /// <summary>
    /// An exception which is thrown in relation to an OData request.
    /// </summary>
#if NET45
    [Serializable]
#endif

    public sealed class ODataException : Exception
    {
        /// <summary>
        /// Initialises a new instance of the <see cref="ODataException"/> class.
        /// </summary>
        public ODataException()
        {
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="ODataException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public ODataException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="ODataException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference (Nothing in Visual Basic) if no inner exception is specified.</param>
        public ODataException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="ODataException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="statusCode">The HTTP status code that describes the error.</param>
        public ODataException(string message, HttpStatusCode statusCode)
            : this(message, statusCode, null)
        {
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="ODataException" /> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="statusCode">The HTTP status code that describes the error.</param>
        /// <param name="target">The target of the exception.</param>
        public ODataException(string message, HttpStatusCode statusCode, string target)
            : base(message)
        {
            StatusCode = statusCode;
            Target = target;
        }

#if NET45
        private ODataException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            StatusCode = (HttpStatusCode)Enum.Parse(typeof(HttpStatusCode), info.GetString("StatusCode"));
            Target = info.GetString("Target");
        }
#endif

        /// <summary>
        /// Gets or sets the HTTP status code that describes the error.
        /// </summary>
        public HttpStatusCode StatusCode { get; set; } = HttpStatusCode.InternalServerError;

        /// <summary>
        /// Gets or sets the target of the exception.
        /// </summary>
        public string Target { get; set; }

#if NET45
        /// <inheritdoc/>
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info != null)
            {
                info.AddValue("StatusCode", StatusCode.ToString());
                info.AddValue("Target", Target);
            }

            base.GetObjectData(info, context);
        }
#endif
    }
}
