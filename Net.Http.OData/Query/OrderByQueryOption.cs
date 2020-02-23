// -----------------------------------------------------------------------
// <copyright file="OrderByQueryOption.cs" company="Project Contributors">
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

namespace Net.Http.OData.Query
{
    /// <summary>
    /// A class containing deserialised values from the $orderby query option.
    /// </summary>
    [System.Diagnostics.DebuggerDisplay("{RawValue}")]
    public sealed class OrderByQueryOption : QueryOption
    {
        /// <summary>
        /// Initialises a new instance of the <see cref="OrderByQueryOption"/> class.
        /// </summary>
        /// <param name="rawValue">The raw request value.</param>
        /// <param name="model">The model.</param>
        internal OrderByQueryOption(string rawValue, EdmComplexType model)
            : base(rawValue)
        {
            if (model is null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            if (rawValue.IndexOf(',') > 0)
            {
                Properties = rawValue.Slice(',', rawValue.IndexOf('=') + 1)
                    .Select(raw => new OrderByProperty(raw, model))
                    .ToArray();
            }
            else
            {
                string property = rawValue.SubstringAfter('=');

                Properties = new[] { new OrderByProperty(property, model) };
            }
        }

        /// <summary>
        /// Gets the properties the query should be ordered by.
        /// </summary>
        public IReadOnlyList<OrderByProperty> Properties { get; }
    }
}
