﻿// -----------------------------------------------------------------------
// <copyright file="QueryOption.cs" company="Project Contributors">
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

namespace Net.Http.OData.Query
{
    /// <summary>
    /// The base class for an OData System Query Option.
    /// </summary>
    [System.Diagnostics.DebuggerDisplay("{RawValue}")]
    public abstract class QueryOption
    {
        /// <summary>
        /// Initialises a new instance of the <see cref="QueryOption"/> class.
        /// </summary>
        /// <param name="rawValue">The raw value.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="rawValue"/> is null.</exception>
        protected QueryOption(string rawValue)
            => RawValue = rawValue ?? throw new ArgumentNullException(nameof(rawValue));

        /// <summary>
        /// Gets the raw request value.
        /// </summary>
        public string RawValue { get; }
    }
}
