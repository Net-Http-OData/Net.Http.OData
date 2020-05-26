// -----------------------------------------------------------------------
// <copyright file="ODataRawQueryOptions.cs" company="Project Contributors">
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

namespace Net.Http.OData.Query
{
    /// <summary>
    /// A class which contains the raw request values.
    /// </summary>
    public sealed class ODataRawQueryOptions
    {
        private readonly string _rawQuery;

        /// <summary>
        /// Initialises a new instance of the <see cref="ODataRawQueryOptions"/> class.
        /// </summary>
        /// <param name="rawQuery">The raw query.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="rawQuery"/> is null.</exception>
        /// <exception cref="ODataException">Thrown if an error occurs parsing the <paramref name="rawQuery"/>.</exception>
        internal ODataRawQueryOptions(string rawQuery)
        {
            if (rawQuery is null)
            {
                throw new ArgumentNullException(nameof(rawQuery));
            }

            // Any + signs we want in the data should have been encoded as %2B,
            // so do the replace first otherwise we replace legitemate + signs!
            _rawQuery = rawQuery.Replace('+', ' ');

            if (_rawQuery.Length > 0)
            {
                foreach (string queryOption in _rawQuery.Slice('&', _rawQuery.IndexOf('?') + 1))
                {
                    // Decode the chunks to prevent splitting the query on an '&' which is actually part of a string value
                    string rawQueryOption = Uri.UnescapeDataString(queryOption);

                    if (rawQueryOption.StartsWith(ODataUriNames.SelectQueryOption, StringComparison.Ordinal))
                    {
                        Select = rawQueryOption;
                    }
                    else if (rawQueryOption.StartsWith(ODataUriNames.FilterQueryOption, StringComparison.Ordinal))
                    {
                        Filter = rawQueryOption;
                    }
                    else if (rawQueryOption.StartsWith(ODataUriNames.OrderByQueryOption, StringComparison.Ordinal))
                    {
                        OrderBy = rawQueryOption;
                    }
                    else if (rawQueryOption.StartsWith(ODataUriNames.SkipTokenQueryOption, StringComparison.Ordinal))
                    {
                        SkipToken = rawQueryOption;
                    }
                    else if (rawQueryOption.StartsWith(ODataUriNames.SkipQueryOption, StringComparison.Ordinal))
                    {
                        Skip = rawQueryOption;
                    }
                    else if (rawQueryOption.StartsWith(ODataUriNames.TopQueryOption, StringComparison.Ordinal))
                    {
                        Top = rawQueryOption;
                    }
                    else if (rawQueryOption.StartsWith(ODataUriNames.CountQueryOption, StringComparison.Ordinal))
                    {
                        Count = rawQueryOption;
                    }
                    else if (rawQueryOption.StartsWith(ODataUriNames.FormatQueryOption, StringComparison.Ordinal))
                    {
                        Format = rawQueryOption;
                    }
                    else if (rawQueryOption.StartsWith(ODataUriNames.ExpandQueryOption, StringComparison.Ordinal))
                    {
                        Expand = rawQueryOption;
                    }
                    else if (rawQueryOption.StartsWith(ODataUriNames.SearchQueryOption, StringComparison.Ordinal))
                    {
                        Search = rawQueryOption;
                    }
                    else if (rawQueryOption.StartsWith("$", StringComparison.Ordinal))
                    {
                        string optionName = rawQueryOption.SubstringBefore('=');

                        throw ODataException.BadRequest(ExceptionMessage.UnsupportedQueryOption(optionName), optionName);
                    }
                }
            }
        }

        /// <summary>
        /// Gets the raw $count query value from the incoming request Uri if specified.
        /// </summary>
        public string Count { get; }

        /// <summary>
        /// Gets the raw $expand query value from the incoming request Uri if specified.
        /// </summary>
        public string Expand { get; }

        /// <summary>
        /// Gets the raw $filter query value from the incoming request Uri if specified.
        /// </summary>
        public string Filter { get; }

        /// <summary>
        /// Gets the raw $format query value from the incoming request Uri if specified.
        /// </summary>
        public string Format { get; }

        /// <summary>
        /// Gets the raw $orderby query value from the incoming request Uri if specified.
        /// </summary>
        public string OrderBy { get; }

        /// <summary>
        /// Gets the raw $search query value from the incoming request Uri if specified.
        /// </summary>
        public string Search { get; }

        /// <summary>
        /// Gets the raw $select query value from the incoming request Uri if specified.
        /// </summary>
        public string Select { get; }

        /// <summary>
        /// Gets the raw $skip query value from the incoming request Uri if specified.
        /// </summary>
        public string Skip { get; }

        /// <summary>
        /// Gets the raw $skip token query value from the incoming request Uri if specified.
        /// </summary>
        public string SkipToken { get; }

        /// <summary>
        /// Gets the raw $top query value from the incoming request Uri if specified.
        /// </summary>
        public string Top { get; }

        /// <inheritdoc/>
        public override string ToString() => _rawQuery;
    }
}
