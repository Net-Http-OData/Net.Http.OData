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
        /// <returns>The populated <see cref="ODataErrorContent"/>.</returns>
        public static ODataErrorContent Create(int code, string message)
            => Create(code, message, null);

        /// <summary>
        /// Creates a new <see cref="ODataErrorContent"/> for the specified values.
        /// </summary>
        /// <param name="code">The error code.</param>
        /// <param name="message">The error message.</param>
        /// <param name="target">The error target.</param>
        /// <returns>The populated <see cref="ODataErrorContent"/>.</returns>
        public static ODataErrorContent Create(int code, string message, string target)
            => new ODataErrorContent { Error = new ODataError { Code = code.ToString(CultureInfo.InvariantCulture), Message = message, Target = target } };

        /// <summary>
        /// Creates a new <see cref="ODataErrorContent"/> from the specified <see cref="ODataException"/>.
        /// </summary>
        /// <param name="odataException">The ODataException to create the ODataErrorContent from.</param>
        /// <returns>The populated <see cref="ODataErrorContent"/>.</returns>
        public static ODataErrorContent Create(ODataException odataException)
            => Create((int)odataException?.StatusCode, odataException?.Message, odataException?.Target);
    }
}
