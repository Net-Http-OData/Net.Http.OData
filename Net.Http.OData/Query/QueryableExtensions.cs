// -----------------------------------------------------------------------
// <copyright file="QueryableExtensions.cs" company="Project Contributors">
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
using System.Dynamic;
using System.Linq;
using Net.Http.OData.Query;
using Net.Http.OData.Query.Linq;

namespace Net.Http.OData.Linq
{
    /// <summary>
    /// Adds support query support for ODataQueryOptions to an IQueryable.
    /// </summary>
    public static class QueryableExtensions
    {
        /// <summary>
        /// Applies the query expressed by the <see cref="ODataQueryOptions"/> to the specified <see cref="IQueryable"/>.
        /// </summary>
        /// <param name="queryable">The <see cref="IQueryable"/> to apply the query options to.</param>
        /// <param name="queryOptions">The <see cref="ODataQueryOptions"/> to apply.</param>
        /// <returns>The result of the OData query.</returns>
        public static IEnumerable<ExpandoObject> Apply(this IQueryable queryable, ODataQueryOptions queryOptions)
        {
            if (queryOptions is null)
            {
                throw new ArgumentNullException(nameof(queryOptions));
            }

            if (queryable is null)
            {
                throw new ArgumentNullException(nameof(queryable));
            }

            if (queryable.ElementType != queryOptions.EntitySet.EdmType.ClrType)
            {
                throw new InvalidOperationException(ExceptionMessage.QueryableNotExpectedType(queryOptions.EntitySet.EdmType.ClrType));
            }

            return ODataObjectBuilder.BuildODataObjects(queryable, queryOptions);
        }
    }
}
