// -----------------------------------------------------------------------
// <copyright file="ExceptionMessage.cs" company="Project Contributors">
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
using System.Globalization;

namespace Net.Http.OData
{
    internal static class ExceptionMessage
    {
        internal static string GenericUnableToParseFilter { get; } = "Unable to parse the $filter OData query option.";

        internal static string EdmTypeDoesNotContainProperty(string edmType, string propertyName)
            => $"The type '{edmType}' does not contain a property named '{propertyName}'.";

        internal static string EntityDataModelDoesNotContainEntitySet(string entitySetName)
            => $"This service does not contain a collection named '{entitySetName}'.";

        internal static string InvalidOperator(string operatorName)
            => $"The operator '{operatorName}' is not a valid OData operator.";

        internal static string InvalidOrderByDirection(string orderByDirection, string propertyName)
            => $"The supplied order value '{orderByDirection}' for {propertyName} is invalid, valid options are 'asc' and 'desc'";

        internal static string PropertyNotNavigable(string propertyName, string propertyPath)
            => $"The property '{propertyName}' in the path '{propertyPath}' is not a navigable property.";

        internal static string QueryNodeKindNotSupported(string queryNodeKind)
            => $"Query nodes of type '{queryNodeKind}' are not supported by this service.";

        internal static string QueryOptionValueCannotBeEmpty(string queryOption)
            => $"If the OData query option {queryOption} is specified, it's value cannot be empty.";

        internal static string QueryOptionValueMustBePositiveInteger(string queryOption)
            => $"The value specified for OData query option {queryOption} must be a non-negative numeric value.";

        internal static string QueryOptionValueNotSupported(string queryOption, string value, string supportedValue)
            => $"The value '{value}' specified for OData query option {queryOption} is not supported by this service, acceptable values are {supportedValue}.";

        internal static string UnableToParseFilter(string reason, int position = 0) // Add 1 as the position is zero indexed but the exception shouldn't use that as the caller may not get that.
            => $"Unable to parse the $filter OData query option, {reason}{(position > 0 ? " at position " + (position + 1).ToString(CultureInfo.InvariantCulture) : string.Empty)}.";

        internal static string UnsupportedQueryOption(string queryOption)
            => $"The OData query option '{queryOption}' is not supported by this service.";
    }
}
