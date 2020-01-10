// -----------------------------------------------------------------------
// <copyright file="ODataError.cs" company="Project Contributors">
// Copyright 2012 - 2020 Project Contributors
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//    http://www.apache.org/licenses/LICENSE-2.0
//
// </copyright>
// -----------------------------------------------------------------------
namespace Net.Http.OData
{
    /// <summary>
    /// A class representing the OData details of an error.
    /// </summary>
    public sealed class ODataError
    {
        /// <summary>
        /// Initialises a new instance of the <see cref="ODataError"/> class.
        /// </summary>
        /// <param name="code">The error code.</param>
        /// <param name="message">The error message.</param>
        /// <param name="target">The error target.</param>
        public ODataError(string code, string message, string target)
        {
            Code = code;
            Message = message;
            Target = target;
        }

        /// <summary>
        /// Gets the error code.
        /// </summary>
        [Newtonsoft.Json.JsonProperty("code", NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore, Order = 0)]
        public string Code { get; }

        /// <summary>
        /// Gets the error message.
        /// </summary>
        [Newtonsoft.Json.JsonProperty("message", NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore, Order = 1)]
        public string Message { get; }

        /// <summary>
        /// Gets the error target.
        /// </summary>
        [Newtonsoft.Json.JsonProperty("target", NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore, Order = 2)]
        public string Target { get; }
    }
}
