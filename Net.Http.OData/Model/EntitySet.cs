// -----------------------------------------------------------------------
// <copyright file="EntitySet.cs" company="Project Contributors">
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

namespace Net.Http.OData.Model
{
    /// <summary>
    /// A class which represents an Entity Set in the Entity Data Model.
    /// </summary>
    [System.Diagnostics.DebuggerDisplay("{Name} - {EdmType.FullName}")]
    public sealed class EntitySet
    {
        internal EntitySet(string name, EdmComplexType edmType, EdmProperty entityKey, Capabilities capabilities)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Entity Set name must be specified", nameof(name));
            }

            Name = name;
            EdmType = edmType ?? throw new ArgumentNullException(nameof(edmType));
            EntityKey = entityKey;
            Capabilities = capabilities;
        }

        /// <summary>
        /// Gets the <see cref="Capabilities"/> of the Entity Set.
        /// </summary>
        public Capabilities Capabilities { get; }

        /// <summary>
        /// Gets the <see cref="EdmComplexType"/> of the entities in the set.
        /// </summary>
        public EdmComplexType EdmType { get; }

        /// <summary>
        /// Gets the <see cref="EdmProperty"/> which is the entity key.
        /// </summary>
        public EdmProperty EntityKey { get; }

        /// <summary>
        /// Gets the name of the Entity Set.
        /// </summary>
        public string Name { get; }
    }
}
