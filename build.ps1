# A script for building, testing and viewing code coverage locally
$scriptPath = Split-Path $script:MyInvocation.MyCommand.Path
$testResults = Join-Path $scriptPath -ChildPath '\Net.Http.OData.Tests\TestResults'

if (Test-Path $testResults){
    Remove-Item -Path $testResults -Recurse
}

dotnet clean
dotnet build
dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat=cobertura /p:CoverletOutput=.\TestResults\

Set-Location Net.Http.OData.Tests

dotnet reportgenerator "-reports:TestResults\coverage.cobertura.xml" "-targetdir:TestResults\Coverage" -reporttypes:HTML;

Start-Process "TestResults\Coverage\index.htm"

Set-Location ..
