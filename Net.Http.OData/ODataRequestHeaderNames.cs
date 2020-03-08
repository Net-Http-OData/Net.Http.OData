// -----------------------------------------------------------------------
// <copyright file="ODataRequestHeaderNames.cs" company="Project Contributors">
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
    /// The header names for OData HTTP Request Headers.
    /// </summary>
    public static class ODataRequestHeaderNames
    {
        /// <summary>
        /// The OData isolation header.
        /// </summary>
        public const string ODataIsolation = "OData-Isolation";

        /// <summary>
        /// The OData max version header which contains the maximum OData version acceptable by the client in the response.
        /// </summary>
        /// <remarks>
        /// Clients SHOULD specify an OData-MaxVersion request header.
        /// If specified the service MUST generate a response with an OData-Version less than or equal to the specified OData-MaxVersion.
        /// If OData-MaxVersion is not specified, then the service SHOULD interpret the request as having an OData-MaxVersion equal to the maximum version supported by the service.
        /// </remarks>
        public const string ODataMaxVersion = "OData-MaxVersion";

        /// <summary>
        /// The OData version header which contains the OData version used by the client to generate the request.
        /// </summary>
        /// <remarks>
        /// OData clients SHOULD use the OData-Version header on a request to specify the version of the protocol used to generate the request.
        /// If present on a request, the service MUST interpret the request according to the rules defined in the specified version of the protocol, or fail the request with a 4xx response code.
        /// If not specified in a request, the service MUST assume the request is generated using the minimum of the OData-MaxVersion, if specified, and the maximum version of the protocol that the service understands.
        /// </remarks>
        public const string ODataVersion = "OData-Version";
    }
}
