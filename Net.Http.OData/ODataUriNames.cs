// -----------------------------------------------------------------------
// <copyright file="ODataUriNames.cs" company="Project Contributors">
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
    /// The names for OData URI segments.
    /// </summary>
    internal static class ODataUriNames
    {
        internal const string CountQueryOption = "$count";
        internal const string Entity = "$entity";
        internal const string ExpandQueryOption = "$expand";
        internal const string FilterQueryOption = "$filter";
        internal const string FormatQueryOption = "$format";
        internal const string Metadata = "$metadata";
        internal const string OrderByQueryOption = "$orderby";
        internal const string SearchQueryOption = "$search";
        internal const string SelectQueryOption = "$select";
        internal const string SkipQueryOption = "$skip";
        internal const string SkipTokenQueryOption = "$skiptoken";
        internal const string TopQueryOption = "$top";
    }
}
