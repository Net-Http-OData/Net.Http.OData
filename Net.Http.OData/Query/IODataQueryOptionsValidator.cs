// -----------------------------------------------------------------------
// <copyright file="IODataQueryOptionsValidator.cs" company="Project Contributors">
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
    /// The interface for an a class which validates an <see cref="ODataQueryOptions"/> instance using <see cref="ODataValidationSettings"/>.
    /// </summary>
    public interface IODataQueryOptionsValidator
    {
        /// <summary>
        /// Validates the specified query options using the specified validation settings.
        /// </summary>
        /// <param name="queryOptions">The query options to validate.</param>
        /// <param name="validationSettings">The validation settings to configure the validation.</param>
        void Validate(ODataQueryOptions queryOptions, ODataValidationSettings validationSettings);
    }
}
