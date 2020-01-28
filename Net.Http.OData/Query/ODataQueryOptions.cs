// -----------------------------------------------------------------------
// <copyright file="ODataQueryOptions.cs" company="Project Contributors">
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
using System.Net;
using Net.Http.OData.Model;

namespace Net.Http.OData.Query
{
    /// <summary>
    /// An object which contains the query options in an OData query.
    /// </summary>
    public sealed class ODataQueryOptions
    {
        private SelectExpandQueryOption _expand;
        private FilterQueryOption _filter;
        private FormatQueryOption _format;
        private OrderByQueryOption _orderBy;
        private SelectExpandQueryOption _select;
        private SkipTokenQueryOption _skipToken;

        /// <summary>
        /// Initialises a new instance of the <see cref="ODataQueryOptions" /> class.
        /// </summary>
        /// <param name="query">The query fom the request URI.</param>
        /// <param name="entitySet">The Entity Set to apply the OData query against.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="query"/> or <paramref name="entitySet"/> are null.</exception>
        public ODataQueryOptions(string query, EntitySet entitySet)
        {
            EntitySet = entitySet ?? throw new ArgumentNullException(nameof(entitySet));
            RawValues = new ODataRawQueryOptions(query ?? throw new ArgumentNullException(nameof(query)));
        }

        /// <summary>
        /// Gets a value indicating whether the count query option has been specified.
        /// </summary>
        public bool Count => RawValues.Count?.Equals("$count=true", StringComparison.Ordinal) == true;

        /// <summary>
        /// Gets the <see cref="EntitySet"/> to apply the OData query against.
        /// </summary>
        public EntitySet EntitySet { get; }

        /// <summary>
        /// Gets the expand query option.
        /// </summary>
        public SelectExpandQueryOption Expand
        {
            get
            {
                if (_expand is null && RawValues.Expand != null)
                {
                    _expand = new SelectExpandQueryOption(RawValues.Expand, EntitySet.EdmType);
                }

                return _expand;
            }
        }

        /// <summary>
        /// Gets the filter query option.
        /// </summary>
        public FilterQueryOption Filter
        {
            get
            {
                if (_filter is null && RawValues.Filter != null)
                {
                    _filter = new FilterQueryOption(RawValues.Filter, EntitySet.EdmType);
                }

                return _filter;
            }
        }

        /// <summary>
        /// Gets the format query option.
        /// </summary>
        public FormatQueryOption Format
        {
            get
            {
                if (_format is null && RawValues.Format != null)
                {
                    _format = new FormatQueryOption(RawValues.Format);
                }

                return _format;
            }
        }

        /// <summary>
        /// Gets the order by query option.
        /// </summary>
        public OrderByQueryOption OrderBy
        {
            get
            {
                if (_orderBy is null && RawValues.OrderBy != null)
                {
                    _orderBy = new OrderByQueryOption(RawValues.OrderBy, EntitySet.EdmType);
                }

                return _orderBy;
            }
        }

        /// <summary>
        /// Gets the raw values of the OData query request.
        /// </summary>
        public ODataRawQueryOptions RawValues { get; }

        /// <summary>
        /// Gets the search query option.
        /// </summary>
        public string Search => RawValues.Search?.Substring(RawValues.Search.IndexOf('=') + 1);

        /// <summary>
        /// Gets the select query option.
        /// </summary>
        public SelectExpandQueryOption Select
        {
            get
            {
                if (_select is null && RawValues.Select != null)
                {
                    _select = new SelectExpandQueryOption(RawValues.Select, EntitySet.EdmType);
                }

                return _select;
            }
        }

        /// <summary>
        /// Gets the skip query option.
        /// </summary>
        public int? Skip => ParseInt(RawValues.Skip);

        /// <summary>
        /// Gets the skip token query option.
        /// </summary>
        public SkipTokenQueryOption SkipToken
        {
            get
            {
                if (_skipToken is null && RawValues.SkipToken != null)
                {
                    _skipToken = new SkipTokenQueryOption(RawValues.SkipToken);
                }

                return _skipToken;
            }
        }

        /// <summary>
        /// Gets the top query option.
        /// </summary>
        public int? Top => ParseInt(RawValues.Top);

        private static int? ParseInt(string rawValue)
        {
            if (rawValue is null)
            {
                return null;
            }

            int equals = rawValue.IndexOf('=') + 1;
            string value = rawValue.Substring(equals, rawValue.Length - equals);

            if (int.TryParse(value, out int integer))
            {
                return integer;
            }

            string queryOption = rawValue.Substring(0, equals - 1);

            throw new ODataException(HttpStatusCode.BadRequest, $"The value for OData query {queryOption} must be a non-negative numeric value");
        }
    }
}
