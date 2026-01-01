@echo off
echo ========================================
echo Starting AuthAPI
echo ========================================
echo.

REM Kill any existing dotnet processes (optional)
REM taskkill /IM dotnet.exe /F 2>nul

echo Project Path: C:\cache_project\Backend\AuthAPI
echo.

cd /d "C:\cache_project\Backend\AuthAPI"

echo Starting API...
echo HTTPS: https://localhost:7000
echo HTTP:  http://localhost:5000
echo Swagger: https://localhost:7000/swagger
echo.
echo Press Ctrl+C to stop the API
echo.

dotnet run

pause
