// -----------------------------------------------------------------------
// <copyright file="ExpressionExtensions.cs" company="Project Contributors">
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

using System.Linq.Expressions;
using System.Reflection;

namespace Net.Http.OData.Model
{
    internal static class ExpressionExtensions
    {
        internal static MemberInfo GetMemberInfo(this Expression expression)
        {
            var lambdaExpression = (LambdaExpression)expression;

            MemberExpression memberExpression = lambdaExpression.Body is UnaryExpression unaryExpression
                ? (MemberExpression)unaryExpression.Operand
                : (MemberExpression)lambdaExpression.Body;

            return memberExpression.Member;
        }
    }
}
