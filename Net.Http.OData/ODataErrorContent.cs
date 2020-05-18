// -----------------------------------------------------------------------
// <copyright file="ODataErrorContent.cs" company="Project Contributors">
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
using System.Collections.Generic;
using System.Globalization;

namespace Net.Http.OData
{
    /// <summary>
    /// A class representing the OData content of an error.
    /// </summary>
    public sealed class ODataErrorContent
    {
        /// <summary>
        /// Gets or sets the <see cref="ODataError"/> details.
        /// </summary>
        public ODataError Error { get; set; }

        /// <summary>
        /// Creates a new <see cref="ODataErrorContent"/> for the specified values.
        /// </summary>
        /// <param name="code">The error code.</param>
        /// <param name="message">The error message.</param>
        /// <param name="target">The error target.</param>
        /// <param name="details">The details of the error.</param>
        /// <returns>The populated <see cref="ODataErrorContent"/>.</returns>
        public static ODataErrorContent Create(int code, string message, string target = null, IEnumerable<ODataErrorDetail> details = null)
            => new ODataErrorContent { Error = new ODataError { Code = code.ToString(CultureInfo.InvariantCulture), Details = details, Message = message, Target = target } };
    }
}
