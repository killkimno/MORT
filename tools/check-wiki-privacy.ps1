[CmdletBinding()]
param(
    [string]$RepositoryRoot
)

$ErrorActionPreference = "Stop"
if ([string]::IsNullOrWhiteSpace($RepositoryRoot)) {
    $RepositoryRoot = [IO.Path]::GetFullPath((Join-Path $PSScriptRoot ".."))
}

$wikiDirectory = Join-Path $RepositoryRoot "docs\wiki"
$targets = @(
    "index.html",
    "wiki-content.json",
    "feature-content.json",
    "file-overrides.json"
) | ForEach-Object { Join-Path $wikiDirectory $_ }

$rules = [ordered]@{
    "email address" = "(?i)[A-Z0-9._%+-]+@[A-Z0-9.-]+\.[A-Z]{2,}"
    "user home path" = "(?i)(?:[A-Z]:\\Users\\|/Users/|/home/)[^\\/\s<]+"
    "API key or bearer token" = "(?i)\b(?:AIza[0-9A-Za-z_-]{20,}|sk-[0-9A-Za-z_-]{16,}|gh[pousr]_[0-9A-Za-z]{20,}|Bearer\s+[0-9A-Za-z._-]{16,})\b"
    "assigned credential" = "(?i)\b(?:api[_ -]?key|secret|token|password|credential)\s*[:=]\s*[""'][^""']{6,}[""']"
    "Korean phone number" = "\b01[016789][- ]?\d{3,4}[- ]?\d{4}\b"
    "Korean resident number" = "\b\d{6}[- ]?[1-4]\d{6}\b"
    "environment account name" = "(?i)\b(?:killk|killkimno)\b"
}

$violations = New-Object Collections.Generic.List[string]
foreach ($target in $targets) {
    if (-not (Test-Path -LiteralPath $target)) {
        throw "Wiki privacy check target is missing: $target"
    }

    $text = Get-Content -LiteralPath $target -Raw -Encoding UTF8
    foreach ($rule in $rules.GetEnumerator()) {
        if ([regex]::IsMatch($text, $rule.Value)) {
            $relativePath = $target.Substring($RepositoryRoot.TrimEnd("\").Length + 1)
            $violations.Add("$($rule.Key): $relativePath")
        }
    }
}

if ($violations.Count -gt 0) {
    throw "Wiki privacy check failed:`n$($violations -join [Environment]::NewLine)"
}

Write-Host "Wiki privacy check passed."
