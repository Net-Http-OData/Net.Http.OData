// -----------------------------------------------------------------------
// <copyright file="ODataResponseHeaderNames.cs" company="Project Contributors">
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
    /// The header names for OData HTTP Response Headers.
    /// </summary>
    public static class ODataResponseHeaderNames
    {
        /// <summary>
        /// The OData version header which contains the OData version used to generate the response.
        /// </summary>
        /// <remarks>OData services MUST include the OData-Version header on a response to specify the version of the protocol used to generate the response.</remarks>
        public const string ODataVersion = "OData-Version";
    }
}
