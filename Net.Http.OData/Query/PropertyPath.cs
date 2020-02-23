// -----------------------------------------------------------------------
// <copyright file="PropertyPath.cs" company="Project Contributors">
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
    /// A class which represents a property path.
    /// </summary>
    [System.Diagnostics.DebuggerDisplay("{Property}/{Next}")]
    public sealed class PropertyPath
    {
        /// <summary>
        /// Initialises a new instance of the <see cref="PropertyPath"/> class for the specified <see cref="EdmProperty"/> with no next path segment.
        /// </summary>
        /// <param name="property">The <see cref="EdmProperty"/> that the path segment represents.</param>
        internal PropertyPath(EdmProperty property)
            : this(property, null)
        {
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="PropertyPath"/> class for the specified <see cref="EdmProperty"/> with the next path segment.
        /// </summary>
        /// <param name="property">The <see cref="EdmProperty"/> that the path segment represents.</param>
        /// <param name="next">The next <see cref="PropertyPath"/> in the property path.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="property"/> is null.</exception>
        internal PropertyPath(EdmProperty property, PropertyPath next)
        {
            Property = property ?? throw new ArgumentNullException(nameof(property));
            Next = next;
        }

        /// <summary>
        /// Gets the next property in the path being referenced in the query, or null if this instance is the last (or only) property in the path.
        /// </summary>
        public PropertyPath Next { get; }

        /// <summary>
        /// Gets the <see cref="EdmProperty"/> representing the property being referenced in the query.
        /// </summary>
        public EdmProperty Property { get; }

        /// <summary>
        /// Creates the <see cref="PropertyPath"/> for the given property path.
        /// </summary>
        /// <param name="rawPropertyPath">The raw property path.</param>
        /// <param name="edmComplexType">The <see cref="EdmComplexType"/> which contains the first property in the property path.</param>
        /// <returns>The <see cref="PropertyPath"/> for the given property path.</returns>
        internal static PropertyPath For(string rawPropertyPath, EdmComplexType edmComplexType)
        {
            if (rawPropertyPath.IndexOf('/') == -1)
            {
                return new PropertyPath(edmComplexType.GetProperty(rawPropertyPath));
            }

            string[] nameSegments = rawPropertyPath.Split(SplitCharacter.ForwardSlash);

            var edmProperties = new EdmProperty[nameSegments.Length];

            EdmComplexType model = edmComplexType;

            for (int i = 0; i < nameSegments.Length; i++)
            {
                edmProperties[i] = model.GetProperty(nameSegments[i]);

                // All properties in the path except the last must be navigable.
                if (i < nameSegments.Length - 1 && !edmProperties[i].IsNavigable)
                {
                    throw new ODataException(
                        $"The property '{nameSegments[i]}' in the path '{rawPropertyPath}' is not a navigable property.",
                        HttpStatusCode.BadRequest);
                }

                model = edmProperties[i].PropertyType as EdmComplexType;
            }

            PropertyPath propertyPath = null;

            for (int i = edmProperties.Length - 1; i >= 0; i--)
            {
                propertyPath = new PropertyPath(edmProperties[i], propertyPath);
            }

            return propertyPath;
        }
    }
}
