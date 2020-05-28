// -----------------------------------------------------------------------
// <copyright file="Lexer.cs" company="Project Contributors">
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
using System.Text.RegularExpressions;

namespace Net.Http.OData.Query.Parsers
{
    internal struct Lexer
    {
        // Expect whitespace, a comma or close parenthesis to follow (or the end of the line).
        private const string LookAheadExpected = @"(?=\s|,|\)|$)";

        // Expect whitespace to follow (or the end of the line).
        private const string LookAheadWhitespace = @"(?=\s|$)";

        // More restrictive expressions should be added before less restrictive expressions which could also match.
        // Also, within those bounds then order by the most common first where possible.
        private static readonly TokenDefinition[] s_tokenDefinitions = new[]
        {
            new TokenDefinition(TokenType.Whitespace,           @"\s", ignore: true),
            new TokenDefinition(TokenType.ForwardSlash,         @"\/", ignore: true),
            new TokenDefinition(TokenType.BinaryOperator,       @"(eq|ne|gt|ge|lt|le|has|add|sub|mul|div|mod)" + LookAheadWhitespace),
            new TokenDefinition(TokenType.And,                  @"and" + LookAheadWhitespace),
            new TokenDefinition(TokenType.Or,                   @"or" + LookAheadWhitespace),
            new TokenDefinition(TokenType.True,                 @"true" + LookAheadExpected),
            new TokenDefinition(TokenType.False,                @"false" + LookAheadExpected),
            new TokenDefinition(TokenType.DateTimeOffset,       @"\d{4}-\d{2}-\d{2}T\d{2}:\d{2}(:\d{2}(\.\d{1,12})?)?(Z|[+-]\d{2}:\d{2})?" + LookAheadExpected),
            new TokenDefinition(TokenType.Date,                 @"\d{4}-\d{2}-\d{2}" + LookAheadExpected),
            new TokenDefinition(TokenType.Guid,                 @"[0-9a-fA-F]{8}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{12}" + LookAheadExpected),
            new TokenDefinition(TokenType.Integer,              @"(\+|-)?\d+(l|L)?" + LookAheadExpected),
            new TokenDefinition(TokenType.Decimal,              @"(\+|-)?\d+((m|M)|\.\d+(m|M)?)" + LookAheadExpected),
            new TokenDefinition(TokenType.OpenParentheses,      @"\("),
            new TokenDefinition(TokenType.CloseParentheses,     @"\)"),
            new TokenDefinition(TokenType.Null,                 @"null" + LookAheadExpected),
            new TokenDefinition(TokenType.UnaryOperator,        @"not" + LookAheadWhitespace),
            new TokenDefinition(TokenType.Double,               @"((\+|-)?(\d+(\.\d+)?(d|D)|\d+\.\d+(e|E)\d+)|NaN|INF|-INF)" + LookAheadExpected),
            new TokenDefinition(TokenType.PropertyName,         $@"[a-zA-Z_]+((\w+\/?)+{LookAheadExpected}|(?=/(all|any)\())"),
            new TokenDefinition(TokenType.LambdaOperator,       @"(all|any)+(?=\()"), // Lambda is expected to be followed by a opening '('.
            new TokenDefinition(TokenType.LambdaAlias,          @"[a-z]+:[a-z]+(?=\/)"),
            new TokenDefinition(TokenType.FunctionName,         @"[a-z]+(?=\()"), // Function name is expected to be followed by a opening '('.
            new TokenDefinition(TokenType.String,               @"'(?:''|[\w\s-.~!$&()*+,;=@\\\/]*)*'" + LookAheadExpected),
            new TokenDefinition(TokenType.Comma,                @",(?=\s?)"), // Permit optional whitespace after the comma for spacing between function parameters.
            new TokenDefinition(TokenType.Single,               @"(\+|-)?\d+(\.\d+)?(f|F)" + LookAheadExpected),
            new TokenDefinition(TokenType.TimeOfDay,            @"\d{2}:\d{2}(:\d{2}(\.\d{1,12})?)?" + LookAheadExpected),
            new TokenDefinition(TokenType.Enum,                 @"\w+(\.\w+)+'\w+(\,\w+)*'" + LookAheadExpected),
            new TokenDefinition(TokenType.EdmType,              @"([a-zA-Z]+\.)+[a-zA-Z0-9]+(?=\))"), // EdmType is only expected as the last argument in a function so expect a closing ')'.
            new TokenDefinition(TokenType.Duration,             @"duration'(-)?P\d+DT\d{2}H\d{2}M\d{2}\.\d+S'" + LookAheadExpected),
            new TokenDefinition(TokenType.Base64Binary,         @"binary'[a-zA-Z0-9\+/]*={0,2}'" + LookAheadExpected),
        };

        private readonly string _content;
        private int _position;

        internal Lexer(string content)
        {
            _content = content;
            Current = default;
            _position = content.StartsWith("$filter=", StringComparison.Ordinal) ? content.IndexOf('=') + 1 : 0;
        }

        internal Token Current { get; private set; }

        internal bool MoveNext()
        {
            if (_content.Length == _position)
            {
                return false;
            }

            for (int i = 0; i < s_tokenDefinitions.Length; i++)
            {
                TokenDefinition tokenDefinition = s_tokenDefinitions[i];

                Match match = tokenDefinition.Regex.Match(_content, _position);

                if (match.Success)
                {
                    if (tokenDefinition.Ignore)
                    {
                        _position += match.Length;
                        i = -1;
                        continue;
                    }

                    Current = tokenDefinition.CreateToken(match, _position);
                    _position += match.Length;

                    return true;
                }
            }

            if (_content.Length != _position)
            {
                throw ODataException.BadRequest(ExceptionMessage.GenericUnableToParseFilter, ODataUriNames.FilterQueryOption);
            }

            return false;
        }
    }
}
