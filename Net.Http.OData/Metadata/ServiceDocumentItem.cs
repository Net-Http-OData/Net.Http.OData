﻿// -----------------------------------------------------------------------
// <copyright file="ServiceDocumentItem.cs" company="Project Contributors">
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

namespace Net.Http.OData.Metadata
{
    /// <summary>
    /// Represents an item in the service document.
    /// </summary>
    public sealed class ServiceDocumentItem
    {
        private ServiceDocumentItem(string kind, string name, Uri url)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("The name must be specified", nameof(name));
            }

            Name = name;
            Kind = kind;
            Url = url ?? throw new ArgumentNullException(nameof(url));
        }

        /// <summary>
        /// Gets the name of the item.
        /// </summary>
        public string Kind { get; }

        /// <summary>
        /// Gets the name of the item.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets the URL of the item.
        /// </summary>
        public Uri Url { get; }

        /// <summary>
        /// Creates a service document item which represents an Entity Set in the Entity Data Model.
        /// </summary>
        /// <param name="name">The name of the item.</param>
        /// <param name="url">The URL of the item.</param>
        /// <returns>A service document item which represents an Entity Set in the Entity Data Model.</returns>
        public static ServiceDocumentItem EntitySet(string name, Uri url) => new ServiceDocumentItem("EntitySet", name, url);
    }
}
