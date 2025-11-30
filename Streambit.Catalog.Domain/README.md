# Streambit.Catalog.Domain

## Overview

The **Streambit.Catalog.Domain** project represents the core domain layer in a Clean Architecture implementation. It contains all business domain models, aggregates, and value objects with **no external dependencies** (except .NET). This is the innermost layer, encapsulating the essential business logic and rules of the Streambit Catalog system.

## Purpose

This project defines:
- **Domain Entities** - Rich domain models with business behavior
- **Aggregates** - Cohesive groups of entities treated as a single unit
- **Value Objects** - Immutable objects representing domain concepts
- **Domain Rules** - Business logic and constraints
- **No Infrastructure** - Independent of databases, APIs, or frameworks

## Architecture

### Project Structure

```
Streambit.Catalog.Domain/
├── Aggregates/                          # Domain aggregates
│   ├── MovieAggregate/                  # Movie aggregate root
│   │   ├── Movie.cs                     # Movie aggregate root entity
│   │   ├── MovieGenre.cs                # Genre relationship
│   │   ├── MovieProvider.cs             # Provider relationship
│   │   ├── MovieImage.cs                # Movie images
│   │   └── MovieStatus.cs               # Status enumeration
│   │
│   ├── GenreAggregate/                  # Genre aggregate
│   │   └── Genre.cs                     # Genre entity
│   │
│   ├── LanguageAggregate/               # Language aggregate
│   │   └── Language.cs                  # Language entity
│   │
│   ├── ProviderAggregate/               # Provider aggregate
│   │   └── Provider.cs                  # Provider entity
│   │
│   ├── CountryAggregate/                # Country aggregate
│   │   └── Country.cs                   # Country entity
│   │
│   └── CompanyAggregate/                # Production company aggregate
│       └── ProductionCompany.cs         # Company entity
│
├── Streambit.Catalog.Domain.csproj
└── README.md
```

## Key Components

### Aggregates

Aggregates are clusters of entities and value objects treated as a single unit for data changes:

### Movie Aggregate

The core aggregate representing a movie and its relationships:

#### Movie Entity (Aggregate Root)

```csharp
public class Movie
{
    // Identifier
    public int Id { get; set; }

    // Core Properties
    public string Title { get; set; }
    public string Description { get; set; }
    public int LanguageId { get; set; }
    public decimal Rating { get; set; }

    // Status
    public MovieStatus Status { get; set; }

    // Relationships
    public Language Language { get; set; }
    public ICollection<MovieGenre> MovieGenres { get; set; } = new List<MovieGenre>();
    public ICollection<MovieProvider> MovieProviders { get; set; } = new List<MovieProvider>();
    public ICollection<MovieImage> Images { get; set; } = new List<MovieImage>();

    // Audit
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    // Domain Methods
    public void UpdateTitle(string newTitle)
    {
        if (string.IsNullOrWhiteSpace(newTitle))
            throw new ArgumentException("Title cannot be empty");
        
        Title = newTitle;
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateDescription(string newDescription)
    {
        Description = newDescription;
        UpdatedAt = DateTime.UtcNow;
    }

    public void AddGenre(int genreId)
    {
        if (MovieGenres.Any(mg => mg.GenreId == genreId))
            throw new InvalidOperationException("Genre already added to this movie");
        
        MovieGenres.Add(new MovieGenre { MovieId = Id, GenreId = genreId });
    }

    public void RemoveGenre(int genreId)
    {
        var movieGenre = MovieGenres.FirstOrDefault(mg => mg.GenreId == genreId);
        if (movieGenre != null)
            MovieGenres.Remove(movieGenre);
    }

    public void ChangeStatus(MovieStatus newStatus)
    {
        if (Status == newStatus)
            throw new InvalidOperationException("Movie is already in this status");
        
        Status = newStatus;
        UpdatedAt = DateTime.UtcNow;
    }
}
```

#### MovieGenre (Relationship Entity)

```csharp
public class MovieGenre
{
    // Composite Key
    public int MovieId { get; set; }
    public int GenreId { get; set; }

    // Navigation Properties
    public Movie Movie { get; set; }
    public Genre Genre { get; set; }
}
```

#### MovieProvider (Relationship Entity)

```csharp
public class MovieProvider
{
    // Composite Key
    public int MovieId { get; set; }
    public int ProviderId { get; set; }

    // Additional Data
    public string ProviderName { get; set; }

    // Navigation Properties
    public Movie Movie { get; set; }
    public Provider Provider { get; set; }
}
```

#### MovieImage

```csharp
public class MovieImage
{
    public int Id { get; set; }
    public int MovieId { get; set; }
    public string ImageUrl { get; set; }
    public string ImageType { get; set; } // Poster, Backdrop, etc.
    
    public Movie Movie { get; set; }
}
```

#### MovieStatus (Enumeration)

```csharp
public enum MovieStatus
{
    Active = 1,
    Inactive = 2,
    Archived = 3,
    Pending = 4,
    Draft = 5
}
```

### Genre Aggregate

```csharp
public class Genre
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }

    public ICollection<MovieGenre> MovieGenres { get; set; } = new List<MovieGenre>();

    public void UpdateName(string newName)
    {
        if (string.IsNullOrWhiteSpace(newName))
            throw new ArgumentException("Name cannot be empty");
        
        Name = newName;
    }
}
```

### Language Aggregate

```csharp
public class Language
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string IsoCode { get; set; } // ISO 639-1 (e.g., "en", "es")

    public void UpdateLanguage(string name, string isoCode)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name cannot be empty");
        
        if (string.IsNullOrWhiteSpace(isoCode) || isoCode.Length != 2)
            throw new ArgumentException("ISO Code must be 2 characters");
        
        Name = name;
        IsoCode = isoCode;
    }
}
```

### Provider Aggregate

```csharp
public class Provider
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string LogoUrl { get; set; }
    public string WebsiteUrl { get; set; }

    public ICollection<MovieProvider> MovieProviders { get; set; } = new List<MovieProvider>();
}
```

### Country Aggregate

```csharp
public class Country
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string IsoCode { get; set; } // ISO 3166-1
}
```

### Production Company Aggregate

```csharp
public class ProductionCompany
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string LogoPath { get; set; }
    public string OriginCountry { get; set; }
}
```

## Domain Concepts

### Aggregate Pattern

An aggregate is a cluster of domain objects (entities and value objects) that are treated as a single unit. In this system:

- **Movie Aggregate Root** - Controls access to MovieGenre, MovieProvider, and MovieImage
- **Genre Aggregate** - Stands alone with optional relationships to Movies
- **Language Aggregate** - Independent language definitions
- **Provider Aggregate** - Streaming providers

### Domain Rules

Business rules enforced at the domain level:

```csharp
// Rule: Movie title cannot be empty
movie.UpdateTitle(""); // Throws ArgumentException

// Rule: Cannot add same genre twice
movie.AddGenre(1);
movie.AddGenre(1); // Throws InvalidOperationException

// Rule: Cannot change to same status
movie.ChangeStatus(MovieStatus.Active);
movie.ChangeStatus(MovieStatus.Active); // Throws InvalidOperationException

// Rule: ISO language code must be 2 characters
language.UpdateLanguage("English", "eng"); // Throws ArgumentException
```

## Design Principles

### Clean Architecture

- **No Framework Dependencies** - Domain layer is independent of external frameworks
- **Testability** - Pure business logic is easy to unit test
- **Maintainability** - Clear separation of concerns

### Domain-Driven Design

- **Ubiquitous Language** - Class names reflect business concepts (Movie, Genre, Provider)
- **Aggregates** - Group related entities with a root
- **Domain Rules** - Business logic resides in entities, not services
- **Rich Domain Models** - Entities contain both data and behavior

## Technology Stack

| Component | Version |
|-----------|---------|
| .NET SDK | 8.0+ |
| C# Language | 12+ |

## No External Dependencies

This project intentionally has:
- ✅ No Entity Framework references
- ✅ No database providers
- ✅ No HTTP clients
- ✅ No logging frameworks
- ✅ No external APIs

It only depends on .NET runtime libraries.

## Usage

### Creating Domain Entities

```csharp
// Create a movie
var movie = new Movie
{
    Title = "Inception",
    Description = "A mind-bending thriller",
    LanguageId = 1,
    Status = MovieStatus.Active,
    CreatedAt = DateTime.UtcNow
};

// Add genres through domain methods
movie.AddGenre(1); // Sci-Fi
movie.AddGenre(5); // Thriller

// Update properties through domain methods
movie.UpdateTitle("Inception (Extended)");
movie.UpdateDescription("The ultimate sci-fi experience");

// Verify business rules are applied
try
{
    movie.AddGenre(1); // Will throw - already added
}
catch (InvalidOperationException ex)
{
    Console.WriteLine(ex.Message);
}
```

## Best Practices

1. **Rich Models** - Implement business logic in domain entities
2. **Immutability** - Use value objects for immutable concepts
3. **Validation** - Validate business rules in domain methods, not repositories
4. **No Getters/Setters** - Hide implementation details
5. **Factory Methods** - Use for complex entity creation
6. **Aggregate Boundaries** - Keep aggregates focused and small
7. **Ubiquitous Language** - Use business terminology

## Example: Factory Method

```csharp
public partial class Movie
{
    public static Movie CreateNew(string title, string description, int languageId)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("Title cannot be empty");

        return new Movie
        {
            Title = title,
            Description = description,
            LanguageId = languageId,
            Status = MovieStatus.Draft,
            CreatedAt = DateTime.UtcNow
        };
    }
}

// Usage
var movie = Movie.CreateNew("The Matrix", "A sci-fi masterpiece", 1);
```

## Contributing Guidelines

1. **Maintain Domain Integrity** - Keep business rules in domain entities
2. **No Infrastructure Code** - Never reference databases or APIs
3. **Clear Responsibility** - Each aggregate has one reason to change
4. **Domain Language** - Use business terminology in method names
5. **Validation** - Enforce business rules through domain methods
6. **Unit Tests** - Domain logic should be thoroughly tested

## Next Steps

- Review `Streambit.Catalog.Dal` for database mappings
- Explore `Streambit.Catalog.Application` for use cases
- Check `Streambit.Catalog.Api` for API controllers
