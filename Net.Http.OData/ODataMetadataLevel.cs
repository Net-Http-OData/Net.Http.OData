// -----------------------------------------------------------------------
// <copyright file="ODataMetadataLevel.cs" company="Project Contributors">
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
    /// The different levels of metadata which should be included in the response.
    /// </summary>
    /// <remarks>
    /// <![CDATA[http://docs.oasis-open.org/odata/odata-json-format/v4.0/errata03/os/odata-json-format-v4.0-errata03-os-complete.html#_Toc453766617]]>
    /// </remarks>
    public enum ODataMetadataLevel
    {
        /// <summary>
        /// No metadata should be included in the response.
        /// </summary>
        /// <remarks>
        /// Indicates that the service SHOULD omit control information other than odata.nextLink and odata.count. These annotations MUST continue to be included, as applicable, even in the odata.metadata=none case.
        /// </remarks>
        None = 0,

        /// <summary>
        /// The minimal metadata should be included in the response.
        /// </summary>
        /// <remarks>
        /// Indicates that the service SHOULD remove computable control information from the payload wherever possible.
        /// </remarks>
        Minimal = 1,

        /// <summary>
        /// The full metadata should be included in the response.
        /// </summary>
        /// <remarks>
        /// Indicates that the service MUST include all control information explicitly in the payload.
        /// </remarks>
        Full = 2,
    }
}
