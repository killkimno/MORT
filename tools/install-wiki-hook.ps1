[CmdletBinding()]
param()

$ErrorActionPreference = "Stop"
$repositoryRoot = [IO.Path]::GetFullPath((Join-Path $PSScriptRoot ".."))

Push-Location $repositoryRoot
try {
    git config core.hooksPath .githooks
    if ($LASTEXITCODE -ne 0) {
        throw "Failed to configure core.hooksPath."
    }
    Write-Host "Git hooks enabled from .githooks."
}
finally {
    Pop-Location
}
