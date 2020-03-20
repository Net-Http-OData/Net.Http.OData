// -----------------------------------------------------------------------
// <copyright file="ConstantNode{T}.cs" company="Project Contributors">
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

using Net.Http.OData.Model;

namespace Net.Http.OData.Query.Expressions
{
    /// <summary>
    /// A QueryNode which represents a strongly typed constant value.
    /// </summary>
    /// <typeparam name="T">The type of the constant node value.</typeparam>
    public sealed class ConstantNode<T> : ConstantNode
    {
        /// <summary>
        /// Initialises a new instance of the <see cref="ConstantNode{T}" /> class.
        /// </summary>
        /// <param name="edmType">The <see cref="EdmType"/> of the value.</param>
        /// <param name="literalText">The literal text.</param>
        /// <param name="value">The value.</param>
        internal ConstantNode(EdmType edmType, string literalText, T value)
            : base(edmType, literalText)
            => Value = value;

        /// <summary>
        /// Gets the constant value.
        /// </summary>
        public new T Value { get; }

        /// <inheritdoc/>
        protected override object ValueAsObject() => Value;
    }
}
