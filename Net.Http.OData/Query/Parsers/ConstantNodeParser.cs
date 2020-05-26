// -----------------------------------------------------------------------
// <copyright file="ConstantNodeParser.cs" company="Project Contributors">
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
using System.Globalization;
using Net.Http.OData.Model;
using Net.Http.OData.Query.Expressions;

namespace Net.Http.OData.Query.Parsers
{
    internal static class ConstantNodeParser
    {
        internal static ConstantNode ParseConstantNode(Token token)
        {
            switch (token.TokenType)
            {
                case TokenType.Base64Binary:
                    byte[] binaryValue = Convert.FromBase64String(token.Value);
                    return ConstantNode.Binary(token.Value, binaryValue);

                case TokenType.Date:
                    if (DateTime.TryParseExact(token.Value, ParserSettings.ODataDateFormat, ParserSettings.CultureInfo, DateTimeStyles.AssumeLocal, out DateTime dateTimeValue))
                    {
                        return ConstantNode.Date(token.Value, dateTimeValue);
                    }

                    throw ODataException.BadRequest(ExceptionMessage.UnableToParseDate, ODataUriNames.FilterQueryOption);

                case TokenType.DateTimeOffset:
                    if (DateTimeOffset.TryParse(token.Value, ParserSettings.CultureInfo, ParserSettings.DateTimeStyles, out DateTimeOffset dateTimeOffsetValue))
                    {
                        return ConstantNode.DateTimeOffset(token.Value, dateTimeOffsetValue);
                    }

                    throw ODataException.BadRequest(ExceptionMessage.UnableToParseDateTimeOffset, ODataUriNames.FilterQueryOption);

                case TokenType.Decimal:
                    string decimalText = token.Value.EndsWith("m", StringComparison.OrdinalIgnoreCase)
                        ? token.Value.Substring(0, token.Value.Length - 1)
                        : token.Value;
                    decimal decimalValue = decimal.Parse(decimalText, ParserSettings.CultureInfo);
                    return ConstantNode.Decimal(token.Value, decimalValue);

                case TokenType.Double:
                    return ParseDouble(token);

                case TokenType.Duration:
                    string durationText = token.Value.Substring(9, token.Value.Length - 10)
                        .Replace("P", string.Empty)
                        .Replace("DT", ".")
                        .Replace("H", ":")
                        .Replace("M", ":")
                        .Replace("S", string.Empty);
                    var durationTimeSpanValue = TimeSpan.Parse(durationText, ParserSettings.CultureInfo);
                    return ConstantNode.Duration(token.Value, durationTimeSpanValue);

                case TokenType.EdmType:
                    EdmType edmType = EdmType.GetEdmType(token.Value);
                    return ConstantNode.EdmTypeNode(token.Value, edmType);

                case TokenType.Enum:
                    int firstQuote = token.Value.IndexOf('\'');
                    string edmEnumTypeName = token.Value.Substring(0, firstQuote);
                    EdmEnumType edmEnumType = (EdmEnumType)EdmType.GetEdmType(edmEnumTypeName);
                    string edmEnumMemberName = token.Value.Substring(firstQuote + 1, token.Value.Length - firstQuote - 2);
                    object enumValue = edmEnumType.GetClrValue(edmEnumMemberName);

                    return new ConstantNode(edmEnumType, token.Value, enumValue);

                case TokenType.False:
                    return ConstantNode.False;

                case TokenType.Guid:
                    var guidValue = Guid.ParseExact(token.Value, "D");
                    return ConstantNode.Guid(token.Value, guidValue);

                case TokenType.Integer:
                    return ParseInteger(token);

                case TokenType.Null:
                    return ConstantNode.Null;

                case TokenType.Single:
                    string singleText = token.Value.Substring(0, token.Value.Length - 1);
                    float singleValue = float.Parse(singleText, ParserSettings.CultureInfo);
                    return ConstantNode.Single(token.Value, singleValue);

                case TokenType.String:
                    string stringText = token.Value.Trim('\'').Replace("''", "'");
                    return ConstantNode.String(token.Value, stringText);

                case TokenType.TimeOfDay:
                    var timeSpanTimeOfDayValue = TimeSpan.Parse(token.Value, ParserSettings.CultureInfo);
                    return ConstantNode.Time(token.Value, timeSpanTimeOfDayValue);

                case TokenType.True:
                    return ConstantNode.True;

                default:
                    throw new NotSupportedException(token.TokenType.ToString());
            }
        }

        private static ConstantNode ParseDouble(Token token)
        {
            switch (token.Value)
            {
                case "NaN":
                    return ConstantNode.NaN;

                case "INF":
                    return ConstantNode.PositiveInfinity;

                case "-INF":
                    return ConstantNode.NegativeInfinity;

                default:
                    string doubleText = token.Value.EndsWith("d", StringComparison.OrdinalIgnoreCase)
                        ? token.Value.Substring(0, token.Value.Length - 1)
                        : token.Value;
                    double doubleValue = double.Parse(doubleText, ParserSettings.CultureInfo);
                    return ConstantNode.Double(token.Value, doubleValue);
            }
        }

        private static ConstantNode ParseInteger(Token token)
        {
            switch (token.Value)
            {
                case "0":
                    return ConstantNode.Int32Zero;

                case "0l":
                case "0L":
                    return ConstantNode.Int64Zero;

                default:
                    string integerText = token.Value;
                    bool is64BitSuffix = integerText.EndsWith("l", StringComparison.OrdinalIgnoreCase);

                    if (!is64BitSuffix && int.TryParse(integerText, out int int32Value))
                    {
                        return ConstantNode.Int32(token.Value, int32Value);
                    }

                    string int64Text = !is64BitSuffix ? integerText : integerText.Substring(0, integerText.Length - 1);
                    long int64Value = long.Parse(int64Text, ParserSettings.CultureInfo);
                    return ConstantNode.Int64(token.Value, int64Value);
            }
        }
    }
}
