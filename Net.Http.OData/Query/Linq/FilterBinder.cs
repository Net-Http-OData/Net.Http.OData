// <copyright file="FilterBinder.cs" company="Project Contributors">
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
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Net.Http.OData.Query.Expressions;

namespace Net.Http.OData.Query.Linq
{
    internal static class FilterBinder
    {
        private static readonly MethodInfo s_enumHasFlag = typeof(Enum).GetMethod("HasFlag");

        private static readonly MethodInfo s_stringConcat = typeof(string).GetMethod("Concat", new[] { typeof(string), typeof(string) });

        private static readonly MethodInfo s_stringContains = typeof(string).GetMethod("Contains", new[] { typeof(string) });

        private static readonly MethodInfo s_stringEndsWith = typeof(string).GetMethod("EndsWith", new[] { typeof(string) });

        private static readonly MethodInfo s_stringIndexOf = typeof(string).GetMethod("IndexOf", new[] { typeof(string) });

        private static readonly PropertyInfo s_stringLength = typeof(string).GetProperty("Length");

        private static readonly MethodInfo s_stringStartsWith = typeof(string).GetMethod("StartsWith", new[] { typeof(string) });

        private static readonly MethodInfo s_stringSubstring = typeof(string).GetMethod("Substring", new[] { typeof(int) });

        private static readonly MethodInfo s_stringToLower = typeof(string).GetMethod("ToLower", Type.EmptyTypes);

        private static readonly MethodInfo s_stringToUpper = typeof(string).GetMethod("ToUpper", Type.EmptyTypes);

        private static readonly MethodInfo s_stringTrim = typeof(string).GetMethod("Trim", Type.EmptyTypes);

        internal static IQueryable ApplyFilter(this IQueryable queryable, ODataQueryOptions queryOptions)
        {
            if (queryOptions.Filter == null)
            {
                return queryable;
            }

            Expression predicateBody = Bind(queryOptions.Filter.Expression);

            LambdaExpression lambdaExpression = Expression.Lambda(
                typeof(Func<,>).MakeGenericType(queryOptions.EntitySet.EdmType.ClrType, typeof(bool)),
                predicateBody,
                new ParameterExpression[] { queryOptions.EntitySet.EdmType.ParameterExpression });

            MethodCallExpression whereCallExpression = Expression.Call(
                typeof(Queryable),
                "Where",
                new Type[] { queryable.ElementType },
                queryable.Expression,
                lambdaExpression);

            return queryable.Provider.CreateQuery(whereCallExpression);
        }

        private static Expression Bind(QueryNode queryNode)
        {
            switch (queryNode.Kind)
            {
                case QueryNodeKind.BinaryOperator:
                    return Bind((BinaryOperatorNode)queryNode);

                case QueryNodeKind.Constant:
                    return Bind((ConstantNode)queryNode);

                case QueryNodeKind.FunctionCall:
                    return Bind((FunctionCallNode)queryNode);

                case QueryNodeKind.PropertyAccess:
                    return Bind((PropertyAccessNode)queryNode);

                case QueryNodeKind.UnaryOperator:
                    return Bind((UnaryOperatorNode)queryNode);

                default:
                    throw new NotSupportedException($"Query nodes of type '{queryNode.Kind}' are not supported by this service.");
            }
        }

        private static Expression Bind(BinaryOperatorNode binaryOperatorNode)
        {
            Expression leftExpression = Bind(binaryOperatorNode.Left);
            Expression rightExpression = Bind(binaryOperatorNode.Right);

            switch (binaryOperatorNode.OperatorKind)
            {
                case BinaryOperatorKind.Add:
                    if (leftExpression.Type != rightExpression.Type)
                    {
                        rightExpression = Expression.Convert(rightExpression, leftExpression.Type);
                    }

                    return Expression.Add(leftExpression, rightExpression);

                case BinaryOperatorKind.And:
                    return Expression.And(leftExpression, rightExpression);

                case BinaryOperatorKind.Divide:
                    if (leftExpression.Type != rightExpression.Type)
                    {
                        rightExpression = Expression.Convert(rightExpression, leftExpression.Type);
                    }

                    return Expression.Divide(leftExpression, rightExpression);

                case BinaryOperatorKind.Equal:
                    return Expression.Equal(leftExpression, rightExpression);

                case BinaryOperatorKind.Has:
                    return Expression.Call(leftExpression, s_enumHasFlag, Expression.Convert(rightExpression, typeof(Enum)));

                case BinaryOperatorKind.GreaterThan:
                    return Expression.GreaterThan(leftExpression, rightExpression);

                case BinaryOperatorKind.GreaterThanOrEqual:
                    return Expression.GreaterThanOrEqual(leftExpression, rightExpression);

                case BinaryOperatorKind.LessThan:
                    return Expression.LessThan(leftExpression, rightExpression);

                case BinaryOperatorKind.LessThanOrEqual:
                    return Expression.LessThanOrEqual(leftExpression, rightExpression);

                case BinaryOperatorKind.Modulo:
                    if (leftExpression.Type != rightExpression.Type)
                    {
                        rightExpression = Expression.Convert(rightExpression, leftExpression.Type);
                    }

                    return Expression.Modulo(leftExpression, rightExpression);

                case BinaryOperatorKind.Multiply:
                    if (leftExpression.Type != rightExpression.Type)
                    {
                        rightExpression = Expression.Convert(rightExpression, leftExpression.Type);
                    }

                    return Expression.Multiply(leftExpression, rightExpression);

                case BinaryOperatorKind.NotEqual:
                    return Expression.NotEqual(leftExpression, rightExpression);

                case BinaryOperatorKind.Or:
                    return Expression.Or(leftExpression, rightExpression);

                case BinaryOperatorKind.Subtract:
                    if (leftExpression.Type != rightExpression.Type)
                    {
                        rightExpression = Expression.Convert(rightExpression, leftExpression.Type);
                    }

                    return Expression.Subtract(leftExpression, rightExpression);

                default:
                    throw new NotSupportedException($"Binary query nodes of type '{binaryOperatorNode.OperatorKind}' are not supported by this service.");
            }
        }

        private static Expression Bind(ConstantNode constantNode)
            => constantNode.Value == null ? Expression.Constant(null) : Expression.Constant(constantNode.Value, constantNode.EdmType.ClrType);

        private static Expression Bind(FunctionCallNode functionCallNode)
        {
            switch (functionCallNode.Name)
            {
                case "concat":
                    return Expression.Call(s_stringConcat, Bind(functionCallNode.Parameters[0]), Bind(functionCallNode.Parameters[1]));

                case "contains":
                    return Expression.Call(Bind(functionCallNode.Parameters[0]), s_stringContains, Bind(functionCallNode.Parameters[1]));

                case "endswith":
                    return Expression.Call(Bind(functionCallNode.Parameters[0]), s_stringEndsWith, Bind(functionCallNode.Parameters[1]));

                case "indexof":
                    return Expression.Call(Bind(functionCallNode.Parameters[0]), s_stringIndexOf, Bind(functionCallNode.Parameters[1]));

                case "length":
                    return Expression.Property(Bind(functionCallNode.Parameters[0]), s_stringLength);

                case "startswith":
                    return Expression.Call(Bind(functionCallNode.Parameters[0]), s_stringStartsWith, Bind(functionCallNode.Parameters[1]));

                case "substring":
                    return Expression.Call(Bind(functionCallNode.Parameters[0]), s_stringSubstring, Bind(functionCallNode.Parameters[1]));

                case "tolower":
                    return Expression.Call(Bind(functionCallNode.Parameters[0]), s_stringToLower);

                case "toupper":
                    return Expression.Call(Bind(functionCallNode.Parameters[0]), s_stringToUpper);

                case "trim":
                    return Expression.Call(Bind(functionCallNode.Parameters[0]), s_stringTrim);

                default:
                    throw new NotSupportedException($"The function '{functionCallNode.Name}' is not supported by this service.");
            }
        }

        private static Expression Bind(PropertyAccessNode propertyAccessNode)
            => propertyAccessNode.PropertyPath.MemberExpression;

        private static Expression Bind(UnaryOperatorNode unaryOperatorNode)
        {
            switch (unaryOperatorNode.OperatorKind)
            {
                case UnaryOperatorKind.Not:
                    return Expression.Not(Bind(unaryOperatorNode.Operand));

                default:
                    throw new NotSupportedException($"Unar query nodes of type '{unaryOperatorNode.OperatorKind}' are not supported by this service.");
            }
        }
    }
}
