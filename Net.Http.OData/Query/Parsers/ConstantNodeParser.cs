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
using System.Reflection;
using Net.Http.OData.Model;
using Net.Http.OData.Query.Expressions;

namespace Net.Http.OData.Query.Parsers
{
    internal static class ConstantNodeParser
    {
        private const string ODataDateFormat = "yyyy-MM-dd";

        internal static ConstantNode ParseConstantNode(Token token)
        {
            switch (token.TokenType)
            {
                case TokenType.Base64Binary:
                    byte[] binary = Convert.FromBase64String(token.Value);
                    return ConstantNode.Binary(token.Value, binary);

                case TokenType.Date:
                    var dateTimeValue = DateTime.ParseExact(token.Value, ODataDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.AssumeLocal);
                    return ConstantNode.Date(token.Value, dateTimeValue);

                case TokenType.DateTimeOffset:
                    var dateTimeOffsetValue = DateTimeOffset.Parse(token.Value, CultureInfo.InvariantCulture, DateTimeStyles.AssumeLocal);
                    return ConstantNode.DateTimeOffset(token.Value, dateTimeOffsetValue);

                case TokenType.Decimal:
                    string decimalText = token.Value.Substring(0, token.Value.Length - 1);
                    decimal decimalValue = decimal.Parse(decimalText, CultureInfo.InvariantCulture);
                    return ConstantNode.Decimal(token.Value, decimalValue);

                case TokenType.Double:
                    string doubleText = token.Value.EndsWith("d", StringComparison.OrdinalIgnoreCase)
                        ? token.Value.Substring(0, token.Value.Length - 1)
                        : token.Value;
                    double doubleValue = double.Parse(doubleText, CultureInfo.InvariantCulture);
                    return ConstantNode.Double(token.Value, doubleValue);

                case TokenType.Duration:
                    string durationText = token.Value.Substring(9, token.Value.Length - 10)
                        .Replace("P", string.Empty)
                        .Replace("DT", ".")
                        .Replace("H", ":")
                        .Replace("M", ":")
                        .Replace("S", string.Empty);
                    var durationTimeSpanValue = TimeSpan.Parse(durationText, CultureInfo.InvariantCulture);
                    return ConstantNode.Duration(token.Value, durationTimeSpanValue);

                case TokenType.EdmType:
                    string edmTypeName = token.Value;
                    EdmType edmType = EdmType.GetEdmType(edmTypeName);
                    return ConstantNode.EdmTypeNode(token.Value, edmType);

                case TokenType.Enum:
                    int firstQuote = token.Value.IndexOf('\'');
                    string edmEnumTypeName = token.Value.Substring(0, firstQuote);
                    EdmEnumType edmEnumType = (EdmEnumType)EdmType.GetEdmType(edmEnumTypeName);
                    string edmEnumMemberName = token.Value.Substring(firstQuote + 1, token.Value.Length - firstQuote - 2);
                    object enumValue = edmEnumType.GetClrValue(edmEnumMemberName);
                    Type constantNodeType = typeof(ConstantNode<>).MakeGenericType(edmEnumType.ClrType);

                    return (ConstantNode)Activator.CreateInstance(constantNodeType, BindingFlags.Instance | BindingFlags.NonPublic, null, new[] { edmEnumType, token.Value, enumValue }, null);

                case TokenType.False:
                    return ConstantNode.False;

                case TokenType.Guid:
                    var guidValue = Guid.ParseExact(token.Value, "D");
                    return ConstantNode.Guid(token.Value, guidValue);

                case TokenType.Integer:
                    string integerText = token.Value;

                    if (integerText == "0")
                    {
                        return ConstantNode.Int32Zero;
                    }

                    if (integerText == "0l" || integerText == "0L")
                    {
                        return ConstantNode.Int64Zero;
                    }

                    bool is64BitSuffix = integerText.EndsWith("l", StringComparison.OrdinalIgnoreCase);

                    if (!is64BitSuffix && int.TryParse(integerText, out int int32Value))
                    {
                        return ConstantNode.Int32(token.Value, int32Value);
                    }

                    string int64Text = !is64BitSuffix ? integerText : integerText.Substring(0, integerText.Length - 1);
                    long int64Value = long.Parse(int64Text, CultureInfo.InvariantCulture);
                    return ConstantNode.Int64(token.Value, int64Value);

                case TokenType.Null:
                    return ConstantNode.Null;

                case TokenType.Single:
                    string singleText = token.Value.Substring(0, token.Value.Length - 1);
                    float singleValue = float.Parse(singleText, CultureInfo.InvariantCulture);
                    return ConstantNode.Single(token.Value, singleValue);

                case TokenType.String:
                    string stringText = token.Value.Trim('\'').Replace("''", "'");
                    return ConstantNode.String(token.Value, stringText);

                case TokenType.TimeOfDay:
                    var timeSpanTimeOfDayValue = TimeSpan.Parse(token.Value, CultureInfo.InvariantCulture);
                    return ConstantNode.Time(token.Value, timeSpanTimeOfDayValue);

                case TokenType.True:
                    return ConstantNode.True;

                default:
                    throw new NotSupportedException(token.TokenType.ToString());
            }
        }
    }
}
