// -----------------------------------------------------------------------
// <copyright file="SkipBinder.cs" company="Project Contributors">
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
using System.Linq;
using System.Linq.Expressions;

namespace Net.Http.OData.Query.Linq
{
    internal static class SkipBinder
    {
        internal static IQueryable ApplySkip(this IQueryable queryable, ODataQueryOptions queryOptions)
        {
            if (queryOptions.Skip == null)
            {
                return queryable;
            }

            MethodCallExpression skipCallExpression = Expression.Call(
                typeof(Queryable),
                "Skip",
                new Type[] { queryOptions.EntitySet.EdmType.ClrType },
                queryable.Expression,
                Expression.Constant(queryOptions.Skip.Value));

            return queryable.Provider.CreateQuery(skipCallExpression);
        }
    }
}
