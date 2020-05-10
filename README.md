# Net.Http.OData

Net.Http.OData is a .NET Standard library which makes it easy to consume an OData 4.0 Query.

![Nuget](https://img.shields.io/nuget/dt/Net.Http.OData)

|Branch|Status|
|------|------|
|/develop|![GitHub last commit (branch)](https://img.shields.io/github/last-commit/Net-Http-OData/Net.Http.OData/develop) [![Build Status](https://dev.azure.com/trevorpilley/Net.Http.OData/_apis/build/status/Net-Http-OData.Net.Http.OData?branchName=develop)](https://dev.azure.com/trevorpilley/Net.Http.OData/_build/latest?definitionId=37&branchName=develop) ![Nuget (with prereleases)](https://img.shields.io/nuget/vpre/Net.Http.OData)|
|/master|![GitHub last commit](https://img.shields.io/github/last-commit/Net-Http-OData/Net.Http.OData/master) [![Build Status](https://dev.azure.com/trevorpilley/Net.Http.OData/_apis/build/status/Net-Http-OData.Net.Http.OData?branchName=master)](https://dev.azure.com/trevorpilley/Net.Http.OData/_build/latest?definitionId=37&branchName=master) ![Nuget](https://img.shields.io/nuget/v/Net.Http.OData) ![GitHub Release Date](https://img.shields.io/github/release-date/Net-Http-OData/Net.Http.OData)|

## Installation

Install the nuget package `dotnet add package Net.Http.OData` or `Install-Package Net.Http.OData`

## Entity Data Model

The Entity Data Model describes the entitites in the OData service, the classes are in the namespace [Net.Http.OData.Model](https://github.com/Net-Http-OData/Net.Http.OData/wiki/Net.Http.OData.Model).

## Query

The Query classes describe an OData Query request, the classes are in the namespace [Net.Http.OData.Query](https://github.com/Net-Http-OData/Net.Http.OData/wiki/Net.Http.OData.Query).

Also see:

* [Parsing Query Options](https://github.com/Net-Http-OData/Net.Http.OData/wiki/Parsing-Query-Options) for details of how to parse the `ODataQueryOptions` class.
* [Supported Query Syntax](https://github.com/Net-Http-OData/Net.Http.OData/wiki/Supported-Query-Syntax) for details of the OData query syntax supported by the library.

For further details regarding the OData 4.0 specification, see:

* [Part 1 - Protocol](http://docs.oasis-open.org/odata/odata/v4.0/odata-v4.0-part1-protocol.html)
* [Part 2 - URL Conventions](http://docs.oasis-open.org/odata/odata/v4.0/odata-v4.0-part2-url-conventions.html)
* [Part 3 - Common Schema Definition Language (CSDL)](http://docs.oasis-open.org/odata/odata/v4.0/odata-v4.0-part3-csdl.html)

### Supported .NET Versions

The NuGet Package contains binaries compiled against:

* .NET Standard 2.0
  * _Has an implicit dependency on Newtonsoft.Json 10.0.1 or later due to the internal use of the JsonPropertyAttribute_
  * _Has an implicit dependency on System.Text.Json 4.6.0 or later due to the internal use of the JsonPropertyNameAttribute_

To find out more, head over to the [Wiki](https://github.com/Net-Http-OData/Net.Http.OData/wiki).
