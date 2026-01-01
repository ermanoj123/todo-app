# AuthAPI - .NET 9 Web API

## Quick Start

### 1. Install EF Core Tools
```bash
dotnet tool install --global dotnet-ef
```

### 2. Create Migration
```bash
dotnet ef migrations add InitialCreate
```

### 3. Update Database
```bash
dotnet ef database update
```

### 4. Run the API
```bash
dotnet run
```

## API Documentation

Access Swagger UI at: `https://localhost:7000/swagger`

## Configuration

Edit `appsettings.json` to configure:
- Database connection string
- JWT settings (secret key, expiry, etc.)

## Project Structure

- **Controllers/**: API endpoints
- **Data/**: Database context
- **Models/**: Data models and DTOs
- **Services/**: Business logic and JWT token generation

## Testing the API

### Using Swagger
1. Navigate to `https://localhost:7000/swagger`
2. Test endpoints directly from the UI

### Using curl

**Register:**
```bash
curl -X POST https://localhost:7000/api/auth/register \
  -H "Content-Type: application/json" \
  -d '{
    "username": "testuser",
    "email": "test@example.com",
    "password": "password123",
    "firstName": "Test",
    "lastName": "User"
  }'
```

**Login:**
```bash
curl -X POST https://localhost:7000/api/auth/login \
  -H "Content-Type: application/json" \
  -d '{
    "username": "testuser",
    "password": "password123"
  }'
```

**Get Profile (with token):**
```bash
curl -X GET https://localhost:7000/api/auth/profile \
  -H "Authorization: Bearer YOUR_TOKEN_HERE"
```
