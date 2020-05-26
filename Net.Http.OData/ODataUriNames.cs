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
    public static class ODataUriNames
    {
        /// <summary>
        /// The string representing the $count query option.
        /// </summary>
        public const string CountQueryOption = "$count";

        /// <summary>
        /// The string representing the $entity segment in the URI.
        /// </summary>
        public const string Entity = "$entity";

        /// <summary>
        /// The string representing the $expand query option.
        /// </summary>
        public const string ExpandQueryOption = "$expand";

        /// <summary>
        /// The string representing the $filter query option.
        /// </summary>
        public const string FilterQueryOption = "$filter";

        /// <summary>
        /// The string representing the $format query option.
        /// </summary>
        public const string FormatQueryOption = "$format";

        /// <summary>
        /// The string representing the $metadata segment in the URI.
        /// </summary>
        public const string Metadata = "$metadata";

        /// <summary>
        /// The string representing the $orderby query option.
        /// </summary>
        public const string OrderByQueryOption = "$orderby";

        /// <summary>
        /// The string representing the $search query option.
        /// </summary>
        public const string SearchQueryOption = "$search";

        /// <summary>
        /// The string representing the $select query option.
        /// </summary>
        public const string SelectQueryOption = "$select";

        /// <summary>
        /// The string representing the $skip query option.
        /// </summary>
        public const string SkipQueryOption = "$skip";

        /// <summary>
        /// The string representing the $skiptoken query option.
        /// </summary>
        public const string SkipTokenQueryOption = "$skiptoken";

        /// <summary>
        /// The string representing the $top query option.
        /// </summary>
        public const string TopQueryOption = "$top";
    }
}
