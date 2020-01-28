﻿// -----------------------------------------------------------------------
// <copyright file="FunctionCallNode.cs" company="Project Contributors">
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
using System.Collections.Generic;

namespace Net.Http.OData.Query.Expressions
{
    /// <summary>
    /// A QueryNode which represents a function call.
    /// </summary>
    [System.Diagnostics.DebuggerDisplay("{Name}")]
    public sealed class FunctionCallNode : QueryNode
    {
        /// <summary>
        /// Initialises a new instance of the <see cref="FunctionCallNode" /> class.
        /// </summary>
        /// <param name="name">The name of the function.</param>
        internal FunctionCallNode(string name)
        {
            Name = name;
        }

        /// <inheritdoc/>
        public override QueryNodeKind Kind { get; } = QueryNodeKind.FunctionCall;

        /// <summary>
        /// Gets the name of the function.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets the parameters for the function call.
        /// </summary>
        public IReadOnlyList<QueryNode> Parameters { get; } = new List<QueryNode>();

        internal void AddParameter(QueryNode queryNode) => ((IList<QueryNode>)Parameters).Add(queryNode);
    }
}
