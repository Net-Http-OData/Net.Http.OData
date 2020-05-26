// -----------------------------------------------------------------------
// <copyright file="ODataUtility.cs" company="Project Contributors">
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
using System.Globalization;
using System.Linq;
using System.Text;
using Net.Http.OData.Model;
using Net.Http.OData.Query;

namespace Net.Http.OData
{
    /// <summary>
    /// A class containing URI utility functions.
    /// </summary>
    public static class ODataUtility
    {
        private const string SchemeDelimiter = "://";
        private static readonly char[] s_nonNameCharacters = new[] { '(', '/', '?', '$', '%' };

        /// <summary>
        /// Returns a <see cref="string"/> containing the the @odata.context.
        /// </summary>
        /// <param name="metadataLevel">The OData metadata level for the current request.</param>
        /// <param name="scheme">The scheme, which should be HTTP or HTTPS.</param>
        /// <param name="host">The host (e.g. odataservice.org).</param>
        /// <param name="path">The path (e.g. /odata/...)</param>
        /// <returns>A <see cref="string"/> containing the the @odata.context.</returns>
        public static string ODataContext(ODataMetadataLevel metadataLevel, string scheme, string host, string path)
            => metadataLevel != ODataMetadataLevel.None ? ODataContextUriBuilder(scheme, host, path).ToString() : default;

        /// <summary>
        /// Returns a <see cref="string"/> containing the the @odata.context.
        /// </summary>
        /// <param name="metadataLevel">The OData metadata level for the current request.</param>
        /// <param name="scheme">The scheme, which should be HTTP or HTTPS.</param>
        /// <param name="host">The host (e.g. odataservice.org).</param>
        /// <param name="path">The path (e.g. /odata/...)</param>
        /// <param name="entitySet">The <see cref="EntitySet"/>.</param>
        /// <returns>A <see cref="string"/> containing the the @odata.context.</returns>
        public static string ODataContext(ODataMetadataLevel metadataLevel, string scheme, string host, string path, EntitySet entitySet)
            => metadataLevel != ODataMetadataLevel.None ? ODataContextUriBuilder(scheme, host, path, entitySet).ToString() : default;

        /// <summary>
        /// Returns a <see cref="string"/> containing the the @odata.context.
        /// </summary>
        /// <param name="metadataLevel">The OData metadata level for the current request.</param>
        /// <param name="scheme">The scheme, which should be HTTP or HTTPS.</param>
        /// <param name="host">The host (e.g. odataservice.org).</param>
        /// <param name="path">The path (e.g. /odata/...)</param>
        /// <param name="entitySet">The <see cref="EntitySet"/>.</param>
        /// <param name="selectQueryOption">The $select query option.</param>
        /// <returns>A <see cref="string"/> containing the the @odata.context.</returns>
        public static string ODataContext(ODataMetadataLevel metadataLevel, string scheme, string host, string path, EntitySet entitySet, SelectExpandQueryOption selectQueryOption)
            => metadataLevel != ODataMetadataLevel.None ? ODataContextUriBuilder(scheme, host, path, entitySet, selectQueryOption).ToString() : default;

        /// <summary>
        /// Returns a <see cref="string"/> containing the the @odata.context.
        /// </summary>
        /// <param name="metadataLevel">The OData metadata level for the current request.</param>
        /// <param name="scheme">The scheme, which should be HTTP or HTTPS.</param>
        /// <param name="host">The host (e.g. odataservice.org).</param>
        /// <param name="path">The path (e.g. /odata/...)</param>
        /// <param name="entitySet">The <see cref="EntitySet"/>.</param>
        /// <typeparam name="TEntityKey">The type of the entity key.</typeparam>
        /// <returns>A <see cref="string"/> containing the the @odata.context.</returns>
#pragma warning disable S2326 // Unused type parameters should be removed

        public static string ODataContext<TEntityKey>(ODataMetadataLevel metadataLevel, string scheme, string host, string path, EntitySet entitySet)
#pragma warning restore S2326 // Unused type parameters should be removed
            => metadataLevel != ODataMetadataLevel.None ? ODataContextUriBuilder(scheme, host, path, entitySet).Append("/").Append(ODataUriNames.Entity).ToString() : default;

        /// <summary>
        /// Returns a <see cref="string"/> containing the the @odata.context.
        /// </summary>
        /// <param name="metadataLevel">The OData metadata level for the current request.</param>
        /// <param name="scheme">The scheme, which should be HTTP or HTTPS.</param>
        /// <param name="host">The host (e.g. odataservice.org).</param>
        /// <param name="path">The path (e.g. /odata/...)</param>
        /// <param name="entitySet">The <see cref="EntitySet"/>.</param>
        /// <param name="entityKey">The entity key for the entity set.</param>
        /// <param name="propertyName">The property name.</param>
        /// <typeparam name="TEntityKey">The type of the entity key.</typeparam>
        /// <returns>A <see cref="string"/> containing the the @odata.context.</returns>
        public static string ODataContext<TEntityKey>(ODataMetadataLevel metadataLevel, string scheme, string host, string path, EntitySet entitySet, TEntityKey entityKey, string propertyName)
            => metadataLevel != ODataMetadataLevel.None ? ODataContextUriBuilder(scheme, host, path, entitySet, entityKey, propertyName).ToString() : default;

        /// <summary>
        /// Returns a <see cref="string"/> containing the the @odata.id.
        /// </summary>
        /// <param name="scheme">The scheme, which should be HTTP or HTTPS.</param>
        /// <param name="host">The host (e.g. odataservice.org).</param>
        /// <param name="path">The path (e.g. /odata/...)</param>
        /// <param name="entitySet">The <see cref="EntitySet"/>.</param>
        /// <param name="entityKey">The entity key for the entity set.</param>
        /// <typeparam name="TEntityKey">The type of the entity key.</typeparam>
        /// <returns>A <see cref="string"/> containing the the @odata.id.</returns>
        public static string ODataId<TEntityKey>(string scheme, string host, string path, EntitySet entitySet, TEntityKey entityKey)
            => ODataContextUriBuilder(scheme, host, path, entitySet).Replace("$metadata#", string.Empty).AppendEntityKey(entityKey).ToString();

        /// <summary>
        /// Returns a <see cref="Uri"/> which represents the root of the OData service.
        /// </summary>
        /// <param name="scheme">The scheme, which should be HTTP or HTTPS.</param>
        /// <param name="host">The host (e.g. odataservice.org).</param>
        /// <param name="path">The path (e.g. /odata/...)</param>
        /// <returns>A <see cref="Uri"/> which represents the root of the OData service.</returns>
        public static Uri ODataServiceRootUri(string scheme, string host, string path)
            => new Uri(ODataServiceUriBuilder(scheme, host, path).ToString());

        /// <summary>
        /// Resolves the entity set name from the specified path segment of a URI.
        /// </summary>
        /// <param name="path">The path segment of a URI.</param>
        /// <returns>The name of the entity set in the path, or null if it is not an OData path.</returns>
        public static string ResolveEntitySetName(string path)
        {
            if (!string.IsNullOrWhiteSpace(path))
            {
                int odataIndex = path.IndexOf("odata", StringComparison.OrdinalIgnoreCase);

                if (odataIndex >= 0)
                {
                    int odataEndIndex = odataIndex + 5;
                    int entitySetNameStartIndex = path.IndexOf('/', odataEndIndex) + 1;
                    int entitySetNameEndIndex = path.IndexOfAny(s_nonNameCharacters, entitySetNameStartIndex);

                    if (entitySetNameStartIndex == 0 && entitySetNameEndIndex == 0)
                    {
                        return null;
                    }

                    if (entitySetNameEndIndex < 0)
                    {
                        entitySetNameEndIndex = path.Length;
                    }

                    string entitySetName = path.Substring(entitySetNameStartIndex, entitySetNameEndIndex - entitySetNameStartIndex);

                    return entitySetName;
                }
            }

            return null;
        }

        private static StringBuilder AppendEntityKey<TEntityKey>(this StringBuilder contextUriBuilder, TEntityKey entityKey)
        {
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

        private static StringBuilder ODataContextUriBuilder(string scheme, string host, string path)
            => ODataServiceUriBuilder(scheme, host, path).Append(ODataUriNames.Metadata);

        private static StringBuilder ODataContextUriBuilder(string scheme, string host, string path, EntitySet entitySet)
        {
            if (entitySet is null)
            {
                throw new ArgumentNullException(nameof(entitySet));
            }

            return ODataContextUriBuilder(scheme, host, path).Append('#').Append(entitySet.Name);
        }

        private static StringBuilder ODataContextUriBuilder(string scheme, string host, string path, EntitySet entitySet, SelectExpandQueryOption selectQueryOption)
        {
            StringBuilder contextUriBuilder = ODataContextUriBuilder(scheme, host, path, entitySet);

            if (selectQueryOption?.RawValue.Equals("$select=*", StringComparison.Ordinal) == true)
            {
                contextUriBuilder.Append("(*)");
            }
            else if (selectQueryOption?.PropertyPaths.Count > 0)
            {
                contextUriBuilder.AppendFormat(CultureInfo.InvariantCulture, "({0})", string.Join(",", selectQueryOption.PropertyPaths.Select(p => p.Property.Name)));
            }

            return contextUriBuilder;
        }

        private static StringBuilder ODataContextUriBuilder<TEntityKey>(string scheme, string host, string path, EntitySet entitySet, TEntityKey entityKey, string propertyName)
        {
            StringBuilder contextUriBuilder = ODataContextUriBuilder(scheme, host, path, entitySet)
                .AppendEntityKey(entityKey)
                .Append('/')
                .Append(propertyName);

            return contextUriBuilder;
        }

        private static StringBuilder ODataServiceUriBuilder(string scheme, string host, string path)
        {
            if (string.IsNullOrWhiteSpace(scheme))
            {
                throw new ArgumentException("Scheme must be specified", nameof(scheme));
            }

            if (string.IsNullOrWhiteSpace(host))
            {
                throw new ArgumentException("Host must be specified", nameof(host));
            }

            if (string.IsNullOrWhiteSpace(path))
            {
                throw new ArgumentException("Path must be specified", nameof(path));
            }

            // Adding 5 for the length of the word 'odata'.
            int odataEndIndex = path.IndexOf("odata", StringComparison.OrdinalIgnoreCase) + 5;

            // PERF: Calculate string length to allocate correct buffer size for StringBuilder, adding 1 for trailing slash.
            int length = scheme.Length + SchemeDelimiter.Length + host.Length + odataEndIndex + 1;

            StringBuilder uriBuilder = new StringBuilder(length)
                  .Append(scheme)
                  .Append(SchemeDelimiter)
                  .Append(host);

            // PERF: Append character by character up to the end of 'odata' to avoid the string allocation for using substring.
            for (int i = 0; i < odataEndIndex; i++)
            {
                uriBuilder.Append(path[i]);
            }

            uriBuilder.Append('/');

            System.Diagnostics.Debug.Assert(uriBuilder.Capacity == length, "The StringBuilder should not increase from the originally specified size");
            return uriBuilder;
        }
    }
}
