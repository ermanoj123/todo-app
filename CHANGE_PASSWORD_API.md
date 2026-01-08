# Change Password API Documentation

## Endpoint
`POST /api/auth/change-password`

## Description
This endpoint allows authenticated users to change their password.

## Authentication
Requires a valid JWT token in the Authorization header:
```
Authorization: Bearer <your-jwt-token>
```

## Request Body
```json
{
  "currentPassword": "string",
  "newPassword": "string",
  "confirmPassword": "string"
}
```

### Field Validations
- **currentPassword**: Required
- **newPassword**: Required, minimum 6 characters
- **confirmPassword**: Required, must match newPassword

## Response

### Success (200 OK)
```json
{
  "message": "Password changed successfully"
}
```

### Error Responses

#### 400 Bad Request - Invalid Current Password
```json
{
  "message": "Current password is incorrect"
}
```

#### 400 Bad Request - Validation Error
```json
{
  "errors": {
    "NewPassword": ["Password must be at least 6 characters long"],
    "ConfirmPassword": ["New password and confirm password do not match"]
  }
}
```

#### 401 Unauthorized
```json
{
  "message": "Unauthorized"
}
```

#### 500 Internal Server Error
```json
{
  "message": "An error occurred while changing password"
}
```

## Example Usage

### Using cURL
```bash
curl -X POST http://localhost:5000/api/auth/change-password \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer YOUR_JWT_TOKEN" \
  -d '{
    "currentPassword": "oldPassword123",
    "newPassword": "newPassword456",
    "confirmPassword": "newPassword456"
  }'
```

### Using JavaScript (Fetch API)
```javascript
const changePassword = async (currentPassword, newPassword, confirmPassword) => {
  const token = localStorage.getItem('token'); // or however you store the token
  
  const response = await fetch('http://localhost:5000/api/auth/change-password', {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json',
      'Authorization': `Bearer ${token}`
    },
    body: JSON.stringify({
      currentPassword,
      newPassword,
      confirmPassword
    })
  });
  
  const data = await response.json();
  
  if (response.ok) {
    console.log('Password changed successfully');
    return data;
  } else {
    console.error('Error:', data.message);
    throw new Error(data.message);
  }
};

// Usage
changePassword('oldPassword123', 'newPassword456', 'newPassword456')
  .then(result => console.log(result))
  .catch(error => console.error(error));
```

## Security Notes
1. The endpoint requires authentication via JWT token
2. The current password is verified before allowing the change
3. Passwords are hashed using SHA256 before storage
4. The new password must be at least 6 characters long
5. The confirm password must match the new password

## Implementation Details
- The username is extracted from the JWT token claims
- The current password is verified against the stored hash
- If verification succeeds, the new password is hashed and saved
- All password operations are performed asynchronously
