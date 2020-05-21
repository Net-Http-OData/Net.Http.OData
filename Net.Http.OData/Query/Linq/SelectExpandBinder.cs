// <copyright file="SelectExpandBinder.cs" company="Project Contributors">
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
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using Net.Http.OData.Model;
using Net.Http.OData.Query.Expressions;

namespace Net.Http.OData.Query.Linq
{
    internal static class SelectExpandBinder
    {
        internal static ExpandoObject ApplySelectExpand(object entity, EntitySet entitySet, SelectExpandQueryOption selectQueryOption, SelectExpandQueryOption expandQueryOption)
        {
            IEnumerable<PropertyPath> propertyPaths = selectQueryOption?.PropertyPaths ?? entitySet.EdmType.Properties.Where(p => !p.IsNavigable).Select(PropertyPath.For);

            if (expandQueryOption != null)
            {
                propertyPaths = propertyPaths.Concat(expandQueryOption.PropertyPaths);
            }

            return BuildODataDynamicObject(entity, propertyPaths);
        }

        private static ExpandoObject BuildODataDynamicObject(object entity, IEnumerable<PropertyPath> propertyPaths)
        {
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
