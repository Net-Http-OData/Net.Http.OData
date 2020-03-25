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
    /// A class which is used to return the OData content to an OData request.
    /// </summary>
    /// <remarks>
    /// <![CDATA[http://docs.oasis-open.org/odata/odata-json-format/v4.0/errata03/os/odata-json-format-v4.0-errata03-os-complete.html#_Toc453766627]]>
    /// </remarks>
    public sealed class ODataResponseContent
    {
        /// <summary>
        /// Gets or sets the context URI for the payload, unless 'odata.metadata=none' is specified in the request.
        /// </summary>
        [Newtonsoft.Json.JsonProperty("@odata.context", NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore, Order = 0)]
#if NETSTANDARD2_0
        [System.Text.Json.Serialization.JsonPropertyName("@odata.context")]
#endif
        public string Context { get; set; }

        /// <summary>
        /// Gets or sets the total count of members in the collection represented by the request when '$count=true' is specified.
        /// </summary>
        [Newtonsoft.Json.JsonProperty("@odata.count", NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore, Order = 1)]
#if NETSTANDARD2_0
        [System.Text.Json.Serialization.JsonPropertyName("@odata.count")]
#endif
        public long? Count { get; set; }

        /// <summary>
        /// Gets or sets the URL that allows retrieving the next subset of the requested collection, its presence indicates that a response is only a subset of the requested collection.
        /// </summary>
        [Newtonsoft.Json.JsonProperty("@odata.nextLink", NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore, Order = 2)]
#if NETSTANDARD2_0
        [System.Text.Json.Serialization.JsonPropertyName("@odata.nextLink")]
#endif
        public string NextLink { get; set; }

        /// <summary>
        /// Gets or sets the payload containing the result of the OData request.
        /// </summary>
        [Newtonsoft.Json.JsonProperty(NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore, Order = 3)]
        public object Value { get; set; }
    }
}
