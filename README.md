Net.Http.OData
==============

|Service|Status|
|-------|------|
||[![NuGet version](https://badge.fury.io/nu/Net.Http.OData.svg)](http://badge.fury.io/nu/Net.Http.OData)|
|/develop|[![Build Status](https://dev.azure.com/trevorpilley/Net.Http.OData/_apis/build/status/Net-Http-OData.Net.Http.OData?branchName=develop)](https://dev.azure.com/trevorpilley/Net.Http.OData/_build/latest?definitionId=19&branchName=develop)|
|/master|[![Build Status](https://dev.azure.com/trevorpilley/Net.Http.OData/_apis/build/status/Net-Http-OData.Net.Http.OData?branchName=master)](https://dev.azure.com/trevorpilley/Net.Http.OData/_build/latest?definitionId=19&branchName=master)|

Net.Http.OData is a .NET Standard library which makes it easy to consume an OData 4.0 Query.

## Installation

Install the nuget package `Install-Package Net.Http.OData` or `dotnet add package Net.Http.OData`

## Entity Data Model

The Entity Data Model describes the entitites in the OData service, the classes are in the namespace `Net.Http.OData.Model`.

### EdmCollectionType

Represents a collection type in the Entity Data Model.

|Property|Description|
|--------|-----------|
|EdmType ContainedType { get; }|Gets the EdmType contained in the collection.|

### EdmComplexType

Represents a complex type in the Entity Data Model.

|Property|Description|
|--------|-----------|
|EdmType BaseType { get; }|Gets the EdmType from which this EdmComplexType directly inherits.|
|IReadOnlyList<EdmProperty> Properties { get; }|Gets the properties defined on the type.|

|Method|Description|
|--------|-----------|
|EdmProperty GetProperty(string name)|Gets the property with the specified name.|

### EdmEnumMember

Represents an enum member in the Entity Data Model.

|Property|Description|
|--------|-----------|
|string Name { get; }|Gets the name of the enum member.|
|int Value { get; }|Gets the integer value of the enum member.|

### EdmEnumType

Represents an Enum type in the Entity Data Model.

|Property|Description|
|--------|-----------|
|IReadOnlyList<EdmEnumMember> Members { get; }|Gets the EdmEnumMembers that represent the values of the underlying enum.|

### EdmPrimitiveType

Represents a primitive type in the Entity Data Model.

### EdmProperty

Represents an entity property in the Entity Data Model.

|Property|Description|
|--------|-----------|
|EdmComplexType DeclaringType { get; }|Gets the type in the Entity Data Model which declares this property.|
|bool IsNavigable|Gets a value indicating whether the property is navigable (i.e. a navigation property).|
|bool IsNullable { get; }|Gets a value indicating whether the property is nullable.|
|string Name { get; }|Gets the name of the property.|
|EdmType PropertyType { get; }|Gets the type of the property in the Entity Data Model.|

### EdmType

Represents a type in the Entity Data Model.

|Property|Description|
|--------|-----------|
|Type ClrType { get; }|Gets the CLR type.|
|string FullName { get; }|Gets the full name.|
|string Name { get; }|Gets the name.|

### EntityDataModel

Represents the Entity Data Model.

|Property|Description|
|--------|-----------|
|static EntityDataModel Current|Gets the current Entity Data Model.|
|IReadOnlyDictionary<string, EntitySet> EntitySets { get; }|Gets the Entity Sets defined in the Entity Data Model.|
|IReadOnlyCollection<string> FilterFunctions { get; }|Gets the filter functions provided by the service.|
|IReadOnlyCollection<string> SupportedFormats { get; }|Gets the formats supported by the service.|

|Method|Description|
|------|-----------|
|EntitySet EntitySetForPath(string path)|Gets the EntitySet for the path segment of a URI.|

### EntityDataModelBuilder

Used to build the Entity Data Model using a fluent API, it should be used once at application startup.

|Method|Description|
|------|-----------|
|RegisterEntitySet<T>(string entitySetName, Expression<Func<T, object>> entityKeyExpression)|Registers an Entity Set of the specified type to the Entity Data Model with the specified name which can only be queried.|
|RegisterEntitySet<T>(string entitySetName, Expression<Func<T, object>> entityKeyExpression, Capabilities capabilities)|Registers an Entity Set of the specified type to the Entity Data Model with the specified name and Capabilities.|

### EntitySet

Represents an Entity Set in the Entity Data Model.

|Property|Description|
|--------|-----------|
|Capabilities Capabilities { get; }|Gets the Capabilities of the Entity Set.|
|EdmComplexType EdmType { get; }|Gets the EdmComplexType of the entities in the set.|
|EdmProperty EntityKey { get; }|Gets the EdmProperty which is the entity key.|
|string Name { get; }|Gets the name of the Entity Set.|

### Supported .NET Versions

The NuGet Package contains binaries compiled against:

* .NET Framework 4.5
* .NET Standard 2.0

To find out more, head over to the [Wiki](https://github.com/Net-Http-OData/Net.Http.OData/wiki).
