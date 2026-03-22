# Script to update Preskool dashboard HTML paths
$sourceFile = "src\client\apps\src\app\admin\preskool-template\src\app\features\dashboards\admin-dashboard\admin-dashboard.component.html"
$targetFile = "src\client\apps\src\app\admin\dashboard\admin-dashboard-preskool.component.html"

if (Test-Path $sourceFile) {
    Write-Host "Reading source file..."
    $content = Get-Content $sourceFile -Raw -Encoding UTF8
    
    Write-Host "Replacing image paths..."
    $content = $content -replace 'assets/img/', '/preskool-assets/images/'
    
    Write-Host "Writing to target file..."
    $content | Set-Content $targetFile -Encoding UTF8 -NoNewline
    
    Write-Host "Done! File created at: $targetFile"
} else {
    Write-Host "Source file not found: $sourceFile"
    Write-Host "Current directory: $(Get-Location)"
}



