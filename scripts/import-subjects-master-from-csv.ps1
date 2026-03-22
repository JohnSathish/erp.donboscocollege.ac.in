<#
.SYNOPSIS
  Converts subjects_master CSV to JSON and POSTs to POST /api/admissions/admin/class-xii-subjects (full replace).

.PARAMETER CsvPath
  Path to UTF-8 CSV with columns: boardCode, streamCode, subjectName, sortOrder

.PARAMETER BaseUrl
  API root, e.g. http://localhost:5227

.PARAMETER AdminToken
  JWT Bearer token for an Admin user (from POST /api/admin/auth/login or your admin login flow).

.EXAMPLE
  .\import-subjects-master-from-csv.ps1 -CsvPath "..\docs\subjects_master_import_template.csv" -BaseUrl "http://localhost:5227" -AdminToken "eyJhbG..."
#>
param(
    [Parameter(Mandatory = $true)]
    [string] $CsvPath,

    [Parameter(Mandatory = $false)]
    [string] $BaseUrl = "http://localhost:5227",

    [Parameter(Mandatory = $true)]
    [string] $AdminToken
)

$ErrorActionPreference = "Stop"
$resolved = Resolve-Path -Path $CsvPath
$rows = Import-Csv -Path $resolved -Encoding UTF8

$payloadRows = New-Object System.Collections.Generic.List[object]
foreach ($r in $rows) {
    $b = [string](@($r.boardCode, $r.BoardCode) | Where-Object { $_ } | Select-Object -First 1)
    $s = [string](@($r.streamCode, $r.StreamCode) | Where-Object { $_ } | Select-Object -First 1)
    $n = [string](@($r.subjectName, $r.SubjectName) | Where-Object { $_ } | Select-Object -First 1)
    $so = if ($null -ne $r.sortOrder -and $r.sortOrder -ne "") { $r.sortOrder } else { $r.SortOrder }
    $b = $b.Trim()
    $s = $s.Trim()
    $n = $n.Trim()
    if ([string]::IsNullOrWhiteSpace($b) -and [string]::IsNullOrWhiteSpace($s) -and [string]::IsNullOrWhiteSpace($n)) {
        continue
    }
    if ([string]::IsNullOrWhiteSpace($b) -or [string]::IsNullOrWhiteSpace($s) -or [string]::IsNullOrWhiteSpace($n)) {
        Write-Warning "Skipping incomplete row: board=$b stream=$s subject=$n"
        continue
    }
    $sortInt = 0
    if (-not [int]::TryParse([string]$so, [ref]$sortInt)) {
        throw "Invalid sortOrder for row: $b / $s / $n (value: $so)"
    }
    $payloadRows.Add([ordered]@{
        boardCode   = $b
        streamCode  = $s
        subjectName = $n
        sortOrder   = $sortInt
    })
}

if ($payloadRows.Count -eq 0) {
    throw "No data rows found in CSV."
}

$bodyObj = @{ rows = $payloadRows.ToArray() }
$json = $bodyObj | ConvertTo-Json -Depth 5 -Compress
$uri = "$BaseUrl/api/admissions/admin/class-xii-subjects".TrimEnd('/')
Write-Host "POST $uri ($($payloadRows.Count) rows)..."

$headers = @{
    Authorization = "Bearer $AdminToken"
    "Content-Type" = "application/json"
}

try {
    $response = Invoke-RestMethod -Uri $uri -Method Post -Headers $headers -Body $json -ContentType "application/json; charset=utf-8"
    Write-Host "OK: importedRowCount = $($response.importedRowCount)"
    $response
}
catch {
    $err = $_.ErrorDetails.Message
    if ($err) { Write-Host "Error: $err" -ForegroundColor Red }
    throw
}
