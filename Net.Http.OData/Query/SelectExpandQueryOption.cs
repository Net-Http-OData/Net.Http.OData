﻿// -----------------------------------------------------------------------
// <copyright file="SelectExpandQueryOption.cs" company="Project Contributors">
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
using System.Linq;
using Net.Http.OData.Model;
using Net.Http.OData.Query.Expressions;

namespace Net.Http.OData.Query
{
    /// <summary>
    /// A class containing deserialised values from the $select or $expand query option.
    /// </summary>
    [System.Diagnostics.DebuggerDisplay("{RawValue}")]
    public sealed class SelectExpandQueryOption : QueryOption
    {
        /// <summary>
        /// Initialises a new instance of the <see cref="SelectExpandQueryOption" /> class.
        /// </summary>
        /// <param name="rawValue">The raw request value.</param>
        /// <param name="model">The model.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="rawValue"/> is null.</exception>
        internal SelectExpandQueryOption(string rawValue, EdmComplexType model)
            : base(rawValue)
        {
            if (model is null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            var propertyPaths = new List<PropertyPath>();

            foreach (string propertyPathName in rawValue.Slice(',', rawValue.IndexOf('=') + 1))
            {
                if (propertyPathName == "*")
                {
                    if (rawValue.StartsWith("$select", StringComparison.Ordinal))
                    {
                        propertyPaths.AddRange(model.Properties.Where(p => !p.IsNavigable).Select(PropertyPath.For));
                    }
                    else if (rawValue.StartsWith("$expand", StringComparison.Ordinal))
                    {
                        propertyPaths.AddRange(model.Properties.Where(p => p.IsNavigable).Select(PropertyPath.For));
                    }
                }
                else
                {
                    propertyPaths.Add(PropertyPath.For(propertyPathName, model));
                }
            }

            PropertyPaths = propertyPaths;
        }

        /// <summary>
        /// Gets the property paths specified in the query.
        /// </summary>
        public IReadOnlyList<PropertyPath> PropertyPaths { get; }
    }
}
