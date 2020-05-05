// -----------------------------------------------------------------------
// <copyright file="EdmComplexType.cs" company="Project Contributors">
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

namespace Net.Http.OData.Model
{
    /// <summary>
    /// A class which represents a complex type in the Entity Data Model.
    /// </summary>
    /// <seealso cref="EdmType" />
    [System.Diagnostics.DebuggerDisplay("{FullName}: {ClrType}")]
    public sealed class EdmComplexType : EdmType
    {
        internal EdmComplexType(Type clrType, IReadOnlyList<EdmProperty> properties)
            : this(clrType, properties, null)
        {
        }

        internal EdmComplexType(Type clrType, IReadOnlyList<EdmProperty> properties, EdmType baseType)
            : base(clrType)
        {
            Properties = properties ?? throw new ArgumentNullException(nameof(properties));
            BaseType = baseType;
        }

        /// <summary>
        /// Gets the <see cref="EdmType"/> from which this <see cref="EdmComplexType"/> directly inherits.
        /// </summary>
        public EdmType BaseType { get; }

        /// <summary>
        /// Gets the <see cref="EdmProperty"/> instances representing the properties defined on this type.
        /// </summary>
        public IReadOnlyList<EdmProperty> Properties { get; }

        /// <summary>
        /// Gets the <see cref="EdmProperty"/> declared in this type with the specified name.
        /// </summary>
        /// <param name="name">The name of the property.</param>
        /// <returns>The <see cref="EdmProperty"/> declared in this type with the specified name.</returns>
        /// <exception cref="ODataException">The type does not contain a property with the specified name.</exception>
        public EdmProperty GetProperty(string name)
        {
            for (int i = 0; i < Properties.Count; i++)
            {
                EdmProperty property = Properties[i];

                if (property.Name.Equals(name, StringComparison.Ordinal))
                {
                    return property;
                }
            }

            throw ODataException.BadRequest(ExceptionMessage.EdmTypeDoesNotContainProperty(FullName, name), FullName);
        }
    }
}
