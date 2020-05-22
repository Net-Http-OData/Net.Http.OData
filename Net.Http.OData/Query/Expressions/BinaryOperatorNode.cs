// -----------------------------------------------------------------------
// <copyright file="BinaryOperatorNode.cs" company="Project Contributors">
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
    /// A QueryNode which represents a binary operator with a left and right branch.
    /// </summary>
    [System.Diagnostics.DebuggerDisplay("{Left} {OperatorKind} {Right}")]
    public sealed class BinaryOperatorNode : QueryNode
    {
        /// <summary>
        /// Initialises a new instance of the <see cref="BinaryOperatorNode"/> class.
        /// </summary>
        /// <param name="left">The left query node.</param>
        /// <param name="operatorKind">Kind of the operator.</param>
        /// <param name="right">The right query node.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="left"/> is null.</exception>
        internal BinaryOperatorNode(QueryNode left, BinaryOperatorKind operatorKind, QueryNode right)
        {
            Left = left ?? throw new ArgumentNullException(nameof(left));
            OperatorKind = operatorKind;
            Right = right;
        }

        /// <inheritdoc/>
        public override QueryNodeKind Kind => QueryNodeKind.BinaryOperator;

        /// <summary>
        /// Gets the left query node.
        /// </summary>
        public QueryNode Left { get; }

        /// <summary>
        /// Gets the kind of the operator.
        /// </summary>
        public BinaryOperatorKind OperatorKind { get; }

        /// <summary>
        /// Gets the right query node.
        /// </summary>
        public QueryNode Right { get; internal set; }
    }
}
