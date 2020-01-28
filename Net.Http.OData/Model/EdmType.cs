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
    public abstract class EdmType : IEquatable<EdmType>
    {
        /// <summary>
        /// Initialises a new instance of the <see cref="EdmType"/> class.
        /// </summary>
        /// <param name="name">The name of the type.</param>
        /// <param name="fullName">The full name of the type.</param>
        /// <param name="clrType">The CLR type.</param>
        protected EdmType(string name, string fullName, Type clrType)
        {
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
            ClrType = clrType ?? throw new ArgumentNullException(nameof(clrType));
        }

        /// <summary>
        /// Gets the CLR type.
        /// </summary>
        public Type ClrType { get; }

        /// <summary>
        /// Gets the full name.
        /// </summary>
        public string FullName { get; }

        /// <summary>
        /// Gets the name.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets the type for the specified CLR type in the Entity Data Model.
        /// </summary>
        /// <param name="clrType">The CLR type to find in the Entity Data Model.</param>
        /// <returns>The EdmType for the specified CLR type, if found; otherwise, null.</returns>
        public static EdmType GetEdmType(Type clrType)
            => EdmTypeCache.Map.TryGetValue(clrType, out EdmType edmType) ? edmType : default;

        /// <inheritdoc/>
        public override bool Equals(object obj) => Equals(obj as EdmType);

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>
        /// true if the current object is equal to the <paramref name="other" /> parameter; otherwise, false.
        /// </returns>
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
