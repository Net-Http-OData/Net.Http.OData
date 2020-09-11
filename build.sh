#!/bin/sh

rm -r Net.Http.OData.Tests/TestResults/

dotnet tool update --global dotnet-reportgenerator-globaltool

dotnet clean
dotnet build
dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat=cobertura /p:CoverletOutput=./TestResults/

cd Net.Http.OData.Tests
reportgenerator "-reports:TestResults/coverage.cobertura.xml" "-targetdir:TestResults/Coverage" -reporttypes:HTML
open "TestResults/Coverage/index.htm"

dotnet pack --no-build
