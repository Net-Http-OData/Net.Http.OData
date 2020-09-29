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
using Net.Http.OData.Model;
using Net.Http.OData.Query.Expressions;

namespace Net.Http.OData.Query.Linq
{
    internal static class FilterBinder
    {
        private static readonly PropertyInfo s_dateTimeDay = typeof(DateTime).GetProperty("Day");
        private static readonly PropertyInfo s_dateTimeMonth = typeof(DateTime).GetProperty("Month");
        private static readonly PropertyInfo s_dateTimeOffsetDate = typeof(DateTimeOffset).GetProperty("Date");
        private static readonly PropertyInfo s_dateTimeOffsetDay = typeof(DateTimeOffset).GetProperty("Day");
        private static readonly PropertyInfo s_dateTimeOffsetHour = typeof(DateTimeOffset).GetProperty("Hour");
        private static readonly FieldInfo s_dateTimeOffsetMaxValue = typeof(DateTimeOffset).GetField("MaxValue");
        private static readonly PropertyInfo s_dateTimeOffsetMinute = typeof(DateTimeOffset).GetProperty("Minute");
        private static readonly FieldInfo s_dateTimeOffsetMinValue = typeof(DateTimeOffset).GetField("MinValue");
        private static readonly PropertyInfo s_dateTimeOffsetMonth = typeof(DateTimeOffset).GetProperty("Month");
        private static readonly PropertyInfo s_dateTimeOffsetNow = typeof(DateTimeOffset).GetProperty("Now");
        private static readonly PropertyInfo s_dateTimeOffsetSecond = typeof(DateTimeOffset).GetProperty("Second");
        private static readonly PropertyInfo s_dateTimeOffsetYear = typeof(DateTimeOffset).GetProperty("Year");
        private static readonly PropertyInfo s_dateTimeYear = typeof(DateTime).GetProperty("Year");
        private static readonly MethodInfo s_enumHasFlag = typeof(Enum).GetMethod("HasFlag");
        private static readonly MethodInfo s_mathCeilingDecimal = typeof(Math).GetMethod("Ceiling", new[] { typeof(decimal) });
        private static readonly MethodInfo s_mathCeilingDouble = typeof(Math).GetMethod("Ceiling", new[] { typeof(double) });
        private static readonly MethodInfo s_mathFloorDecimal = typeof(Math).GetMethod("Floor", new[] { typeof(decimal) });
        private static readonly MethodInfo s_mathFloorDouble = typeof(Math).GetMethod("Floor", new[] { typeof(double) });
        private static readonly MethodInfo s_mathRoundDecimal = typeof(Math).GetMethod("Round", new[] { typeof(decimal) });
        private static readonly MethodInfo s_mathRoundDouble = typeof(Math).GetMethod("Round", new[] { typeof(double) });
        private static readonly MethodInfo s_stringConcat = typeof(string).GetMethod("Concat", new[] { typeof(string), typeof(string) });
        private static readonly MethodInfo s_stringContains = typeof(string).GetMethod("Contains", new[] { typeof(string) });
        private static readonly MethodInfo s_stringEndsWith = typeof(string).GetMethod("EndsWith", new[] { typeof(string) });
        private static readonly MethodInfo s_stringIndexOf = typeof(string).GetMethod("IndexOf", new[] { typeof(string) });
        private static readonly PropertyInfo s_stringLength = typeof(string).GetProperty("Length");
        private static readonly MethodInfo s_stringStartsWith = typeof(string).GetMethod("StartsWith", new[] { typeof(string) });
        private static readonly MethodInfo s_stringSubstringInt = typeof(string).GetMethod("Substring", new[] { typeof(int) });
        private static readonly MethodInfo s_stringSubstringIntInt = typeof(string).GetMethod("Substring", new[] { typeof(int), typeof(int) });
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

                case QueryNodeKind.LambdaOperator:
                    return Bind((LambdaOperatorNode)queryNode);

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
                    return Expression.Add(leftExpression, ConvertInNecessary(rightExpression, leftExpression.Type));

                case BinaryOperatorKind.And:
                    return Expression.And(leftExpression, rightExpression);

                case BinaryOperatorKind.Divide:
                    return Expression.Divide(leftExpression, ConvertInNecessary(rightExpression, leftExpression.Type));

                case BinaryOperatorKind.Equal:
                    return Expression.Equal(leftExpression, ConvertInNecessary(rightExpression, leftExpression.Type));

                case BinaryOperatorKind.Has:
                    return Expression.Call(leftExpression, s_enumHasFlag, Expression.Convert(rightExpression, typeof(Enum)));

                case BinaryOperatorKind.GreaterThan:
                    return Expression.GreaterThan(leftExpression, ConvertInNecessary(rightExpression, leftExpression.Type));

                case BinaryOperatorKind.GreaterThanOrEqual:
                    return Expression.GreaterThanOrEqual(leftExpression, ConvertInNecessary(rightExpression, leftExpression.Type));

                case BinaryOperatorKind.LessThan:
                    return Expression.LessThan(leftExpression, ConvertInNecessary(rightExpression, leftExpression.Type));

                case BinaryOperatorKind.LessThanOrEqual:
                    return Expression.LessThanOrEqual(leftExpression, ConvertInNecessary(rightExpression, leftExpression.Type));

                case BinaryOperatorKind.Modulo:
                    return Expression.Modulo(leftExpression, ConvertInNecessary(rightExpression, leftExpression.Type));

                case BinaryOperatorKind.Multiply:
                    return Expression.Multiply(leftExpression, ConvertInNecessary(rightExpression, leftExpression.Type));

                case BinaryOperatorKind.NotEqual:
                    return Expression.NotEqual(leftExpression, ConvertInNecessary(rightExpression, leftExpression.Type));

                case BinaryOperatorKind.Or:
                    return Expression.Or(leftExpression, rightExpression);

                case BinaryOperatorKind.Subtract:
                    return Expression.Subtract(leftExpression, ConvertInNecessary(rightExpression, leftExpression.Type));

                default:
                    throw new NotSupportedException($"Binary query nodes of type '{binaryOperatorNode.OperatorKind}' are not supported by this service.");
            }
        }

        private static Expression Bind(ConstantNode constantNode) =>
            constantNode.Value == null ? Expression.Constant(null) : Expression.Constant(constantNode.Value, constantNode.EdmType.ClrType);

        private static Expression Bind(FunctionCallNode functionCallNode)
        {
            switch (functionCallNode.Name)
            {
                case "ceiling":
                    switch (((PropertyAccessNode)functionCallNode.Parameters[0]).PropertyPath.InnerMostProperty.PropertyType.FullName)
                    {
                        case "Edm.Decimal":
                            return Expression.Call(s_mathCeilingDecimal, Bind(functionCallNode.Parameters[0]));

                        case "Edm.Double":
                            return Expression.Call(s_mathCeilingDouble, Bind(functionCallNode.Parameters[0]));

                        default:
                            throw new NotSupportedException();
                    }

                case "concat":
                    return Expression.Call(s_stringConcat, Bind(functionCallNode.Parameters[0]), Bind(functionCallNode.Parameters[1]));

                case "contains":
                    return Expression.Call(Bind(functionCallNode.Parameters[0]), s_stringContains, Bind(functionCallNode.Parameters[1]));

                case "date":
                    return Expression.Property(Bind(functionCallNode.Parameters[0]), s_dateTimeOffsetDate);

                case "day":
                    switch (((PropertyAccessNode)functionCallNode.Parameters[0]).PropertyPath.InnerMostProperty.PropertyType.FullName)
                    {
                        case "Edm.Date":
                            return Expression.Property(Bind(functionCallNode.Parameters[0]), s_dateTimeDay);

                        case "Edm.DateTimeOffset":
                            return Expression.Property(Bind(functionCallNode.Parameters[0]), s_dateTimeOffsetDay);

                        default:
                            throw new NotSupportedException();
                    }

                case "endswith":
                    return Expression.Call(Bind(functionCallNode.Parameters[0]), s_stringEndsWith, Bind(functionCallNode.Parameters[1]));

                case "floor":
                    switch (((PropertyAccessNode)functionCallNode.Parameters[0]).PropertyPath.InnerMostProperty.PropertyType.FullName)
                    {
                        case "Edm.Decimal":
                            return Expression.Call(s_mathFloorDecimal, Bind(functionCallNode.Parameters[0]));

                        case "Edm.Double":
                            return Expression.Call(s_mathFloorDouble, Bind(functionCallNode.Parameters[0]));

                        default:
                            throw new NotSupportedException();
                    }

                case "hour":
                    switch (((PropertyAccessNode)functionCallNode.Parameters[0]).PropertyPath.InnerMostProperty.PropertyType.FullName)
                    {
                        case "Edm.DateTimeOffset":
                            return Expression.Property(Bind(functionCallNode.Parameters[0]), s_dateTimeOffsetHour);

                        default:
                            throw new NotSupportedException();
                    }

                case "indexof":
                    return Expression.Call(Bind(functionCallNode.Parameters[0]), s_stringIndexOf, Bind(functionCallNode.Parameters[1]));

                case "length":
                    return Expression.Property(Bind(functionCallNode.Parameters[0]), s_stringLength);

                case "maxdatetime":
                    return Expression.Field(null, s_dateTimeOffsetMaxValue);

                case "mindatetime":
                    return Expression.Field(null, s_dateTimeOffsetMinValue);

                case "minute":
                    switch (((PropertyAccessNode)functionCallNode.Parameters[0]).PropertyPath.InnerMostProperty.PropertyType.FullName)
                    {
                        case "Edm.DateTimeOffset":
                            return Expression.Property(Bind(functionCallNode.Parameters[0]), s_dateTimeOffsetMinute);

                        default:
                            throw new NotSupportedException();
                    }

                case "month":
                    switch (((PropertyAccessNode)functionCallNode.Parameters[0]).PropertyPath.InnerMostProperty.PropertyType.FullName)
                    {
                        case "Edm.Date":
                            return Expression.Property(Bind(functionCallNode.Parameters[0]), s_dateTimeMonth);

                        case "Edm.DateTimeOffset":
                            return Expression.Property(Bind(functionCallNode.Parameters[0]), s_dateTimeOffsetMonth);

                        default:
                            throw new NotSupportedException();
                    }

                case "now":
                    return Expression.Property(null, s_dateTimeOffsetNow);

                case "round":
                    switch (((PropertyAccessNode)functionCallNode.Parameters[0]).PropertyPath.InnerMostProperty.PropertyType.FullName)
                    {
                        case "Edm.Decimal":
                            return Expression.Call(s_mathRoundDecimal, Bind(functionCallNode.Parameters[0]));

                        case "Edm.Double":
                            return Expression.Call(s_mathRoundDouble, Bind(functionCallNode.Parameters[0]));

                        default:
                            throw new NotSupportedException();
                    }

                case "second":
                    switch (((PropertyAccessNode)functionCallNode.Parameters[0]).PropertyPath.InnerMostProperty.PropertyType.FullName)
                    {
                        case "Edm.DateTimeOffset":
                            return Expression.Property(Bind(functionCallNode.Parameters[0]), s_dateTimeOffsetSecond);

                        default:
                            throw new NotSupportedException();
                    }

                case "startswith":
                    return Expression.Call(Bind(functionCallNode.Parameters[0]), s_stringStartsWith, Bind(functionCallNode.Parameters[1]));

                case "substring":
                    if (functionCallNode.Parameters.Count == 2)
                    {
                        return Expression.Call(Bind(functionCallNode.Parameters[0]), s_stringSubstringInt, Bind(functionCallNode.Parameters[1]));
                    }
                    else if (functionCallNode.Parameters.Count == 3)
                    {
                        return Expression.Call(Bind(functionCallNode.Parameters[0]), s_stringSubstringIntInt, Bind(functionCallNode.Parameters[1]), Bind(functionCallNode.Parameters[2]));
                    }
                    else
                    {
                        throw new NotSupportedException();
                    }

                case "tolower":
                    return Expression.Call(Bind(functionCallNode.Parameters[0]), s_stringToLower);

                case "toupper":
                    return Expression.Call(Bind(functionCallNode.Parameters[0]), s_stringToUpper);

                case "trim":
                    return Expression.Call(Bind(functionCallNode.Parameters[0]), s_stringTrim);

                case "year":
                    switch (((PropertyAccessNode)functionCallNode.Parameters[0]).PropertyPath.InnerMostProperty.PropertyType.FullName)
                    {
                        case "Edm.Date":
                            return Expression.Property(Bind(functionCallNode.Parameters[0]), s_dateTimeYear);

                        case "Edm.DateTimeOffset":
                            return Expression.Property(Bind(functionCallNode.Parameters[0]), s_dateTimeOffsetYear);

                        default:
                            throw new NotSupportedException();
                    }

                default:
                    throw new NotSupportedException($"The function '{functionCallNode.Name}' is not supported by this service.");
            }
        }

        private static Expression Bind(LambdaOperatorNode lambdaOperatorNode)
        {
            var propertyAccessNode = (PropertyAccessNode)lambdaOperatorNode.Parameter;
            var edmCollectionType = (EdmCollectionType)propertyAccessNode.PropertyPath.InnerMostProperty.PropertyType;

            Expression predicateBody = Bind(lambdaOperatorNode.Body);

            LambdaExpression lambdaExpression = Expression.Lambda(
                typeof(Func<,>).MakeGenericType(edmCollectionType.ContainedType.ClrType, typeof(bool)),
                predicateBody,
                new ParameterExpression[] { ((EdmComplexType)edmCollectionType.ContainedType).ParameterExpression });

            return Expression.Call(
                typeof(Enumerable), // TODO: does this need to target the collection as an IQueryable instead?
                lambdaOperatorNode.OperatorKind.ToString(),
                new Type[] { edmCollectionType.ContainedType.ClrType },
                propertyAccessNode.PropertyPath.MemberExpression,
                lambdaExpression);
        }

        private static Expression Bind(PropertyAccessNode propertyAccessNode) =>
            propertyAccessNode.PropertyPath.MemberExpression;

        private static Expression Bind(UnaryOperatorNode unaryOperatorNode)
        {
            switch (unaryOperatorNode.OperatorKind)
            {
                case UnaryOperatorKind.Not:
                    return Expression.Not(Bind(unaryOperatorNode.Operand));

                default:
                    throw new NotSupportedException($"Unary query nodes of type '{unaryOperatorNode.OperatorKind}' are not supported by this service.");
            }
        }

        private static Expression ConvertInNecessary(Expression expression, Type type) =>
            expression.Type != type ? Expression.Convert(expression, type) : expression;
    }
}
