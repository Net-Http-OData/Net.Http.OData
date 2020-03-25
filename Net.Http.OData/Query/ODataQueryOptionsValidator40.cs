// -----------------------------------------------------------------------
// <copyright file="ODataQueryOptionsValidator40.cs" company="Project Contributors">
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
using Net.Http.OData.Query.Validators;

namespace Net.Http.OData.Query
{
    /// <summary>
    /// An implementation of <see cref="IODataQueryOptionsValidator"/> for OData 4.0.
    /// </summary>
    /// <remarks>
    /// The current implementation is temporary to re-use the existing static validators, once OData 4.01 is implemented this will need re-writing.
    /// </remarks>
    internal sealed class ODataQueryOptionsValidator40 : IODataQueryOptionsValidator
    {
        /// <summary>
        /// Validates the specified query options using the specified validation settings.
        /// </summary>
        /// <param name="queryOptions">The query options to validate.</param>
        /// <param name="validationSettings">The validation settings to configure the validation.</param>
        public void Validate(ODataQueryOptions queryOptions, ODataValidationSettings validationSettings)
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
