# Streambit.Catalog.Api

## Overview

The **Streambit.Catalog.Api** project is the entry point and presentation layer of the Streambit Catalog system. It exposes RESTful endpoints for managing movies, genres, languages, and providers through versioned API routes. Built with ASP.NET Core 8+, it implements clean architecture principles with dependency injection, API versioning, and comprehensive Swagger documentation.

## Purpose

This project serves as the HTTP API layer that:
- Exposes versioned REST endpoints (`/api/v1`, `/api/v2`)
- Handles HTTP request/response processing
- Manages API contracts (DTOs) for data serialization
- Provides automatic API documentation via Swagger/OpenAPI
- Orchestrates service registration and middleware configuration

## Architecture

### Project Structure

```
Streambit.Catalog.Api/
├── Controllers/           # HTTP endpoint handlers
│   ├── V1/               # Version 1 controllers
│   └── V2/               # Version 2 controllers
├── Contracts/            # Request/Response DTOs
│   ├── Genres/           # Genre contracts
│   ├── Languages/        # Language contracts
│   ├── Movies/           # Movie contracts
│   └── Providers/        # Provider contracts
├── Extensions/           # Extension methods
│   └── RegistrarExtensions.cs
├── Registrars/           # Service registration modules
├── Options/              # Configuration classes
├── Properties/           # Launch settings
├── Program.cs            # Application entry point
├── GlobalUsings.cs       # Global using statements
├── appsettings.json      # Configuration
└── Streambit.Catalog.Api.csproj
```

## Key Components

### Controllers

Organize API endpoints by resource and version:

- **V1 Controllers**
  - `LanguagesController` - Language CRUD operations
  - `GenresController` - Genre CRUD operations
  - `MoviesController` - Movie CRUD operations
  - `ProvidersController` - Provider management

- **V2 Controllers**
  - `MoviesController` - Enhanced movie endpoints with new features

### Contracts

Data Transfer Objects for API communication:

```csharp
// Request Contracts
public class CreateLanguageRequest { }
public class CreateGenreRequest { }
public class CreateMovieRequest { }

// Response Contracts
public class LanguageResponse { }
public class GenreResponse { }
public class MovieResponse { }
```

### Registrars

Modular service registration implementing the Registrar pattern:

- `RegistrarExtensions` - Extension methods for service registration
- `IRegistrar` - Base registrar interface
- `IWebApplicationBuilderRegistrar` - Configure services
- `IWebApplicationRegistrar` - Configure pipeline
- `MvcRegistrar` - MVC/API registration
- `DbRegistrar` - Database service registration
- `SwaggerRegistrar` - Swagger/OpenAPI configuration

## Technology Stack

| Component | Version | Purpose |
|-----------|---------|---------|
| .NET SDK | 8.0+ | Framework |
| ASP.NET Core | 9.0.8 | Web API framework |
| MediatR | 13.0.0 | CQRS pattern implementation |
| AutoMapper | 15.0.1 | DTO mapping |
| Entity Framework Core | 9.0.8 | ORM |
| Asp.Versioning.Mvc | 8.1.0 | API versioning |
| Swashbuckle.AspNetCore | 9.0.1 | Swagger/OpenAPI |

## Configuration

### appsettings.json

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information"
    }
  },
  "AllowedHosts": "*",
  "ConnectionStrings": {
    "Default": "Server=localhost;Database=streambit_catalog;User Id=postgres;Password=your_password;"
  }
}
```

### appsettings.Development.json

Development-specific configuration for local testing with debug logging and database seeds.

## API Endpoints

### Languages API (v1)

```
GET    /api/v1/languages                    # Get all languages
GET    /api/v1/languages/{id}               # Get language by ID
POST   /api/v1/languages/CreateLanguage     # Create new language
```

### Genres API (v1)

```
GET    /api/v1/genres                       # Get all genres
GET    /api/v1/genres/{id}                  # Get genre by ID
POST   /api/v1/genres/CreateGenre           # Create new genre
```

### Movies API (v1)

```
GET    /api/v1/movies                       # Get all movies
GET    /api/v1/movies/{id}                  # Get movie by ID
POST   /api/v1/movies/CreateMovie           # Create new movie
```

### Movies API (v2)

```
GET    /api/v2/movies/{id}                  # Get movie by ID (enhanced)
POST   /api/v2/movies                       # Create movie (v2 implementation)
```

### Providers API (v1)

```
GET    /api/v1/providers                    # Get all providers
GET    /api/v1/providers/{id}               # Get provider by ID
```

## Dependency Injection

Services are registered in the `Registrar` pattern:

```csharp
builder.RegisterServices(typeof(Program));  // Registers all services
var app = builder.Build();
app.RegisterPipelineComponents(typeof(Program)); // Configures pipeline
```

## Documentation

- **Swagger UI** - Interactive API documentation available at `/swagger`
- **OpenAPI Specification** - JSON schema at `/swagger/v1/swagger.json`

## Dependencies

### Project References
- `Streambit.Catalog.Application` - Business logic layer
- `Streambit.Catalog.Dal` - Data access layer
- `Streambit.Catalog.Domain` - Domain models

## Running the Application

```bash
# Build
dotnet build

# Run
dotnet run

# Run with specific configuration
dotnet run --configuration Development

# Access API
# https://localhost:5001
# Swagger: https://localhost:5001/swagger
```

## Docker Support

A `docker-compose.yml` file is provided for containerized deployment:

```bash
docker-compose up -d
```

## HTTP Request Examples

See `Streambit.Catalog.http` for example HTTP requests that can be tested with VS Code REST Client extension.

## Contributing Guidelines

1. **Versioning:** Use appropriate API version in routes
2. **Contracts:** Define clear request/response DTOs
3. **Routing:** Follow RESTful conventions with resource-based endpoints
4. **Documentation:** Update Swagger configuration for new endpoints
5. **Error Handling:** Return appropriate HTTP status codes

## Error Handling

Standard HTTP status codes are returned:
- `200 OK` - Successful request
- `201 Created` - Resource created
- `400 Bad Request` - Invalid input
- `404 Not Found` - Resource not found
- `500 Internal Server Error` - Server error

## Next Steps

- Explore API documentation at `/swagger`
- Review `Streambit.Catalog.Application` for business logic
- Check `Streambit.Catalog.Domain` for domain models
