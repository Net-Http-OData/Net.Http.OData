// -----------------------------------------------------------------------
// <copyright file="OrderByProperty.cs" company="Project Contributors">
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
using Net.Http.OData.Model;

namespace Net.Http.OData.Query
{
    /// <summary>
    /// A class containing deserialised values from the $orderby query option.
    /// </summary>
    [System.Diagnostics.DebuggerDisplay("{RawValue}")]
    public sealed class OrderByProperty
    {
        /// <summary>
        /// Initialises a new instance of the <see cref="OrderByProperty"/> class.
        /// </summary>
        /// <param name="rawValue">The raw value.</param>
        /// <param name="model">The model.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="rawValue"/> or <paramref name="model"/> are null.</exception>
        /// <exception cref="ODataException">Thrown if there is an error parsing the <paramref name="rawValue"/>.</exception>
        internal OrderByProperty(string rawValue, EdmComplexType model)
        {
            RawValue = rawValue ?? throw new ArgumentNullException(nameof(rawValue));

            if (model is null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            if (rawValue.IndexOf(' ') == -1)
            {
                PropertyPath = PropertyPath.For(rawValue, model);
            }
            else
            {
                PropertyPath = PropertyPath.For(rawValue.SubstringBefore(' '), model);

                if (rawValue.EndsWith(" asc", StringComparison.Ordinal))
                {
                    Direction = OrderByDirection.Ascending;
                }
                else if (rawValue.EndsWith(" desc", StringComparison.Ordinal))
                {
                    Direction = OrderByDirection.Descending;
                }
                else
                {
                    throw ODataException.BadRequest(ExceptionMessage.InvalidOrderByDirection(rawValue.SubstringAfter(' '), PropertyPath.Property.Name));
                }
            }
        }

        /// <summary>
        /// Gets the direction the property should be ordered by.
        /// </summary>
        public OrderByDirection Direction { get; }

        /// <summary>
        /// Gets the property path to order by.
        /// </summary>
        public PropertyPath PropertyPath { get; }

        /// <summary>
        /// Gets the raw request value.
        /// </summary>
        public string RawValue { get; }
    }
}
