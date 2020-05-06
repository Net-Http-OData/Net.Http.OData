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

            return ApplyToImpl(queryOptions, queryable);
        }

        private static IEnumerable<ExpandoObject> ApplyToImpl(ODataQueryOptions queryOptions, IQueryable queryable)
        {
            foreach (object entity in queryable)
            {
                yield return BuildExpando(queryOptions, entity);
            }
        }

        private static ExpandoObject BuildExpando(ODataQueryOptions queryOptions, object entity)
        {
            IEnumerable<PropertyPath> propertyPaths = queryOptions.Select?.PropertyPaths ?? queryOptions.EntitySet.EdmType.Properties.Where(p => !p.IsNavigable).Select(PropertyPath.For);

            var expandoObject = new ExpandoObject();

            foreach (PropertyPath propertyPath in propertyPaths)
            {
                PropertyPath path = propertyPath;
                var dictionary = (IDictionary<string, object>)expandoObject;
                object obj = entity;

                while (path.Next != null)
                {
                    dictionary[path.Property.Name] = new ExpandoObject();
                    dictionary = (IDictionary<string, object>)dictionary[path.Property.Name];
                    obj = path.Property.ClrProperty.GetValue(entity);
                    path = path.Next;
                }

                dictionary[path.Property.Name] = path.Property.ClrProperty.GetValue(obj);
            }

            return expandoObject;
        }
    }
}
