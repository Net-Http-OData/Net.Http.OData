REM A script for building, testing and viewing code coverage locally
dotnet build
dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat=cobertura /p:CoverletOutput=.\TestResults\
cd Net.Http.OData.Tests
dotnet reportgenerator "-reports:TestResults\coverage.cobertura.xml" "-targetdir:TestResults\html" -reporttypes:HTML;
start "" "TestResults\html\index.htm"
cd..
