﻿// -----------------------------------------------------------------------
// <copyright file="QueryNode.cs" company="Project Contributors">
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
namespace Net.Http.OData.Query.Expressions
{
    /// <summary>
    /// The base class for a query node.
    /// </summary>
    public abstract class QueryNode
    {
        /// <summary>
        /// Gets the kind of query node.
        /// </summary>
        public abstract QueryNodeKind Kind { get; }
    }
}
