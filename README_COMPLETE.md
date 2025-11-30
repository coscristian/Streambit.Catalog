# Streambit.Catalog

Complete movie catalog management system built with Clean Architecture and Domain-Driven Design principles. A comprehensive .NET 8+ solution featuring versioned RESTful APIs, CQRS pattern implementation, and Entity Framework Core integration with PostgreSQL.

---

## 📋 Quick Links

- **[API Layer](./Streambit.Catalog/README.md)** - Presentation and HTTP endpoints
- **[Application Layer](./Streambit.Catalog.Application/README.md)** - Business logic with CQRS
- **[Data Access Layer](./Streambit.Catalog.Dal/README.md)** - Database and EF Core configuration
- **[Domain Layer](./Streambit.Catalog.Domain/README.md)** - Core domain models and aggregates

---

## 📋 Table of Contents

- [Project Overview](#project-overview)
- [Architecture](#architecture)
- [Project Structure](#project-structure)
- [Technology Stack](#technology-stack)
- [Getting Started](#getting-started)
- [API Endpoints](#api-endpoints)
- [Database Schema](#database-schema)
- [Contributing](#contributing)
- [License](#license)

---

## 🎯 Project Overview

**Streambit.Catalog** is a multi-layered application implementing industry-standard architectural patterns:

### Key Features

✅ **Clean Architecture** - Concentric layers with clear separation of concerns  
✅ **CQRS Pattern** - Command and Query separation through MediatR  
✅ **Domain-Driven Design** - Rich domain models and aggregates  
✅ **API Versioning** - Support for multiple API versions (v1, v2)  
✅ **Entity Framework Core** - Modern ORM with PostgreSQL provider  
✅ **Swagger Documentation** - Interactive API documentation  
✅ **ASP.NET Core Identity** - Authentication and authorization ready  
✅ **Async/Await** - Fully asynchronous operations  

### Business Domain

The system manages:
- **Movies** - Film information with ratings, genres, and providers
- **Genres** - Movie genre classifications
- **Languages** - Original and available languages (ISO 639-1 codes)
- **Providers** - Streaming providers (Netflix, Amazon Prime, etc.)
- **Images** - Movie posters, backdrops, and artwork

---

## 🏗 Architecture

### Layered Architecture Diagram

```
┌─────────────────────────────────────────────────────┐
│           API Layer (Presentation)                   │
│  Controllers │ Contracts │ Registrars │ DTOs        │
│  • HTTP Endpoints          • REST Routes             │
│  • API Versioning          • Swagger Docs            │
│  • Request Validation      • Error Handling          │
└────────────────────┬────────────────────────────────┘
                     │ Depends on
┌────────────────────▼────────────────────────────────┐
│      Application Layer (Business Logic)              │
│  Commands │ Queries │ Handlers │ DTOs               │
│  • Use Cases               • MediatR Pipeline        │
│  • Business Rules          • AutoMapper              │
│  • Validation              • CQRS Pattern            │
└────────────────────┬────────────────────────────────┘
                     │ Depends on
┌────────────────────▼────────────────────────────────┐
│         Domain Layer (Core Business)                 │
│  Entities │ Aggregates │ Value Objects              │
│  • Movie, Genre, Language, Provider                  │
│  • Business Rules Enforcement                        │
│  • Rich Domain Models                                │
└────────────────────┬────────────────────────────────┘
                     │ Depends on
┌────────────────────▼────────────────────────────────┐
│      Data Access Layer (Persistence)                 │
│  DbContext │ Configurations │ Migrations             │
│  • Entity Framework Core   • PostgreSQL Driver       │
│  • Database Mappings       • Relationship Config     │
└─────────────────────────────────────────────────────┘
```

### Request Processing Flow

```
1. HTTP Request arrives at API
   ↓
2. Controller receives request
   ↓
3. DTO deserialization and validation
   ↓
4. Map DTO to Command/Query object
   ↓
5. Send through MediatR mediator
   ↓
6. MediatR routes to appropriate Handler
   ↓
7. Handler executes business logic
   ↓
8. Access data through DbContext
   ↓
9. Return mapped response DTO
   ↓
10. Controller returns HTTP response
```

---

## 📁 Project Structure

```
Streambit.Catalog/                          (Root)
│
├── Streambit.Catalog/                      🔵 API LAYER
│   ├── Controllers/
│   │   ├── V1/
│   │   │   ├── LanguagesController.cs
│   │   │   ├── GenresController.cs
│   │   │   ├── MoviesController.cs
│   │   │   └── ProvidersController.cs
│   │   └── V2/
│   │       └── MoviesController.cs
│   ├── Contracts/
│   │   ├── Genres/
│   │   ├── Languages/
│   │   ├── Movies/
│   │   └── Providers/
│   ├── Registrars/
│   │   ├── DbRegistrar.cs
│   │   ├── MvcRegistrar.cs
│   │   ├── SwaggerRegistrar.cs
│   │   └── RegistrarExtensions.cs
│   ├── Extensions/
│   ├── Options/
│   ├── Program.cs
│   ├── GlobalUsings.cs
│   ├── Streambit.Catalog.Api.csproj
│   ├── appsettings.json
│   ├── appsettings.Development.json
│   ├── docker-compose.yml
│   ├── Streambit.Catalog.http
│   └── README.md
│
├── Streambit.Catalog.Application/          🟢 APPLICATION LAYER
│   ├── Languages/
│   │   ├── Commands/
│   │   │   └── CreateLanguageCommand.cs
│   │   ├── CommandHandlers/
│   │   │   └── CreateLanguageCommandHandler.cs
│   │   ├── Queries/
│   │   │   └── GetAllLanguagesQuery.cs
│   │   └── QueryHandlers/
│   │       └── GetAllLanguagesQueryHandler.cs
│   ├── Genres/
│   │   ├── Commands/
│   │   ├── CommandHandlers/
│   │   ├── Queries/
│   │   └── QueryHandlers/
│   ├── Movies/
│   │   ├── Commands/
│   │   ├── CommandHandlers/
│   │   ├── Queries/
│   │   └── QueryHandlers/
│   ├── MappingProfiles/
│   │   ├── LanguageMap.cs
│   │   ├── MovieMap.cs
│   │   └── GenreMap.cs
│   ├── Streambit.Catalog.Application.csproj
│   └── README.md
│
├── Streambit.Catalog.Dal/                  🟡 DATA ACCESS LAYER
│   ├── DataContext.cs
│   ├── Configurations/
│   │   ├── MovieConfig.cs
│   │   ├── MovieGenreConfig.cs
│   │   ├── MovieProviderConfig.cs
│   │   ├── MovieImageConfig.cs
│   │   ├── GenreConfig.cs
│   │   ├── LanguageConfig.cs
│   │   ├── ProviderConfig.cs
│   │   ├── IdentityUserLoginConfig.cs
│   │   ├── IdentityUserRoleConfig.cs
│   │   └── IdentityUserTokenConfig.cs
│   ├── Migrations/
│   │   ├── 20240101000000_InitialCreate.cs
│   │   ├── 20240102000000_AddMovieProvider.cs
│   │   └── ...
│   ├── Streambit.Catalog.Dal.csproj
│   └── README.md
│
├── Streambit.Catalog.Domain/               🔴 DOMAIN LAYER
│   ├── Aggregates/
│   │   ├── MovieAggregate/
│   │   │   ├── Movie.cs
│   │   │   ├── MovieGenre.cs
│   │   │   ├── MovieProvider.cs
│   │   │   ├── MovieImage.cs
│   │   │   └── MovieStatus.cs
│   │   ├── GenreAggregate/
│   │   │   └── Genre.cs
│   │   ├── LanguageAggregate/
│   │   │   └── Language.cs
│   │   ├── ProviderAggregate/
│   │   │   └── Provider.cs
│   │   ├── CountryAggregate/
│   │   │   └── Country.cs
│   │   └── CompanyAggregate/
│   │       └── ProductionCompany.cs
│   ├── Streambit.Catalog.Domain.csproj
│   └── README.md
│
├── Streambit.Catalog.sln                   (Solution file)
├── LICENSE.txt
├── README.md                               (This file)
└── .gitignore

```

---

## 📚 Individual Project Documentation

Each project has comprehensive documentation in its dedicated README file:

### 1. **[Streambit.Catalog](./Streambit.Catalog/README.md)** - API Layer 🔵

**Presentation Layer** - Exposes REST endpoints for external consumption.

**Key Responsibilities:**
- HTTP request/response handling
- API versioning (v1, v2)
- Contract-based Data Transfer Objects (DTOs)
- Service registration and dependency injection configuration
- Swagger/OpenAPI documentation generation
- Error handling and validation

**Main Components:**
- `Controllers/V1/` - Version 1 API endpoints
- `Controllers/V2/` - Version 2 API endpoints
- `Contracts/` - Request/Response DTOs
- `Registrars/` - Service registration modules

[→ Read Full API Documentation](./Streambit.Catalog/README.md)

### 2. **[Streambit.Catalog.Application](./Streambit.Catalog.Application/README.md)** - Application Layer 🟢

**Business Logic Layer** - Implements CQRS pattern with use cases.

**Key Responsibilities:**
- Command handling (Create, Update, Delete operations)
- Query handling (Read-only operations)
- Use case orchestration
- DTO/Domain model mapping with AutoMapper
- Business logic implementation
- Input validation and error handling

**Main Components:**
- `Languages/` - Language use cases
- `Genres/` - Genre use cases
- `Movies/` - Movie use cases
- `MappingProfiles/` - AutoMapper configurations

[→ Read Full Application Documentation](./Streambit.Catalog.Application/README.md)

### 3. **[Streambit.Catalog.Dal](./Streambit.Catalog.Dal/README.md)** - Data Access Layer 🟡

**Infrastructure Layer** - Manages all data persistence operations.

**Key Responsibilities:**
- Database context management (Entity Framework Core)
- Entity type configurations and relationships
- Database migrations and schema management
- PostgreSQL integration
- ASP.NET Core Identity setup

**Main Components:**
- `DataContext.cs` - Entity Framework DbContext
- `Configurations/` - Entity mappings and relationships
- `Migrations/` - Database schema versions

[→ Read Full Data Access Documentation](./Streambit.Catalog.Dal/README.md)

### 4. **[Streambit.Catalog.Domain](./Streambit.Catalog.Domain/README.md)** - Domain Layer 🔴

**Core Business Layer** - Rich domain models with business logic (no external dependencies).

**Key Responsibilities:**
- Domain entity definitions
- Aggregate root management
- Business rule enforcement
- Value object definitions
- Domain logic implementation

**Main Components:**
- `Aggregates/MovieAggregate/` - Movie aggregate with relationships
- `Aggregates/GenreAggregate/` - Genre aggregate
- `Aggregates/LanguageAggregate/` - Language aggregate
- `Aggregates/ProviderAggregate/` - Provider aggregate

[→ Read Full Domain Documentation](./Streambit.Catalog.Domain/README.md)

---

## 🛠 Technology Stack

### Core Platform
| Component | Version | Purpose |
|-----------|---------|---------|
| **.NET SDK** | 8.0+ | Framework and runtime |
| **C#** | 12+ | Programming language |

### Web Framework
| Component | Version | Purpose |
|-----------|---------|---------|
| **ASP.NET Core** | 9.0.8 | Web framework |
| **Asp.Versioning.Mvc** | 8.1.0 | API versioning |

### Business Logic
| Component | Version | Purpose |
|-----------|---------|---------|
| **MediatR** | 13.0.0 | CQRS pattern implementation |
| **AutoMapper** | 15.0.1 | Object-to-object mapping |

### Data Access
| Component | Version | Purpose |
|-----------|---------|---------|
| **Entity Framework Core** | 9.0.8 | ORM (Object-Relational Mapper) |
| **PostgreSQL Provider** | Latest | SQL database driver |

### Database
| Component | Version | Purpose |
|-----------|---------|---------|
| **PostgreSQL** | 12+ | Relational database |

### Documentation & API
| Component | Version | Purpose |
|-----------|---------|---------|
| **Swashbuckle.AspNetCore** | 9.0.1 | Swagger/OpenAPI documentation |

### Authentication
| Component | Version | Purpose |
|-----------|---------|---------|
| **ASP.NET Core Identity** | 9.0.6 | Authentication framework |

---

## 🚀 Getting Started

### Prerequisites

Before starting, ensure you have:

- **[.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)** or later
- **[PostgreSQL 12+](https://www.postgresql.org/download/)** or Docker
- **[Git](https://git-scm.com/)** for version control
- **IDE**: Visual Studio Code, Visual Studio 2022, or JetBrains Rider

### Step-by-Step Installation

#### 1️⃣ Clone the Repository

```bash
git clone https://github.com/coscristian/Streambit.Catalog.git
cd Streambit.Catalog
```

#### 2️⃣ Configure Database Connection

Edit `Streambit.Catalog/appsettings.json`:

```json
{
  "ConnectionStrings": {
    "Default": "Server=localhost;Port=5432;Database=streambit_catalog;User Id=postgres;Password=your_password_here;"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information"
    }
  }
}
```

**PostgreSQL Connection String Format:**
```
Server=<hostname>;Port=<port>;Database=<database_name>;User Id=<username>;Password=<password>;
```

#### 3️⃣ Restore NuGet Dependencies

```bash
dotnet restore
```

#### 4️⃣ Build the Solution

```bash
dotnet build
```

#### 5️⃣ Create Database and Apply Migrations

```bash
# Navigate to Data Access Layer project
cd Streambit.Catalog.Dal

# Create initial migration (if needed)
dotnet ef migrations add InitialCreate

# Apply migrations to create database schema
dotnet ef database update

# Return to root directory
cd ..
```

#### 6️⃣ Run the Application

```bash
# Run the API project
dotnet run --project Streambit.Catalog

# Or specify configuration
dotnet run --project Streambit.Catalog --configuration Development
```

#### 7️⃣ Access the Application

Once running, access:

- **API Base URL:** `https://localhost:5001`
- **Swagger UI:** `https://localhost:5001/swagger`
- **OpenAPI JSON:** `https://localhost:5001/swagger/v1/swagger.json`

### Docker Setup (Alternative)

If you prefer using Docker for PostgreSQL:

```bash
# Start PostgreSQL container
docker-compose up -d

# Connection string will be:
# Server=localhost;Port=5432;Database=streambit_catalog;User Id=postgres;Password=postgres;
```

---

## 📡 API Endpoints

### Languages Endpoints (v1)

```http
GET    /api/v1/languages                    Get all languages
GET    /api/v1/languages/{id}               Get language by ID
POST   /api/v1/languages/CreateLanguage     Create new language
```

**Example Request:**
```http
POST /api/v1/languages/CreateLanguage HTTP/1.1
Content-Type: application/json

{
  "name": "English",
  "isoCode": "en"
}
```

### Genres Endpoints (v1)

```http
GET    /api/v1/genres                       Get all genres
GET    /api/v1/genres/{id}                  Get genre by ID
POST   /api/v1/genres/CreateGenre           Create new genre
```

### Movies Endpoints (v1)

```http
GET    /api/v1/movies                       Get all movies
GET    /api/v1/movies/{id}                  Get movie by ID
POST   /api/v1/movies/CreateMovie           Create new movie
```

**Example Request:**
```http
POST /api/v1/movies/CreateMovie HTTP/1.1
Content-Type: application/json

{
  "title": "The Matrix",
  "description": "A sci-fi masterpiece",
  "languageId": 1,
  "rating": 8.7,
  "genreIds": [1, 5]
}
```

### Movies Endpoints (v2) - Enhanced

```http
GET    /api/v2/movies/{id}                  Get movie by ID (v2)
POST   /api/v2/movies                       Create movie (v2)
```

### Providers Endpoints (v1)

```http
GET    /api/v1/providers                    Get all providers
GET    /api/v1/providers/{id}               Get provider by ID
```

---

## 💾 Database Schema

### Entity Relationship Diagram

```
┌─────────────────────────────────┐
│           Movies                 │
│  ─────────────────────────────  │
│  ID (PK)                         │
│  Title (Required)                │
│  Description                     │
│  Rating                          │
│  Status (Enum)                   │
│  LanguageId (FK) ──┐             │
│  CreatedAt         │             │
│  UpdatedAt         │             │
└────────┬───────────┼─────────────┘
         │           │
    ┌────┴─┐    ┌────┴──────────────────┐
    │      │    │                       │
┌───┴──────┴──────────┐         ┌───────┴────────┐
│   MovieGenres       │         │   Languages    │
│  ────────────────  │         │  ────────────  │
│  MovieId (FK/PK)   │         │  ID (PK)       │
│  GenreId (FK/PK)   │         │  Name          │
│  ◄──────────┐      │         │  IsoCode       │
└────────────┤───────┘         └────────────────┘
             │
        ┌────┴──┐
        │       │
┌───────┴──┐  ┌─┴──────────┐
│  Genres  │  │ Providers  │
│─────────│  │────────────│
│ ID (PK) │  │ ID (PK)    │
│ Name    │  │ Name       │
│ Desc    │  │ LogoUrl    │
└─────────┘  └────────────┘

┌──────────────────────────────┐
│     MovieProviders            │
│  ────────────────────────── │
│  MovieId (FK/PK) ──┐        │
│  ProviderId (FK/PK)├──┐     │
│  ProviderName      │  │     │
└────────────────────┘  │     │
                        ▼     │
                   References Providers
                        ◄─────┘

┌──────────────────────────────┐
│      MovieImages              │
│  ────────────────────────── │
│  ID (PK)                     │
│  MovieId (FK) ──┐            │
│  ImageUrl       │            │
│  ImageType      │            │
│                 ▼            │
│          References Movies   │
└──────────────────────────────┘
```

### Key Aggregates & Entities

| Aggregate | Root Entity | Composition |
|-----------|------------|-------------|
| **Movie** | `Movie` | `MovieGenre`, `MovieProvider`, `MovieImage` |
| **Genre** | `Genre` | Standalone |
| **Language** | `Language` | Standalone |
| **Provider** | `Provider` | `MovieProvider` |
| **Country** | `Country` | Standalone |
| **Company** | `ProductionCompany` | Standalone |

---

## 🏛 Architecture Patterns

### Clean Architecture

**Concentric Circles of Dependencies:**

```
┌─────────────────────────────────┐
│   External Frameworks & Tools   │
├─────────────────────────────────┤
│   Interface Adapters            │ ← Controllers, Gateways
├─────────────────────────────────┤
│   Application Business Rules    │ ← Handlers, Services
├─────────────────────────────────┤
│   Enterprise Business Rules     │ ← Entities, Aggregates
└─────────────────────────────────┘
```

**Principles:**
- ✅ Dependencies point inward
- ✅ Domain layer has no external dependencies
- ✅ Can test domain logic without infrastructure
- ✅ Framework agnostic business logic

### CQRS (Command Query Responsibility Segregation)

**Separation of Read and Write Operations:**

```
User Request
    ↓
    ├─→ Command (Mutation) → CommandHandler → Modify Database
    │
    └─→ Query (Read) → QueryHandler → Read Database
```

**Benefits:**
- ✅ Clear intent of operations
- ✅ Independent optimization of reads and writes
- ✅ Better scalability
- ✅ Easier testing

### Domain-Driven Design

**Building Blocks:**

| Concept | Purpose | Example |
|---------|---------|---------|
| **Aggregate** | Cluster of entities | Movie (with Genres & Providers) |
| **Entity** | Domain object with identity | Movie, Genre, Language |
| **Value Object** | Immutable concept | Rating, MovieStatus |
| **Domain Rule** | Business constraint | "Cannot add same genre twice" |
| **Ubiquitous Language** | Shared terminology | "Movie", "Genre", "Provider" |

---

## 🧪 Testing

### Running Unit Tests

```bash
# Run all tests
dotnet test

# Run specific test project
dotnet test Streambit.Catalog.Tests

# Run with coverage
dotnet test /p:CollectCoverage=true
```

### Running Integration Tests

```bash
# Run integration tests with real database
dotnet test Streambit.Catalog.IntegrationTests
```

---

## 📝 Contributing

We welcome contributions! Follow these guidelines:

### Before Contributing

1. **Understand Architecture** - Review this README and individual project docs
2. **Follow Patterns** - Maintain Clean Architecture and CQRS patterns
3. **Code Style** - Follow C# naming conventions and best practices

### Adding New Features

#### Step 1: Create Domain Entity
In `Streambit.Catalog.Domain/Aggregates/`:
```csharp
public class NewEntity
{
    public int Id { get; set; }
    public string Name { get; set; }
    
    // Domain methods with business logic
}
```

#### Step 2: Create Data Configuration
In `Streambit.Catalog.Dal/Configurations/`:
```csharp
public class NewEntityConfig : IEntityTypeConfiguration<NewEntity>
{
    public void Configure(EntityTypeBuilder<NewEntity> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Name).IsRequired();
    }
}
```

#### Step 3: Create Application Layer
In `Streambit.Catalog.Application/`:

Create command/query:
```csharp
public class CreateNewEntityCommand : IRequest<NewEntityResponse>
{
    public string Name { get; set; }
}
```

Create handler:
```csharp
public class CreateNewEntityCommandHandler : IRequestHandler<CreateNewEntityCommand, NewEntityResponse>
{
    public async Task<NewEntityResponse> Handle(CreateNewEntityCommand request, CancellationToken cancellationToken)
    {
        // Implementation
    }
}
```

#### Step 4: Create API Layer
In `Streambit.Catalog/`:

Create contracts:
```csharp
public class CreateNewEntityRequest { }
public class NewEntityResponse { }
```

Create controller:
```csharp
[ApiController]
[Route("api/v1/[controller]")]
public class NewEntitiesController : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Create(CreateNewEntityRequest request)
    {
        // Implementation
    }
}
```

#### Step 5: Update Database Migration
```bash
dotnet ef migrations add AddNewEntity --project Streambit.Catalog.Dal
dotnet ef database update --project Streambit.Catalog.Dal
```

### Commit Guidelines

```
feat: add new feature description
fix: bug fix description
docs: documentation updates
refactor: code refactoring
test: add tests
chore: maintenance
```

---

## 🔍 Troubleshooting

### Common Issues and Solutions

#### 1. Database Connection Failed

**Error:** `No connection could be made because the target machine actively refused it`

**Solution:**
```bash
# Verify PostgreSQL is running
psql -U postgres

# Check connection string format
# Server=localhost;Port=5432;Database=streambit_catalog;User Id=postgres;Password=xxx;
```

#### 2. Migration Issues

```bash
# Remove last migration
dotnet ef migrations remove --project Streambit.Catalog.Dal

# Reset database (⚠️ destroys all data)
dotnet ef database drop --force --project Streambit.Catalog.Dal

# Reapply migrations
dotnet ef database update --project Streambit.Catalog.Dal
```

#### 3. Build Errors

```bash
# Clean solution
dotnet clean

# Restore packages
dotnet restore

# Rebuild
dotnet build
```

#### 4. Port Already in Use

If port 5001 is already in use:

```json
// In Properties/launchSettings.json
"https": {
  "commandName": "Project",
  "applicationUrl": "https://localhost:5003"
}
```

---

## 📚 Resources & References

### Architecture & Design Patterns

- [Clean Architecture - Uncle Bob](https://blog.cleancoder.com/uncle-bob/2012/08/13/the-clean-architecture.html)
- [CQRS Pattern - Martin Fowler](https://martinfowler.com/bliki/CQRS.html)
- [Domain-Driven Design](https://en.wikipedia.org/wiki/Domain-driven_design)
- [MediatR GitHub](https://github.com/jbogard/MediatR)

### Technology Documentation

- [.NET 8 Documentation](https://learn.microsoft.com/en-us/dotnet/)
- [ASP.NET Core](https://learn.microsoft.com/en-us/aspnet/core/)
- [Entity Framework Core](https://learn.microsoft.com/en-us/ef/core/)
- [PostgreSQL](https://www.postgresql.org/docs/)
- [AutoMapper](https://automapper.org/)

---

## 📄 License

This project is licensed under the MIT License - see the [LICENSE.txt](./LICENSE.txt) file for details.

```
MIT License

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software.

See LICENSE.txt for full license text.
```

---

## 👥 Project Information

| Property | Value |
|----------|-------|
| **Project Name** | Streambit.Catalog |
| **Description** | Movie Catalog Management System |
| **Repository** | [GitHub](https://github.com/coscristian/Streambit.Catalog) |
| **Current Branch** | `feature/get-movies` |
| **Owner** | coscristian |
| **Status** | Active Development |
| **Framework** | .NET 8+ |
| **Last Updated** | November 2025 |

---

## 📞 Support

### Getting Help

1. **Documentation** - Review individual README files in each project
2. **Troubleshooting** - Check [Troubleshooting](#troubleshooting) section
3. **Issues** - Open an issue on [GitHub](https://github.com/coscristian/Streambit.Catalog/issues)
4. **Discussions** - Join project discussions for Q&A

### Quick Links

- [API Documentation](./Streambit.Catalog/README.md)
- [Application Documentation](./Streambit.Catalog.Application/README.md)
- [Data Access Documentation](./Streambit.Catalog.Dal/README.md)
- [Domain Documentation](./Streambit.Catalog.Domain/README.md)

---

**Happy Coding! 🚀**
