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
namespace Net.Http.OData
{
    /// <summary>
    /// A class which is used to return OData content.
    /// </summary>
    public sealed class ODataResponseContent
    {
        /// <summary>
        /// Gets or sets the URI to the metadata.
        /// </summary>
        [Newtonsoft.Json.JsonProperty("@odata.context", NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore, Order = 0)]
#if NETSTANDARD2_0
        [System.Text.Json.Serialization.JsonPropertyName("@odata.context")]
#endif
        public string Context { get; set; }

        /// <summary>
        /// Gets or sets the total result count.
        /// </summary>
        [Newtonsoft.Json.JsonProperty("@odata.count", NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore, Order = 1)]
#if NETSTANDARD2_0
        [System.Text.Json.Serialization.JsonPropertyName("@odata.count")]
#endif
        public int? Count { get; set; }

        /// <summary>
        /// Gets or sets the URI to the next results in a paged response.
        /// </summary>
        [Newtonsoft.Json.JsonProperty("@odata.nextLink", NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore, Order = 2)]
#if NETSTANDARD2_0
        [System.Text.Json.Serialization.JsonPropertyName("@odata.nextLink")]
#endif
        public string NextLink { get; set; }

        /// <summary>
        /// Gets or sets the value to be returned.
        /// </summary>
        [Newtonsoft.Json.JsonProperty("value", NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore, Order = 3)]
        public object Value { get; set; }
    }
}
