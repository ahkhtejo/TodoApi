# Running the API

The API can be run in two ways:

1. **Directly using the .NET CLI**
2. **Using Docker**

---

## Option 1 — Run with .NET CLI

### 1. Restore dependencies

From the project root:

```bash
dotnet restore
```

### 2. Build the project

```bash
dotnet build
```

For a Release build:

```bash
dotnet build -c Release
```

### 3. Apply database migrations

The project uses **SQLite** with Entity Framework Core.

Run:

```bash
dotnet ef database update
```

This creates/updates the SQLite database according to the configured migrations.

The database file will be:

```text
ToDos.db
```

### 4. Run the API

For development:

```bash
dotnet run
```

Or explicitly use the project:

```bash
dotnet run --project ToDos.csproj
```

The API will be available at:

```text
http://localhost:5037
```

### 5. Run using the Release configuration

```bash
dotnet run -c Release
```

---

## Option 2 — Run with Docker

The project includes a `Dockerfile`, allowing the application to be built and executed inside a Docker container.

### 1. Build the Docker image

From the project root:

```bash
docker build -t todos-api .
```

This creates a Docker image called:

```text
todos-api
```

### 2. Run the Docker container

```bash
docker run -d \
  --name todos-api \
  -p 5037:8080 \
  todos-api
```

The API will then be available at:

```text
http://localhost:5037
```

The port mapping is:

```text
Host              Container
5037       --->   8080
```

---

## Check Running Containers

To verify that the container is running:

```bash
docker ps
```

You should see something similar to:

```text
CONTAINER ID   IMAGE        PORTS
xxxxxxxxxxxx   todos-api    0.0.0.0:5037->8080/tcp
```

---

## View Container Logs

To view the API logs:

```bash
docker logs todos-api
```

To follow the logs in real time:

```bash
docker logs -f todos-api
```

---

## Stop the Container

```bash
docker stop todos-api
```

---

## Start the Container Again

If the container already exists:

```bash
docker start todos-api
```

---

## Remove the Container

```bash
docker rm todos-api
```

If the container is still running:

```bash
docker rm -f todos-api
```

---

## Remove the Docker Image

```bash
docker rmi todos-api
```

---

# Docker Development Workflow

A typical Docker development workflow is:

```text
Change Code
    │
    ▼
Build Docker Image
    │
    ▼
docker build -t todos-api .
    │
    ▼
Run Container
    │
    ▼
docker run -d --name todos-api -p 5037:8080 todos-api
    │
    ▼
Test API
    │
    ▼
http://localhost:5037/scalar/
```

---

# .NET Development Workflow

When developing without Docker:

```text
Change Code
    │
    ▼
dotnet restore
    │
    ▼
dotnet build
    │
    ▼
dotnet ef database update
    │
    ▼
dotnet run
    │
    ▼
Test API
    │
    ▼
http://localhost:5037/scalar/
```

---

# Quick Start

For developers who already have the environment configured:

### .NET

```bash
dotnet restore
dotnet build
dotnet ef database update
dotnet run
```

Then open:

```text
http://localhost:5037/scalar/
```

### Docker

```bash
docker build -t todos-api .
docker run -d --name todos-api -p 5037:8080 todos-api
```

Then open:

```text
http://localhost:5037/scalar/
```

---

# Dockerfile

The project contains a `Dockerfile` in the project root.

A typical ASP.NET Core multi-stage Docker build looks like:

```dockerfile
# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /src

COPY ["ToDos.csproj", "./"]

RUN dotnet restore "ToDos.csproj"

COPY . .

RUN dotnet build "ToDos.csproj" -c Release -o /app/build

RUN dotnet publish "ToDos.csproj" \
    -c Release \
    -o /app/publish \
    /p:UseAppHost=false


# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final

WORKDIR /app

COPY --from=build /app/publish .

EXPOSE 8080

ENTRYPOINT ["dotnet", "ToDos.dll"]
```

> **Note:** The exact .NET version, project name, DLL name, and exposed port should match the actual `Dockerfile` in the repository.

---

# SQLite and Docker

Because the application uses **SQLite**, the database is stored as a file rather than in a separate database server.

Example:

```text
ToDos.db
```

When running the API directly with .NET, this file can be stored in the project directory.

When running inside Docker, it is recommended to use a **Docker volume** if database persistence is required.

Example:

```bash
docker volume create todos-data
```

Then:

```bash
docker run -d \
  --name todos-api \
  -p 5037:8080 \
  -v todos-data:/app/data \
  todos-api
```

The SQLite connection string should point to the mounted location:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=/app/data/ToDos.db"
  }
}
```

This ensures that removing and recreating the container does not automatically remove the database.

---

# Verify the API

After starting the application, verify that the API is responding:

```bash
curl http://localhost:5037/Health/api/ping
```

You can also open the Scalar interface:

```text
http://localhost:5037/scalar/
```

---

# Common Docker Commands

| Command                                                 | Description              |
| ------------------------------------------------------- | ------------------------ |
| `docker build -t todos-api .`                           | Build the image          |
| `docker run -d --name todos-api -p 5037:8080 todos-api` | Run container            |
| `docker ps`                                             | List running containers  |
| `docker ps -a`                                          | List all containers      |
| `docker logs todos-api`                                 | View logs                |
| `docker logs -f todos-api`                              | Follow logs              |
| `docker stop todos-api`                                 | Stop container           |
| `docker start todos-api`                                | Start existing container |
| `docker restart todos-api`                              | Restart container        |
| `docker rm todos-api`                                   | Remove container         |
| `docker rm -f todos-api`                                | Force remove container   |
| `docker images`                                         | List images              |
| `docker rmi todos-api`                                  | Remove image             |

---

# Recommended Development Commands

### Normal development

```bash
dotnet restore
dotnet build
dotnet ef database update
dotnet run
```

### Docker development

```bash
docker build -t todos-api .
docker run -d --name todos-api -p 5037:8080 todos-api
```

### Check API

```bash
curl http://localhost:5037/Health/api/ping
```

### Open API documentation

```text
http://localhost:5037/scalar/
```
