// <copyright file="TopBinder.cs" company="Project Contributors">
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
    internal static class TopBinder
    {
        internal static IQueryable ApplyTop(this IQueryable queryable, ODataQueryOptions queryOptions)
        {
            if (queryOptions.Top == null)
            {
                return queryable;
            }

            MethodCallExpression takeCallExpression = Expression.Call(
                typeof(Queryable),
                "Take",
                new Type[] { queryOptions.EntitySet.EdmType.ClrType },
                queryable.Expression,
                Expression.Constant(queryOptions.Top.Value));

            return queryable.Provider.CreateQuery(takeCallExpression);
        }
    }
}
