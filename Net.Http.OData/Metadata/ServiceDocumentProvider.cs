// -----------------------------------------------------------------------
// <copyright file="ServiceDocumentProvider.cs" company="Project Contributors">
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
using System.Collections.Generic;
using System.Linq;
using Net.Http.OData.Model;

namespace Net.Http.OData.Metadata
{
    /// <summary>
    /// Provides the service document for the Entity Data Model.
    /// </summary>
    public static class ServiceDocumentProvider
    {
        /// <summary>
        /// Creates the <see cref="ServiceDocumentItem"/>s that represent the Entity Data Model.
        /// </summary>
        /// <param name="entityDataModel">The Entity Data Model.</param>
        /// <param name="requestOptions">The OData request options.</param>
        /// <returns>The <see cref="ServiceDocumentItem"/>s.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="entityDataModel"/> or <paramref name="requestOptions"/> are null.</exception>
        public static IEnumerable<ServiceDocumentItem> Create(EntityDataModel entityDataModel, ODataRequestOptions requestOptions)
        {
            if (entityDataModel is null)
            {
                throw new ArgumentNullException(nameof(entityDataModel));
            }

            if (requestOptions is null)
            {
                throw new ArgumentNullException(nameof(requestOptions));
            }

            IEnumerable<ServiceDocumentItem> serviceDocumentItems = entityDataModel.EntitySets.Select(
                kvp =>
                {
                    var setUri = new Uri(kvp.Key, UriKind.Relative);
                    setUri = requestOptions.MetadataLevel == ODataMetadataLevel.None ? new Uri(requestOptions.ServiceRootUri, setUri) : setUri;

                    return ServiceDocumentItem.EntitySet(kvp.Key, setUri);
                });

            return serviceDocumentItems;
        }
    }
}
