﻿// -----------------------------------------------------------------------
// <copyright file="SelectQueryOptionValidator.cs" company="Project Contributors">
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
namespace Net.Http.OData.Query.Validators
{
    /// <summary>
    /// A class which validates the $select query option based upon the <see cref="ODataValidationSettings"/>.
    /// </summary>
    internal static class SelectQueryOptionValidator
    {
        /// <summary>
        /// Validates the specified query options.
        /// </summary>
        /// <param name="queryOptions">The query options.</param>
        /// <param name="validationSettings">The validation settings.</param>
        /// <exception cref="ODataException">Thrown if the validation fails.</exception>
        internal static void Validate(ODataQueryOptions queryOptions, ODataValidationSettings validationSettings)
        {
            if (queryOptions.RawValues.Select is null)
            {
                return;
            }

            if ((validationSettings.AllowedQueryOptions & AllowedQueryOptions.Select) != AllowedQueryOptions.Select)
            {
                throw ODataException.NotImplemented("The query option $select is not implemented by this service", "$select");
            }
        }
    }
}
