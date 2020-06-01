// -----------------------------------------------------------------------
// <copyright file="AllowedLambdaOperators.cs" company="Project Contributors">
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
    /// An enumeration which represents the lambda operators allowed in the $filter query option of an OData query.
    /// </summary>
    [Flags]
    public enum AllowedLambdaOperators
    {
        /// <summary>
        /// Specifies that no lambda operators are allowed in the $filter query option.
        /// </summary>
        None = 0,

        /// <summary>
        /// Specifies that the 'all' lambda operator is allowed in the $filter query option.
        /// </summary>
        All = 1,

        /// <summary>
        /// Specifies that the 'any' lambda operator is allowed in the $filter query option.
        /// </summary>
        Any = 2,

        /// <summary>
        /// Specifies that all lambda operators are allowed in the $filter query option.
        /// </summary>
        AllOperators = All | Any,
    }
}
