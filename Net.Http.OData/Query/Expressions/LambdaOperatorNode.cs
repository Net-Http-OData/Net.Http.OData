// -----------------------------------------------------------------------
// <copyright file="LambdaOperatorNode.cs" company="Project Contributors">
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
    /// A QueryNode which represents a lambda operator.
    /// </summary>
    public sealed class LambdaOperatorNode : QueryNode
    {
        /// <summary>
        /// Initialises a new instance of the <see cref="LambdaOperatorNode"/> class.
        /// </summary>
        /// <param name="parameter">The query node which represents the parameter.</param>
        /// <param name="operatorKind">Kind of the operator.</param>
        /// <param name="alias">The lambda expression alias (e.g. "d:d").</param>
        /// <param name="body">The lambda expression.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="parameter"/> or <paramref name="body"/> is null.</exception>
        internal LambdaOperatorNode(QueryNode parameter, LambdaOperatorKind operatorKind, string alias, QueryNode body)
        {
            Parameter = parameter ?? throw new ArgumentNullException(nameof(parameter));
            OperatorKind = operatorKind;
            Alias = alias ?? throw new ArgumentNullException(nameof(alias));
            Body = body ?? throw new ArgumentNullException(nameof(body));
        }

        /// <summary>
        /// Gets the lambda expression alias (e.g. "d:d").
        /// </summary>
        public string Alias { get; }

        /// <summary>
        /// Gets the lambda body.
        /// </summary>
        public QueryNode Body { get; }

        /// <inheritdoc/>
        public override QueryNodeKind Kind => QueryNodeKind.LambdaOperator;

        /// <summary>
        /// Gets the kind of the operator.
        /// </summary>
        public LambdaOperatorKind OperatorKind { get; }

        /// <summary>
        /// Gets the query node which represents the parameter.
        /// </summary>
        public QueryNode Parameter { get; }
    }
}
