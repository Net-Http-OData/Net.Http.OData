// -----------------------------------------------------------------------
// <copyright file="ODataQueryOptionsExtensions.cs" company="Project Contributors">
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

namespace Net.Http.OData.Query.Validators
{
    /// <summary>
    /// Extension methods for validating the <see cref="ODataQueryOptions"/>.
    /// </summary>
    public static class ODataQueryOptionsExtensions
    {
        /// <summary>
        /// Validates the specified query options.
        /// </summary>
        /// <param name="queryOptions">The query options.</param>
        /// <param name="validationSettings">The validation settings.</param>
        public static void Validate(this ODataQueryOptions queryOptions, ODataValidationSettings validationSettings)
        {
            if (queryOptions is null)
            {
                throw new ArgumentNullException(nameof(queryOptions));
            }

            if (validationSettings is null)
            {
                throw new ArgumentNullException(nameof(validationSettings));
            }

            CountQueryOptionValidator.Validate(queryOptions, validationSettings);
            ExpandQueryOptionValidator.Validate(queryOptions, validationSettings);
            FilterQueryOptionValidator.Validate(queryOptions, validationSettings);
            FormatQueryOptionValidator.Validate(queryOptions, validationSettings);
            OrderByQueryOptionValidator.Validate(queryOptions, validationSettings);
            SearchQueryOptionValidator.Validate(queryOptions, validationSettings);
            SelectQueryOptionValidator.Validate(queryOptions, validationSettings);
            SkipQueryOptionValidator.Validate(queryOptions, validationSettings);
            SkipTokenQueryOptionValidator.Validate(queryOptions, validationSettings);
            TopQueryOptionValidator.Validate(queryOptions, validationSettings);
        }
    }
}
