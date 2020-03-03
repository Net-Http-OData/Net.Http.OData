// -----------------------------------------------------------------------
// <copyright file="EdmType.cs" company="Project Contributors">
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
using System.Linq;

namespace Net.Http.OData.Model
{
    /// <summary>
    /// A class which represents a type in the Entity Data Model.
    /// </summary>
    [System.Diagnostics.DebuggerDisplay("{FullName}: {ClrType}")]
#pragma warning disable S4035 // Classes implementing "IEquatable<T>" should be sealed
    public abstract class EdmType : IEquatable<EdmType>
#pragma warning restore S4035 // Classes implementing "IEquatable<T>" should be sealed
    {
        /// <summary>
        /// Initialises a new instance of the <see cref="EdmType"/> class.
        /// </summary>
        /// <param name="clrType">The underlying CLR <see cref="Type"/> this type in the Entity Data Model represents.</param>
        protected EdmType(Type clrType)
            : this(clrType, clrType?.Name, clrType?.FullName)
        {
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="EdmType"/> class.
        /// </summary>
        /// <param name="clrType">The underlying CLR <see cref="Type"/> this type in the Entity Data Model represents.</param>
        /// <param name="name">The name of the Entity Data Model Type.</param>
        /// <param name="fullName">The full name of the Entity Data Model Type.</param>
        protected EdmType(Type clrType, string name, string fullName)
        {
            ClrType = clrType ?? throw new ArgumentNullException(nameof(clrType));

            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Name must be specified", nameof(name));
            }

            if (string.IsNullOrWhiteSpace(fullName))
            {
                throw new ArgumentException("FullName must be specified", nameof(fullName));
            }

            Name = name;
            FullName = fullName;
        }

        /// <summary>
        /// Gets the underlying CLR <see cref="Type"/> this type in the Entity Data Model represents.
        /// </summary>
        public Type ClrType { get; }

        /// <summary>
        /// Gets the full name of the Entity Data Model Type.
        /// </summary>
        public string FullName { get; }

        /// <summary>
        /// Gets the name of the Entity Data Model Type.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets the <see cref="EdmType"/> for the specified CLR <see cref="Type"/> in the Entity Data Model.
        /// </summary>
        /// <param name="clrType">The CLR <see cref="Type"/> to find in the Entity Data Model.</param>
        /// <returns>The <see cref="EdmType"/> for the specified CLR <see cref="Type"/>, if found; otherwise, null.</returns>
        public static EdmType GetEdmType(Type clrType)
            => EdmTypeCache.Map.TryGetValue(clrType, out EdmType edmType) ? edmType : default;

        /// <inheritdoc/>
        public override bool Equals(object obj) => Equals(obj as EdmType);

        /// <inheritdoc/>
        public bool Equals(EdmType other)
        {
            if (other is null)
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return ClrType.Equals(other.ClrType);
        }

        /// <inheritdoc/>
        public override int GetHashCode() => ClrType.GetHashCode();

        /// <inheritdoc/>
        public override string ToString() => FullName;

        /// <summary>
        /// Gets the type with the specified name in the Entity Data Model.
        /// </summary>
        /// <param name="edmTypeName">Name of the type in the Entity Data Model.</param>
        /// <returns>The EdmType with the specified name, if found; otherwise, null.</returns>
        /// <remarks>
        /// This method shouldn't be public, there are multiple System.Types mapped to the same EdmType name.
        /// At present, this method is only used to resolve Enums.
        /// </remarks>
        internal static EdmType GetEdmType(string edmTypeName)
            => EdmTypeCache.Map.Values.FirstOrDefault(t => t.FullName.Equals(edmTypeName, StringComparison.Ordinal));
    }
}
