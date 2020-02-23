﻿// -----------------------------------------------------------------------
// <copyright file="Token.cs" company="Project Contributors">
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
namespace Net.Http.OData.Query.Parsers
{
    [System.Diagnostics.DebuggerDisplay("{TokenType}: {Value}")]
    internal struct Token
    {
        internal Token(TokenType tokenType, string value)
        {
            Value = value;
            TokenType = tokenType;
        }

        internal TokenType TokenType { get; }

        internal string Value { get; }
    }
}
