// -----------------------------------------------------------------------
// <copyright file="ODataFunctionNames.cs" company="Project Contributors">
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
namespace Net.Http.OData
{
    /// <summary>
    /// The names of OData functions.
    /// </summary>
    internal static class ODataFunctionNames
    {
        // String functions
        internal const string Concat = "concat";
        internal const string Contains = "contains";
        internal const string Endswith = "endswith";
        internal const string IndexOf = "indexof";
        internal const string Length = "length";
        internal const string Startswith = "startswith";
        internal const string Substring = "substring";
        internal const string ToLower = "tolower";
        internal const string ToUpper = "toupper";
        internal const string Trim = "trim";

        // Date functions
        internal const string Date = "date";
        internal const string Day = "day";
        internal const string FractionalSeconds = "fractionalseconds";
        internal const string Hour = "hour";
        internal const string MaxDateTime = "maxdatetime";
        internal const string MinDateTime = "mindatetime";
        internal const string Minute = "minute";
        internal const string Month = "month";
        internal const string Now = "now";
        internal const string Second = "second";
        internal const string TotalOffsetMinutes = "totaloffsetminutes";
        internal const string Year = "year";

        // Math functions
        internal const string Ceiling = "ceiling";
        internal const string Floor = "floor";
        internal const string Round = "round";

        // Type functions
        internal const string Cast = "cast";
        internal const string IsOf = "isof";
    }
}
