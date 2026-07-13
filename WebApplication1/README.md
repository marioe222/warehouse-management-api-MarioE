# Warehouse Management API - Session 03 Refactor

## Overview

This session focuses on refactoring the Warehouse Management API using DDD architecture, Repository Pattern, Dependency Injection, and CQRS-style use cases.

The goal was to improve the project structure by separating business logic, application logic, data access, and API responsibilities while keeping the existing API behavior.

---

# Architecture

The project is divided into four layers:

- Warehouse.Domain
- Warehouse.Application
- Warehouse.Infrastructure
- Warehouse.Presentation

---

# Layer Responsibilities

## Warehouse.Domain

Responsible for core business logic.

Contains:

- Entities
- Business rules
- Repository interfaces

Examples:

- Product
- Supplier
- StockMovement
- WarehouseItem
- ProductImage


Business rules:

- Product name is required
- SKU is required
- Price must be greater than zero
- Quantity cannot be negative
- Archived products cannot be updated
- Inactive suppliers cannot be assigned


---

## Warehouse.Application

Responsible for application use cases.

Contains:

- Commands
- Queries
- Handlers

Implemented use cases:

### Product Commands

- CreateProduct
- UpdateProductQuantity
- UpdateProductPrice
- ArchiveProduct
- AssignSupplierToProduct


### Product Queries

- GetProductById
- ListProducts
- SearchProducts


### Supplier Commands

- CreateSupplier
- DeactivateSupplier


### Supplier Queries

- GetSupplierById
- ListSuppliers


MediatR is used to separate requests and handlers.

---

## Warehouse.Infrastructure

Responsible for technical implementation.

Contains:

- Repository implementations
- Data access logic

Repositories:

- ProductRepository
- SupplierRepository


The Application and Domain layers do not know how data is stored.

---

## Warehouse.Presentation

Responsible for HTTP communication.

Contains:

- Controllers
- API endpoints

Controllers only:

- Receive HTTP requests
- Call application use cases
- Return HTTP responses

Business rules and storage access were removed from controllers.

---

# Refactored Endpoints

## Products

| Method | Endpoint |
|---|---|
| GET | `/api/products` |
| GET | `/api/products/{id}` |
| GET | `/api/products/search` |
| POST | `/api/products` |
| POST | `/api/products/{id}/quantity` |
| POST | `/api/products/{id}/price` |
| POST | `/api/products/{id}/image` |
| DELETE | `/api/products/{id}` |
| GET | `/api/products/server-time` |
| POST | `/api/products/{id}/assign-supplier/{supplierId}` |


## Suppliers

| Method | Endpoint |
|---|---|
| GET | `/api/suppliers` |
| GET | `/api/suppliers/{id}` |
| POST | `/api/suppliers` |
| DELETE | `/api/suppliers/{id}` |

---

# Swagger Screenshots

The API endpoints were tested successfully using Swagger UI.

## Swagger API Documentation

![Swagger API](screenshots/Swagger-Api.png)


## Get Products Endpoint

![Get Products](screenshots/Swagger-GetProducts.png)


## Get Products Response

![Get Products Response](screenshots/Swagger-GetProductsResponse.png)


## Create Product Endpoint

![Create Product](screenshots/Swagger-PostProducts.png)


## Create Product Response

![Create Product Response](screenshots/Swagger-PostProductsResponse.png)

---

# Test Results

Successfully tested:

✅ Create Product  
✅ Get All Products  
✅ Get Product By Id  
✅ Search Products  
✅ Update Product Quantity  
✅ Update Product Price  
✅ Archive Product  
✅ Create Supplier  
✅ Get Suppliers  
✅ Get Supplier By Id  
✅ Assign Supplier To Product


Status codes tested:

- 201 Created
- 200 OK
- 204 No Content
- 400 Bad Request
- 404 Not Found

---

# Session 03 Result

The Warehouse Management API was successfully refactored using:

- Domain-Driven Design (DDD)
- Repository Pattern
- Dependency Injection
- CQRS-style separation
- MediatR request handling

The project now has a cleaner architecture with separated responsibilities between Domain, Application, Infrastructure, and Presentation layers.

---

# Session 05 Improvements

This session focuses on improving API reliability by adding middleware, filters, validation handling, exception handling, and additional dashboard and metadata endpoints.

---

## Middleware vs Filters

### Middleware

Middleware handles HTTP-level concerns and runs in the ASP.NET Core request pipeline.

Implemented middleware:

- CorrelationIdMiddleware
    - Adds a unique correlation ID to each request.

- RequestTimingMiddleware
    - Measures and logs request execution time.

- ExceptionHandlingMiddleware
    - Handles unexpected exceptions and returns a consistent error response.

### Filters

Filters are MVC/action-level components that execute around controller actions.

Implemented filters:

- ValidationFilter
    - Handles validation errors and returns HTTP 400 Bad Request.

- ActionLoggingFilter
    - Logs controller action execution.

## Metadata Endpoint

Added reflection-based validation metadata inspection.

Endpoint:

GET /api/metadata/validation/{dtoName}

Example:

GET /api/metadata/validation/CreateProductRequest

The endpoint returns DTO properties and validation attributes.


## Inventory Dashboard

Endpoint:

GET /api/inventory/dashboard

Returns:

- Total products
- Available products
- Low stock products
- Total suppliers
- Active suppliers

## Stock Adjustment

Endpoint:

POST /api/stock-adjustments

Features:

- Creates stock adjustment records.
- Uses CQRS with MediatR.
- Uses repository pattern.
- Validates request using FluentValidation.

# Test Results

Successfully tested:

✅ Create Product  
✅ Get All Products  
✅ Get Product By Id  
✅ Search Products  
✅ Update Product Quantity  
✅ Update Product Price  
✅ Archive Product  
✅ Create Supplier  
✅ Get Suppliers  
✅ Assign Supplier To Product  
✅ Create Stock Adjustment  
✅ Inventory Dashboard  
✅ Metadata Validation Endpoint


Status codes tested:

- 201 Created
- 200 OK
- 204 No Content
- 400 Bad Request
- 404 Not Found
- 500 handled by exception middleware

## Swagger Screenshots

### Screenshot 3280

![Screenshot 3280](screenshots/Screenshot%20(3280).png)

### Screenshot 3281

![Screenshot 3281](screenshots/Screenshot%20(3281).png)

### Screenshot 3282

![Screenshot 3282](screenshots/Screenshot%20(3282).png)

### Screenshot 3283

![Screenshot 3283](screenshots/Screenshot%20(3283).png)

### Screenshot 3284

![Screenshot 3284](screenshots/Screenshot%20(3284).png)

### Screenshot 3285

![Screenshot 3285](screenshots/Screenshot%20(3285).png)

### Screenshot 3286

![Screenshot 3286](screenshots/Screenshot%20(3286).png)

### Screenshot 3288

![Screenshot 3288](screenshots/Screenshot%20(3288).png)

### Screenshot 3292

![Screenshot 3292](screenshots/Screenshot%20(3292).png)

### Screenshot 3293

![Screenshot 3293](screenshots/Screenshot%20(3293).png)

### Screenshot 3294

![Screenshot 3294](screenshots/Screenshot%20(3294).png)

### Screenshot 3295

![Screenshot 3295](screenshots/Screenshot%20(3295).png)


### Screenshot 3297

![Screenshot 3297](screenshots/Screenshot%20(3297).png)

### Screenshot 3299

![Screenshot 3299](screenshots/Screenshot%20(3299).png)