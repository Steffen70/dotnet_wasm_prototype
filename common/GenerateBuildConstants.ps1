param (
    [string]$ApiUrl
)

Write-Host "Generating BuildConstants.cs..."
(Get-Content "BuildConstants.template.cs") -replace "%API_URL%", $ApiUrl | Set-Content "BuildConstants.cs"