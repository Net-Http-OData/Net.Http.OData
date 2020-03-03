// -----------------------------------------------------------------------
// <copyright file="FilterQueryOptionValidator.cs" company="Project Contributors">
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
using System.Net;

namespace Net.Http.OData.Query.Validators
{
    /// <summary>
    /// A class which validates the $filter query option based upon the <see cref="ODataValidationSettings"/>.
    /// </summary>
    internal static class FilterQueryOptionValidator
    {
        /// <summary>
        /// Validates the specified query options.
        /// </summary>
        /// <param name="queryOptions">The query options.</param>
        /// <param name="validationSettings">The validation settings.</param>
        /// <exception cref="ODataException">Thrown if the validation fails.</exception>
        internal static void Validate(ODataQueryOptions queryOptions, ODataValidationSettings validationSettings)
        {
            if (queryOptions.Filter is null)
            {
                return;
            }

            if ((validationSettings.AllowedQueryOptions & AllowedQueryOptions.Filter) != AllowedQueryOptions.Filter)
            {
                throw new ODataException("The query option $filter is not implemented by this service", HttpStatusCode.NotImplemented);
            }

            ValidateTypeFunctions(queryOptions, validationSettings);
            ValidateStringFunctions(queryOptions, validationSettings);
            ValidateDateTimeFunctions(queryOptions, validationSettings);
            ValidateMathFunctions(queryOptions, validationSettings);
            ValidateLogicalOperators(queryOptions, validationSettings);
            ValidateArithmeticOperators(queryOptions, validationSettings);
        }

        private static void ValidateArithmeticOperators(ODataQueryOptions queryOptions, ODataValidationSettings validationSettings)
        {
            if (validationSettings.AllowedArithmeticOperators == AllowedArithmeticOperators.All)
            {
                return;
            }

            string rawFilterValue = queryOptions.RawValues.Filter;

            if ((validationSettings.AllowedArithmeticOperators & AllowedArithmeticOperators.Add) != AllowedArithmeticOperators.Add
                && rawFilterValue.Contains(" add "))
            {
                throw new ODataException("Unsupported operator add", HttpStatusCode.NotImplemented);
            }

            if ((validationSettings.AllowedArithmeticOperators & AllowedArithmeticOperators.Divide) != AllowedArithmeticOperators.Divide
                && rawFilterValue.Contains(" div "))
            {
                throw new ODataException("Unsupported operator div", HttpStatusCode.NotImplemented);
            }

            if ((validationSettings.AllowedArithmeticOperators & AllowedArithmeticOperators.Modulo) != AllowedArithmeticOperators.Modulo
                && rawFilterValue.Contains(" mod "))
            {
                throw new ODataException("Unsupported operator mod", HttpStatusCode.NotImplemented);
            }

            if ((validationSettings.AllowedArithmeticOperators & AllowedArithmeticOperators.Multiply) != AllowedArithmeticOperators.Multiply
                && rawFilterValue.Contains(" mul "))
            {
                throw new ODataException("Unsupported operator mul", HttpStatusCode.NotImplemented);
            }

            if ((validationSettings.AllowedArithmeticOperators & AllowedArithmeticOperators.Subtract) != AllowedArithmeticOperators.Subtract
                && rawFilterValue.Contains(" sub "))
            {
                throw new ODataException("Unsupported operator sub", HttpStatusCode.NotImplemented);
            }
        }

        private static void ValidateDateTimeFunctions(ODataQueryOptions queryOptions, ODataValidationSettings validationSettings)
        {
            if (validationSettings.AllowedFunctions == AllowedFunctions.AllFunctions
                || validationSettings.AllowedFunctions == AllowedFunctions.AllDateTimeFunctions)
            {
                return;
            }

            string rawFilterValue = queryOptions.RawValues.Filter;

            if ((validationSettings.AllowedFunctions & AllowedFunctions.Date) != AllowedFunctions.Date
                && rawFilterValue.Contains("date("))
            {
                throw new ODataException("Unsupported function date", HttpStatusCode.NotImplemented);
            }

            if ((validationSettings.AllowedFunctions & AllowedFunctions.Day) != AllowedFunctions.Day
                && rawFilterValue.Contains("day("))
            {
                throw new ODataException("Unsupported function day", HttpStatusCode.NotImplemented);
            }

            if ((validationSettings.AllowedFunctions & AllowedFunctions.FractionalSeconds) != AllowedFunctions.FractionalSeconds
                && rawFilterValue.Contains("fractionalseconds("))
            {
                throw new ODataException("Unsupported function fractionalseconds", HttpStatusCode.NotImplemented);
            }

            if ((validationSettings.AllowedFunctions & AllowedFunctions.Hour) != AllowedFunctions.Hour
                && rawFilterValue.Contains("hour("))
            {
                throw new ODataException("Unsupported function hour", HttpStatusCode.NotImplemented);
            }

            if ((validationSettings.AllowedFunctions & AllowedFunctions.MaxDateTime) != AllowedFunctions.MaxDateTime
                && rawFilterValue.Contains("maxdatetime("))
            {
                throw new ODataException("Unsupported function maxdatetime", HttpStatusCode.NotImplemented);
            }

            if ((validationSettings.AllowedFunctions & AllowedFunctions.MinDateTime) != AllowedFunctions.MinDateTime
                && rawFilterValue.Contains("mindatetime("))
            {
                throw new ODataException("Unsupported function mindatetime", HttpStatusCode.NotImplemented);
            }

            if ((validationSettings.AllowedFunctions & AllowedFunctions.Minute) != AllowedFunctions.Minute
                && rawFilterValue.Contains("minute("))
            {
                throw new ODataException("Unsupported function minute", HttpStatusCode.NotImplemented);
            }

            if ((validationSettings.AllowedFunctions & AllowedFunctions.Month) != AllowedFunctions.Month
                && rawFilterValue.Contains("month("))
            {
                throw new ODataException("Unsupported function month", HttpStatusCode.NotImplemented);
            }

            if ((validationSettings.AllowedFunctions & AllowedFunctions.Now) != AllowedFunctions.Now
                && rawFilterValue.Contains("now("))
            {
                throw new ODataException("Unsupported function now", HttpStatusCode.NotImplemented);
            }

            if ((validationSettings.AllowedFunctions & AllowedFunctions.Second) != AllowedFunctions.Second
                && rawFilterValue.Contains("second("))
            {
                throw new ODataException("Unsupported function second", HttpStatusCode.NotImplemented);
            }

            if ((validationSettings.AllowedFunctions & AllowedFunctions.Year) != AllowedFunctions.Year
                && rawFilterValue.Contains("year("))
            {
                throw new ODataException("Unsupported function year", HttpStatusCode.NotImplemented);
            }
        }

        private static void ValidateTypeFunctions(ODataQueryOptions queryOptions, ODataValidationSettings validationSettings)
        {
            if (validationSettings.AllowedFunctions == AllowedFunctions.AllFunctions
                || validationSettings.AllowedFunctions == AllowedFunctions.AllTypeFunctions)
            {
                return;
            }

            string rawFilterValue = queryOptions.RawValues.Filter;

            if ((validationSettings.AllowedFunctions & AllowedFunctions.Cast) != AllowedFunctions.Cast
                && rawFilterValue.Contains("cast("))
            {
                throw new ODataException("Unsupported function cast", HttpStatusCode.NotImplemented);
            }

            if ((validationSettings.AllowedFunctions & AllowedFunctions.IsOf) != AllowedFunctions.IsOf
                && rawFilterValue.Contains("isof("))
            {
                throw new ODataException("Unsupported function isof", HttpStatusCode.NotImplemented);
            }
        }

        private static void ValidateLogicalOperators(ODataQueryOptions queryOptions, ODataValidationSettings validationSettings)
        {
            if (validationSettings.AllowedLogicalOperators == AllowedLogicalOperators.All)
            {
                return;
            }

            string rawFilterValue = queryOptions.RawValues.Filter;

            if ((validationSettings.AllowedLogicalOperators & AllowedLogicalOperators.And) != AllowedLogicalOperators.And
                && rawFilterValue.Contains(" and "))
            {
                throw new ODataException("Unsupported operator and", HttpStatusCode.NotImplemented);
            }

            if ((validationSettings.AllowedLogicalOperators & AllowedLogicalOperators.Or) != AllowedLogicalOperators.Or
                && rawFilterValue.Contains(" or "))
            {
                throw new ODataException("Unsupported operator or", HttpStatusCode.NotImplemented);
            }

            if ((validationSettings.AllowedLogicalOperators & AllowedLogicalOperators.Equal) != AllowedLogicalOperators.Equal
                && rawFilterValue.Contains(" eq "))
            {
                throw new ODataException("Unsupported operator eq", HttpStatusCode.NotImplemented);
            }

            if ((validationSettings.AllowedLogicalOperators & AllowedLogicalOperators.NotEqual) != AllowedLogicalOperators.NotEqual
                && rawFilterValue.Contains(" ne "))
            {
                throw new ODataException("Unsupported operator ne", HttpStatusCode.NotImplemented);
            }

            if ((validationSettings.AllowedLogicalOperators & AllowedLogicalOperators.GreaterThan) != AllowedLogicalOperators.GreaterThan
                && rawFilterValue.Contains(" gt "))
            {
                throw new ODataException("Unsupported operator gt", HttpStatusCode.NotImplemented);
            }

            if ((validationSettings.AllowedLogicalOperators & AllowedLogicalOperators.GreaterThanOrEqual) != AllowedLogicalOperators.GreaterThanOrEqual
                && rawFilterValue.Contains(" ge "))
            {
                throw new ODataException("Unsupported operator ge", HttpStatusCode.NotImplemented);
            }

            if ((validationSettings.AllowedLogicalOperators & AllowedLogicalOperators.LessThan) != AllowedLogicalOperators.LessThan
                && rawFilterValue.Contains(" lt "))
            {
                throw new ODataException("Unsupported operator lt", HttpStatusCode.NotImplemented);
            }

            if ((validationSettings.AllowedLogicalOperators & AllowedLogicalOperators.LessThanOrEqual) != AllowedLogicalOperators.LessThanOrEqual
                && rawFilterValue.Contains(" le "))
            {
                throw new ODataException("Unsupported operator le", HttpStatusCode.NotImplemented);
            }

            if ((validationSettings.AllowedLogicalOperators & AllowedLogicalOperators.Has) != AllowedLogicalOperators.Has
                && rawFilterValue.Contains(" has "))
            {
                throw new ODataException("Unsupported operator has", HttpStatusCode.NotImplemented);
            }

            if ((validationSettings.AllowedLogicalOperators & AllowedLogicalOperators.Not) != AllowedLogicalOperators.Not
                && rawFilterValue.Contains("not "))
            {
                throw new ODataException("Unsupported operator not", HttpStatusCode.NotImplemented);
            }
        }

        private static void ValidateMathFunctions(ODataQueryOptions queryOptions, ODataValidationSettings validationSettings)
        {
            if (validationSettings.AllowedFunctions == AllowedFunctions.AllFunctions
                || validationSettings.AllowedFunctions == AllowedFunctions.AllMathFunctions)
            {
                return;
            }

            string rawFilterValue = queryOptions.RawValues.Filter;

            if ((validationSettings.AllowedFunctions & AllowedFunctions.Ceiling) != AllowedFunctions.Ceiling
                && rawFilterValue.Contains("ceiling("))
            {
                throw new ODataException("Unsupported function ceiling", HttpStatusCode.NotImplemented);
            }

            if ((validationSettings.AllowedFunctions & AllowedFunctions.Floor) != AllowedFunctions.Floor
                && rawFilterValue.Contains("floor("))
            {
                throw new ODataException("Unsupported function floor", HttpStatusCode.NotImplemented);
            }

            if ((validationSettings.AllowedFunctions & AllowedFunctions.Round) != AllowedFunctions.Round
                && rawFilterValue.Contains("round("))
            {
                throw new ODataException("Unsupported function round", HttpStatusCode.NotImplemented);
            }
        }

        private static void ValidateStringFunctions(ODataQueryOptions queryOptions, ODataValidationSettings validationSettings)
        {
            if (validationSettings.AllowedFunctions == AllowedFunctions.AllFunctions
                || validationSettings.AllowedFunctions == AllowedFunctions.AllStringFunctions)
            {
                return;
            }

            string rawFilterValue = queryOptions.RawValues.Filter;

            if ((validationSettings.AllowedFunctions & AllowedFunctions.Contains) != AllowedFunctions.Contains
                && rawFilterValue.Contains("contains("))
            {
                throw new ODataException("Unsupported function contains", HttpStatusCode.NotImplemented);
            }

            if ((validationSettings.AllowedFunctions & AllowedFunctions.Concat) != AllowedFunctions.Concat
                && rawFilterValue.Contains("concat("))
            {
                throw new ODataException("Unsupported function concat", HttpStatusCode.NotImplemented);
            }

            if ((validationSettings.AllowedFunctions & AllowedFunctions.EndsWith) != AllowedFunctions.EndsWith
                && rawFilterValue.Contains("endswith("))
            {
                throw new ODataException("Unsupported function endswith", HttpStatusCode.NotImplemented);
            }

            if ((validationSettings.AllowedFunctions & AllowedFunctions.IndexOf) != AllowedFunctions.IndexOf
                && rawFilterValue.Contains("indexof("))
            {
                throw new ODataException("Unsupported function indexof", HttpStatusCode.NotImplemented);
            }

            if ((validationSettings.AllowedFunctions & AllowedFunctions.Length) != AllowedFunctions.Length
                && rawFilterValue.Contains("length("))
            {
                throw new ODataException("Unsupported function length", HttpStatusCode.NotImplemented);
            }

            if ((validationSettings.AllowedFunctions & AllowedFunctions.StartsWith) != AllowedFunctions.StartsWith
                && rawFilterValue.Contains("startswith("))
            {
                throw new ODataException("Unsupported function startswith", HttpStatusCode.NotImplemented);
            }

            if ((validationSettings.AllowedFunctions & AllowedFunctions.Substring) != AllowedFunctions.Substring
                && rawFilterValue.Contains("substring("))
            {
                throw new ODataException("Unsupported function substring", HttpStatusCode.NotImplemented);
            }

            if ((validationSettings.AllowedFunctions & AllowedFunctions.ToLower) != AllowedFunctions.ToLower
                && rawFilterValue.Contains("tolower("))
            {
                throw new ODataException("Unsupported function tolower", HttpStatusCode.NotImplemented);
            }

            if ((validationSettings.AllowedFunctions & AllowedFunctions.ToUpper) != AllowedFunctions.ToUpper
                && rawFilterValue.Contains("toupper("))
            {
                throw new ODataException("Unsupported function toupper", HttpStatusCode.NotImplemented);
            }

            if ((validationSettings.AllowedFunctions & AllowedFunctions.Trim) != AllowedFunctions.Trim
                && rawFilterValue.Contains("trim("))
            {
                throw new ODataException("Unsupported function trim", HttpStatusCode.NotImplemented);
            }
        }
    }
}
