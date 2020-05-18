// <copyright file="OrderByBinder.cs" company="Project Contributors">
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
    internal static class OrderByBinder
    {
        internal static IQueryable ApplyOrder(this IQueryable queryable, ODataQueryOptions queryOptions)
        {
            if (queryOptions.OrderBy == null)
            {
                return queryable;
            }

            IQueryable q = queryable;

            for (int i = 0; i < queryOptions.OrderBy.Properties.Count; i++)
            {
                OrderByProperty orderByProperty = queryOptions.OrderBy.Properties[i];

                // Represents the method call itself "OrderBy(entity => entity.Property)"
                MethodCallExpression orderByCallExpression = Expression.Call(
                    typeof(Queryable),
                    OrderByMethodName(orderByProperty.Direction, i),
                    new Type[] { queryOptions.EntitySet.EdmType.ClrType, orderByProperty.PropertyPath.InnerMostProperty.ClrProperty.PropertyType },
                    q.Expression,
                    orderByProperty.PropertyPath.LambdaExpression);

                q = q.Provider.CreateQuery(orderByCallExpression);
            }

            return q;
        }

        private static string OrderByMethodName(OrderByDirection direction, int index)
        {
            if (direction == OrderByDirection.Ascending)
            {
                return index == 0 ? "OrderBy" : "ThenBy";
            }

            return index == 0 ? "OrderByDescending" : "ThenByDescending";
        }
    }
}
