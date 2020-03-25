# A script for building, testing and viewing code coverage locally
param(
    [bool]$showCoverage = $true,
    [bool]$createNupkg = $false
)
$scriptPath = Split-Path $script:MyInvocation.MyCommand.Path
$testResults = Join-Path $scriptPath -ChildPath '\Net.Http.OData.Tests\TestResults'

if (Test-Path $testResults) {
    Remove-Item -Path $testResults -Recurse
}

if ((Test-NetConnection).PingSucceeded) {
    dotnet tool update --global dotnet-reportgenerator-globaltool
}

dotnet clean
dotnet build
dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat=cobertura /p:CoverletOutput=.\TestResults\

if ($createNupkg) {
    dotnet pack --no-build
}

if ($showCoverage) {
    Set-Location Net.Http.OData.Tests
    reportgenerator "-reports:TestResults\coverage.cobertura.xml" "-targetdir:TestResults\Coverage" -reporttypes:HTML
    Start-Process "TestResults\Coverage\index.htm"
    Set-Location ..
}
