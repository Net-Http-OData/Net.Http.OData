// -----------------------------------------------------------------------
// <copyright file="TokenType.cs" company="Project Contributors">
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
#pragma warning disable SA1124 // Do not use regions

    internal enum TokenType
    {
        #region Syntax

        And,
        BinaryOperator,
        Comma,
        CloseParentheses,
        FunctionName,
        PropertyName,
        OpenParentheses,
        Or,
        UnaryOperator,
        Whitespace,

        #endregion Syntax

        #region DataTypes

        Base64Binary,
        Date,
        DateTimeOffset,
        Decimal,
        Double,
        Duration,
        Enum,
        False,
        Guid,
        Integer,
        Null,
        Single,
        String,
        TimeOfDay,
        True,

        #endregion DataTypes
    }

#pragma warning restore SA1124 // Do not use regions
}
