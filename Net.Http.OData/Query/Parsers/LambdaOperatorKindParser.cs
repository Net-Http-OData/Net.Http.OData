// -----------------------------------------------------------------------
// <copyright file="LambdaOperatorKindParser.cs" company="Project Contributors">
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
using Net.Http.OData.Query.Expressions;

namespace Net.Http.OData.Query.Parsers
{
    internal static class LambdaOperatorKindParser
    {
        private static readonly Dictionary<string, LambdaOperatorKind> s_operatorTypeMap = new Dictionary<string, LambdaOperatorKind>
        {
            ["all"] = LambdaOperatorKind.All,
            ["any"] = LambdaOperatorKind.Any,
        };

        internal static LambdaOperatorKind ToLambdaOperatorKind(this string operatorType)
            => s_operatorTypeMap.TryGetValue(operatorType, out LambdaOperatorKind binaryOperatorKind)
                ? binaryOperatorKind
                : throw ODataException.BadRequest(ExceptionMessage.InvalidOperator(operatorType), ODataUriNames.FilterQueryOption);
    }
}
