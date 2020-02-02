// -----------------------------------------------------------------------
// <copyright file="EntityDataModelBuilder.cs" company="Project Contributors">
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
using System.Linq.Expressions;
using System.Reflection;

namespace Net.Http.OData.Model
{
    /// <summary>
    /// The class used to build the <see cref="EntityDataModel"/> using a fluent API, it should be used once at application startup.
    /// </summary>
    public sealed class EntityDataModelBuilder
    {
        private readonly EntityDataModel _entityDataModel;
        private readonly Dictionary<string, EntitySet> _entitySets;

        /// <summary>
        /// Initialises a new instance of the <see cref="EntityDataModelBuilder"/> class.
        /// </summary>
        /// <param name="entitySetNameComparer">The equality comparer to use for the Entity Set name.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="entitySetNameComparer"/> is null.</exception>
        public EntityDataModelBuilder(IEqualityComparer<string> entitySetNameComparer)
        {
            _entitySets = new Dictionary<string, EntitySet>(entitySetNameComparer) ?? throw new ArgumentNullException(nameof(entitySetNameComparer));
            _entityDataModel = new EntityDataModel(_entitySets);
        }

        /// <summary>
        /// Builds the Entity Data Model containing the collections registered.
        /// </summary>
        /// <returns>The Entity Data Model.</returns>
        public EntityDataModel BuildModel() => EntityDataModel.Current = _entityDataModel;

        /// <summary>
        /// Registers an Entity Set of the specified type to the Entity Data Model with the specified name which can only be queried.
        /// </summary>
        /// <typeparam name="T">The type exposed by the collection.</typeparam>
        /// <param name="entitySetName">Name of the Entity Set.</param>
        /// <param name="entityKeyExpression">The Entity Key expression.</param>
        /// <returns>This entity data model builder.</returns>
        public EntityDataModelBuilder RegisterEntitySet<T>(string entitySetName, Expression<Func<T, object>> entityKeyExpression)
            => RegisterEntitySet(entitySetName, entityKeyExpression, Capabilities.None);

        /// <summary>
        /// Registers an Entity Set of the specified type to the Entity Data Model with the specified name and <see cref="Capabilities"/>.
        /// </summary>
        /// <typeparam name="T">The type exposed by the collection.</typeparam>
        /// <param name="entitySetName">Name of the Entity Set.</param>
        /// <param name="entityKeyExpression">The Entity Key expression.</param>
        /// <param name="capabilities">The capabilities of the Entity Set.</param>
        /// <returns>This entity data model builder.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="entityKeyExpression"/> is null.</exception>
        public EntityDataModelBuilder RegisterEntitySet<T>(string entitySetName, Expression<Func<T, object>> entityKeyExpression, Capabilities capabilities)
        {
            if (entityKeyExpression is null)
            {
                throw new ArgumentNullException(nameof(entityKeyExpression));
            }

            var edmType = (EdmComplexType)EdmTypeCache.Map.GetOrAdd(typeof(T), EdmTypeResolver);

            EdmProperty entityKey = edmType.BaseType is null ? edmType.GetProperty(entityKeyExpression.GetMemberInfo().Name) : null;

            var entitySet = new EntitySet(entitySetName, edmType, entityKey, capabilities);

            _entitySets.Add(entitySet.Name, entitySet);

            return this;
        }

        private EdmType EdmTypeResolver(Type clrType)
        {
            if (clrType.IsEnum)
            {
                Array enumValues = Enum.GetValues(clrType);

                var members = new List<EdmEnumMember>(enumValues.Length);

                foreach (object value in enumValues)
                {
                    members.Add(new EdmEnumMember(value.ToString(), (int)value));
                }

                return EdmTypeCache.Map.GetOrAdd(clrType, t => new EdmEnumType(t, members.AsReadOnly()));
            }

            if (clrType.IsGenericType)
            {
                Type innerType = clrType.GetGenericArguments()[0];

                if (typeof(IEnumerable<>).MakeGenericType(innerType).IsAssignableFrom(clrType))
                {
                    EdmType containedType = EdmTypeCache.Map.GetOrAdd(innerType, EdmTypeResolver);

                    return EdmTypeCache.Map.GetOrAdd(clrType, t => new EdmCollectionType(t, containedType));
                }
                else
                {
                    throw new NotSupportedException(clrType.FullName);
                }
            }

            EdmType baseEdmType = clrType.BaseType != typeof(object) ? EdmTypeCache.Map.GetOrAdd(clrType.BaseType, EdmTypeResolver) : null;

            PropertyInfo[] clrTypeProperties = clrType.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);

            var edmProperties = new List<EdmProperty>(clrTypeProperties.Length);
            var edmComplexType = (EdmComplexType)EdmTypeCache.Map.GetOrAdd(clrType, t => new EdmComplexType(t, baseEdmType, edmProperties.AsReadOnly()));

            edmProperties.AddRange(clrTypeProperties
                .OrderBy(p => p.Name)
                .Select(p => new EdmProperty(p, EdmTypeCache.Map.GetOrAdd(p.PropertyType, EdmTypeResolver), edmComplexType, _entityDataModel.IsEntitySet)));

            return edmComplexType;
        }
    }
}
