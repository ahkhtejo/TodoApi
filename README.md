# ToDos API

A RESTful Task Management API built with **ASP.NET Core**, **Entity Framework Core**, and **SQLite**.

The API provides functionality for managing:

* Users
* Tasks
* Comments
* Task statuses
* Task priorities
* User types
* API health checks

---

## 📋 Table of Contents

* [Overview](#overview)
* [Technology Stack](#technology-stack)
* [High-Level Design](#high-level-design)
* [Architecture](#architecture)
* [Project Structure](#project-structure)
* [Getting Started](#getting-started)
* [Configuration](#configuration)
* [Database](#database)
* [Running the API](#running-the-api)
* [API Documentation](#api-documentation)
* [Users API](#users-api)
* [Tasks API](#tasks-api)
* [Comments API](#comments-api)
* [Status API](#status-api)
* [Configuration API](#configuration-api)
* [Health API](#health-api)
* [Data Model](#data-model)
* [Relationships](#relationships)
* [Pagination](#pagination)
* [Error Handling](#error-handling)
* [Development Guidelines](#development-guidelines)
* [Future Improvements](#future-improvements)

---

# Overview

The **ToDos API** is a backend service for managing tasks and their related users, comments, and statuses.

The API follows a REST-style architecture and exposes HTTP endpoints that can be consumed by:

* Web applications
* Mobile applications
* Other backend services
* Desktop applications
* API testing tools

The default development URL is:

```text
http://localhost:5037
```

---

# Technology Stack

| Technology            | Purpose                       |
| --------------------- | ----------------------------- |
| C#                    | Programming language          |
| ASP.NET Core          | Web API framework             |
| Entity Framework Core | ORM / database access         |
| SQLite                | Database                      |
| OpenAPI 3.1.1         | API specification             |
| Scalar                | API documentation and testing |
| LINQ                  | Data querying and projection  |
| Dependency Injection  | Service management            |
| Git                   | Source control                |

---

# High-Level Design

## HLD Overview

The application follows a **layered architecture** where each layer has a specific responsibility.

```text
                         ┌─────────────────────────┐
                         │        API Client       │
                         │                         │
                         │ Web / Mobile / Postman  │
                         │ Other Applications      │
                         └────────────┬────────────┘
                                      │
                                      │ HTTP / JSON
                                      ▼
                         ┌─────────────────────────┐
                         │      API Layer          │
                         │                         │
                         │     Controllers         │
                         │                         │
                         │ UserController           │
                         │ TaskController           │
                         │ CommentController        │
                         │ StatusController         │
                         │ ConfigController         │
                         │ HealthController         │
                         └────────────┬────────────┘
                                      │
                                      ▼
                         ┌─────────────────────────┐
                         │    Business Layer       │
                         │                         │
                         │       Services          │
                         │                         │
                         │ UserService              │
                         │ TaskService              │
                         │ CommentService           │
                         │ StatusService            │
                         └────────────┬────────────┘
                                      │
                                      ▼
                         ┌─────────────────────────┐
                         │   Data Access Layer     │
                         │                         │
                         │   Entity Framework Core │
                         │       DbContext         │
                         └────────────┬────────────┘
                                      │
                                      │ SQL
                                      ▼
                         ┌─────────────────────────┐
                         │         SQLite          │
                         │                         │
                         │       ToDos.db          │
                         └─────────────────────────┘
```

---

## HLD Components

### 1. Client Layer

The client layer represents applications consuming the API.

Examples:

```text
Web Application
Mobile Application
Postman
Scalar
Other Backend Services
```

Clients communicate with the API using HTTP requests.

---

### 2. API Layer

The API layer contains the ASP.NET Core controllers.

Responsibilities include:

* Receiving HTTP requests
* Validating request parameters
* Calling the appropriate service
* Returning HTTP responses
* Mapping HTTP status codes

Example:

```text
POST /Task
       │
       ▼
TaskController
       │
       ▼
TaskService
```

Controllers should not contain complex business logic.

---

### 3. Business Layer

The service layer contains the application's business logic.

Example:

```text
ITaskService
     │
     ▼
TaskService
```

Responsibilities include:

* Creating tasks
* Updating tasks
* Retrieving tasks
* Deleting tasks
* Validating business rules
* Mapping entities to DTOs

---

### 4. Data Access Layer

Entity Framework Core is responsible for communication with SQLite.

```text
Service
   │
   ▼
DbContext
   │
   ▼
Entity Framework Core
   │
   ▼
SQLite
```

Database access should be performed asynchronously whenever possible.

Example:

```csharp
var tasks = await _dbContext.Tasks
    .ToListAsync();
```

---

### 5. Database

The application uses **SQLite** as its database.

The database is stored locally as a file:

```text
ToDos.db
```

SQLite is suitable for this project because it:

* Requires no separate database server
* Is lightweight
* Is easy to configure
* Is easy to use during development
* Is portable
* Works well with Entity Framework Core

---

# Architecture

The application follows a layered architecture.

```text
HTTP Request
     │
     ▼
┌───────────────────┐
│    Controller     │
└─────────┬─────────┘
          │
          ▼
┌───────────────────┐
│     Service       │
└─────────┬─────────┘
          │
          ▼
┌───────────────────┐
│    DbContext      │
└─────────┬─────────┘
          │
          ▼
┌───────────────────┐
│      SQLite       │
└───────────────────┘
          │
          ▼
      Database
```

The response follows the reverse path:

```text
Database
   │
   ▼
DbContext
   │
   ▼
Service
   │
   ▼
DTO
   │
   ▼
Controller
   │
   ▼
HTTP Response
```

---

# Project Structure

```text
ToDos/
│
├── Controllers/
│   ├── CommentController.cs
│   ├── ConfigController.cs
│   ├── HealthController.cs
│   ├── StatusController.cs
│   ├── TaskController.cs
│   └── UserController.cs
│
├── Data/
│   └── AppDbContext.cs
│
├── Models/
│   ├── User.cs
│   ├── TodoTask.cs
│   ├── Comment.cs
│   └── Status.cs
│
├── DTOs/
│   ├── User/
│   ├── Task/
│   ├── Comment/
│   └── Status/
│
├── Services/
│   ├── Interfaces/
│   │   ├── IUserService.cs
│   │   ├── ITaskService.cs
│   │   ├── ICommentService.cs
│   │   └── IStatusService.cs
│   │
│   ├── UserService.cs
│   ├── TaskService.cs
│   ├── CommentService.cs
│   └── StatusService.cs
│
├── Mappings/
│   └── ...
│
├── Migrations/
│   └── ...
│
├── Properties/
│
├── appsettings.json
├── Program.cs
├── ToDos.db
└── README.md
```

---

# Getting Started

## Prerequisites

Install:

* .NET SDK
* Git
* Scalar or Postman

Check the installed .NET version:

```bash
dotnet --version
```

No SQL Server installation is required because the application uses SQLite.

---

# Clone the Repository

```bash
git clone <repository-url>
cd ToDos
```

Restore NuGet packages:

```bash
dotnet restore
```

Build the project:

```bash
dotnet build
```

---

# Database

The application uses **SQLite with Entity Framework Core**.

The database is stored locally:

```text
ToDos.db
```

Example connection string:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=ToDos.db"
  }
}
```

---

## Entity Framework Core SQLite Package

The project should use the SQLite EF Core provider:

```bash
dotnet add package Microsoft.EntityFrameworkCore.Sqlite
```

For migrations:

```bash
dotnet add package Microsoft.EntityFrameworkCore.Design
```

---

# Database Migration

Create a migration:

```bash
dotnet ef migrations add InitialCreate
```

Apply the migration:

```bash
dotnet ef database update
```

After the migration is applied, the SQLite database will contain the required tables.

---

# Running the API

Run the application:

```bash
dotnet run
```

The API will be available at:

```text
http://localhost:5037
```

---

# API Documentation

The API uses **OpenAPI 3.1.1**.

API information:

```text
Title: ToDos | v1
Version: 1.0.0
OpenAPI Version: 3.1.1
Base URL: http://localhost:5037
```

If Scalar is configured, open:

```text
http://localhost:5037/scalar/
```

Scalar provides an interactive interface where you can:

1. Browse API endpoints
2. Enter request parameters
3. Provide JSON request bodies
4. Execute requests
5. View API responses

---

# Users API

Base route:

```text
/User
```

## Get All Users

```http
GET /User
```

Example:

```bash
curl http://localhost:5037/User
```

---

## Create User

```http
POST /User
Content-Type: application/json
```

Example:

```json
{
  "name": "Ahmed",
  "birthDate": "1995-01-01T00:00:00Z",
  "email": "ahmed@example.com",
  "userType": 0
}
```

### User Type

```text
0 = Male
1 = Female
2 = Other
```

---

## Get User By ID

```http
GET /User/{userId}
```

Example:

```http
GET /User/1
```

---

## Delete User

```http
DELETE /User/{userId}
```

Example:

```http
DELETE /User/1
```

---

# Tasks API

Base route:

```text
/Task
```

## Get All Tasks

```http
GET /Task
```

Optional pagination:

```http
GET /Task?pageNumber=1&pageSize=10
```

---

## Create Task

```http
POST /Task
Content-Type: application/json
```

Example:

```json
{
  "name": "Learn ASP.NET Core",
  "description": "Study ASP.NET Core Web API",
  "startDate": "2026-09-10T08:00:00Z",
  "endDate": "2026-09-15T17:00:00Z",
  "priority": 4,
  "userId": 1,
  "statusId": 1
}
```

### Priority

```text
1 = Low
2 = Medium
3 = Normal
4 = High
```

---

## Update Task

```http
PUT /Task
Content-Type: application/json
```

Example:

```json
{
  "id": 1,
  "name": "Learn ASP.NET Core",
  "description": "Complete the Web API learning path",
  "startDate": "2026-09-10T08:00:00Z",
  "endDate": "2026-09-20T17:00:00Z",
  "priority": 4,
  "userId": 1,
  "statusId": 2
}
```

---

## Get Task By ID

```http
GET /Task/{TaskId}
```

Example:

```http
GET /Task/1
```

---

## Delete Task

```http
DELETE /Task/{TaskId}
```

Example:

```http
DELETE /Task/1
```

---

## Get Tasks By User

```http
GET /Task/api/{UserID}
```

Example:

```http
GET /Task/api/1
```

---

# Comments API

Base route:

```text
/Comment
```

## Get All Comments

```http
GET /Comment
```

Optional pagination:

```http
GET /Comment?pageNumber=1&pageSize=10
```

---

## Create Comment

```http
POST /Comment
Content-Type: application/json
```

Example:

```json
{
  "body": "This task needs to be completed before Friday.",
  "userId": 1,
  "todoTaskID": 1
}
```

---

## Get Comment By ID

```http
GET /Comment/{CommentID}
```

Example:

```http
GET /Comment/1
```

If the comment does not exist:

```http
404 Not Found
```

---

## Delete Comment

```http
DELETE /Comment/{CommentID}
```

Example:

```http
DELETE /Comment/1
```

---

# Status API

Base route:

```text
/Status
```

## Get All Statuses

```http
GET /Status
```

Optional pagination:

```http
GET /Status?pageNumber=1&pageSize=10
```

---

## Create Status

```http
POST /Status
Content-Type: application/json
```

Example:

```json
{
  "statusName": "InProgress",
  "statusDescription": "The task is currently being worked on."
}
```

`StatusName` is unique.

---

## Get Status By ID

```http
GET /Status/{StatusID}
```

Example:

```http
GET /Status/1
```

---

## Delete Status

```http
DELETE /Status/{StatusID}
```

Example:

```http
DELETE /Status/1
```

---

# Configuration API

## Get Priorities

```http
GET /Config/priority
```

Returns:

```text
Low
Medium
Normal
High
```

---

## Get User Types

```http
GET /Config/UsersType
```

Returns:

```text
Male
Female
Other
```

---

# Health API

## Ping

```http
GET /Health/api/ping
```

Example:

```bash
curl http://localhost:5037/Health/api/ping
```

---

## Hello

```http
GET /Health/api/hi
```

These endpoints can be used for:

* Application health checks
* Monitoring
* Deployment validation
* Connectivity testing

---

# Data Model

## User

```text
User
├── Id
├── Name
├── Email
├── BirthDate
└── UserType
```

## TodoTask

```text
TodoTask
├── Id
├── Name
├── Description
├── StartDate
├── EndDate
├── CreatedAt
├── UpdatedAt
├── Priority
├── UserId
└── StatusId
```

## Comment

```text
Comment
├── Id
├── Body
├── CreatedAt
├── UpdatedAt
├── UserId
└── TodoTaskId
```

## Status

```text
Status
├── Id
├── StatusName
└── StatusDescription
```

---

# Entity Relationships

The intended database relationships are:

```text
User
 │
 ├─────────────── 1 : N ──────────────► TodoTask
 │
 └─────────────── 1 : N ──────────────► Comment

TodoTask
 │
 ├─────────────── N : 1 ──────────────► User
 │
 ├─────────────── N : 1 ──────────────► Status
 │
 └─────────────── 1 : N ──────────────► Comment

Comment
 │
 ├─────────────── N : 1 ──────────────► User
 └─────────────── N : 1 ──────────────► TodoTask
```

Conceptually:

```text
                    ┌──────────────┐
                    │     User     │
                    └──────┬───────┘
                           │
                    1      │      N
                           │
             ┌─────────────┴─────────────┐
             ▼                           ▼
      ┌─────────────┐             ┌─────────────┐
      │  TodoTask   │             │   Comment   │
      └──────┬──────┘             └──────▲──────┘
             │                           │
          1  │  N                        │
             │                           │
             └───────────────────────────┘
             
      ┌─────────────┐
      │   Status    │
      └──────┬──────┘
             │
          1  │  N
             │
             ▼
        ┌──────────┐
        │ TodoTask │
        └──────────┘
```

---

# Pagination

The following endpoints support pagination:

```text
GET /Task
GET /Comment
GET /Status
```

Parameters:

| Parameter    | Description                |
| ------------ | -------------------------- |
| `pageNumber` | Page number                |
| `pageSize`   | Number of records per page |

Example:

```http
GET /Task?pageNumber=1&pageSize=20
```

---

# Dependency Injection

The application uses ASP.NET Core's built-in Dependency Injection.

Example service registration:

```csharp
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ITaskService, TaskService>();
builder.Services.AddScoped<ICommentService, CommentService>();
builder.Services.AddScoped<IStatusService, StatusService>();
```

Controllers receive services through constructor injection:

```csharp
public class TaskController : ControllerBase
{
    private readonly ITaskService _taskService;

    public TaskController(ITaskService taskService)
    {
        _taskService = taskService;
    }
}
```

---

# DTO Pattern

DTOs are used to separate the API contract from database entities.

```text
Database Entity
       │
       ▼
    Service
       │
       ▼
      DTO
       │
       ▼
 HTTP Response
```

Example:

```csharp
public class CommentTaskDto
{
    public int Id { get; set; }

    public string Body { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }
}
```

This prevents database entities from being directly exposed by the API.

---

# Database Access

Database access is handled through Entity Framework Core.

Example:

```csharp
var tasks = await _dbContext.Tasks
    .ToListAsync();
```

For DTO projection:

```csharp
var tasks = await _dbContext.Tasks
    .Select(t => new GetAllTasks
    {
        Id = t.Id,
        Name = t.Name,
        Description = t.Description
    })
    .ToListAsync();
```

Projection is preferred when only specific fields are required.

---

# Error Handling

The API should use standard HTTP status codes.

| Status                      | Meaning                        |
| --------------------------- | ------------------------------ |
| `200 OK`                    | Request completed successfully |
| `201 Created`               | Resource created               |
| `400 Bad Request`           | Invalid request                |
| `404 Not Found`             | Resource not found             |
| `409 Conflict`              | Resource conflict              |
| `500 Internal Server Error` | Unexpected server error        |

Example:

```http
GET /Comment/999
```

If the comment does not exist:

```http
404 Not Found
```

---

# Testing

The API can be tested using Scalar.

Start the API:

```bash
dotnet run
```

Open:

```text
http://localhost:5037/scalar/
```

You can also use Postman or curl.

Example:

```bash
curl -X GET http://localhost:5037/Task
```

---

# Typical Workflow

A typical workflow is:

```text
1. Create User
       │
       ▼
2. Create Status
       │
       ▼
3. Create Task
       │
       ▼
4. Assign Task to User
       │
       ▼
5. Add Comments
       │
       ▼
6. Update Task Status
       │
       ▼
7. Retrieve Task
```

Example:

```text
User
 │
 └── TodoTask
       │
       ├── Priority: High
       │
       ├── Status: InProgress
       │
       └── Comments
            ├── Comment 1
            └── Comment 2
```

---

# API Endpoint Summary

| Method | Endpoint               | Description       |
| ------ | ---------------------- | ----------------- |
| GET    | `/User`                | Get all users     |
| POST   | `/User`                | Create user       |
| GET    | `/User/{userId}`       | Get user          |
| DELETE | `/User/{userId}`       | Delete user       |
| GET    | `/Task`                | Get all tasks     |
| POST   | `/Task`                | Create task       |
| PUT    | `/Task`                | Update task       |
| GET    | `/Task/{TaskId}`       | Get task          |
| DELETE | `/Task/{TaskId}`       | Delete task       |
| GET    | `/Task/api/{UserID}`   | Get tasks by user |
| GET    | `/Comment`             | Get all comments  |
| POST   | `/Comment`             | Create comment    |
| GET    | `/Comment/{CommentID}` | Get comment       |
| DELETE | `/Comment/{CommentID}` | Delete comment    |
| GET    | `/Status`              | Get all statuses  |
| POST   | `/Status`              | Create status     |
| GET    | `/Status/{StatusID}`   | Get status        |
| DELETE | `/Status/{StatusID}`   | Delete status     |
| GET    | `/Config/priority`     | Get priorities    |
| GET    | `/Config/UsersType`    | Get user types    |
| GET    | `/Health/api/ping`     | API health check  |
| GET    | `/Health/api/hi`       | Health endpoint   |

---

# OpenAPI

The API contract uses **OpenAPI 3.1.1**.

```text
Title       : ToDos | v1
Version     : 1.0.0
OpenAPI     : 3.1.1
Base URL    : http://localhost:5037
Database    : SQLite
```

The OpenAPI document can be used with:

* Scalar
* Postman
* OpenAPI-compatible tools
* API client generators

---

# Future Improvements

Potential future improvements include:

* JWT authentication
* Authorization and role-based access
* Global exception handling middleware
* Standardized API response model
* FluentValidation
* Structured logging
* Unit tests
* Integration tests
* Docker support
* Database seeding
* Advanced filtering
* Sorting
* Improved pagination metadata
* API versioning
* Rate limiting
* Caching
* Production database support

---

# License

This project is currently intended for learning and development purposes.

Add an appropriate license if the project is distributed publicly.
