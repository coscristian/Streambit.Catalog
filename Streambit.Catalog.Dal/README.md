# Streambit.Catalog.Dal

## Overview

The **Streambit.Catalog.Dal** (Data Access Layer) project manages all database operations using **Entity Framework Core**. It provides the data context, entity configurations, and database migrations for the Streambit Catalog system. This layer abstracts the persistence mechanism and ensures clean separation between business logic and database concerns.

## Purpose

This project handles:
- **Database Context** - `DataContext` for managing entities and database operations
- **Entity Configurations** - Fluent API mappings for entity relationships and constraints
- **Migrations** - Database schema versioning and management
- **PostgreSQL Integration** - SQL database connectivity and queries
- **Identity Framework** - ASP.NET Core Identity entity mappings

## Architecture

### Project Structure

```
Streambit.Catalog.Dal/
├── Configurations/                      # Entity Framework configurations
│   ├── MovieConfig.cs                   # Movie entity configuration
│   ├── MovieGenreConfig.cs              # Movie-Genre junction configuration
│   ├── MovieProviderConfig.cs           # Movie-Provider configuration
│   ├── MovieImageConfig.cs              # Movie images configuration
│   ├── GenreConfig.cs                   # Genre entity configuration
│   ├── LanguageConfig.cs                # Language entity configuration
│   ├── ProviderConfig.cs                # Provider entity configuration
│   ├── IdentityUserLoginConfig.cs       # Identity user login configuration
│   ├── IdentityUserRoleConfig.cs        # Identity user role configuration
│   ├── IdentityUserTokenConfig.cs       # Identity user token configuration
│   └── ...more configurations
├── Migrations/                          # Database schema migrations
│   ├── 20240101000000_InitialCreate.cs
│   ├── 20240102000000_AddMovieProvider.cs
│   └── ...
├── DataContext.cs                       # Main database context
├── Streambit.Catalog.Dal.csproj         # Project file
└── README.md
```

## Key Components

### DataContext

The main Entity Framework Core database context:

```csharp
public class DataContext : IdentityDbContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {
    }

    // Core entities
    public DbSet<Movie> Movies { get; set; }
    public DbSet<MovieGenre> MovieGenres { get; set; }
    public DbSet<MovieProvider> MovieProviders { get; set; }
    public DbSet<MovieImage> MovieImages { get; set; }
    public DbSet<Genre> Genres { get; set; }
    public DbSet<Language> Languages { get; set; }
    public DbSet<Provider> Providers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        // Apply all entity configurations
        modelBuilder.ApplyConfiguration(new MovieConfig());
        modelBuilder.ApplyConfiguration(new MovieGenreConfig());
        modelBuilder.ApplyConfiguration(new MovieProviderConfig());
        modelBuilder.ApplyConfiguration(new MovieImageConfig());
        modelBuilder.ApplyConfiguration(new GenreConfig());
        modelBuilder.ApplyConfiguration(new ProviderConfig());
        
        // Apply Identity framework configurations
        modelBuilder.ApplyConfiguration(new IdentityUserLoginConfig());
        modelBuilder.ApplyConfiguration(new IdentityUserRoleConfig());
        modelBuilder.ApplyConfiguration(new IdentityUserTokenConfig());
    }
}
```

### Entity Configurations

Fluent API configurations for database mappings:

#### MovieConfig
```csharp
public class MovieConfig : IEntityTypeConfiguration<Movie>
{
    public void Configure(EntityTypeBuilder<Movie> builder)
    {
        builder.HasKey(m => m.Id);
        
        builder.Property(m => m.Title)
            .IsRequired()
            .HasMaxLength(255);
        
        builder.Property(m => m.Description)
            .HasMaxLength(1000);
        
        builder.HasOne<Language>()
            .WithMany()
            .HasForeignKey(m => m.LanguageId);
        
        builder.HasMany(m => m.MovieGenres)
            .WithOne()
            .HasForeignKey(mg => mg.MovieId);
    }
}
```

#### MovieGenreConfig
```csharp
public class MovieGenreConfig : IEntityTypeConfiguration<MovieGenre>
{
    public void Configure(EntityTypeBuilder<MovieGenre> builder)
    {
        builder.HasKey(mg => new { mg.MovieId, mg.GenreId });
        
        builder.HasOne<Movie>()
            .WithMany(m => m.MovieGenres)
            .HasForeignKey(mg => mg.MovieId);
        
        builder.HasOne<Genre>()
            .WithMany()
            .HasForeignKey(mg => mg.GenreId);
    }
}
```

#### GenreConfig
```csharp
public class GenreConfig : IEntityTypeConfiguration<Genre>
{
    public void Configure(EntityTypeBuilder<Genre> builder)
    {
        builder.HasKey(g => g.Id);
        
        builder.Property(g => g.Name)
            .IsRequired()
            .HasMaxLength(100);
    }
}
```

#### LanguageConfig
```csharp
public class LanguageConfig : IEntityTypeConfiguration<Language>
{
    public void Configure(EntityTypeBuilder<Language> builder)
    {
        builder.HasKey(l => l.Id);
        
        builder.Property(l => l.Name)
            .IsRequired()
            .HasMaxLength(100);
        
        builder.Property(l => l.IsoCode)
            .IsRequired()
            .HasMaxLength(5);
    }
}
```

#### Identity Configurations
```csharp
public class IdentityUserLoginConfig : IEntityTypeConfiguration<IdentityUserLogin<string>>
{
    public void Configure(EntityTypeBuilder<IdentityUserLogin<string>> builder)
    {
        builder.HasKey(x => new { x.LoginProvider, x.ProviderKey });
    }
}
```

## Database Schema

### Entity Relationship Diagram

```
┌─────────────────────────────────────────────────┐
│                    Movies                        │
│  ─────────────────────────────────────────────  │
│  Id (PK)                                         │
│  Title (Required, Max 255)                       │
│  Description (Max 1000)                          │
│  LanguageId (FK) ──────────┐                    │
│  CreatedAt                  │                    │
│  UpdatedAt                  │                    │
└──────────┬──────────────────┼──────────────────┘
           │                  │
         ┌─┴──────────────────┴──┐
         │                       │
    ┌────┴────────────┐    ┌────┴──────────┐
    │   MovieGenres   │    │   Languages    │
    │ ─────────────── │    │ ───────────── │
    │ MovieId (FK/PK) │    │ Id (PK)        │
    │ GenreId (FK/PK) │    │ Name (Required)│
    │ ◄────────────┐  │    │ IsoCode        │
    └──────┬───────┤  │    └────────────────┘
           │       │  │
      ┌────┴───┐   │  │
      │ Genres │   │  │
      │─────────  │  │
      │ Id (PK)   │  │
      │ Name (Req)│  │
      └────────────  │
            ◄────────┘

┌──────────────────────────────────────────┐
│        MovieProviders                    │
│  ─────────────────────────────────────  │
│  MovieId (FK/PK) ─────────┐             │
│  ProviderId (FK/PK) ──┐   │             │
│  ProviderName         │   │             │
│  ▼                    │   │             │
│  ┌──────────────┐     │   │             │
│  │  Providers   │     │   │             │
│  │─────────────│     │   │             │
│  │ Id (PK)     │◄────┴─┘ │             │
│  │ Name        │         │             │
│  └──────────────         │             │
│                          │             │
│         ◄─────────────────┘             │
└──────────────────────────────────────────┘

┌──────────────────────────────────────────┐
│        MovieImages                       │
│  ─────────────────────────────────────  │
│  Id (PK)                                 │
│  MovieId (FK) ─────┐                    │
│  ImageUrl          │                    │
│  ImageType         │                    │
│                    │                    │
│         ◄──────────┘ (references Movies)│
└──────────────────────────────────────────┘
```

## Technology Stack

| Component | Version | Purpose |
|-----------|---------|---------|
| .NET SDK | 8.0+ | Framework |
| Entity Framework Core | 9.0.8 | ORM |
| Microsoft.EntityFrameworkCore | 9.0.8 | Core ORM |
| PostgreSQL | 12+ | Database provider |

## Configuration

### Database Connection

In `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "Default": "Server=localhost;Port=5432;Database=streambit_catalog;User Id=postgres;Password=your_password;"
  }
}
```

### Service Registration

In API `Program.cs`:

```csharp
services.AddDbContext<DataContext>(options =>
    options.UseNpgsql(
        configuration.GetConnectionString("Default"),
        b => b.MigrationsAssembly("Streambit.Catalog.Dal")
    )
);
```

## Migrations

### Create Initial Migration

```bash
dotnet ef migrations add InitialCreate --project Streambit.Catalog.Dal
```

### Apply Migrations

```bash
dotnet ef database update --project Streambit.Catalog.Dal
```

### Rollback Last Migration

```bash
dotnet ef migrations remove --project Streambit.Catalog.Dal
```

### View Migration History

```bash
dotnet ef migrations list --project Streambit.Catalog.Dal
```

## Common Operations

### Add Movie with Genres

```csharp
var movie = new Movie
{
    Title = "The Matrix",
    Description = "A sci-fi masterpiece",
    LanguageId = 1
};

movie.MovieGenres.Add(new MovieGenre { GenreId = 1 }); // Sci-Fi
movie.MovieGenres.Add(new MovieGenre { GenreId = 5 }); // Action

await context.Movies.AddAsync(movie);
await context.SaveChangesAsync();
```

### Query Movies with Genres

```csharp
var movies = await context.Movies
    .Include(m => m.MovieGenres)
    .ThenInclude(mg => mg.Genre)
    .Where(m => m.Status == MovieStatus.Active)
    .ToListAsync();
```

### Update Movie

```csharp
var movie = await context.Movies.FindAsync(id);
if (movie != null)
{
    movie.Title = "New Title";
    movie.UpdatedAt = DateTime.UtcNow;
    await context.SaveChangesAsync();
}
```

### Delete Movie with Related Data

```csharp
var movie = await context.Movies
    .Include(m => m.MovieGenres)
    .FirstOrDefaultAsync(m => m.Id == id);

if (movie != null)
{
    context.Movies.Remove(movie); // Cascade delete MovieGenres
    await context.SaveChangesAsync();
}
```

## Dependencies

### Project References
- `Streambit.Catalog.Domain` - Domain entities and aggregates

## Best Practices

1. **Lazy Loading Disabled** - Use explicit `.Include()` for related data
2. **Async Operations** - Always use async/await for I/O operations
3. **Entity Validation** - Configure constraints in entity configurations
4. **Foreign Keys** - Define relationships using Fluent API
5. **Migrations** - Create meaningful migration names describing changes
6. **Change Tracking** - Be aware of DbContext lifetime and tracking
7. **Connection String** - Never hardcode connection strings in code

## Troubleshooting

### Migration Conflicts

```bash
# Remove pending migrations
dotnet ef migrations remove --project Streambit.Catalog.Dal

# Create fresh migration
dotnet ef migrations add <MigrationName> --project Streambit.Catalog.Dal
```

### Database Sync Issues

```bash
# Drop and recreate database
dotnet ef database drop --project Streambit.Catalog.Dal
dotnet ef database update --project Streambit.Catalog.Dal
```

### Connection Issues

- Verify PostgreSQL is running
- Check connection string in `appsettings.json`
- Verify database credentials
- Ensure database exists

## Next Steps

- Review `Streambit.Catalog.Domain` for entity definitions
- Explore `Streambit.Catalog.Application` for business logic
- Check `Streambit.Catalog.Api` for controller implementations
