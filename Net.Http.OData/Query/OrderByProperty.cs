﻿// -----------------------------------------------------------------------
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
using System.Net;
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
        /// <exception cref="ArgumentNullException">Thrown if raw value or model are null.</exception>
        /// <exception cref="ArgumentOutOfRangeException">If supplied, the direction should be either 'asc' or 'desc'.</exception>
        internal OrderByProperty(string rawValue, EdmComplexType model)
        {
            if (model is null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            RawValue = rawValue ?? throw new ArgumentNullException(nameof(rawValue));

            int space = rawValue.IndexOf(' ');

            if (space == -1)
            {
                PropertyPath = PropertyPath.For(rawValue, model);
            }
            else
            {
                PropertyPath = PropertyPath.For(rawValue.Substring(0, space), model);

                switch (rawValue.Substring(space + 1, rawValue.Length - (space + 1)))
                {
                    case "asc":
                        Direction = OrderByDirection.Ascending;
                        break;

                    case "desc":
                        Direction = OrderByDirection.Descending;
                        break;

                    default:
                        throw new ODataException(HttpStatusCode.BadRequest, $"The supplied order value for {PropertyPath.Property.Name} is invalid, valid options are 'asc' and 'desc'");
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
