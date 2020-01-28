// -----------------------------------------------------------------------
// <copyright file="EdmProperty.cs" company="Project Contributors">
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
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace Net.Http.OData.Model
{
    /// <summary>
    /// A class which represents an entity property in the Entity Data Model.
    /// </summary>
    [System.Diagnostics.DebuggerDisplay("{Name}")]
    public sealed class EdmProperty
    {
        private readonly Func<EdmType, bool> _isNavigableFunc;

        /// <summary>
        /// Initialises a new instance of the <see cref="EdmProperty" /> class.
        /// </summary>
        /// <param name="propertyInfo">The PropertyInfo for the property.</param>
        /// <param name="propertyType">Type of the edm.</param>
        /// <param name="declaringType">Type of the declaring.</param>
        /// <param name="isNavigableFunc">A function which indicates whether the property is a navigation property.</param>
        /// <exception cref="ArgumentNullException">Constructor argument not specified.</exception>
        internal EdmProperty(PropertyInfo propertyInfo, EdmType propertyType, EdmComplexType declaringType, Func<EdmType, bool> isNavigableFunc)
        {
            Name = propertyInfo?.Name ?? throw new ArgumentNullException(nameof(propertyInfo));
            PropertyType = propertyType ?? throw new ArgumentNullException(nameof(propertyType));
            DeclaringType = declaringType ?? throw new ArgumentNullException(nameof(declaringType));
            _isNavigableFunc = isNavigableFunc;

            IsNullable = Nullable.GetUnderlyingType(propertyType.ClrType) != null
                || ((propertyType.ClrType.IsClass || propertyType.ClrType.IsInterface) && propertyInfo.GetCustomAttribute<RequiredAttribute>() == null);
        }

        /// <summary>
        /// Gets the type in the Entity Data Model which declares this property.
        /// </summary>
        public EdmComplexType DeclaringType { get; }

        /// <summary>
        /// Gets a value indicating whether the property is navigable (i.e. a navigation property).
        /// </summary>
        public bool IsNavigable => _isNavigableFunc((PropertyType as EdmCollectionType)?.ContainedType ?? PropertyType);

        /// <summary>
        /// Gets a value indicating whether the property is nullable.
        /// </summary>
        public bool IsNullable { get; }

        /// <summary>
        /// Gets the name of the property.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets the type of the property in the Entity Data Model.
        /// </summary>
        public EdmType PropertyType { get; }

        /// <inheritdoc/>
        public override string ToString() => Name;
    }
}
