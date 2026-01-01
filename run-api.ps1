# Kill any existing dotnet processes running on port 7000 or 5000
Write-Host "Stopping any existing API processes..." -ForegroundColor Yellow

# Kill processes using port 7000 (HTTPS)
$port7000 = Get-NetTCPConnection -LocalPort 7000 -ErrorAction SilentlyContinue
if ($port7000) {
    $processId = $port7000.OwningProcess
    Stop-Process -Id $processId -Force -ErrorAction SilentlyContinue
    Write-Host "Stopped process on port 7000" -ForegroundColor Green
}

# Kill processes using port 5000 (HTTP)
$port5000 = Get-NetTCPConnection -LocalPort 5000 -ErrorAction SilentlyContinue
if ($port5000) {
    $processId = $port5000.OwningProcess
    Stop-Process -Id $processId -Force -ErrorAction SilentlyContinue
    Write-Host "Stopped process on port 5000" -ForegroundColor Green
}

# Alternative: Kill all dotnet processes (use with caution)
# Get-Process -Name "dotnet" -ErrorAction SilentlyContinue | Stop-Process -Force

Write-Host "`nStarting AuthAPI..." -ForegroundColor Cyan
Write-Host "Project Path: C:\cache_project\Backend\AuthAPI" -ForegroundColor Gray

# Navigate to project directory and run
Set-Location "C:\cache_project\Backend\AuthAPI"

# Run the API
Write-Host "`nAPI is starting..." -ForegroundColor Green
Write-Host "HTTPS: https://localhost:7000" -ForegroundColor Cyan
Write-Host "HTTP:  http://localhost:5000" -ForegroundColor Cyan
Write-Host "Swagger: https://localhost:7000/swagger" -ForegroundColor Cyan
Write-Host "`nPress Ctrl+C to stop the API`n" -ForegroundColor Yellow

dotnet run
