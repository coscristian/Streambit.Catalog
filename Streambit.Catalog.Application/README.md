# Streambit.Catalog.Application

## Overview

The **Streambit.Catalog.Application** project implements the business logic layer using the **CQRS (Command Query Responsibility Segregation)** pattern with **MediatR**. It serves as the application layer in Clean Architecture, handling all use cases, validations, and orchestration between the presentation layer and data access layer.

## Purpose

This project encapsulates:
- **Command Handlers** - Execute data mutations (Create, Update, Delete operations)
- **Query Handlers** - Retrieve and aggregate data
- **AutoMapper Profiles** - Transform between DTOs and domain models
- **Business Logic** - Validation and processing rules
- **CQRS Orchestration** - Centralized request handling through MediatR

## Architecture

### Project Structure

```
Streambit.Catalog.Application/
├── Genres/                          # Genre use cases
│   ├── Commands/                    # Genre commands
│   ├── CommandHandlers/             # Genre command handlers
│   ├── Queries/                     # Genre queries
│   └── QueryHandlers/               # Genre query handlers
├── Languages/                       # Language use cases
│   ├── Commands/                    # Language commands
│   ├── CommandHandlers/             # Language command handlers
│   ├── Queries/                     # Language queries
│   └── QueryHandlers/               # Language query handlers
├── Movies/                          # Movie use cases
│   ├── Commands/                    # Movie commands
│   ├── CommandHandlers/             # Movie command handlers
│   ├── Queries/                     # Movie queries
│   └── QueryHandlers/               # Movie query handlers
├── MappingProfiles/                 # AutoMapper configurations
├── Streambit.Catalog.Application.csproj
└── README.md
```

## Key Components

### CQRS Pattern

#### Commands
Commands represent actions that **modify data**:

```csharp
// Create Language Command
public class CreateLanguageCommand : IRequest<LanguageResponse>
{
    public string Name { get; set; }
    public string IsoCode { get; set; }
}

// Create Genre Command
public class CreateGenreCommand : IRequest<GenreResponse>
{
    public string Name { get; set; }
    public string Description { get; set; }
}

// Create Movie Command
public class CreateMovieCommand : IRequest<MovieResponse>
{
    public string Title { get; set; }
    public string Description { get; set; }
    public int LanguageId { get; set; }
    public List<int> GenreIds { get; set; }
}
```

#### Command Handlers
Process commands and return results:

```csharp
public class CreateLanguageCommandHandler : IRequestHandler<CreateLanguageCommand, LanguageResponse>
{
    private readonly DataContext _context;
    private readonly IMapper _mapper;

    public CreateLanguageCommandHandler(DataContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<LanguageResponse> Handle(CreateLanguageCommand request, CancellationToken cancellationToken)
    {
        // Business logic and validation
        var language = new Language { /* ... */ };
        await _context.Languages.AddAsync(language, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        
        return _mapper.Map<LanguageResponse>(language);
    }
}
```

#### Queries
Queries represent **read-only** operations:

```csharp
// Get All Languages Query
public class GetAllLanguagesQuery : IRequest<List<LanguageResponse>> { }

// Get Language By ID Query
public class GetLanguageByIdQuery : IRequest<LanguageResponse>
{
    public int Id { get; set; }
}

// Get All Movies Query
public class GetAllMoviesQuery : IRequest<List<MovieResponse>> { }

// Get Movie By ID Query
public class GetMovieByIdQuery : IRequest<MovieResponse>
{
    public int Id { get; set; }
}
```

#### Query Handlers
Execute queries and return data:

```csharp
public class GetAllLanguagesQueryHandler : IRequestHandler<GetAllLanguagesQuery, List<LanguageResponse>>
{
    private readonly DataContext _context;
    private readonly IMapper _mapper;

    public GetAllLanguagesQueryHandler(DataContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<LanguageResponse>> Handle(GetAllLanguagesQuery request, CancellationToken cancellationToken)
    {
        var languages = await _context.Languages.ToListAsync(cancellationToken);
        return _mapper.Map<List<LanguageResponse>>(languages);
    }
}
```

### AutoMapper Profiles

Map between domain entities and DTOs:

```csharp
public class LanguageMapProfile : Profile
{
    public LanguageMapProfile()
    {
        CreateMap<Language, LanguageResponse>();
        CreateMap<CreateLanguageCommand, Language>();
        CreateMap<UpdateLanguageCommand, Language>();
    }
}

public class MovieMapProfile : Profile
{
    public MovieMapProfile()
    {
        CreateMap<Movie, MovieResponse>()
            .ForMember(dest => dest.GenreIds, opt => opt.MapFrom(src => src.MovieGenres.Select(mg => mg.GenreId)));
        
        CreateMap<CreateMovieCommand, Movie>();
    }
}
```

## Use Cases

### Languages Module

| Use Case | Command/Query | Handler |
|----------|---------------|---------|
| Create Language | `CreateLanguageCommand` | `CreateLanguageCommandHandler` |
| Get All Languages | `GetAllLanguagesQuery` | `GetAllLanguagesQueryHandler` |
| Get Language by ID | `GetLanguageByIdQuery` | `GetLanguageByIdQueryHandler` |

### Genres Module

| Use Case | Command/Query | Handler |
|----------|---------------|---------|
| Create Genre | `CreateGenreCommand` | `CreateGenreCommandHandler` |
| Get All Genres | `GetAllGenresQuery` | `GetAllGenresQueryHandler` |
| Get Genre by ID | `GetGenreByIdQuery` | `GetGenreByIdQueryHandler` |

### Movies Module

| Use Case | Command/Query | Handler |
|----------|---------------|---------|
| Create Movie | `CreateMovieCommand` | `CreateMovieCommandHandler` |
| Get All Movies | `GetAllMoviesQuery` | `GetAllMoviesQueryHandler` |
| Get Movie by ID | `GetMovieByIdQuery` | `GetMovieByIdQueryHandler` |
| Update Movie | `UpdateMovieCommand` | `UpdateMovieCommandHandler` |

## Technology Stack

| Component | Version | Purpose |
|-----------|---------|---------|
| .NET SDK | 8.0+ | Framework |
| MediatR | 13.0.0 | CQRS mediator |
| AutoMapper | 15.0.1 | Object mapping |
| Entity Framework Core | 9.0.8 | ORM |

## Dependencies

### Project References
- `Streambit.Catalog.Domain` - Domain entities and aggregates
- `Streambit.Catalog.Dal` - Data access context

## Usage Flow

### From Controller to Handler

```
1. API Controller receives HTTP request
   ↓
2. Controller creates Command/Query object
   ↓
3. Controller calls mediator.Send(command)
   ↓
4. MediatR routes to appropriate Handler
   ↓
5. Handler executes business logic
   ↓
6. Handler returns response (DTO)
   ↓
7. Controller returns HTTP response
```

## Example: Creating a Movie

```csharp
// API Controller
[HttpPost("CreateMovie")]
public async Task<IActionResult> CreateMovie(CreateMovieRequest request)
{
    var command = _mapper.Map<CreateMovieCommand>(request);
    var result = await _mediator.Send(command);
    return CreatedAtAction(nameof(GetMovieById), new { id = result.Id }, result);
}

// Application Layer
public class CreateMovieCommandHandler : IRequestHandler<CreateMovieCommand, MovieResponse>
{
    public async Task<MovieResponse> Handle(CreateMovieCommand request, CancellationToken cancellationToken)
    {
        // Validate input
        if (string.IsNullOrEmpty(request.Title))
            throw new ArgumentException("Title is required");

        // Create domain entity
        var movie = new Movie
        {
            Title = request.Title,
            Description = request.Description,
            LanguageId = request.LanguageId,
            CreatedAt = DateTime.UtcNow
        };

        // Add genres
        foreach (var genreId in request.GenreIds)
        {
            movie.MovieGenres.Add(new MovieGenre { GenreId = genreId });
        }

        // Persist
        _context.Movies.Add(movie);
        await _context.SaveChangesAsync(cancellationToken);

        // Return mapped response
        return _mapper.Map<MovieResponse>(movie);
    }
}
```

## Service Registration

In the API `Program.cs`:

```csharp
// Register MediatR
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));

// Register AutoMapper
builder.Services.AddAutoMapper(typeof(Program).Assembly);
```

## Validation

Commands and queries should include validation:

```csharp
public class CreateMovieCommandValidator : AbstractValidator<CreateMovieCommand>
{
    public CreateMovieCommandValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required")
            .MaximumLength(255).WithMessage("Title must not exceed 255 characters");

        RuleFor(x => x.Description)
            .MaximumLength(1000).WithMessage("Description must not exceed 1000 characters");

        RuleFor(x => x.LanguageId)
            .GreaterThan(0).WithMessage("Valid language ID is required");
    }
}
```

## Benefits of CQRS Pattern

- **Separation of Concerns** - Commands and queries are clearly separated
- **Scalability** - Read and write operations can be optimized independently
- **Testability** - Handlers are easy to unit test with mocked dependencies
- **Maintainability** - Clear structure makes code easier to understand and modify
- **Single Responsibility** - Each handler has one reason to change

## Contributing Guidelines

1. **Commands:** Represent actions that change state
2. **Queries:** Represent read-only operations
3. **Handlers:** Keep business logic clean and focused
4. **Mapping:** Use AutoMapper for consistent DTO transformation
5. **Validation:** Validate input in handlers or use validators
6. **Async/Await:** Always use async patterns for I/O operations

## Next Steps

- Review `Streambit.Catalog.Domain` for domain entities
- Explore `Streambit.Catalog.Dal` for database configuration
- Check `Streambit.Catalog.Api` for controller implementations
