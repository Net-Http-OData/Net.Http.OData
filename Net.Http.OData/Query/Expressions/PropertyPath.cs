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
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq.Expressions;
using Net.Http.OData.Model;

namespace Net.Http.OData.Query.Expressions
{
    /// <summary>
    /// A class which represents a property path.
    /// </summary>
    [System.Diagnostics.DebuggerDisplay("{Property}/{Next}")]
    public sealed class PropertyPath
    {
        private static readonly ConcurrentDictionary<KeyValuePair<EdmComplexType, string>, PropertyPath> s_edmComplexTypeCache = new ConcurrentDictionary<KeyValuePair<EdmComplexType, string>, PropertyPath>();
        private static readonly ConcurrentDictionary<EdmProperty, PropertyPath> s_edmPropertyCache = new ConcurrentDictionary<EdmProperty, PropertyPath>();

        /// <summary>
        /// Initialises a new instance of the <see cref="PropertyPath"/> class for the specified <see cref="EdmProperty"/> with no next path segment.
        /// </summary>
        /// <param name="property">The <see cref="EdmProperty"/> that the path segment represents.</param>
        private PropertyPath(EdmProperty property)
            : this(property, null)
        {
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="PropertyPath"/> class for the specified <see cref="EdmProperty"/> with the next path segment.
        /// </summary>
        /// <param name="property">The <see cref="EdmProperty"/> that the path segment represents.</param>
        /// <param name="next">The next <see cref="PropertyPath"/> in the property path.</param>
        private PropertyPath(EdmProperty property, PropertyPath next)
        {
            Property = property;
            Next = next;

            // 'entity' in the lambda expression 'entity => entity.Property'
            ParameterExpression entityParameterExpression = Property.DeclaringType.ParameterExpression;

            PropertyPath path = this;
            Type propertyType = Property.ClrProperty.PropertyType;

            // 'Property' in the lambda expression 'entity => entity.Property'
            MemberExpression propertyMemberExpression = Expression.Property(entityParameterExpression, path.Property.Name);

            while (path.Next != null)
            {
                path = path.Next;
                propertyMemberExpression = Expression.Property(propertyMemberExpression, path.Property.Name);
                propertyType = path.Property.ClrProperty.PropertyType;
            }

            MemberExpression = propertyMemberExpression;

            // Represents the lambda in the method argument '(entity => entity.Property)'
            LambdaExpression = Expression.Lambda(
                typeof(Func<,>).MakeGenericType(Property.DeclaringType.ClrType, propertyType),
                propertyMemberExpression,
                new ParameterExpression[] { entityParameterExpression });
        }

        /// <summary>
        /// Gets the <see cref="EdmProperty"/> representing the inner most property being referenced in the query.
        /// </summary>
        public EdmProperty InnerMostProperty
        {
            get
            {
                if (Next == null)
                {
                    return Property;
                }

                PropertyPath path = Next;

                while (path.Next != null)
                {
                    path = path.Next;
                }

                return path.Property;
            }
        }

        /// <summary>
        /// Gets the next property in the path being referenced in the query, or null if this instance is the last (or only) property in the path.
        /// </summary>
        public PropertyPath Next { get; }

        /// <summary>
        /// Gets the <see cref="EdmProperty"/> representing the property being referenced in the query.
        /// </summary>
        public EdmProperty Property { get; }

        internal LambdaExpression LambdaExpression { get; }

        internal MemberExpression MemberExpression { get; }

        /// <summary>
        /// Creates the <see cref="PropertyPath"/> for the given <see cref="EdmProperty"/>.
        /// </summary>
        /// <param name="property">The <see cref="EdmProperty"/> that the path segment represents.</param>
        /// <returns>The <see cref="PropertyPath"/> for the given EdmProperty.</returns>
        internal static PropertyPath For(EdmProperty property)
            => s_edmPropertyCache.GetOrAdd(property ?? throw new ArgumentNullException(nameof(property)), p => new PropertyPath(p));

        /// <summary>
        /// Creates the <see cref="PropertyPath"/> for the given property path.
        /// </summary>
        /// <param name="edmComplexType">The <see cref="EdmComplexType"/> which contains the first property in the property path.</param>
        /// <param name="rawPropertyPath">The raw property path.</param>
        /// <returns>The <see cref="PropertyPath"/> for the given property path.</returns>
        internal static PropertyPath For(EdmComplexType edmComplexType, string rawPropertyPath)
        {
            if (rawPropertyPath.IndexOf('/') == -1)
            {
                return For(edmComplexType.GetProperty(rawPropertyPath));
            }

            return s_edmComplexTypeCache.GetOrAdd(
                new KeyValuePair<EdmComplexType, string>(edmComplexType, rawPropertyPath),
                kvp =>
                {
                    string[] nameSegments = kvp.Value.Split(SplitCharacter.ForwardSlash);

                    var edmProperties = new EdmProperty[nameSegments.Length];

                    EdmComplexType model = kvp.Key;

                    for (int i = 0; i < nameSegments.Length; i++)
                    {
                        edmProperties[i] = model.GetProperty(nameSegments[i]);

                        // All properties in the path except the last must be navigable.
                        if (i < nameSegments.Length - 1 && !edmProperties[i].IsNavigable)
                        {
                            throw ODataException.BadRequest(ExceptionMessage.PropertyNotNavigable(nameSegments[i], kvp.Value), model.FullName);
                        }

                        model = edmProperties[i].PropertyType as EdmComplexType;
                    }

                    PropertyPath propertyPath = null;

                    for (int i = edmProperties.Length - 1; i >= 0; i--)
                    {
                        propertyPath = new PropertyPath(edmProperties[i], propertyPath);
                    }

                    return propertyPath;
                });
        }
    }
}
