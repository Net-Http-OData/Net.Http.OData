// -----------------------------------------------------------------------
// <copyright file="ODataErrorContent.cs" company="Project Contributors">
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
    using System.Globalization;

    /// <summary>
    /// A class representing the OData content of an error.
    /// </summary>
    public sealed class ODataErrorContent
    {
        private ODataErrorContent(ODataError error) => this.Error = error;

        /// <summary>
        /// Gets the <see cref="ODataError"/> details.
        /// </summary>
        [Newtonsoft.Json.JsonProperty("error", NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore, Order = 0)]
        public ODataError Error { get; }

        /// <summary>
        /// Creates a new <see cref="ODataErrorContent"/> for the specified values.
        /// </summary>
        /// <param name="code">The error code.</param>
        /// <param name="message">The error message.</param>
        /// <param name="target">The error target.</param>
        /// <returns>The populated <see cref="ODataErrorContent"/>.</returns>
        public static ODataErrorContent Create(int code, string message, string target)
            => new ODataErrorContent(new ODataError(code.ToString(CultureInfo.InvariantCulture), message, target));
    }
}
