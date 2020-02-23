// -----------------------------------------------------------------------
// <copyright file="PropertyAccessNode.cs" company="Project Contributors">
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

namespace Net.Http.OData.Query.Expressions
{
    /// <summary>
    /// A QueryNode which represents a property.
    /// </summary>
    [System.Diagnostics.DebuggerDisplay("{PropertyPath}")]
    public sealed class PropertyAccessNode : QueryNode
    {
        /// <summary>
        /// Initialises a new instance of the <see cref="PropertyAccessNode"/> class.
        /// </summary>
        /// <param name="propertyPath">The property path being referenced in the query.</param>
        internal PropertyAccessNode(PropertyPath propertyPath)
            => PropertyPath = propertyPath ?? throw new ArgumentNullException(nameof(propertyPath));

        /// <inheritdoc/>
        public override QueryNodeKind Kind { get; } = QueryNodeKind.PropertyAccess;

        /// <summary>
        /// Gets the property path being referenced in the query.
        /// </summary>
        public PropertyPath PropertyPath { get; }
    }
}
