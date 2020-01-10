﻿// -----------------------------------------------------------------------
// <copyright file="UriExtensions.cs" company="Project Contributors">
// Copyright 2012 - 2020 Project Contributors
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
using System.Globalization;
using System.Linq;
using System.Text;
using Net.Http.OData.Model;
using Net.Http.OData.Query;

namespace Net.Http.OData
{
    /// <summary>
    /// Extensions for the Uri class.
    /// </summary>
    public static class UriExtensions
    {
        private static readonly char[] s_nonNameCharacters = new[] { '(', '/', '$', '%' };

        /// <summary>
        /// Builds the OData context URI.
        /// </summary>
        /// <param name="requestUri">The OData request URI.</param>
        /// <returns>The OData context URI.</returns>
        public static StringBuilder ODataContextUriBuilder(this Uri requestUri)
        {
            StringBuilder contextUriBuilder = ODataServiceUriBuilder(requestUri);
            contextUriBuilder.Append("$metadata");

            return contextUriBuilder;
        }

        /// <summary>
        /// Builds the OData context URI.
        /// </summary>
        /// <param name="requestUri">The OData request URI.</param>
        /// <param name="entitySet">The entity set.</param>
        /// <returns>The OData context URI.</returns>
        public static StringBuilder ODataContextUriBuilder(this Uri requestUri, EntitySet entitySet)
        {
            if (entitySet is null)
            {
                throw new ArgumentNullException(nameof(entitySet));
            }

            StringBuilder contextUriBuilder = ODataContextUriBuilder(requestUri);
            contextUriBuilder.Append("#").Append(entitySet.Name);

            return contextUriBuilder;
        }

        /// <summary>
        /// Builds the OData context URI.
        /// </summary>
        /// <param name="requestUri">The OData request URI.</param>
        /// <param name="entitySet">The entity set.</param>
        /// <param name="selectExpandQueryOption">The select query option.</param>
        /// <returns>The OData context URI.</returns>
        public static StringBuilder ODataContextUriBuilder(this Uri requestUri, EntitySet entitySet, SelectExpandQueryOption selectExpandQueryOption)
        {
            if (entitySet is null)
            {
                throw new ArgumentNullException(nameof(entitySet));
            }

            if (selectExpandQueryOption is null)
            {
                throw new ArgumentNullException(nameof(selectExpandQueryOption));
            }

            StringBuilder contextUriBuilder = ODataContextUriBuilder(requestUri);
            contextUriBuilder.Append("#").Append(entitySet.Name);

            if (selectExpandQueryOption?.RawValue.Equals("$select=*", StringComparison.Ordinal) == true)
            {
                contextUriBuilder.Append("(*)");
            }
            else if (selectExpandQueryOption?.PropertyPaths.Count > 0)
            {
                contextUriBuilder.AppendFormat(CultureInfo.InvariantCulture, "({0})", string.Join(",", selectExpandQueryOption.PropertyPaths.Select(p => p.Property.Name)));
            }

            return contextUriBuilder;
        }

        /// <summary>
        /// Builds the OData context URI.
        /// </summary>
        /// <param name="requestUri">The OData request URI.</param>
        /// <param name="entitySet">The entity set.</param>
        /// <param name="entityKey">The entity key.</param>
        /// <typeparam name="TEntityKey">The type of the entity key.</typeparam>
        /// <returns>The OData context URI.</returns>
        public static StringBuilder ODataContextUriBuilder<TEntityKey>(this Uri requestUri, EntitySet entitySet, TEntityKey entityKey)
        {
            StringBuilder contextUriBuilder = ODataContextUriBuilder(requestUri, entitySet);
            contextUriBuilder.Append("/$entity");

            return contextUriBuilder;
        }

        /// <summary>
        /// Builds the OData context URI.
        /// </summary>
        /// <param name="requestUri">The OData request URI.</param>
        /// <param name="entitySet">The entity set.</param>
        /// <param name="entityKey">The entity key.</param>
        /// <param name="propertyName">The name of the property.</param>
        /// <typeparam name="TEntityKey">The type of the entity key.</typeparam>
        /// <returns>The OData context URI.</returns>
        public static StringBuilder ODataContextUriBuilder<TEntityKey>(this Uri requestUri, EntitySet entitySet, TEntityKey entityKey, string propertyName)
        {
            StringBuilder contextUriBuilder = ODataContextUriBuilder(requestUri, entitySet);

            if (typeof(TEntityKey) == typeof(string))
            {
                contextUriBuilder.Append("('").Append(entityKey.ToString()).Append("')");
            }
            else
            {
                contextUriBuilder.Append("(").Append(entityKey.ToString()).Append(")");
            }

            contextUriBuilder.Append('/').Append(propertyName);

            return contextUriBuilder;
        }

        /// <summary>
        /// Builds the OData entity URI.
        /// </summary>
        /// <param name="requestUri">The OData request URI.</param>
        /// <param name="entitySet">The entity set.</param>
        /// <param name="entityKey">The entity key.</param>
        /// <typeparam name="TEntityKey">The type of the entity key.</typeparam>
        /// <returns>The OData entity URI.</returns>
        public static StringBuilder ODataEntityUriBuilder<TEntityKey>(this Uri requestUri, EntitySet entitySet, TEntityKey entityKey)
        {
            if (entitySet is null)
            {
                throw new ArgumentNullException(nameof(entitySet));
            }

            StringBuilder contextUriBuilder = ODataServiceUriBuilder(requestUri);
            contextUriBuilder.Append(entitySet.Name);

            if (typeof(TEntityKey) == typeof(string))
            {
                contextUriBuilder.Append("('").Append(entityKey.ToString()).Append("')");
            }
            else
            {
                contextUriBuilder.Append("(").Append(entityKey.ToString()).Append(")");
            }

            return contextUriBuilder;
        }

        /// <summary>
        /// Resolves the name of the Entity Set referenced in the request.
        /// </summary>
        /// <param name="requestUri">The HTTP request message which led to ths OData request.</param>
        /// <returns>The name of the Entity Set referenced in the request, or null if no entity set was referenced.</returns>
        public static string ResolveODataEntitySetName(this Uri requestUri)
        {
            if (requestUri is null)
            {
                throw new ArgumentNullException(nameof(requestUri));
            }

            int modelNameSegmentIndex = -1;

            for (int i = 0; i < requestUri.Segments.Length; i++)
            {
                if (requestUri.Segments[i].StartsWith("odata", StringComparison.OrdinalIgnoreCase))
                {
                    modelNameSegmentIndex = i + 1;
                    break;
                }
            }

            if (modelNameSegmentIndex < 0 || modelNameSegmentIndex >= requestUri.Segments.Length)
            {
                return null;
            }

            string modelNameSegment = requestUri.Segments[modelNameSegmentIndex];

            int nonNameCharacterIndex = modelNameSegment.IndexOfAny(s_nonNameCharacters);

            if (nonNameCharacterIndex > 0)
            {
                modelNameSegment = modelNameSegment.Substring(0, nonNameCharacterIndex);
            }

            return modelNameSegment;
        }

        /// <summary>
        /// Builds the OData service URI.
        /// </summary>
        /// <param name="requestUri">The OData request URI.</param>
        /// <returns>The OData service URI.</returns>
        public static Uri ResolveODataServiceUri(this Uri requestUri)
            => new Uri(ODataServiceUriBuilder(requestUri).ToString());

        private static StringBuilder ODataServiceUriBuilder(Uri requestUri)
        {
            if (requestUri is null)
            {
                throw new ArgumentNullException(nameof(requestUri));
            }

            StringBuilder uriBuilder = new StringBuilder()
                .Append(requestUri.Scheme)
                .Append(Uri.SchemeDelimiter)
                .Append(requestUri.Authority);

            for (int i = 0; i < requestUri.Segments.Length; i++)
            {
                string segment = requestUri.Segments[i];
                uriBuilder.Append(segment);

                if (segment.StartsWith("odata", StringComparison.OrdinalIgnoreCase))
                {
                    if (!segment.EndsWith("/", StringComparison.Ordinal))
                    {
                        uriBuilder.Append('/');
                    }

                    break;
                }
            }

            return uriBuilder;
        }
    }
}
