# Run AuthAPI with IIS Express (if you prefer IIS Express over Kestrel)

Write-Host "Stopping any existing IIS Express processes..." -ForegroundColor Yellow
taskkill /IM iisexpress.exe /F 2>$null

Write-Host "`nStarting AuthAPI with IIS Express..." -ForegroundColor Cyan
Write-Host "Project Path: C:\cache_project\Backend\AuthAPI" -ForegroundColor Gray
Write-Host "Port: 7000" -ForegroundColor Cyan
Write-Host "`nAPI will be available at: https://localhost:7000" -ForegroundColor Green
Write-Host "Press Ctrl+C to stop`n" -ForegroundColor Yellow

# Run with IIS Express
& "C:\Program Files (x86)\IIS Express\iisexpress.exe" /path:"C:\cache_project\Backend\AuthAPI" /port:7000 /systray:false

# Alternative with specific ports for HTTP and HTTPS
# & "C:\Program Files (x86)\IIS Express\iisexpress.exe" /path:"C:\cache_project\Backend\AuthAPI" /port:5000 /systray:false
