﻿// -----------------------------------------------------------------------
// <copyright file="FormatQueryOption.cs" company="Project Contributors">
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
using System.Net.Http.Headers;

namespace Net.Http.OData.Query
{
    /// <summary>
    /// A class containing deserialised values from the $format query option.
    /// </summary>
    [System.Diagnostics.DebuggerDisplay("{RawValue}")]
    public sealed class FormatQueryOption : QueryOption
    {
        private static readonly MediaTypeHeaderValue s_atomXml = new MediaTypeHeaderValue("application/atom+xml");
        private static readonly MediaTypeHeaderValue s_json = new MediaTypeHeaderValue("application/json");
        private static readonly MediaTypeHeaderValue s_xml = new MediaTypeHeaderValue("application/xml");

        /// <summary>
        /// Initialises a new instance of the <see cref="FormatQueryOption"/> class.
        /// </summary>
        /// <param name="rawValue">The raw request value.</param>
        internal FormatQueryOption(string rawValue)
            : base(rawValue)
        {
            switch (rawValue)
            {
                case "$format=atom":
                    MediaTypeHeaderValue = s_atomXml;
                    break;

                case "$format=json":
                    MediaTypeHeaderValue = s_json;
                    break;

                case "$format=xml":
                    MediaTypeHeaderValue = s_xml;
                    break;

                default:
                    string value = rawValue.SubstringAfter('=');

                    MediaTypeHeaderValue = new MediaTypeHeaderValue(value);
                    break;
            }
        }

        /// <summary>
        /// Gets the media type header value.
        /// </summary>
        public MediaTypeHeaderValue MediaTypeHeaderValue { get; }
    }
}
