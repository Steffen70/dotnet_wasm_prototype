param (
    [string]$UnoPackageGuid,
    [string]$UnoPackageDir,
    [string]$PublishDir
)

# Replace %PACKAGE_GUID% in index.html
Write-Host "Patching index.html with UnoPackageGuid: $UnoPackageGuid"
$content = Get-Content 'src/index.html'
$fixed = $content -replace '%PACKAGE_GUID%', $UnoPackageGuid
Set-Content -Path "$PublishDir/wwwroot/index.html" -Value $fixed

# Remove sourceMappingURL lines from JS files
Write-Host "Stripping sourceMappingURL from JS files..."
Get-ChildItem -Recurse -Filter '*.js' $PublishDir | ForEach-Object {
    (Get-Content $_) | Where-Object { $_ -notmatch 'sourceMappingURL' } | Set-Content $_
}

# Remove 'runtimeOptions' block from mono-config.json
Write-Host "Removing runtimeOptions block from mono-config.json..."
$configPath = Join-Path $UnoPackageDir 'mono-config.json'
$lines = Get-Content $configPath
$inBlock = $false
$filtered = foreach ($line in $lines) {
    if ($line -match '^\s*"runtimeOptions"\s*:\s*\[') {
        $inBlock = $true; continue
    }
    if ($inBlock -and $line -match '\],?') {
        $inBlock = $false; continue
    }
    if (-not $inBlock) {
        $line
    }
}
$filtered | Set-Content $configPath
