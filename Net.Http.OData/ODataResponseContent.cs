// -----------------------------------------------------------------------
// <copyright file="ODataResponseContent.cs" company="Project Contributors">
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

namespace Net.Http.WebApi.OData
{
    /// <summary>
    /// A class which is used to return OData content.
    /// </summary>
    public sealed class ODataResponseContent
    {
        /// <summary>
        /// Initialises a new instance of the <see cref="ODataResponseContent"/> class.
        /// </summary>
        /// <param name="value">The value to be returned.</param>
        /// <param name="context">The @odata.context for the value.</param>
        public ODataResponseContent(object value, string context)
            : this(value, context, null, null)
        {
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="ODataResponseContent"/> class.
        /// </summary>
        /// <param name="value">The value to be returned.</param>
        /// <param name="context">The @odata.context for the value.</param>
        /// <param name="count">The @odata.count (total result count).</param>
        public ODataResponseContent(object value, string context, int? count)
            : this(value, context, count, null)
        {
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="ODataResponseContent"/> class.
        /// </summary>
        /// <param name="value">The value to be returned.</param>
        /// <param name="context">The @odata.context for the value.</param>
        /// <param name="count">The @odata.count (total result count).</param>
        /// <param name="nextLink">The @odata.nextLink to the next results in a paged response.</param>
        public ODataResponseContent(object value, string context, int? count, string nextLink)
        {
            Value = value;
            Context = context;
            Count = count;
            NextLink = nextLink;
        }

        /// <summary>
        /// Gets the URI to the metadata.
        /// </summary>
        [Newtonsoft.Json.JsonProperty("@odata.context", NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore, Order = 0)]
        public string Context { get; }

        /// <summary>
        /// Gets the total result count.
        /// </summary>
        [Newtonsoft.Json.JsonProperty("@odata.count", NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore, Order = 1)]
        public int? Count { get; }

        /// <summary>
        /// Gets the URI to the next results in a paged response.
        /// </summary>
        [Newtonsoft.Json.JsonProperty("@odata.nextLink", NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore, Order = 2)]
        public string NextLink { get; }

        /// <summary>
        /// Gets the value to be returned.
        /// </summary>
        [Newtonsoft.Json.JsonProperty("value", NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore, Order = 3)]
        public object Value { get; }
    }
}
