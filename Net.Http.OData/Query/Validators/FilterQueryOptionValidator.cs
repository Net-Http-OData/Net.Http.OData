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
using System;

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
                throw ODataException.NotImplemented("The query option $filter is not implemented by this service", ODataUriNames.FilterQueryOption);
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
                throw ODataException.NotImplemented("Unsupported operator add", ODataUriNames.FilterQueryOption);
            }

            if ((validationSettings.AllowedArithmeticOperators & AllowedArithmeticOperators.Divide) != AllowedArithmeticOperators.Divide
                && rawFilterValue.Contains(" div "))
            {
                throw ODataException.NotImplemented("Unsupported operator div", ODataUriNames.FilterQueryOption);
            }

            if ((validationSettings.AllowedArithmeticOperators & AllowedArithmeticOperators.Modulo) != AllowedArithmeticOperators.Modulo
                && rawFilterValue.Contains(" mod "))
            {
                throw ODataException.NotImplemented("Unsupported operator mod", ODataUriNames.FilterQueryOption);
            }

            if ((validationSettings.AllowedArithmeticOperators & AllowedArithmeticOperators.Multiply) != AllowedArithmeticOperators.Multiply
                && rawFilterValue.Contains(" mul "))
            {
                throw ODataException.NotImplemented("Unsupported operator mul", ODataUriNames.FilterQueryOption);
            }

            if ((validationSettings.AllowedArithmeticOperators & AllowedArithmeticOperators.Subtract) != AllowedArithmeticOperators.Subtract
                && rawFilterValue.Contains(" sub "))
            {
                throw ODataException.NotImplemented("Unsupported operator sub", ODataUriNames.FilterQueryOption);
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
                throw ODataException.NotImplemented("Unsupported function date", ODataUriNames.FilterQueryOption);
            }

            if ((validationSettings.AllowedFunctions & AllowedFunctions.Day) != AllowedFunctions.Day
                && rawFilterValue.Contains("day("))
            {
                throw ODataException.NotImplemented("Unsupported function day", ODataUriNames.FilterQueryOption);
            }

            if ((validationSettings.AllowedFunctions & AllowedFunctions.FractionalSeconds) != AllowedFunctions.FractionalSeconds
                && rawFilterValue.Contains("fractionalseconds("))
            {
                throw ODataException.NotImplemented("Unsupported function fractionalseconds", ODataUriNames.FilterQueryOption);
            }

            if ((validationSettings.AllowedFunctions & AllowedFunctions.Hour) != AllowedFunctions.Hour
                && rawFilterValue.Contains("hour("))
            {
                throw ODataException.NotImplemented("Unsupported function hour", ODataUriNames.FilterQueryOption);
            }

            if ((validationSettings.AllowedFunctions & AllowedFunctions.MaxDateTime) != AllowedFunctions.MaxDateTime
                && rawFilterValue.Contains("maxdatetime("))
            {
                throw ODataException.NotImplemented("Unsupported function maxdatetime", ODataUriNames.FilterQueryOption);
            }

            if ((validationSettings.AllowedFunctions & AllowedFunctions.MinDateTime) != AllowedFunctions.MinDateTime
                && rawFilterValue.Contains("mindatetime("))
            {
                throw ODataException.NotImplemented("Unsupported function mindatetime", ODataUriNames.FilterQueryOption);
            }

            if ((validationSettings.AllowedFunctions & AllowedFunctions.Minute) != AllowedFunctions.Minute
                && rawFilterValue.Contains("minute("))
            {
                throw ODataException.NotImplemented("Unsupported function minute", ODataUriNames.FilterQueryOption);
            }

            if ((validationSettings.AllowedFunctions & AllowedFunctions.Month) != AllowedFunctions.Month
                && rawFilterValue.Contains("month("))
            {
                throw ODataException.NotImplemented("Unsupported function month", ODataUriNames.FilterQueryOption);
            }

            if ((validationSettings.AllowedFunctions & AllowedFunctions.Now) != AllowedFunctions.Now
                && rawFilterValue.Contains("now("))
            {
                throw ODataException.NotImplemented("Unsupported function now", ODataUriNames.FilterQueryOption);
            }

            if ((validationSettings.AllowedFunctions & AllowedFunctions.Second) != AllowedFunctions.Second
                && rawFilterValue.Contains("second("))
            {
                throw ODataException.NotImplemented("Unsupported function second", ODataUriNames.FilterQueryOption);
            }

            if ((validationSettings.AllowedFunctions & AllowedFunctions.Time) != AllowedFunctions.Time
                && (rawFilterValue.StartsWith("$filter=time(", StringComparison.Ordinal) || rawFilterValue.Contains(" time(")))
            {
                throw ODataException.NotImplemented("Unsupported function time", ODataUriNames.FilterQueryOption);
            }

            if ((validationSettings.AllowedFunctions & AllowedFunctions.TotalOffsetMinutes) != AllowedFunctions.TotalOffsetMinutes
                && rawFilterValue.Contains("totaloffsetminutes("))
            {
                throw ODataException.NotImplemented("Unsupported function totaloffsetminutes", ODataUriNames.FilterQueryOption);
            }

            if ((validationSettings.AllowedFunctions & AllowedFunctions.TotalSeconds) != AllowedFunctions.TotalSeconds
                && rawFilterValue.Contains("totalseconds("))
            {
                throw ODataException.NotImplemented("Unsupported function totalseconds", ODataUriNames.FilterQueryOption);
            }

            if ((validationSettings.AllowedFunctions & AllowedFunctions.Year) != AllowedFunctions.Year
                && rawFilterValue.Contains("year("))
            {
                throw ODataException.NotImplemented("Unsupported function year", ODataUriNames.FilterQueryOption);
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
                throw ODataException.NotImplemented("Unsupported operator and", ODataUriNames.FilterQueryOption);
            }

            if ((validationSettings.AllowedLogicalOperators & AllowedLogicalOperators.Or) != AllowedLogicalOperators.Or
                && rawFilterValue.Contains(" or "))
            {
                throw ODataException.NotImplemented("Unsupported operator or", ODataUriNames.FilterQueryOption);
            }

            if ((validationSettings.AllowedLogicalOperators & AllowedLogicalOperators.Equal) != AllowedLogicalOperators.Equal
                && rawFilterValue.Contains(" eq "))
            {
                throw ODataException.NotImplemented("Unsupported operator eq", ODataUriNames.FilterQueryOption);
            }

            if ((validationSettings.AllowedLogicalOperators & AllowedLogicalOperators.NotEqual) != AllowedLogicalOperators.NotEqual
                && rawFilterValue.Contains(" ne "))
            {
                throw ODataException.NotImplemented("Unsupported operator ne", ODataUriNames.FilterQueryOption);
            }

            if ((validationSettings.AllowedLogicalOperators & AllowedLogicalOperators.GreaterThan) != AllowedLogicalOperators.GreaterThan
                && rawFilterValue.Contains(" gt "))
            {
                throw ODataException.NotImplemented("Unsupported operator gt", ODataUriNames.FilterQueryOption);
            }

            if ((validationSettings.AllowedLogicalOperators & AllowedLogicalOperators.GreaterThanOrEqual) != AllowedLogicalOperators.GreaterThanOrEqual
                && rawFilterValue.Contains(" ge "))
            {
                throw ODataException.NotImplemented("Unsupported operator ge", ODataUriNames.FilterQueryOption);
            }

            if ((validationSettings.AllowedLogicalOperators & AllowedLogicalOperators.LessThan) != AllowedLogicalOperators.LessThan
                && rawFilterValue.Contains(" lt "))
            {
                throw ODataException.NotImplemented("Unsupported operator lt", ODataUriNames.FilterQueryOption);
            }

            if ((validationSettings.AllowedLogicalOperators & AllowedLogicalOperators.LessThanOrEqual) != AllowedLogicalOperators.LessThanOrEqual
                && rawFilterValue.Contains(" le "))
            {
                throw ODataException.NotImplemented("Unsupported operator le", ODataUriNames.FilterQueryOption);
            }

            if ((validationSettings.AllowedLogicalOperators & AllowedLogicalOperators.Has) != AllowedLogicalOperators.Has
                && rawFilterValue.Contains(" has "))
            {
                throw ODataException.NotImplemented("Unsupported operator has", ODataUriNames.FilterQueryOption);
            }

            if ((validationSettings.AllowedLogicalOperators & AllowedLogicalOperators.Not) != AllowedLogicalOperators.Not
                && rawFilterValue.Contains("not "))
            {
                throw ODataException.NotImplemented("Unsupported operator not", ODataUriNames.FilterQueryOption);
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
                throw ODataException.NotImplemented("Unsupported function ceiling", ODataUriNames.FilterQueryOption);
            }

            if ((validationSettings.AllowedFunctions & AllowedFunctions.Floor) != AllowedFunctions.Floor
                && rawFilterValue.Contains("floor("))
            {
                throw ODataException.NotImplemented("Unsupported function floor", ODataUriNames.FilterQueryOption);
            }

            if ((validationSettings.AllowedFunctions & AllowedFunctions.Round) != AllowedFunctions.Round
                && rawFilterValue.Contains("round("))
            {
                throw ODataException.NotImplemented("Unsupported function round", ODataUriNames.FilterQueryOption);
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
                throw ODataException.NotImplemented("Unsupported function contains", ODataUriNames.FilterQueryOption);
            }

            if ((validationSettings.AllowedFunctions & AllowedFunctions.Concat) != AllowedFunctions.Concat
                && rawFilterValue.Contains("concat("))
            {
                throw ODataException.NotImplemented("Unsupported function concat", ODataUriNames.FilterQueryOption);
            }

            if ((validationSettings.AllowedFunctions & AllowedFunctions.EndsWith) != AllowedFunctions.EndsWith
                && rawFilterValue.Contains("endswith("))
            {
                throw ODataException.NotImplemented("Unsupported function endswith", ODataUriNames.FilterQueryOption);
            }

            if ((validationSettings.AllowedFunctions & AllowedFunctions.IndexOf) != AllowedFunctions.IndexOf
                && rawFilterValue.Contains("indexof("))
            {
                throw ODataException.NotImplemented("Unsupported function indexof", ODataUriNames.FilterQueryOption);
            }

            if ((validationSettings.AllowedFunctions & AllowedFunctions.Length) != AllowedFunctions.Length
                && rawFilterValue.Contains("length("))
            {
                throw ODataException.NotImplemented("Unsupported function length", ODataUriNames.FilterQueryOption);
            }

            if ((validationSettings.AllowedFunctions & AllowedFunctions.StartsWith) != AllowedFunctions.StartsWith
                && rawFilterValue.Contains("startswith("))
            {
                throw ODataException.NotImplemented("Unsupported function startswith", ODataUriNames.FilterQueryOption);
            }

            if ((validationSettings.AllowedFunctions & AllowedFunctions.Substring) != AllowedFunctions.Substring
                && rawFilterValue.Contains("substring("))
            {
                throw ODataException.NotImplemented("Unsupported function substring", ODataUriNames.FilterQueryOption);
            }

            if ((validationSettings.AllowedFunctions & AllowedFunctions.ToLower) != AllowedFunctions.ToLower
                && rawFilterValue.Contains("tolower("))
            {
                throw ODataException.NotImplemented("Unsupported function tolower", ODataUriNames.FilterQueryOption);
            }

            if ((validationSettings.AllowedFunctions & AllowedFunctions.ToUpper) != AllowedFunctions.ToUpper
                && rawFilterValue.Contains("toupper("))
            {
                throw ODataException.NotImplemented("Unsupported function toupper", ODataUriNames.FilterQueryOption);
            }

            if ((validationSettings.AllowedFunctions & AllowedFunctions.Trim) != AllowedFunctions.Trim
                && rawFilterValue.Contains("trim("))
            {
                throw ODataException.NotImplemented("Unsupported function trim", ODataUriNames.FilterQueryOption);
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
                throw ODataException.NotImplemented("Unsupported function cast", ODataUriNames.FilterQueryOption);
            }

            if ((validationSettings.AllowedFunctions & AllowedFunctions.IsOf) != AllowedFunctions.IsOf
                && rawFilterValue.Contains("isof("))
            {
                throw ODataException.NotImplemented("Unsupported function isof", ODataUriNames.FilterQueryOption);
            }
        }
    }
}
