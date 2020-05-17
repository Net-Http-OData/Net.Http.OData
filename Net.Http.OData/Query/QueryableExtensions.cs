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
using Net.Http.OData.Model;
using Net.Http.OData.Query;
using Net.Http.OData.Query.Expressions;
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
                throw new InvalidOperationException();
            }

            return ApplyImpl(queryable, queryOptions);
        }

        private static IEnumerable<ExpandoObject> ApplyImpl(IQueryable queryable, ODataQueryOptions queryOptions)
        {
            foreach (object entity in queryable.ApplyOrder(queryOptions).ApplySkip(queryOptions).ApplyTop(queryOptions))
            {
                yield return ApplySelect(entity, queryOptions);
            }
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
    }
}
