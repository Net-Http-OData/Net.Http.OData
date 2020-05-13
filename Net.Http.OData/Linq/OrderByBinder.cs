// -----------------------------------------------------------------------
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
using Net.Http.OData.Query;
using Net.Http.OData.Query.Binders;
using Net.Http.OData.Query.Expressions;

namespace Net.Http.OData.Linq
{
    /// <summary>
    /// The binder class which can append the $order by query option.
    /// </summary>
    public sealed class OrderByBinder : AbstractOrderByBinder
    {
        private int _index;

        private OrderByBinder(IQueryable queryable)
            => Queryable = queryable;

        /// <summary>
        /// Gets the IQueryable with the order by applied.
        /// </summary>
        public IQueryable Queryable { get; private set; }

        /// <summary>
        /// Binds the <see cref="OrderByQueryOption"/> in the <see cref="ODataQueryOptions"/> to the <see cref="IQueryable"/>.
        /// </summary>
        /// <param name="queryOptions">The <see cref="ODataQueryOptions"/> to bind.</param>
        /// <param name="queryable">The <see cref="IQueryable"/> to bind to.</param>
        /// <returns>The ordered <see cref="IQueryable"/>.</returns>
        public static IQueryable BindOrderBy(ODataQueryOptions queryOptions, IQueryable queryable)
        {
            if (queryOptions is null)
            {
                throw new ArgumentNullException(nameof(queryOptions));
            }

            if (queryable is null)
            {
                throw new ArgumentNullException(nameof(queryable));
            }

            var orderByBinder = new OrderByBinder(queryable);
            orderByBinder.Bind(queryOptions.OrderBy);

            return orderByBinder.Queryable;
        }

        /// <inheritdoc/>
        protected override void Bind(OrderByProperty orderByProperty)
        {
            PropertyPath path = orderByProperty.PropertyPath;
            Type entityType = path.Property.DeclaringType.ClrType;
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
                OrderByMethodName(orderByProperty.Direction, _index++),
                new Type[] { entityType, propertyType },
                Queryable.Expression,
                lambdaExpression);

            Queryable = Queryable.Provider.CreateQuery(orderByCallExpression);
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
