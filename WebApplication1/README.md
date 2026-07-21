# Session 7 - Firebase Authentication, Authorization and MinIO Storage

## Overview

This session extends the existing Warehouse Management API by adding:

- Firebase Authentication
- Role-based Authorization (Admin/User)
- MinIO Object Storage
- File metadata persistence in PostgreSQL

The project continues using the existing layered architecture (Presentation, Application, Domain, Infrastructure).

---

## Firebase Setup

1. Create a Firebase project from the Firebase Console.
2. Enable **Email/Password** authentication.
3. Create at least two users:
  - Admin user
  - Normal user
4. Generate a Firebase Service Account key:
  - Firebase Console → Project Settings → Service Accounts → Generate new private key
5. Place the downloaded `serviceAccount.json` file in:

```
Warehouse.Infrastructure/Firebase/
```

> **Important:** Do not commit the service account file to Git.

---

## Firebase Configuration

Add the Firebase Project ID to your configuration.

Example:

```json
"Firebase": {
  "ProjectId": "warehouse-api-5f159"
}
```

The API validates Firebase ID tokens using:

- Issuer: `https://securetoken.google.com/{ProjectId}`
- Audience: `{ProjectId}`

---

## Authorization

The API uses Firebase Custom Claims for authorization.

Supported roles:

- admin
- user

### Admin Permissions

- Create products
- Update products
- Delete products
- Upload files
- Manage warehouse resources

### User Permissions

- View products
- View suppliers
- View stock information
- View dashboard

Requests without a valid token return:

```
401 Unauthorized
```

Authenticated users without sufficient permissions receive:

```
403 Forbidden
```

---

## MinIO Setup

MinIO is used as the object storage provider for warehouse files.

Run MinIO using Docker Compose from the solution root:

```bash
docker compose up -d
```

MinIO API:

```
http://localhost:9000
```

MinIO Console:

```
http://localhost:9001
```

Login credentials:

```
Username: admin
Password: password123
```

Create the following bucket:

```
warehouse-assets
```

---

## MinIO Configuration

Configure MinIO in `appsettings.json`:

```json
"Minio": {
  "Endpoint": "localhost:9000",
  "AccessKey": "admin",
  "SecretKey": "password123",
  "BucketName": "warehouse-assets",
  "UseSSL": false
}
```

---

## File Storage

Warehouse files are stored in MinIO.

The SQL database stores only metadata:

- File Name
- Object Key
- Content Type
- File Size
- Related Entity ID
- Upload Date

File bytes are **not** stored in PostgreSQL.

---

## Test Accounts

Example accounts used during testing:

| Role | Email |
|------|-------|
| Admin | admin@test.com |
| User | user@test.com |

Passwords are omitted.

---

## Obtaining a Firebase ID Token

1. Sign in using Firebase Authentication.
2. Copy the generated Firebase ID token.
3. Use the token in API requests.

Postman:

```
Authorization
→ Bearer Token
→ Paste Firebase ID Token
```

If custom claims (roles) are changed, sign in again to obtain a new token containing the updated role.

---

## API Testing

### No Token

Expected response:

```
401 Unauthorized
```

### User Token

Allowed:

- GET Products
- GET Suppliers
- GET Dashboard
- GET Stock Information

Restricted:

- POST
- PUT
- DELETE
- File Upload

Expected response:

```
403 Forbidden
```

### Admin Token

Allowed:

- Create Products
- Update Products
- Delete Products
- Upload Product Images
- Upload Supplier Documents

---

## File Upload

Example endpoint:

```
POST /api/products/{id}/image
```

Request type:

```
multipart/form-data
```

Required field:

```
file
```

After a successful upload:

- The file is stored in the `warehouse-assets` bucket.
- Only file metadata is saved in PostgreSQL.
- A unique object key is generated to prevent overwriting existing files.