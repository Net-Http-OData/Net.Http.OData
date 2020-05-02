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
using Net.Http.OData.Model;

namespace Net.Http.OData.Query
{
    /// <summary>
    /// A class which contains the query options in an OData query.
    /// </summary>
    public sealed class ODataQueryOptions
    {
        private readonly IODataQueryOptionsValidator _validator;
        private SelectExpandQueryOption _expand;
        private FilterQueryOption _filter;
        private FormatQueryOption _format;
        private OrderByQueryOption _orderBy;
        private SearchQueryOption _search;
        private SelectExpandQueryOption _select;
        private SkipTokenQueryOption _skipToken;

        /// <summary>
        /// Initialises a new instance of the <see cref="ODataQueryOptions" /> class.
        /// </summary>
        /// <param name="query">The query fom the request URI.</param>
        /// <param name="entitySet">The Entity Set to apply the OData query against.</param>
        /// <param name="validator">The query options validator to use.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="query"/>, <paramref name="entitySet"/> or <paramref name="validator"/> are null.</exception>
        public ODataQueryOptions(string query, EntitySet entitySet, IODataQueryOptionsValidator validator)
        {
            EntitySet = entitySet ?? throw new ArgumentNullException(nameof(entitySet));
            RawValues = new ODataRawQueryOptions(query ?? throw new ArgumentNullException(nameof(query)));
            _validator = validator ?? throw new ArgumentNullException(nameof(validator));
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
        /// Gets <see cref="SelectExpandQueryOption"/> which represents the $expand query option.
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
        /// Gets <see cref="FilterQueryOption"/> which represents the $filter query option.
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
        /// Gets <see cref="FormatQueryOption"/> which represents the $format query option.
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
        /// Gets <see cref="OrderByQueryOption"/> which represents the $orderby query option.
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
        /// Gets <see cref="SearchQueryOption"/> which represents the $search query option.
        /// </summary>
        public SearchQueryOption Search
        {
            get
            {
                if (_search is null && RawValues.Search != null)
                {
                    _search = new SearchQueryOption(RawValues.Search);
                }

                return _search;
            }
        }

        /// <summary>
        /// Gets <see cref="SelectExpandQueryOption"/> which represents the $select query option.
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
        /// Gets the integer value specified in the $skip query option.
        /// </summary>
        public int? Skip => ParseInt(RawValues.Skip);

        /// <summary>
        /// Gets <see cref="SkipTokenQueryOption"/> which represents the $skiptoken query option.
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
        /// Gets the integer value specified in the $top query option.
        /// </summary>
        public int? Top => ParseInt(RawValues.Top);

        /// <summary>
        /// Validates this instance using the specified validation settings.
        /// </summary>
        /// <param name="validationSettings">The validation settings to configure the validation.</param>
        public void Validate(ODataValidationSettings validationSettings) => _validator.Validate(this, validationSettings);

        private static int? ParseInt(string rawValue)
        {
            if (rawValue is null)
            {
                return null;
            }

            string value = rawValue.SubstringAfter('=');

            if (int.TryParse(value, out int integer))
            {
                return integer;
            }

            string queryOption = rawValue.SubstringBefore('=');

            throw ODataException.BadRequest(ExceptionMessage.QueryOptionValueMustBePositiveInteger(queryOption), queryOption);
        }
    }
}
