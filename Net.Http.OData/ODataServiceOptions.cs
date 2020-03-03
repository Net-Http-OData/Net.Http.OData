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
        public ODataServiceOptions(
            ODataVersion minVersion,
            ODataVersion maxVersion)
        {
            MaxVersion = maxVersion;
            MinVersion = minVersion;

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
                "year",

                // Math functions
                "ceiling",
                "floor",
                "round",

                // Type functions
                "cast",
                "isof",
            };

            SupportedMetadataLevels = new[]
            {
                ODataMetadataLevel.None,
                ODataMetadataLevel.Minimal,
            };
        }

        /// <summary>
        /// Gets the default OData service options with the minimum and maximum versions as supported by the library.
        /// </summary>
        public static ODataServiceOptions Default { get; } = new ODataServiceOptions(ODataVersion.MinVersion, ODataVersion.MaxVersion);

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
        /// Gets the metadata levels supported by the service.
        /// </summary>
        public IReadOnlyCollection<ODataMetadataLevel> SupportedMetadataLevels { get; }
    }
}
