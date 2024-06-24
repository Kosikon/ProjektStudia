# Base URL
$BaseUrl = "http://localhost:5002/api/users"

# Test data
$Username = "testuser"
$Password = "testpassword"
$NewUsername = "newuser"
$NewPassword = "newpassword"
$UpdatedUsername = "updateduser"
$UpdatedPassword = "updatedpassword"
$Role = "user"
$UpdatedRole = "admin"

# Authenticate
Write-Output "Authenticating..."
try {
    $authResponse = Invoke-RestMethod -Uri "$BaseUrl/authenticate" -Method Post -ContentType "application/json" -Body (@{Username=$Username; Password=$Password} | ConvertTo-Json)
    $Token = $authResponse.Token

    if (-not $Token) {
        Write-Output "Authentication failed."
        exit 1
    }
} catch {
    Write-Output "Authentication request failed: $_"
    exit 1
}

# Get All Users
Write-Output "Getting all users..."
try {
    Invoke-RestMethod -Uri $BaseUrl -Method Get -Headers @{Authorization = "Bearer $Token"}
    Write-Output ""
} catch {
    Write-Output "Get all users request failed: $_"
}

# Create User
Write-Output "Creating new user..."
try {
    $createResponse = Invoke-RestMethod -Uri $BaseUrl -Method Post -ContentType "application/json" -Headers @{Authorization = "Bearer $Token"} -Body (@{username=$NewUsername; role=$Role; password=$NewPassword} | ConvertTo-Json)
    $NewUserId = $createResponse.id
    Write-Output "New user ID: $NewUserId"
    Write-Output ""
} catch {
    Write-Output "Create user request failed: $_"
}

# Get User by ID
if ($NewUserId) {
    Write-Output "Getting user by ID..."
    try {
        Invoke-RestMethod -Uri "$BaseUrl/$NewUserId" -Method Get -Headers @{Authorization = "Bearer $Token"}
        Write-Output ""
    } catch {
        Write-Output "Get user by ID request failed: $_"
    }
} else {
    Write-Output "User creation failed, cannot get user by ID."
}

# Update User
if ($NewUserId) {
    Write-Output "Updating user..."
    try {
        Invoke-RestMethod -Uri "$BaseUrl/$NewUserId" -Method Put -ContentType "application/json" -Headers @{Authorization = "Bearer $Token"} -Body (@{username=$UpdatedUsername; role=$UpdatedRole; password=$UpdatedPassword} | ConvertTo-Json)
        Write-Output ""
    } catch {
        Write-Output "Update user request failed: $_"
    }
} else {
    Write-Output "User creation failed, cannot update user."
}

# Delete User
if ($NewUserId) {
    Write-Output "Deleting user..."
    try {
        Invoke-RestMethod -Uri "$BaseUrl/$NewUserId" -Method Delete -Headers @{Authorization = "Bearer $Token"}
        Write-Output ""
    } catch {
        Write-Output "Delete user request failed: $_"
    }
} else {
    Write-Output "User creation failed, cannot delete user."
}

Write-Output "Testing completed."


#Zadzialalo o 2 w nocy. Mission accomplished