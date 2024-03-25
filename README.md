# Session Management Backend

## Tech Stack: ASP.NET Core Web API

This backend system is designed for session management, user authentication, and key management using ASP.NET Core Web API + Swagger.

## Table of Contents
# API Reference

## Auth API
| Method | URL                             | Description           | Role  |
|--------|---------------------------------|-----------------------|-------|
| POST   | `api/Auth/RegisterAdmin`        | Admin Registration    | Admin |
| POST   | `api/Auth/RegisterUser`         | User Registration     | User  |
| POST   | `api/Auth/Login`                | Log in                |       |

## User API
| Method | URL                      | Description                   | Role  |
|--------|--------------------------|-------------------------------|-------|
| GET    | `api/User`               | Get all user list             | User |
| GET    | `api/User/id`            | Get Specific user by ID       | User |
| PUT    | `api/user/id`            | Update user information       | User |
| DELETE | `api/user/id`            | Delete user by id             | Admin |
| PATCH  | `api/user/id/block`      | Block user by id              | Admin |
| PATCH  | `api/user/id/change-password` | Change user password      | User  |

## Key API
| Method | URL                    | Description                   | Role  |
|--------|------------------------|-------------------------------|-------|
| GET    | `api/key`               | Get all key list              | Admin |
| POST   | `api/key`               | Create a new key              | Admin |
| GET    | `api/key/id`            | Get a key by id               | Admin |
| DELETE | `api/key/id`            | Delete a key by id            | Admin |
| POST   | `api/key/change-key-value/id` | Change key value of existing key | Admin |

## Key Extension API
| Method | URL                           | Description                          | Role  |
|--------|-------------------------------|--------------------------------------|-------|
| POST   | `api/KeyExtension`            | Extend the expiry date by one year   | Admin |
| GET    | `api/KeyExtension/check-expiration/id` | Check the expiry date of a key by id | User  |
| GET    | `api/KeyExtension/check-expiration-by-value/keyvalue` | Check the expiry date of a key by its key value | User |

## User Key API
| Method | URL                    | Description                | Role  |
|--------|------------------------|----------------------------|-------|
| GET    | `api/UserKey/id`       | Check userKey ID            | User  |
| GET    | `api/UserKey/key-info/id` | Check key information      | User  |
| POST   | `api/UserKey`          | Register a key for a user   | User  |


  
## List of API Endpoints

### Auth API

#### `POST api/Auth/RegisterAdmin`

- **Description:** Allows the registration of an admin user.

  - **Request Body:**
    ```json
    {
      "username": "user@gmail.com",
      "password": "password here"
    }
    ```

  - **Response:**
    - **Success (201 Created):** Admin registration successful. Returns the newly created admin user details.
      ```json
      User Registered as Admin Successfully
      ```
    - **Error (4xx):** If registration fails due to validation errors or duplicate entries, an appropriate error response is returned.
      ```json
      {
        "error": "User already exists"
      }
      ```

#### `POST api/Auth/RegisterUser`

- **Description:** Allows the registration of a regular user.

  - **Request Body:**
    ```json
    {
      "userID": 0,
      "username": "jhon@gmail.com",
      "email": "jhon@gmail.com",
      "password": "jhondoe",
      "phoneNumber": "+251948751236",
      "address": "Ababa",
      "isBlocked": false,
      "isEmailVerified": false
    }
    ```

  - **Response:**
    - **Success (201 Created):** User registration successful. Returns the newly created user details.
      ```json
      {
        "userID": 8,
        "username": "jhon@gmail.com",
        "email": "jhon@gmail.com",
        "password": null,
        "phoneNumber": "+251948751236",
        "address": "Ababa",
        "isBlocked": false,
        "isEmailVerified": false
      }
      ```
    - **Error (4xx):** If registration fails due to validation errors or duplicate entries, an appropriate error response is returned.
      ```json
      {
        "error": "User already exists"
      }
      ```

#### `POST api/Auth/Login`

- **Description:** Handles user login.

  - **Request Body:**
    ```json
    {
      "username": "anteneh@gmail.com",
      "password": "anteneh"
    }
    ```

  - **Response:**
    - **Success (200 OK):** User authentication successful. Returns an authentication token.
      ```json
      {
        "token": "New Generated Token"
      }
      ```
    - **Error (4xx):** If login fails due to incorrect credentials or other issues, an appropriate error response is returned.
      ```json
      {
        "error": "Invalid credentials. Please try again."
      }
      ```

## User API

### `GET api/User`

- **Description:** Retrieves a list of all users.

  - **Authorization:** Requires a valid authentication token.

  - **Response:**
    - **Success (200 OK):** Returns a list of user details.
      ```json
      [
        {
          "userID": 1,
          "username": "john_doe",
          "email": "john.doe@example.com",
          "phoneNumber": "+1234567890",
          "address": "123 Main St, Cityville",
          "isBlocked": false,
          "isEmailVerified": true
        },
        // ... other users
      ]
      ```

### `GET api/User/{id}`

- **Description:** Retrieves details of a specific user by ID.

  - **Authorization:** Requires a valid authentication token.

  - **Response:**
    - **Success (200 OK):** Returns the user details.
      ```json
      {
        "userID": 1,
        "username": "john_doe",
        "email": "john.doe@example.com",
        "phoneNumber": "+1234567890",
        "address": "123 Main St, Cityville",
        "isBlocked": false,
        "isEmailVerified": true
      }
      ```

### `PUT api/User/{id}`

- **Description:** Updates user information.

  - **Authorization:** Requires a valid authentication token.

  - **Request Body:**
    ```json
    {
      "phoneNumber": "+9876543210",
      "address": "456 Oak St, Townsville"
    }
    ```

  - **Response:**
    - **Success (200 OK):** User information updated successfully.

  - **Error (4xx/5xx):** Returns appropriate error responses for invalid requests or server errors.

### `DELETE api/User/{id}`

- **Description:** Deletes a user by ID.

  - **Authorization:** Requires a valid authentication token.

  - **Response:**
    - **Success (200 OK):** User deleted successfully.

  - **Error (4xx/5xx):** Returns appropriate error responses for invalid requests or server errors.

### `PATCH api/User/{id}/block`

- **Description:** Toggles the block status of a user.

  - **Authorization:** Requires a valid authentication token with "Admin" role.

  - **Response:**
    - **Success (200 OK):** Returns the updated block status.
      ```json
      {
        "IsBlocked": true
      }
      ```

### `PATCH api/User/{id}/change-password`

- **Description:** Changes the password of a user.

  - **Authorization:** Requires a valid authentication token.

  - **Request Body:**
    ```json
    {
      "currentPassword": "old_password",
      "newPassword": "new_password"
    }
    ```

  - **Response:**
    - **Success (200 OK):** Password changed successfully.

  - **Error (4xx/5xx):** Returns appropriate error responses for invalid requests or server errors.


### Key API

#### `GET api/Key`

- **Description:** Retrieves a list of all keys.

  - **Response:**
    - **Success (200 OK):** Returns a list of key details.
      ```json
      [
        {
          "keyID": 1,
          "keyValue": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
          "startDate": "2024-02-16T06:45:52.705114Z",
          "expiryDate": "2025-02-16T06:45:52.705114Z",
          "maxMachines": 5
        },
        // ... other keys
      ]
      ```

## Key API

### `GET api/Key`

- **Description:** Retrieves a list of all keys.

  - **Authorization:** Requires a valid authentication token with "Admin" role.

  - **Response:**
    - **Success (200 OK):** Returns a list of key details.
      ```json
      [
        {
          "keyID": 1,
          "keyValue": "unique_key_value",
          "startDate": "2024-02-19T12:00:00Z",
          "expiryDate": "2025-02-19T12:00:00Z",
          "maxMachines": 5
        },
        // ... other keys
      ]
      ```

### `GET api/Key/{id}`

- **Description:** Retrieves details of a specific key by ID.

  - **Authorization:** Requires a valid authentication token.

  - **Response:**
    - **Success (200 OK):** Returns the key details.
      ```json
      {
        "keyID": 1,
        "keyValue": "unique_key_value",
        "startDate": "2024-02-19T12:00:00Z",
        "expiryDate": "2025-02-19T12:00:00Z",
        "maxMachines": 5
      }
      ```

### `POST api/Key`

- **Description:** Creates a new key.

  - **Authorization:** Requires a valid authentication token with "Admin" role.

  - **Request Body:**
    ```json
    {
      "maxMachines": 5
    }
    ```

  - **Response:**
    - **Success (201 Created):** Returns the created key details.
      ```json
      {
        "keyID": 2,
        "keyValue": "new_unique_key",
        "startDate": "2024-02-19T12:00:00Z",
        "expiryDate": "2025-02-19T12:00:00Z",
        "maxMachines": 5
      }
      ```

### `PUT api/Key/{id}`

- **Description:** Updates key information.

  - **Authorization:** Requires a valid authentication token with "Admin" role.

  - **Request Body:**
    ```json
    {
      "keyValue": "updated_key_value",
      "startDate": "2024-02-19T12:00:00Z",
      "expiryDate": "2025-02-19T12:00:00Z",
      "maxMachines": 10
    }
    ```

  - **Response:**
    - **Success (204 No Content):** Key information updated successfully.

### `DELETE api/Key/{id}`

- **Description:** Deletes a key by ID.

  - **Authorization:** Requires a valid authentication token with "Admin" role.

  - **Response:**
    - **Success (200 OK):** Key deleted successfully.

### `POST api/Key/change-key-value/{id}`

- **Description:** Changes the value of a key.

  - **Authorization:** Requires a valid authentication token with "Admin" role.

  - **Response:**
    - **Success (200 OK):** Returns the updated key details.
      ```json
      {
        "keyID": 1,
        "keyValue": "new_unique_key",
        "startDate": "2024-02-19T12:00:00Z",
        "expiryDate": "2025-02-19T12:00:00Z",
        "maxMachines": 5
      }
      ```

## Key Extension API

### `GET api/KeyExtension/{id}`

- **Description:** Retrieves details of a specific key extension by ID.

  - **Authorization:** Requires a valid authentication token with "Admin" role.

  - **Response:**
    - **Success (200 OK):** Returns the key extension details.
      ```json
      {
        "extensionID": 1,
        "keyID": 1,
        "extensionDate": "2024-02-19T12:00:00Z",
        "newExpiryDate": "2025-02-19T12:00:00Z"
      }
      ```

### `POST api/KeyExtension`

- **Description:** Creates a new key extension.

  - **Authorization:** Requires a valid authentication token with "Admin" role.

  - **Request Body:**
    ```json
    {
      "keyID": 1
    }
    ```

  - **Response:**
    - **Success (201 Created):** Returns the created key extension details.
      ```json
      {
        "extensionID": 2,
        "keyID": 1,
        "extensionDate": "2024-02-19T12:00:00Z",
        "newExpiryDate": "2026-02-19T12:00:00Z"
      }
      ```

### `GET api/KeyExtension/check-expiration/{keyId}`

- **Description:** Checks if a key or its extension is expired.

  - **Authorization:** Requires a valid authentication token.

  - **Response:**
    - **Success (200 OK):** Returns expiration status and date.
      ```json
      {
        "Expired": false,
        "ExpiryDate": "2026-02-19T12:00:00Z"
      }
      ```

### `GET api/KeyExtension/check-expiration-by-value/{keyValue}`

- **Description:** Checks if a key or its extension is expired using key value.

  - **Authorization:** Requires a valid authentication token.

  - **Response:**
    - **Success (200 OK):** Returns expiration status and date.
      ```json
      {
        "Expired": false,
        "ExpiryDate": "2026-02-19T12:00:00Z"
      }
      ```


## User Key API

### `GET api/UserKey/{id}`

- **Description:** Retrieves details of a specific user key by ID.

  - **Authorization:** Requires a valid authentication token.

  - **Response:**
    - **Success (200 OK):** Returns the user key details.
      ```json
      {
        "userKeyID": 1,
        "userID": 1,
        "keyID": 1,
        "machinesUsed": 3,
        "key": {
          "keyID": 1,
          "keyValue": "unique-key-value",
          "startDate": "2023-02-19T12:00:00Z",
          "expiryDate": "2024-02-19T12:00:00Z",
          "maxMachines": 5
        }
      }
      ```

### `GET api/UserKey/key-info/{keyId}`

- **Description:** Retrieves information about a specific key, including usage statistics.

  - **Authorization:** Requires a valid authentication token.

  - **Response:**
    - **Success (200 OK):** Returns key information.
      ```json
      {
        "maxMachines": 5,
        "usedMachines": 3,
        "leftMachines": 2,
        "userIDs": [1, 2, 3]
      }
      ```

### `POST api/UserKey`

- **Description:** Creates a new user key.

  - **Authorization:** Requires a valid authentication token.

  - **Request Body:**
    ```json
    {
      "userID": 1,
      "keyValue": "unique-key-value"
    }
    ```

  - **Response:**
    - **Success (201 Created):** Returns the created user key details.
      ```json
      {
        "userKeyID": 2,
        "userID": 1,
        "keyID": 1,
        "machinesUsed": 1
      }
      ```

---
## Contact and Contributors

For inquiries, support, or contributions, feel free to reach out to:

- Leader Content here - Project Lead
- [Anteneh Solomon](antenehcs@gmail.co  ) - Backend - Developer +251943291709
- 
### Important Links
-
-

## License



---

Thank you for using Session Management Backend! We appreciate your support.
