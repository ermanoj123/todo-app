# 🚀 How to Run AuthAPI

## Option 1: Using PowerShell Script (Recommended)

### Simple Run
```powershell
.\run-api.ps1
```

This script will:
- Stop any existing processes on ports 5000 and 7000
- Navigate to the project directory
- Start the API with `dotnet run`

### Access Points
- **HTTPS**: https://localhost:7000
- **HTTP**: http://localhost:5000
- **Swagger**: https://localhost:7000/swagger

---

## Option 2: Using Batch File

### Simple Run
```cmd
run-api.bat
```

Double-click the `run-api.bat` file or run it from command prompt.

---

## Option 3: Using IIS Express

### Run with IIS Express
```powershell
.\run-with-iis-express.ps1
```

This uses IIS Express instead of Kestrel (the default .NET server).

**Note**: Requires IIS Express to be installed at:
`C:\Program Files (x86)\IIS Express\iisexpress.exe`

---

## Option 4: Manual Command

### From PowerShell or Command Prompt

```bash
cd C:\cache_project\Backend\AuthAPI
dotnet run
```

---

## Option 5: Using Visual Studio

1. Open `AuthAPI.csproj` in Visual Studio
2. Press **F5** or click the green "Play" button
3. API will start automatically

---

## Option 6: Using VS Code

1. Open the project folder in VS Code
2. Press **F5** to start debugging
3. Or use the terminal: `dotnet run`

---

## Stopping the API

### If running in terminal:
- Press **Ctrl + C**

### If running in background:
```powershell
# PowerShell - Kill processes on specific ports
Get-NetTCPConnection -LocalPort 7000 | ForEach-Object { Stop-Process -Id $_.OwningProcess -Force }
Get-NetTCPConnection -LocalPort 5000 | ForEach-Object { Stop-Process -Id $_.OwningProcess -Force }
```

```cmd
REM Command Prompt - Kill all dotnet processes
taskkill /IM dotnet.exe /F
```

---

## Custom Port Configuration

### Change Ports in launchSettings.json

Edit: `Properties/launchSettings.json`

```json
{
  "profiles": {
    "https": {
      "commandName": "Project",
      "dotnetRunMessages": true,
      "launchBrowser": true,
      "applicationUrl": "https://localhost:7000;http://localhost:5000",
      "environmentVariables": {
        "ASPNETCORE_ENVIRONMENT": "Development"
      }
    }
  }
}
```

Change `7000` and `5000` to your desired ports.

---

## Environment Variables

### Development
```powershell
$env:ASPNETCORE_ENVIRONMENT = "Development"
dotnet run
```

### Production
```powershell
$env:ASPNETCORE_ENVIRONMENT = "Production"
dotnet run
```

---

## Troubleshooting

### Port Already in Use

**Error**: "Unable to bind to https://localhost:7000"

**Solution 1**: Kill the process using the port
```powershell
# Find process using port 7000
Get-NetTCPConnection -LocalPort 7000

# Kill it
Stop-Process -Id <ProcessId> -Force
```

**Solution 2**: Change the port in `launchSettings.json`

### Database Not Found

**Error**: "Cannot open database 'AuthDB'"

**Solution**: Run migrations
```bash
dotnet ef database update
```

### Missing Dependencies

**Error**: Package not found

**Solution**: Restore packages
```bash
dotnet restore
dotnet build
```

---

## Quick Commands Reference

```bash
# Restore packages
dotnet restore

# Build project
dotnet build

# Run project
dotnet run

# Run with specific environment
dotnet run --environment Production

# Run on specific URL
dotnet run --urls "https://localhost:8000;http://localhost:8001"

# Watch mode (auto-restart on file changes)
dotnet watch run

# Create migration
dotnet ef migrations add MigrationName

# Update database
dotnet ef database update

# Clean build artifacts
dotnet clean
```

---

## Performance Tips

### Use Watch Mode for Development
```bash
dotnet watch run
```
This automatically restarts the API when you make code changes.

### Build in Release Mode
```bash
dotnet build -c Release
dotnet run -c Release
```

---

## Running Multiple Instances

To run multiple instances on different ports:

**Instance 1:**
```bash
dotnet run --urls "https://localhost:7000"
```

**Instance 2:**
```bash
dotnet run --urls "https://localhost:7001"
```

---

## Scripts Summary

| Script | Purpose | Usage |
|--------|---------|-------|
| `run-api.ps1` | PowerShell startup script | `.\run-api.ps1` |
| `run-api.bat` | Batch file startup | `run-api.bat` |
| `run-with-iis-express.ps1` | IIS Express startup | `.\run-with-iis-express.ps1` |

---

## Recommended Workflow

### For Development:
1. Use `dotnet watch run` for auto-restart
2. Keep Swagger open: https://localhost:7000/swagger
3. Monitor console for logs

### For Testing:
1. Use `.\run-api.ps1` for clean start
2. Test with Swagger UI
3. Check database after operations

### For Production:
1. Build in Release mode
2. Use environment variables for config
3. Set up proper hosting (IIS, Azure, etc.)

---

## Next Steps

After starting the API:
1. ✅ Open Swagger: https://localhost:7000/swagger
2. ✅ Test `/api/auth/register` endpoint
3. ✅ Test `/api/auth/login` endpoint
4. ✅ Copy JWT token from response
5. ✅ Use token to test `/api/auth/profile`

---

**Need Help?**
- Check the main README.md
- Review QUICKSTART.md
- Consult SETUP_CHECKLIST.md
