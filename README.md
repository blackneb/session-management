# Session Management Backend

## TechStack: ASP.NET Core Web API

This backend system is designed for session management, user authentication, and key management using ASP.NET Core Web API.

## List of API
| Method | URL | Description | Role |
| ------ | --- | ----------- | ---- |
| Auth API |
| POST | `api/Auth/RegisterAdmin` | Admin Registration |  |
| POST | `api/Auth/RegisterUser` | User Registration |  |
| POST | `api/Auth/Login` | Log in  |  |
| User API |  
| GET | `api/User` | Get all user list |  |
| GET | `api/User/id` | Get Specific user by ID |  |
| PUT | `api/user/id` | Update user information |  |
| DELETE | `api/user/id` | Delete user by id |  |
| PATCH | `api/user/id/block` | Block user by id |  |
| PATCH | `api/user/id/change-password` | Change user password |  |
| Key API |
| GET | `api/key` | Get all key list |  |
| POST | `api/key` | Create a new key |  |
| GET | `api/key/id` | Get a key by id |  |
| DELETE | `api/key/id` | Delete a key by id |  |
| POST | `api/key/change-key-value/id` | change key value of existing key |  |
| Key Extension API |
| POST | `api/KeyExtension` | Extend the expire date by one year |  |
| GET | `api/KeyExtension/check-expiration/id` | Check the expiry date of a key by id |  |
| GET | `api/KeyExtension/check-expiration-by-value/keyvalue` | Check the expiry date of a key by it's key value |  |
| User Key API  |
| GET | `api/UserKey/id` | Check userKey ID  |  |
| GET | `api/UserKey/key-info/id` | Check key information |  |
| POST | `api/UserKey` | Register a key for a user |  |

### Auth API

#### `POST api/Auth/RegisterAdmin`
- **Description:** Allows the registration of an admin user.
- **Response:**
  - **Success (201 Created):** Admin registration successful. Returns the newly created admin user details.
    ```json
    {
      "userId": 1,
      "username": "admin",
      "role": "admin",
      "token": "generated_token"
    }
    ```
  - **Error (4xx):** If registration fails due to validation errors or duplicate entries, an appropriate error response is returned.
    ```json
    {
      "error": "Registration failed. Username already exists."
    }
    ```

#### `POST api/Auth/RegisterUser`
- **Description:** Allows the registration of a regular user.
- **Response:**
  - **Success (201 Created):** User registration successful. Returns the newly created user details.
    ```json
    {
      "userId": 2,
      "username": "user123",
      "role": "user",
      "token": "generated_token"
    }
    ```
  - **Error (4xx):** If registration fails due to validation errors or duplicate entries, an appropriate error response is returned.
    ```json
    {
      "error": "Invalid email format."
    }
    ```

#### `POST api/Auth/Login`
- **Description:** Handles user login.
- **Response:**
  - **Success (200 OK):** User authentication successful. Returns an authentication token.
    ```json
    {
      "userId": 2,
      "username": "user123",
      "role": "user",
      "token": "generated_token"
    }
    ```
  - **Error (4xx):** If login fails due to incorrect credentials or other issues, an appropriate error response is returned.
    ```json
    {
      "error": "Invalid credentials. Please try again."
    }
    ```

### User API

#### `GET api/User`
- **Description:** Retrieves a list of all users.
- **Response:**
  - **Success (200 OK):** Returns a list of user details.
    ```json
    [
      {
        "userId": 1,
        "username": "admin",
        "role": "admin"
      },
      {
        "userId": 2,
        "username": "user123",
        "role": "user"
      }
    ]
    ```
  - **Error (4xx):** If the request fails, an appropriate error response is returned.
    ```json
    {
      "error": "Unauthorized. Please provide a valid token."
    }
    ```

#### `GET api/User/id`
- **Description:** Retrieves details of a specific user based on ID.
- **Response:**
  - **Success (200 OK):** Returns the details of the specified user.
    ```json
    {
      "userId": 2,
      "username": "user123",
      "role": "user"
    }
    ```
  - **Error (4xx):** If the specified user ID is not found or if the request fails, an appropriate error response is returned.
    ```json
    {
      "error": "User not found."
    }
    ```

#### `PUT api/user/id`
- **Description:** Updates user information.
- **Response:**
  - **Success (200 OK):** Returns the updated user details.
    ```json
    {
      "userId": 2,
      "username": "user123_updated",
      "role": "user"
    }
    ```
  - **Error (4xx):** If the update fails due to validation errors or other issues, an appropriate error response is returned.
    ```json
    {
      "error": "Invalid data provided for update."
    }
    ```

#### `DELETE api/user/id`
- **Description:** Deletes a user based on ID.
- **Response:**
  - **Success (204 No Content):** User deletion successful.
  - **Error (4xx):** If the specified user ID is not found or if the deletion fails, an appropriate error response is returned.
    ```json
    {
      "error": "User not found for deletion."
    }
    ```

#### `PATCH api/user/id/block`
- **Description:** Blocks a user based on ID.
- **Response:**
  - **Success (200 OK):** Returns details after blocking the user.
    ```json
    {
      "userId": 2,
      "username": "user123",
      "role": "user",
      "status": "blocked"
    }
    ```
  - **Error (4xx):** If the specified user ID is not found or if blocking fails, an appropriate error response is returned.
    ```json
    {
      "error": "User not found for blocking."
    }
    ```

#### `PATCH api/user/id/change-password`
- **Description:** Changes the password of a user based on ID.
- **Response:**
  - **Success (200 OK):** Returns details after changing the password.
    ```json
    {
      "userId": 2,
      "username": "user123",
      "role": "user",
      "status": "active"
    }
    ```
  - **Error (4xx):** If the specified user ID is not found or if changing the password fails, an appropriate error response is returned.
    ```json
    {
      "error": "User not found for changing password."
    }
    ```
### Key API

#### `GET api/key`
- **Description:** Retrieves a list of all keys.
- **Response:**
  - **Success (200 OK):** Returns a list of key details.
    ```json
    [
      {
        "keyId": 1,
        "keyValue": "key1",
        "description": "Sample key 1"
      },
      {
        "keyId": 2,
        "keyValue": "key2",
        "description": "Sample key 2"
      }
    ]
    ```
  - **Error (4xx):** If the request fails, an appropriate error response is returned.
    ```json
    {
      "error": "Unauthorized. Please provide a valid token."
    }
    ```

#### `POST api/key`
- **Description:** Creates a new key.
- **Response:**
  - **Success (201 Created):** Returns details of the newly created key.
    ```json
    {
      "keyId": 3,
      "keyValue": "key3",
      "description": "Sample key 3"
    }
    ```
  - **Error (4xx):** If key creation fails due to validation errors or other issues, an appropriate error response is returned.
    ```json
    {
      "error": "Invalid key data provided for creation."
    }
    ```

#### `GET api/key/id`
- **Description:** Retrieves details of a specific key based on ID.
- **Response:**
  - **Success (200 OK):** Returns the details of the specified key.
    ```json
    {
      "keyId": 2,
      "keyValue": "key2",
      "description": "Sample key 2"
    }
    ```
  - **Error (4xx):** If the specified key ID is not found or if the request fails, an appropriate error response is returned.
    ```json
    {
      "error": "Key not found."
    }
    ```

#### `DELETE api/key/id`
- **Description:** Deletes a key based on ID.
- **Response:**
  - **Success (204 No Content):** Key deletion successful.
  - **Error (4xx):** If the specified key ID is not found or if the deletion fails, an appropriate error response is returned.
    ```json
    {
      "error": "Key not found for deletion."
    }
    ```

#### `POST api/key/change-key-value/id`
- **Description:** Changes the value of an existing key based on ID.
- **Response:**
  - **Success (200 OK):** Returns details after changing the key value.
    ```json
    {
      "keyId": 2,
      "keyValue": "key2_updated",
      "description": "Sample key 2"
    }
    ```
  - **Error (4xx):** If the specified key ID is not found or if changing the key value fails, an appropriate error response is returned.
    ```json
    {
      "error": "Key not found for changing value."
    }
    ```

### Key Extension API

#### `POST api/KeyExtension`
- **Description:** Extends the expiration date of a key by one year.
- **Response:**
  - **Success (200 OK):** Returns details after extending the key expiration date.
    ```json
    {
      "keyId": 2,
      "keyValue": "key2",
      "description": "Sample key 2",
      "expirationDate": "2025-02-16T00:00:00Z"
    }
    ```
  - **Error (4xx):** If the specified key ID is not found or if extension fails, an appropriate error response is returned.
    ```json
    {
      "error": "Key not found for extension."
    }
    ```

#### `GET api/KeyExtension/check-expiration/id`
- **Description:** Checks the expiration date of a key based on ID.
- **Response:**
  - **Success (200 OK):** Returns the expiration date details of the specified key.
    ```json
    {
      "keyId": 2,
      "keyValue": "key2",
      "description": "Sample key 2",
      "expirationDate": "2025-02-16T00:00:00Z"
    }
    ```
  - **Error (4xx):** If the specified key ID is not found or if the request fails, an appropriate error response is returned.
    ```json
    {
      "error": "Key not found for expiration check."
    }
    ```

#### `GET api/KeyExtension/check-expiration-by-value/keyvalue`
- **Description:** Checks the expiration date of a key based on its key value.
- **Response:**
  - **Success (200 OK):** Returns the expiration date details of the specified key.
    ```json
    {
      "keyId": 2,
      "keyValue": "key2",
      "description": "Sample key 2",
      "expirationDate": "2025-02-16T00:00:00Z"
    }
    ```
  - **Error (4xx):** If the specified key value is not found or if the request fails, an appropriate error response is returned.
    ```json
    {
      "error": "Key value not found for expiration check."
    }
    ```
### User Key API

#### `GET api/UserKey/id`
- **Description:** Checks userKey ID.
- **Response:**
  - **Success (200 OK):** Returns userKey ID details.
    ```json
    {
      "userKeyId": 1,
      "userId": 2,
      "keyId": 2,
      "expirationDate": "2025-02-16T00:00:00Z"
    }
    ```
  - **Error (4xx):** If the specified userKey ID is not found or if the request fails, an appropriate error response is returned.
    ```json
    {
      "error": "UserKey ID not found."
    }
    ```

#### `GET api/UserKey/key-info/id`
- **Description:** Checks key information associated with a userKey ID.
- **Response:**
  - **Success (200 OK):** Returns details of the key associated with the specified userKey ID.
    ```json
    {
      "keyId": 2,
      "keyValue": "key2",
      "description": "Sample key 2",
      "expirationDate": "2025-02-16T00:00:00Z"
    }
    ```
  - **Error (4xx):** If the specified userKey ID is not found or if the request fails, an appropriate error response is returned.
    ```json
    {
      "error": "UserKey ID not found for key information."
    }
    ```

#### `POST api/UserKey`
- **Description:** Registers a key for a user.
- **Response:**
  - **Success (200 OK):** Returns details after registering the key for the user.
    ```json
    {
      "userKeyId": 3,
      "userId": 2,
      "keyId": 3,
      "expirationDate": "2025-02-16T00:00:00Z"
    }
    ```
  - **Error (4xx):** If the specified user ID or key ID is not found or if registration fails, an appropriate error response is returned.
    ```json
    {
      "error": "User or Key not found for registration."
    }
    ```
