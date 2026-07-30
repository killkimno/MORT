[CmdletBinding()]
param(
    [string]$RepositoryRoot,
    [switch]$Check
)

$ErrorActionPreference = "Stop"
$arguments = @{}
if (-not [string]::IsNullOrWhiteSpace($RepositoryRoot)) { $arguments.RepositoryRoot = $RepositoryRoot }
if ($Check) { $arguments.Check = $true }

& (Join-Path $PSScriptRoot "update-wiki-detailed.ps1") @arguments

$privacyArguments = @{}
if (-not [string]::IsNullOrWhiteSpace($RepositoryRoot)) { $privacyArguments.RepositoryRoot = $RepositoryRoot }
& (Join-Path $PSScriptRoot "check-wiki-privacy.ps1") @privacyArguments