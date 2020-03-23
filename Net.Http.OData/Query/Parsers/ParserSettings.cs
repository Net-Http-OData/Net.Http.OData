// -----------------------------------------------------------------------
// <copyright file="ParserSettings.cs" company="Project Contributors">
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
using System.Globalization;

namespace Net.Http.OData.Query.Parsers
{
    /// <summary>
    /// A class which contains settings used by the parsers.
    /// </summary>
    public static class ParserSettings
    {
        internal const string ODataDateFormat = "yyyy-MM-dd";

        /// <summary>
        /// Gets or sets the <see cref="DateTimeStyles"/> to use for parsing <see cref="System.DateTimeOffset"/>, defaults to <see cref="DateTimeStyles.AssumeUniversal"/>.
        /// </summary>
        public static DateTimeStyles DateTimeStyles { get; set; } = DateTimeStyles.AssumeUniversal;

        internal static CultureInfo CultureInfo { get; } = CultureInfo.InvariantCulture;
    }
}
