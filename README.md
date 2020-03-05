Net.Http.OData
==============

|Service|Status|
|-------|------|
||[![NuGet version](https://badge.fury.io/nu/Net.Http.OData.svg)](http://badge.fury.io/nu/Net.Http.OData)|
|/develop|[![Build Status](https://dev.azure.com/trevorpilley/Net.Http.OData/_apis/build/status/Net-Http-OData.Net.Http.OData?branchName=develop)](https://dev.azure.com/trevorpilley/Net.Http.OData/_build/latest?definitionId=19&branchName=develop)|
|/master|[![Build Status](https://dev.azure.com/trevorpilley/Net.Http.OData/_apis/build/status/Net-Http-OData.Net.Http.OData?branchName=master)](https://dev.azure.com/trevorpilley/Net.Http.OData/_build/latest?definitionId=19&branchName=master)|

Net.Http.OData is a .NET Standard library which makes it easy to consume an OData 4.0 Query.

## Installation

Install the nuget package `dotnet add package Net.Http.OData` or `Install-Package Net.Http.OData`

## Entity Data Model

The Entity Data Model describes the entitites in the OData service, the classes are in the namespace `[Net.Http.OData.Model](https://github.com/Net-Http-OData/Net.Http.OData/wiki/Net.Http.OData.Model)`.

## Query

The Query classes describe an OData Query request, the classes are in the namespace `[Net.Http.OData.Query](https://github.com/Net-Http-OData/Net.Http.OData/wiki/Net.Http.OData.Query)`.

Also see:

* [Parsing Query Options](https://github.com/Net-Http-OData/Net.Http.OData/wiki/Parsing-Query-Options) for details of how to parse the `ODataQueryOptions` class.
* [Supported Query Syntax](https://github.com/Net-Http-OData/Net.Http.OData/wiki/Supported-Query-Syntax) for details of the OData query syntax supported by the library.

### Supported .NET Versions

The NuGet Package contains binaries compiled against:

* .NET Standard 2.0
* .NET Framework 4.5

To find out more, head over to the [Wiki](https://github.com/Net-Http-OData/Net.Http.OData/wiki).
