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
using Net.Http.OData.Query.Expressions;

namespace Net.Http.OData.Query.Linq
{
    internal static class FilterBinder
    {
        internal static IQueryable ApplyFilter(this IQueryable queryable, ODataQueryOptions queryOptions)
        {
            if (queryOptions.Filter == null)
            {
                return queryable;
            }

            var filterBinder = new FilterBinderImpl(queryable, queryOptions);
            return filterBinder.Bind();
        }

        private sealed class FilterBinderImpl
        {
            private readonly IQueryable _queryable;
            private readonly ODataQueryOptions _queryOptions;

            internal FilterBinderImpl(IQueryable queryable, ODataQueryOptions queryOptions)
            {
                _queryable = queryable;
                _queryOptions = queryOptions;
            }

            internal IQueryable Bind()
            {
                Expression predicateBody = Bind(_queryOptions.Filter.Expression);

                LambdaExpression lambdaExpression = Expression.Lambda(
                    typeof(Func<,>).MakeGenericType(_queryOptions.EntitySet.EdmType.ClrType, typeof(bool)),
                    predicateBody,
                    new ParameterExpression[] { _queryOptions.EntitySet.EdmType.ParameterExpression });

                MethodCallExpression whereCallExpression = Expression.Call(
                    typeof(Queryable),
                    "Where",
                    new Type[] { _queryable.ElementType },
                    _queryable.Expression,
                    lambdaExpression);

                return _queryable.Provider.CreateQuery(whereCallExpression);
            }

            private Expression Bind(QueryNode queryNode)
            {
                switch (queryNode.Kind)
                {
                    case QueryNodeKind.BinaryOperator:
                        return Bind((BinaryOperatorNode)queryNode);

                    case QueryNodeKind.Constant:
                        return Bind((ConstantNode)queryNode);

                    case QueryNodeKind.PropertyAccess:
                        return Bind((PropertyAccessNode)queryNode);

                    case QueryNodeKind.UnaryOperator:
                        return Bind((UnaryOperatorNode)queryNode);

                    default:
                        throw new NotSupportedException($"Query nodes of type '{queryNode.Kind}' are not supported by this service.");
                }
            }

            private Expression Bind(BinaryOperatorNode binaryOperatorNode)
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
                        return Expression.Call(leftExpression, typeof(Enum).GetMethod("HasFlag"), Expression.Convert(rightExpression, typeof(Enum)));

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

            private Expression Bind(ConstantNode constantNode)
                => constantNode.Value == null ? Expression.Constant(null) : Expression.Constant(constantNode.Value, constantNode.EdmType.ClrType);

            private Expression Bind(PropertyAccessNode propertyAccessNode)
                => propertyAccessNode.PropertyPath.MemberExpression;

            private Expression Bind(UnaryOperatorNode unaryOperatorNode)
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
}
