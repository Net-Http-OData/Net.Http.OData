// -----------------------------------------------------------------------
// <copyright file="UriUtility.cs" company="Project Contributors">
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

namespace Net.Http.OData
{
    /// <summary>
    /// A class containing URI utility functions.
    /// </summary>
    public static class UriUtility
    {
        private static readonly char[] s_nonNameCharacters = new[] { '(', '/', '$', '%' };

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
    }
}
