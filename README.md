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

See the [Entity Data Model](https://github.com/Net-Http-OData/Net.Http.OData/wiki/Entity-Data-Model) page in the wiki.

## Query

The Query classes describe an OData Query request, the classes are in the namespace `Net.Http.OData.Query`.

* See the [Query])https://github.com/Net-Http-OData/Net.Http.OData/wiki/Query) page in the wiki for details of the classes.
* See the [Query - Supported Syntax](https://github.com/Net-Http-OData/Net.Http.OData/wiki/Query---Supported-Syntax) page in the wiki for details of the OData query syntax supported by the library.
* See the [Query - Parsing Query Options](https://github.com/Net-Http-OData/Net.Http.OData/wiki/Query---Parsing-Query-Options) page in the wiki for details of how to parse the `ODataQueryOptions` class.

### Supported .NET Versions

The NuGet Package contains binaries compiled against:

* .NET Framework 4.5
* .NET Standard 2.0

To find out more, head over to the [Wiki](https://github.com/Net-Http-OData/Net.Http.OData/wiki).
