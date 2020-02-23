// -----------------------------------------------------------------------
// <copyright file="EntityDataModel.cs" company="Project Contributors">
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
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace Net.Http.OData.Model
{
    /// <summary>
    /// A class which represents the Entity Data Model.
    /// </summary>
    public sealed class EntityDataModel
    {
        internal EntityDataModel(IReadOnlyDictionary<string, EntitySet> entitySets)
            => EntitySets = entitySets ?? throw new System.ArgumentNullException(nameof(entitySets));

        /// <summary>
        /// Gets the current Entity Data Model.
        /// </summary>
        /// <remarks>
        /// Will be null until <see cref="EntityDataModelBuilder" />.BuildModel() has been called.
        /// </remarks>
        public static EntityDataModel Current { get; internal set; }

        /// <summary>
        /// Gets the Entity Sets defined in the Entity Data Model.
        /// </summary>
        public IReadOnlyDictionary<string, EntitySet> EntitySets { get; }

        /// <summary>
        /// Gets the <see cref="EntitySet"/> for the path segment of a URI.
        /// </summary>
        /// <param name="path">The path segment of a URI.</param>
        /// <returns>The <see cref="EntitySet"/>.</returns>
        public EntitySet EntitySetForPath(string path)
        {
            string entitySetName = ODataUtility.ResolveEntitySetName(path);

            if (EntitySets.TryGetValue(entitySetName, out EntitySet entitySet))
            {
                return entitySet;
            }

            throw new ODataException($"This service does not contain a collection named '{entitySetName}'", HttpStatusCode.BadRequest);
        }

        /// <summary>
        /// Gets a value indicating whether the specified <see cref="EdmType"/> is an <see cref="EntitySet"/>.
        /// </summary>
        /// <param name="edmType">The <see cref="EdmType"/> to check.</param>
        /// <returns>True if the <see cref="EdmType"/> is an <see cref="EntitySet"/> in the Entity Data Model; otherwise false.</returns>
        internal bool IsEntitySet(EdmType edmType) => EntitySets.Values.Any(x => x.EdmType == edmType);
    }
}
