﻿// -----------------------------------------------------------------------
// <copyright file="SearchQueryOption.cs" company="Project Contributors">
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
namespace Net.Http.OData.Query
{
    /// <summary>
    /// A class containing deserialised values from the $search query option.
    /// </summary>
    [System.Diagnostics.DebuggerDisplay("{RawValue}")]
    public sealed class SearchQueryOption : QueryOption
    {
        /// <summary>
        /// Initialises a new instance of the <see cref="SearchQueryOption"/> class.
        /// </summary>
        /// <param name="rawValue">The raw request value.</param>
        internal SearchQueryOption(string rawValue)
            : base(rawValue) => Expression = rawValue.SubstringAfter('=');

        /// <summary>
        /// Gets the search expression.
        /// </summary>
        public string Expression { get; }
    }
}
