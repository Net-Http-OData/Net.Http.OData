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
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;
using Net.Http.OData.Model;
using Net.Http.OData.Query;
using Net.Http.OData.Query.Expressions;

namespace Net.Http.OData.Linq
{
    /// <summary>
    /// Adds support for IQueryable to the ODataQueryOptions.
    /// </summary>
    public static class ODataQueryOptionsExtensions
    {
        /// <summary>
        /// Applies the query expressed by the <see cref="ODataQueryOptions"/> to the specified <see cref="IQueryable"/>.
        /// </summary>
        /// <param name="queryOptions">The <see cref="ODataQueryOptions"/> to apply.</param>
        /// <param name="queryable">The <see cref="IQueryable"/> to apply the query options to.</param>
        /// <returns>The result of the OData query.</returns>
        public static IEnumerable<ExpandoObject> ApplyTo(this ODataQueryOptions queryOptions, IQueryable queryable)
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
                throw new InvalidOperationException();
            }

            return Apply(queryOptions, queryable);
        }

        private static IEnumerable<ExpandoObject> Apply(ODataQueryOptions queryOptions, IQueryable queryable)
        {
            foreach (object entity in queryable.ApplyOrder(queryOptions).ApplySkip(queryOptions).ApplyTop(queryOptions))
            {
                yield return ApplySelect(entity, queryOptions);
            }
        }

        private static IQueryable ApplyOrder(this IQueryable queryable, ODataQueryOptions queryOptions)
        {
            if (queryOptions.OrderBy == null)
            {
                return queryable;
            }

            IQueryable q = queryable;

            for (int i = 0; i < queryOptions.OrderBy.Properties.Count; i++)
            {
                OrderByProperty orderByProperty = queryOptions.OrderBy.Properties[i];
                PropertyPath path = orderByProperty.PropertyPath;
                Type entityType = queryOptions.EntitySet.EdmType.ClrType;
                Type propertyType = path.Property.ClrProperty.PropertyType;

                // the 'entity' in the lambda expression (entity => entity.Property)
                ParameterExpression entityParameterExpression = Expression.Parameter(entityType, "entity");

                // the 'property' in the lambda expression (entity => entity.Property)
                MemberExpression propertyMemberExpression = Expression.Property(entityParameterExpression, path.Property.Name);

                while (path.Next != null)
                {
                    path = path.Next;
                    propertyMemberExpression = Expression.Property(propertyMemberExpression, path.Property.Name);
                    propertyType = path.Property.ClrProperty.PropertyType;
                }

                // Represents the lambda in the method argument "(entity => entity.Property)"
                LambdaExpression lambdaExpression = Expression.Lambda(
                    typeof(Func<,>).MakeGenericType(entityType, propertyType),
                    propertyMemberExpression,
                    new ParameterExpression[] { entityParameterExpression });

                // Represents the method call itself "OrderBy(entity => entity.Property)"
                MethodCallExpression orderByCallExpression = Expression.Call(
                    typeof(Queryable),
                    OrderByMethodName(orderByProperty.Direction, i),
                    new Type[] { entityType, propertyType },
                    q.Expression,
                    lambdaExpression);

                q = q.Provider.CreateQuery(orderByCallExpression);
            }

            return q;
        }

        private static ExpandoObject ApplySelect(object entity, ODataQueryOptions queryOptions)
        {
            IEnumerable<PropertyPath> propertyPaths = queryOptions.Select?.PropertyPaths ?? queryOptions.EntitySet.EdmType.Properties.Where(p => !p.IsNavigable).Select(PropertyPath.For);

            if (queryOptions.Expand != null)
            {
                propertyPaths = propertyPaths.Concat(queryOptions.Expand.PropertyPaths);
            }

            var expandoObject = new ExpandoObject();

            foreach (PropertyPath propertyPath in propertyPaths)
            {
                PropertyPath path = propertyPath;
                var dictionary = (IDictionary<string, object>)expandoObject;
                object obj = entity;

                while (path.Next != null)
                {
                    if (!dictionary.ContainsKey(path.Property.Name))
                    {
                        dictionary[path.Property.Name] = new ExpandoObject();
                    }

                    dictionary = (IDictionary<string, object>)dictionary[path.Property.Name];
                    obj = path.Property.ClrProperty.GetValue(obj);
                    path = path.Next;
                }

                if (path.Property.IsNavigable)
                {
                    dictionary[path.Property.Name] = new ExpandoObject();
                    dictionary = (IDictionary<string, object>)dictionary[path.Property.Name];
                    obj = path.Property.ClrProperty.GetValue(obj);

                    var edmComplexType = path.Property.PropertyType as EdmComplexType;

                    while (edmComplexType != null)
                    {
                        foreach (EdmProperty edmProperty in edmComplexType.Properties)
                        {
                            if (!edmProperty.IsNavigable)
                            {
                                dictionary[edmProperty.Name] = edmProperty.ClrProperty.GetValue(obj);
                            }
                        }

                        edmComplexType = edmComplexType.BaseType as EdmComplexType;
                    }
                }
                else
                {
                    dictionary[path.Property.Name] = path.Property.ClrProperty.GetValue(obj);
                }
            }

            return expandoObject;
        }

        private static IQueryable ApplySkip(this IQueryable queryable, ODataQueryOptions queryOptions)
        {
            if (queryOptions.Skip.HasValue)
            {
                MethodCallExpression skipCallExpression = Expression.Call(
                    typeof(Queryable),
                    "Skip",
                    new Type[] { queryOptions.EntitySet.EdmType.ClrType },
                    queryable.Expression,
                    Expression.Constant(queryOptions.Skip.Value));

                return queryable.Provider.CreateQuery(skipCallExpression);
            }

            return queryable;
        }

        private static IQueryable ApplyTop(this IQueryable queryable, ODataQueryOptions queryOptions)
        {
            if (queryOptions.Top.HasValue)
            {
                MethodCallExpression skipCallExpression = Expression.Call(
                    typeof(Queryable),
                    "Take",
                    new Type[] { queryOptions.EntitySet.EdmType.ClrType },
                    queryable.Expression,
                    Expression.Constant(queryOptions.Top.Value));

                return queryable.Provider.CreateQuery(skipCallExpression);
            }

            return queryable;
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
