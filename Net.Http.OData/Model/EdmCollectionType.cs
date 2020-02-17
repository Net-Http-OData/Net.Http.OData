// -----------------------------------------------------------------------
// <copyright file="EdmCollectionType.cs" company="Project Contributors">
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

namespace Net.Http.OData.Model
{
    /// <summary>
    /// A class which represents a collection type in the Entity Data Model.
    /// </summary>
    /// <seealso cref="EdmType"/>
    [System.Diagnostics.DebuggerDisplay("{FullName}: {ClrType}")]
    public sealed class EdmCollectionType : EdmType
    {
        internal EdmCollectionType(Type clrType, EdmType containedType)
            : base("Collection", $"Collection({containedType?.FullName})", clrType)
            => ContainedType = containedType ?? throw new ArgumentNullException(nameof(containedType));

        /// <summary>
        /// Gets the <see cref="EdmType"/> contained in the collection.
        /// </summary>
        public EdmType ContainedType { get; }
    }
}
