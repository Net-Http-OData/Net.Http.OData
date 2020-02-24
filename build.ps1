# A script for building, testing and viewing code coverage locally
dotnet clean
dotnet build
dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat=cobertura /p:CoverletOutput=.\TestResults\

Set-Location Net.Http.OData.Tests

dotnet reportgenerator "-reports:TestResults\coverage.cobertura.xml" "-targetdir:TestResults\html" -reporttypes:HTML;

Start-Process "TestResults\html\index.htm"

Set-Location ..
