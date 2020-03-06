// -----------------------------------------------------------------------
// <copyright file="ODataServiceOptions.cs" company="Project Contributors">
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
using System.Collections.Generic;

namespace Net.Http.OData
{
    /// <summary>
    /// A class which represents the OData service options for an OData service.
    /// </summary>
    public sealed class ODataServiceOptions
    {
        /// <summary>
        /// Initialises a new instance of the <see cref="ODataServiceOptions"/> class.
        /// </summary>
        /// <param name="minVersion">The minimum OData version supported by the service.</param>
        /// <param name="maxVersion">The maximum OData version supported by the service.</param>
        /// <param name="supportedIsolationLevels">The OData-Isolation levels supported by the service.</param>
        /// <param name="supportedMediaTypes">The media types supported by the service.</param>
        /// <exception cref="ArgumentNullException">Thrown if any constructor argument is null.</exception>
        public ODataServiceOptions(
            ODataVersion minVersion,
            ODataVersion maxVersion,
            IReadOnlyCollection<ODataIsolationLevel> supportedIsolationLevels,
            IReadOnlyCollection<string> supportedMediaTypes)
        {
            MaxVersion = maxVersion ?? throw new ArgumentNullException(nameof(maxVersion));
            MinVersion = minVersion ?? throw new ArgumentNullException(nameof(minVersion));

            SupportedFilterFunctions = new[]
            {
                // String functions
                "concat",
                "contains",
                "endswith",
                "indexof",
                "length",
                "startswith",
                "substring",
                "tolower",
                "toupper",
                "trim",

                // Date functions
                "date",
                "day",
                "fractionalseconds",
                "hour",
                "maxdatetime",
                "mindatetime",
                "minute",
                "month",
                "now",
                "second",
                "totaloffsetminutes",
                "year",

                // Math functions
                "ceiling",
                "floor",
                "round",

                // Type functions
                "cast",
                "isof",
            };

            SupportedIsolationLevels = supportedIsolationLevels ?? throw new ArgumentNullException(nameof(supportedIsolationLevels));
            SupportedMediaTypes = supportedMediaTypes ?? throw new ArgumentNullException(nameof(supportedMediaTypes));

            SupportedMetadataLevels = new[]
            {
                ODataMetadataLevel.None,
                ODataMetadataLevel.Minimal,
            };
        }

        /// <summary>
        /// Gets or sets the current OData service options.
        /// </summary>
        public static ODataServiceOptions Current { get; set; }

        /// <summary>
        /// Gets the maximum OData version supported by the service.
        /// </summary>
        public ODataVersion MaxVersion { get; }

        /// <summary>
        /// Gets the minimum OData version supported by the service.
        /// </summary>
        public ODataVersion MinVersion { get; }

        /// <summary>
        /// Gets the functions supported in $filter queries by the service.
        /// </summary>
        public IReadOnlyCollection<string> SupportedFilterFunctions { get; }

        /// <summary>
        /// Gets the OData-Isolation levels supported by the service.
        /// </summary>
        public IReadOnlyCollection<ODataIsolationLevel> SupportedIsolationLevels { get; }

        /// <summary>
        /// Gets the media types supported by the service.
        /// </summary>
        public IReadOnlyCollection<string> SupportedMediaTypes { get; }

        /// <summary>
        /// Gets the odata.metadata levels supported by the service.
        /// </summary>
        public IReadOnlyCollection<ODataMetadataLevel> SupportedMetadataLevels { get; }
    }
}
