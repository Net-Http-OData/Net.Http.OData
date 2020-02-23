// -----------------------------------------------------------------------
// <copyright file="TokenDefinition.cs" company="Project Contributors">
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
using System.Text.RegularExpressions;

namespace Net.Http.OData.Query.Parsers
{
    [System.Diagnostics.DebuggerDisplay("{tokenType}: {Regex}")]
    internal struct TokenDefinition
    {
        private readonly TokenType _tokenType;

        internal TokenDefinition(TokenType tokenType, string expression)
            : this(tokenType, expression, false)
        {
        }

        internal TokenDefinition(TokenType tokenType, string expression, bool ignore)
        {
            _tokenType = tokenType;
            Regex = new Regex(@"\G" + expression, RegexOptions.Singleline);
            Ignore = ignore;
        }

        internal bool Ignore { get; }

        internal Regex Regex { get; }

        internal Token CreateToken(Match match) => new Token(_tokenType, match.Value);
    }
}
