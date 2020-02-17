// -----------------------------------------------------------------------
// <copyright file="MetadataProvider.cs" company="Project Contributors">
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
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;
using Net.Http.OData.Model;

namespace Net.Http.OData.Metadata
{
    /// <summary>
    /// Provides the Metadata XML document for the Entity Data Model.
    /// </summary>
    public static class MetadataProvider
    {
        private static readonly XNamespace s_edmNs = "http://docs.oasis-open.org/odata/ns/edm";
        private static readonly XNamespace s_edmxNs = "http://docs.oasis-open.org/odata/ns/edmx";

        /// <summary>
        /// Creates an <see cref="XDocument"/> containing the Metadata XML document for the Entity Data Model.
        /// </summary>
        /// <param name="entityDataModel">The Entity Data Model to include the Metadata for.</param>
        /// <param name="serviceOptions">The <see cref="ODataServiceOptions"/> for the service.</param>
        /// <returns>An <see cref="XDocument"/> containing the Metadata XML document for the Entity Data Model.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="entityDataModel"/> or <paramref name="serviceOptions"/> is null.</exception>
        public static XDocument Create(EntityDataModel entityDataModel, ODataServiceOptions serviceOptions)
        {
            if (entityDataModel is null)
            {
                throw new ArgumentNullException(nameof(entityDataModel));
            }

            if (serviceOptions is null)
            {
                throw new ArgumentNullException(nameof(serviceOptions));
            }

            var document = new XDocument(
                new XDeclaration("1.0", "utf-8", null),
                new XElement(
                    s_edmxNs + "Edmx",
                    new XAttribute(XNamespace.Xmlns + "edmx", s_edmxNs),
                    new XAttribute("Version", serviceOptions.MaxVersion),
                    new XElement(
                        s_edmxNs + "DataServices",
                        new XElement(
                            s_edmNs + "Schema",
                            new XAttribute("xmlns", s_edmNs),
                            new XAttribute("Namespace", entityDataModel.EntitySets.First().Value.EdmType.ClrType.Namespace),
                            GetEnumTypes(entityDataModel),
                            GetComplexTypes(entityDataModel),
                            GetEntityTypes(entityDataModel),
                            GetFunctions(),
                            GetActions(),
                            GetEntityContainer(entityDataModel),
                            GetAnnotations(entityDataModel, serviceOptions)))));

            return document;
        }

        private static IEnumerable<XElement> GetActions() => Enumerable.Empty<XElement>();

        private static XElement GetAnnotations(EntityDataModel entityDataModel, ODataServiceOptions serviceOptions)
        {
            var annotations = new XElement(
                s_edmNs + "Annotations",
                new XAttribute("Target", entityDataModel.EntitySets.First().Value.EdmType.ClrType.Namespace + ".DefaultContainer"),
                new XElement(
                    s_edmNs + "Annotation",
                    new XAttribute("Term", "Org.OData.Capabilities.V1.DereferenceableIDs"),
                    new XAttribute("Bool", "true")),
                new XElement(
                    s_edmNs + "Annotation",
                    new XAttribute("Term", "Org.OData.Capabilities.V1.ConventionalIDs"),
                    new XAttribute("Bool", "true")),
                new XElement(
                    s_edmNs + "Annotation",
                    new XAttribute("Term", "Org.OData.Capabilities.V1.ConformanceLevel"),
                    new XElement(s_edmNs + "EnumMember", "Org.OData.Capabilities.V1.ConformanceLevelType/Minimal")),
                new XElement(
                    s_edmNs + "Annotation",
                    new XAttribute("Term", "Org.OData.Capabilities.V1.SupportedFormats"),
                    new XElement(
                        s_edmNs + "Collection",
#pragma warning disable CA1308 // Normalize strings to uppercase
                        serviceOptions.SupportedMetadataLevels.Select(metadataLevel => new XElement(s_edmNs + "String", $"application/json;odata.metadata={metadataLevel.ToString().ToLowerInvariant()}")))),
#pragma warning restore CA1308 // Normalize strings to uppercase
                new XElement(
                    s_edmNs + "Annotation",
                    new XAttribute("Term", "Org.OData.Capabilities.V1.AsynchronousRequestsSupported"),
                    new XAttribute("Bool", "false")),
                new XElement(
                    s_edmNs + "Annotation",
                    new XAttribute("Term", "Org.OData.Capabilities.V1.BatchContinueOnErrorSupported"),
                    new XAttribute("Bool", "false")),
                new XElement(
                    s_edmNs + "Annotation",
                    new XAttribute("Term", "Org.OData.Capabilities.V1.FilterFunctions"),
                    new XElement(
                        s_edmNs + "Collection",
                        serviceOptions.SupportedFilterFunctions.Select(function => new XElement(s_edmNs + "String", function)))));

            return annotations;
        }

        private static IEnumerable<XElement> GetComplexTypes(EntityDataModel entityDataModel)
        {
            // Any types used in the model which aren't Entity Sets.
            IEnumerable<EdmComplexType> complexCollectionTypes = entityDataModel.EntitySets.Values
                .SelectMany(t => t.EdmType.Properties)
                .Select(p => p.PropertyType)
                .OfType<EdmCollectionType>()
                .Select(t => t.ContainedType)
                .OfType<EdmComplexType>();

            IEnumerable<XElement> complexTypes = entityDataModel.EntitySets.Values
                .SelectMany(t => t.EdmType.Properties)
                .Select(p => p.PropertyType)
                .OfType<EdmComplexType>()
                .Concat(complexCollectionTypes)
                .Where(t => !entityDataModel.IsEntitySet(t))
                .Distinct()
                .Select(t =>
                {
                    var element = new XElement(
                       s_edmNs + "ComplexType",
                       new XAttribute("Name", t.Name),
                       GetProperties(t.Properties));

                    if (t.BaseType != null)
                    {
                        element.Add(new XAttribute("BaseType", t.BaseType.FullName));
                    }

                    return element;
                });

            return complexTypes;
        }

        private static XElement GetEntityContainer(EntityDataModel entityDataModel)
        {
            var entityContainer = new XElement(
                s_edmNs + "EntityContainer",
                new XAttribute(
                    "Name", "DefaultContainer"),
                entityDataModel.EntitySets.Select(
                    kvp => new XElement(
                        s_edmNs + "EntitySet",
                        new XAttribute("Name", kvp.Key),
                        new XAttribute("EntityType", kvp.Value.EdmType.ClrType.FullName),
                        new XElement(
                            s_edmNs + "Annotation",
                            new XAttribute("Term", "Org.OData.Core.V1.ResourcePath"),
                            new XAttribute("String", kvp.Key)),
                        new XElement(
                            s_edmNs + "Annotation",
                            new XAttribute("Term", "Org.OData.Capabilities.V1.InsertRestrictions"),
                            new XElement(
                                s_edmNs + "Record",
                                new XElement(
                                    s_edmNs + "PropertyValue",
                                    new XAttribute("Property", "Insertable"),
                                    new XAttribute("Bool", ((kvp.Value.Capabilities & Capabilities.Insertable) == Capabilities.Insertable).ToString(CultureInfo.InvariantCulture))))),
                        new XElement(
                            s_edmNs + "Annotation",
                            new XAttribute("Term", "Org.OData.Capabilities.V1.UpdateRestrictions"),
                            new XElement(
                                s_edmNs + "Record",
                                new XElement(
                                    s_edmNs + "PropertyValue",
                                    new XAttribute("Property", "Updatable"),
                                    new XAttribute("Bool", ((kvp.Value.Capabilities & Capabilities.Updatable) == Capabilities.Updatable).ToString(CultureInfo.InvariantCulture))))),
                        new XElement(
                            s_edmNs + "Annotation",
                            new XAttribute("Term", "Org.OData.Capabilities.V1.DeleteRestrictions"),
                            new XElement(
                                s_edmNs + "Record",
                                new XElement(
                                    s_edmNs + "PropertyValue",
                                    new XAttribute("Property", "Deletable"),
                                    new XAttribute("Bool", ((kvp.Value.Capabilities & Capabilities.Deletable) == Capabilities.Deletable).ToString(CultureInfo.InvariantCulture))))))));

            return entityContainer;
        }

        private static XElement GetEntityKey(EdmProperty edmProperty)
            => new XElement(
                s_edmNs + "Key",
                new XElement(
                    s_edmNs + "PropertyRef",
                    new XAttribute("Name", edmProperty.Name)));

        private static IEnumerable<XElement> GetEntityTypes(EntityDataModel entityDataModel)
        {
            IEnumerable<XElement> entityTypes = entityDataModel.EntitySets.Values.Select(
                t =>
                {
                    var element = new XElement(
                      s_edmNs + "EntityType",
                      new XAttribute("Name", t.Name),
                      GetProperties(t.EdmType.Properties));

                    if (t.EdmType.BaseType is null)
                    {
                        element.AddFirst(GetEntityKey(t.EntityKey));
                    }
                    else
                    {
                        element.Add(new XAttribute("BaseType", t.EdmType.BaseType.FullName));
                    }

                    return element;
                });

            return entityTypes;
        }

        private static IEnumerable<XElement> GetEnumTypes(EntityDataModel entityDataModel)
        {
            // Any enums defined in the model.
            IEnumerable<XElement> enumTypes = entityDataModel.EntitySets
                .SelectMany(kvp => kvp.Value.EdmType.Properties)
                .Select(p => p.PropertyType)
                .OfType<EdmEnumType>()
                .Distinct()
                .Select(t => new XElement(
                    s_edmNs + "EnumType",
                    new XAttribute("Name", t.Name),
                    new XAttribute("UnderlyingType", EdmType.GetEdmType(Enum.GetUnderlyingType(t.ClrType)).FullName),
                    new XAttribute("IsFlags", (t.ClrType.GetCustomAttribute<FlagsAttribute>() != null).ToString(CultureInfo.InvariantCulture)),
                    t.Members.Select(m => new XElement(
                        s_edmNs + "Member",
                        new XAttribute("Name", m.Name),
                        new XAttribute("Value", m.Value.ToString(CultureInfo.InvariantCulture))))));

            return enumTypes;
        }

        private static IEnumerable<XElement> GetFunctions() => Enumerable.Empty<XElement>();

        private static IEnumerable<XElement> GetProperties(IEnumerable<EdmProperty> properties)
            => properties
            .Where(p => !p.IsNavigable)
            .Select(p =>
            {
                var element = new XElement(s_edmNs + "Property", new XAttribute("Name", p.Name), new XAttribute("Type", p.PropertyType.FullName));

                if (!p.IsNullable)
                {
                    element.Add(new XAttribute("Nullable", "false"));
                }

                return element;
            })
            .Concat(properties
                .Where(p => p.IsNavigable)
                .Select(p => new XElement(s_edmNs + "NavigationProperty", new XAttribute("Name", p.Name), new XAttribute("Type", p.PropertyType.FullName))));
    }
}
