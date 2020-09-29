// -----------------------------------------------------------------------
// <copyright file="EdmEnumType.cs" company="Project Contributors">
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
    /// A class which represents an Enum type in the Entity Data Model.
    /// </summary>
    /// <seealso cref="EdmType" />
    [System.Diagnostics.DebuggerDisplay("{Name}: {ClrType}")]
    public sealed class EdmEnumType : EdmType
    {
        internal EdmEnumType(Type clrType, IReadOnlyList<EdmEnumMember> members)
            : base(clrType) =>
            Members = members ?? throw new ArgumentNullException(nameof(members));

        /// <summary>
        /// Gets the <see cref="EdmEnumMember"/>s that represent the values of the underlying enum.
        /// </summary>
        public IReadOnlyList<EdmEnumMember> Members { get; }

        /// <summary>
        /// Gets the CLR Enum value for the specified Enum member in the Entity Data Model.
        /// </summary>
        /// <param name="value">The Enum string value in the Entity Data Model.</param>
        /// <returns>An object containing the CLR Enum value.</returns>
        public object GetClrValue(string value) => Enum.Parse(ClrType, value);
    }
}
