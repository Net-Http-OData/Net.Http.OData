// -----------------------------------------------------------------------
// <copyright file="ODataRequestOptions.cs" company="Project Contributors">
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
using System;

namespace Net.Http.OData
{
    /// <summary>
    /// Contains client specified OData options (or defaults if not specified) for a request.
    /// </summary>
    public sealed class ODataRequestOptions
    {
        /// <summary>
        /// Initialises a new instance of the <see cref="ODataRequestOptions"/> class.
        /// </summary>
        /// <param name="serviceRootUri">The root URI of the OData Service.</param>
        /// <param name="isolationLevel">The OData-Isolation requested by the client, or None if not otherwise specified.</param>
        /// <param name="metadataLevel">The odata.metadata level specified in the ACCEPT header (or $format query option) by the client, or Minimal if not otherwise specified.</param>
        /// <param name="odataVersion">The OData-Version used by the client to generate the request, or the maximum supported by this library if not otherwise specified.</param>
        /// <param name="odataMaxVersion">The OData-MaxVersion requested by the client for the server response, or the latest supported by this library if not otherwise specified.</param>
        /// <exception cref="ArgumentNullException">Thrown if any constructor argument is null.</exception>
        public ODataRequestOptions(
            Uri serviceRootUri,
            ODataIsolationLevel isolationLevel,
            ODataMetadataLevel metadataLevel,
            ODataVersion odataVersion,
            ODataVersion odataMaxVersion)
        {
            ServiceRootUri = serviceRootUri ?? throw new ArgumentNullException(nameof(serviceRootUri));
            IsolationLevel = isolationLevel;
            MetadataLevel = metadataLevel;
            ODataVersion = odataVersion ?? throw new ArgumentNullException(nameof(odataVersion));
            ODataMaxVersion = odataMaxVersion ?? throw new ArgumentNullException(nameof(odataMaxVersion));
        }

        /// <summary>
        /// Gets the OData-Isolation requested by the client, or None if not otherwise specified.
        /// </summary>
        public ODataIsolationLevel IsolationLevel { get; }

        /// <summary>
        /// Gets the odata.metadata level specified in the ACCEPT header by the client, or Minimal if not otherwise specified.
        /// </summary>
        public ODataMetadataLevel MetadataLevel { get; }

        /// <summary>
        /// Gets the OData-MaxVersion requested by the client for the server response, or the latest supported by this library if not otherwise specified.
        /// </summary>
        public ODataVersion ODataMaxVersion { get; }

        /// <summary>
        /// Gets the OData-Version used by the client to generate the request, or the maximum supported by this library if not otherwise specified.
        /// </summary>
        public ODataVersion ODataVersion { get; }

        /// <summary>
        /// Gets the root URI of the OData Service.
        /// </summary>
        public Uri ServiceRootUri { get; }
    }
}
